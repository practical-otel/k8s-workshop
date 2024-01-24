using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Azure.Core;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.Compute;
using Pulumi.AzureNative.ContainerService.V20230502Preview;
using Pulumi.AzureNative.ContainerService.V20230502Preview.Inputs;
using Pulumi.AzureNative.ContainerService.V20230502Preview.Outputs;
using Pulumi.AzureNative.Network;
using Pulumi.AzureNative.Resources;
using Pulumi.Tls;
using K8s = Pulumi.Kubernetes;
using ResourceIdentityType = Pulumi.AzureNative.ContainerService.V20230502Preview.ResourceIdentityType;

public class AKSCluster : ComponentResource
{
    private const string DnsZoneContributorRoleDefinitionId = "/providers/Microsoft.Authorization/roleDefinitions/befefa01-2a29-4197-83a8-272ff33ce314";

    public AKSCluster(string name, AKSClusterArgs args, ComponentResourceOptions? options = null)
        : base("aks-otel-demo:aks:cluster", name, args, options)
    {
        
        var config = new Pulumi.Config();
        var location = config.Get("location") ?? "uksouth";

        // Create an Azure Resource Group
        var resourceGroup = new ResourceGroup(name, new ResourceGroupArgs
        {
            Location = location
        });
        // Generate an SSH key
        var sshKey = new PrivateKey($"{name}-ssh-key", new PrivateKeyArgs
        {
            Algorithm = "RSA",
            RsaBits = 4096
        });

        var cluster = new ManagedCluster(name, new ManagedClusterArgs
        {
            ResourceGroupName = resourceGroup.Name,
            NodeResourceGroup = resourceGroup.Name.Apply(rg => $"{name}-{rg}-nodes"),
            Location = resourceGroup.Location,
            Identity = new ManagedClusterIdentityArgs
            {
                Type = ResourceIdentityType.SystemAssigned
            },
            DnsPrefix = name,
            EnableRBAC = true,
            KubernetesVersion = "1.28",
            LinuxProfile = new ContainerServiceLinuxProfileArgs
            {
                AdminUsername = "testuser",
                Ssh = new ContainerServiceSshConfigurationArgs
                {
                    PublicKeys =
                    {
                        new ContainerServiceSshPublicKeyArgs
                        {
                            KeyData = sshKey.PublicKeyOpenssh,
                        }
                    }
                }
            },
            IngressProfile = new ManagedClusterIngressProfileArgs
            {
                WebAppRouting = new ManagedClusterIngressProfileWebAppRoutingArgs
                {
                    Enabled = true,
                    DnsZoneResourceId = args.DnsZoneId
                }
            },
            AgentPoolProfiles = new[]
            {
                new ManagedClusterAgentPoolProfileArgs
                {
                    Name = "basepool",
                    Count = 1,
                    MaxPods = 110,
                    Mode = AgentPoolMode.System,
                    OsType = OSType.Linux,
                    Type = AgentPoolType.VirtualMachineScaleSets,
                    VmSize = VirtualMachineSizeTypes.Standard_A2_v2.ToString(),
                }
            }
        });

        var agentPool = new AgentPool($"{name}agents", new AgentPoolArgs {
            AgentPoolName = "userpool",
            ResourceGroupName = resourceGroup.Name,
            Count = 1,
            MaxPods = 110,
            Mode = AgentPoolMode.User,
            OsType = OSType.Linux,
            Type = AgentPoolType.VirtualMachineScaleSets,
            VmSize = VirtualMachineSizeTypes.Standard_DS2_v2.ToString(),
            ResourceName = cluster.Name,
        }, new CustomResourceOptions { 
            DeletedWith = cluster,
            ReplaceOnChanges = { "vmSize" },
            DeleteBeforeReplace = true });

        var roleAssignment = new RoleAssignment($"{name}-cluster-dns-contributor", new()
        {
            PrincipalId = cluster.IngressProfile.Apply(ip => ip?.WebAppRouting!.Identity.ObjectId!),
            PrincipalType = PrincipalType.ServicePrincipal,
            RoleDefinitionId = DnsZoneContributorRoleDefinitionId,
            Scope = args.DnsZoneId
        });

        this.ClusterName = cluster.Name;
        this.ClusterResourceGroup = resourceGroup.Name;
    }

    [Output("clusterName")]
    public Output<string> ClusterName { get; set; }

    [Output("clusterResourceGroup")]
    public Output<string> ClusterResourceGroup { get; set; }

}

public class AKSClusterArgs : Pulumi.ResourceArgs
{
    public Input<string> DnsZoneId { get; set; } = null!;
}
