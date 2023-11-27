using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AnswerByUser;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class AnswerByUserService : IAnswerByUserService
    {
        private readonly IAnswerByUserRepository _answerByUserRepository;

        public AnswerByUserService(IAnswerByUserRepository answerByUserRepository)
        {
            _answerByUserRepository = answerByUserRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> AnsweredSurveyUserDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId)
        {
            return await _answerByUserRepository.AnsweredSurveyUserDatatable(sentParameters, surveyId);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> ReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? surveyId, string search)
        {
            return await _answerByUserRepository.ReportDatatable(sentParameters, surveyId, search);
        }

        public async Task InsertRange(IEnumerable<AnswerByUser> answers)
        {
            await _answerByUserRepository.InsertRange(answers);
        }

        public async Task<bool> WasSurveyAnswered(Guid surveyUserId)
        {
            return await _answerByUserRepository.WasSurveyAnswered(surveyUserId);
        }

        public async Task AddRange(IEnumerable<AnswerByUser> answersByUser)
        {
            await _answerByUserRepository.AddRange(answersByUser);
        }

        public async Task<IEnumerable<AnswerByUser>> GetAnswerByUserBySurveyId(Guid eid)
        {
            return await _answerByUserRepository.GetAnswerByUserBySurveyId(eid);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAnswerByUserByQuestionIdDataTable(DataTablesStructs.SentParameters parameters, Guid qid)
        {
            return await _answerByUserRepository.GetAnswerByUserByQuestionIdDataTable(parameters, qid);
        }

        public Task<DataTablesStructs.ReturnedData<object>> GetAnswersFromTextQuesitonDataTable(DataTablesStructs.SentParameters sentParameters, Guid questionId, bool? graduated = null)
            => _answerByUserRepository.GetAnswersFromTextQuesitonDataTable(sentParameters, questionId, graduated);

        public async Task<IEnumerable<AnswerByUserTemplate>> GetAnswerByUserByQuestionId(Guid qid)
        {
            return await _answerByUserRepository.GetAnswerByUserByQuestionId(qid);
        }

        public Task<List<SurveyUserReportTemplate>> GetUserAnswersBySurvey(Guid surveyId)
            => _answerByUserRepository.GetUserAnswersBySurvey(surveyId);
    }
}
