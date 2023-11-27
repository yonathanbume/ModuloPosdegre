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
    public class ConvocationQuestionRepository : Repository<ConvocationQuestion> , IConvocationQuestionRepository
    {
        public ConvocationQuestionRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<ConvocationQuestion>> GetAllBySectionId(Guid convocationSectionId)
            => await _context.ConvocationQuestions.Where(x => x.ConvocationSectionId == convocationSectionId).ToListAsync();

        public async Task<bool> AnyByDescription(Guid sectionId, string description, Guid? ignoredId = null)
            => await _context.ConvocationQuestions.Where(x => x.ConvocationSectionId == sectionId).AnyAsync(x => x.Description.ToLower().Equals(description.ToLower()) && x.Id != ignoredId);

    }
}
