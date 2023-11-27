using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.AcademicCounter.Models.Request;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.AcademicCounter.Controllers
{
    [Area("AcademicCounter")]
    [Route("ventanilla/solicitudes")]
    [Authorize(Roles = 
        CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN+ "," +
        CORE.Helpers.ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_COUNTER + "," + 
        CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_RECORD + "," + 
        CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF)]
    public class RequestController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IDependencyService _dependencyService;
        private readonly IDocumentTypeService _documentTypeService;
        private readonly IRecordHistoryService _recordHistoryService;
        private readonly IInternalProcedureService _internalProcedureService;
        private readonly ITermService _termService;
        private readonly IUserProcedureService _userProcedureService;
        private readonly IProcedureService _procedureService;
        private readonly IClassifierService _classifierService;
        private readonly IRecordHistoryObservationService _recordHistoryObservationService;
        private readonly IUserService _userService;
        private readonly IDataTablesService _dataTablesService;
        private readonly IUserInternalProcedureService _userInternalProcedureService;

        public RequestController(
            AkdemicContext context,
            IStudentService studentService,
            IDependencyService dependencyService,
            IDocumentTypeService documentTypeService,
            IRecordHistoryService recordHistoryService,
            IInternalProcedureService internalProcedureService,
            ITermService termService,
            IUserProcedureService userProcedureService,
            IProcedureService procedureService,
            IClassifierService classifierService,
            IRecordHistoryObservationService recordHistoryObservationService,
            IUserService userService,
            IDataTablesService dataTablesService,
            IUserInternalProcedureService userInternalProcedureService) : base(context)
        {
            _studentService = studentService;
            _dependencyService = dependencyService;
            _documentTypeService = documentTypeService;
            _recordHistoryService = recordHistoryService;
            _internalProcedureService = internalProcedureService;
            _termService = termService;
            _userProcedureService = userProcedureService;
            _procedureService = procedureService;
            _classifierService = classifierService;
            _recordHistoryObservationService = recordHistoryObservationService;
            _userService = userService;
            _dataTablesService = dataTablesService;
            _userInternalProcedureService = userInternalProcedureService;
        }

        /// <summary>
        /// Vista donde se muestra las solicitudes del trámite
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Método para generar una solicitud
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la nueva solicitud</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("generar")]
        public async Task<IActionResult> CreateRecord(CreateRecordViewModel model)
        {
            var student = await _studentService.Get(model.StudentId);
            student.User = await _userService.Get(student.UserId);
            var user = await _userService.GetUserByClaim(User);
            var term = await _termService.GetActiveTerm();

            var procedureStaticType = ConstantHelpers.RECORDS.PROCEDURE_STATIC_TYPE[model.RecordType];
            var procedure = await _procedureService.GetByStaticType(procedureStaticType);

            if (procedure is null)
                return BadRequest($"No se encontró el trámite de {ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[procedureStaticType]}.");

            //var userProcedure = new UserProcedure
            //{
            //    ProcedureId = procedure.Id,
            //    TermId = term.Id,
            //    UserId = user.Id,
            //    DNI = user.Dni,
            //    Status = ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED
            //};

            var recordHistory = new RecordHistory
            {
                Type = model.RecordType,
                Date = DateTime.UtcNow,
                Number = await _recordHistoryService.GetLatestRecordNumberByType(model.RecordType, term.Year) + 1,
                StudentId = model.StudentId,
                DerivedUserId = model.AcademicRecordStaffId,
                RecordTermId = model.RecordType == ConstantHelpers.RECORDS.REGULARSTUDIES ? model.TermId : null,
                //UserProcedure = userProcedure
            };

            if (model.RecordType == ConstantHelpers.RECORDS.CERTIFICATEOFSTUDIESPARTIAL)
            {
                if (model.RangeType == 1)
                {
                    recordHistory.StartAcademicYear = model.StartAcademicYear;
                    recordHistory.EndAcademicYear = model.EndAcademicYear;
                }

                if (model.RangeType == 2)
                {
                    recordHistory.StartTermId = model.StartTerm;
                    recordHistory.EndTermId = model.EndTerm;
                }
            }

            await _recordHistoryService.Insert(recordHistory);
            return Ok(recordHistory.Id);
        }

        /// <summary>
        /// Obtiene el listado de los tipos de registro para ser usado en select
        /// </summary>
        /// <returns>Listado de tipos</returns>
        [HttpGet("tipos/get")]
        public IActionResult GetRecordTypes()
        {
            var selectlist = new List<SelectListItem>()
            {
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.STUDYRECORD.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.STUDYRECORD]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.PROOFONINCOME.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.PROOFONINCOME]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.ENROLLMENT.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.ENROLLMENT]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.REGULARSTUDIES.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.REGULARSTUDIES]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.EGRESS.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.EGRESS]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.MERITCHART.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.MERITCHART]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.UPPERFIFTH.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.UPPERFIFTH]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.UPPERTHIRD.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.UPPERTHIRD]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.ACADEMICRECORD.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.ACADEMICRECORD]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.ACADEMICPERFORMANCESUMMARY.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.ACADEMICPERFORMANCESUMMARY]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.BACHELOR.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.BACHELOR]
                },
                new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.JOBTITLE.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.JOBTITLE]
                },
                 new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.CERTIFICATEOFSTUDIES.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.CERTIFICATEOFSTUDIES]
                },
                  new SelectListItem {
                    Value = CORE.Helpers.ConstantHelpers.RECORDS.CERTIFICATEOFSTUDIESPARTIAL.ToString(),
                    Text = CORE.Helpers.ConstantHelpers.RECORDS.VALUES[CORE.Helpers.ConstantHelpers.RECORDS.CERTIFICATEOFSTUDIESPARTIAL]
                }
            };

            var result = selectlist
                .Select(x => new
                {
                    id = x.Value,
                    text = x.Text
                })
                .OrderBy(x => x.text)
                .ToList();

            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el listado de observaciones para ser usado en tablas
        /// </summary>
        /// <param name="recordHistoryId">Identificador de la solicitud</param>
        /// <returns>Listado de observaciones</returns>
        [HttpGet("get-observaciones")]
        public async Task<IActionResult> GetObservations(Guid recordHistoryId)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _recordHistoryObservationService.GetObservationsDatatableByRecordHistoryId(parameters, recordHistoryId);
            return Ok(result);
        }

        /// <summary>
        /// Método para finalizar la solicitud
        /// </summary>
        /// <param name="id">Identificador de la solicitud</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpGet("finalizar")]
        public async Task<IActionResult> Finish(Guid id)
        {
            var userInteralProcedure = await _userInternalProcedureService.Get(id);
            userInteralProcedure.Status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ACCEPTED;
            await _userInternalProcedureService.Update(userInteralProcedure);
            return Ok();
        }

        /// <summary>
        /// Obtiene el historial de las solicitudes del alumno
        /// </summary>
        /// <param name="studentId">Identificador del alumno</param>
        /// <param name="type">Tipo de solicitud</param>
        /// <returns>Historial de solicitudes</returns>
        [HttpGet("historial/get")]
        public async Task<IActionResult> GetRecordHistory(Guid studentId, int type)
        {
            var dataDB = await _context.RecordHistories
                .Where(x => x.StudentId == studentId && x.Type == type)
                .Include(x => x.DerivedUser)
                //.Include(x => x.UserProcedure)
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    id = x.Id,
                    date = x.Date.ToLocalDateTimeFormat(),
                    number = $"{x.Number.ToString().PadLeft(5, '0')}-{x.Date.Year}",
                    academicrecord = x.DerivedUser?.UserName,
                    //status = x.UserProcedure != null ? ConstantHelpers.USER_PROCEDURES.STATUS.VALUES[x.UserProcedure.Status] : null
                })
                .OrderByDescending(x => x.date)
                .ToList();

            var result = new
            {
                data = data
            };

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de estudiantes
        /// </summary>
        /// <param name="term">Texto de búsqueda</param>
        /// <returns>Listado de estudiantes</returns>
        [HttpGet("buscar")]
        public async Task<IActionResult> Search(string term)
        {
            var students = await _studentService.SearchStudentByTerm(term, null, User);
            return Ok(new { items = students });
        }

        [AllowAnonymous]
        [HttpGet("generar-procedures")]
        public async Task<IActionResult> GenerateProcedures()
        {
            var studentRoleId = await _context.Roles.Where(x => x.Name == ConstantHelpers.ROLES.STUDENTS).Select(x => x.Id).FirstOrDefaultAsync();
            var procedures = new List<Procedure>
            {
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.STUDYRECORD], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.STUDYRECORD, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.PROOFONINCOME], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.PROOFONINCOME,ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.ENROLLMENT], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.ENROLLMENT, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.REGULARSTUDIES], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.REGULARSTUDIES, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.EGRESS], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.EGRESS, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.MERITCHART], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.MERITCHART, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.UPPERFIFTH], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.UPPERFIFTH, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.UPPERTHIRD], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.UPPERTHIRD, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.ACADEMICRECORD], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.ACADEMICRECORD, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.ACADEMICPERFORMANCESUMMARY], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.ACADEMICPERFORMANCESUMMARY, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.BACHELOR], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.BACHELOR, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.JOBTITLE], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.JOBTITLE, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.CERTIFICATEOFSTUDIES], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.CERTIFICATEOFSTUDIES, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.CERTIFICATEOFSTUDIESPARTIAL], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.CERTIFICATEOFSTUDIESPARTIAL, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.BACHELOR_DEGREE_APPLICATION], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.BACHELOR_DEGREE_APPLICATION, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
                new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.TITLE_DEGREE_APPLICATION], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.TITLE_DEGREE_APPLICATION, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            };

            await _context.Procedures.AddRangeAsync(procedures);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Obtiene el listado de periodos matriculados del alumno
        /// </summary>
        /// <param name="studentId">Identificador del alumno</param>
        /// <returns>Listado de periodos académicos</returns>
        [HttpGet("get-periodos-alumno")]
        public async Task<IActionResult> GetTermsByStudent(Guid studentId)
        {
            var data = await _context.StudentSections.Where(x => x.StudentId == studentId)
                .Select(x => new
                {
                    id = x.Section.CourseTerm.TermId,
                    text = x.Section.CourseTerm.Term.Name
                })
                .Distinct()
                .ToListAsync();

            data = data.OrderBy(x => x.text).ToList();

            return Ok(data);
        }

        /// <summary>
        /// Obtiene los ciclos concluidos del alumno
        /// </summary>
        /// <param name="studentId">Identificador del alumno</param>
        /// <returns>Listado de ciclos</returns>
        [HttpGet("get-ciclos-alumno")]
        public async Task<IActionResult> GetAcademicYears(Guid studentId)
        {
            var curriculumId = await _context.Students.Where(x => x.Id == studentId).Select(x => x.CurriculumId).FirstOrDefaultAsync();
            var data = await _context.AcademicYearCourses.Where(x => x.CurriculumId == curriculumId).Select(x => x.AcademicYear).ToListAsync();
            var result = data
                .Distinct()
                .Select(x => new
                {
                    id = x,
                    text = ConstantHelpers.ACADEMIC_YEAR.ROMAN_NUMERALS[x]
                })
                .OrderBy(x => x.id)
                .ToList();

            return Ok(result);
        }
    }
}
