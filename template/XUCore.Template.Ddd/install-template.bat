@echo off

set Version=1.1.3

echo %Version%

dotnet new -u XUCore.Template.Ddd

dotnet new --install XUCore.Template.Ddd::%Version%

pause

