using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICourseRecognitionService
    {
        Task DeleteById(Guid id);
        Task<object> GetRecognitionAcademicHistoriesDatatable(Guid recognitionId);
    }
}
