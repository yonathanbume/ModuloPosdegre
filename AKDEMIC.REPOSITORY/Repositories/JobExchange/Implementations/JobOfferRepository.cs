using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class JobOfferRepository: Repository<JobOffer> , IJobOfferRepository
    {
        public JobOfferRepository(AkdemicContext context):base(context){ }

        public IQueryable<JobOffer> GetAllIQueryable(Guid? careerId = null)
        {
            var query = _context.JobOffers
                .Include(x => x.Company)
                    .ThenInclude(x => x.User)
                .Include(x => x.JobOfferLanguages)
                .ThenInclude(x=>x.Language)
                .Include(x => x.JobOfferAbilities)
                .ThenInclude(x=>x.Ability)
                .Include(x => x.JobOfferApplications)
                .Include(x => x.JobOfferCareers)
                .ThenInclude(x=>x.Career)
                .AsQueryable();

            if (careerId.HasValue)
            {
                query = query.Where(x => x.JobOfferCareers.Any(y => y.CareerId == careerId));
            }
               
            return query;
        }
        public IQueryable<JobOffer> GetAllIQueryable(List<Guid> careers)
        {
            var query = _context.JobOffers
                .Include(x => x.Company)
                    .ThenInclude(x => x.User)
                .Include(x => x.JobOfferLanguages)
                .ThenInclude(x => x.Language)
                .Include(x => x.JobOfferAbilities)
                .ThenInclude(x => x.Ability)
                .Include(x => x.JobOfferApplications)
                .Include(x => x.JobOfferCareers)
                .ThenInclude(x => x.Career)
                .AsQueryable();

            if (careers!=null&&!careers.Contains(Guid.Empty))
            {
                var client = query.Select(x =>new
                {
                    x.Id,
                   careers= x.JobOfferCareers.Select(y =>y.CareerId).ToList()
                }).ToList();
                var ids = client.Where(x => x.careers.Any(y => careers.Contains(y))).Select(x=>x.Id).ToList();
                query = query.Where(x => ids.Contains(x.Id));
            }

            return query;
        }
        public async Task<IEnumerable<JobOffer>> GetLastJobOffersAvailable(int take, Guid? careerId = null)
        {
            var query = _context.JobOffers
                .Include(x => x.JobOfferApplications)
                    .ThenInclude(x => x.Student)
                .Include(x => x.Company)
                    .ThenInclude(x => x.User)
                .Where(x => x.Status == ConstantHelpers.JobOffer.Status.ACTIVE)
                .AsQueryable();
            if (careerId.HasValue)
            {
                query = query.Where(x => x.JobOfferCareers.Any(y => y.CareerId == careerId));
            }
            var queryTaked = await query.OrderByDescending(x => x.CreatedAt).Take(take).ToListAsync();
            return queryTaked;
        }

        public async Task<JobOffer> GetWithIncludes(Guid id)
        {
            var query = _context.JobOffers
                .Include(x => x.JobOfferCareers)
                    .ThenInclude(x => x.Career)
                .Include(x => x.JobOfferLanguages)
                    .ThenInclude(x => x.Language)
                .Include(x => x.JobOfferAbilities)
                    .ThenInclude(x => x.Ability)
                .Include(x => x.JobOfferApplications)
                .Include(x => x.Company)
                    .ThenInclude(x => x.User)
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<object> GetJobsOffersForCompaniesChart(string searchValue = null, string startSearchDate = null, string endSearchDate = null)
        {
            var query = _context.JobOffers.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                var search = searchValue.Trim();
                query = query.Where(x => x.Company.User.Name.ToUpper().Contains(search.ToUpper()));
            }


            if (!string.IsNullOrEmpty(startSearchDate))
            {
                var StartDate = ConvertHelpers.DatepickerToUtcDateTime(startSearchDate);
                query = query.Where(x => x.StartDate >= StartDate);
            }

            if (!string.IsNullOrEmpty(endSearchDate))
            {
                var EndDate = ConvertHelpers.DatepickerToUtcDateTime(endSearchDate);
                query = query.Where(x => x.EndDate <= EndDate);
            }


            var companies = await _context.Companies
                .Select(x => new
                {
                    Name = x.User.Name,
                    Count = query.Where(y => y.CompanyId == x.Id).Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync();

            var result = new
            {
                categories = companies.Select(x => x.Name).ToList(),
                data = companies.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobsOffersForCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string startSearchDate = null, string endSearchDate = null)
        {
            var query = _context.JobOffers.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                var search = searchValue.Trim();
                query = query.Where(x => x.Company.User.Name.ToUpper().Contains(search.ToUpper()));
            }

            if (!string.IsNullOrEmpty(startSearchDate))
            {
                var StartDate = ConvertHelpers.DatepickerToUtcDateTime(startSearchDate);
                query = query.Where(x => x.StartDate >= StartDate);
            }

            if (!string.IsNullOrEmpty(endSearchDate))
            {
                var EndDate = ConvertHelpers.DatepickerToUtcDateTime(endSearchDate);
                query = query.Where(x => x.EndDate <= EndDate);
            }

            var recordsFiltered = await query
                    .Select(x => x.CompanyId)
                    .Distinct()
                    .CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.CompanyId,
                    CompanyName = x.Company.User.Name
                })
                .GroupBy(x => new { x.CompanyId, x.CompanyName })
                .OrderBy(x => x.Key.CompanyName)
                .Select(x => new
                {
                    x.Key.CompanyId,
                    x.Key.CompanyName,
                    Accepted = x.Count()
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobsOffersCreatedByDatatable(DataTablesStructs.SentParameters sentParameters, string userName, string searchValue = null)
        {
            Expression<Func<JobOffer, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Company.User.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Position);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.StartDate);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.EndDate);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Status);
                    break;
            }

            var query = _context.JobOffers.Where(x => x.CreatedBy == userName).AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Company.User.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.Position.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    CompanyName = x.Company.User.Name,
                    x.Position,
                    StartDate = x.StartDate.ToLocalDateFormat(),
                    EndDate = x.EndDate.ToLocalDateFormat(),
                    x.Status,
                    StatusText = ConstantHelpers.JobOffer.Status.VALUES.ContainsKey(x.Status) ?
                                ConstantHelpers.JobOffer.Status.VALUES[x.Status] : ""
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<List<JobOfferTemplate>> GetAllByCompanyId(Guid companyId, int? status = null, DateTime? endDate = null)
        {
            var query = _context.JobOffers
                .Where(x => x.CompanyId == companyId)
                .AsNoTracking();

            if (status != null)
                query = query.Where(x => x.Status == status);

            if (endDate != null)
                query = query.Where(x => x.EndDate.Date >= endDate);

            var result = await query
                .Select(x => new JobOfferTemplate
                {
                    Id = x.Id,
                    Position = x.Position,
                    Company = x.Company.User.Name,
                    CompanyImage = string.IsNullOrEmpty(x.Company.User.Picture) ? "/images/demo/company.jpg" : "imagenes/" + x.Company.User.Picture,
                    PublishDate = x.CreatedAt.ToDefaultTimeZone().ToLongDateString(),
                    Salary = x.Salary,
                    Type = ConstantHelpers.JobOffer.Type.VALUES.ContainsKey(x.Type) ? ConstantHelpers.JobOffer.Type.VALUES[x.Type] : "-",
                    WorkType = ConstantHelpers.JobOffer.WorkType.VALUES.ContainsKey(x.WorkType) ?  ConstantHelpers.JobOffer.WorkType.VALUES[x.WorkType] : "-",
                    Functions = x.Functions,
                    EndDate = x.EndDate,
                    StartDate = x.StartDate,
                    Status = x.Status
                })
                .ToListAsync();

            return result;

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetHomeJobsOffersDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<JobOffer, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Position);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Type);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Company.User.FullName);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.StartDate);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.EndDate);
                    break;
            }

            var query = _context.JobOffers.Where(x => x.Status == ConstantHelpers.JobOffer.Status.ACTIVE).AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Position,
                    CompanyName = x.Company.User.FullName,
                    Type = ConstantHelpers.COMPANIES.TYPES.VALUES.ContainsKey(x.Type) ? ConstantHelpers.COMPANIES.TYPES.VALUES[x.Type] : "",
                    StartDate = x.StartDate.ToLocalDateFormat(),
                    EndDate = x.EndDate.ToLocalDateFormat()
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<object> GetJobExchangeJobOfferCareerReportChart(List<Guid> careers = null)
        {
            var categoriesData = new List<string>();
            var seriesData = new List<int>();

            var careerQuery = _context.Careers.AsNoTracking();
            var jobOffers = _context.JobOffers.AsNoTracking();


            if (careers != null && careers.Count > 0)
            {
                careerQuery = careerQuery.Where(x => careers.Contains(x.Id));
                jobOffers = jobOffers.Where(x => x.JobOfferCareers.Any(y => careers.Contains(y.CareerId)));
            }


            var data = await careerQuery
                .Select(x => new
                {
                    Name = x.Name,
                    Count = jobOffers.Where(y => y.JobOfferCareers.Any(z => z.CareerId == x.Id)).Count()
                })
                .ToListAsync();

            return new
            {
                categoriesData = data.Select(x => x.Name).ToList(),
                seriesData = data.Select(x => x.Count).ToList()
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeJobOfferCareerReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers = null)
        {
            var categoriesData = new List<string>();
            var seriesData = new List<int>();

            var careerQuery = _context.Careers.AsNoTracking();
            var jobOffers = _context.JobOffers.AsNoTracking();


            if (careers != null && careers.Count > 0)
            {
                careerQuery = careerQuery.Where(x => careers.Contains(x.Id));
                jobOffers = jobOffers.Where(x => x.JobOfferCareers.Any(y => careers.Contains(y.CareerId)));
            }


            var data = await careerQuery
                .Select(x => new
                {
                    Name = x.Name,
                    Count = jobOffers.Where(y => y.JobOfferCareers.Any(z => z.CareerId == x.Id)).Count()
                })
                .ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }
    }
}
