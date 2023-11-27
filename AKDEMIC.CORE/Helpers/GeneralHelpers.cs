using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Helpers
{
    public class GeneralHelpers
    {
        public static string GetDatabase()
        {
            throw new NotImplementedException();
        }

        public static string GetDatabaseConnectionString()
        {
            switch (ConstantHelpers.GENERAL.DATABASES.DATABASE)
            {
                case ConstantHelpers.DATABASES.MYSQL:
                    return GetMySqlDatabaseConnectionString();
                case ConstantHelpers.DATABASES.SQL:
                    return GetSqlDatabaseConnectionString();
                default:
                    return "";
            }
        }

        public static string GetMySqlDatabaseConnectionString()
        {
            return ConstantHelpers.DATABASES.CONNECTION_STRINGS.MYSQL.VALUES[ConstantHelpers.GENERAL.DATABASES.CONNECTION_STRINGS.CONNECTION_STRING];
        }

        public static string GetMySqlDatabaseConnectionStringDatabaseName()
        {
            var connectionString = ConstantHelpers.DATABASES.CONNECTION_STRINGS.MYSQL.VALUES[ConstantHelpers.GENERAL.DATABASES.CONNECTION_STRINGS.CONNECTION_STRING];
            var connectionStringSplit = connectionString.Split(";");
            var databaseName = "";

            for (var i = 0; i < connectionStringSplit.Length; i++)
            {
                var connectionStringSplitValue = connectionStringSplit[i];
                var connectionStringSplitValueUpper = connectionStringSplitValue.ToUpper();

                if (connectionStringSplitValueUpper.Contains("DATABASE"))
                {
                    var connectionStringSplitValueSplit = connectionStringSplitValue.Split("=");
                    databaseName = connectionStringSplitValueSplit[1];

                    break;
                }
            }

            return databaseName;
        }

        public static string GetSqlDatabaseConnectionString()
        {
            return ConstantHelpers.DATABASES.CONNECTION_STRINGS.SQL.VALUES[ConstantHelpers.GENERAL.DATABASES.CONNECTION_STRINGS.CONNECTION_STRING];
        }

        public static string GetSqlDatabaseConnectionStringDatabaseName()
        {
            var connectionString = ConstantHelpers.DATABASES.CONNECTION_STRINGS.SQL.VALUES[ConstantHelpers.GENERAL.DATABASES.CONNECTION_STRINGS.CONNECTION_STRING];
            var connectionStringSplit = connectionString.Split(";");
            var databaseName = "";

            for (var i = 0; i < connectionStringSplit.Length; i++)
            {
                var connectionStringSplitValue = connectionStringSplit[i];
                var connectionStringSplitValueUpper = connectionStringSplitValue.ToUpper();

                if (connectionStringSplitValueUpper.Contains("INITIAL CATALOG"))
                {
                    var connectionStringSplitValueSplit = connectionStringSplitValue.Split("=");
                    databaseName = connectionStringSplitValueSplit[1];

                    break;
                }
            }

            return databaseName;
        }

        public static string GetDatabaseVersion()
        {
            throw new NotImplementedException();
        }

        public static string GetInstitutionAbbreviation()
        {
            return ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value];
        }

        public static string GetInstitutionLocation()
        {
            return ConstantHelpers.Institution.Locations[ConstantHelpers.GENERAL.Institution.Value];
        }

        public static string GetInstitutionName()
        {
            return ConstantHelpers.Institution.Names[ConstantHelpers.GENERAL.Institution.Value];
        }

        public static string GetTheme()
        {
            return ConstantHelpers.Institution.Values[ConstantHelpers.GENERAL.Themes.Value];
        }

        public static Dictionary<int, string> GetSystems()
        {

            return ConstantHelpers.Solution.Institution[ConstantHelpers.GENERAL.Institution.Value];
        }

        public static async Task GetFileForDownload(HttpContext httpContext, IOptions<CloudStorageCredentials> _storageCredentials, string container, string fileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                var cloudStorageService = new CloudStorageService(_storageCredentials);

                var file = Path.GetFileNameWithoutExtension(fileName)+ Path.GetExtension(fileName);

                var contentDisposition = $"attachment;filename=\"{file.Normalize().Replace(' ', '_')}\"";

                await cloudStorageService.TryDownload(memoryStream, container, fileName);

                httpContext.Response.Headers["Content-Disposition"] = contentDisposition;
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(httpContext.Response.Body);
            }
        }

        public static async Task<string> GetImageForStringPartialView(IOptions<CloudStorageCredentials> _storageCredentials, string pictureUrl,string container=null, bool folderForUserImages = false)
        {
            using (var memoryStream = new MemoryStream())
            {
                var cloudStorageService = new CloudStorageService(_storageCredentials);

                await cloudStorageService.TryDownload(memoryStream, container, pictureUrl,folderForUserImages);
                if (memoryStream == null) return $"data:image/*;base64";

                var imageBytes = memoryStream.ToArray();

                return $"data:image/*;base64,{Convert.ToBase64String(imageBytes)}";
            }
        }

        //public static string CreateQR(string url)
        //{
        //    QRCodeGenerator qrGenerator = new QRCodeGenerator();            
        //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        //    QRCode qrCode = new QRCode(qrCodeData);
        //    var qrCodeImage = qrCode.GetGraphic(3);
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //        var bitMap = stream.ToArray();
        //        return $"data:image/png;base64,{Convert.ToBase64String(bitMap)}";
        //    }
        //}            

        public static void ExecuteBashShellScript(string shellFilePath)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c " + shellFilePath,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                }
            };
            process.Start();
            process.WaitForExit();
        }

        public static string GetApplicationRoute(int application, bool isDevelopment = false)
        {
            try
            {
                if (isDevelopment) return ConstantHelpers.Solution.Routes[0][application];

                return ConstantHelpers.Solution.Routes[ConstantHelpers.GENERAL.Institution.Value][application];
            }
            catch (Exception)
            {
                return ConstantHelpers.Solution.Routes[0][application];
            }
        }

        public static string GetAuthority(bool isDevelopment = false)
        {
            try
            {
                if (isDevelopment) return ConstantHelpers.Authentication.SingleSignOn.LOCALHOST_AUTHORITY;

                return ConstantHelpers.Authentication.SingleSignOn.Authorities[ConstantHelpers.GENERAL.Institution.Value];
            }
            catch (Exception)
            {
                return ConstantHelpers.Authentication.SingleSignOn.LOCALHOST_AUTHORITY;
            }
        }

        public static decimal GetUNICADecryptedGrade(string encryptedGrade)
        {
            var encryptedGradeSubstring = encryptedGrade.Substring(1, 2);

            switch (encryptedGradeSubstring)
            {
                case "DE": // No Se Presento (NSP)
                    return -1;
                case "^_": // Desaprobado Por Inasistencia (DPI)
                    return -2;
            }

            var encryptedGradeCharacters = encryptedGradeSubstring.ToCharArray();

            Array.Reverse(encryptedGradeCharacters);

            int DecryptFirstDigit(char encryptedGradeCharacter)
            {
                return encryptedGradeCharacter switch
                {
                    'e' => 0,
                    'g' => 1,
                    'i' => 2,
                    _ => 0,
                };
            }

            int DecryptSecondDigit(char encryptedGradeCharacter)
            {
                return encryptedGradeCharacter switch
                {
                    'd' => 0,
                    'f' => 1,
                    'h' => 2,
                    'j' => 3,
                    'l' => 4,
                    'n' => 5,
                    'p' => 6,
                    'r' => 7,
                    't' => 8,
                    'v' => 9,
                    _ => 0,
                };
            }

            var encryptedGradeCharactersLength = encryptedGradeCharacters.Length;
            var decryptedGrade = 0M;

            for (var i = 0; i < encryptedGradeCharacters.Length; i++)
            {
                var encryptedGradeCharacter = encryptedGradeCharacters[i];
                var multiplier = (int)Math.Pow(10, encryptedGradeCharactersLength - i - 1);

                switch (i)
                {
                    case 0:
                        decryptedGrade += DecryptFirstDigit(encryptedGradeCharacter) * multiplier;

                        break;
                    case 1:
                        decryptedGrade += DecryptSecondDigit(encryptedGradeCharacter) * multiplier;

                        break;
                }
            }
            
            return decryptedGrade;
        }
    }
}
