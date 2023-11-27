using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Implementations
{
    public class CourseConferenceRespository : Repository<CourseConference>, ICourseConferenceRespository
    {
        public CourseConferenceRespository(AkdemicContext context) : base(context) { }
        public async Task<List<CourseConferenceTemplate>> GetCourseConferenceSiscoToHome()
        {
            var confere = await _context.CourseConferences.FirstOrDefaultAsync();
            var coursesConferences = await _context.CourseConferences
               .Select(x => new CourseConferenceTemplate
               {
                   Id = x.Id,
                   Date = x.StartDate.ToString("dddd, dd MMMM yyyy", new CultureInfo("es-PE")),
                   Time = x.StartDate.ToLocalTimeFormat(),
                   Title = x.Name,
                   Place = x.Location,
                   Url = x.Image
               }).ToListAsync();
            return coursesConferences;
        }

        public async Task<CourseConferenceTemplate> DetailCourseConferenceSiscoToHome(Guid id)
        {
            var confere = await _context.CourseConferences.FirstOrDefaultAsync();
            var courseConference = await _context.CourseConferences.Where(x => x.Id == id).FirstOrDefaultAsync();
            var model = new CourseConferenceTemplate
            {
                Id = courseConference.Id,
                Date = courseConference.StartDate.ToDefaultTimeZone().ToString("dddd, MMMM yyyy", new CultureInfo("es-PE")),
                Time = courseConference.StartDate.ToLocalTimeFormat(),
                Title = courseConference.Name,
                Place = courseConference.Location,
                Content = courseConference.Content,
                Url = courseConference.Image
            };
            return model;
        }

        public async Task<CourseConferenceHomeTemplate> GetCourseConference(Guid id)
        {
            var nowDate = DateTime.UtcNow.ToDefaultTimeZone().Date;

            var query = await _context.CourseConferences
                .Include(x => x.Exhibitors)
                .Include(x => x.Faculty)
                .Where(x => x.Id == id)
                .Where(x => x.StartDate >= nowDate)
                .ToListAsync();

            var course = query
                .Select(x => new CourseConferenceHomeTemplate
                {
                    Id = x.Id,
                    Name = x.Name,
                    CertificateDate = x.CertificateDate.ToString("dd/MM/yyyy"),
                    StartDate = x.StartDate.ToString("dd/MM/yyyy"),
                    Content = x.Content,
                    Duration = x.Duration,
                    Faculty = x.Faculty.Name,
                    Location = x.Location,
                    Modality = x.Modality,
                    Image = x.Image,
                    Presentation = x.Presentation,
                    Requirements = x.Requirements,
                    Vacancies = x.Vacancies.ToString(),
                    Exhibitors = x.Exhibitors.Select(y => new ExhibitorTemplate
                    {
                        Name = y.Responsible,
                        Organization = y.OriginOrganization,
                        Topic = y.Topic
                    }).ToList()
                }).First();
            return course;
        }

        public async Task<List<CourseConferenceHomeTemplate>> GetCourseConferences(int page)
        {
            var nowDate = DateTime.UtcNow.ToDefaultTimeZone().Date;

            var list = await _context.CourseConferences
                .Include(x => x.Faculty)
                .Where(x => x.StartDate >= nowDate)
                .OrderBy(x => x.StartDate)
                .Skip(5 * page)
                .Take(5)
                .ToListAsync();

            var result = list.Select(x => new CourseConferenceHomeTemplate
            {
                Id = x.Id,
                Name = x.Name,
                CertificateDate = x.CertificateDate.ToString("dd/MM/yyyy"),
                StartDate = x.StartDate.ToString("dd/MM/yyyy"),
                Content = x.Content,
                Duration = x.Duration,
                Faculty = x.Faculty.Name,
                Location = x.Location,
                Modality = x.Modality,
                Image = x.Image,
                Presentation = x.Presentation,
                Requirements = x.Requirements,
                Vacancies = x.Vacancies.ToString()
            }).ToList();
            return result;
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GerCourseConferenceDataTable(DataTablesStructs.SentParameters sentParameters, string search, Guid? careerId = null)
        {
            Expression<Func<CourseConference, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.StartDate); break;
                default:
                    orderByPredicate = ((x) => x.Name); break;
            }

            var query = _context.CourseConferences.AsNoTracking();

            if (careerId != null)
            {
                var facultyId = await _context.Careers.Where(x => x.Id == careerId).Select(x => x.FacultyId).FirstOrDefaultAsync();

                query = query.Where(x => x.FacultyId == facultyId);
            }

            int filteredcount = await query.CountAsync();

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    startDate = x.StartDate.ToString("dd/MM/yyyy"),
                    finalReport = x.FinalReportUrl,
                    hasFinalReport = string.IsNullOrEmpty(x.FinalReportUrl) ? false : true
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> ExistCourseConferenceName(string name, Guid? id = null)
        {
            var query = _context.CourseConferences.AsQueryable();
            if (id.HasValue)
            {
                return await query.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()) && x.Id != id.Value);
            }
            else
            {
                return await query.AnyAsync(x => x.Name.ToLower().Equals(name.ToLower()));
            }
        }
    }
}
