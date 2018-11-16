using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LearnPhotoWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LearnPhotoWebApp.Pages
{
    public class AboutModel : PageModel
    {
        private static CloudBlobClient _blobClient;
        private static CloudBlobContainer _blobContainer;

        public List<PhotosModel> GalleryImages { get; set; }

        public async Task OnGetAsync()
        {
            GalleryImages = new List<PhotosModel>();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Startup.ConnectionString);
            _blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = _blobClient.GetContainerReference("blobphotocontainer");
            await _blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            BlobContinuationToken blobContinuationToken = null;
            do
            {
                var results = await _blobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
                blobContinuationToken = results.ContinuationToken;
                foreach (IListBlobItem item in results.Results)
                {
                    GalleryImages.Add(new PhotosModel { ImageUrl = item.Uri });
                    Console.WriteLine(item.Uri);
                }
            } while (blobContinuationToken != null); 
            ViewData["SuccessMessage"] = TempData["SuccessMessage"];

        }
    }
}
