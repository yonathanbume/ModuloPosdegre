using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.SanctionedPostulant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class SanctionedPostulantRepository : Repository<SanctionedPostulant>, ISanctionedPostulantRepository
    {
        public SanctionedPostulantRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSanctionedPostulantDatatable(DataTablesStructs.SentParameters parameters, Guid? applicationTermId, string search)
        {
            Expression<Func<SanctionedPostulant, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.DNI;
                    break;
                case "1":
                    orderByPredicate = (x) => x.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.ApplicationTerm.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.FullName;
                    break;
            }


            var query = _context.SanctionedPostulants
               .AsNoTracking();

            if (applicationTermId.HasValue && applicationTermId != Guid.Empty)
                query = query.Where(x => x.ApplicationTermId == applicationTermId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.FullName.ToLower().Trim().Contains(search.ToLower().Trim()) || x.DNI.ToLower().Trim().Contains(search.ToLower().Trim()));

            var recordsFiltered = await query.CountAsync();


            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.FullName,
                    document = x.DNI,
                    applicationTerm = x.ApplicationTermId.HasValue ? $"{x.ApplicationTerm.Term.Name} - {x.ApplicationTerm.Name}" : "Sin Asignar"
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
        
        public async Task<List<SanctionedPostulantTemplate>> GetSanctionedPostulantData(Guid? applicationTermId, string search)
        {
            var query = _context.SanctionedPostulants
               .AsNoTracking();

            if (applicationTermId.HasValue && applicationTermId != Guid.Empty)
                query = query.Where(x => x.ApplicationTermId == applicationTermId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.FullName.ToLower().Trim().Contains(search.ToLower().Trim()) || x.DNI.ToLower().Trim().Contains(search.ToLower().Trim()));

            var data = await query
                .Select(x => new SanctionedPostulantTemplate
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Document = x.DNI,
                    Term = x.ApplicationTerm.Term.Name,
                    ApplicationTerm = x.ApplicationTermId.HasValue ? $"{x.ApplicationTerm.Term.Name} - {x.ApplicationTerm.Name}" : "Sin Asignar"
                })
                .ToListAsync();

            return data;
        }

        public async Task<bool> SanctionedDNI(string dni, Guid applicationTermId)
        {
            var result = await _context.SanctionedPostulants.AnyAsync(x => x.DNI.ToUpper() == dni.ToUpper()
            && (!x.ApplicationTermId.HasValue || x.ApplicationTermId == applicationTermId));
            return result;
        }

        public async Task<bool> AnyByDni(string dni, Guid? applicationTermId ,Guid? ignoredId = null)
            => await _context.SanctionedPostulants.AnyAsync(x => x.DNI.Trim().ToLower() == dni.ToLower().Trim() && x.ApplicationTermId == applicationTermId && x.Id != ignoredId);
    }
}
