# Adv_POS License Key Generator (vendor only)

This tool issues license keys for the offline, machine-locked licensing in Adv_POS.
**Keep `private_key.xml` secret** — anyone with it can issue keys.

## Build
```
csc Program.cs         # produces LicenseKeyGen.exe (Windows, .NET Framework)
```
(or add it as a small Console App project in Visual Studio.)

## One-time: make your own key pair
```
LicenseKeyGen genkeys
```
This writes `private_key.xml` (keep secret, back it up) and `public_key.xml`.
Open `public_key.xml`, copy its whole content, and paste it into
`supershop/Licensing/LicenseManager.cs` as the value of the `PublicKeyXml`
constant, then rebuild Adv_POS. Do this **before** shipping so your keys are
the only ones that work.

## Issue a key to a customer
1. Ask the customer for the **Machine ID** shown on the app's activation screen.
2. Run:
   ```
   LicenseKeyGen sign ABCDE-FGHIJ-KLMNP 0 STD
   ```
   - `0` = never expires. For a yearly license use e.g. `20271231`.
   - `STD` = edition label (free text).
3. Send the printed **License Key** to the customer. They paste it into the
   activation screen and press Activate. The key only works on that machine.

## How it is secure
The key is an RSA-2048 signature of `MachineID|expiry|edition`. The app checks it
with the public key, so a key cannot be forged and cannot be moved to another PC.
No internet or server is required.
