using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AnswerByUser;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IAnswerByUserService
    {
        Task AddRange(IEnumerable<AnswerByUser> answersByUser);
        Task<DataTablesStructs.ReturnedData<object>> AnsweredSurveyUserDatatable(DataTablesStructs.SentParameters sentParameters,Guid surveyId);
        Task<DataTablesStructs.ReturnedData<object>> ReportDatatable(DataTablesStructs.SentParameters sentParameters,Guid? surveyId,string search);
        Task<bool> WasSurveyAnswered(Guid surveyUserId);
        Task InsertRange(IEnumerable<AnswerByUser> answers);
        Task<IEnumerable<AnswerByUser>> GetAnswerByUserBySurveyId( Guid eid);
        Task<DataTablesStructs.ReturnedData<object>> GetAnswerByUserByQuestionIdDataTable(DataTablesStructs.SentParameters parameters, Guid qid);
        Task<DataTablesStructs.ReturnedData<object>> GetAnswersFromTextQuesitonDataTable(DataTablesStructs.SentParameters sentParameters, Guid questionId, bool? graduated = null);
        Task<IEnumerable<AnswerByUserTemplate>> GetAnswerByUserByQuestionId(Guid qid);
        Task<List<SurveyUserReportTemplate>> GetUserAnswersBySurvey(Guid surveyId);
    }
}
