# 

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

Open Prometheus at [http://localhost:9090] (see exercise 2 on how to enable port forwarding)

Enter `otelcol_process_runtime_total_sys_memory_bytes` in the box, and click execute.