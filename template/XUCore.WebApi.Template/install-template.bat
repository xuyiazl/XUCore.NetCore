@echo off

set Version=2.0.3

echo %Version%

dotnet new -u XUCore.WebApi.Template

dotnet new --install XUCore.WebApi.Template::%Version%

pause

