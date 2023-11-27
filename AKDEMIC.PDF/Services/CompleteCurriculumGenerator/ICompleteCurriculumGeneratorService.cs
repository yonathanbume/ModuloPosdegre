using AKDEMIC.PDF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.CompleteCurriculumGenerator
{
    public interface ICompleteCurriculumGeneratorService
    {
        Task<Result> GetCompleteCurriculumPDF(Guid studentId, Guid? userProcedureId);
    }
}
