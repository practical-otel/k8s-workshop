using System.Collections.Generic;
using Pulumi;
using Pulumi.AzureNative.Network;

return await Deployment.RunAsync(() =>
{
    var config = new Config();
    var dnsResourceGroup = config.Require("dnsResourceGroup");
    var dnsZoneName = config.Require("dnsZoneName");

    var dnsZone = GetZone.Invoke(new GetZoneInvokeArgs{
        ResourceGroupName = dnsResourceGroup,
        ZoneName = dnsZoneName
    });

    var cluster = new AKSCluster("aks-otel-demo", new AKSClusterArgs{
        DnsZoneId = dnsZone.Apply(d => d.Id)
    });
    
    return new Dictionary<string, object?>
    {
        ["clusterName"] = cluster.ClusterName,
        ["clusterResourceGroup"] = cluster.ClusterResourceGroup,
    };
});