using Microsoft.Extensions.Logging.Abstractions;
using System.IO;
using System.Threading.Tasks;

namespace Homely.Storage.Blobs.Tests
{
    public abstract class CommonTestSetup
    {
        protected string TestImageName = "2018-tesla-model-x-p100d.jpg";
        protected string TestClassInstanceName = "elon-musk";
        protected SomeFakeUser TestUser => new SomeFakeUser
        {
            Name = "Elon Musk",
            Age = 40
        };

        protected AzureBlob Blob => new AzureBlob("UseDevelopmentStorage=true", "test-container", new NullLogger<AzureBlob>());

        protected async Task<AzureBlob> GetAzureBlobAsync(bool setupInitialBlobData = true, string contentType = "text/plain")
        {
            if (setupInitialBlobData)
            {
                // Do we have the image already?
                if (await Blob.GetAsync(TestImageName) == null)
                {
                    var image = await File.ReadAllBytesAsync("2018-tesla-model-x-p100d.jpg");
                    await Blob.AddAsync(image, contentType, TestImageName);
                }

                if (await Blob.GetAsync<SomeFakeUser>(TestClassInstanceName) == null)
                {
                    await Blob.AddAsync(TestUser, TestClassInstanceName, contentType, default);
                }
            }

            return Blob;
        }
    }
}
