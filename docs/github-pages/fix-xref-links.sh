#!/bin/bash

# Function to replace directory path in .html files
replace_dir_path() {
    local dir="_site/api-docs/$1"
    local files=$(find "$dir" -name "*.html")

    for file in $files
    do
        if [[ $1 == "di" ]]; 
        then
            sed -i "s|api-docs/di/||g" "$file"
        else
            sed -i "s|api-docs/netcore/||g" "$file"
            sed -i "s|api-docs/netstd2/||g" "$file"
        fi

        # echo "Fixed links in $file"
    done
}

# Call the function for each directory
replace_dir_path "di"
replace_dir_path "netcore"
replace_dir_path "netstd2"