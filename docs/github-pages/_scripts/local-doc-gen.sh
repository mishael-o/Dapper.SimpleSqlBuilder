#!/bin/bash

set -e

readonly default_port=8080
readonly script_dir=$(dirname $0)

# Read port number from the first argument, use default port if not provided
readonly port=${1:-$default_port}

# Validate port
if ! [[ $port =~ ^[0-9]+$ ]] ; then
   echo "Error: Port must be a number" >&2
   exit 1
fi

readonly base_url="http://localhost:$port"
echo "Base URL: $base_url"

# Remove Existing Documentation
if [ -d "_site" ]; then
  rm -r _site
fi

if [ -d "api-docs" ]; then
  rm -r api-docs
fi

# Generate Xrefmap
$script_dir/generate-xrefmap.sh $base_url

# Generate Metadata
$script_dir/generate_metadata.sh

# Build Documentation
docfx build docfx.json

# Serve Documentation
docfx serve _site --port $port
