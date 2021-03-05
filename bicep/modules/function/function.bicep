param location string
param suffix string
param appServiceId string


resource workspace 'Microsoft.OperationalInsights/workspaces@2020-08-01' = {
  name: 'wrk-${suffix}'
  location: location
}

resource appInsight 'microsoft.insights/components@2020-02-02-preview' = {
  name: 'appins-${suffix}'
  kind: 'web'
  location: location
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: workspace.id
  }  
}

resource str 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: 'strf${suffix}'
  kind: 'StorageV2'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
}

resource function 'Microsoft.Web/sites@2018-11-01' = {
  name: 'func-${suffix}'
  location: location
  kind: 'functionapp'
  properties: {
    siteConfig: {
      appSettings: [
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~3'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsight.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsight.properties.ConnectionString
        }
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=strf${suffix};AccountKey=${listKeys(str.id,'2019-06-01').keys[0].value};EndpointSuffix=core.windows.net'
        }
      ]
    }
    serverFarmId: appServiceId
    clientAffinityEnabled: false
  }
}