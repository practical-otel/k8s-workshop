#

```sh
helm install opentelemetry-operator open-telemetry/opentelemetry-operator --values operator-values.yaml
```

```sh
helm repo add jaegertracing https://jaegertracing.github.io/helm-charts
```

```sh
helm install jaeger jaegertracing/jaeger --values jaeger-value.yaml
```

```sh
kubectl apply -f instrumentation.yaml
```

Be careful about namespaces.

