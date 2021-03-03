param vnetId string
param location string
param webApiId string
param webApiSubnetId string

var privateDNSZoneName = 'privatelink.azurewebsites.net'
var privateEndpointName = 'searchApiLink'
var privateLinkConnectionName = 'searchApiLink${uniqueString(resourceGroup().name)}'

resource privateEndpoint 'Microsoft.Network/privateEndpoints@2020-06-01' = {
  name: privateEndpointName
  location: location
  properties: {
    subnet: {
      id: webApiSubnetId
    }
    privateLinkServiceConnections: [
      {
        name: privateLinkConnectionName
        properties: {
          privateLinkServiceId: webApiId
          groupIds: [
            'sites'
          ]
        }
      }
    ]
  }
}

resource privateDNSZone 'Microsoft.Network/privateDnsZones@2020-06-01' = {
  name: privateDNSZoneName
  location: 'global'
}

resource virtualNetworkLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2020-06-01' = {
  name: '${privateDNSZone.name}/${privateDNSZone.name}-link'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: vnetId
    }
  }
}

resource privateDNSZoneGroup 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2020-06-01' = {
  name: '${privateEndpoint.name}/default'
  properties: {
    privateDnsZoneConfigs: [
      {
        name: 'privatelink-azurewebsites-net'
        properties: {
          privateDnsZoneId: privateDNSZone.id
        }
      }
    ]
  }
}