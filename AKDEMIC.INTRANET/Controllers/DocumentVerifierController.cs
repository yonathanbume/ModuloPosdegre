using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.INTRANET.ViewModels.DocumentVerifierViewModels;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Controllers
{
    [AllowAnonymous]
    [Route("verificar-documento")]
    public class DocumentVerifierController : BaseController
    {
        private readonly IEvaluationReportService _evaluationReportService;
        private readonly ITeacherService _teacherService;
        private readonly ICourseService _courseService;
        private readonly ISectionService _sectionService;
        private readonly ICareerService _careerService;
        private readonly IFacultyService _facultyService;
        private readonly IDirectedCourseService _directedCourseService;
        private readonly ITeacherSectionService _teacherSectionService;
        private readonly IRecordHistoryService _recordHistoryService;
        private readonly AkdemicContext _context;
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;

        public DocumentVerifierController(IEvaluationReportService evaluationReportService,ITeacherService teacherService,
            ICourseService courseService, ISectionService sectionService, ICareerService careerService, IFacultyService facultyService,
            IDirectedCourseService directedCourseService, ITeacherSectionService teacherSectionService,
            IRecordHistoryService recordHistoryService,
            AkdemicContext context,
            IStudentService studentService, IUserService userService)
        {
            _evaluationReportService = evaluationReportService;
            _teacherService = teacherService;
            _courseService = courseService;
            _sectionService = sectionService;
            _careerService = careerService;
            _facultyService = facultyService;
            _directedCourseService = directedCourseService;
            _teacherSectionService = teacherSectionService;
            _recordHistoryService = recordHistoryService;
            _context = context;
            _studentService = studentService;
            _userService = userService;
        }

        /// <summary>
        /// Obtiene la vista para verificar actas
        /// </summary>
        /// <param name="id">Identificador del acta</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("actas")]
        public async Task<IActionResult> EvaluationReportVerifier(Guid id)
        {
            var act = await _evaluationReportService.GetEvaluationReportById(id);
            var model = new EvaluationReportViewModel();
            model.RelationalId = act.GeneratedId.ToString().PadLeft(6, '0');
            if (act.Type == ConstantHelpers.Intranet.EvaluationReportType.REGULAR)
            {
                var section = await _sectionService.GetSectionWithTermAndCareer(act.SectionId.Value);
                if (section == null)
                    section = await _teacherSectionService.GetTeacherSectionsWithTermAndCareer(act.SectionId.Value);

                var teacherSection = await _teacherSectionService.GetTeacherSectionBySection(act.SectionId.Value);
                model.Term = section.CourseTerm?.Term.Name;
                model.Teacher = teacherSection == null ? "-" : teacherSection.Teacher.User.FullName;
                model.Career = section.CourseTerm?.Course?.Career?.Name;
                model.Faculty = section.CourseTerm?.Course?.Career?.Faculty?.Name;
                model.Section = section.Code;
                model.Type = "General";
                model.Credits = section.CourseTerm?.Course?.Credits.ToString();
                model.ReceptionDate = act.Status == CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.RECEIVED ? act.ReceptionDate.ToLocalDateFormat() : "-";
                model.GeneratedDate = act.LastReportGeneratedDate.ToLocalDateTimeFormat();
            }
            else if(act.Type == ConstantHelpers.Intranet.EvaluationReportType.DIRECTED_COURSE)
            {
                var entity = await _directedCourseService.GetAllByTeacherIdAndCourseId(act.CourseId.Value, act.TeacherId);
                if (entity.Count() < 0)
                    return BadRequest("Error al encontrar el curso dirigido");

                var directedCourse = entity.ToArray();

                var teacher = await _userService.Get(act.TeacherId);
                var course = await _courseService.GetAsync(act.CourseId.Value);
                var career = await _careerService.Get(directedCourse[0].CareerId);
                var faculty = await _facultyService.Get(career.Id);
                var term = await _termService.Get(directedCourse[0].TermId);

                model.Term = term.Name;
                model.Teacher = teacher.FullName;
                model.Career = career.Name;
                model.Faculty = faculty.Name;
                model.Type = "Curso Dirigido";
                model.Credits = course.Credits.ToString();
                model.ReceptionDate = act.Status == CORE.Helpers.ConstantHelpers.Intranet.EvaluationReport.RECEIVED ? act.ReceptionDate.ToLocalDateFormat() : "-";
                model.GeneratedDate = act.LastReportGeneratedDate.ToLocalDateTimeFormat();
            }

            return View(model);
        }

        /// <summary>
        /// Obtiene la vista inicial para verificar constancias
        /// </summary>
        /// <param name="id">Identificador del historial de registro</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("constancias")]
        public async Task<IActionResult> ConstancyVerifier(Guid id)
        {
            var record = await _recordHistoryService.Get(id);
            var userProcedure = await _context.UserProcedures.Where(x => x.RecordHistoryId == record.Id).FirstOrDefaultAsync();

            var number = $"{record.Number.ToString().PadLeft(5, '0')}-{record.Date.Year}";
            var student = await _studentService.Get(record.StudentId.Value);
            var career = await _careerService.Get(student.CareerId);
            var faculty = await _facultyService.Get(career.FacultyId);
            var user = await _userService.GetUserByStudent(record.StudentId.Value);

            var model = new ConstancyViewModel
            {
                Type = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[record.Type],
                Career = career.Name,
                Faculty =faculty.Name,
                Student = user.FullName
            };

            if(userProcedure != null)
            {
                model.UserProcedureCode = userProcedure.Correlative;
                model.RequestDate = userProcedure.CreatedAt.ToLocalDateTimeFormat();
            }

            return View(model);
        }
    }

}
