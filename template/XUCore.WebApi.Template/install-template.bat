@echo off

set Version=1.0.6

echo %Version%

dotnet new -u XUCore.WebApi.Template

dotnet new --install XUCore.WebApi.Template::%Version%

pause

