@echo off
echo ====================================
echo Llama.cpp Server Loader - Publish Script
echo ====================================
echo.

echo Checking for .NET SDK...
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET SDK not found!
    echo Please download and install .NET 8.0 SDK from:
    echo https://dotnet.microsoft.com/download/dotnet/8.0
    echo.
    pause
    exit /b 1
)

echo .NET SDK found!
echo.

echo Publishing as single-file application...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false
if %errorlevel% neq 0 (
    echo ERROR: Publish failed
    pause
    exit /b 1
)

echo.
echo ====================================
echo Publish completed successfully!
echo ====================================
echo.
echo Single-file executable location:
echo bin\Release\net8.0-windows\win-x64\publish\LlamaCppLoader.exe
echo.
echo This executable includes all dependencies and can be
echo distributed without requiring .NET Runtime installation.
echo.
pause
