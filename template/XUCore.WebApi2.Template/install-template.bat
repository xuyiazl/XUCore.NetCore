@echo off

set Version=2.0.1

echo %Version%

dotnet new -u XUCore.WebApi2.Template

dotnet new --install XUCore.WebApi2.Template::%Version%

pause

