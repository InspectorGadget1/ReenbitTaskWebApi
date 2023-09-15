using Azure.Storage.Blobs.Models;

namespace Reenbit_Task.Services
{
    public interface IAzureBlobService
    {
        Task<Azure.Response<BlobContentInfo>> UploadFileAsync(IFormFile file, string userEmail);
    }
}
