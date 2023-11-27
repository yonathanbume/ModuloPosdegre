using Microsoft.AspNetCore.Http;
using System.IO;

namespace AKDEMIC.CORE.Extensions
{
    public static class IFormFileExtensions
    {
        public static bool HasContentType(this IFormFile formFile, string contentTypes, string separator = ",")
        {
            var tmpContentTypes = contentTypes.Split(separator);

            return formFile.HasContentType(tmpContentTypes);
        }

        public static bool HasContentType(this IFormFile formFile, string[] contentTypes)
        {
            for (var i = 0; i < contentTypes.Length; i++)
            {
                var contentType = contentTypes[i].ToLower();
                var formFileContentType = formFile.ContentType.ToLower();

                if (contentType.StartsWith("*"))
                {
                    contentType = contentType.Remove(0);

                    if (formFileContentType.EndsWith(contentType))
                    {
                        return true;
                    }
                }
                else if (contentType.EndsWith("*"))
                {
                    contentType = contentType.Remove(contentType.Length - 1);

                    if (formFileContentType.StartsWith(contentType))
                    {
                        return true;
                    }
                }
                else
                {
                    if (formFileContentType == contentType)
                    {
                        return true;
                    }
                }

            }

            return false;
        }

        public static bool HasExtension(this IFormFile formFile, string extensions, string separator = ",")
        {
            var tmpExtensions = extensions.Split(separator);

            return formFile.HasExtension(tmpExtensions);
        }

        public static bool HasExtension(this IFormFile formFile, string[] extensions)
        {
            for (var i = 0; i < extensions.Length; i++)
            {
                var extension = extensions[i].ToLower();

                if (!extension.StartsWith("."))
                {
                    extension = $".{extension}";
                }

                if (Path.GetExtension(formFile.FileName).ToLower() == extension)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
