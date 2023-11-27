using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CourseCertificateRepository : Repository<CourseCertificate>, ICourseCertificateRepository
    {
        public CourseCertificateRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            Expression<Func<CourseCertificate, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                default:
                    break;
            }

            var query = _context.CourseCertificates.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue)) query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    createdAt = x.CreatedAt.ToLocalDateFormat()
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

        public async Task<object> GetSelect2ClientSide()
        {
            var result = await _context.CourseCertificates
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();

            return result;
        }
    }
}
