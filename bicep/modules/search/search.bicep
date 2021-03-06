param suffix string
param location string
param lockDownEnv bool

var publicNetworkAccess = lockDownEnv ? 'disabled' : 'enabled'

resource search 'Microsoft.Search/searchServices@2020-08-01' = {
  name: 'searchdemo-${suffix}'
  location: location
  sku: {
    name: 'basic'
  }
  properties: {
    publicNetworkAccess: publicNetworkAccess
  }
}

output searchApiId string = search.id
output searchDNS string = '${search.name}.search.windows.net'