#!/bin/bash

if [ -z "$1" ]; then
    echo "Error: Base URL not provided" >&2
    exit 1
fi

## Assign the first argument to base_url and remove trailing slash if present
readonly base_url=${1%/}

readonly gen_folder="_xref-gen"
readonly file_path="xrefs"

# Copy and update xrefmap files with base URL in href property.
copy_and_update_xrefmap_files() {
    local -r files=("core.xrefmap.yml" "netstd2.xrefmap.yml")

    for file in "${files[@]}"; do
        # Copy file to xrefs folder
        cp -f "$gen_folder/$file" "$file_path"

        # Update href property in xrefmap file with base URL
        sed -i "/href: http/!s|href: |href: $base_url/|g" "$file_path/$file"

        # Update uid property in netstd2.xrefmap.yml to prefix with netstd2.
        if [ "$file" == "netstd2.xrefmap.yml" ]; then
            sed -i "s|uid: |uid: netstd2.|g" "$file_path/$file"
        fi
    done
}

docfx metadata docfx-xref.json
docfx build docfx-xref.json
copy_and_update_xrefmap_files
rm -r $gen_folder
rm -r api-docs
