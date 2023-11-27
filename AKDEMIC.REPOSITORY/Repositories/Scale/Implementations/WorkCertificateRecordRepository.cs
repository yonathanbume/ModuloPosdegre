using AKDEMIC.CORE.Structs;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class WorkCertificateRecordRepository : Repository<WorkCertificateRecord> , IWorkCertificateRecordRepository
    {
        public WorkCertificateRecordRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkCertificateRecordsDatatable(DataTablesStructs.SentParameters sentParameters, string userId)
        {
            Expression<Func<ScaleExtraContractField, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.LaborPosition); break;
                case "2":
                    orderByPredicate = ((x) => x.ScaleResolution.BeginDate); break;
                case "3":
                    orderByPredicate = ((x) => x.ScaleResolution.EndDate); break;
            }

            var query = _context.ScaleExtraContractFields
                .Where(x => x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == "Contratos" && x.ScaleResolution.UserId == userId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var dependencies = await _context.Dependencies.Select(x => new { x.Id, x.Name }).ToListAsync();

            var dataDB = await query
               .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
               .Skip(sentParameters.PagingFirstRecord)
               .Take(sentParameters.RecordsPerDraw)
               .Select(x => new
               {
                   x.Id,
                   x.ScaleResolution.UserId,
                   x.ScaleResolution.BeginDate,
                   x.ScaleResolution.ResolutionNumber,
                   x.LaborPosition,
                   x.ScaleResolution.EndDate,
                   x.DependencyId
               })
               .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.Id,
                    startDate = x.BeginDate.ToLocalDateFormat(),
                    endDate = x.EndDate.ToLocalDateFormat(),
                    dependency = dependencies.Where(y => y.Id == x.DependencyId).Select(y => y.Name).FirstOrDefault() ?? "-",
                    laborPosition = $"{x.ResolutionNumber} - {x.LaborPosition}"
                })
                .ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
