using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface ITeacherSurveyService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetSatisfactionPendingUsersBySurveyDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherOnSurveySectionDatatable(DataTablesStructs.SentParameters sentParameters, Guid seccionId, Guid surveyId);
        Task Add(TeacherSurvey teacherSurvey);     
    }
}
