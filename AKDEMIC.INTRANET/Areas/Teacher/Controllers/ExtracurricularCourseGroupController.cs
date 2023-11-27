using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Teacher.Models.ExtracurricularCourseGroupViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("/docente/gruposextracurriculares")]
    public class ExtracurricularCourseGroupController : BaseController
    {
        private readonly IExtracurricularCourseGroupService _extracurricularCourseGroupService;
        private readonly IExtracurricularCourseGroupStudentService _extracurricularCourseGroupStudentService;
        private readonly IExtracurricularCourseGroupAssistanceService _extracurricularCourseGroupAssistanceService;
        private readonly IExtracurricularCourseGroupAssistanceStudentService _extracurricularCourseGroupAssistanceStudentService;

        public ExtracurricularCourseGroupController(IUserService userService,
            ITermService termService,
            IExtracurricularCourseGroupService extracurricularCourseGroupService,
            IExtracurricularCourseGroupStudentService extracurricularCourseGroupStudentService,
            IExtracurricularCourseGroupAssistanceService extracurricularCourseGroupAssistanceService,
            IExtracurricularCourseGroupAssistanceStudentService extracurricularCourseGroupAssistanceStudentService) : base(userService, termService)
        {
            _extracurricularCourseGroupService = extracurricularCourseGroupService;
            _extracurricularCourseGroupStudentService = extracurricularCourseGroupStudentService;
            _extracurricularCourseGroupAssistanceService = extracurricularCourseGroupAssistanceService;
            _extracurricularCourseGroupAssistanceStudentService = extracurricularCourseGroupAssistanceStudentService;
        }

        /// <summary>
        /// Vista pricipal donde se gestionan los cursos extracurriculares
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Vista de asistencia de estudiantes
        /// </summary>
        /// <param name="id">Identificador del curso extracurricular</param>
        /// <param name="name">Nombre del curso extracurricular</param>
        /// <returns>Vista de asistencia</returns>
        [HttpGet("assistance/{id}/{name}")]
        public IActionResult Assistance(Guid id, String name)
        {
            ViewBag.GroupId = id;
            ViewBag.Group = name;
            return View();
        }

        /// <summary>
        /// Vista donde se listan las fechas que necesitan tomar asistencia
        /// </summary>
        /// <param name="id">Identificador del curso extracurricular</param>
        /// <param name="name">Nombre del curso extracurricular</param>
        /// <returns>Vista historial</returns>
        [HttpGet("historic/{id}/{name}")]
        public IActionResult Historic(Guid id, String name)
        {
            ViewBag.GroupId = id;
            ViewBag.Group = name;
            return View();
        }

        /// <summary>
        /// Vista donde se muestra el historial de asistencia de los estudiantes
        /// </summary>
        /// <param name="id">Identificador del curso extracurricular</param>
        /// <param name="group">Nombre del grupo</param>
        /// <returns>Vista historial de asistencia</returns>
        [HttpGet("historic/assistance/{id}/{group}")]
        public async Task<IActionResult> HistoricAssistance(Guid id, string group)
        {
            var assistance = await _extracurricularCourseGroupAssistanceService.Get(id);
            ViewBag.AssistanceGroupId = id;
            ViewBag.Group = group;
            ViewBag.Date = $"{assistance.RegisterDate.ToShortDateString()} {assistance.RegisterDate.ToShortTimeString()}";
            ViewBag.GroupId = assistance.GroupId;
            return View();
        }

        /// <summary>
        /// Vista donde se registra las calificaciones de los estudiantes
        /// </summary>
        /// <param name="id">Identificador del curso extracurricular</param>
        /// <param name="name">Nombre del curso extracurricular</param>
        /// <returns>Vista califación de los estudiantes</returns>
        [HttpGet("qualification/{id}/{name}")]
        public IActionResult Qualification(Guid id, String name)
        {
            ViewBag.GroupId = id;
            ViewBag.Group = name;
            return View();
        }

        /// <summary>
        /// Método para obtener el listado de cursos extracurriculares asignados al docente logueado
        /// </summary>
        /// <returns>Lista de cursos extracurriculares</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetExtracurricularCourseGroups()
        {
            var userId = GetUserId();
            var extracurricularCourseGroups = await _extracurricularCourseGroupService.GetAll(userId);

            return Ok(extracurricularCourseGroups.Select(x => new
            {
                id = x.Id,
                course = $"{x.ExtracurricularCourse.Code} - {x.ExtracurricularCourse.Name}",
                group = $"{x.Code}"
            }).ToList());
        }

        /// <summary>
        /// Obtiene el listado de alumnos inscritos en el curso extracurricular
        /// </summary>
        /// <param name="id">Identificador del grupo</param>
        /// <returns>Listado de alumnos</returns>
        [HttpGet("section/{id}/students")]
        public async Task<IActionResult> GetStudents(Guid id)
        {
            var students = await _extracurricularCourseGroupStudentService.GetAllByGroup(id, ConstantHelpers.PAYMENT.STATUS.PAID);
            return Ok(students.Select(x => new
            {
                studentId = x.StudentId,
                student = x.Student.User.FullName,
                score = x.Score.ToString("0.00")
            }).ToList());
        }

        /// <summary>
        /// Método para guardar la asistencia de los alumnos asignados al grupo del curso extracurricular
        /// </summary>
        /// <param name="assistance">Objeto que contiene los datos de los alumnos</param>
        /// <param name="groupId">Identificador del grupo</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("assistance/save")]
        public async Task<IActionResult> SaveAssistances(string assistance, Guid groupId)
        {
            var list = JsonConvert.DeserializeObject<List<AssistanceStudentViewModel>>(assistance);
            if (list.Count <= 0)
                return BadRequest("Ocurrió un error al procesar los datos");

            var assistanceCourseGroup = new ExtracurricularCourseGroupAssistance() { GroupId = groupId, RegisterDate = DateTime.Now };
            await _extracurricularCourseGroupAssistanceService.Insert(assistanceCourseGroup);

            var assistanceStudents = list.Select(x => new ExtracurricularCourseGroupAssistanceStudent()
            {
                GroupAssistanceId = assistanceCourseGroup.Id,
                GroupStudentId = x.StudentId,
                State = x.Assistance
            });

            await _extracurricularCourseGroupAssistanceStudentService.InsertRange(assistanceStudents);
            return Ok();
        }

        /// <summary>
        /// Método para guardar las notas de los estudiantes
        /// </summary>
        /// <param name="score">Objeto que contiene las calificaciones de los alumnos</param>
        /// <param name="groupId">Identificador del grupo</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("score/save")]
        public async Task<IActionResult> SaveScores(string score, Guid groupId)
        {
            var list = JsonConvert.DeserializeObject<List<ScoreStudentViewModel>>(score);
            if (list.Count <= 0)
                return BadRequest("Ocurrió un error al procesar los datos");

            var term = await GetActiveTerm();
            var studentsGroup = await _extracurricularCourseGroupStudentService.GetAllByGroup(groupId);

            foreach (var sg in studentsGroup)
            {
                var data = list.FirstOrDefault(x => x.StudentId == sg.StudentId);
                if (data != null)
                {
                    sg.Score = data.Score;
                    sg.Approved = sg.Score >= term.MinGrade;
                }
            }

            await _extracurricularCourseGroupStudentService.UpdateRange(studentsGroup);
            return Ok();
        }

        /// <summary>
        /// Obtiene el listado de alumnos inscritos en el grupo del curso extracurricular
        /// </summary>
        /// <param name="id">Identificador del grupo</param>
        /// <returns>Listado de alumnos</returns>
        [HttpGet("section/{id}/assistances")]
        public async Task<IActionResult> GetHistoric(Guid id)
        {
            var extracurricularCourseGroupAssistances = await _extracurricularCourseGroupAssistanceService.GetAllByGroup(id);

            return Ok(extracurricularCourseGroupAssistances.Select(x => new
            {
                id = x.Id,
                date = $"{x.RegisterDate.ToShortDateString()} {x.RegisterDate.ToShortTimeString()}"
            }).ToList());
        }

        /// <summary>
        /// Obtiene el listado de asistencias del grupo
        /// </summary>
        /// <param name="id">Identificador de la asistencia</param>
        /// <returns>Listado de asistencias</returns>
        [HttpGet("section/{id}/assistances/students")]
        public async Task<IActionResult> GetAssistancesStudents(Guid id)
        {
            var extracurricularCourseGroupAssistanceStudents = await _extracurricularCourseGroupAssistanceStudentService.GetAllByAssistance(id);

            return Ok(extracurricularCourseGroupAssistanceStudents.Select(x => new
            {
                student = x.GroupStudent.Student.User.FullName,
                state = x.State
            }).OrderBy(x => x.student).ToList());
        }
    }
}
