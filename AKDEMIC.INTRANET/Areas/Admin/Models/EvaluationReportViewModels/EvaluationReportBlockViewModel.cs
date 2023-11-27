using PdfSharp.Pdf;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.EvaluationReportViewModels
{
    public class EvaluationReportBlockViewModel
    {
        public string Teacher { get; set; }

        public PdfDocument Pdf { get; set; }

        public byte[] Document { get; set; }
    }
}
