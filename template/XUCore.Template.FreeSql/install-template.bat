@echo off

set Version=0.0.2

echo %Version%

dotnet new -u XUCore.Template.FreeSql

dotnet new --install XUCore.Template.FreeSql::%Version%

pause

