using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Homely.Storage.Blobs
{
    public interface IBlob
    {
        /// <summary>
        /// Streams an item from Azure Blob storage.
        /// </summary>
        /// <remarks>No custom json-deserialization occurs here. Just raw retrevial via stream.</remarks>
        /// <param name="blobId">string: The identifier/name of this content to be stored on Azure.</param>
        /// <param name="target">Stream: stream to contain the blob data.</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for a task to complete.</param>
        /// <returns>A System.Threading.Tasks.Task object that represents the asynchronous operation.</returns>
        Task<bool> GetAsync(string blobId, 
                            Stream target,
                            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves an item from Azure Blob storage.
        /// </summary>
        /// <remarks>The item must have been json-serialized otherwise it cannot be deserialized properly.</remarks>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="blobId">string: The identifier/name of this content to be stored on Azure.</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for a task to complete.</param>
        /// <returns>A System.Threading.Tasks.Task object that represents the asynchronous operation.</returns>
        Task<T> GetAsync<T>(string blobId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves an item from Azure Blob storage.
        /// </summary>
        /// <remarks>The item must have been json-serialized otherwise it cannot be deserialized properly.</remarks>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="blobId">string: The identifier/name of this content to be stored on Azure.</param>
        /// <param name="existingPropertiesOrMetaData">IList&lt;string&gt;: collection of keys to extract from the blob's properties and/or custom meta data.</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for a task to complete.</param>
        /// <returns>A System.Threading.Tasks.Task object that represents the asynchronous operation.</returns>
        Task<BlobData<T>> GetAsync<T>(string blobId, 
                                      IList<string> existingPropertiesOrMetaData = default, 
                                      CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an item from Azure blob storage.
        /// </summary>
        /// <param name="blobId">Unique blob identifier to delete.</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for a task to complete.</param>
        /// <returns>A System.Threading.Tasks.Task object that represents the asynchronous operation.</returns>
        Task DeleteAsync(string blobId, CancellationToken cancellationToken = default);

        /// <summary>
        /// This adds an object to an Azure Blob Storage by serializing the object to json
        /// and then copying up to the destination blob.
        /// </summary>
        /// <remarks>Default encoding is set as UTF8.<br/>The item to store must be serializable.</remarks>
        /// <param name="item">object: the item to store in the azure blob.</param>
        /// <param name="blobId">Optional. The identifier/name of this content to be stored on Azure. If not supplied, then a new <code>Guid</code> will be used.</param>
        /// <param name="cacheControlType">Optional. Allows the ability to set the cache-control of the blob content</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for a task to complete.</param>
        /// <returns>string: blob name.</returns>
        Task<string> AddAsync(object item, 
                              string blobId = null,
                              CacheControlType cacheControlType = CacheControlType.None,
                              CancellationToken cancellationToken = default);

        /// <summary>
        /// This adds an object to an Azure Blob Storage by serializing the object to json
        /// and then copying up to the destination blob.
        /// </summary>
        /// <remarks>The item to store must be serializable.</remarks>
        /// <param name="item">object: the item to store in the azure blob.</param>
        /// <param name="blobId">The identifier/name of this content to be stored on Azure. If <code>null</code>, then a new <code>Guid</code> will be used.</param>
        /// <param name="encoding">The encoding type to serialize the <code>item</code>.</param>
        /// <param name="cacheControlType">Optional. Allows the ability to set the cache-control of the blob content</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for a task to complete.</param>
        /// <returns>string: blob name.</returns>
        Task<string> AddAsync(object item,
                              string blobId,
                              Encoding encoding,
                              CacheControlType cacheControlType = CacheControlType.None,
                              CancellationToken cancellationToken = default);

        /// <summary>
        /// This adds an object to an Azure Blob Storage, as is.
        /// </summary>
        /// <param name="content">byte[]: source item as a byte array.</param>
        /// <param name="blobId">Optional. The identifier/name of this content to be stored on Azure. If not supplied, then a new <code>Guid</code> will be used.</param>
        /// <param name="contentType">Optional. What type of content exists in the file. If none is provided, then Azure defaults this value to <code>application/octet-stream</code>.</param>
        /// <param name="cacheControlType">Optional. Allows the ability to set the cache-control of the blob content</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for a task to complete.</param>
        /// <returns>string: blob name.</returns>
        Task<string> AddAsync(byte[] content,
                              string blobId = null,
                              string contentType = null,
                              CacheControlType cacheControlType = CacheControlType.None,
                              CancellationToken cancellationToken = default);

        /// <summary>
        /// This adds an object to an Azure Blob Storage, as is.
        /// </summary>
        /// <param name="content">stream: source item as a stream.</param>
        /// <param name="blobId">Optional. The identifier/name of this content to be stored on Azure. If not supplied, then a new <code>Guid</code> will be used.</param>
        /// <param name="contentType">Optional. What type of content exists in the file. If none is provided, then Azure defaults this value to <code>application/octet-stream</code>.</param>
        /// <param name="cacheControlType">Optional. Allows the ability to set the cache-control of the blob content</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for a task to complete.</param>
        /// <returns>string: blob name.</returns>
        Task<string> AddAsync(Stream content,
                              string blobId = null,
                              string contentType = null,
                              CacheControlType cacheControlType = CacheControlType.None,
                              CancellationToken cancellationToken = default);

        /// <summary>
        /// This adds an object to an Azure Blob Storage from a Uri.
        /// </summary>
        /// <param name="sourceUri">string: source URI to upload from.</param>
        /// <param name="blobId">Optional. The identifier/name of this content to be stored on Azure. If not supplied, then a new <code>Guid</code> will be used.</param>
        /// <param name="contentType">Optional. What type of content exists in the file. If none is provided, then Azure defaults this value to <code>application/octet-stream</code>.</param>
        /// <param name="cacheControlType">Optional. Allows the ability to set the cache-control of the blob content</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for a task to complete.</param>
        /// <returns>string: blob name.</returns>
        Task<string> AddAsync(Uri sourceUri,
                              string blobId = null,
                              string contentType = null,
                              CacheControlType cacheControlType = CacheControlType.None,
                              CancellationToken cancellationToken = default);

        /// <summary>
        /// This adds an collection objects to an azure blob storage by serializing the objects to json
        /// and then copying up to the destination blob.
        /// </summary>
        /// <remarks>The items to store must be serializable.</remarks> 
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="items">object[]: the items to store in the azure blob.</param>
        /// <param name="batchSize">How many items to store in a single batch.</param>
        /// <param name="cacheControlType">Optional. Allows the ability to set the cache-control of the blob content</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for a task to complete.</param>
        /// <returns>string: blob name.</returns>
        /// <remarks>BatchSize defaults to 25 for no particular reason. Just felt ok...</remarks>
        Task<IList<string>> AddBatchAsync<T>(ICollection<T> items,
                                             int batchSize = 25,
                                             CacheControlType cacheControlType = CacheControlType.None,
                                             CancellationToken cancellationToken = default);

        /// <summary>
        /// This adds an collection objects to an azure blob storage by serializing the objects to json
        /// and then copying up to the destination blob.
        /// </summary>
        /// <remarks>The items to store must be serializable.</remarks>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="items">object[]: the items to store in the azure blob.</param>
        /// <param name="encoding">The encoding type to serialize the <code>item</code>.</param>
        /// <param name="batchSize">How many items to store in a single batch.</param>
        /// <param name="cacheControlType">Optional. Allows the ability to set the cache-control of the blob content</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for a task to complete.</param>
        /// <returns>string: blob name.</returns>
        /// <remarks>BatchSize defaults to 25 for no particular reason. Just felt ok...</remarks>
        Task<IList<string>> AddBatchAsync<T>(ICollection<T> items,
                                             Encoding encoding,
                                             int batchSize = 25,
                                             CacheControlType cacheControlType = CacheControlType.None,
                                             CancellationToken cancellationToken = default);
    }
}
