#!/bin/bash

cd cluster
eval $(pulumi stack output --shell)

echo "Getting credentials for $1"
name=$1
clusterDetails=${!name}
clusterName=$(echo $clusterDetails | jq -r .clusterName)
clusterResourceGroup=$(echo $clusterDetails | jq -r .clusterResourceGroup)

az aks get-credentials -n $clusterName -g $clusterResourceGroup -f -

cd - > /dev/null