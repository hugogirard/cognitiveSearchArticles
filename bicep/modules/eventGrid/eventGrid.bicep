param location string
param suffix string

resource event 'Microsoft.EventGrid/topics@2020-06-01' = {
  name: 'evnt-new-article-${suffix}'
  location: location
}