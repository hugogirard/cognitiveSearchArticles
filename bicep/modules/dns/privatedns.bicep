param vnetId string
param location string
param webApiId string
param sqlId string
param webApiSubnetId string
param sqlSubnetId string

param searchSubnetId string
param searchId string
param searchDns string

var zones = [
  {
    endpointName: 'searchApiLink'
    zoneName: 'privatelink.azurewebsites.net'
    privateLinkName: 'searchApiLink${uniqueString(resourceGroup().name)}'
    subnetId: webApiSubnetId
    refId: webApiId
    groupIds: 'sites'
    configName: 'apiConfig'
  }
  {
    endpointName: 'sqlLink'
    zoneName: 'privatelink${environment().suffixes.sqlServerHostname}'
    privateLinkName: 'sqlLink${uniqueString(resourceGroup().name)}'
    subnetId: sqlSubnetId
    refId: sqlId
    groupIds: 'sqlServer'
    configName: 'sqlConfig'
  }
  {
    endpointName: 'searchLink'
    zoneName: searchDns
    privateLinkName: 'searchLink${uniqueString(resourceGroup().name)}'
    subnetId: searchSubnetId
    refId: searchId
    groupIds: 'searchService'
    configName: 'searchConfig'    
  }
]


resource privateEndpoint 'Microsoft.Network/privateEndpoints@2020-06-01' = [for zone in zones: {
  name: zone.endpointName
  location: location
  properties: {
    subnet: {
      id: zone.subnetId
    }
    privateLinkServiceConnections: [
      {
        name: zone.privateLinkName
        properties: {
          privateLinkServiceId: zone.refId
          groupIds: [
            zone.groupIds
          ]
        }
      }
    ]
  }  
}]


resource privateDNSZone 'Microsoft.Network/privateDnsZones@2020-06-01' = [for zone in zones: {
  name: zone.zoneName
  location: 'global'
}]

resource virtualNetworkLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2020-06-01' = [for zone in zones: {
  name: '${zone.zoneName}/${zone.zoneName}-link'
  dependsOn: [
    privateDNSZone
  ]
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: vnetId
    }
  }
}]

resource privateDNSZoneGroupWebApp 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2020-06-01' = [for i in range(0,length(zones)): {
  name: '${zones[i].endpointName}/default'
  properties: {
    privateDnsZoneConfigs: [
      {
        name: zones[i].configName
        properties: {
          privateDnsZoneId: privateDNSZone[i].id
        }
      }
    ]
  }
}]