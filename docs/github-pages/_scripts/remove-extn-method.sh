#!/bin/bash

# Define the block of text to remove
PATTERN="- h4: Extension Methods"

files=$(find api-docs -name "*.yml" -o -name "*.yaml")

# Loop over all .yml files in the current directory
for file in $files
do
  # ignore file starts with toc
  if [[ $file == *"toc.yml"* ]] || [[ $file == *"toc.yaml"* ]]; then
    continue
  fi

  # Use sed to remove the block of text
  sed -i "/$PATTERN/,+3d" "$file"
done