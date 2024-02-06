#!/bin/bash

script_dir=$(dirname $0)

# Remove Existing Documentation
if [ -d "_site" ]; then
  rm -r _site
fi

if [ -d "api-docs" ]; then
  rm -r api-docs
fi

# Generate Generate XrefMap
$script_dir/generate-xrefmap.sh

# Generate Metadata
docfx metadata docfx.json

# Remove Extension Methods
$script_dir/remove-extn-method.sh

# Build Documentation
docfx build docfx.json

# Fix Xref Links
$script_dir/fix-xref-links.sh

# Serve Documentation
docfx serve _site