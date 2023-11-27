using OBS;
using OBS.Model;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services
{
    public class HuaweiStorageService : IHuaweiStorageService
    {
        //region LA-Santiago
        private readonly string EndPoint = "https://obs.la-south-2.myhuaweicloud.com";
        private readonly string AccessKey = "1JMHFANSFPEMK07J6AIY";
        private readonly string SecretAccessKey = "hi9AwQjlRcaUT4u0ktlWyD4KOCZfoUHp82qH4Wzk";

        public async Task<Stream> DownloadFileAsync(Stream stream, string bucketName, string fileName)
        {
            //Initialize configuration parameters.
            var config = new ObsConfig();
            config.Endpoint = EndPoint;
            // Create an instance of ObsClient.
            var client = new ObsClient(AccessKey, SecretAccessKey, config);
            // Use the instance to access OBS.
            var request = new GetObjectRequest()
            {
                BucketName = bucketName,
                ObjectKey = fileName,
            };

            using (var response = client.GetObject(request))
            {
                await response.OutputStream.CopyToAsync(stream);
                return stream;
            }
        }

        public string UploadFile(Stream stream, string bucketName, string fileName)
        {
            //Initialize configuration parameters.
            var config = new ObsConfig();
            config.Endpoint = EndPoint;
            // Create an instance of ObsClient.
            var client = new ObsClient(AccessKey, SecretAccessKey, config);
            // Use the instance to access OBS.
            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    ObjectKey = fileName,
                    InputStream = stream
                };

                var response = client.PutObject(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //var name = response.ObjectUrl.Split('/').Last();
                    //return name;
                    return fileName;
                }

                return null;
            }
            catch (ObsException ex)
            {
                Console.WriteLine("ErrorCode: {0}", ex.ErrorCode);
                Console.WriteLine("ErrorMessage: {0}", ex.ErrorMessage);
                return null;
            }
        }
    }
}
