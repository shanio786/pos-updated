# Licensing (offline, machine-locked)

Adv_POS uses a simple, secure, **offline** license. No server, no internet.

## For the shop (end user)
1. Install and run Adv_POS. On first run it shows an **Activation** screen with a
   **Machine ID** (unique to that computer).
2. Send the Machine ID to your supplier.
3. The supplier gives you a **License Key**. Paste it in and press **Activate**.
4. Done — the app runs on that computer. The key works only on that machine.

## For the supplier (vendor)
The key generator is in `Tools/LicenseKeyGen` (see its README).

1. **Once**, make your own key pair and embed the public key:
   ```
   LicenseKeyGen genkeys
   ```
   Copy `public_key.xml` content into `supershop/Licensing/LicenseManager.cs`
   (`PublicKeyXml`), rebuild, and ship that build. Keep `private_key.xml` secret.
2. To issue a license, take the customer's Machine ID and run:
   ```
   LicenseKeyGen sign ABCDE-FGHIJ-KLMNP 0 STD
   ```
   `0` = never expires (use `20271231` for a dated license). Send the printed key.

## Why it is secure
The key is an RSA-2048 signature of `MachineID | expiry | edition`. The app
verifies it with the embedded public key, so a key cannot be forged and cannot be
copied to another PC. Verified: correct machine passes; other machine, expired,
tampered and forged keys are all rejected.

> The build in this repo ships with a **sample** public key. Generate your own
> pair before selling so only your keys are valid.
