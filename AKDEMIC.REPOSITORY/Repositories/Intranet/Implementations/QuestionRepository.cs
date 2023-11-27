using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class QuestionRepository: Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(AkdemicContext context) : base(context) { }

        public override async Task DeleteById(Guid id)
        {
            var question = await _context.Question
                                    .Include(x => x.Answers)
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();

            _context.Answer.RemoveRange(question.Answers);
            await _context.SaveChangesAsync();
            await base.DeleteById(id);
        }

        public async Task<Question> GetIncludeAnswers(Guid id)
        {
            var query = _context.Question
                .Include(x => x.Answers)
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }
    }
}
