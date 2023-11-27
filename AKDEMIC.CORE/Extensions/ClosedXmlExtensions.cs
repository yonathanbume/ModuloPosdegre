using AKDEMIC.CORE.Helpers;
using ClosedXML.Excel;
using System.IO;

namespace AKDEMIC.CORE.Extensions
{
    public static class ClosedXmlExtensions
    {
        public static void AddHeaderToWorkSheet(this IXLWorksheet worksheet, string title, string logo, int tableLength = 5)
        {
            worksheet.Row(1).InsertRowsAbove(5);

            var mergeRangeColumn = "F";

            if (tableLength != 5) mergeRangeColumn = NumberToLetter(tableLength);

            if (!string.IsNullOrEmpty(logo))
            {
                using (var memoryStream = new MemoryStream(File.ReadAllBytes(logo)))
                {
                    worksheet.AddPicture(memoryStream).MoveTo(worksheet.Cell("A1")).WithSize(60, 60);
                }
            }

            worksheet.Cell(2, 2).Value = GeneralHelpers.GetInstitutionName().ToUpper();
            worksheet.Cell(2, 2).Style.Font.FontSize = 12;
            worksheet.Cell(2, 2).Style.Font.Bold = true;
            worksheet.Cell(2, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(2, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"B2:{mergeRangeColumn}2").Merge();

            worksheet.Cell(3, 2).Value = title;
            worksheet.Cell(3, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(3, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range($"B3:{mergeRangeColumn}3").Merge();
        }

        private static string NumberToLetter(int number)
        {
            switch (number)
            {
                case 1: return "A";
                case 2: return "B";
                case 3: return "C";
                case 4: return "D";
                case 5: return "E";
                case 6: return "F";
                case 7: return "G";
                case 8: return "H";
                case 9: return "I";
                case 10: return "J";
                case 11: return "K";
                case 12: return "L";
                case 13: return "M";
                case 14: return "N";
                case 15: return "O";
                case 16: return "P";
                case 17: return "Q";
                case 18: return "R";
                case 19: return "S";
                case 20: return "T";
                case 21: return "U";
                case 22: return "V";
                case 23: return "W";
                case 24: return "X";
                case 25: return "Y";
                case 26: return "Z";
                case 27: return "AA";
                case 28: return "AB";
                case 29: return "AC";
                case 30: return "AD";
                case 31: return "AE";
                case 32: return "AF";
                case 33: return "AG";
                case 34: return "AH";
                case 35: return "AI";
                case 36: return "AJ";
                case 37: return "AK";
                case 38: return "AL";
                case 39: return "AM";
                case 40: return "AN";
                case 41: return "AO";
                case 42: return "AP";
                case 43: return "AQ";
                case 44: return "AR";
                case 45: return "AS";
                case 46: return "AT";
                case 47: return "AU";
                case 48: return "AV";
                case 49: return "AW";
                case 50: return "AX";
                case 51: return "AY";
                case 52: return "AZ";
                default:
                    return "E";
            }
        }
    }
}
