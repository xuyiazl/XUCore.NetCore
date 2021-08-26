@echo off

set version=1.1.4

echo version=%version%

cd E:\GitHub\XUCore.NetCore\template\XUCore.Template.Ddd

nuget pack XUCore.Template.Ddd.nuspec -NoDefaultExcludes -OutputDirectory .

cd /

pause

