using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class ProcedureAdmissionTypeRepository : Repository<ProcedureAdmissionType>, IProcedureAdmissionTypeRepository
    {
        public ProcedureAdmissionTypeRepository(AkdemicContext context) : base(context) { }

        public async Task<ProcedureAdmissionType> Get(Guid procedureId, Guid admissionTypeId)
            => await _context.ProcedureAdmissionTypes.Where(x => x.ProcedureId == procedureId && x.AdmissionTypeId == admissionTypeId).FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetProcedureAdmissionTypeToAssignDatatable(DataTablesStructs.SentParameters parameters, Guid procedureId, string search)
        {
            Expression<Func<AdmissionType, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.Name); break;
            }

            var query = _context.AdmissionTypes.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(search.Trim().ToLower()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Abbreviation,
                    assigned = x.ProcedureAdmissionTypes.Any(y=>y.ProcedureId == procedureId)
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
    }
}
