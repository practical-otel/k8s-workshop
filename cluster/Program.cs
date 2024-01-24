using System.Collections.Generic;
using System.IO;
using Pulumi;
using Pulumi.AzureNative.Network;

return await Deployment.RunAsync(() =>
{
    var file = File.ReadAllLines("clusters.txt");

    var config = new Config();
    var dnsResourceGroup = config.Require("dnsResourceGroup");
    var dnsZoneName = config.Require("dnsZoneName");

    var dnsZone = GetZone.Invoke(new GetZoneInvokeArgs{
        ResourceGroupName = dnsResourceGroup,
        ZoneName = dnsZoneName
    });

    var clusters = new Dictionary<string, object?>();

    foreach (var name in file)
    {
        var cluster = new AKSCluster($"{name}-aks-otel-demo", new AKSClusterArgs{
            DnsZoneId = dnsZone.Apply(d => d.Id)
        });

        clusters.Add(name, new Dictionary<string, object?>{
            ["clusterName"] = cluster.ClusterName,
            ["clusterResourceGroup"] = cluster.ClusterResourceGroup
        });
    }
    
    return clusters;
});