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
    public class ConvocationSectionRepository : Repository<ConvocationSection> , IConvocationSectionRepository
    {
        public ConvocationSectionRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<ConvocationSection>> GetSectionsByConvocationId(Guid convocationId)
        {
            var result = await _context.ConvocationSections.Where(x => x.ConvocationId == convocationId)
                .OrderBy(x=>x.CreatedAt)
                .Select(x => new ConvocationSection
                {
                    Id = x.Id,
                    ConvocationId = x.ConvocationId,
                    Title = x.Title,
                    ConvocationQuestions = x.ConvocationQuestions
                    .OrderBy(x=>x.CreatedAt)
                    .Select(y => new ConvocationQuestion
                    {
                        Id = y.Id,
                        Description = y.Description,
                        ConvocationSectionId = y.ConvocationSectionId,
                        Type = y.Type,
                        ConvocationAnswers = y.ConvocationAnswers
                        .Select(z => new ConvocationAnswer
                        {
                            Id = z.Id,
                            ConvocationQuestionId = z.ConvocationQuestionId,
                            Description = z.Description
                        }).ToArray()
                    }).ToArray()
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<bool> AnyByTitle(Guid convocationId, string title, Guid? ignoredId = null)
            => await _context.ConvocationSections.Where(x => x.ConvocationId == convocationId && x.Id != ignoredId).AnyAsync(x => x.Title.ToLower().Equals(title.ToLower()));
    }
}
