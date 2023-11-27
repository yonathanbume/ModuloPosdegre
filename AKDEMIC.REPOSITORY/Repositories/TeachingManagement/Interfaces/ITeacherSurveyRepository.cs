using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface ITeacherSurveyRepository: IRepository<TeacherSurvey>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetSatisfactionPendingUsersBySurveyDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId,string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherOnSurveySectionDatatable(DataTablesStructs.SentParameters sentParameters, Guid seccionId, Guid surveyId);
    }
}
