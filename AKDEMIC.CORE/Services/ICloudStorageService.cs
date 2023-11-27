using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services
{
    public interface ICloudStorageService
    {
        Task<string> UploadFile(Stream stream, string container, string extension, string systemFolder = null);
        Task<string> UploadFile(Stream stream, string container, string filename, string extension, string systemFolder = null, bool isUserImage = false);
        Task<string> UploadProductBinary(Stream stream, string container, string systemFolder = null);
        Task<bool> TryDeleteProductBinary(string fileName);
        Task<bool> TryDelete(string fileName, string cloudStorageContainer);
        Task<Stream> TryDownload(Stream stream, string cloudStorageContainer, string filePath, bool folderForUserImages = false);
        Task<HashSet<(string, MemoryStream)>> DownloadFilesInBulk(string container, List<string> fileReferences, List<string> fileNames);
        Task<MemoryStream> DownloadImage(string container, string filename);
    }
}
