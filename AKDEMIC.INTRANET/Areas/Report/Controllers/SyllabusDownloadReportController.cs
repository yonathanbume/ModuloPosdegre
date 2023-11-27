using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + CORE.Helpers.ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Report")]
    [Route("reporte/descarga-silabo")]
    public class SyllabusDownloadReportController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly AkdemicContext _context;

        public SyllabusDownloadReportController(
            IDataTablesService dataTablesService,
            AkdemicContext context
            )
        {
            _dataTablesService = dataTablesService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("get-seciones-datatable")]
        public async Task<IActionResult> GetSectionsDatatable(Guid termId, Guid? careerId, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var query = _context.Sections.Where(x => x.CourseTerm.TermId == termId);

            if (careerId.HasValue)
                query = query.Where(x => x.CourseTerm.Course.AcademicYearCourses.Any(y => y.Curriculum.CareerId == careerId));

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.CourseTerm.Course.Name.ToLower().Contains(search.Trim().ToLower()) || x.CourseTerm.Course.Code.ToLower().Contains(search.Trim().ToLower()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    courseCode = x.CourseTerm.Course.Code,
                    courseName = x.CourseTerm.Course.Name,
                    x.Code,
                    enrolled = x.StudentSections.Count()
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };

            return Ok(result);
        }

        [HttpGet("get-datatable")]
        public async Task<IActionResult> GetStudentSectionsDatatable(Guid sectionId, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var query = _context.StudentSections.Where(x => x.SectionId == sectionId).AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Student.User.FullName.ToLower().Contains(search.Trim().ToLower()) || x.Student.User.UserName.ToLower().Contains(search.Trim().ToLower()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Student.User.FullName,
                    x.Student.User.UserName,
                    SyllabusDownloadDate = x.SyllabusDownloadDate.ToLocalDateTimeFormat(),
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };

            return Ok(result);
        }
    }
}
