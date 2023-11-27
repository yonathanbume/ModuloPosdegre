using AKDEMIC.PDF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.ReportCardGenerator
{
    public interface IReportCardGeneratorService
    {
        Task<Result> GetReportCardPDF(Guid studentId, Guid termId, Guid? userProcedureId = null);
    }
}
