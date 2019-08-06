using System.IO;

namespace Homely.Storage.Blobs
{
    public class BlobStream : BlobBase
    {
        public Stream Stream { get; set; }        
    }
}