#!/bin/bash

# Trouver les fichiers volumineux dans l'historique Git
echo "Finding large files in the repository..."
large_files=$(git rev-list --objects --all | grep $(git verify-pack -v .git/objects/pack/pack-*.idx \
| sort -k 3 -n \
| tail -5 \
| awk '{print$1}'))

if [ -z "$large_files" ]; then
    echo "No large files found."
    exit 0
fi

echo "Large files found:"
echo "$large_files"

# Initialiser Git LFS (si ce n'est pas déjà fait)
git lfs install

# Suivre les fichiers volumineux avec Git LFS
echo "Tracking large files with Git LFS..."
while IFS= read -r line; do
    file_path=$(echo $line | awk '{print $2}')
    if [ -n "$file_path" ]; then
        git lfs track "$file_path"
        git add "$file_path"
    fi
done <<< "$large_files"

# Ajouter et committer les fichiers suivis par Git LFS
git add .gitattributes
git commit -m "Track large files with Git LFS"

echo "Pushing changes to the remote repository..."
git push origin main

echo "Done."
