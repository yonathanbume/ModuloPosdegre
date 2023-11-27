using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class TeacherSurveyService : ITeacherSurveyService
    {
        private readonly ITeacherSurveyRepository _teacherSurveyRepository;

        public TeacherSurveyService(ITeacherSurveyRepository teacherSurveyRepository)
        {
            _teacherSurveyRepository = teacherSurveyRepository;
        }

        public async Task Add(TeacherSurvey teacherSurvey)
        {
            await _teacherSurveyRepository.Add(teacherSurvey);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSatisfactionPendingUsersBySurveyDatatable(DataTablesStructs.SentParameters sentParameters,Guid surveyId, string searchValue = null)
        {
            return await _teacherSurveyRepository.GetSatisfactionPendingUsersBySurveyDatatable(sentParameters, surveyId,searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherOnSurveySectionDatatable(DataTablesStructs.SentParameters sentParameters, Guid seccionId, Guid surveyId)
        {
            return await _teacherSurveyRepository.GetTeacherOnSurveySectionDatatable(sentParameters, seccionId, surveyId);
        }
    }
}
