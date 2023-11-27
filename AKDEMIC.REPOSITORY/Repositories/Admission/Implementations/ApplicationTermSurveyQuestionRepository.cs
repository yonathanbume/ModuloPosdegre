using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class ApplicationTermSurveyQuestionRepository : Repository<ApplicationTermSurveyQuestion>, IApplicationTermSurveyQuestionRepository
    {
        public ApplicationTermSurveyQuestionRepository(AkdemicContext context) : base(context) { }

        public async Task<List<ApplicationTermSurveyQuestion>> GetQuestionsBySurveyId(Guid applicationTermSurveyId)
            => await _context.ApplicationTermSurveyQuestions.Where(x => x.ApplicationTermSurveyId == applicationTermSurveyId).Include(x=>x.ApplicationTermSurveyAnswers).ToListAsync();

        public async Task DeleteAnswers(Guid questionId)
        {
            var answers = await _context.ApplicationTermSurveyAnswers.Where(x => x.ApplicationTermSurveyQuestionId == questionId).ToListAsync();
            _context.ApplicationTermSurveyAnswers.RemoveRange(answers);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ApplicationTermSurveyAnswer>> GetAnswersByQuestionId(Guid questionId)
            => await _context.ApplicationTermSurveyAnswers.Where(x => x.ApplicationTermSurveyQuestionId == questionId).ToListAsync();

    }
}
