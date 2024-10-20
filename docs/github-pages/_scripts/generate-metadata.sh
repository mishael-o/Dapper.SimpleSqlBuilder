#!/bin/bash

readonly metadata_file_dir="api-docs"

change_netstd2_uids() {
    local -r file_dir="$metadata_file_dir/netstd2"

    # Loop through all yaml files in the api-docs/netstd2 folder
    for file in $(find $file_dir -name "*.yml" -o -name "*.yaml"); do
        # Update the uid property to prefix with netstd2.
        sed -i "s|uid: |uid: netstd2.|g" $file

        # Update text that contains <xref href="Dapper. to <xref href="netstd2.Dapper.
        sed -i "s|<xref href=\"Dapper.SimpleSqlBuilder.|<xref href=\"netstd2.Dapper.SimpleSqlBuilder.|g" $file
    done
}

remove_extension_methods() {
    # Block of text to remove
    local -r pattern="- h4: Extension Methods"

    # Loop through all yaml files in the api-docs folder
    for file in $(find $metadata_file_dir -name "*.yml" -o -name "*.yaml"); do
        # Ignore file starts with toc
        if [[ $file == *"toc.yml"* ]] || [[ $file == *"toc.yaml"* ]]; then
            continue
        fi

        # Use sed to remove the block of text
        sed -i "/$pattern/,+3d" "$file"
    done
}

docfx metadata docfx.json
change_netstd2_uids
remove_extension_methods
