[CmdletBinding()]
Param(
    [string]$Script = "0-build/build.cake",
    [string]$Target,
    [string]$Configuration,
    [ValidateSet("Quiet", "Minimal", "Normal", "Verbose", "Diagnostic")]
    [string]$Verbosity = "Verbose",
    [switch]$Help,
    [Alias("WhatIf", "Noop")]
    [switch]$DryRun,
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$ScriptArgs
)

# Build Cake arguments
$cakeArguments = @("$Script");
if ($Target) { $cakeArguments += "-target=$Target" }
if ($Configuration) { $cakeArguments += "-configuration=$Configuration" }
if ($Verbosity) { $cakeArguments += "-verbosity=$Verbosity" }
if ($Help) { $cakeArguments += "-showdescription" }
if ($DryRun) { $cakeArguments += "-dryrun" }
if ($Experimental) { $cakeArguments += "-experimental" }
if ($Mono) { $cakeArguments += "-mono" }
$cakeArguments += $ScriptArgs

# Install  cake.tool
dotnet tool install --global cake.tool --version 0.30.0

# Start Cake
dotnet cake $cakeArguments

exit $LASTEXITCODE