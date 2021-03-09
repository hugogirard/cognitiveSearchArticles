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
output searchDNS string = 'https://${search.name}.search.windows.net'
output searchApiKey string = listKeys(search.id,'2020-08-01').keys[0].value