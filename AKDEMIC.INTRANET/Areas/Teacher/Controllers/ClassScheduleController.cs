using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Areas.Teacher.Models.ClassScheduleViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System.Collections.Generic;
using AKDEMIC.CORE.Services;
using DinkToPdf.Contracts;
using AKDEMIC.INTRANET.Model;
using Microsoft.Extensions.Options;

namespace AKDEMIC.INTRANET.Areas.Teacher.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.TEACHERS)]
    [Area("Teacher")]
    [Route("profesor/horario-ciclo")]
    public class ClassScheduleController : BaseController
    {
        private readonly AkdemicContext _context;
        private readonly ITeacherService _teacherService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IAcademicDepartmentService _academicDepartmentService;
        private readonly ITextSharpService _textSharpService;
        private readonly IWorkerLaborCategoryService _workerLaborCategoryService;
        private readonly ITeacherScheduleService _teacherScheduleService;
        private readonly INonTeachingLoadService _nonTeachingLoadService;
        private readonly IClassService _classService;
        private readonly ICareerService _careerService;
        private readonly IWorkerLaborConditionService _workerLaborConditionService;
        private readonly IWorkerLaborInformationService _workerLaborInformationService;
        private readonly IFacultyService _facultyService;
        private readonly IExtraTeachingLoadService _extraTeachingLoadService;
        private readonly IConfigurationService _configurationService;
        private readonly IViewRenderService _viewRenderService;
        private readonly IConverter _converter;
        private readonly INonTeachingLoadScheduleService _nonTeachingLoadScheduleService;
        private readonly IClassScheduleService _classScheduleService;
        protected ReportSettings _reportSettings;

        public ClassScheduleController(IUserService userService,
            ITermService termService,
            AkdemicContext context,
            ITeacherService teacherService,
            IClassService classService,
            ICareerService careerService,
            IWorkerLaborConditionService workerLaborConditionService,
            IWorkerLaborInformationService workerLaborInformationService,
            IFacultyService facultyService,
            IExtraTeachingLoadService extraTeachingLoadService,
            IConfigurationService configurationService,
            IViewRenderService viewRenderService,
            IConverter converter,
            IWebHostEnvironment environment,
            IAcademicDepartmentService academicDepartmentService,
            ITextSharpService textSharpService,
            IWorkerLaborCategoryService workerLaborCategoryService,
            ITeacherScheduleService teacherScheduleService,
            INonTeachingLoadService nonTeachingLoadService,
            INonTeachingLoadScheduleService nonTeachingLoadScheduleService,
            IClassScheduleService classScheduleService,
            IOptionsSnapshot<ReportSettings> reportSettings
        ) : base(userService, termService)
        {
            _context = context;
            _teacherService = teacherService;
            _classService = classService;
            _careerService = careerService;
            _workerLaborConditionService = workerLaborConditionService;
            _workerLaborInformationService = workerLaborInformationService;
            _facultyService = facultyService;
            _extraTeachingLoadService = extraTeachingLoadService;
            _configurationService = configurationService;
            _viewRenderService = viewRenderService;
            _converter = converter;
            _hostingEnvironment = environment;
            _academicDepartmentService = academicDepartmentService;
            _textSharpService = textSharpService;
            _workerLaborCategoryService = workerLaborCategoryService;
            _teacherScheduleService = teacherScheduleService;
            _nonTeachingLoadService = nonTeachingLoadService;
            _nonTeachingLoadScheduleService = nonTeachingLoadScheduleService;
            _classScheduleService = classScheduleService;
            _reportSettings = reportSettings.Value;
        }

        /// <summary>
        /// Vista principal donde se encuentra el horario del docente
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View(new IndexViewModel());
        }

        /// <summary>
        /// Método para obtener la carga academica lectiva y no lectiva del docente logueado
        /// </summary>
        /// <param name="start">Fecha de inicio</param>
        /// <param name="end">Fecha fin</param>
        /// <returns>Horario del docente</returns>
        [Route("get")]
        public async Task<IActionResult> GetTeacherSchedules(DateTime start, DateTime end)
        {
            var term = await GetActiveTerm();
            if (term == null)
                term = new ENTITIES.Models.Enrollment.Term();
            var userId = GetUserId();

            var result = await _teacherService.GetTeacherCompleteSchedule(term.Id, userId, start.AddDays(-1), end.AddDays(1));

            return Ok(result);
        }

        /// <summary>
        /// Método para obtener el detalle de la clase
        /// </summary>
        /// <param name="id">Identificador de la clase</param>
        /// <returns>Objeto que contiene los datos de la clase</returns>
        [Route("{id}/get")]
        public async Task<IActionResult> GetTeacherSchedule(Guid id)
        {
            var term = await GetActiveTerm();
            var userId = GetUserId();

            var @class = await _classService.GetAsModelAByIdAndTeacherId(id, userId);

            if (@class != null)
            {
                return Ok(@class);
            }
            else
            {
                var nonTeachingLoad = await _nonTeachingLoadScheduleService.GetScheduleObjectDetail(id);
                return Ok(nonTeachingLoad);
            }
        }

        /// <summary>
        /// Reporte de carga académica del docente logueado
        /// </summary>
        /// <returns>Archivo en formato PDF</returns>
        [HttpGet("carga-academica")]
        public async Task<IActionResult> AcademicCharge()
        {
            var term = await GetActiveTerm();
            var termId = term.Id;
            var teacherId = GetUserId();

            var model = new AcademicChargeViewModel();

            model.HeaderText = _reportSettings.AcademicChargeHeaderText;

            var teacherAcademicChargue = await _context.TeacherAcademicCharges.Where(x => x.TeacherId == teacherId && x.TermId == termId)
                .FirstOrDefaultAsync();

            if (teacherAcademicChargue == null)
            {
                model.Status = "Pendiente";
            }
            else
            {
                model.Observation = teacherAcademicChargue.Observation;

                if (teacherAcademicChargue.IsValidated)
                {
                    model.Status = "Validado";
                }
                else
                {
                    model.Status = "Con Observaciones";
                }
            }

            model.Image = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png");
            model.academicYear = term.Year;
            model.cycle = term.Number;
            model.term = term.Name;

            var teacher = await _teacherService.GetAsync(teacherId);
            teacher.User = await _userService.Get(teacherId);

            ///////////////////////////////////
            if (teacher.AcademicDepartmentId.HasValue && teacher.AcademicDepartmentId != Guid.Empty)
            {
                var academicdepartment = await _academicDepartmentService.Get(teacher.AcademicDepartmentId.Value);
                if (academicdepartment != null)
                {
                    model.academicDepartment = academicdepartment.Name;
                    var faculty = await _facultyService.Get(academicdepartment.FacultyId);
                    model.facultyTeacher = faculty.Name;
                }
            }

            if (teacher.CareerId.HasValue)
            {
                var teachercareer = await _careerService.Get(teacher.CareerId.Value);
                model.careerTeacher = teachercareer.Name;
            }

            ///////////////////////////////////
            model.fullName = teacher.User.FullName;
            model.teachercode = teacher.User.UserName;

            if (teacher.TeacherDedicationId.HasValue)
            {
                model.dedication = await _context.TeacherDedication.Where(x => x.Id == teacher.TeacherDedicationId).Select(x => x.Name).FirstOrDefaultAsync();
            }
            
            model.condition = "Sin Asignar";
            model.category = "Sin Asignar";
            model.teacherName = teacher.User.FullName;
            var workerLaborInformation = await _workerLaborInformationService.GetByUserId(teacherId);

            int.TryParse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.Enrollment.PEDAGOGICAL_HOUR_TIME), out int pedagogical_hour_time);

            if (workerLaborInformation != null)
            {
                if (workerLaborInformation.WorkerLaborConditionId.HasValue)
                    workerLaborInformation.WorkerLaborCondition = await _workerLaborConditionService.Get(workerLaborInformation.WorkerLaborConditionId.Value);

                if (workerLaborInformation.WorkerLaborCategoryId.HasValue)
                    workerLaborInformation.WorkerLaborCategory = await _workerLaborCategoryService.GetAsync(workerLaborInformation.WorkerLaborCategoryId.Value);

                if (workerLaborInformation.WorkerLaborCategory != null)
                    model.category = workerLaborInformation.WorkerLaborCategory.Name;

                if (workerLaborInformation.WorkerLaborCondition != null)
                    model.condition = workerLaborInformation.WorkerLaborCondition.Name;

                if (workerLaborInformation.AcademicDepartmentId.HasValue)
                {
                    var academicDepartment = await _academicDepartmentService.Get(workerLaborInformation.AcademicDepartmentId.Value);
                    model.academicDepartment = academicDepartment.Name;

                    var academicDepartmentDirector = await _context.Users.Where(x => x.Id == academicDepartment.AcademicDepartmentDirectorId).Select(x => x.FullName).FirstOrDefaultAsync();
                    var facultyAcademicDepartment = await _context.Faculties.Where(x => x.Id == academicDepartment.FacultyId).FirstOrDefaultAsync();

                    model.AcademicDepartmentDirector = academicDepartmentDirector;

                    if (facultyAcademicDepartment != null)
                    {
                        model.facultyTeacher = facultyAcademicDepartment.Name;
                        var dean = await _context.Users.Where(x => x.Id == facultyAcademicDepartment.DeanId).Select(x => x.FullName).FirstOrDefaultAsync();
                        model.DeanFaculty = dean;
                    }
                }
            }
            else
            {
                model.academicDepartment = string.IsNullOrEmpty(model.academicDepartment) ? "-" : model.academicDepartment;
                model.facultyTeacher = string.IsNullOrEmpty(model.facultyTeacher) ? "-" : model.facultyTeacher;
            }

            var degree = await _context.WorkerBachelorDegrees.Where(x => x.UserId == teacher.UserId).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();
            var title = await _context.WorkerProfessionalTitles.Where(x => x.UserId == teacher.UserId).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();
            var master = await _context.WorkerMasterDegrees.Where(x => x.UserId == teacher.UserId).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();
            var doctoral = await _context.WorkerDoctoralDegrees.Where(x => x.UserId == teacher.UserId).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();

            model.TeacherTitle = title?.Name;

            if (doctoral != null)
            {
                model.HighestAcademicDegree = "Grado de Doctor:";
                model.HighestAcademicDegreeDescription = doctoral.Specialty;
            }
            else if (master != null)
            {
                model.HighestAcademicDegree = "Grado de Magíster:";
                model.HighestAcademicDegreeDescription = master.Name;

            }
            else if (degree != null)
            {
                model.HighestAcademicDegree = "Grado Académico:";
                model.HighestAcademicDegreeDescription = degree.Name;
            }

            var sections = await _teacherScheduleService.GetAllAsTemplateA(term.Id, teacherId);

            Int16 cont = 0;
            Decimal sumatot = 0M;

            foreach (var sect in sections)
            {
                cont++;
                model.Sections.Add(new SectionViewModel
                {
                    SectionId = sect.SectionId,
                    counter = "L" + cont,
                    course = sect.Course,
                    CourseCode = sect.CourseCode,
                    CourseName = sect.CourseName,
                    section = sect.Section,
                    classroom = sect.Classroom,
                    career = sect.Career,
                    AcademicYears = sect.AcademicYear,
                    students = sect.Students,
                    hourPractice = sect.PracticalHours.ToString("0.00"),
                    hourPracticeNumber = sect.PracticalHours,
                    hourTeoric = sect.TeoricHours.ToString("0.00"),
                    hourTeoricNumber = sect.TeoricHours,
                    totalHoursNumber = (sect.PracticalHours + sect.TeoricHours),
                    totalHours = (sect.PracticalHours + sect.TeoricHours).ToString("0.00"),
                    CourseTermModality = sect.CourseTermModality,
                });
                sumatot += (Decimal)(sect.PracticalHours + sect.TeoricHours);
            }

            var extraTeachingLoad = await _extraTeachingLoadService.GetExtraTeachingLoad(term.Id, teacherId);

            if (extraTeachingLoad != null)
            {
                if (extraTeachingLoad.EvaluationHours != 0)
                {
                    model.Sections.Add(new SectionViewModel
                    {
                        course = "HORAS DE EVALUACIÓN",
                        AcademicYears = "-",
                        students = 0,
                        SectionId = Guid.NewGuid(),
                        section = "-",
                        turn = "-",
                        career = "-",
                        CourseTermModality = "-",
                        totalHoursNumber = (double)extraTeachingLoad.EvaluationHours,
                        totalHours = extraTeachingLoad.EvaluationHours.ToString("0.00")
                    });

                    sumatot += extraTeachingLoad.EvaluationHours;
                }

                if (extraTeachingLoad.OtherAcademicActivities != 0)
                {
                    model.Sections.Add(new SectionViewModel
                    {
                        course = "OTRAS ACTIVIDADES ACADÉMICAS",
                        AcademicYears = "-",
                        students = 0,
                        SectionId = Guid.NewGuid(),
                        section = "-",
                        turn = "-",
                        career = "-",
                        CourseTermModality = "-",
                        totalHoursNumber = (double)extraTeachingLoad.OtherAcademicActivities,
                        totalHours = extraTeachingLoad.OtherAcademicActivities.ToString("0.00")
                    });

                    sumatot += extraTeachingLoad.OtherAcademicActivities;
                }
            }

            model.totalHorasLectivas = sumatot.ToString("0.00");
            model.totalHoras = sumatot;
            var nonActivities = await _nonTeachingLoadService.GetAll(teacherId);

            sumatot = 0;
            foreach (var na in nonActivities)
            {
                cont++;

                var hours = (decimal)na.NonTeachingLoadSchedules.Sum(x => (x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalHours));

                if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG)
                {
                    if (na.TeachingLoadType.Category == ConstantHelpers.TEACHING_LOAD.CATEGORY.COMPLEMENTARY)
                    {
                        hours = (decimal)(na.NonTeachingLoadSchedules.Sum(x => (x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalMinutes) / (double)pedagogical_hour_time));
                    }
                }

                model.NonActivities.Add(new NonActivityViewModel
                {
                    TeachingLoadTypeId = na.TeachingLoadTypeId,
                    Resolution = na.Resolution,
                    Location = na.Location,
                    StartDate = na.StartDate.HasValue ? na.StartDate.Value.ToLocalDateFormat() : " ",
                    EndDate = na.EndDate.HasValue ? na.EndDate.Value.ToLocalDateFormat() : " ",
                    Category = na.TeachingLoadType.Category,
                    Name = na.Name,
                    Unit = na.TeachingLoadType.Name,
                    Hours = hours.ToString("0.00"),
                    Monday = na.NonTeachingLoadSchedules.Where(x => ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay) == DayOfWeek.Monday).Sum(x => (x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalHours)).ToString("0.00"),
                    Tuesday = na.NonTeachingLoadSchedules.Where(x => ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay) == DayOfWeek.Tuesday).Sum(x => (x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalHours)).ToString("0.00"),
                    Wednesday = na.NonTeachingLoadSchedules.Where(x => ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay) == DayOfWeek.Wednesday).Sum(x => (x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalHours)).ToString("0.00"),
                    Thursday = na.NonTeachingLoadSchedules.Where(x => ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay) == DayOfWeek.Thursday).Sum(x => (x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalHours)).ToString("0.00"),
                    Friday = na.NonTeachingLoadSchedules.Where(x => ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay) == DayOfWeek.Friday).Sum(x => (x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalHours)).ToString("0.00"),
                    Saturday = na.NonTeachingLoadSchedules.Where(x => ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay) == DayOfWeek.Saturday).Sum(x => (x.EndTime.ToLocalTimeSpanUtc().Subtract(x.StartTime.ToLocalTimeSpanUtc()).TotalHours)).ToString("0.00"),

                    MondayRange = string.Join("<br>", na.NonTeachingLoadSchedules.Where(x => ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay) == DayOfWeek.Monday).Select(x => $"{x.StartTime.ToLocalDateTimeFormatUtc()}-{x.EndTime.ToLocalDateTimeFormatUtc()}").ToArray()),
                    TuesdayRange = string.Join("<br>", na.NonTeachingLoadSchedules.Where(x => ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay) == DayOfWeek.Tuesday).Select(x => $"{x.StartTime.ToLocalDateTimeFormatUtc()}-{x.EndTime.ToLocalDateTimeFormatUtc()}").ToArray()),
                    WednesdayRange = string.Join("<br>", na.NonTeachingLoadSchedules.Where(x => ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay) == DayOfWeek.Wednesday).Select(x => $"{x.StartTime.ToLocalDateTimeFormatUtc()}-{x.EndTime.ToLocalDateTimeFormatUtc()}").ToArray()),
                    ThursdayRange = string.Join("<br>", na.NonTeachingLoadSchedules.Where(x => ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay) == DayOfWeek.Thursday).Select(x => $"{x.StartTime.ToLocalDateTimeFormatUtc()}-{x.EndTime.ToLocalDateTimeFormatUtc()}").ToArray()),
                    FridayRange = string.Join("<br>", na.NonTeachingLoadSchedules.Where(x => ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay) == DayOfWeek.Friday).Select(x => $"{x.StartTime.ToLocalDateTimeFormatUtc()}-{x.EndTime.ToLocalDateTimeFormatUtc()}").ToArray()),
                    SaturdayRange = string.Join("<br>", na.NonTeachingLoadSchedules.Where(x => ConstantHelpers.WEEKDAY.TO_ENUM(x.WeekDay) == DayOfWeek.Saturday).Select(x => $"{x.StartTime.ToLocalDateTimeFormatUtc()}-{x.EndTime.ToLocalDateTimeFormatUtc()}").ToArray()),
                });
                sumatot += hours;
            }

            model.totalHorasNoLectivas = sumatot.ToString("0.00");
            model.totalHoras += sumatot;
            model.Image = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png");

            //Schedule Parameters
            model.ScheduleParameters = new List<(TimeSpan, TimeSpan, string)>();
            int start_time = 7;
            int end_time = 23;

            for (int i = start_time; i < end_time; i++)
            {
                model.ScheduleParameters.Add((new TimeSpan(0, i, 0, 0, 0), new TimeSpan(0, i + 1, 0, 0), $"{i:00}.00 - {i + 1:00}.00"));
            }

            var restes = await _classScheduleService.GetAllAsModelA(term.Id, teacherId);

            var restesList = restes.ToList();

            foreach (var item in restes)
            {
                var hourStarTime = item.StartTime.Hours;
                var hourEndTime = item.EndTime.Minutes > 0 ? item.EndTime.Hours + 1 : item.EndTime.Hours;

                var hourStarTimeLocal = item.StartTime.ToLocalTimeSpanUtc().Hours;
                var hourEndTimeLocal = item.EndTime.Minutes > 0 ? item.EndTime.ToLocalTimeSpanUtc().Hours + 1 : item.EndTime.ToLocalTimeSpanUtc().Hours;

                var totalParts = hourEndTimeLocal - hourStarTimeLocal;
                if (totalParts > 1)
                {
                    var tpmTimeSpam = new TimeSpan(hourStarTime, 0, 0);

                    for (int i = 0; i < totalParts; i++)
                    {
                        var startTime = new TimeSpan();
                        var entTime = new TimeSpan();

                        if (i == 0)
                        {
                            tpmTimeSpam = tpmTimeSpam.Add(TimeSpan.FromHours(1));
                            startTime = item.StartTime;
                            entTime = tpmTimeSpam;
                        }
                        else if (i != totalParts - 1)
                        {
                            startTime = tpmTimeSpam;
                            tpmTimeSpam = tpmTimeSpam.Add(TimeSpan.FromHours(1));
                            entTime = tpmTimeSpam;
                        }
                        else
                        {
                            startTime = tpmTimeSpam;
                            entTime = item.EndTime;
                        }

                        restesList.Add(new REPOSITORY.Repositories.Intranet.Templates.ClassSchedule.ClassScheduleTemplateA
                        {
                            SectionId = item.SectionId,
                            Code = item.Code,
                            EndTime = entTime,
                            StartTime = startTime,
                            WeekDay = item.WeekDay,
                        });
                    }

                    restesList.Remove(item);
                }
            }

            var classSchedule = restesList.Select(x => new ClassSchedule
            {
                StartTime = x.StartTime,
                StartTimeLocal = x.StartTime.ToLocalTimeSpanUtc(),
                Code = x.Code,
                EndTime = x.EndTime,
                EndTimeLocal = x.EndTime.ToLocalTimeSpanUtc(),
                //TimeText = x.TimeText,
                SectionId = x.SectionId,
                TimeText = $"{x.StartTime.ToLocalDateTimeFormatUtc()} - {x.EndTime.ToLocalDateTimeFormatUtc()}",
                WeekDay = x.WeekDay,
                SessionType = x.SessionType
            }).ToList();
            model.ClassSchedules = classSchedule;

            var earlyShiftEnd = await _configurationService.GetByKey(ConstantHelpers.Configuration.TeacherManagement.EARLY_SHIFT_END);
            var lateShiftEnd = await _configurationService.GetByKey(ConstantHelpers.Configuration.TeacherManagement.LATE_SHIFT_END);
            var nightShiftEnd = await _configurationService.GetByKey(ConstantHelpers.Configuration.TeacherManagement.NIGHT_SHIFT_END);

            var shifts = new List<Tuple<TimeSpan, string>>();
            if (earlyShiftEnd != null && !string.IsNullOrEmpty(earlyShiftEnd.Value))
                shifts.Add(new Tuple<TimeSpan, string>(ConvertHelpers.TimepickerToTimeSpan(earlyShiftEnd.Value), "M"));

            if (lateShiftEnd != null && !string.IsNullOrEmpty(lateShiftEnd.Value))
                shifts.Add(new Tuple<TimeSpan, string>(ConvertHelpers.TimepickerToTimeSpan(lateShiftEnd.Value), "T"));

            if (nightShiftEnd != null && !string.IsNullOrEmpty(nightShiftEnd.Value))
                shifts.Add(new Tuple<TimeSpan, string>(ConvertHelpers.TimepickerToTimeSpan(nightShiftEnd.Value), "N"));

            model.Sections = model.Sections
                //.GroupBy(x => new { x.section, x.classroom, x.course, x.career })
                .GroupBy(x => x.SectionId)
                .Select(x => new SectionViewModel
                {
                    SectionId = x.Key,
                    course = x.Select(y => y.course).FirstOrDefault(),
                    section = x.Select(y => y.section).FirstOrDefault(),
                    classroom = x.Select(y => y.classroom).FirstOrDefault(),
                    CourseCode = x.Select(y => y.CourseCode).FirstOrDefault(),
                    CourseName = x.Select(y => y.CourseName).FirstOrDefault(),
                    //course = x.Key.course,
                    //section = x.Key.section,
                    //classroom = x.Key.classroom,
                    AcademicYears = x.Select(x => x.AcademicYears).FirstOrDefault(),
                    career = x.Select(y => y.career).FirstOrDefault(),
                    year = term.Year,
                    students = x.Select(y => y.students).FirstOrDefault(),
                    hourPractice = x.Select(y => y.hourPracticeNumber).Sum().ToString("0.00"),
                    hourTeoric = x.Select(y => y.hourTeoricNumber).Sum().ToString("0.00"),
                    totalHours = x.Select(y => y.totalHoursNumber).Sum().ToString("0.00"),
                    turn = GetClassShift(classSchedule.Where(z => z.SectionId == x.Select(y => y.SectionId).FirstOrDefault())
                    .Select(x => x.StartTimeLocal).FirstOrDefault(), shifts),
                    CourseTermModality = x.Select(y => y.CourseTermModality).FirstOrDefault(),
                    ClassSchedules = restes.Where(y => y.SectionId == x.Key)
                    .Select(y => new ClassSchedule
                    {
                        StartTime = y.StartTime,
                        StartTimeLocal = y.StartTime.ToLocalTimeSpanUtc(),
                        Code = y.Code,
                        EndTime = y.EndTime,
                        EndTimeLocal = y.EndTime.ToLocalTimeSpanUtc(),
                        SectionId = y.SectionId,
                        TimeText = $"{y.StartTime.ToLocalDateTimeFormatUtc()} - {y.EndTime.ToLocalDateTimeFormatUtc()}",
                        WeekDay = y.WeekDay,
                        SessionType = y.SessionType,
                        TotalHours = y.EndTime.ToLocalDateTimeUtc().TimeOfDay.Subtract(y.StartTime.ToLocalDateTimeUtc().TimeOfDay).TotalMinutes / pedagogical_hour_time,
                        SectionGroup = y.SectionGroup,
                        SectionGroupStudentsCount = y.SectionGroupStudentsCount
                    })
                    .ToList()
                })
                .ToList();

            model.AuthoritySignature = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.DOCUMENT_ACADEMIC_CHARGE);

            var globalSettings = new DinkToPdf.GlobalSettings();

            var objectSettings = new List<DinkToPdf.ObjectSettings>();

            if (
                ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG ||
                ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.Akdemic
                )
            {
                globalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Landscape,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 15, Left = 5, Right = 5 },
                    DocumentTitle = $"Reporte Carga Académica",
                };
                var footerSettings = new DinkToPdf.FooterSettings
                {
                    FontName = "Arial",
                    FontSize = 6,
                    Spacing = 10,
                    Line = true,
                    Left = $"Fecha de impresion: {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    //Right = "Pág. [page]",
                };

                var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Teacher/Views/ClassSchedule/ReportUNJBG.cshtml", model);
                var partialview = await _viewRenderService.RenderToStringAsync("/Areas/Teacher/Views/ClassSchedule/_ReportUNJBG.cshtml", model);


                objectSettings.Add(new DinkToPdf.ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = viewToString,
                    WebSettings = { DefaultEncoding = "utf-8" },
                    FooterSettings = footerSettings
                });

                objectSettings.Add(new DinkToPdf.ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = partialview,
                    WebSettings = { DefaultEncoding = "utf-8" },
                    FooterSettings = footerSettings
                });
            }
            else
            {
                globalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = $"Carga Académica de {model.teacherName}"
                };

                var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Teacher/Views/ClassSchedule/AcademicChargePDF.cshtml", model);

                objectSettings.Add(new DinkToPdf.ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = viewToString,
                    WebSettings = { DefaultEncoding = "utf-8" },
                });
            }

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings
            };

            pdf.Objects.AddRange(objectSettings);

            var fileByte = _converter.Convert(pdf);

            fileByte = _textSharpService.RemoveEmptyPages(fileByte, 4000);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileByte, "application/pdf");
        }

        /// <summary>
        /// Método para obtener el turno del horario de clases dictado por el docente
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="shifts"></param>
        /// <returns></returns>
        private string GetClassShift(TimeSpan startTime, List<Tuple<TimeSpan, string>> shifts)
        {
            foreach (var item in shifts)
            {
                if (startTime < item.Item1)
                    return item.Item2;
            }

            return "-";
        }

    }
}
