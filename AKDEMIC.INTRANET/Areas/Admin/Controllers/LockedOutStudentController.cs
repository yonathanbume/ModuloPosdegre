using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.LockedOutViewModels;
using AKDEMIC.INTRANET.Areas.Admin.Models.StudentViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConstantHelpers = AKDEMIC.CORE.Helpers.ConstantHelpers;
using ConvertHelpers = AKDEMIC.CORE.Helpers.ConvertHelpers;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.CAREER_DIRECTOR + "," + ConstantHelpers.ROLES.ACADEMIC_RECORD + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE)]
    [Area("Admin")]
    [Route("admin/alumnos-bloqueados")]
    public class LockedOutStudentController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IStudentService _studentService;
        private readonly ITermService _termService;

        public LockedOutStudentController(IUserService userService,
            UserManager<ApplicationUser> userManager,
            IStudentService studentService,
            ITermService termService,
            IDataTablesService dataTablesService,
            IOptions<CloudStorageCredentials> storageCredentials) : base(userManager, userService, dataTablesService)
        {
            _storageCredentials = storageCredentials;
            _studentService = studentService;
            _termService = termService;
        }

        /// <summary>
        /// Vista principal donde se gestionan los alumnos bloquedaos
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene el listado de alumnos para ser usado en tablas
        /// </summary>
        /// <param name="facultyId">Identificador de la facultades</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="programId">Identificador del programa académico</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de alumnos</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid? facultyId = null, Guid? careerId = null, Guid? programId = null, string search = null)
        {
            var sentParameters = GetSentParameters();
            var result = await _studentService.GetLockedOutStudentsDatatable(sentParameters, search, facultyId, careerId, academicProgramId: programId);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el listado de alumnos para ser usado en select
        /// </summary>
        /// <param name="term">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de alumnos</returns>
        [HttpGet("usuarios/get")]
        public async Task<IActionResult> GetUsersStudentsSelect2(string term)
        {
            var result = await _userService.GetUsersStudentsSelect2ServerSide(term);
            return Ok(new { items = result });
        }

        /// <summary>
        /// Obtiene el motivo del bloqueo
        /// </summary>
        /// <param name="userId">Identificador del alumno</param>
        /// <returns>Motivo del bloqueo</returns>
        [HttpGet("get-motivo")]
        public async Task<IActionResult> GetLastReason(string userId)
        {
            var user = await _userService.Get(userId);
            return Ok(user.LockedOutReason);
        }


        /// <summary>
        /// Método para bloquear al alumno
        /// </summary>
        /// <param name="model">Objeto que contiene los datos del bloqueo</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("guardar")]
        public async Task<IActionResult> CreateLockOut(LockedOutViewModel model)
        {
            var adminuser = GetUserId();
            var user = await _userService.Get(model.UserId);
            user.IsLockedOut = true;
            user.LockedOutReason = model.Reason;
            await _userService.Update(user);

            var student = await _studentService.GetStudentByUser(model.UserId);
            if (student != null)
            {
                if (student.Observations == null)
                {
                    student.Observations = new List<StudentObservation>();
                }

                var term = await _termService.GetActiveTerm();
                student.Observations.Add(new StudentObservation()
                {
                    Observation = model.Reason,
                    Type = ConstantHelpers.OBSERVATION_TYPES.LOCK_OUT,
                    UserId = adminuser,
                    TermId = term != null ? term.Id : (Guid?)null
                });
                await _studentService.Update(student);
            }

            return Ok();
        }

        /// <summary>
        /// Método para desbloquear al alumno
        /// </summary>
        /// <param name="id">Identificador del alumno</param>
        /// <param name="reason">Razón de desbloqueo</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("desbloquear/{id}")]
        public async Task<IActionResult> Unlock(string id, string reason)
        {
            var adminuser = GetUserId();
            var user = await _userService.Get(id);
            user.IsLockedOut = false;
            user.LockedOutReason = null;

            var student = await _studentService.GetStudentByUser(id);
            if (student != null)
            {
                if (student.Observations == null)
                {
                    student.Observations = new List<StudentObservation>();
                }
                var term = await _termService.GetActiveTerm();

                student.Observations.Add(new StudentObservation()
                {
                    Observation = reason,
                    Type = ConstantHelpers.OBSERVATION_TYPES.UNLOCK,
                    UserId = adminuser,
                    TermId = term != null ? term.Id : (Guid?)null
                });
                await _studentService.Update(student);
            }

            await _userService.Update(user);

            return Ok();
        }
    }
}
