using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly AkdemicContext _context;

        public TeacherController(AkdemicContext context)
        {
            _context = context;
        }

        [HttpGet("cantidad-docentes-carga-academica-condición")]
        public async Task<IActionResult> GetTeachersWithAcademicChargeByCondition(string termName)
        {
            var query = _context.Teachers.AsNoTracking();

            if (string.IsNullOrEmpty(termName))
            {
                query = query.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE));
            }
            else
            {
                query = query.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.Term.Name == termName));
            }

            var data = await query
                .GroupBy(x => x.User.WorkerLaborInformation.WorkerLaborCondition.Name)
                .Select(x => new
                {
                    x.Key,
                    count = x.Count()
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("cantidad-docentes-carga-academica")]
        public async Task<IActionResult> GetTeachersWithAcademicCharge(string termName)
        {
            var query = _context.Teachers.AsNoTracking();

            if (string.IsNullOrEmpty(termName))
            {
                query = query.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE));
            }
            else
            {
                query = query.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.Term.Name == termName));
            }

            var teachers = await query.CountAsync();
            return Ok(teachers);
        }

        [HttpGet("cantidad-docentes-doctor-carga-academica")]
        public async Task<IActionResult> GetTeachersDoctoralDegreesWithAcademicCharge(string termName)
        {
            var query = _context.Teachers.Where(x => x.User.WorkerDoctoralDegrees.Any()).AsNoTracking();

            if (string.IsNullOrEmpty(termName))
            {
                query = query.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE));
            }
            else
            {
                query = query.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.Term.Name == termName));
            }


            var teachers = await query.CountAsync();
            return Ok(teachers);
        }

        [HttpGet("cantidad-docentes-tutores")]
        public async Task<IActionResult> GetTeachersTutorsWithAcademicCharge()
        {
            var teachers = await _context.Teachers.Where(x => x.User.Tutors.Any()).CountAsync();
            return Ok(teachers);
        }

        [HttpGet("cantidad-docentes-magister-carga-academica")]
        public async Task<IActionResult> GetTeacherMasterWithAcademicCharge(string termName)
        {
            var query = _context.Teachers.Where(x => x.User.WorkerMasterDegrees.Any()).AsNoTracking();

            if (string.IsNullOrEmpty(termName))
            {
                query = query.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE));
            }
            else
            {
                query = query.Where(x => x.TeacherSections.Any(y => y.Section.CourseTerm.Term.Name == termName));
            }

            var teachers = await query.CountAsync();
            return Ok(teachers);
        }

        [HttpGet("cantidad-docentes-renacyt")]
        public async Task<IActionResult> GetTeacherRENACYT()
        {
            var teachers = await _context.Teachers.Where(x => x.User.WorkerLaborInformation.HasRenacyt).CountAsync();
            return Ok(teachers);
        }

        [HttpGet("cantidad-docentes-concytec")]
        public async Task<IActionResult> GetTeacherCONCYTEC()
        {
            var teachers = await _context.Teachers.Where(x => x.User.WorkerLaborInformation.HasConcytec).CountAsync();
            return Ok(teachers);
        }
    }
}
