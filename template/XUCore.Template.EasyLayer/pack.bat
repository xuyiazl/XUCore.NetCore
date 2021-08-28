@echo off

set version=1.1.5

echo version=%version%

cd E:\GitHub\XUCore.NetCore\template\XUCore.Template.EasyLayer

nuget pack XUCore.Template.EasyLayer.nuspec -NoDefaultExcludes -OutputDirectory .

cd /

pause

