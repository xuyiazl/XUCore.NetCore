@echo off

set Version=1.0.3

echo %Version%

dotnet new -u XUCore.WebApi2.Template

dotnet new --install XUCore.WebApi2.Template::%Version%

pause

