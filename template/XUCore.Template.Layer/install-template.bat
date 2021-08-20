@echo off

set Version=1.1.1

echo %Version%

dotnet new -u XUCore.Template.Layer

dotnet new --install XUCore.Template.Layer::%Version%

pause

