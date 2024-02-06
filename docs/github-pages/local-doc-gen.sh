#!/bin/bash

# Remove Existing Documentation
if [ -d "_site" ]; then
  rm -r _site
fi

if [ -d "api-docs" ]; then
  rm -r api-docs
fi

# Generate Generate XrefMap
docfx metadata docfx-xref.json
docfx build docfx-xref.json
cp -f _xref-gen/core.xrefmap.yml xrefs
cp -f _xref-gen/netstd2.xrefmap.yml xrefs
rm -r _xref-gen
rm -r api-docs

# Generate Metadata
docfx metadata docfx.json

# Remove Extension Methods
./remove-ext-method.sh

# Build Documentation
docfx build docfx.json

# Fix Xref Links
./fix-xref-links.sh

# Serve Documentation
docfx serve _site