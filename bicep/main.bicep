param location string
param vnetAddressSpace string
param frontendSubnetAddressPrefix string
param searchSubnetAddressPrefix string
param searchApiSubnetAddressPrefix string
param indexerSubnetAddressPrefix string
param storageSubnetAddressPrefix string
param sqlSubnetAddressPrefix string
param lockDownEnv bool = false

param adminUsername string {
  secure: true
}
param adminPassword string {
  secure: true
}

var suffix = uniqueString(resourceGroup().id)

module vnet './modules/vnet/network.bicep' = if (lockDownEnv) {
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

module eventGrid './modules/eventGrid/eventGrid.bicep' = {
  name: 'eventGrid'
  params: {
    suffix: suffix
    location: location    
  }
}

module webapp './modules/webapp/webapp.bicep' = {
  name: 'webapp'
  params: {
    location: location
    frontEndsubnetId: vnet.outputs.webAppsubnetId
    suffix: suffix
    lockDownEnv: lockDownEnv
  }
}

module function './modules/function/function.bicep' = {
  name: 'function'
  params: {
    appServiceId: webapp.outputs.appServiceId
    location: location
    suffix: suffix    
  }
}

module str './modules/storage/storage.bicep' = {
  name: 'str'
  params: {
    location: location
    suffix: suffix
  }
}

module search './modules/search/search.bicep' = {
  name: 'search'
  params: {
    suffix: suffix
    location: location
    lockDownEnv: lockDownEnv
  }
}


module dns './modules/dns/privatedns.bicep' = if (lockDownEnv) {
  name: 'dns'
  dependsOn: [
    webapp
    sql
    search
  ]
  params: {
    location: location
    vnetId: vnet.outputs.vnetId
    webApiId: webapp.outputs.apiId
    webApiSubnetId: vnet.outputs.apiSubnetId
    sqlSubnetId: vnet.outputs.sqlSubnetId
    sqlId: sql.outputs.sqlServerId
    searchSubnetId: vnet.outputs.searchSubnetId
    searchId: search.outputs.searchApiId
    searchDns: search.outputs.searchDNS
  }
}

module sql './modules/sql/sql.bicep' = {
  name: 'sql'
  params: {
    location: location
    suffix: suffix
    adminPassword: adminPassword
    adminUsername: adminUsername
  }
}


