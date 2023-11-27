using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class ApplicationTermManagerRepository : Repository<ApplicationTermManager>, IApplicationTermManagerRepository
    {
        public ApplicationTermManagerRepository(AkdemicContext context) : base(context)
        {
        }
        private IQueryable<ApplicationTermManager> GetByAppTermId(Guid id, string searchValue = null)
        {
            var managers = _context.ApplicationTermManagers
                            .Include(x => x.User)
                            .Where(x => x.ApplicationTermId == id).AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                    managers = managers.Where(x => EF.Functions.Contains(x.User.FullName, searchValue));
                else
                    managers = managers.Where(q => q.User.UserName.Contains(searchValue) || q.User.FullName.Contains(searchValue));

            return managers.OrderBy(x=>x.User.FullName);
        }
        public async Task<List<ApplicationTermManager>> GetByApplicationTermId(Guid id)
        {
            return await GetByAppTermId(id).ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<ApplicationTermManager>> GetByApplicationTermIdDataTable(DataTablesStructs.SentParameters sentParameters, Guid id, string searchValue)
        {
            var managers = GetByAppTermId(id, searchValue);

            return await managers.ToDataTables(sentParameters);
        }

        public async Task<bool> AnyByApplicationTerm(Guid id, string userId)
        {
            return await _context.ApplicationTermManagers.AnyAsync(x => x.UserId == userId && x.ApplicationTermId == id);
        }
    }
}
