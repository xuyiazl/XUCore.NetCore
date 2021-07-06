@echo off

set version=1.0.3

echo version=%version%

cd E:\GitHub\XUCore.NetCore\template\XUCore.SimpleApi.Template

nuget pack XUCore.SimpleApi.Template.nuspec -NoDefaultExcludes -OutputDirectory .

cd /

pause

