# Paths
$packFolder = (Get-Item -Path "./nupkgs" -Verbose).FullName
$slnPath = Join-Path $packFolder "../"
$srcPath = Join-Path $slnPath "src"

# List of projects
$projects = (
    "XUCore",
    "XUCore.Excel",
    "XUCore.Script",
    "XUCore.NetCore",
    "XUCore.NetCore.AspectCore",
    "XUCore.NetCore.Data",
    "XUCore.NetCore.FreeSql",
    "XUCore.NetCore.Mongo",
    "XUCore.NetCore.Redis",
    "XUCoreApp"
)
