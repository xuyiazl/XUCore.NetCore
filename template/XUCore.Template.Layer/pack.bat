@echo off

set version=1.0.0

echo version=%version%

cd E:\GitHub\XUCore.NetCore\template\XUCore.Template.Layer

nuget pack XUCore.Template.Layer.nuspec -NoDefaultExcludes -OutputDirectory .

cd /

pause

