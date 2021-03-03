param location string
param vnetAddressSpace string
param frontendSubnetAddressPrefix string
param searchSubnetAddressPrefix string
param searchApiSubnetAddressPrefix string
param indexerSubnetAddressPrefix string
param storageSubnetAddressPrefix string
param sqlSubnetAddressPrefix string
param adminUsername string {
  secure: true
}
param adminPassword string {
  secure: true
}

var suffix = uniqueString(resourceGroup().id)

module vnet './modules/vnet/network.bicep' = {
  name: 'vnet'
  params: {
    location: location
    vnetAddressSpace: vnetAddressSpace
    frontendSubnetAddressPrefix: frontendSubnetAddressPrefix
    searchSubnetAddressPrefix: searchSubnetAddressPrefix
    searchApiSubnetAddressPrefix: searchApiSubnetAddressPrefix
    indexerSubnetAddressPrefix: indexerSubnetAddressPrefix
    storageSubnetAddressPrefix: storageSubnetAddressPrefix
    sqlSubnetAddressPrefix: sqlSubnetAddressPrefix
  }
}

// module webapp './modules/webapp/webapp.bicep' = {
//   name: 'webapp'
//   params: {
//     location: location
//     frontEndsubnetId: vnet.outputs.webAppsubnetId
//     suffix: suffix
//   }
// }

// module dns './modules/dns/privatedns.bicep' = {
//   name: 'dns'
//   params: {
//     location: location
//     vnetId: vnet.outputs.vnetId
//     webApiId: webapp.outputs.apiId
//     webApiSubnetId: vnet.outputs.apiSubnetId
//   }
// }

module sql './modules/sql/sql.bicep' = {
  name: 'sql'
  params: {
    location: location
    suffix: suffix
    adminPassword: adminPassword
    adminUsername: adminUsername
  }
}

module search './modules/search/search.bicep' = {
  name: 'search'
  params: {
    suffix: suffix
    location: location
  }
}



