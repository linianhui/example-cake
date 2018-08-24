[string]$SCRIPT = "0-build/build.cake"

# Install  cake.tool
dotnet tool install --global cake.tool --version 0.30.0

# Start Cake
Write-Host "dotnet cake $SCRIPT $ARGS" -ForegroundColor GREEN
dotnet cake $SCRIPT $ARGS
