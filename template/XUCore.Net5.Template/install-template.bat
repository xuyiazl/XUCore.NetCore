@echo off

set Version=1.0.8

echo %Version%

dotnet new -u XUCore.Net5.Template

dotnet new --install XUCore.Net5.Template::%Version%

pause

