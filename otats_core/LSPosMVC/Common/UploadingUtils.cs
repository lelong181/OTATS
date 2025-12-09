using System;
using System.IO;
using System.Web;
using System.Web.Caching;

namespace LSPosMVC.Common
{
    public static class UploadingUtils
    {
        const string RemoveTaskKeyPrefix = "DXRemoveTask_";

        public static void RemoveFileWithDelay(string key, string fullPath, int delay)
        {
            RemoveFileWithDelayInternal(key, fullPath, delay, FileSystemRemoveAction);
        }
        static void FileSystemRemoveAction(string key, object value, CacheItemRemovedReason reason)
        {
            string fileFullPath = value.ToString();
            if (File.Exists(fileFullPath))
                File.Delete(fileFullPath);
        }
        static void RemoveFileWithDelayInternal(string fileKey, object fileData, int delay, CacheItemRemovedCallback removeAction)
        {
            string key = RemoveTaskKeyPrefix + fileKey;
            if (HttpRuntime.Cache[key] == null)
            {
                DateTime absoluteExpiration = DateTime.UtcNow.Add(new TimeSpan(0, delay, 0));
                HttpRuntime.Cache.Insert(key, fileData, null, absoluteExpiration,
                    Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, removeAction);
            }
        }

        class AzureFileInfo
        {
            public string FileKeyName { get; set; }
            public string StorageAccountName { get; set; }
            public string AccessKey { get; set; }
            public string ContainerName { get; set; }

            public AzureFileInfo(string fileKeyName, string storageAccountName, string accessKey, string containerName)
            {
                FileKeyName = fileKeyName;
                StorageAccountName = storageAccountName;
                AccessKey = accessKey;
                ContainerName = containerName;
            }
        }

        class AmazonFileInfo
        {
            public string FileKeyName { get; set; }
            public string AccessKeyID { get; set; }
            public string SecretAccessKey { get; set; }
            public string BucketName { get; set; }
            public string Region { get; set; }

            public AmazonFileInfo(string fileKeyName, string accessKeyID, string secretAccessKey, string bucketName, string region)
            {
                FileKeyName = fileKeyName;
                AccessKeyID = accessKeyID;
                SecretAccessKey = secretAccessKey;
                BucketName = bucketName;
                Region = region;
            }
        }
        class DropboxFileInfo
        {
            public string FileKeyName { get; set; }
            public string AccessTokenValue { get; set; }

            public DropboxFileInfo(string fileKeyName, string accessTokenValue)
            {
                FileKeyName = fileKeyName;
                AccessTokenValue = accessTokenValue;
            }
        }
    }
}