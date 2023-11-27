using AKDEMIC.PDF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.CertificateGenerator
{
    public interface ICertificateGeneratorService
    {
        Task<Result> GeneratePdf(byte recordType, Guid recordHistoryId, Guid? userProcedureId = null);
    }
}
