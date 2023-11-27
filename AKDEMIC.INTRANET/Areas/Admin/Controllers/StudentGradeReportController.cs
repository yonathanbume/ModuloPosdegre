using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.CORE.Helpers;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/reporte-notas")]
    public class StudentGradeReportController : BaseController
    {
        public StudentGradeReportController(AkdemicContext context) : base(context) { }

        /// <summary>
        /// Vista donde se muestra el reporte estadístico de notas por facultad y escuelas profesionales
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene la cantidad de promedios promedios agrupados por rango de notas
        /// </summary>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <param name="pid">Identificador del programa académico</param>
        /// <param name="tid">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene los datos para ser usado en el reporte</returns>
        [Route("carrera")]
        public async Task<IActionResult> GetReportForFacultyAndCareer(Guid cid, Guid pid, Guid tid)
        {
            if (tid == Guid.Empty)
                tid = (await _context.Terms.FirstOrDefaultAsync(x => x.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.ACTIVE)).Id;

            var program = await _context.AcademicPrograms.Where(x => x.Id == pid).FirstOrDefaultAsync();
            if (program == null) return Ok();

            var result = await _context.Terms
                .Where(x => x.Id == tid)
                .Select(x => new object[]
                {
                    x.Name,
                    x.AcademicSummaries.Count(acs => acs.CareerId == cid && acs.Student.AcademicProgramId == pid && acs.TermId == x.Id && acs.WeightedAverageGrade >= 0 && acs.WeightedAverageGrade < 5),
                    x.AcademicSummaries.Count(acs => acs.CareerId == cid && acs.Student.AcademicProgramId == pid && acs.TermId == x.Id && acs.WeightedAverageGrade >= 5 && acs.WeightedAverageGrade < 10),
                    x.AcademicSummaries.Count(acs => acs.CareerId == cid && acs.Student.AcademicProgramId == pid && acs.TermId == x.Id && acs.WeightedAverageGrade >= 10 && acs.WeightedAverageGrade < 15),
                    x.AcademicSummaries.Count(acs => acs.CareerId == cid && acs.Student.AcademicProgramId == pid && acs.TermId == x.Id && acs.WeightedAverageGrade >= 15 && acs.WeightedAverageGrade <= 20),
                }).ToArrayAsync();

            return Ok(result);
        }

        /// <summary>
        /// Obtiene la cantidad de promedios finales por curso agrupados por rango de notas
        /// </summary>
        /// <param name="cid">Identificador de la escuela profesional</param>
        /// <param name="ccid">Identificador del curso</param>
        /// <param name="tid">Identificador del periodo académico</param>
        /// <param name="planId">Identificador del plan de estudio</param>
        /// <returns>Objeto que contiene los datos para ser usado en el reporte</returns>
        [Route("curso")]
        public async Task<IActionResult> GetReportForCourse(Guid cid, Guid ccid, Guid tid, Guid planId)
        {
            if (tid == Guid.Empty)
                tid = (await _context.Terms.FirstOrDefaultAsync(x => x.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.ACTIVE)).Id;

            var terms = await _context.Terms
              .Where(x => x.Id == tid)
              .Select(x => new
              {
                  x.Name
              }).ToListAsync();

            var studentSectionsQuery = await _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == tid && x.Section.CourseTerm.CourseId == ccid && x.Student.CareerId == cid)
                .Select(x => new
                {
                    x.FinalGrade,
                    curriculums = x.Section.CourseTerm.Course.Career.Curriculums.Select(y => y.Id).ToList()
                })
                .ToArrayAsync();
            var studentSections = studentSectionsQuery
                        .Where(x => x.curriculums.Contains(planId))
                        .Select(x => x.FinalGrade)
                        .ToArray();
            var result = terms
                .Select(x => new object[]
                {
                    x.Name,
                    studentSections.Where(g => g >= 0 && g < 5).Count(),
                    studentSections.Where(g => g >= 5 && g < 10).Count(),
                    studentSections.Where(g => g >= 10 && g < 15).Count(),
                    studentSections.Where(g => g >= 15 && g <= 20).Count()
                }).ToArray();

            return Ok(result);

        }
    }
}
