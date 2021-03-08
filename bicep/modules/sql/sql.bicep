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

output sqlServerId string = server.id
output sqlCnxString string = 'Server=tcp:${reference(server.name).fullyQualifiedDomainName},1433;Initial Catalog=${dbname};Persist Security Info=False;User ID=${adminUsername};Password=${adminPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'