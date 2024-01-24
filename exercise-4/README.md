# Install OpenSearch and push the logs pipeline to it

```shell
helm repo add opensearch https://opensearch-project.github.io/helm-charts/
helm repo update
```

```shell
helm install opensearch opensearch/opensearch --values opensearch-values.yaml
```

Forward port 9200 from the container.

```shell
curl -XGET https://localhost:9200 -u 'admin:admin' --insecure
```

