@echo off
mkdir Zip

xcopy Gavilya\bin\Release\net8.0-windows\ Zip /s /e /y
xcopy Gavilya.Fps\bin\Release\ Zip /s /e /y

pause
