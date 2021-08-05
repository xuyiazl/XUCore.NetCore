@echo off

set version=2.0.2

echo version=%version%

cd E:\GitHub\XUCore.NetCore\template\XUCore.WebApi.Template

nuget pack XUCore.WebApi.Template.nuspec -NoDefaultExcludes -OutputDirectory .

cd /

pause

