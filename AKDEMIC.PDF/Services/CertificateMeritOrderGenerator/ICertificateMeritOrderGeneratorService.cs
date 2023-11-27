using AKDEMIC.PDF.Models;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.CertificateMeritOrderGenerator
{
    public interface ICertificateMeritOrderGeneratorService
    {
        Task<Result> GetCertificateMeritOrderPDF(Guid recordHistoryId, Guid? userProcedureId);
    }
}
