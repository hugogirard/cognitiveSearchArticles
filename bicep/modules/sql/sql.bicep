param adminUsername string {
  secure: true
}
param adminPassword string {
  secure: true
}
param location string
param suffix string

var sqlServerName = concat('sqlserverArticle-',suffix)
var dbname = 'articleDB'

resource server 'Microsoft.Sql/servers@2019-06-01-preview'  = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: adminUsername
    administratorLoginPassword: adminPassword
  }  
}

resource database 'Microsoft.Sql/servers/databases@2019-06-01-preview' = {
  name: concat(server.name,'/',dbname)
  dependsOn: [
    server
  ]
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
  }
}