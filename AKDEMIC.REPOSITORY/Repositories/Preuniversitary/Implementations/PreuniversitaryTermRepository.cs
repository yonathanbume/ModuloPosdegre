using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Implementations
{
    public class PreuniversitaryTermRepository : Repository<PreuniversitaryTerm>, IPreuniversitaryTermRepository
    {
        public PreuniversitaryTermRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchValue = null)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.UserName); break;
                case "3":
                    orderByPredicate = ((x) => x.Email); break;
                case "4":
                    orderByPredicate = ((x) => x.PhoneNumber); break;
                default:
                    orderByPredicate = ((x) => x.FullName); break;
            }

            var preuniversitaryTermAsync = _context.PreuniversitaryTerms.FindAsync(preuniversitaryTermId);
            var query = _context.Users.Where(x => x.UserRoles
                .Any(r => r.Role.Name == ConstantHelpers.ROLES.PRE_UNIVERSITARY_STUDENTS))
                .Where(x => x.PreuniversitaryUserGroups.Any(pu => pu.PreuniversitaryGroup.PreuniversitaryTermId == preuniversitaryTermId))
                .AsQueryable();

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      name = x.FullName,
                      userName = x.UserName,
                      email = x.Email,
                      phoneNumber = x.PhoneNumber
                  })
                  .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<PreuniversitaryTerm> GetActivePreUniversitaryApplicationTerm()
            => await _context.PreuniversitaryTerms.Where(x => x.StartDate <= DateTime.UtcNow && DateTime.UtcNow <= x.EndDate).FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetTermDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<PreuniversitaryTerm, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Year); break;
                case "1":
                    orderByPredicate = ((x) => x.StartDate); break;
                case "2":
                    orderByPredicate = ((x) => x.EndDate); break;
                case "3":
                    orderByPredicate = ((x) => x.ClassStartDate); break;
                case "4":
                    orderByPredicate = ((x) => x.ClassEndDate); break;
                case "5":
                    orderByPredicate = ((x) => x.ClassEndDate); break;
                case "6":
                    orderByPredicate = ((x) => x.ClassEndDate); break;
                default:
                    orderByPredicate = ((x) => x.StartDate);
                    sentParameters.OrderDirection = ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION;
                    break;
            }

            var query = _context.PreuniversitaryTerms.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Year.ToString().Contains(searchValue) ||
                                            x.Order.ToString().Contains(searchValue));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      x.Year,
                      x.Order,
                      startDate = x.StartDate.ToLocalDateFormat(),
                      endDate = x.EndDate.ToLocalDateFormat(),
                      classStartDate = x.ClassStartDate.ToLocalDateFormat(),
                      classEndDate = x.ClassEndDate.ToLocalDateFormat(),
                      postulantStartDate = x.PostulantStartDate.ToLocalDateFormat(),
                      postulantEndDate = x.PostulantEndDate.ToLocalDateFormat()
                  })
                  .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<bool> AnyByDateTimeConflict(DateTime parsedStartDate, DateTime parsedEndDate, Guid? ignoredId = null)
            => await _context.PreuniversitaryTerms.AnyAsync(t => t.Id != ignoredId && (t.StartDate <= parsedStartDate && parsedStartDate <= t.EndDate) || (t.StartDate <= parsedEndDate && parsedEndDate <= t.EndDate) || (parsedStartDate <= t.StartDate && t.StartDate <= parsedEndDate) || (parsedStartDate <= t.EndDate && t.EndDate <= parsedEndDate));

            //=> _context.PreuniversitaryTerms
            //        .Select(x => new
            //        {
            //            x.Id,
            //            x.StartDate,
            //            x.EndDate
            //        })
            //        .AsEnumerable()
            //        .Any(x => x.Id != ignoredId && ConvertHelpers.DateTimeConflict(x.StartDate, x.EndDate, parsedStartDate, parsedEndDate));

        public async Task<object> GetPreunviersitaryTermSelect2ClientSide()
        {
            var result = await _context.PreuniversitaryTerms
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Order)
                .Select(x => new
                {
                    id = x.Id,
                    Text = x.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<object> GetCurrentPreuniversitaryTerm()
            => await _context.PreuniversitaryTerms.Where(x => x.StartDate <= DateTime.UtcNow && DateTime.UtcNow < x.EndDate)
            .Select(x => new { id = x.Id, text = x.Name }).FirstOrDefaultAsync();
    }
}
