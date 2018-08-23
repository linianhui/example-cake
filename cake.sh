#!/usr/bin/env bash

# Define default arguments.
SCRIPT="0-build/build.cake"
CAKE_ARGUMENTS=()

# Parse arguments.
for i in "$@"; do
    case $1 in
        -s|--script) SCRIPT="$2"; shift ;;
        --) shift; CAKE_ARGUMENTS+=("$@"); break ;;
        *) CAKE_ARGUMENTS+=("$1") ;;
    esac
    shift
done

# Install  cake.tool
dotnet tool install --global cake.tool --version 0.30.0

# Start Cake
dotnet cake $SCRIPT "${CAKE_ARGUMENTS[@]}"
