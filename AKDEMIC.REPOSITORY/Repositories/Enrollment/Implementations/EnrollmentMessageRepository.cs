using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
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
    public class EnrollmentMessageRepository : Repository<EnrollmentMessage>, IEnrollmentMessageRepository
    {
        public EnrollmentMessageRepository(AkdemicContext context) : base(context)
        {
        }
        public override async Task<EnrollmentMessage>Get(Guid id)
        {
            return await _context.EnrollmentMessages
                .Where(x => x.Id== id)
                .Include(x => x.User)
                .FirstOrDefaultAsync();
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid careerId, string searchValue)
        {
            Expression<Func<EnrollmentMessage, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.WasAttended;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var query = _context.EnrollmentMessages
                .AsNoTracking();

            var term = await _context.Terms
                .FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            if (term != null)
            {
                query = query.Where(x => x.CreatedAt >= term.EnrollmentStartDate);
            }

            var students = new List<string>();
            if (careerId != Guid.Empty)
            {
                students = await _context.Students.Where(x => x.CareerId == careerId).Select(x => x.UserId).ToListAsync();
                query = query.Where(x => students.Contains(x.UserId));
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();


            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    Id = x.Id,
                    Name =x.User.FullName ??  x.Name,
                    x.Message,
                    x.PhoneNumber,
                    x.Email,
                    x.UserId,
                    x.WasAttended,
                    date = x.CreatedAt.ToLocalDateTimeFormat()
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
    }
}
