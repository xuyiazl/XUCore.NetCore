@echo off

set version=1.0.2

echo version=%version%

cd E:\GitHub\XUCore.NetCore\template\XUCore.Template.EasyLayer

nuget pack XUCore.Template.EasyLayer.nuspec -NoDefaultExcludes -OutputDirectory .

cd /

pause
