using AKDEMIC.PDF.Models;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.CertificateOfStudiesGenerator
{
    public interface ICertificateOfStudiesGeneratorService
    {
        Task<Result> GetCertificateOfStudiesPDF(Guid studentId, Guid? userProcedureId, Guid? recordHistoryId = null);
    }
}
