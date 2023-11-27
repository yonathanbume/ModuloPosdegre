using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class StudentExperienceRepository : Repository<StudentExperience>, IStudentExperienceRepository
    {
        public StudentExperienceRepository(AkdemicContext context) : base(context) { }

        public async Task<StudentExperience> FirstOrDefaultById(Guid id)
        {
            var result = _context.StudentExperiences.Include(x=>x.Student).Where(x => x.Id == id);
            return await result.FirstOrDefaultAsync();
        }

        public async Task<List<ExperienceDate>> GetAllByStudentTemplate(Guid studentId)
        {
            var result = await _context.StudentExperiences
                .Where(x => x.StudentId == studentId)
                .Select(x => new ExperienceDate
                {
                    Description = (x.CompanyId.HasValue) ? x.Company.User.Name : x.CompanyName,
                    RangeDate = (x.CurrentWork) ? $"{x.StartDate.ToLocalDateFormat()} - Hasta la actualidad " : $"{x.StartDate.ToLocalDateFormat()} - {x.EndDate.ToLocalDateFormat()}",
                    Position = x.Position
                })
                .ToListAsync();

            return result;
        }

        public async Task<StudentExperience> GetLastByStartDate()
        {
            var query = _context.StudentExperiences
                .OrderBy(x => x.StartDate);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<object> GetStudentExperiencesByStudent(Guid studentId)
        {
            var result = await _context.StudentExperiences
                .Where(x => x.StudentId == studentId)
                .Select(x => new
                {
                    x.Id,
                    CompanyName = x.CompanyId == null ? x.CompanyName ?? "" : x.Company.User.FullName,
                    x.Area,
                    x.Description,
                    x.CurrentWork,
                    StartDate = x.StartDate.ToLocalDateFormat(),
                    EndDate = x.EndDate == null ? "" : x.EndDate.ToLocalDateFormat(),
                    Image = (x.CompanyId != null && !String.IsNullOrEmpty(x.Company.User.Picture)) ? "/imagenes/" +x.Company.User.Picture : $@"\images\demo\company.jpg"
                })
                .ToListAsync();

            return result;
        }

        public async Task<object> GetStudentExperiencesById(Guid id)
        {
            var result = _context.StudentExperiences.Include(x => x.Company).Where(x => x.Id == id).
                Select(x => new
                {
                    id = x.Id,
                    companyName = x.CompanyId.HasValue ? null : x.CompanyName,
                    position = x.Position,
                    area = x.Area,
                    description = x.Description,
                    enterprise = x.CompanyId.HasValue ? x.CompanyId : null,
                    startDate = x.StartDate.ToLocalDateFormat(),
                    endDate = x.EndDate.ToLocalDateFormat(),
                    x.IsPrivate,
                    currentWork = x.CurrentWork
                });
            return await result.FirstOrDefaultAsync();
        }

        public async Task<object> GetStudentExperiencePersonalized(Guid id)
        {
            var result = _context.StudentExperiences.Include(x => x.Company).Where(x => x.Id == id).
                Select(x => new
                {
                    id = x.Id,
                    companyName = x.CompanyId.HasValue ? (!String.IsNullOrEmpty(x.Company.User.MaternalSurname) && !String.IsNullOrEmpty(x.Company.User.PaternalSurname)) ? x.Company.User.FullName : x.Company.User.Name : x.CompanyName,
                    position = x.Position,
                    area = x.Area,
                    description = x.Description,
                    startDate = x.StartDate.ToLocalDateFormat(),
                    endDate = x.EndDate.HasValue ? x.EndDate.ToLocalDateFormat() : "Hasta la actualidad",
                    currentWork = x.CurrentWork
                });
            return await result.FirstOrDefaultAsync();
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentWorkingDatatable(DataTablesStructs.SentParameters sentParameters, Guid? companyId, Guid? careerId, string searchValue = null)
        {
            var query = _context.StudentExperiences.Where(x => x.CompanyId == companyId)
                .Where(x=>x.CurrentWork == true)
                 .AsNoTracking();

            if (careerId.HasValue)
            {
                query = query.Where(x => x.Student.CareerId == careerId);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Company.User.Name.Contains(searchValue) || x.Student.User.FullName.Contains(searchValue) || x.Student.Career.Name.Contains(searchValue));
            }         

            Expression<Func<StudentExperience, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                x.StudentId,
                x.Student.User.FullName,
                Career = x.Student.Career.Name
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);

        }

        public async Task<bool> ExistStudentExperienceByCompany(Guid companyId, Guid StudentId)
        {
             return await _context.StudentExperiences.AnyAsync(x => x.StudentId == StudentId && x.CompanyId == companyId && x.CurrentWork == true);
        }

        public async Task<StudentExperience> GetYearWorking()
            => await _context.StudentExperiences.OrderBy(x => x.StartDate).FirstOrDefaultAsync();

        public async Task<object> GetStudentExperiencesByCompanyIdSelect2ClientSide(Guid companyId)
        {
            var result = await _context.StudentExperiences.Where(x => x.CompanyId == companyId)
                .Select(x => new
                {
                    id = x.StudentId,
                    text = x.Student.User.FullName
                })
                .OrderBy(x => x.text)
                .Distinct()
                .ToListAsync();

            return result;
        }

        public async Task<object> GetWorkingBachelorsChart(string startSearchDate = null, string endSearchDate = null)
        {
            //Todas las experiencias de trabajo de los estudiantes
            var query = _context.StudentExperiences
                .Where(x => x.CurrentWork)
                .AsQueryable();

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

            var bachellorWorking = await query.Where(x => x.Student.Status == ConstantHelpers.Student.States.BACHELOR).Select(x => x.StudentId).Distinct().CountAsync();
            var notBachellorWorking = await query.Select(x => x.StudentId).Distinct().CountAsync();

            var result = new
            {
                categories = new List<string>() { "Bachiller", "No Bachiller" },
                data = new List<int>() { bachellorWorking, notBachellorWorking }
            };

            return result;
        }

        public async Task<object> GetJobExchangeStudentExperienceCareerReportChart(List<Guid> careers = null)
        {
            var categoriesData = new List<string>();
            var seriesData = new List<int>();

            var careersQuery = _context.Careers.AsNoTracking();
            var studentExperiencesQuery = _context.StudentExperiences.Where(x => x.CurrentWork).AsNoTracking();


            if (careers != null && careers.Count > 0)
            {
                careersQuery = careersQuery.Where(x => careers.Contains(x.Id));
                studentExperiencesQuery = studentExperiencesQuery.Where(x => careers.Contains(x.Student.CareerId));
            }

            var data = await careersQuery
                .Select(x => new
                {
                    Name = x.Name,
                    Count = studentExperiencesQuery.Where(y => y.Student.CareerId == x.Id).Count()
                })
                .ToListAsync();

            return new
            {
                categoriesData = data.Select(x => x.Name).ToList(),
                seriesData = data.Select(x => x.Count).ToList(),
            };

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentExperienceCareerReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers = null)
        {
            var careersQuery = _context.Careers.AsNoTracking();
            var studentExperiencesQuery = _context.StudentExperiences.Where(x => x.CurrentWork).AsNoTracking();


            if (careers != null && careers.Count > 0)
            {
                careersQuery = careersQuery.Where(x => careers.Contains(x.Id));
                studentExperiencesQuery = studentExperiencesQuery.Where(x => careers.Contains(x.Student.CareerId));
            }

            var data = await careersQuery
                .Select(x => new
                {
                    Name = x.Name,
                    Count = studentExperiencesQuery.Where(y => y.Student.CareerId == x.Id).Count()
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

        public async Task<object> GetJobExchangeStudentExperienceSectorReportChart()
        {
            var categoriesData = new List<string>();
            var seriesData = new List<int>();

            var studentExperiencesQuery = _context.StudentExperiences.AsNoTracking();

            var privateSectorCount = await studentExperiencesQuery.Where(x => x.IsPrivate).Select(x => x.StudentId).Distinct().CountAsync();
            var publicSectorCount = await studentExperiencesQuery.Where(x => !x.IsPrivate).Select(x => x.StudentId).Distinct().CountAsync();

            return new
            {
                categoriesData = new List<string>
                {
                    "Sector Privado" ,
                    "Sector Público" ,
                },
                seriesData = new List<int>
                {
                    privateSectorCount,
                    publicSectorCount
                }
            };
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentExperienceSectorReportDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            var categoriesData = new List<string>();
            var seriesData = new List<int>();

            var studentExperiencesQuery = _context.StudentExperiences.AsNoTracking();

            var privateSectorCount = await studentExperiencesQuery.Where(x => x.IsPrivate).Select(x => x.StudentId).Distinct().CountAsync();
            var publicSectorCount = await studentExperiencesQuery.Where(x => !x.IsPrivate).Select(x => x.StudentId).Distinct().CountAsync();

            var data = new List<dynamic>
            {
                new {
                    name = "Sector Privado" ,
                    count = privateSectorCount
                },
                new {
                    name = "Sector Público" ,
                    count = publicSectorCount
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
    }
}
