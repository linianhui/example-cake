#!/bin/sh

SCRIPT="0-build/build.cake"

# Install  cake.tool
dotnet tool install --global cake.tool --version 0.30.0

# Start Cake

echo "\033[32mdotnet cake $SCRIPT $@"

dotnet cake $SCRIPT "$@"
