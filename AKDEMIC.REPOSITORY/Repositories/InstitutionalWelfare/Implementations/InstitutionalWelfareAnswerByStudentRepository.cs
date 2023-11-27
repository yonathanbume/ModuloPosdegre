using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareAnswerByStudentRepository : Repository<InstitutionalWelfareAnswerByStudent>, IInstitutionalWelfareAnswerByStudentRepository
    {
        public InstitutionalWelfareAnswerByStudentRepository(AkdemicContext akdemicContext): base(akdemicContext)
        {

        }

        public async Task<bool> ExistAnswerByStudent(Guid recordId, Guid studentId, Guid termId)
        {
            var result = await _context.InstitutionalWelfareAnswerByStudents.Where(p => p.TermId == termId).AnyAsync(x => x.InstitutionalWelfareQuestion.InstitutionalWelfareSection.InstitutionalWelfareRecordId == recordId && x.StudentId == studentId);
            return result;
        }

        public async Task<bool> ExistAnswerByUserName(Guid recordId, string userName, Guid termId)
        {
            var result = await _context.InstitutionalWelfareAnswerByStudents.Where(p=>p.TermId == termId).AnyAsync(x => x.InstitutionalWelfareQuestion.InstitutionalWelfareSection.InstitutionalWelfareRecordId == recordId && x.Student.User.UserName == userName);
            return result;
        }

        public async Task<List<RecordUserReportTemplate>> GetUserAnswersByRecord(Guid recordId, Guid termId)
        {
            var surveyAnswerByUsers = await _context.InstitutionalRecordCategorizationByStudents
                .AsNoTracking()
                .Where(x => x.InstitutionalWelfareRecordId == recordId)
                .Select(x => new RecordUserReportTemplate
                {
                    UserName = x.Student.User.UserName,
                    FullName = x.Student.User.FullName,
                    AnswersQuestions = x.Student.InstitutionalWelfareAnswerByStudents
                        .Where(y => y.InstitutionalWelfareQuestion.InstitutionalWelfareSection.InstitutionalWelfareRecordId == x.InstitutionalWelfareRecordId && y.TermId == termId)
                        .Select(y => new RecordAnswerReportTemplate
                        {
                            QuestionId = y.InstitutionalWelfareQuestionId,
                            Question = y.InstitutionalWelfareQuestion.Description,
                            Answer = y.InstitutionalWelfareQuestion.Type == ConstantHelpers.SURVEY.TEXT_QUESTION ? y.AnswerDescription : y.InstitutionalWelfareAnswer.Description
                        }).ToList()
                }).ToListAsync();

            return surveyAnswerByUsers;
        }
    }
}
