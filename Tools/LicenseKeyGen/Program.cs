using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

// Adv_POS - License Key Generator (VENDOR TOOL - keep private_key.xml secret!)
//
// Build (once):
//   csc Program.cs           (or compile as a small console project)
// Generate a fresh key pair (first time only):
//   LicenseKeyGen genkeys
//     -> writes private_key.xml (keep secret) and public_key.xml
//        Put the contents of public_key.xml into
//        supershop/Licensing/LicenseManager.cs  (PublicKeyXml constant).
// Issue a license for a customer:
//   LicenseKeyGen sign <MachineID> [expiry yyyyMMdd | 0] [edition]
//     e.g.  LicenseKeyGen sign ABCDE-FGHIJ-KLMNP 0 STD
//     -> prints the License Key to give the customer.
class Program
{
    static int Main(string[] args)
    {
        try
        {
            if (args.Length >= 1 && args[0] == "genkeys")
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
                {
                    File.WriteAllText("private_key.xml", rsa.ToXmlString(true));
                    File.WriteAllText("public_key.xml", rsa.ToXmlString(false));
                }
                Console.WriteLine("Created private_key.xml (KEEP SECRET) and public_key.xml.");
                Console.WriteLine("Copy public_key.xml content into LicenseManager.PublicKeyXml.");
                return 0;
            }

            if (args.Length >= 2 && args[0] == "sign")
            {
                string machineId = args[1].Trim();
                string expiry = args.Length >= 3 ? args[2].Trim() : "0";
                string edition = args.Length >= 4 ? args[3].Trim() : "STD";
                if (!File.Exists("private_key.xml"))
                {
                    Console.WriteLine("private_key.xml not found. Run 'LicenseKeyGen genkeys' first.");
                    return 1;
                }
                string message = machineId + "|" + expiry + "|" + edition;
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(File.ReadAllText("private_key.xml"));
                    byte[] sig = rsa.SignData(Encoding.UTF8.GetBytes(message), new SHA256CryptoServiceProvider());
                    string key = expiry + "|" + edition + "|" + Convert.ToBase64String(sig);
                    Console.WriteLine();
                    Console.WriteLine("License Key for " + machineId +
                                      (expiry == "0" ? " (never expires)" : " (expires " + expiry + ")") + ":");
                    Console.WriteLine();
                    Console.WriteLine(key);
                    Console.WriteLine();
                }
                return 0;
            }

            Console.WriteLine("Usage:");
            Console.WriteLine("  LicenseKeyGen genkeys");
            Console.WriteLine("  LicenseKeyGen sign <MachineID> [expiry yyyyMMdd | 0] [edition]");
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return 1;
        }
    }
}
