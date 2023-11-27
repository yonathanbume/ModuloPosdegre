using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services
{
    public interface IHuaweiStorageService
    {
        Task<Stream> DownloadFileAsync(Stream stream, string bucketName, string fileName);
        string UploadFile(Stream stream, string bucketName, string fileName);
    }
}
