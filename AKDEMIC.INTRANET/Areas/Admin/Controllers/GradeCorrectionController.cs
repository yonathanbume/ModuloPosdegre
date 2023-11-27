using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.INTRANET.Areas.Admin.Models.GradeCorrectionViewModels;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using System.IO;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Data;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles =
        ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
        ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," +
        ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/correccion-notas")]
    public class GradeCorrectionController : BaseController
    {
        private readonly IGradeCorrectionService _gradeCorrectionService;
        private readonly IGradeService _gradeService;
        private readonly ISectionService _sectionService;
        private readonly ICourseTermService _courseTermService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IEvaluationService _evaluationService;
        private readonly ISubstituteExamCorrectionService _substituteExamCorrectionService;
        private readonly ISubstituteExamService _substituteExamService;
        private readonly IConfigurationService _configurationService;
        private readonly AkdemicContext _context;
        private readonly ITermService _termService;

        public GradeCorrectionController(
            IGradeCorrectionService gradeCorrectionService,
            IOptions<CloudStorageCredentials> storageCredentials,
            IGradeService gradeService,
            ISectionService sectionService,
            ICourseTermService courseTermService,
            IStudentSectionService studentSectionService,
            IEvaluationService evaluationService,
            IDataTablesService dataTablesService,
            AkdemicContext context,
            ITermService termService,
            ISubstituteExamCorrectionService substituteExamCorrectionService,
            ISubstituteExamService substituteExamService,
            IConfigurationService configurationService
         ) : base(dataTablesService)
        {
            _context = context;
            _termService = termService;
            _gradeCorrectionService = gradeCorrectionService;
            _gradeService = gradeService;
            _sectionService = sectionService;
            _courseTermService = courseTermService;
            _studentSectionService = studentSectionService;
            _storageCredentials = storageCredentials;
            _evaluationService = evaluationService;
            _substituteExamCorrectionService = substituteExamCorrectionService;
            _substituteExamService = substituteExamService;
            _configurationService = configurationService;
        }

        /// <summary>
        /// Vista donde se gestionan las solicitudes de corrección de notas
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR))
            {
                var careerDirectorGradeCorrection = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.CAREER_DIRECTOR_GRADE_CORRECTION));
                if (!careerDirectorGradeCorrection)
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if (User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
            {
                var academicDepartmentGradeCorrection = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.ACADEMIC_DEPARTMENT_GRADE_CORRECTION));
                if (!academicDepartmentGradeCorrection)
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View();
        }

        /// <summary>
        /// Obtiene el listado de solicitudes de de correcion de notas
        /// </summary>
        /// <param name="id">Identificador del periodo académico</param>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <param name="state">Estado</param>
        /// <returns>Objeto que contiene el listado de solicitudes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetGradeCorrections(Guid id, string searchValue, int? state = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _gradeCorrectionService.GetAllByRoleDatatable(sentParameters, id, searchValue, state, User);

            return Ok(result);
        }

        /// <summary>
        /// Vista donde se gestionan las correcciones de sustitutorio
        /// </summary>
        /// <returns>Vista sustitutorio</returns>
        [HttpGet("sustitutorio")]
        public IActionResult SubstituteExamCorrection() => View();

        /// <summary>
        /// Obtiene el listado de solicitudes de correción de sustitutorio
        /// </summary>
        /// <param name="id">Identificador del periodo académico</param>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <param name="state">Estado</param>
        /// <returns>Objeto que contiene el listado de solicitudes</returns>
        [HttpGet("get-sustitutorio")]
        public async Task<IActionResult> GetSubstituteExamCorrections(Guid id, string searchValue, int? state = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _substituteExamCorrectionService.GetAllDatatable(sentParameters, null, id, searchValue, state);

            return Ok(result);
        }

        /// <summary>
        /// Método para calificar la corrección de nota
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de corrección de notas</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("post")]
        public async Task<IActionResult> ChangeState(CorrectionViewModel model)
        {
            var gradeCorrection = await _gradeCorrectionService.Get(model.Id);
            if (gradeCorrection == null) return BadRequest();

            if (gradeCorrection.State != ConstantHelpers.GRADECORRECTION_STATES.PENDING)
                return BadRequest("La corrección ya fue aprobada o desaprobada anteriormente.");

            var grade = await _gradeService.Get(gradeCorrection.GradeId);

            var studentSection = await _studentSectionService.Get(grade.StudentSectionId);

            var section = await _sectionService.Get(studentSection.SectionId);
            var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
            var term = await _termService.Get(courseTerm.TermId);

            if (term.Status != ConstantHelpers.TERM_STATES.ACTIVE)
                return BadRequest("No se puede cambiar el estado de solicitudes de periodos académicos finalizados.");

            var anySubstituteExams = await _substituteExamService.AnySubstituteExamByStudent(studentSection.StudentId, studentSection.SectionId, ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED);

            if (anySubstituteExams)
            {
                return BadRequest("El estudiante tiene un examen sustitutorio pendiente.");
            }

            if (model.Status)
            {
                gradeCorrection.State = ConstantHelpers.GRADECORRECTION_STATES.APPROBED;

                grade.Value = gradeCorrection.NewGrade.Value;
                grade.Attended = true;
                grade.CreatorIP = gradeCorrection.CreatorIP;
                grade.Attended = true;
                grade.Attended = !gradeCorrection.NotTaken;

                await _gradeService.Update(grade);
                await _studentSectionService.RecalculateFinalGrade(grade.StudentSectionId);
            }
            else
            {
                gradeCorrection.State = ConstantHelpers.GRADECORRECTION_STATES.DECLINED;
            }

            await _gradeCorrectionService.Update(gradeCorrection);
            return Ok();
        }

        /// <summary>
        /// Método para calificar la corrección de sustitutorio
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de corrección de sustitutorio</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("post-sustitutorio")]
        public async Task<IActionResult> ChangeStateSubstituteExam(CorrectionViewModel model)
        {
            var substituteExamCorrection = await _substituteExamCorrectionService.Get(model.Id);
            if (substituteExamCorrection == null) return BadRequest();

            if (substituteExamCorrection.State != ConstantHelpers.GRADECORRECTION_STATES.PENDING)
                return BadRequest("La corrección ya fue aprobada o desaprobada anteriormente.");

            var substitute_exam_evaluation_type = int.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAMEN_EVALUATION_TYPE));

            if (model.Status)
            {
                substituteExamCorrection.State = ConstantHelpers.GRADECORRECTION_STATES.APPROBED;

                var exam = await _substituteExamService.GetAsync(substituteExamCorrection.SubstituteExamId);

                if (substituteExamCorrection.NewGrade > 14 &&
                (substitute_exam_evaluation_type == ConstantHelpers.SUBSTITUTE_EXAM_EVALUATION_TYPE.DIRECTED_FINAL_GRADE ||
                substitute_exam_evaluation_type == ConstantHelpers.SUBSTITUTE_EXAM_EVALUATION_TYPE.AVERAGE_WITH_PREVIOUS_GRADE))
                    return BadRequest("La nota no puede ser mayor a 14.");

                exam.Status = ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED;
                var studentSection = await _studentSectionService.GetByStudentAndCourseTerm(exam.StudentId, exam.Section.CourseTermId);

                //CONFIGURACION DE EXAMEN SUSTITUTORIO
                if (substitute_exam_evaluation_type == ConstantHelpers.SUBSTITUTE_EXAM_EVALUATION_TYPE.DIRECTED_FINAL_GRADE)
                {
                    var newScore = (int)Math.Round(substituteExamCorrection.NewGrade);
                    exam.ExamScore = newScore;
                    studentSection.FinalGrade = studentSection.FinalGrade > newScore ? studentSection.FinalGrade : newScore;
                }
                else if (substitute_exam_evaluation_type == ConstantHelpers.SUBSTITUTE_EXAM_EVALUATION_TYPE.AVERAGE_WITH_PREVIOUS_GRADE)
                {
                    var newScore = (int)Math.Round((exam.PrevFinalScore.Value + substituteExamCorrection.NewGrade) / 2);
                    exam.ExamScore = newScore;
                    studentSection.FinalGrade = studentSection.FinalGrade > newScore ? studentSection.FinalGrade : newScore;
                }
                else if (substitute_exam_evaluation_type == ConstantHelpers.SUBSTITUTE_EXAM_EVALUATION_TYPE.GRADE_BY_FACTOR)
                {
                    var newScore = (int)Math.Round(Convert.ToDouble(substituteExamCorrection.NewGrade) * ConstantHelpers.SUBSTITUTE_EXAM_EVALUATION_FACTOR.FACTORS[(int)Math.Round(substituteExamCorrection.NewGrade)]);
                    exam.ExamScore = newScore;
                    studentSection.FinalGrade = studentSection.FinalGrade > newScore ? studentSection.FinalGrade : newScore;
                }


                await _substituteExamService.UpdateAsync(exam);

                await _studentSectionService.RecalculateFinalGrade(studentSection.Id);

                var academicHistory = await _context.AcademicHistories.Where(x => x.StudentId == exam.StudentId && x.SectionId == exam.SectionId).FirstOrDefaultAsync();
                if (academicHistory != null)
                {
                    var section = await _context.Sections.Where(x => x.Id == exam.SectionId).FirstOrDefaultAsync();
                    var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();
                    var term = await _context.Terms.Where(x => x.Id == courseTerm.TermId).FirstOrDefaultAsync();

                    academicHistory.Grade = studentSection.FinalGrade;
                    academicHistory.Approved = studentSection.FinalGrade >= term.MinGrade;

                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                substituteExamCorrection.State = ConstantHelpers.GRADECORRECTION_STATES.DECLINED;
            }

            await _substituteExamCorrectionService.Update(substituteExamCorrection);
            return Ok();
        }

        /// <summary>
        /// Método para descargar archivos
        /// </summary>
        /// <param name="path">Ruta del archivo</param>
        /// <returns>Archivo</returns>
        [AllowAnonymous]
        [HttpGet("archivo/{*path}")]
        public async Task DownloadImage(string path)
        {
            using (var mem = new MemoryStream())
            {
                var storage = new CloudStorageService(_storageCredentials);

                await storage.TryDownload(mem, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.GRADE_CORRECTION, path);

                // Download file
                var fileName = Path.GetFileName(path);
                var text = $"inline;filename=\"{fileName.Normalize().Replace(' ', '_')}\"";
                HttpContext.Response.Headers["Content-Disposition"] = text;
                mem.Position = 0;
                await mem.CopyToAsync(HttpContext.Response.Body);
            }
        }

        /// <summary>
        /// Obtiene el valor de la configuración
        /// </summary>
        /// <param name="key">Identificador de la configuración</param>
        /// <returns>Valor de la configuración</returns>
        private async Task<string> GetConfigurationValue(string key)
        {
            var values = await _configurationService.GetDataDictionary();
            return values.ContainsKey(key) ? values[key] :

                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES.ContainsKey(key) ?
                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[key] : "";
        }

    }
}
