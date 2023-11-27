using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public sealed class ContractRepository : Repository<Contract>, IContractRepository
    {
        public ContractRepository(AkdemicContext context) : base(context) { }

        async Task<object> IContractRepository.GetAllAsModelA()
        {
            var query = _context.Contract
            .AsQueryable();

            var result = await query.Select(x => new
            {
                id = x.Id,
                resolution = x.Resolution,
                comment = x.Commentario,
                begin = x.Begin.ToShortDateString(),
                end = x.End.ToShortDateString(),
            }).ToListAsync();

            return result;
        }
    }
}