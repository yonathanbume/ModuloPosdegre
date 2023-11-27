using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class TeachingLoadSubTypeRepository : Repository<TeachingLoadSubType>, ITeachingLoadSubTypeRepository
    {
        public TeachingLoadSubTypeRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachingLoadSubTypeDatatable(DataTablesStructs.SentParameters parameters, Guid? teachingLoadTypeId)
        {
            var query = _context.TeachingLoadSubTypes
                .AsNoTracking();

            if (teachingLoadTypeId.HasValue)
                query = query.Where(x => x.TeachingLoadTypeId == teachingLoadTypeId);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.TeachingLoadTypeId,
                    TeachingLoadType = x.TeachingLoadType.Name,
                    x.FixedTime,
                    x.MinHours,
                    x.Enabled,
                    x.MaxHours
                })
                .ToListAsync();

            var recordsTotal = data.Count;
            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    
        public async Task<object> GetTeachingLoadSubTypeSelect2(Guid? teachingLoadtypeId, bool? enabled = null)
        {
            var query = _context.TeachingLoadSubTypes.AsNoTracking();

            if (enabled.HasValue && enabled.Value)
                query = query.Where(x => x.Enabled);

            if (teachingLoadtypeId.HasValue)
                query = query.Where(x => x.TeachingLoadTypeId == teachingLoadtypeId);

            var result = await query
                .Select(x => new
                {
                    x.Id,
                    x.MinHours,
                    x.MaxHours,
                    x.FixedTime,
                    Text  = x.Name
                })
                .ToListAsync();

            return result;
        }
    }
}
