@echo off

set Version=2.0.1

echo %Version%

dotnet new -u XUCore.SimpleApi.Template

dotnet new --install XUCore.SimpleApi.Template::%Version%

pause

