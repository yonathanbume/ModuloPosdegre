using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.ExtraordinaryEvaluationViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)]
    [Area("Admin")]
    [Route("admin/evaluacion-extraordinaria")]
    public class ExtraordinaryEvaluationController : BaseController
    {
        private readonly IExtraordinaryEvaluationService _extraordinaryEvaluationService;
        private readonly ITermService _termService;
        private readonly IExtraordinaryEvaluationStudentService _extraordinaryEvaluationStudentService;
        private readonly IExtraordinaryEvaluationCommitteeService _extraordinaryEvaluationCommitteeService;
        private readonly IDataTablesService _dataTablesService;
        private readonly ICourseTermService _courseTermService;
        private readonly ITeacherService _teacherService;
        private readonly ICourseService _courseService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IUserService _userService;
        private readonly ICareerService careerService;
        private readonly ISelect2Service _select2Service;
        private readonly IStudentService _studentService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public ExtraordinaryEvaluationController(
            IExtraordinaryEvaluationService extraordinaryEvaluationService,
            ITermService termService,
            IExtraordinaryEvaluationStudentService extraordinaryEvaluationStudentService,
            IExtraordinaryEvaluationCommitteeService extraordinaryEvaluationCommitteeService,
            IDataTablesService dataTablesService,
            ICourseTermService courseTermService,
            ITeacherService teacherService,
            ICourseService courseService,
            IStudentSectionService studentSectionService,
            IUserService userService,
            ICareerService careerService,
            ISelect2Service select2Service,
            IStudentService studentService,
            IOptions<CloudStorageCredentials> storageCredentials
            )
        {
            _extraordinaryEvaluationService = extraordinaryEvaluationService;
            _termService = termService;
            _extraordinaryEvaluationStudentService = extraordinaryEvaluationStudentService;
            _extraordinaryEvaluationCommitteeService = extraordinaryEvaluationCommitteeService;
            _dataTablesService = dataTablesService;
            _courseTermService = courseTermService;
            _teacherService = teacherService;
            _courseService = courseService;
            _studentSectionService = studentSectionService;
            _userService = userService;
            this.careerService = careerService;
            _select2Service = select2Service;
            _studentService = studentService;
            _storageCredentials = storageCredentials;
        }

        /// <summary>
        /// Vista donde se gestionan las evaluaciones extraordinarias
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de evaluaciones extraordinarias
        /// </summary>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <param name="type">Tipo de evaluación</param>
        /// <returns>Objeto que contiene el listado de evaluaciones extraordinarias</returns>
        [HttpGet("get-evaluaciones-extraordinarias")]
        public async Task<IActionResult> GetExtraordinaryEvaluations(string searchValue, byte? type)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _extraordinaryEvaluationService.GetExtraordinaryEvaluationsDatatable(parameters, searchValue, null, null, null, User, null, type);
            return Ok(result);
        }

        /// <summary>
        /// Método para agregar una evaluación extraordinaria
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la nueva evaluación extraordinaria</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("agregar-evaluacion-extraordinaria")]
        public async Task<IActionResult> AddExtraordinaryEvaluation(ExtraordinaryEvaluationViewModel model)
        {
            var termActive = await _termService.GetActiveTerm();
            if (termActive == null)
                return BadRequest("No existe periodo activo");

            if (model.Committee != null && model.Committee.Any(y => y == model.TeacherId))
                return BadRequest("El docente presidente no puede pertenecer al comité.");

            var entity = new ExtraordinaryEvaluation
            {
                Type = model.Type,
                CourseId = model.CourseId,
                Resolution = model.Resolution,
                TermId = termActive.Id,
                TeacherId = model.TeacherId
            };

            if (model.Committee != null && model.Committee.Any())
            {
                entity.ExtraordinaryEvaluationCommittees = model.Committee.Select(y => new ExtraordinaryEvaluationCommittee
                {
                    TeacherId = y
                })
                .ToList();
            }

            var storage = new CloudStorageService(_storageCredentials);

            if (model.ResolutionFile != null)
            {
                entity.ResolutionFile = await storage.UploadFile(model.ResolutionFile.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.RESOLUTIONS,
                    Path.GetExtension(model.ResolutionFile.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }


            await _extraordinaryEvaluationService.Insert(entity);
            return Ok(entity.Id);
        }

        /// <summary>
        /// Método para eliminar una evaluación extraordinaria
        /// </summary>
        /// <param name="id">Identificador de la evaluación extraordinaria</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar-evaluacion-extraordinaria")]
        public async Task<IActionResult> DeleteExtraordinaryEvaluation(Guid id)
        {
            var evaluation = await _extraordinaryEvaluationService.Get(id);
            var students = await _extraordinaryEvaluationStudentService.GetByExtraordinaryEvaluationId(evaluation.Id);

            if (students.Any())
                return BadRequest("Se encontraron estudiantes asignados a la evaluación.");

            var committee = await _extraordinaryEvaluationCommitteeService.GetCommittee(evaluation.Id);

            await _extraordinaryEvaluationCommitteeService.DeleteRange(committee);
            await _extraordinaryEvaluationService.Delete(evaluation);
            return Ok();
        }

        /// <summary>
        /// Obtiene el detalle de la evaluación extraordinaria
        /// </summary>
        /// <param name="id">Identificador de la evaluación extraordinaria</param>
        /// <returns>Objeto que contiene los datos de la evaluación extraordinaria</returns>
        [HttpGet("get-evaluacion-extraordinaria")]
        public async Task<IActionResult> GetExtraordinaryEvaluation(Guid id)
        {
            var evaluation = await _extraordinaryEvaluationService.Get(id);
            var committee = await _extraordinaryEvaluationCommitteeService.GetCommittee(evaluation.Id);
            var teacher = await _userService.Get(evaluation.TeacherId);
            var course = await _courseService.GetAsync(evaluation.CourseId);

            var result = new
            {
                Type = evaluation.Type,
                course = course.FullName,
                evaluation.Id,
                committee = committee.Select(x => new
                {
                    id = x.TeacherId,
                    text = x.Teacher.User.FullName
                }).ToList(),
                teacher = teacher?.FullName,
                teacherId = teacher?.Id,
                resolution = evaluation.Resolution,
                resolutionFileUrl = evaluation.ResolutionFile
            };

            return Ok(result);
        }

        /// <summary>
        /// Método para actualizar una evaluación extraordinaria
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados de la evaluación extraordinaria</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar-evaluacion-extraordinaria")]
        public async Task<IActionResult> EditExtraordinaryEvaluation(ExtraordinaryEvaluationViewModel model)
        {
            var evaluation = await _extraordinaryEvaluationService.Get(model.Id.Value);
            evaluation.TeacherId = model.TeacherId;
            evaluation.Resolution = model.Resolution;
            evaluation.Type = model.Type;

            var storage = new CloudStorageService(_storageCredentials);

            if (model.ResolutionFile != null)
            {
                if (!string.IsNullOrEmpty(evaluation.ResolutionFile))
                {
                    await storage.TryDelete(evaluation.ResolutionFile, CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.RESOLUTIONS);
                }

                evaluation.ResolutionFile = await storage.UploadFile(model.ResolutionFile.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.RESOLUTIONS,
                    Path.GetExtension(model.ResolutionFile.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            await _extraordinaryEvaluationCommitteeService.DeleteCommitteeByEvalutionId(model.Id.Value);

            if (model.Committee != null && model.Committee.Any())
            {
                var committee = model.Committee.Select(x => new ExtraordinaryEvaluationCommittee
                {
                    ExtraordinaryEvaluationId = model.Id.Value,
                    TeacherId = x
                }).ToList();

                await _extraordinaryEvaluationCommitteeService.InsertRange(committee);
            }

            await _extraordinaryEvaluationService.Update(evaluation);


            return Ok();
        }

        /// <summary>
        /// Vista donde se gestionan los alumnos asignados
        /// </summary>
        /// <param name="id">identificador de la evaluación extraordinaria</param>
        /// <returns>Vista detalle</returns>
        [HttpGet("detalles/{id}")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var entity = await _extraordinaryEvaluationService.Get(id);
            var commitee = await _extraordinaryEvaluationCommitteeService.GetCommittee(entity.Id);
            var teacher = await _userService.Get(entity.TeacherId);
            var course = await _courseService.GetAsync(entity.CourseId);
            var term = await _termService.Get(entity.TermId);

            var model = new ExtraordinaryEvaluationViewModel
            {
                Id = entity.Id,
                Committee = commitee.Select(x => x.Teacher.User.FullName).ToList(),
                CourseId = entity.CourseId,
                Resolution = entity.Resolution,
                ResolutionFileUrl = entity.ResolutionFile,
                Teacher = teacher?.FullName,
                Course = $"{course.Code} - {course.Name}",
                TermName = term.Name,
                Type = entity.Type
            };

            return View(model);
        }

        /// <summary>
        /// Obtiene el listado de estudiantes asignados a la evaluación extraordinaria
        /// </summary>
        /// <param name="extraordinaryEvaluationId">identificador de la evaluación extraordinaria</param>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de estudiantes asignados</returns>
        [HttpGet("get-estudiantes-asignados")]
        public async Task<IActionResult> GetAssignedStudents(Guid extraordinaryEvaluationId, string searchValue)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _extraordinaryEvaluationStudentService.GetStudentsDatatable(parameters, extraordinaryEvaluationId, searchValue);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de estudiantes asignados a la evaluación extraordinaria para ser usado en los select
        /// </summary>
        /// <param name="courseId">Identificador del curso</param>
        /// <param name="searchValue">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de estudiantes asignados</returns>
        [HttpGet("get-estudiantes-json")]
        public async Task<IActionResult> GetEnrolledStudents(Guid courseId, string searchValue)
        {
            var parameters = _select2Service.GetRequestParameters();
            var result = await _studentService.GetStudentsWithPendingCourse(parameters, courseId, searchValue);
            return Ok(result);
        }

        /// <summary>
        /// Método para asignar un estudiante a la evaluación extraordinaria
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del estudiante</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("agregar-estudiante")]
        public async Task<IActionResult> AddStudent(StudentEvaluationViewModel model)
        {
            var students = await _extraordinaryEvaluationStudentService.GetByExtraordinaryEvaluationId(model.ExtraordinaryEvaluationId);

            if (students.Any(y => y.StudentId == model.StudentId))
                return BadRequest("El estudiante seleccionado ya se encuentra asignado a la evaluación");

            var entity = new ExtraordinaryEvaluationStudent
            {
                ExtraordinaryEvaluationId = model.ExtraordinaryEvaluationId,
                Status = ConstantHelpers.Intranet.ExtraordinaryEvaluationStudent.PENDING,
                StudentId = model.StudentId,
            };

            await _extraordinaryEvaluationStudentService.Insert(entity);
            return Ok();
        }

        /// <summary>
        /// Método para remover un estudiante de la evaluación extraordinaria
        /// </summary>
        /// <param name="id">Identificador del estudiante asignado</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("eliminar-estudiante")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var entity = await _extraordinaryEvaluationStudentService.Get(id);

            if (entity.Status != ConstantHelpers.Intranet.ExtraordinaryEvaluationStudent.PENDING)
                return BadRequest("El estudiante ya ha sido calificado.");

            await _extraordinaryEvaluationStudentService.Delete(entity);
            return Ok();
        }
    }
}
