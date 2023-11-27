using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class TeacherPortfolioRepository : Repository<TeacherPortfolio>, ITeacherPortfolioRepository
    {
        public TeacherPortfolioRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<object> GetPortfolioDatatable(DataTablesStructs.SentParameters parameters, string teacherId, byte folder, string search)
        {
            var query = _context.TeacherPortfolios
                .Where(x=> x.Folder == folder && x.TeacherId == teacherId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(search.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    CreatedAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    x.Id,
                    x.Name,
                    x.Folder,
                    x.File
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetPortfolioCurriculumDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string teacherId, string search)
        {
            var coursesId = await _context.TeacherSections.Where(x => x.Section.CourseTerm.TermId == termId && x.TeacherId == teacherId).Select(x => x.Section.CourseTerm.CourseId).ToListAsync();
            var query = _context.Curriculums.Where(x => x.AcademicYearCourses.Any(y => coursesId.Contains(y.CourseId))).AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Code.Trim().ToLower().Contains(search.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    Career = x.Career.Name,
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetPortfolioCourseSyllabusDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string teacherId, string search)
        {
            var coursesId = await _context.TeacherSections.Where(x => x.Section.CourseTerm.TermId == termId && x.TeacherId == teacherId).Select(x => x.Section.CourseTerm.CourseId).ToListAsync();
            var query = _context.SyllabusTeachers.Where(x => x.CourseTerm.TermId == termId && coursesId.Contains(x.CourseTerm.CourseId) && x.Status == ConstantHelpers.SYLLABUS_TEACHER.STATUS.PRESENTED).AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.CourseTerm.Course.Name.Trim().ToLower().Contains(search.ToLower().Trim()) || x.CourseTerm.Course.Code.Trim().ToLower().Contains(search.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    PresentationDate = x.PresentationDate.ToLocalDateTimeFormat(),
                    x.CourseTerm.Course.Code,
                    x.CourseTerm.Course.Name,
                    x.Url
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetPortfolioCurricularDesignDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string teacherId, string search)
        {
            var coursesId = await _context.TeacherSections.Where(x => x.Section.CourseTerm.TermId == termId && x.TeacherId == teacherId).Select(x => x.Section.CourseTerm.CourseId).ToListAsync();
            var query = _context.Curriculums.Where(x => x.AcademicYearCourses.Any(y => coursesId.Contains(y.CourseId)) && !string.IsNullOrEmpty(x.CurricularDesignFile)).AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Code.Trim().ToLower().Contains(search.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    Career = x.Career.Name,
                    File = x.CurricularDesignFile
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
