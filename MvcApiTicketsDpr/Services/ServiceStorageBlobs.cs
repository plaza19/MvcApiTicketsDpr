using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcApiTicketsDpr.Services
{
    public class ServiceStorageBlobs
    {
        private BlobServiceClient client;
        public ServiceStorageBlobs(BlobServiceClient client)
        {
            this.client = client;
        }

        public async Task UploadBlobAsync(String containerName, String blobName, Stream stream)
        {
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);
            var data = await containerClient.UploadBlobAsync(blobName, stream);

        }

    }
}
