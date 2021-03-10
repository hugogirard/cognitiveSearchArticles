﻿using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServer.Pages.Articles.Services
{
    public class AzureStorageService : IStorageService
    {
        private readonly BlobContainerClient _container;
        private readonly ILogger<AzureStorageService> _logger;

        public AzureStorageService(IConfiguration configuration, ILogger<AzureStorageService> logger)
        {
            _container = new BlobContainerClient(configuration["Storage:CnxString"], configuration["Storage:Container"]);
            _logger = logger;
        }

        public async Task<string> UploadAsync(string id, Stream file)
        {
            try
            {
                BlobClient blob = _container.GetBlobClient(id);

                await blob.UploadAsync(file,false, CancellationToken.None);

                return blob.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return string.Empty;
        }
    }
}
