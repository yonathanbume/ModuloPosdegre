using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Teacher.Models;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using Microsoft.AspNetCore.Http;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System.Security.Claims;
using AKDEMIC.CORE.Services;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Options;
using System.IO;
using AKDEMIC.INTRANET.Areas.Teacher.Models.GradeCorrectionViewModels;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("profesor/correccion-notas")]
    public class GradeCorrectionController : BaseController
    {
        private readonly IGradeCorrectionService _gradeCorrectionService;
        private readonly ISectionService _sectionService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IGradeService _gradeService;
        private readonly IGradeRegistrationService _gradeRegistrationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICourseTermService _courseTermService;
        private readonly IConfigurationService _configurationService;
        private readonly IRecordsConceptService _recordsConceptService;
        private readonly IConceptService _conceptService;
        private readonly IPaymentService _paymentService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly ITeacherSectionService _teacherSectionService;
        private readonly ISubstituteExamService _substituteExamService;
        private readonly IStudentService _studentService;
        private readonly ISubstituteExamCorrectionService _substituteExamCorrectionService;
        private readonly IActionContextAccessor _accessor;

        public GradeCorrectionController(IUserService userService,
             ITermService termService,
             ICourseTermService courseTermService,
             ISectionService sectionService,
             IStudentSectionService studentSectionService,
             IGradeService gradeService, IGradeRegistrationService gradeRegistrationService,
             IHttpContextAccessor httpContextAccessor,
             IGradeCorrectionService gradeCorrectionService,
             IConfigurationService configurationService,
             IRecordsConceptService recordsConceptService,
             IConceptService conceptService,
             IActionContextAccessor accessor,
             IOptions<CloudStorageCredentials> storageCredentials,
             IPaymentService paymentService,
             ITeacherSectionService teacherSectionService,
             ISubstituteExamService substituteExamService,
             IStudentService studentService,
             ISubstituteExamCorrectionService substituteExamCorrectionService) : base(userService, termService)
        {
            _gradeCorrectionService = gradeCorrectionService;
            _accessor = accessor;
            _sectionService = sectionService;
            _courseTermService = courseTermService;
            _studentSectionService = studentSectionService;
            _gradeService = gradeService;
            _gradeRegistrationService = gradeRegistrationService;
            _httpContextAccessor = httpContextAccessor;
            _configurationService = configurationService;
            _conceptService = conceptService;
            _paymentService = paymentService;
            _recordsConceptService = recordsConceptService;
            _storageCredentials = storageCredentials;
            _teacherSectionService = teacherSectionService;
            _substituteExamService = substituteExamService;
            _studentService = studentService;
            _substituteExamCorrectionService = substituteExamCorrectionService;
        }

        /// <summary>
        /// Vista donde se gestionan las correcciones de notas
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de solicitudes de correcciones de notas asociadas al docente logueado
        /// </summary>
        /// <param name="id">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene el listado de solicitudes de correcciones</returns>
        [Route("get/{id}")]
        public async Task<IActionResult> GetGradeCorrections(Guid id)
        {
            var userId = GetUserId();
            var gradeCorrections = await _gradeCorrectionService.GetAll(userId, id);
            var sustiCorrections = await _substituteExamCorrectionService.GetAll(userId, id);
            var data1 = gradeCorrections.Select(gc => new CorrectionsViewModel
            {
                Id = gc.Id,
                Code = gc.Grade.StudentSection.Student.User.UserName,
                Student = gc.Grade.StudentSection.Student.User.FullName,
                Course = gc.Grade.StudentSection.Section.CourseTerm.Course.Name,
                Section = gc.Grade.StudentSection.Section.Code,
                Evaluation = gc.Grade.Evaluation.Name,
                Grade = gc.NewGrade,
                OldGrade = gc.OldGrade,
                State = gc.State,
                Observations = gc.Observations,
                FilePath = gc.FilePath,
                RequestedByStudent = gc.RequestedByStudent
            }).ToList();

            var data2 = sustiCorrections.Select(sc => new CorrectionsViewModel
            {
                Code = sc.SubstituteExam.Student.User.UserName,
                Student = sc.SubstituteExam.Student.User.FullName,
                Course = sc.SubstituteExam.CourseTerm.Course.Name,
                Section = sc.SubstituteExam.Section.Code,
                Evaluation = "SUSTITUTORIO",
                Grade = sc.NewGrade,
                State = sc.State
            });
            return Ok(data1);
        }

        /// <summary>
        /// Obtiene el listado de cursos asignados al docente logueado
        /// </summary>
        /// <returns>Listado de cursos</returns>
        [Route("cursos/get")]
        public async Task<IActionResult> GetActiveCourses()
        {
            var userId = GetUserId();
            var result = await _teacherSectionService.GetTeacherCoursesSelect2(userId);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de secciones asignados al docente logueado
        /// </summary>
        /// <param name="id">identificador del curso</param>
        /// <returns>Listado de secciones</returns>
        [Route("secciones/get/{id}")]
        public async Task<IActionResult> GetActiveSections(Guid id)
        {
            var userId = GetUserId();
            var result = await _teacherSectionService.GetTeacherCourseSectionsSelect2(userId, id);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de alumnos matriculados en una sección
        /// </summary>
        /// <param name="id">Identificador de la sección</param>
        /// <returns>Listado de alumnos</returns>
        [Route("alumnos/get/{id}")]
        public async Task<IActionResult> GetStudentsSection(Guid id)
        {
            var result = await _studentSectionService.GetSectionStudentsSelect2(id);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de notas asignadas a un alumno
        /// </summary>
        /// <param name="id">Identificador del alumno-sección</param>
        /// <returns>Listado de notas</returns>
        [Route("notas/get/{id}")]
        public async Task<IActionResult> GetStudentGrades(Guid id)
        {
            var evaluationsByUnits = Convert.ToBoolean(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT));

            var grades = await _gradeService.GetAll(id);
            var studentSection = await _studentSectionService.Get(id);
            var susti = await _substituteExamService.GetSubstituteExamByStudentAndSectionId(studentSection.StudentId, studentSection.SectionId);

            if (evaluationsByUnits)
            {
                var result = grades
                    .OrderBy(x => x.Evaluation.CourseUnit.Number)
                    .GroupBy(x => x.Evaluation.CourseUnitId)
                    .Select(x => new
                    {
                        text = x.Where(y=>y.Evaluation.CourseUnitId == x.Key).Select(y=>y.Evaluation.CourseUnit.Name).FirstOrDefault(),
                        children = x.OrderBy(y=>y.Evaluation.Week).Select(y => new
                        {
                            id = y.Id,
                            text = $"{y.Evaluation.Name} : {y.Value}"
                        }).ToList()
                    })
                    .ToList();

                if (susti != null)
                {
                    var listSubstituteExams = new List<SubstituteExam>
                    {
                        susti
                    };

                    result.Add(new
                    {
                        text = "SUSTITUTORIO",
                        children = listSubstituteExams.Select(x=> new
                        {
                            id = x.Id,
                            text  = $"SUSTITUTORIO: {susti.ExamScore.Value.ToString("00.00")}"
                        }).ToList()
                    });
                }

                return Ok(new { items = result });

            }
            else
            {
                var result = grades.OrderBy(y=>y.Evaluation.Week).Select(g => new
                {
                    id = g.Id,
                    text = g.Evaluation.Name + ": " + g.Value,
                    rendered = true
                }).ToList();

                if (susti != null)
                    result.Add(new { id = susti.Id, text = $"SUSTITUTORIO: {susti.ExamScore.Value.ToString("00.00")}", rendered = true });

                return Ok(new { items = result });
            }
        }

        /// <summary>
        /// Método para guardar la solicitud de corrección de nota
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la solicitud</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("registrar")]
        public async Task<IActionResult> CreateRequest(GradeCorrectionViewModel model)
        {
            var iPAddress = GetRequestIP();

            var userId = GetUserId();

            var toPay = bool.Parse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.RECORD_RECTIFICATION_CHARGE));
            var record_rectification_charge_note = await _recordsConceptService.GetValueByRecordType(ConstantHelpers.RECORDS.RECTIFICATIONCHARGENOTE) ?? Guid.Empty;

            var grade = await _gradeService.Get(model.GradeId);
            var susti = await _substituteExamService.GetAsync(model.GradeId);
            if (grade != null)
            {
                var studentSection = await _studentSectionService.Get(grade.StudentSectionId);
                var anySubstituteExams = await _substituteExamService.AnySubstituteExamByStudent(studentSection.StudentId, studentSection.SectionId, ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED);

                var teacherSections = await _teacherSectionService.GetAllBySection(studentSection.SectionId);

                var confiGradesByPrincipalTeacher = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.GRADES_CAN_ONLY_PUBLISHED_BY_PRINCIPAL_TEACHER);

                if (Convert.ToBoolean(confiGradesByPrincipalTeacher))
                {
                    if (!teacherSections.Any(y => y.TeacherId == userId && y.IsPrincipal))
                    {
                        return BadRequest("Solo los docentes principales pueden ingresar solicitudes.");
                    }
                }

                if (anySubstituteExams)
                {
                    return BadRequest("El estudiante tiene un examen sustitutorio pendiente.");
                }

                var gradeCorrection = new GradeCorrection
                {
                    TeacherId = GetUserId(),
                    GradeId = model.GradeId,
                    NewGrade = !model.NotTaken ? model.NewGrade : 0,
                    CreatorIP = iPAddress,
                    ToPay = toPay,
                    Observations = model.Observations,
                    OldGrade = grade.Value,
                    NotTaken = model.NotTaken,
                };

                if (model.File != null)
                {
                    var storage = new CloudStorageService(_storageCredentials);
                    var fileUrl = await storage.UploadFile(model.File.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.GRADE_CORRECTION, Path.GetExtension(model.File.FileName));
                    gradeCorrection.FilePath = fileUrl;
                }

                await _gradeCorrectionService.Insert(gradeCorrection);

                if (toPay)
                {
                    var concept = await _conceptService.GetConcept(record_rectification_charge_note);
                    if (concept != null)
                    {
                        var total = concept.Amount;
                        var subtotal = total;
                        var igvAmount = 0.00M;
                        var term = await _termService.GetActiveTerm();
                        if (concept.IsTaxed)
                        {
                            subtotal = total / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                            igvAmount = total - subtotal;
                        }
                        var payment = new Payment
                        {
                            UserId = userId,
                            ConceptId = concept.Id,
                            EntityId = gradeCorrection.Id,
                            Description = concept.Description,
                            Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.CONCEPT,
                            Discount = 0.00M,
                            Total = total,
                            SubTotal = subtotal,
                            IgvAmount = igvAmount,
                            Status = CORE.Helpers.ConstantHelpers.PAYMENT.STATUS.PENDING,
                            TermId = term.Id
                        };

                        await _paymentService.Insert(payment);
                    }
                }
            }

            else if (susti != null)
            {
                var teacherSections = await _teacherSectionService.GetAllBySection(susti.SectionId.Value);

                var confiGradesByPrincipalTeacher = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.GRADES_CAN_ONLY_PUBLISHED_BY_PRINCIPAL_TEACHER);

                if (Convert.ToBoolean(confiGradesByPrincipalTeacher))
                {
                    if (!teacherSections.Any(y => y.TeacherId == userId && y.IsPrincipal))
                    {
                        return BadRequest("Solo los docentes principales pueden ingresar solicitudes.");
                    }
                }

                var substituteExamCorrection = new SubstituteExamCorrection
                {
                    TeacherId = GetUserId(),
                    SubstituteExamId = model.GradeId,
                    NewGrade = model.NewGrade,
                    CreatorIP = iPAddress,
                    ToPay = toPay,
                    Observations = model.Observations,
                    OldGrade = Convert.ToDecimal(susti.ExamScore)
                };

                if (model.File != null)
                {
                    var storage = new CloudStorageService(_storageCredentials);
                    var fileUrl = await storage.UploadFile(model.File.OpenReadStream(), CORE.Helpers.ConstantHelpers.CONTAINER_NAMES.SUBSTITUTE_EXAMEN_CORRECTION, Path.GetExtension(model.File.FileName));
                    substituteExamCorrection.FilePath = fileUrl;
                }

                await _substituteExamCorrectionService.Insert(substituteExamCorrection);

                if (toPay)
                {
                    var concept = await _conceptService.GetConcept(record_rectification_charge_note);
                    if (concept != null)
                    {
                        var total = concept.Amount;
                        var subtotal = total;
                        var igvAmount = 0.00M;
                        var term = await _termService.GetActiveTerm();
                        if (concept.IsTaxed)
                        {
                            subtotal = total / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                            igvAmount = total - subtotal;
                        }
                        var payment = new Payment
                        {
                            UserId = userId,
                            ConceptId = concept.Id,
                            EntityId = substituteExamCorrection.Id,
                            Description = concept.Description,
                            Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.CONCEPT,
                            Discount = 0.00M,
                            Total = total,
                            SubTotal = subtotal,
                            IgvAmount = igvAmount,
                            Status = CORE.Helpers.ConstantHelpers.PAYMENT.STATUS.PENDING,
                            TermId = term.Id
                        };

                        await _paymentService.Insert(payment);
                    }
                }
            }
            else
            {
                return BadRequest("No existe la evaluación y la nota");
            }

            return Ok();
        }

        [HttpPost("aceptar-solicitud-estudiante")]
        public async Task<IActionResult> AcceptStudentRequest(GradeCorrectionViewModel model)
        {
            var iPAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            var gradeCorrection = await _gradeCorrectionService.Get(model.Id);

            if (gradeCorrection.State != ConstantHelpers.GRADECORRECTION_STATES.STUDENT_REQUEST)
                return BadRequest("La solicitud no se encuentra pendiente.");

            gradeCorrection.State = ConstantHelpers.GRADECORRECTION_STATES.PENDING;
            gradeCorrection.NewGrade = model.NewGrade;
            gradeCorrection.CreatorIP = iPAddress;
            gradeCorrection.TeacherId = GetUserId();

            await _gradeCorrectionService.Update(gradeCorrection);
            return Ok();
        }

        [HttpPost("denegar-solicitud-estudiante")]
        public async Task<IActionResult> RejectStudentRequest(GradeCorrectionViewModel model)
        {
            var iPAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            var gradeCorrection = await _gradeCorrectionService.Get(model.Id);

            if (gradeCorrection.State != ConstantHelpers.GRADECORRECTION_STATES.STUDENT_REQUEST)
                return BadRequest("La solicitud no se encuentra pendiente.");

            gradeCorrection.State = ConstantHelpers.GRADECORRECTION_STATES.DECLINED;
            //gradeCorrection.NewGrade = model.NewGrade;
            gradeCorrection.CreatorIP = iPAddress;
            gradeCorrection.TeacherId = GetUserId();

            await _gradeCorrectionService.Update(gradeCorrection);
            return Ok();
        }

        /// <summary>
        /// Obtiene la IP pública del docente logueaado
        /// </summary>
        /// <param name="tryUseXForwardHeader"></param>
        /// <returns>IP Pública</returns>
        private string GetRequestIP(bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
            {
                var csvList = GetHeaderValueAs<string>("X-Forwarded-For");
                ip = SplitCsv(csvList).FirstOrDefault();
            }

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (string.IsNullOrWhiteSpace(ip) && _accessor.ActionContext.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (string.IsNullOrWhiteSpace(ip))
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }

        private T GetHeaderValueAs<T>(string headerName)
        {
            StringValues values;

            if (_accessor.ActionContext.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }

        private static List<string> SplitCsv(string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }

    }
}
