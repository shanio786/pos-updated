# Adv_POS — Apne Windows PC par EXE banana aur chalana

Yeh ek **C# WinForms (.NET Framework)** app hai jo **MS SQL Server** se chalti hai.
EXE double-click wala ready file nahi diya ja sakta kyunke:
- Build Windows par Visual Studio se hota hai (vendor libraries + NuGet packages wahin resolve hote hain).
- App ko chalne ke liye SQL Server database chahiye.
- Downloaded EXE ko Chrome/antivirus block kar deta hai.

Isliye EXE aap **2 minute me Visual Studio se khud** bana lenge — aur wahi EXE aapke SQL se connect bhi ho jayega.

---

## 1) Jo pehle se chahiye (aap install kar chuke hain)
- **Visual Studio** (Community) — ".NET desktop development" workload
- **SQL Server Express** (instance: `SQLEXPRESS`)
- **SSMS** (database dekhne ke liye)
- (Reports forms ke liye) **SAP Crystal Reports runtime for Visual Studio** — agar build me `CrystalDecisions` wali error aaye to yeh install karein.

## 2) Latest code apne PC par laayein

**Agar git use kiya tha:**
```
git fetch origin
git checkout claude/database-feature-review-3zf7xg
git pull origin claude/database-feature-review-3zf7xg
```

**Agar ZIP download karte hain (git nahi):**
- GitHub par repo kholein → branch `claude/database-feature-review-3zf7xg` select karein → **Code → Download ZIP**.
- Chrome warning de to **Keep / Keep anyway** click karein (safe hai, aapka apna code hai).
- ZIP ko extract karein.

## 3) Visual Studio me kholein
- `Adv_POS.sln` par double-click.
- Solution Explorer me solution par right-click → **Restore NuGet Packages**.

## 4) Build karein
- Top bar me config **Debug** rakhein (platform `x86` ya `AnyCPU` dono theek).
- Menu: **Build → Build Solution** (ya `Ctrl + Shift + B`).
- Agar `CrystalDecisions...` ya `MySql.Data` ki reference error aaye:
  - Crystal: upar step 1 wala Crystal runtime install karke dobara build.
  - MySql.Data: app MS SQL use karti hai; agar sirf yeh reference missing hai to mujhe bata dein, main isko project se nikaal dunga (zaroorat nahi).

## 5) EXE kahan banega
Build ke baad EXE yahan milega:
```
supershop\bin\Debug\Adv_POS.exe
```
(agar x86 config chuna to: `supershop\bin\x86\Debug\Adv_POS.exe`)

## 6) Chalana (Run)
- Visual Studio me **F5** dabayein (ya upar wala EXE double-click karein).
- Database connection `app.config` me set hai:
  ```
  Data Source=.\SQLEXPRESS ; Initial Catalog=APOSDB
  ```
- Agar database `APOSDB` abhi nahi bana: `Setup\Setup.bat` chala kar bana lein (ya SSMS me script chala dein), phir app chalayein.
- Login screen aayegi → apne user se login karein.

## 7) Meri UI fixes kahan dikhengi
- Label/field overlap (Terminal ka payment panel, Customer/Due/Pay by waghera) ab theek honge.
- VAT ka `%` box ke andar nahi ghusega.
- Yeh sab **modern theme ON** hone par apply hota hai (`app.config` me `ModernTheme=true` — pehle se on hai).

---

### Masla aaye to
Build ya run me koi error aaye to uska **screenshot** bhej dein — main exact fix bata dunga ya code me theek kar ke push kar dunga.
