#!/bin/sh

set -x -e

CAKE_SCRIPT_FILE='0-build/build.cake'
CAKE_ARGS='-verbosity=diagnostic'

export NUGET_REPOSITORY_API_URL='http://nuget-server.test/nuget'
export NUGET_REPOSITORY_API_KEY='123456'

dotnet --info

dotnet tool restore

dotnet format --check --dry-run --verbosity minimal

dotnet cake $CAKE_SCRIPT_FILE $CAKE_ARGS "$@"
