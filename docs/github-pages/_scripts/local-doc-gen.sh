#!/bin/bash

set -e

readonly default_port=8080
readonly script_dir=$(dirname $0)
readonly docfx_cmd=$(command -v docfx 2>/dev/null || command -v docfx.exe 2>/dev/null)

# Read port number from the first argument, use default port if not provided
readonly port=${1:-$default_port}

# Validate port
if ! [[ $port =~ ^[0-9]+$ ]] ; then
   echo "Error: Port must be a number" >&2
   exit 1
fi

if [ -z "$docfx_cmd" ]; then
  echo "Error: docfx is not installed or not available on PATH" >&2
  exit 1
fi

# Remove Existing Documentation
if [ -d "_site" ]; then
  rm -r _site
fi

if [ -d "api-docs" ]; then
  rm -r api-docs
fi

# Restore solution once so repeated metadata passes can skip restore.
dotnet restore ../../src/Dapper.SimpleSqlBuilder.slnx

# Generate Xrefmap
$script_dir/generate-xrefmap.sh

# Generate Metadata
$script_dir/generate-metadata.sh

# Build Documentation
"$docfx_cmd" build docfx.json

# Serve Documentation
"$docfx_cmd" serve _site --port $port
