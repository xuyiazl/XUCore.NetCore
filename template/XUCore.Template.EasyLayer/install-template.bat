@echo off

set Version=1.0.4

echo %Version%

dotnet new -u XUCore.Template.EasyLayer

dotnet new --install XUCore.Template.EasyLayer::%Version%

pause

