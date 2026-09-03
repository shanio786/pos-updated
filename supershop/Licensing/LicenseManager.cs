using System;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace supershop.Licensing
{
    /// <summary>
    /// Offline, machine-locked licensing.
    ///
    /// * Each PC has a Machine ID derived from its hardware.
    /// * The vendor signs "MachineID|expiry|edition" with a PRIVATE RSA key
    ///   (see Tools/LicenseKeyGen) and gives the customer a License Key.
    /// * This app verifies the key with the embedded PUBLIC key, so keys cannot
    ///   be forged and a key only works on the machine it was issued for.
    /// * No internet or server needed.
    ///
    /// Key text format:  expiry|edition|base64(signature)
    ///   expiry  = yyyyMMdd  (or 0 = never expires)
    ///   edition = e.g. STD / PRO
    /// </summary>
    public static class LicenseManager
    {
        // PUBLIC key only. The matching PRIVATE key stays with the vendor.
        // Replace this with your own key pair before shipping (see Tools/LicenseKeyGen).
        const string PublicKeyXml = @"<RSAKeyValue><Modulus>qpwz5UwP8pN3BrEhn5aGhtnq2XMAL368xR+TbuFEH0uJdZjXUaCmAaZwlh8gID4stUYHeC+IU/jLrSqBBqiKgrW9o/p3pDiNCMHjIjmF05gG1K0b0Ek5CFPDtWdRZfCmBEvPu/1EWYAglbTGDFFhJOf3yCz/W2KWBpjsJkhCVAWeE3rJ3Ijw4vgQ2ANa1P5zGb/XIJZ0rjxbdCROokN1sA1k0vU+v0fUBdnI/2KnDNOfuan+Y0+s1VVl0a/WFogRxHwg+l0tsKVqHudnORca+aH0FUxOTB9CYbMwz4IzO99580WquFIDa04hFDPx9KpyQkiAWr2PkYBHRW3F3vLXxw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        static string LicenseFile
        {
            get
            {
                string dir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Adv_POS");
                return Path.Combine(dir, "license.dat");
            }
        }

        /// <summary>Stable per-machine id, formatted as GROUPS-OF-5.</summary>
        public static string GetMachineId()
        {
            string raw = Hw("Win32_Processor", "ProcessorId") + "|" +
                         Hw("Win32_BaseBoard", "SerialNumber") + "|" +
                         Hw("Win32_BIOS", "SerialNumber");
            if (raw.Replace("|", "").Trim().Length == 0)
                raw = Environment.MachineName + "|" + Environment.UserName;   // fallback

            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
                string b32 = Base32(hash, 15);   // 15 chars
                return b32.Substring(0, 5) + "-" + b32.Substring(5, 5) + "-" + b32.Substring(10, 5);
            }
        }

        static string Hw(string cls, string prop)
        {
            try
            {
                using (ManagementObjectSearcher s = new ManagementObjectSearcher("SELECT " + prop + " FROM " + cls))
                    foreach (ManagementObject o in s.Get())
                    {
                        object v = o[prop];
                        if (v != null && v.ToString().Trim().Length > 0) return v.ToString().Trim();
                    }
            }
            catch (Exception ex) { Logger.Error("HW " + cls, ex); }
            return "";
        }

        static readonly char[] B32 = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();
        static string Base32(byte[] data, int chars)
        {
            StringBuilder sb = new StringBuilder();
            int bits = 0, val = 0;
            foreach (byte b in data)
            {
                val = (val << 8) | b; bits += 8;
                while (bits >= 5) { sb.Append(B32[(val >> (bits - 5)) & 31]); bits -= 5; if (sb.Length >= chars) return sb.ToString(); }
            }
            while (sb.Length < chars) sb.Append('2');
            return sb.ToString();
        }

        /// <summary>Verifies a license key against THIS machine. Returns true and the expiry when valid.</summary>
        public static bool VerifyKey(string keyText, out DateTime expiry, out string reason)
        {
            expiry = DateTime.MaxValue; reason = "";
            try
            {
                if (string.IsNullOrEmpty(keyText)) { reason = "No key."; return false; }
                string[] parts = keyText.Trim().Replace("\r", "").Replace("\n", "").Split('|');
                if (parts.Length != 3) { reason = "Key format is not valid."; return false; }

                string expStr = parts[0], edition = parts[1], sigB64 = parts[2];
                string message = GetMachineId() + "|" + expStr + "|" + edition;

                byte[] sig;
                try { sig = Convert.FromBase64String(sigB64); }
                catch { reason = "Key is corrupted."; return false; }

                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(PublicKeyXml);
                    bool ok = rsa.VerifyData(Encoding.UTF8.GetBytes(message), new SHA256CryptoServiceProvider(), sig);
                    if (!ok) { reason = "This key is not valid for this computer."; return false; }
                }

                if (expStr != "0")
                {
                    DateTime exp;
                    if (!DateTime.TryParseExact(expStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out exp))
                    { reason = "Key expiry is invalid."; return false; }
                    expiry = exp.Date.AddDays(1).AddSeconds(-1);
                    if (DateTime.Now > expiry) { reason = "This license expired on " + exp.ToString("yyyy-MM-dd") + "."; return false; }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("VerifyKey", ex); reason = "Could not verify the key."; return false;
            }
        }

        /// <summary>True when a stored, valid license exists for this machine.</summary>
        public static bool IsActivated()
        {
            try
            {
                if (!File.Exists(LicenseFile)) return false;
                DateTime exp; string reason;
                return VerifyKey(File.ReadAllText(LicenseFile), out exp, out reason);
            }
            catch (Exception ex) { Logger.Error("IsActivated", ex); return false; }
        }

        /// <summary>Verifies and stores a key. Returns false with a reason when the key is rejected.</summary>
        public static bool Activate(string keyText, out string reason)
        {
            DateTime exp;
            if (!VerifyKey(keyText, out exp, out reason)) return false;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(LicenseFile));
                File.WriteAllText(LicenseFile, keyText.Trim());
                Logger.Info("License activated (expires " + (exp == DateTime.MaxValue ? "never" : exp.ToString("yyyy-MM-dd")) + ").");
                return true;
            }
            catch (Exception ex) { Logger.Error("Activate store", ex); reason = "Could not save the license: " + ex.Message; return false; }
        }

        /// <summary>Expiry of the stored license (MaxValue = perpetual, MinValue = none/invalid).</summary>
        public static DateTime StoredExpiry()
        {
            try
            {
                if (!File.Exists(LicenseFile)) return DateTime.MinValue;
                DateTime exp; string r;
                return VerifyKey(File.ReadAllText(LicenseFile), out exp, out r) ? exp : DateTime.MinValue;
            }
            catch { return DateTime.MinValue; }
        }
    }
}
