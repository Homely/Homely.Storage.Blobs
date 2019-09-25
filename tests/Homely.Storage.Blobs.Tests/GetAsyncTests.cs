using Homely.Testing;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Homely.Storage.Blobs.Tests
{
    public class GetAsyncTests : CommonTestSetup
    {
        public static TheoryData<IList<string>> Data =>
            new TheoryData<IList<string>>
            {
                { new List<string>() }, // Empty list, which will then just get ignored.
                { new List<string> { "LastModified" }  }, // Case sensitive.
                { new List<string> { "lastmodified" }  }, // Case insensitive.
                { new List<string> { "lastmodified", "contentmd5" }  } // Multiple.
            };

        [Fact]
        public async Task GivenAnExistingStream_GetAsync_ReturnsTrue()
        {
            // Arrange.
            var azureBlob = await GetAzureBlobAsync();
            var fileInfo = new FileInfo(TestImageName);

            using (var stream = new MemoryStream())
            {
                // Act.
                var result = await azureBlob.GetAsync(TestImageName, stream);

                // Assert.
                result.ShouldBeTrue();
                stream.ShouldNotBeNull();
                stream.Length.ShouldBe(fileInfo.Length);
            }
        }

        [Fact]
        public async Task GivenAMissingStream_GetAsync_ReturnsFalse()
        {
            // Arrange.
            var azureBlob = await GetAzureBlobAsync();

            using (var stream = new MemoryStream())
            {
                // Act.
                var result = await azureBlob.GetAsync(Guid.NewGuid().ToString(), stream);

                // Assert.
                result.ShouldBeFalse();
                stream.ShouldNotBeNull();
                stream.Length.ShouldBe(0);
            }
        }

        [Fact]
        public async Task GivenAnExistingClassInstance_GetAsyncGeneric_ReturnsTheInstance()
        {
            // Arrange.
            var azureBlob = await GetAzureBlobAsync();

            // Act.
            var user = await azureBlob.GetAsync<SomeFakeUser>(TestClassInstanceName, default);

            // Assert.
            user.ShouldNotBeNull();
            user.ShouldLookLike(TestUser);
        }

        [Fact]
        public async Task GivenAMissingClassInstance_GetAsyncGeneric_ReturnsNull()
        {
            // Arrange.
            var azureBlob = await GetAzureBlobAsync();

            // Act.
            var user = await azureBlob.GetAsync<SomeFakeUser>(Guid.NewGuid().ToString(), default);

            // Assert.
            user.ShouldBeNull();
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task GivenAnExistingClassInstanceAndSomePropertiesOrMetaData_GetAsyncGeneric_ReturnsTheInstance(IList<string> existingPropertiesOrMetaData)
        {
            // Arrange.
            var azureBlob = await GetAzureBlobAsync();

            // Act.
            var result = await azureBlob.GetAsync<SomeFakeUser>(TestClassInstanceName, existingPropertiesOrMetaData);

            // Assert.
            result.ShouldNotBeNull();
            result.Data.ShouldLookLike(TestUser);
            result.MetaData.Count.ShouldBe(existingPropertiesOrMetaData.Count);
        }

        [Fact]
        public async Task GivenAnExistingClassInstanceAndSomeAMissingPropertiesOrMetaData_GetAsyncGeneric_ThrowsAnException()
        {
            // Arrange.
            const string missingKey = "abcd";
            var azureBlob = await GetAzureBlobAsync();
            var missingPropertiesOrMetaData = new List<string> { missingKey };

            // Act.
            var exception = await Should.ThrowAsync<Exception>(() => azureBlob.GetAsync<SomeFakeUser>(TestClassInstanceName, missingPropertiesOrMetaData));

            // Assert.
            exception.ShouldNotBeNull();
            exception.Message.ShouldBe($"BlobProperties and MetaData doesn't contain the expected key: [{missingKey}]. At least one of them should contain that key.");
        }
    }
}
