#!/bin/sh

CAKE_ARGS="0-build/build.cake -verbosity=diagnostic"

export NUGET_REPOSITORY_API_URL='http://nuget-server.test/nuget'
export NUGET_REPOSITORY_API_KEY='123456'

dotnet --info

dotnet tool restore

dotnet cake $CAKE_ARGS "$@"
