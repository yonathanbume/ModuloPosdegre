using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Implementations
{
    public class CompanyInterestRepository : Repository<CompanyInterest>, ICompanyInterestRepository
    {
        public CompanyInterestRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<CompanyInterest>> GetCompanyInterestDatatable(DataTablesStructs.SentParameters sentParameters, string company, string startDate, string endDate, Guid? lineId)
        {
            Expression<Func<CompanyInterest, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Company.User.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Project.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Project.ResearchLine.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Date);
                    break;
                default:
                    orderByPredicate = ((x) => x.Company);
                    break;
            }

            DateTime? startDatetime = null;
            DateTime? endDatetime = null;

            if (!string.IsNullOrEmpty(startDate))
            {
                startDatetime = ConvertHelpers.DatepickerToUtcDateTime(startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                endDatetime = ConvertHelpers.DatepickerToUtcDateTime(endDate);
            }

            IQueryable<CompanyInterest> query = _context.InvestigationCompanyInterests.AsQueryable();

            if (!string.IsNullOrEmpty(company))
            {
                query = query.Where(x => x.Company.User.Name.ToUpper().Contains(company.ToUpper()));
            }

            if (startDatetime.HasValue)
            {
                query = query.Where(x => x.Date > startDatetime);
            }

            if (endDatetime.HasValue)
            {
                query = query.Where(x => x.Date < endDatetime);
            }

            if (lineId.HasValue)
            {
                query = query.Where(x => x.Project.ResearchLineId == lineId.Value);
            }

            query = query.OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate).AsNoTracking();

            Expression<Func<CompanyInterest, CompanyInterest>> selectPredicate = (x) => new CompanyInterest
            {
                Company = new Company
                {
                    User = new ApplicationUser
                    {
                        Name = x.Company.User.Name
                    }
                },
                Project = new Project
                {
                    Name = x.Project.Name,
                    Coordinator = new ApplicationUser
                    {
                        FullName = x.Project.Coordinator.FullName
                    },
                    ResearchLine = new ResearchLine
                    {
                        Name = x.Project.ResearchLine.Name
                    }
                },
                DateFormatted = x.Date.ToLocalDateFormat()
            };

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<CompanyInterest>> GetCompanyInterestProjectDatatable(DataTablesStructs.SentParameters sentParameters, Guid projectId)
        {
            try
            {
                IQueryable<CompanyInterest> query = _context.InvestigationCompanyInterests.AsQueryable();

                query = query.Where(x => x.ProjectId == projectId).OrderBy(x => x.Date).AsNoTracking();

                Expression<Func<CompanyInterest, CompanyInterest>> selectPredicate = (x) => new CompanyInterest
                {
                    Company = new Company
                    {
                        User = new ApplicationUser
                        {
                            Name = x.Company.User.Name,
                            Email = x.Company.User.Email,
                            PhoneNumber = x.Company.User.PhoneNumber
                        }
                    },
                    Project = new Project
                    {
                        Name = x.Project.Name,
                        ResearchLine = new ResearchLine
                        {
                            Name = x.Project.ResearchLine.Name
                        }
                    },
                    DateFormatted = x.Date.ToString("dd/MM/yyyy")                   
                };

                return await query.ToDataTables(sentParameters, selectPredicate);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CompanyInterest> GetByCompanyIdAndProjectId(Guid companyId, Guid projectId)
            => await _context.InvestigationCompanyInterests.Where(x => x.CompanyId == companyId && x.ProjectId == projectId).FirstOrDefaultAsync();
    }
}
