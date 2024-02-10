#!/bin/bash

docfx metadata docfx-xref.json
docfx build docfx-xref.json
cp -f _xref-gen/core.xrefmap.yml xrefs
cp -f _xref-gen/netstd2.xrefmap.yml xrefs
rm -r _xref-gen
rm -r api-docs