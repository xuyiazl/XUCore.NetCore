@echo off

set Version=1.0.0

echo %Version%

dotnet new -u XUCore.Template.Layer

dotnet new --install XUCore.WebApi.Template::%Version%

pause

