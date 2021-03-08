param location string
param suffix string
param frontEndsubnetId string
param lockDownEnv bool
param sqlConnectionStrig string

var appServiceName = concat('appsrvsearch',suffix)

var webapiSearchName = concat('searchapi-',suffix)
var articleApiSearchName = concat('articleapi-',suffix)
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

resource apiSearch 'Microsoft.Web/sites@2020-06-01' = {
  name: webapiSearchName
  location: location
  kind: 'app'
  dependsOn: [
    appservicePlan
  ]
  properties: {
    serverFarmId: appservicePlan.id
    hostNameSslStates: [
      {
        name: '${webapiSearchName}${websiteDNSName}'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: '${webapiSearchName}.scm${websiteDNSName}'
        sslState: 'Disabled'
        hostType: 'Repository'        
      }
    ]
  }
}

resource apiArticle 'Microsoft.Web/sites@2020-06-01' = {
  name: articleApiSearchName
  location: location
  kind: 'app'
  tags: {
    'appname': 'apiArticle'
  }
  dependsOn: [
    appservicePlan
  ]
  properties: {
    siteConfig: {
      connectionStrings: [
        {
          name: 'ArticleDb'
          connectionString: sqlConnectionStrig
        }
      ]
    }
    serverFarmId: appservicePlan.id
    hostNameSslStates: [
      {
        name: '${articleApiSearchName}${websiteDNSName}'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: '${articleApiSearchName}.scm${websiteDNSName}'
        sslState: 'Disabled'
        hostType: 'Repository'        
      }
    ]
  }
}

resource hostnameBindingSearchApi 'Microsoft.Web/sites/hostNameBindings@2020-06-01' = {
  name: '${apiSearch.name}/${apiSearch.name}${websiteDNSName}'
  properties: {
    siteName: apiSearch.name
    hostNameType: 'Verified'
  }
}

resource hostnameBindingArticleApi 'Microsoft.Web/sites/hostNameBindings@2020-06-01' = {
  name: '${apiArticle.name}/${apiArticle.name}${websiteDNSName}'
  properties: {
    siteName: apiArticle.name
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

resource frontEndNetworkConfig 'Microsoft.Web/sites/networkConfig@2020-06-01' = if (lockDownEnv) {
  name: '${web.name}/VirtualNetwork'
  properties: {
    subnetResourceId: frontEndsubnetId
  }
}

output appServiceId string = appservicePlan.id
output apiName string = apiSearch.name
output frontendName string = web.name
output apiId string = apiSearch.id