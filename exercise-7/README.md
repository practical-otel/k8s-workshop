
Add Kubernetes presets and Kubelet metrics presets from https://opentelemetry.io/docs/kubernetes/helm/collector/ to the Collector:

```shell
helm upgrade my-opentelemetry-collector open-telemetry/opentelemetry-collector --values ./values.yaml
```
