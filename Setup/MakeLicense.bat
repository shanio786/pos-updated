@echo off
REM ============================================================
REM  Adv_POS - License Key Maker (VENDOR tool - keep private)
REM  Double-click, paste the customer's Machine ID, get a key.
REM  NEVER give this file to a customer - it holds your signing key.
REM ============================================================
setlocal
cd /d "%~dp0"
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0MakeLicense.ps1" %*
endlocal
