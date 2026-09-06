#!/bin/bash

# Get subpath from first argument or default to "/"
sub_path="${1:-/}"

# Add trailing slash if not present
[[ "${sub_path}" != */ ]] && sub_path="${sub_path}/"

# Add leading slash if not present
[[ "${sub_path}" != /* ]] && sub_path="/${sub_path}"

readonly sub_path
readonly gen_folder="_xref-gen"
readonly file_path="xrefs"
readonly docfx_cmd=$(command -v docfx 2>/dev/null || command -v docfx.exe 2>/dev/null)

if [ -z "$docfx_cmd" ]; then
    echo "Error: docfx is not installed or not available on PATH" >&2
    exit 1
fi

# Copy and update xrefmap files with base URL in href property.
copy_and_update_xrefmap_files() {
    local -r files=("core.xrefmap.yml" "netstd2.xrefmap.yml")

    for file in "${files[@]}"; do
        # Copy file to xrefs folder
        cp -f "$gen_folder/$file" "$file_path"

        # Add forward slash to href property
        sed -i "s|href: |href: $sub_path|g" "$file_path/$file"

        # Update uid property in netstd2.xrefmap.yml to prefix with netstd2.
        if [ "$file" == "netstd2.xrefmap.yml" ]; then
            sed -i "s|uid: |uid: netstd2.|g" "$file_path/$file"
        fi
    done
}

"$docfx_cmd" metadata docfx-xref.json --noRestore
"$docfx_cmd" build docfx-xref.json
copy_and_update_xrefmap_files
rm -r api-docs
rm -r $gen_folder
