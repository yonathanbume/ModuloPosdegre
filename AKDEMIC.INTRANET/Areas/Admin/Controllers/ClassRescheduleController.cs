using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Areas.Admin.Models.ClassRescheduleViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
        ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," +
        ConstantHelpers.ROLES.CAREER_DIRECTOR
        )]
    [Area("Admin")]
    [Route("admin/clases/reprogramaciones")]
    public class ClassRescheduleController : BaseController
    {
        private readonly IClassroomService _classroomService;
        private readonly IClassRescheduleService _classRescheduleService;
        private readonly IClassScheduleService _classScheduleService;
        private readonly IClassService _classService;
        private readonly ITeacherScheduleService _teacherScheduleService;
        private readonly ISectionService _sectionService;
        private readonly ICourseTermService _courseTermService;
        private readonly IConfigurationService _configurationService;
        private readonly ICareerService _careerService;

        public ClassRescheduleController(
            IDataTablesService dataTablesService,
            IClassroomService classroomService,
            IClassRescheduleService classRescheduleService,
            IClassScheduleService classScheduleService,
            ICareerService careerService,
            IClassService classService,
            ITeacherScheduleService teacherScheduleService,
            IUserService userService,
            ISectionService sectionService,
            ICourseTermService courseTermService,
            IConfigurationService configurationService
        ) : base(userService, dataTablesService)
        {
            _classroomService = classroomService;
            _classRescheduleService = classRescheduleService;
            _classScheduleService = classScheduleService;
            _classService = classService;
            _teacherScheduleService = teacherScheduleService;
            _sectionService = sectionService;
            _courseTermService = courseTermService;
            _configurationService = configurationService;
            _careerService = careerService;
        }

        /// <summary>
        /// Vista donde se gestionan las reprogramaciones de clase
        /// </summary>
        /// <returns>Vista</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de solicitudes de reprogramación de clase
        /// </summary>
        /// <param name="startSearchDate">Fec. Inicio</param>
        /// <param name="endSearchDate">Fec. Fin</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de solicitudes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetClassReschedules(string startSearchDate = null, string endSearchDate = null, string search = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _classRescheduleService.GetClassRescheduleDatatable(sentParameters, User, ConstantHelpers.CLASS_RESCHEDULE.STATUS.IN_PROCESS, startSearchDate, endSearchDate, search);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el detalle de la solicitud de reprogramación de clase
        /// </summary>
        /// <param name="id">Identificador de la solicitud</param>
        /// <returns>Datos de la solicitud</returns>
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetClassReschedules(Guid id)
        {
            var result = await _classRescheduleService.GetClassReschedule(id);
            return Ok(result);
        }

        /// <summary>
        /// Método para aceptar una solicitud de reprogramación de clase
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la reprogramación de clase</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("aceptar/post")]
        public async Task<IActionResult> AcceptClassReschedule(ClassRescheduleViewModel model)
        {
            var classReschedule = await _classRescheduleService.Get(model.Id);
            classReschedule.Status = ConstantHelpers.CLASS_RESCHEDULE.STATUS.ACCEPTED;

            var dayOfWeek = ConstantHelpers.WEEKDAY.ENUM_TO_INT(classReschedule.StartDateTime.ToDefaultTimeZone().DayOfWeek);

            var @class = await _classService.Get(classReschedule.ClassId);
            @class.EndTime = classReschedule.EndDateTime;
            @class.IsRescheduled = true;
            @class.StartTime = classReschedule.StartDateTime;
            @class.WeekNumber = dayOfWeek;
            @class.NeedReschedule = false;

            if (classReschedule.Replicate)
            {
                var classSchedule = await _classScheduleService.Get(@class.ClassScheduleId);
                var classroom = await _classroomService.Get(classSchedule.ClassroomId);
                var section = await _sectionService.Get(classSchedule.SectionId);
                var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);

                var st = classReschedule.StartDateTime.TimeOfDay;
                var et = classReschedule.EndDateTime.TimeOfDay;

                if (classroom.Code != "Sin Asignar")
                {
                    var classroomConflicted = await _classScheduleService.GetWithSectionCourseTermCourse(classroom.Id, dayOfWeek, st, et, courseTerm.TermId, classSchedule.Id);

                    if (classroomConflicted != null)
                        return BadRequest(
                            $"El aula seleccionada se encuentra ocupada en ese rango de horas por una clase de la sección {classroomConflicted.Section.Code} del curso {classroomConflicted.Section.CourseTerm.Course.FullName}.");
                }

                var teachers = await _teacherScheduleService.GetAllByClassSchedule(classSchedule.Id);

                var allowTeacherTimeCrossing = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.TeacherManagement.ALLOW_TEACHER_TIME_CROSSING));

                if (teachers != null && teachers.Any())
                {
                    foreach (var tid in teachers.Select(y => y.TeacherId).ToList())
                    {
                        var teacher = (await _userService.Get(tid)).RawFullName;

                        if (!allowTeacherTimeCrossing)
                        {
                            var conflictedClass = await _teacherScheduleService.GetConflictedClass(tid, dayOfWeek, st, et, courseTerm.TermId, classSchedule.Id);
                            if (conflictedClass != null)
                                return BadRequest(
                                    $"El profesor {teacher} está ocupado en ese rango de horas por una clase de la sección {conflictedClass.ClassSchedule.Section.Code} del curso {conflictedClass.ClassSchedule.Section.CourseTerm.Course.FullName}.");
                        }
                    }
                }

                classSchedule.HasBeenRescheduled = true;
                classSchedule.StartTime = st;
                classSchedule.EndTime = et;
                classSchedule.WeekDay = dayOfWeek;

                var @classes = await _classService.GetClassesByByClassScheduleAndSectionIdAndClassroomId(classSchedule.Id, classSchedule.SectionId, classSchedule.ClassroomId);
                @classes = @classes.Where(x => x.StartTime > @class.StartTime).ToList();

                var startTime = classReschedule.StartDateTime;
                var endTime = classReschedule.EndDateTime;

                foreach (var item in classes)
                {
                    startTime = startTime.AddDays(7);
                    endTime = endTime.AddDays(7);
                    item.StartTime = startTime;
                    item.EndTime = endTime;
                    item.WeekNumber = dayOfWeek;
                    item.IsRescheduled = true;
                }
            }

            await _classService.Update(@class);
            await _classRescheduleService.Update(classReschedule);
            return Ok();
        }

        /// <summary>
        /// Método para denegar una solicitud de reprogramación de clase
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la reprogramación de clase</param>
        /// <returns>Código de estado HTTP</returns>
        [Route("denegar/post")]
        [HttpPost]
        public async Task<IActionResult> DenyClassReschedule(ClassRescheduleViewModel model)
        {
            var classReschedule = await _classRescheduleService.Get(model.Id);
            classReschedule.Status = ConstantHelpers.CLASS_RESCHEDULE.STATUS.NOT_APPLICABLE;
            await _classRescheduleService.Update(classReschedule);
            return Ok();
        }
    }
}
