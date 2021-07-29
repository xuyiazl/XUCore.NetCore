@echo off

set version=2.0.0

echo version=%version%

cd E:\GitHub\XUCore.NetCore\template\XUCore.SimpleApi.Template

nuget pack XUCore.SimpleApi.Template.nuspec -NoDefaultExcludes -OutputDirectory .

cd /

pause

