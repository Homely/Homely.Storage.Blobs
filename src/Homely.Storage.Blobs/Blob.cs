namespace Homely.Storage.Blobs
{
    public class Blob<T> : BlobBase
    {
        public T Data { get; set; }
    }
}