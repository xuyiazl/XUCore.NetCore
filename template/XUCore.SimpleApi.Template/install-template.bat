@echo off

set Version=1.0.6

echo %Version%

dotnet new -u XUCore.SimpleApi.Template

dotnet new --install XUCore.SimpleApi.Template::%Version%

pause

