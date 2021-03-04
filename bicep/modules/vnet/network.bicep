param location string
param vnetAddressSpace string
param frontendSubnetAddressPrefix string
param searchSubnetAddressPrefix string
param searchApiSubnetAddressPrefix string
param indexerSubnetAddressPrefix string
param storageSubnetAddressPrefix string
param sqlSubnetAddressPrefix string

resource vnet 'Microsoft.Network/virtualNetworks@2020-06-01' = {
  name: 'vnet-article-db'
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        vnetAddressSpace
      ]
    }
    subnets: [
      {
        name: 'frontEndSubnet'
        properties: {
          addressPrefix: frontendSubnetAddressPrefix
          delegations: [
            {
              name: 'webDelegation'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
              }
            }
          ]          
        }
      }
      {
        name: 'searchSubnet'
        properties: {
          addressPrefix: searchSubnetAddressPrefix
          privateEndpointNetworkPolicies: 'Disabled'
        }
      }
      {
        name: 'searchApiSubnet'
        properties: {
          addressPrefix: searchApiSubnetAddressPrefix
          privateEndpointNetworkPolicies: 'Disabled'
        }
      }
      {
        name: 'indexerSubnet'
        properties: {
          addressPrefix: indexerSubnetAddressPrefix
          delegations: [
            {
              name: 'functionDelegation'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
              }
            }
          ]
        }
      }
      {
        name: 'storageSubnet'
        properties: {
          addressPrefix:  storageSubnetAddressPrefix
          privateEndpointNetworkPolicies: 'Disabled'
        }
      }
      {
        name: 'sqlSubnet'
        properties: {
          addressPrefix: sqlSubnetAddressPrefix
          privateEndpointNetworkPolicies: 'Disabled'
        }
      }
    ]
  }
}

output webAppsubnetId string = vnet.properties.subnets[0].id
output apiSubnetId string = vnet.properties.subnets[2].id
output sqlSubnetId string = vnet.properties.subnets[5].id
output indexerSubnetId string = vnet.properties.subnets[3].id
output vnetId string = vnet.id