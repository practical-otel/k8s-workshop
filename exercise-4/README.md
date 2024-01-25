# Install OpenSearch and push the logs pipeline to it

```shell
helm repo add opensearch https://opensearch-project.github.io/helm-charts/
helm repo update
```

Install OpenSearch, and the dashboard UI

```shell
helm install opensearch opensearch/opensearch --values opensearch-values.yaml
helm install os-dashboards opensearch/opensearch-dashboards --values opensearch-dashboards-values.yaml
```

```shell
helm upgrade my-opentelemetry-collector open-telemetry/opentelemetry-collector --values ./values.yaml
```

Forward port 5601 from the dashboards container, and open up http://localhost:5601

```shell
curl -XGET https://localhost:9200 -u 'admin:admin' --insecure
```

