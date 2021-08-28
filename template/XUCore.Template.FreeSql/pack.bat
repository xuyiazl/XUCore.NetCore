@echo off

set version=0.0.2

echo version=%version%

cd E:\GitHub\XUCore.NetCore\template\XUCore.Template.FreeSql

nuget pack XUCore.Template.FreeSql.nuspec -NoDefaultExcludes -OutputDirectory .

cd /

pause

