@echo off
REM ============================================================
REM  Adv_POS - one-click database setup
REM  Double-click this file. It creates the APOSDB database and
REM  points the app at it. Nothing else to do by hand.
REM ============================================================
setlocal
cd /d "%~dp0"

echo.
echo Starting Adv_POS database setup...
echo.

powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Setup-Database.ps1" %*

if %ERRORLEVEL% NEQ 0 (
  echo.
  echo Setup did not finish. See the message above.
  pause
)
endlocal
