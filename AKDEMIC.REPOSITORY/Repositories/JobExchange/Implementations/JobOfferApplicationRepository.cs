using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
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
    public class JobOfferApplicationRepository : Repository<JobOfferApplication>, IJobOfferApplicationRepository
    {
        public JobOfferApplicationRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<JobOfferApplication>> GetAllByStudent(Guid studentId)
        {
            var query = _context.JobOfferApplications
                .Where(x => x.StudentId == studentId);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<JobOfferApplication>> GetAllWithIncludesByStudent(Guid studentId)
        {
            var query = _context.JobOfferApplications
                .Include(x => x.JobOffer.Company.User)
                .Include(x => x.Student.User)
                .Include(x => x.JobOffer.Company.Sector)
                .Where(x => x.StudentId == studentId);

            return await query.ToListAsync();
        }

        public async Task<JobOfferApplication> GetByJobOfferAndStudent(Guid jobOfferId, Guid studentId)
        {
            return await _context.JobOfferApplications.FindAsync(jobOfferId, studentId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetByJobOfferDatatable(DataTablesStructs.SentParameters sentParameters,List<Guid>careers, Guid? jobOfferId, DateTime startDate, DateTime endDate, string searchValue = null)
        {
            Expression<Func<JobOfferApplication, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.JobOfferId);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.StudentId);
                    break;
                default:
                    orderByPredicate = ((x) => x.JobOfferId);
                    break;
            }

            var query = _context.JobOfferApplications
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsQueryable();

            if (careers!= null && !careers.Contains(Guid.Empty) && careers.Count() >0)
            {
                var client = query.Select(x => new
                {
                    x.JobOfferId,
                    careers = x.JobOffer.JobOfferCareers.Select(y => y.CareerId).ToList()
                }).ToList();

                var ids = client.Where(x => x.careers.Any(y => careers.Contains(y))).Select(x => x.JobOfferId).ToList();
                query = query.Where(x => ids.Contains(x.JobOfferId));
            }
            if (jobOfferId.HasValue && jobOfferId != Guid.Empty)
            {
                query = query.Where(x => x.JobOfferId == jobOfferId);
            }
            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Student.User.UserName.ToLower().Contains(searchValue.ToLower()) || x.Student.User.FullName.ToLower().Contains(searchValue.ToLower()));
            }
            if (startDate != DateTime.MinValue)
            {
                query = query.Where(x => x.Date >= startDate);
            }
            if (endDate != DateTime.MinValue)
            {
                query = query.Where(x => x.Date <= endDate);
            }
            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.StudentId,
                    code = x.Student.User.UserName,
                    name = x.Student.User.FullName,
                    career = x.Student.Career.Name,
                    year = x.Student.CurrentAcademicYear,
                    meritType = ConstantHelpers.ACADEMIC_ORDER.VALUES.ContainsKey(x.Student.CurrentMeritType) ? ConstantHelpers.ACADEMIC_ORDER.VALUES[x.Student.CurrentMeritType] : "--",
                    status = ConstantHelpers.JobOfferApplication.Status.VALUES[x.Status],
                    statusType = x.Status,
                    jobOfferId = x.JobOfferId,
                    studentId = x.StudentId,
                    location = x.JobOffer.Location,
                    company = (x.JobOffer.Company.User.PaternalSurname != null && x.JobOffer.Company.User.MaternalSurname != null) ? x.JobOffer.Company.User.FullName : x.JobOffer.Company.User.Name,
                    hascv = x.Student.CurriculumVitaes.Select(y => y.File != null).FirstOrDefault(),
                    cv = x.Student.CurriculumVitaes.Select(y => y.File).FirstOrDefault()
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
        public async Task<List<JobOfferApplicationTemplate>> GetByJobOfferData(Guid? jobOfferId, DateTime startDate, DateTime endDate, string searchValue = null)
        {
            var query = _context.JobOfferApplications
     .AsQueryable();

            if (jobOfferId.HasValue && jobOfferId != Guid.Empty)
            {
                query = query.Where(x => x.JobOfferId == jobOfferId);
            }
            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Student.User.UserName.ToLower().Contains(searchValue.ToLower()) || x.Student.User.FullName.ToLower().Contains(searchValue.ToLower()));
            }
            if (startDate != DateTime.MinValue)
            {
                query = query.Where(x => x.Date >= startDate);
            }
            if (endDate != DateTime.MinValue)
            {
                query = query.Where(x => x.Date <= endDate);
            }
            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            var data = await query
                .Select(x => new JobOfferApplicationTemplate
                {
                    id = x.StudentId,
                    code = x.Student.User.UserName,
                    name = x.Student.User.FullName,
                    career = x.Student.Career.Name,
                    year = x.Student.CurrentAcademicYear,
                    meritType = ConstantHelpers.ACADEMIC_ORDER.VALUES.ContainsKey(x.Student.CurrentMeritType) ? ConstantHelpers.ACADEMIC_ORDER.VALUES[x.Student.CurrentMeritType] : "--",
                    status = ConstantHelpers.JobOfferApplication.Status.VALUES[x.Status],
                    statusType = x.Status,
                    jobOfferId = x.JobOfferId,
                    studentId = x.StudentId,
                    location = x.JobOffer.Location,
                    company = (x.JobOffer.Company.User.PaternalSurname != null && x.JobOffer.Company.User.MaternalSurname != null) ? x.JobOffer.Company.User.FullName : x.JobOffer.Company.User.Name,
                    hascv = _context.CurriculumVitaes.Where(y => y.StudentId == x.StudentId).FirstOrDefault().File != null
                })
                .ToListAsync();

            return data;
        }

        public async Task<object> GetJobOfferApplicationSelect2ClientSide(Guid companyId, Guid? careerId = null)
        {
            var query = _context.JobOfferApplications
                .Where(x => x.JobOffer.CompanyId == companyId && x.Status == ConstantHelpers.JobOfferApplication.Status.ACCEPTED)
                .AsQueryable();

            if (careerId != null)
                query = query.Where(x => x.Student.CareerId == careerId);

            var result = await query
                .Select(x => new
                {
                    id = x.StudentId,
                    text = x.Student.User.FullName
                })
                .Distinct()
                .OrderBy(x => x.text)
                .ToListAsync();

            return result;
        }

        public async Task<List<JobOfferApplicationAgreementReportTemplate>> GetApplicationWithAgreementReportData()
        {

            var result = await _context.JobOfferApplications
                .Where(x => x.JobOffer.Company.AgreementId != null)
                .Select(x => new JobOfferApplicationAgreementReportTemplate
                {
                    UserName = x.Student.User.UserName,
                    FullName = x.Student.User.FullName,
                    Career = x.Student.Career.Name,
                    Agreement = x.JobOffer.Company.Agreement.Name,
                    Company = x.JobOffer.Company.User.Name,
                    Status = ConstantHelpers.JobOfferApplication.Status.VALUES.ContainsKey(x.Status)?
                        ConstantHelpers.JobOfferApplication.Status.VALUES[x.Status] : "",
                    ApplicationDate = x.Date.ToLocalDateFormat(),
                    JobOfferEndDate = x.JobOffer.EndDate.ToLocalDateFormat(),
                    JobOfferStartDate = x.JobOffer.StartDate.ToLocalDateFormat()
                })
                .ToListAsync();

            return result;
        }

        public async Task<List<JobOfferApplication>> GetJobOfferApplications(Guid jobOfferId)
        {
            var result = await _context.JobOfferApplications.Where(x => x.JobOfferId == jobOfferId)
                .ToListAsync();

            return result;
        }

        public async Task<object> GetJobExchangeStudentWorkTypeReportChart(Guid? facultyId = null, List<Guid> careers = null)
        {
            var categoriesData = new List<string>();
            var seriesData = new List<int>();

            var jobOfferApplicationsQuery = _context.JobOfferApplications.Where(x => x.Status == ConstantHelpers.JobOfferApplication.Status.ACCEPTED).AsNoTracking();

            if (facultyId != null)
            {
                jobOfferApplicationsQuery = jobOfferApplicationsQuery.Where(x => x.Student.Career.FacultyId == facultyId);
            }

            if (careers != null && careers.Count > 0)
            {
                jobOfferApplicationsQuery = jobOfferApplicationsQuery.Where(x => careers.Contains(x.Student.CareerId));
            }

            var partTimeCount = await jobOfferApplicationsQuery.Where(x => x.JobOffer.WorkType == ConstantHelpers.JobOffer.WorkType.PART_TIME).Select(x => x.StudentId).Distinct().CountAsync();
            var fullTimeCount = await jobOfferApplicationsQuery.Where(x => x.JobOffer.WorkType == ConstantHelpers.JobOffer.WorkType.FULL_TIME).Select(x => x.StudentId).Distinct().CountAsync();

            return new
            {
                categoriesData = new List<string> 
                { 
                    ConstantHelpers.JobOffer.WorkType.VALUES[ConstantHelpers.JobOffer.WorkType.PART_TIME] ,
                    ConstantHelpers.JobOffer.WorkType.VALUES[ConstantHelpers.JobOffer.WorkType.FULL_TIME] ,
                },
                seriesData = new List<int>
                {
                    partTimeCount,
                    fullTimeCount
                }
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentWorkTypeReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, List<Guid> careers = null)
        {
            var categoriesData = new List<string>();
            var seriesData = new List<int>();

            var jobOfferApplicationsQuery = _context.JobOfferApplications.Where(x => x.Status == ConstantHelpers.JobOfferApplication.Status.ACCEPTED).AsNoTracking();

            if (facultyId != null)
            {
                jobOfferApplicationsQuery = jobOfferApplicationsQuery.Where(x => x.Student.Career.FacultyId == facultyId);
            }

            if (careers != null && careers.Count > 0)
            {
                jobOfferApplicationsQuery = jobOfferApplicationsQuery.Where(x => careers.Contains(x.Student.CareerId));
            }

            var partTimeCount = await jobOfferApplicationsQuery.Where(x => x.JobOffer.WorkType == ConstantHelpers.JobOffer.WorkType.PART_TIME).Select(x => x.StudentId).Distinct().CountAsync();
            var fullTimeCount = await jobOfferApplicationsQuery.Where(x => x.JobOffer.WorkType == ConstantHelpers.JobOffer.WorkType.FULL_TIME).Select(x => x.StudentId).Distinct().CountAsync();

            var data = new List<dynamic>
            {
                new { 
                    name = ConstantHelpers.JobOffer.WorkType.VALUES[ConstantHelpers.JobOffer.WorkType.PART_TIME],
                    count = partTimeCount
                },
                new {
                    name = ConstantHelpers.JobOffer.WorkType.VALUES[ConstantHelpers.JobOffer.WorkType.FULL_TIME],
                    count = fullTimeCount
                },
            };

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetJobExchangeJobOfferApplicationCareerReportChart(List<Guid> careers = null)
        {
            var categoriesData = new List<string>();
            var seriesData = new List<int>();

            var careerQuery = _context.Careers.AsNoTracking();
            var jobOfferApplications = _context.JobOfferApplications.AsNoTracking();

            if (careers != null && careers.Count > 0)
            {
                careerQuery = careerQuery.Where(x => careers.Contains(x.Id));
                jobOfferApplications = jobOfferApplications.Where(x => careers.Contains(x.Student.CareerId));
            }

            var data = await careerQuery
                .Select(x => new
                {
                    Name = x.Name,
                    Count = jobOfferApplications.Where(y => y.Student.CareerId == x.Id).Count()
                })
                .ToListAsync();

            return new
            {
                categoriesData = data.Select(x => x.Name).ToList(),
                seriesData = data.Select(x => x.Count).ToList()
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeJobOfferApplicationCareerReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers = null)
        {
            var categoriesData = new List<string>();
            var seriesData = new List<int>();

            var careerQuery = _context.Careers.AsNoTracking();
            var jobOfferApplications = _context.JobOfferApplications.AsNoTracking();

            if (careers != null && careers.Count > 0)
            {
                careerQuery = careerQuery.Where(x => careers.Contains(x.Id));
                jobOfferApplications = jobOfferApplications.Where(x => careers.Contains(x.Student.CareerId));
            }

            var data = await careerQuery
                .Select(x => new
                {
                    Name = x.Name,
                    Count = jobOfferApplications.Where(y => y.Student.CareerId == x.Id).Count()
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
