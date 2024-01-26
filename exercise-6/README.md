#

```sh
helm install opentelemetry-operator open-telemetry/opentelemetry-operator --values operator-values.yaml
```

```sh
kubectl apply -f instrumentation.yaml
```

Be careful about namespaces.

