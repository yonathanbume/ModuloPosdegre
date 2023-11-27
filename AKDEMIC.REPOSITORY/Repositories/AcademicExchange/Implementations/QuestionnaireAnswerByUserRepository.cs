using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Implementations
{
    public class QuestionnaireAnswerByUserRepository : Repository<QuestionnaireAnswerByUser>, IQuestionnaireAnswerByUserRepository
    {
        public QuestionnaireAnswerByUserRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<QuestionnaireAnswerByUser>> GetAllByPostulationId(Guid postulationId)
            => await _context.QuestionnaireAnswerByUsers.Where(x => x.PostulationId == postulationId).ToArrayAsync();
    }
}
