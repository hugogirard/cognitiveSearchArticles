param vnetId string
param location string
param webApiId string
param sqlId string
param webApiSubnetId string
param sqlSubnetId string

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
]

var dnsZone = {
  sql: {
    endpointName: 'sqlLink'
    zoneName: 'privatelink${environment().suffixes.sqlServerHostname}'
    privateLinkName: 'sqlLink${uniqueString(resourceGroup().name)}'
  }
}

var privateDNSZoneName = 'privatelink.azurewebsites.net'
var privateEndpointName = 'searchApiLink'
var privateLinkConnectionName = 'searchApiLink${uniqueString(resourceGroup().name)}'


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


// resource privateDNSZoneSql 'Microsoft.Network/privateDnsZones@2020-06-01' = {
//   name: dnsZone.sql.zoneName
//   location: 'global'
// }

// resource privateDNSZoneWebApp 'Microsoft.Network/privateDnsZones@2020-06-01' = {
//   name: privateDNSZoneName
//   location: 'global'
// }

// resource privateEndpointWebApi 'Microsoft.Network/privateEndpoints@2020-06-01' = {
//   name: privateEndpointName
//   location: location
//   properties: {
//     subnet: {
//       id: webApiSubnetId
//     }
//     privateLinkServiceConnections: [
//       {
//         name: privateLinkConnectionName
//         properties: {
//           privateLinkServiceId: webApiId
//           groupIds: [
//             'sites'
//           ]
//         }
//       }
//     ]
//   }
// }

// resource privateEndpointSql 'Microsoft.Network/privateEndpoints@2020-06-01' = {
//   name: dnsZone.sql.endpointName
//   location: location
//   properties: {
//     subnet: {
//       id: sqlSubnetId
//     }
//     privateLinkServiceConnections: [
//       {
//         name: dnsZone.sql.privateLinkName
//         properties: {
//           privateLinkServiceId: sqlId
//           groupIds: [
//             'sqlServer'
//           ]
//         }
//       }
//     ]
//   }
// }



// resource virtualNetworkLinkWebApp 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2020-06-01' = {
//   name: '${privateDNSZoneWebApp.name}/${privateDNSZoneWebApp.name}-link'
//   location: 'global'
//   properties: {
//     registrationEnabled: false
//     virtualNetwork: {
//       id: vnetId
//     }
//   }
// }

// resource virtualNetworkLinkSql 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2020-06-01' = {
//   name: '${privateDNSZoneSql.name}/${privateDNSZoneSql.name}-link'
//   location: 'global'
//   properties: {
//     registrationEnabled: false
//     virtualNetwork: {
//       id: vnetId
//     }
//   }
// }

// resource privateDNSZoneGroupWebApp 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2020-06-01' = {
//   name: '${privateEndpointWebApi.name}/default'
//   properties: {
//     privateDnsZoneConfigs: [
//       {
//         name: 'privatelink-azurewebsites-net'
//         properties: {
//           privateDnsZoneId: privateDNSZoneWebApp.id
//         }
//       }
//     ]
//   }
// }

// resource privateDNSZoneGroupSql'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2020-06-01' = {
//   name: '${privateEndpointSql.name}/default'
//   properties: {
//     privateDnsZoneConfigs: [
//       {
//         name: 'sqlConfig'
//         properties: {
//           privateDnsZoneId: privateDNSZoneSql.id
//         }
//       }
//     ]
//   }
// }