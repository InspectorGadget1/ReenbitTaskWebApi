using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace Reenbit_Task.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobContainerClient containerClient;
        public AzureBlobService(IConfiguration config)
        {
            var blobClient = new BlobServiceClient(config["AzureConnectionString"]);
            this.containerClient = blobClient.GetBlobContainerClient("docs");
        }

        public async Task<Azure.Response<BlobContentInfo>> UploadFileAsync(IFormFile file, string userEmail)
        {
            string fileName = file.FileName;
            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            memoryStream.Position = 0;
            var client = await containerClient.UploadBlobAsync(fileName, memoryStream, default);

            // Add user email as metadata
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            Dictionary<string, string> metadata = new Dictionary<string, string>
            {
                { "UserEmail", userEmail }
            };
            await blobClient.SetMetadataAsync(metadata);
            return client;
        }
    }
}
