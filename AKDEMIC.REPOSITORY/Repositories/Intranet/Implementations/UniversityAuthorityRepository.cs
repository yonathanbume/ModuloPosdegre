using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class UniversityAuthorityRepository : Repository<UniversityAuthority>, IUniversityAuthorityRepository
    {
        public UniversityAuthorityRepository(AkdemicContext context) : base(context) { }

        public override async Task<UniversityAuthority> Get(Guid id)
        {
           return await _context.UniversityAuthorities
                .Where(x => x.Id == id)
                .Include(x => x.UniversityAuthorityHistories)
                .Include(x=>x.User)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> ExistAuthorityType(int type, Guid? universityAuthorityId)
        {
            var query = _context.UniversityAuthorities.AsQueryable();
            if (universityAuthorityId.HasValue)
            {
                query = query.Where(x => x.Id != universityAuthorityId);
            }
            return await query.AnyAsync(x => x.Type == type);
        }

        public async Task<List<UniversityAuthority>> GetUniversityAuthoritiesList()
        {
            return await _context.UniversityAuthorities.Include(x=>x.User).ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUniversityAuthority(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query = _context.UniversityAuthorities
                .Include(x=>x.User)
                .AsQueryable();          

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => 
                x.User.Name.ToLower().Contains(searchValue.ToLower())
                );
               
            }

            var recordsFiltered = 0;
            recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name  =$"{x.User.Name} {x.User.PaternalSurname} {x.User.MaternalSurname}",
                    type = ConstantHelpers.UNIVERSITY_AUTHORITY.TYPE.VALUES[x.Type]                    
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUniversityAuthorityHistory(Guid id)
        {
            var query = _context.UniversityAuthorityHistory
                .Where(x=>x.UniversityAuthorityId == id)
                .AsQueryable();
            
            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderBy(x=>x.CreatedAt)
                .Select(x => new
                {
                    id = x.Id,
                    name = $"{x.User.Name} {x.User.PaternalSurname} {x.User.MaternalSurname}",
                    type = ConstantHelpers.UNIVERSITY_AUTHORITY.TYPE.VALUES[x.Type],
                    date = x.CreatedAt.ToLocalDateFormat(),
                    fileUrl = x.FileUrl
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = recordsFiltered,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
