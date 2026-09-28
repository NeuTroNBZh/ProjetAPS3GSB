@echo off
rem Desinstalle GSB-CR de ce poste (droits administrateur demandes).
net session >nul 2>&1
if errorlevel 1 (
    powershell -NoProfile -Command "Start-Process -FilePath '%~f0' -Verb RunAs"
    exit /b
)
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0desinstaller.ps1"
echo.
pause
