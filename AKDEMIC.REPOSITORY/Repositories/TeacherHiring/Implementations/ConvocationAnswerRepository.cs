using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Implementations
{
    public class ConvocationAnswerRepository : Repository<ConvocationAnswer>, IConvocationAnswerRepository
    {
        public ConvocationAnswerRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<ConvocationAnswer>> GetAllByQuestionId(Guid questionId)
            => await _context.ConvocationAnswers.Where(x => x.ConvocationQuestionId == questionId).ToArrayAsync();

        public async Task<IEnumerable<ConvocationAnswer>> GetAllBySectionId(Guid sectionId)
    => await _context.ConvocationAnswers.Where(x => x.ConvocationQuestion.ConvocationSectionId == sectionId).ToArrayAsync();
    }
}
