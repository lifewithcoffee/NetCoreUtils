@echo off
SET DEBUG_PATH=%CD%\bin\Debug\net5.0
SET NETCOREUTILS_ENVIRONMENT=Development

dotnet %DEBUG_PATH%\TestApp.Cli.dll %*

:: or set the debug path to system path, then use the following statement to
:: replace the above one:
:: 
:: dotnet %~dp0TestApp.Cli.dll %*
@echo on