@echo off

set version=2.0.3

echo version=%version%

cd E:\GitHub\XUCore.NetCore\template\XUCore.WebApi2.Template

nuget pack XUCore.WebApi2.Template.nuspec -NoDefaultExcludes -OutputDirectory .

cd /

pause

