[string]$CAKE_ARGS = "0-build/build.cake -verbosity=diagnostic"

$ENV:NUGET_REPOSITORY_API_URL = "http://nuget-server.test/nuget"
$ENV:NUGET_REPOSITORY_API_KEY = "123456"

dotnet --info

dotnet tool restore

Write-Host "dotnet cake $CAKE_ARGS $ARGS" -ForegroundColor GREEN

dotnet cake $CAKE_ARGS $ARGS