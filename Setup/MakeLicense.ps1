<#
  Adv_POS - license key maker (VENDOR tool)
  -----------------------------------------
  Turns a customer's Machine ID (shown on the app's Activation screen)
  into a License Key they paste back to activate. Works offline.

  Usage:
    - Double-click MakeLicense.bat and paste the Machine ID when asked, OR
    - powershell -File MakeLicense.ps1 -MachineId "Q7624-H9WQJ-63UG5"
    - Add -Expiry 20271231 for a dated license (default 0 = never expires)
    - Add -Edition PRO to change the edition tag (default STD)

  SECURITY: this file holds YOUR private signing key. It is the vendor's
  tool - keep it. NEVER ship MakeLicense.* to a customer; give them only
  the built Adv_POS app (which carries the public key). Before selling to
  others, generate your own key pair and replace both this private key and
  the PublicKeyXml in supershop\Licensing\LicenseManager.cs.
#>
param(
    [string]$MachineId = "",
    [string]$Expiry    = "0",
    [string]$Edition   = "STD"
)

$ErrorActionPreference = "Stop"

# --- the vendor private key (matches PublicKeyXml in LicenseManager.cs) ---
$PrivateKeyXml = '<RSAKeyValue><Modulus>hYVQJrR74iJVaZbXGvMfL9jl0Kq/n4Awiamg5f9CXaqGefTBlqYGUxpH+xlHQXvSzTje5PVwIhmYprC0edOx4SFdw6aL8tB6M8UOuz8PZtE54y+qNfXVorBzVWCJe4Mh8HiiqIxLc5Vmt8BJwWSE5P7d2gWYZtvKjIpaVaM4v4/d/Zn3Sl+Zn7zxWjjRn9z03QGBCsKYBjSc+VNr2UrYgFw/v3TeXtifgYUBlDAjstvzSdkf2lI6yTI5PCmfXxngN+eIMhFXSMoEH1q895vAaoau/37qEjDY5nN496lpmr97SUP5IdyS6jDjcOZCAAg19LlS5UCXt3JWTW/Z1yw6aw==</Modulus><Exponent>AQAB</Exponent><P>u8zMwxi0tOGKKB6VtcHhJfm4swVRvYeZ2EvZmhVW8pJqmXQAyw3jKSzlEGQH6HTG21Xs6WrwKkMUNRUgrpttfSlF+eh8ueHcovdeX8iIztB8ZgH3h5X/pqscNxGI0VuPN8LKSLyPdIN5eBZeq4HTulmrPYqxPLj4ZPHGI/N8VuM=</P><Q>tgJV2Uq2N5AJwn99OFEz189zAfVTw4eoM27kJveey0o6XaODGuNlIrfv6qgxizNFcB6Zk6LB5MWYci8sFvBjbhYpperxmAAvWNC1zvSVv+gSCpJzmNQz2lvmP5MdsaaWv7he92Llltf0pghCLQZ83vJaEFaw+lyZ75UeChbPXNk=</Q><DP>CBP4HtA6PamR6sXeBLJxP9PtCO5KpM9nY7Fni8QtOEPqB+3AdvuC8Ot4cusKmAol35tjlGrAJ+E4xkvBWyFeZUrYMra4XrqDZhMj4RTcJaE8KJsfDZr7Iy0rCodbjj3U/D4Ju3U46ncZS3wS2Ge+Nr7SqdQEaas/LABmWHeJuqc=</DP><DQ>Q1jV31Nv3Vtk4R0/fmk9n6tZSO44Em/N40ozDeAuV9adhiyMxJRDxfb9xXx66KEOHpCUDzb4O+iJoiamT86fXArDWmt2/eGDo3+G5o9GIf6DPno2X+SVU0USC5AqOqQDv+k+6pcjkQ9TrWP86KAv8GcIwGbzkZBWPXqV5PHPxPE=</DQ><InverseQ>qv2ybI+SHQqWWFTU4gKZKHHTYfZN9qp6lq9cbGPVBzajqzN9tBc9El0VCj185aD8evgdC3+tnmUDtcRS9wvRpAF8c6mi7QL4xCZZkZ5ubY/PzIZjQNna5JqPswhOzYwDAsDiI6Zqan8PVFm+6mteB1UAtY3V8uEqO/deH4nDv38=</InverseQ><D>NzspqBERi73DEh301e+Kz4qLqhDvmkV3DoLuDSXVaJSA8xLFP5Fm3VpqOxq+SH61ePHU9eX3IAvxgsZmqVxh4Owh8qxCiMsnOaICkX8o2lGi/fC6Gn8aA6W017TKuh5I3EWyl0ENdv7ukgUHHWn8tRMeZzDYVXgGpbMXYgRWFZsO8fuqTQ4aUxVidVLA+XvAH2LZejLmTHmTjk1v7P0pO162Pc4mnX/Nrmt8ECMBuD8kwE/iCv4YixdVha/EJSuxNdS3+Om7csAaCCwl6ljeRS2YeEejq4tXHZUMLpVtGOOry4ZvUGjOAMZdxEX1NjHsKjbpOivzUhKs+VnOU73uyQ==</D></RSAKeyValue>'

Write-Host ""
Write-Host "==============================================" -ForegroundColor Cyan
Write-Host "   Adv_POS  -  License Key Maker" -ForegroundColor Cyan
Write-Host "==============================================" -ForegroundColor Cyan
Write-Host ""

if ([string]::IsNullOrWhiteSpace($MachineId)) {
    $MachineId = Read-Host "Paste the customer's Machine ID (e.g. Q7624-H9WQJ-63UG5)"
}
$MachineId = $MachineId.Trim()
if ([string]::IsNullOrWhiteSpace($MachineId)) {
    Write-Host "No Machine ID entered." -ForegroundColor Red
    Read-Host "Press Enter to exit"; exit 1
}

# message = MachineId|Expiry|Edition   (must match LicenseManager.VerifyKey)
$message = "$MachineId|$Expiry|$Edition"

try {
    $rsa = New-Object System.Security.Cryptography.RSACryptoServiceProvider
    $rsa.FromXmlString($PrivateKeyXml)
    $bytes = [System.Text.Encoding]::UTF8.GetBytes($message)
    $sig = $rsa.SignData($bytes, (New-Object System.Security.Cryptography.SHA256CryptoServiceProvider))
    $sigB64 = [Convert]::ToBase64String($sig)
    $key = "$Expiry|$Edition|$sigB64"

    Write-Host "Machine ID : $MachineId" -ForegroundColor Gray
    Write-Host "Expiry     : $(if ($Expiry -eq '0') {'never'} else {$Expiry})" -ForegroundColor Gray
    Write-Host "Edition    : $Edition" -ForegroundColor Gray
    Write-Host ""
    Write-Host "LICENSE KEY (give this to the customer):" -ForegroundColor Green
    Write-Host ""
    Write-Host $key
    Write-Host ""
    try { Set-Clipboard -Value $key; Write-Host "(copied to clipboard)" -ForegroundColor DarkGray } catch {}
}
catch {
    Write-Host ("Could not make the key: " + $_.Exception.Message) -ForegroundColor Red
    Read-Host "Press Enter to exit"; exit 1
}

Write-Host ""
Read-Host "Press Enter to close"
