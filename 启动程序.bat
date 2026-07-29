@echo off
echo ========================================
echo   Llama.cpp Server Loader v1.2.0
echo ========================================
echo.
echo 正在启动程序...
echo.

start "" "bin\Release\net8.0-windows\LlamaCppLoader.exe"

echo.
echo 程序已启动！
echo.
echo 新功能提示：
echo - 所有参数都有中英文说明
echo - 鼠标悬停查看详细 ToolTip
echo - 默认值已优化（Context: 65536, Repeat Penalty: 1.08）
echo - 查看 参数详解.md 了解完整说明
echo.
pause
