using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.VisitManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.VisitManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.VisitManagement.Implementations
{
    public class VisitRepository : Repository<Visit> , IVisitRepository
    {
        public VisitRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<Visit>> GetVisitDatatable(DataTablesStructs.SentParameters sentParameters,string startDate = null,string endDate = null, Guid? dependencyId=null, string search = null)
        {
            Expression<Func<Visit, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.VisitDate);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.VisitorInformation.Type);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Dependency.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.UserToVisit.FullName);
                    break;
                default:
                    orderByPredicate = ((x) => x.VisitDate);
                    break;
            }

            var query = _context.Visits.AsQueryable();

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                var converterStartDate = ConvertHelpers.DatepickerToUtcDateTime(startDate);
                var converterEndDate = ConvertHelpers.DatepickerToUtcDateTime(endDate);
                query = query.Where(x => x.VisitDate.Date >= converterStartDate.Date && x.VisitDate.Date <= converterEndDate.Date);
            }

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.UserToVisit.FullName.Trim().ToLower().Contains(search.Trim().ToLower()));

            if (dependencyId.HasValue)
                query = query.Where(x => x.DependencyId == dependencyId);

            query = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            Expression<Func<Visit, Visit>> selectPredicate = (x) => new Visit
            {
                Id = x.Id,
                VisitFormattedDateTime = x.VisitDate.ToLocalDateTimeFormat(),
                VisitorInformation = new VisitorInformation
                {
                    Type = x.VisitorInformation.Type
                },
                UserToVisit = new ApplicationUser
                {
                    FullName = x.UserToVisit.FullName
                },
                Dependency = new Dependency
                {
                    Name = x.Dependency.Name
                },
            };

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<Visit>> GetVisitDatatableToPublic(DataTablesStructs.SentParameters sentParameters,string dateTime)
        {
            var query = _context.Visits.AsQueryable();

            if (!string.IsNullOrEmpty(dateTime))
            {
                var converterDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateTime);
                query = query.Where(x => x.VisitDate.Date == converterDateTime.Date);
            }

            query = query
                .OrderByDescending(x=>x.VisitDate)
                .AsNoTracking();

            Expression<Func<Visit, Visit>> selectPredicate = (x) => new Visit
            {
                Id = x.Id,
                VisitFormattedDate = x.VisitDate.ToLocalDateFormat(),
                VisitFormattedTime = x.VisitDate.ToLocalTimeFormat(),
                VisitorInformation = new VisitorInformation
                {
                    Type = x.VisitorInformation.Type,
                    CompanyRuc = string.IsNullOrEmpty(x.VisitorInformation.CompanyRuc) ? "-" : x.VisitorInformation.CompanyRuc,
                    ExternalUser = new ExternalUser
                    {
                        MaternalSurname = x.VisitorInformation.ExternalUser.MaternalSurname,
                        PaternalSurname = x.VisitorInformation.ExternalUser.PaternalSurname,
                        Name = x.VisitorInformation.ExternalUser.Name
                    }
                },
                UserToVisit = new ApplicationUser
                {
                    FullName = x.UserToVisit.FullName
                },
                Dependency = new Dependency
                {
                    Name = x.Dependency.Name
                },
                Reason = x.Reason
            };

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        public async Task<object> GetVisitByDependenciesChart()
        {
            var visit = await _context.Visits.GroupBy(x => x.Dependency.Name)
                .Select(x => new
                {
                    name = x.Key,
                    y = x.Count()
                }).ToArrayAsync();

            return visit;
        }
    }
}
