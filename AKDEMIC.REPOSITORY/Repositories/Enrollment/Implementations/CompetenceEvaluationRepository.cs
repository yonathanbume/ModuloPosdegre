using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CompetenceEvaluationRepository : Repository<CompetenceEvaluation>, ICompetenceEvaluationRepository
    {
        public CompetenceEvaluationRepository(AkdemicContext context) : base(context) { }

        public Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            throw new System.NotImplementedException();
        }

        public override async Task<IEnumerable<CompetenceEvaluation>> GetAll() => await _context.CompetenceEvaluations.Include(x => x.EvaluationType).ToListAsync();
    }
}
