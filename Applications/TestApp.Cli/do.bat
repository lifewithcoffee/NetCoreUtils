@echo off
SET DEBUG_PATH=%CD%\bin\Debug\net6.0
SET NETCOREUTILS_ENVIRONMENT=Development

:: Using "dotnet run" is too slow
dotnet %DEBUG_PATH%\TestApp.Cli.dll %*

:: or set the debug path to system path, then use the following statement to
:: replace the above one:
:: 
:: dotnet %~dp0TestApp.Cli.dll %*
@echo on