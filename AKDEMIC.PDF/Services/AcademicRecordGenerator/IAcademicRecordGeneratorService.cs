using AKDEMIC.PDF.Models;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.AcademicRecordGenerator
{
    public interface IAcademicRecordGeneratorService
    {
        Task<Result> GetAcademicRecordPDF(Guid studentId, Guid? userProcedureId, Guid? recordHistoryId = null);
    }
}
