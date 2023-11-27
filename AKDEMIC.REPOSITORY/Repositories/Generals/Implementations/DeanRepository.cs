using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class DeanRepository : Repository<Dean>, IDeanRepository
    {
        public DeanRepository(AkdemicContext context):base(context) { }

        public override async Task<Dean> Get(string id)
            => await _context.Deans.FindAsync(id);
    }
}
