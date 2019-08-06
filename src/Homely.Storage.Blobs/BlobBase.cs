using Microsoft.Azure.Storage.Blob;
using System.Collections.Generic;

namespace Homely.Storage.Blobs
{
    public abstract class BlobBase
    {
        public BlobProperties Properties { get; set; }
        public IDictionary<string, string> Metadata { get; set; }
    }
}
