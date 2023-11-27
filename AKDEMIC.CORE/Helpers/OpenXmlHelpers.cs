using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Linq;

namespace AKDEMIC.CORE.Helpers
{
    public class OpenXmlHelpers
    {
        public static class Excel
        {
            public static string GetCellValue(Cell cell)
            {
                if (cell == null)
                {
                    return null;
                }

                switch (cell.DataType)
                {
                    case null when cell.CellValue == null:
                        return null;
                    case null:
                        return cell.CellValue.Text;
                }

                var value = cell.CellValue.Text;

                switch (cell.DataType.Value)
                {
                    case CellValues.SharedString:
                        // For shared strings, look up the value in the shared strings table.
                        // Get worksheet from cell
                        OpenXmlElement parent = cell.Parent;

                        while (parent.Parent != null && parent.Parent != parent && string.Compare(parent.LocalName, "worksheet", true) != 0)
                        {
                            parent = parent.Parent;
                        }

                        if (string.Compare(parent.LocalName, "worksheet", true) != 0)
                        {
                            throw new Exception("Unable to find parent worksheet.");
                        }

                        Worksheet ws = parent as Worksheet;
                        SpreadsheetDocument ssDoc = ws.WorksheetPart.OpenXmlPackage as SpreadsheetDocument;
                        SharedStringTablePart sstPart = ssDoc.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

                        // lookup value in shared string table
                        if (sstPart != null && sstPart.SharedStringTable != null)
                        {
                            value = sstPart.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
                        }

                        break;
                    case CellValues.Boolean:
                        switch (value)
                        {
                            case "0":
                                value = "FALSE";
                                break;
                            default:
                                value = "TRUE";
                                break;
                        }

                        break;
                }

                return value;
            }
        }
    }
}
