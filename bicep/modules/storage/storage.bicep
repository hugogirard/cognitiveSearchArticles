param suffix string
param location string

var strName = 'str${suffix}'

resource str 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: strName
  location: location
  sku: {
    name: 'Standard_LRS'
    tier: 'Standard'
  }
  kind: 'StorageV2'
}