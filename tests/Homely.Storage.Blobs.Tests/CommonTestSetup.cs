using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;

namespace Homely.Storage.Blobs.Tests
{
    public abstract class CommonTestSetup
    {
        protected const string TestImageName = "2018-tesla-model-x-p100d.jpg";

        protected SomeFakeUser TestUser => new SomeFakeUser
        {
            Name = "Elon Musk",
            Age = 40
        };

        protected async Task<(AzureBlob Blob, string ImageBlobId, string TestUserBlobId)> SetupAzureBlobAsync(bool setupInitialBlobData = true)
        {
            var logger = new NullLogger<AzureBlob>();
            var blob = new AzureBlob("UseDevelopmentStorage=true", "test-container", logger);

            var imageBlobId = Guid.NewGuid().ToString();
            var testUserBlobId = Guid.NewGuid().ToString();

            if (setupInitialBlobData)
            {
                var cancellationToken = new CancellationToken();

                // Do we have the image already?
                if (await blob.GetAsync<string>(imageBlobId, cancellationToken) == null)
                {
                    var image = await File.ReadAllBytesAsync(TestImageName);
                    await blob.AddAsync(image, imageBlobId);
                }

                if (await blob.GetAsync<SomeFakeUser>(testUserBlobId, cancellationToken) == null)
                {
                    await blob.AddAsync(TestUser, testUserBlobId);
                }
            }

            return (blob, imageBlobId, testUserBlobId);
        }

        protected async Task<AzureBlob> GetAzureBlobAsync(bool setupInitialBlobData = true)
        {
            var (azureBlob, _, _) = await SetupAzureBlobAsync(setupInitialBlobData);

            return azureBlob;
        }
    }
}
