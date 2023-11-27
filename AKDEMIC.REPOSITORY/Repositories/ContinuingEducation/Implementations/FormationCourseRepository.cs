using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Templates.FormationCourse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ContinuingEducation.Implementations
{
    public class FormationCourseRepository:Repository<ENTITIES.Models.ContinuingEducation.Course> , IFormationCourseRepository
    {      
        public FormationCourseRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByName(string name, Guid? id = null)
        {
            return await _context.FormationCourses.AnyAsync(x => x.Name.ToUpper() == name.ToUpper() && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllFormationCoursesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<ENTITIES.Models.ContinuingEducation.Course, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = (x) => x.CourseType.Name;
                    break;
                case "2":
                    orderByPredicate = ((x) => x.CourseArea.Name);
                    break;
            }

            var query = _context.FormationCourses.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    CourseArea = x.CourseArea.Name,
                    CourseType = x.CourseType.Name,
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

        public async Task<FormationCourseTemplate> GetInformation(Guid id)
        {
            var query = _context.FormationCourses.Where(x => x.Id == id).AsNoTracking();

            var data = await query
                .Select(x => new FormationCourseTemplate
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Requirement = x.Requirement,
                    Place = x.Place,
                    Investment = x.Investment,
                    Duration = x.Duration,
                    Modality = x.Modality,
                    OrganizerEntity = x.OrganizerEntity,
                    OrganizerEntityText = ConstantHelpers.CONTINUING_EDUCATION.COURSES_ORGANIZER_ENTITY.VALUES.ContainsKey(x.OrganizerEntity) ?
                        ConstantHelpers.CONTINUING_EDUCATION.COURSES_ORGANIZER_ENTITY.VALUES[x.OrganizerEntity] : "",
                    CourseAreaId = x.CourseAreaId,
                    CourseAreaName = x.CourseArea.Name,
                    CourseTypeId = x.CourseTypeId,
                    CourseTypeName = x.CourseType.Name,
                    EntityId = x.EntityId,
                    ImageUrl = x.ImageUrl,
                    PresentationUrl = x.PresentationUrl
                })
                .FirstOrDefaultAsync();

            switch (data.OrganizerEntity)
            {
                case ConstantHelpers.CONTINUING_EDUCATION.COURSES_ORGANIZER_ENTITY.FACULTY:
                    var faculty = await _context.Faculties.Where(x => x.Id == data.EntityId).FirstOrDefaultAsync();
                    if (faculty != null)
                        data.OrganizerEntityText = faculty.Name;
                    break;
                case ConstantHelpers.CONTINUING_EDUCATION.COURSES_ORGANIZER_ENTITY.CAREER:
                    var career = await _context.Careers.Where(x => x.Id == data.EntityId).FirstOrDefaultAsync();
                    if (career != null)
                        data.OrganizerEntityText = career.Name;
                    break;
                case ConstantHelpers.CONTINUING_EDUCATION.COURSES_ORGANIZER_ENTITY.ACADEMICDEPARTMENT:
                    var academicDepartment = await _context.AcademicDepartments.Where(x => x.Id == data.EntityId).FirstOrDefaultAsync();
                    if (academicDepartment != null)
                        data.OrganizerEntityText = academicDepartment.Name;
                    break;
                default:
                    break;
            }

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<ENTITIES.Models.ContinuingEducation.Course, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = (x) => x.CourseType.Name;
                    break;
                case "2":
                    orderByPredicate = ((x) => x.CourseArea.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Sections.Count());
                    break;
            }

            var query = _context.FormationCourses.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    CourseArea = x.CourseArea.Name,
                    CourseType = x.CourseType.Name,
                    SectionsCount = x.Sections.Count()
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
    }
}
