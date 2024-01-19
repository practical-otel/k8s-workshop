# Exercise 2 - Prometheus for metrics

Add the prometheus chart repository
```shell
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
```

Install the prometheus chart
```shell
helm install prometheus prometheus-community/prometheus --values prometheus-values.yaml
```

Access Prometheus locally
```shell
export POD_NAME=$(kubectl get pods -l "app.kubernetes.io/name=prometheus,app.kubernetes.io/instance=prometheus" -o jsonpath="{.items[0].metadata.name}")
kubectl --namespace otel port-forward $POD_NAME 9090
```

Open your browser to [http://localhost:9090](https://localhost:9090)

***NOTE:** If you're using the dev container, the above commands may not auto-map the ports required. Click "Ports" tab in your terminal and add port 9090 there*