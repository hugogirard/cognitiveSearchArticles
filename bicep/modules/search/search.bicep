param suffix string
param location string

resource search 'Microsoft.Search/searchServices@2020-08-01' = {
  name: 'searchdemo-${suffix}'
  location: location
  sku: {
    name: 'basic'
  }
  properties: {
    publicNetworkAccess: 'disabled'
  }
}

output searchApiId string = search.id
output searchDNS string = '${search.name}.search.windows.net'