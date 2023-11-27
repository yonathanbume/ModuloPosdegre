using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Implementations
{
    public class ComputerConditionFileRepository : Repository<ComputerConditionFile>, IComputerConditionFileRepository
    {
        public ComputerConditionFileRepository(AkdemicContext context) :base(context) { }

        public async Task<IEnumerable<ComputerConditionFile>> GetAllByComputerId(Guid computerId)
            => await _context.ComputerConditionFiles.Where(x => x.ComputerId == computerId).ToArrayAsync();
    }
}
