param location string
param suffix string
param frontEndsubnetId string

var appServiceName = concat('appsrvsearch',suffix)

var webapiName = concat('searchapi-',suffix)
var webAppWeb = concat('knowledgeweb-',suffix)
var websiteDNSName = '.azurewebsites.net'

resource appservicePlan 'Microsoft.Web/serverfarms@2019-08-01' = {
  name: appServiceName
  location: location
  sku: {
    name: 'P1v2'
    tier: 'PremiumV2'
    size: 'P1v2'
    family: 'Pv2'
    capacity: 1
  }
}

resource api 'Microsoft.Web/sites@2020-06-01' = {
  name: webapiName
  location: location
  kind: 'app'
  dependsOn: [
    appservicePlan
  ]
  properties: {
    serverFarmId: appservicePlan.id
    hostNameSslStates: [
      {
        name: '${webapiName}${websiteDNSName}'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: '${webapiName}.scm${websiteDNSName}'
        sslState: 'Disabled'
        hostType: 'Repository'        
      }
    ]
  }
}

resource hostnameBinding 'Microsoft.Web/sites/hostNameBindings@2020-06-01' = {
  name: '${api.name}/${api.name}${websiteDNSName}'
  properties: {
    siteName: api.name
    hostNameType: 'Verified'
  }
}


resource web 'Microsoft.Web/sites@2019-08-01' = {
  name: webAppWeb
  location: location
  properties: {
    serverFarmId: appservicePlan.id
  }
}

resource frontEndNetworkConfig 'Microsoft.Web/sites/networkConfig@2020-06-01' = {
  name: '${web.name}/VirtualNetwork'
  properties: {
    subnetResourceId: frontEndsubnetId
  }
}

output apiName string = api.name
output frontendName string = web.name
output apiId string = api.id