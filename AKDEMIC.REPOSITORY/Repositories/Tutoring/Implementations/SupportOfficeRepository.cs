using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class SupportOfficeRepository : Repository<SupportOffice>, ISupportOfficeRepository
    {
        public SupportOfficeRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSupportOfficeDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<SupportOffice, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.Name;
                    break;
            }


            var query = _context.SupportOffices
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(q => q.Name.ToUpper().Contains(search));
            }
            
            query = query.AsQueryable();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    id = x.Id,
                    name = x.Name
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<SupportOffice>> GetAllWithOut(Guid? supportOfficeId = null)
        {
            var query = _context.SupportOffices.AsNoTracking();

            if(supportOfficeId.HasValue)
                query =  _context.SupportOffices.Where(x => x.Id != supportOfficeId);

            return await query.ToListAsync();
        }

        public async Task<object> GetSelect2WithOut(Guid? supportOfficeId = null)
        {
            var query = _context.SupportOffices.AsNoTracking();

            if (supportOfficeId != null)
                query = query.Where(x => x.Id != supportOfficeId);

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .ToListAsync();

            return result;
        }
    }
}
