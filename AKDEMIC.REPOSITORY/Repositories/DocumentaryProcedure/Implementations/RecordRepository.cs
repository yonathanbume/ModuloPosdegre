using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class RecordRepository : Repository<Record>, IRecordRepository
    {
        public RecordRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> AnyByName(string value)
        {
            return await _context.Records.AnyAsync(x => x.Name == value);
        }

        public async Task DeleteByName(string name)
        {
            var record = await _context.Records.Where(x => x.Name == name).FirstOrDefaultAsync();
            _context.Records.Remove(record);
            await _context.SaveChangesAsync();
        }
    }
}
