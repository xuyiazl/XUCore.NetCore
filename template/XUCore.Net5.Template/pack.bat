@echo off

set version=1.0.7

echo version=%version%

cd E:\GitHub\XUCore.NetCore\template\XUCore.Net5.Template

nuget pack XUCore.Net5.Template.nuspec -NoDefaultExcludes -OutputDirectory .

cd /

pause

