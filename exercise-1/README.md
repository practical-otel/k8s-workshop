# Exercise 1 - Deploy a collector

In this exercise, you'll deploy a collector that will do nothing other than output it's own metrics.

Create a namespace
```shell
kubectl create namespace otel
```

Set the default current namespace
```shell
kubectl config set-context --current --namespace=otel
```


Create the collectors
```shell
helm install my-opentelemetry-collector open-telemetry/opentelemetry-collector --values ./values.yaml
```

Get the collector pods pods
```shell
kubectl get pods
```

Get the logs
```shell
kubectl logs <pod name>
```

You should see logs for metrics from the collector itself.

## Links

* Helm charts - https://github.com/open-telemetry/opentelemetry-helm-charts
* Collector chart - https://github.com/open-telemetry/opentelemetry-helm-charts/tree/main/charts/opentelemetry-collector