@echo off

set Version=1.1.1

echo %Version%

dotnet new -u XUCore.Template.Ddd

dotnet new --install XUCore.Template.Ddd::%Version%

pause

