using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using OBS;
using OBS.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Environments.Environment.WebServices.PIDE;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace AKDEMIC.CORE.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly CloudBlobClient _blobClient;
        private readonly CloudStorageCredentials _settings;

        private readonly string EndPoint = "https://obs.la-south-2.myhuaweicloud.com";
        private readonly string AccessKey = "1JMHFANSFPEMK07J6AIY";
        private readonly string SecretAccessKey = "hi9AwQjlRcaUT4u0ktlWyD4KOCZfoUHp82qH4Wzk";

        public CloudStorageService(IOptions<CloudStorageCredentials> settings)
        {
            _settings = settings.Value;

            /*var accountUri = new Uri($"https://{_settings.StorageName}.blob.core.windows.net");
            var options = new BlobClientOptions()
            {
                CustomerProvidedKey = new CustomerProvidedKey(_settings.AccessKey)
            };
            var serviceClient = new BlobServiceClient(accountUri, new DefaultAzureCredential(), options);*/

            //if (ConstantHelpers.GENERAL.FileStorage.STORAGE_MODE == ConstantHelpers.FileStorage.Mode.BLOB_STORAGE_MODE)
            //{
            var storageAccount = new CloudStorageAccount(
            new StorageCredentials(
                _settings.StorageName,
                _settings.AccessKey), true);

            _blobClient = storageAccount.CreateCloudBlobClient();
            //}
        }

        private async Task<string> Upload(Stream stream, string cloudStorageContainer, string fileName, string systemFolder = null, bool isUserImage = false)
        {
            if (ConstantHelpers.GENERAL.FileStorage.STORAGE_MODE == ConstantHelpers.FileStorage.Mode.HUAWEI_STORAGE_MODE
                || cloudStorageContainer == "bit4idtest")
            {
                var config = new ObsConfig();
                config.Endpoint = EndPoint;
                var client = new ObsClient(AccessKey, SecretAccessKey, config);

                try
                {
                    var createResponse = client.CreateBucket(new CreateBucketRequest
                    {
                        BucketName = cloudStorageContainer,
                        Location = "la-south-2"
                    });
                    if (createResponse.StatusCode != HttpStatusCode.OK) return null;

                    var request = new PutObjectRequest
                    {
                        BucketName = cloudStorageContainer,
                        ObjectKey = fileName,
                        InputStream = stream
                    };

                    var response = client.PutObject(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                        return $"{cloudStorageContainer}/{fileName}";

                    return null;
                }
                catch (ObsException ex)
                {
                    Console.WriteLine("ErrorCode: {0}", ex.ErrorCode);
                    Console.WriteLine("ErrorMessage: {0}", ex.ErrorMessage);
                    return null;
                }
            }

            if (ConstantHelpers.GENERAL.FileStorage.STORAGE_MODE == ConstantHelpers.FileStorage.Mode.SERVER_STORAGE_MODE || isUserImage)
            {
                var yearFolder = DateTime.Today.Year.ToString();
                var projectFolderPath = "";
                var relativePath = "";

                if (string.IsNullOrEmpty(systemFolder))
                {
                    projectFolderPath = Path.Combine(ConstantHelpers.GENERAL.FileStorage.PATH, yearFolder, cloudStorageContainer);
                    relativePath = Path.Combine(yearFolder, cloudStorageContainer);
                }
                else
                {
                    projectFolderPath = Path.Combine(ConstantHelpers.GENERAL.FileStorage.PATH, yearFolder, systemFolder, cloudStorageContainer);
                    relativePath = Path.Combine(yearFolder, systemFolder, cloudStorageContainer);
                }

                var filePath = Path.Combine(projectFolderPath, fileName);
                relativePath = Path.Combine(relativePath, fileName);

                Directory.CreateDirectory(projectFolderPath);

                using (var newstream = new FileStream(filePath, FileMode.Create))
                {
                    await stream.CopyToAsync(newstream);
                }

                return relativePath;
            }

            if (ConstantHelpers.GENERAL.FileStorage.STORAGE_MODE == ConstantHelpers.FileStorage.Mode.AMAZONS3_STORAGE_MODE)
            {
                var accessKeyS3 = ConstantHelpers.AmazonS3.AccessKey;
                var secretKeyS3 = ConstantHelpers.AmazonS3.SecretKey;
                var bucketNameS3 = ConstantHelpers.AmazonS3.BucketName;
                var credentials = new BasicAWSCredentials(accessKeyS3, secretKeyS3);

                var config = new AmazonS3Config()
                {
                    RegionEndpoint = ConstantHelpers.AmazonS3.RegionEndpoint
                };

                try
                {
                    var uploadRequest = new TransferUtilityUploadRequest()
                    {
                        InputStream = stream,
                        Key = $"{cloudStorageContainer}/{fileName}",
                        BucketName = bucketNameS3,
                        CannedACL = S3CannedACL.NoACL,
                    };

                    // initialise client
                    using var client = new AmazonS3Client(credentials, config);

                    // initialise the transfer/upload tools
                    var transferUtility = new TransferUtility(client);

                    // initiate the file upload
                    await transferUtility.UploadAsync(uploadRequest);

                    return $"{cloudStorageContainer}/{fileName}";
                }
                catch (AmazonS3Exception s3Ex)
                {
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            var container = _blobClient.GetContainerReference(cloudStorageContainer);

            if (await container.CreateIfNotExistsAsync())
            {
                await container.SetPermissionsAsync(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    }
                );
            }

            var blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromStreamAsync(stream);

            return blockBlob.Uri.ToString().Replace("%5C", "/");
        }

        public Task UploadProductBinary(object p, string rESOLUTIONS)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadFile(Stream stream, string container, string fileName, string extension, string systemFolder = null)
        {
            return await Upload(stream, container, $"{fileName}{extension}", systemFolder);
        }

        public async Task<string> UploadFile(Stream stream, string container, string fileName, string extension, string systemFolder = null, bool isUserImage = false)
        {
            return await Upload(stream, container, $"{fileName}{extension}", systemFolder, isUserImage: isUserImage);
        }

        public async Task<string> UploadFile(Stream stream, string container, string extension, string systemFolder = null)
        {
            return await Upload(stream, container, $"{Guid.NewGuid()}{extension}", systemFolder);
        }

        public async Task<string> UploadProductBinary(Stream stream, string container, string systemFolder = null)
        {
            return await Upload(stream, container, Guid.NewGuid().ToString(), systemFolder);
        }

        private async Task<bool> Delete(string fileName, string cloudStorageContainer)
        {
            if (ConstantHelpers.GENERAL.FileStorage.STORAGE_MODE == ConstantHelpers.FileStorage.Mode.HUAWEI_STORAGE_MODE)
            {
                var config = new ObsConfig();
                config.Endpoint = EndPoint;
                var client = new ObsClient(AccessKey, SecretAccessKey, config);
                try
                {
                    fileName = fileName.Split('/').Last();

                    var request = new DeleteObjectRequest
                    {
                        BucketName = cloudStorageContainer,
                        ObjectKey = fileName
                    };

                    var response = client.DeleteObject(request);
                    return response.StatusCode == HttpStatusCode.OK;
                }
                catch (ObsException ex)
                {
                    Console.WriteLine("ErrorCode: {0}", ex.ErrorCode);
                    Console.WriteLine("ErrorMessage: {0}", ex.ErrorMessage);
                    return false;
                }
            }

            if (ConstantHelpers.GENERAL.FileStorage.STORAGE_MODE == ConstantHelpers.FileStorage.Mode.AMAZONS3_STORAGE_MODE)
            {
                var accessKeyS3 = ConstantHelpers.AmazonS3.AccessKey;
                var secretKeyS3 = ConstantHelpers.AmazonS3.SecretKey;
                var bucketNameS3 = ConstantHelpers.AmazonS3.BucketName;
                var credentials = new BasicAWSCredentials(accessKeyS3, secretKeyS3);

                var config = new AmazonS3Config()
                {
                    RegionEndpoint = ConstantHelpers.AmazonS3.RegionEndpoint
                };

                // initialise client
                using var client = new AmazonS3Client(credentials, config);

                try
                {
                    var response = await client.DeleteObjectAsync(bucketNameS3, fileName);

                    return response.HttpStatusCode == HttpStatusCode.OK;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            if (cloudStorageContainer != "bit4idtest")
            {
                if (ConstantHelpers.GENERAL.FileStorage.STORAGE_MODE == ConstantHelpers.FileStorage.Mode.SERVER_STORAGE_MODE)
                {
                    var filePath = Path.Combine(ConstantHelpers.GENERAL.FileStorage.PATH, fileName);
                    if (File.Exists(filePath)) File.Delete(filePath);
                    return true;
                }
            }
            try
            {
                fileName = fileName.Split('/').Last();
                var container = _blobClient.GetContainerReference(cloudStorageContainer);
                var blockBlob = container.GetBlockBlobReference(fileName);

                return await blockBlob.DeleteIfExistsAsync();
            }
            catch (Exception)
            {

                return false;
            }
       
        }

        public async Task<bool> TryDelete(string fileName, string cloudStorageContainer)
        {
            return await Delete(fileName, cloudStorageContainer);
        }

        public async Task<bool> TryDeleteProductBinary(string fileName)
        {
            return await Delete(fileName, "binaries");
        }

        public async Task<bool> TryDeleteProductImage(string fileName)
        {
            return await Delete(fileName, "applications-images");
        }

        private async Task<Stream> Download(Stream stream, string cloudStorageContainer, string fileName, bool folderForUserImages = false)
        {
            if (fileName.Contains("bit4idtest") || ConstantHelpers.GENERAL.FileStorage.STORAGE_MODE == ConstantHelpers.FileStorage.Mode.HUAWEI_STORAGE_MODE)
            {
                var fileNameSplit = fileName.Split('/');
                
                if(fileNameSplit.Length == 4 && int.TryParse(fileNameSplit[0], out var yearFile))
                {
                    return await DownloadByServerStorageMode(stream, fileName);
                }

                var bucket = fileNameSplit[0];
                var file = fileNameSplit[1];

                var config = new ObsConfig();
                config.Endpoint = EndPoint;
                var client = new ObsClient(AccessKey, SecretAccessKey, config);
                var request = new GetObjectRequest()
                {
                    BucketName = bucket,
                    ObjectKey = file,
                };

                using (var response = client.GetObject(request))
                {
                    await response.OutputStream.CopyToAsync(stream);
                    return stream;
                }
            }

            if (ConstantHelpers.GENERAL.FileStorage.STORAGE_MODE == ConstantHelpers.FileStorage.Mode.AMAZONS3_STORAGE_MODE)
            {
                var accessKeyS3 = ConstantHelpers.AmazonS3.AccessKey;
                var secretKeyS3 = ConstantHelpers.AmazonS3.SecretKey;
                var bucketNameS3 = ConstantHelpers.AmazonS3.BucketName;
                var credentials = new BasicAWSCredentials(accessKeyS3, secretKeyS3);

                var config = new AmazonS3Config()
                {
                    RegionEndpoint = ConstantHelpers.AmazonS3.RegionEndpoint
                };

                // initialise client
                using var client = new AmazonS3Client(credentials, config);

                using (var response = await client.GetObjectAsync(bucketNameS3, fileName))
                {
                    await response.ResponseStream.CopyToAsync(stream);
                    return stream;
                }
            }

            Uri uriResult;

            if (!fileName.StartsWith("https://") && fileName.StartsWith("https:/"))
            {
                fileName = fileName.Substring(7);
                fileName = "https://" + fileName;
            }

            var result = Uri.TryCreate(fileName, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if ((ConstantHelpers.GENERAL.FileStorage.STORAGE_MODE == ConstantHelpers.FileStorage.Mode.SERVER_STORAGE_MODE || folderForUserImages) && !result)
            {
                return await DownloadByServerStorageMode(stream, fileName);
            }
            else
            {
                if (result) cloudStorageContainer = fileName.Split('/')[3];

                fileName = fileName.Split($"/{cloudStorageContainer}/").Last();

                var container = _blobClient.GetContainerReference(cloudStorageContainer);
                var blockBlob = container.GetBlockBlobReference(fileName);
                await blockBlob.DownloadToStreamAsync(stream);
                return stream;
            }
        }

        private async Task<Stream> DownloadByServerStorageMode(Stream stream, string fileName )
        {
            var filePath = Path.Combine(ConstantHelpers.GENERAL.FileStorage.PATH, fileName);
            if (File.Exists(filePath))
            {
                using (var file = new FileStream(filePath, FileMode.Open))
                {
                    await file.CopyToAsync(stream);
                }
                return stream;
            }
            return null;
        }

        public async Task<Stream> TryDownload(Stream stream, string cloudStorageContainer, string filePath, bool folderForUserImages = false)
        {
            return await Download(stream, cloudStorageContainer, filePath, folderForUserImages);
        }

        public async Task<Stream> TryDownloadProductBinary(Stream stream, string fileName)
        {
            return await Download(stream, "binaries", fileName);
        }

        public async Task<Stream> TryDownloadProductImage(Stream stream, string fileName)
        {
            return await Download(stream, "applications-images", fileName);
        }

        public async Task<HashSet<(string, MemoryStream)>> DownloadFilesInBulk(string cloudStorageContainer, List<string> fileReferences, List<string> fileNames)
        {
            var listMStream = new HashSet<(string, MemoryStream)>();
            if (ConstantHelpers.GENERAL.FileStorage.STORAGE_MODE == ConstantHelpers.FileStorage.Mode.SERVER_STORAGE_MODE)
            {
                foreach (var item in fileReferences.Select((value, i) => new { i, value }))
                {
                    var filePath = Path.Combine(ConstantHelpers.GENERAL.FileStorage.PATH, item.value);
                    if (File.Exists(filePath))
                    {
                        var mStream = new MemoryStream();
                        using (var file = new FileStream(filePath, FileMode.Open))
                        {
                            await file.CopyToAsync(mStream);
                        }
                        listMStream.Add(($"{fileNames[item.i]}", mStream));
                    }
                }
            }
            else if (ConstantHelpers.GENERAL.FileStorage.STORAGE_MODE == ConstantHelpers.FileStorage.Mode.HUAWEI_STORAGE_MODE)
            {
                var config = new ObsConfig();
                config.Endpoint = EndPoint;
                var client = new ObsClient(AccessKey, SecretAccessKey, config);

                foreach (var item in fileReferences.Select((value, i) => new { i, value }))
                {
                    var fileNameSplit = item.value.Split('/');
                    var bucket = fileNameSplit[0];
                    var file = fileNameSplit[1];

                    var request = new GetObjectRequest()
                    {
                        BucketName = bucket,
                        ObjectKey = file,
                    };

                    var mStream = new MemoryStream();
                    using (var response = client.GetObject(request))
                    {
                        await response.OutputStream.CopyToAsync(mStream);
                    }
                    listMStream.Add(($"{fileNames[item.i]}", mStream));
                }
            }
            else
            {
                var container = _blobClient.GetContainerReference(cloudStorageContainer);

                foreach (var item in fileReferences.Select((value, i) => new { i, value }))
                {
                    var fileName = item.value.Split($"/{cloudStorageContainer}/").Last();
                    var blockBlob = container.GetBlockBlobReference(fileName);
                    var mStream = new MemoryStream();
                    await blockBlob.DownloadToStreamAsync(mStream);
                    listMStream.Add(($"{fileNames[item.i]}", mStream));
                }
            }
            return listMStream;
        }

        public async Task<MemoryStream> DownloadImage(string container, string filename)
        {
            var mStream = new MemoryStream();

            var stream = await Download(mStream, container, filename);

            await stream.CopyToAsync(mStream);

            return mStream;
        }
    }
}