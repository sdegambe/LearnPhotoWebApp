
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace UploadImageFunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            Stream name = req.Body;
            string aa = req.Query["name"];

            CloudStorageAccount storage = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=learnphotoblobstorage;AccountKey=yiv8+toVWd0IOuLcmMaKqjmA9Ky/CEsQyLK0gAtUt/OMJd6Er+C8QpKJqakp4CZD/YuQc0vDqSp+v/6AwSHs5A==;EndpointSuffix=core.windows.net");
            CloudBlobClient blobClient = storage.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("blobphotocontainer");

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(aa);
            await blockBlob.UploadFromStreamAsync(name);

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
