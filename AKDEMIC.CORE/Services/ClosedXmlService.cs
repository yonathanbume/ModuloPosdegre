using System.IO;
using System.Threading.Tasks;
using System.Web;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.CORE.Services
{
    public class ClosedXmlService : IClosedXmlService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClosedXmlService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        async Task IClosedXmlService.GenerateBasicXml<T>(ClosedXmlStruct<T> xmlStruct)
        {

            using (var memoryStream = new MemoryStream())
            {
                using (var xlWorkbook = new XLWorkbook())
                {
                    var worksheet = xlWorkbook.Worksheets.Add($"{xmlStruct.FileName}");

                    var initialColumnHeader = 1;
                    foreach (var columnHeader in xmlStruct.Sheet.ColumnHeaders)
                    {
                        worksheet.BoldThickBorderedCell($"A{initialColumnHeader}").Value = columnHeader;
                        initialColumnHeader++;
                    }

                    worksheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Style.Alignment.WrapText = true;

                    for (int i = 0; i < xmlStruct.Sheet.Data.Count; i++)
                    {
                        for (int j = 1; j <= xmlStruct.Sheet.ColumnHeaders.Length; j++)
                        {
                            worksheet.Cell(i + 2, j).Value = "";
                        }
                    }

                    worksheet.Columns(1, 10).AdjustToContents(0.0, 75.0);
                    xlWorkbook.SaveAs(memoryStream);
                }

                var fileName = HttpUtility.UrlEncode($"{xmlStruct.FileName}.xlsx");
                memoryStream.Position = 0;
                _httpContextAccessor.HttpContext.Response.ContentType = ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.XLSX;
                _httpContextAccessor.HttpContext.Response.Headers["Content-Disposition"] = $"attachment;filename=\"{fileName}\"";
                _httpContextAccessor.HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

                await memoryStream.CopyToAsync(_httpContextAccessor.HttpContext.Response.Body);
            }
        }
    }
}