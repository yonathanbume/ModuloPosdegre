using ClosedXML.Excel;

namespace AKDEMIC.CORE.Extensions
{
    public static class IXLWorksheetExtensions
    {
        public static IXLCell BoldThickBorderedCell(this IXLWorksheet xLWorksheet, string cellAddressInRange)
        {
            var cell = xLWorksheet.Cell(cellAddressInRange);
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            cell.Style.Font.Bold = true;

            return cell;
        }

        public static IXLCell BoldThinBorderedCell(this IXLWorksheet xLWorksheet, string cellAddressInRange)
        {
            var cell = xLWorksheet.Cell(cellAddressInRange);
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            cell.Style.Font.Bold = true;

            return cell;
        }

        public static IXLCell ThickBorderedCell(this IXLWorksheet xLWorksheet, string cellAddressInRange)
        {
            var cell = xLWorksheet.Cell(cellAddressInRange);
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

            return cell;
        }

        public static IXLCell ThinBorderedCell(this IXLWorksheet xLWorksheet, string cellAddressInRange)
        {
            var cell = xLWorksheet.Cell(cellAddressInRange);
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            return cell;
        }
    }
}
