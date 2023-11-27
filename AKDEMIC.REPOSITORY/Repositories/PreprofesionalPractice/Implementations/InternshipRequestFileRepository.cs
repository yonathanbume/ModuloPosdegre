using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Implementations
{
    public class InternshipRequestFileRepository : Repository<InternshipRequestFile> ,IInternshipRequestFileRepository
    {
        public InternshipRequestFileRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid internshipRequestId, byte type)
        {
            Expression<Func<InternshipRequestFile, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                default:
                    break;
            }

            var query = _context.InternshipRequestFiles
                .Where(x=>x.InternshipRequestId == internshipRequestId && x.Type == type)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.FileUrl,
                    x.CreatedAt
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }
    }
}
