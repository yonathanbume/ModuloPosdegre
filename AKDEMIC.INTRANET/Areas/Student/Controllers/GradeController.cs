using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Areas.Student.Models.GradeViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.INTRANET.Filters.Permission;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.REPOSITORY.Data;
using DinkToPdf;
using DinkToPdf.Contracts;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using AKDEMIC.CORE.Extensions;
using System.IO;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Options;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;

namespace AKDEMIC.INTRANET.Areas.Student.Controllers
{
    //[StudentAuthorizationAttribute]
    [Authorize(Roles = ConstantHelpers.ROLES.STUDENTS)]
    [Area("Student")]
    [Route("alumno/notas")]
    public class GradeController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly IStudentService _studentService;
        private readonly IConverter _dinkConverter;
        private readonly AkdemicContext _context;
        private readonly ICourseSyllabusService _courseSyllabusService;
        private readonly IConfigurationService _configurationService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IViewRenderService _viewRenderService;
        private readonly ITeacherSectionService _teacherSectionService;
        private readonly IClassScheduleService _classScheduleService;
        private readonly IAcademicYearCoursePreRequisiteService _academicYearCoursePreRequisiteService;
        private readonly ICourseTermService _courseTermService;
        private readonly ICourseSyllabusWeekService _courseSyllabusWeekService;
        private readonly IDirectedCourseService _directedCourseService;
        private readonly ISyllabusTeacherService _syllabusTeacherService;
        private readonly ICareerService _careerService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IFacultyService _facultyService;
        private readonly IAcademicProgramService _academicProgramService;
        private readonly ICourseSyllabusTeacherService _courseSyllabusTeacherService;
        private readonly ISectionEvaluationService _sectionEvaluationService;
        private readonly ITeacherService _teacherService;
        private readonly IAcademicYearCourseService _academicYearCourseService;

        public GradeController(IUserService userService,
            ITermService termService,
            IStudentService studentService,
            IConverter dinkConverter,
            IWebHostEnvironment hostingEnvironment,
            AkdemicContext context,
            ICourseSyllabusService courseSyllabusService,
            IConfigurationService configurationService,
            IAcademicHistoryService academicHistoryService,
            IStudentSectionService studentSectionService,
            IViewRenderService viewRenderService,
            ITeacherSectionService teacherSectionService,
            IClassScheduleService classScheduleService,
            IAcademicYearCoursePreRequisiteService academicYearCoursePreRequisiteService,
            ICourseTermService courseTermService,
            ICourseSyllabusWeekService courseSyllabusWeekService,
            IDirectedCourseService directedCourseService,
            ISyllabusTeacherService syllabusTeacherService,
            ICareerService careerService,
            IOptions<CloudStorageCredentials> storageCredentials,
            IFacultyService facultyService,
            IAcademicProgramService academicProgramService,
            ICourseSyllabusTeacherService courseSyllabusTeacherService,
            ISectionEvaluationService sectionEvaluationService,
            ITeacherService teacherService,

            IAcademicYearCourseService academicYearCourseService)
            : base(userService, termService)
        {
            _studentService = studentService;
            _dinkConverter = dinkConverter;
            _context = context;
            _courseSyllabusService = courseSyllabusService;
            _configurationService = configurationService;
            _academicHistoryService = academicHistoryService;
            _studentSectionService = studentSectionService;
            _viewRenderService = viewRenderService;
            _teacherSectionService = teacherSectionService;
            _classScheduleService = classScheduleService;
            _academicYearCoursePreRequisiteService = academicYearCoursePreRequisiteService;
            _courseTermService = courseTermService;
            _courseSyllabusWeekService = courseSyllabusWeekService;
            _directedCourseService = directedCourseService;
            _syllabusTeacherService = syllabusTeacherService;
            _careerService = careerService;
            _storageCredentials = storageCredentials;
            _facultyService = facultyService;
            _academicProgramService = academicProgramService;
            _courseSyllabusTeacherService = courseSyllabusTeacherService;
            _sectionEvaluationService = sectionEvaluationService;
            _teacherService = teacherService;
            _academicYearCourseService = academicYearCourseService;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Vista donde se muestra las notas del alumno logueado
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public async Task<IActionResult> Index()
        {
            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);
            var terms = await _termService.GetTermsByStudentSections(student.Id);

            var model = new IndexViewModel()
            {
                HasStudentInformation = student.StudentInformationId != null,
                Student = new StudentViewModel()
                {
                    FullName = student.User.FullName,
                    UserName = student.User.UserName,
                    Career = new CareerViewModel()
                    {
                        Name = student.Career.Name
                    }
                },
                ActiveTerm = null,
                Terms = terms.OrderByDescending(x => x.Name)
                .Select(x => new TermViewModel()
                {
                    Name = x.Name,
                    MinimumValue = x.MinGrade,
                    Id = x.Id
                }).ToList()
            };
            model.ActiveTerm = model.Terms.FirstOrDefault()?.Id;
            return View(model);
        }

        /// <summary>
        /// Obtiene la vista parcial donde se detalla las notas del alumnos logueado
        /// </summary>
        /// <param name="pid">identificador del periodo académico</param>
        /// <returns>Vista parcial</returns>
        [Route("periodo/{pid}/get")]
        public async Task<IActionResult> GetStudentGrades(Guid pid)
        {
            if (pid.Equals(Guid.Empty))
                return BadRequest($"No se pudo encontrar un Periodo Académico con el id {pid}.");

            var term = await _termService.Get(pid);
            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);
            var configuration = await _configurationService.GetByKey(ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT);

            if (configuration is null)
            {
                configuration = new ENTITIES.Models.Configuration
                {
                    Key = ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT,
                    Value = ConstantHelpers.Configuration.TeacherManagement.DEFAULT_VALUES[ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_UNIT]
                };
            }

            bool.TryParse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.TeacherManagement.EVALUATIONS_BY_SECTION), out var evaluationsBySection);

            var evaluationByUnits = Convert.ToBoolean(configuration.Value);

            if (term != null)
            {
                var studentSections = await _context.StudentSections.Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == term.Id).ToListAsync();
                var courses = new List<EnrolledCourseViewModel>();

                foreach (var studentSection in studentSections)
                {
                    var section = await _context.Sections.Where(x => x.Id == studentSection.SectionId).FirstOrDefaultAsync();
                    var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();
                    var course = await _context.Courses.Where(x => x.Id == courseTerm.CourseId).FirstOrDefaultAsync();

                    var syllabusTeacher = await _context.SyllabusTeachers.Where(x => x.SyllabusRequest.TermId == term.Id && x.CourseTermId == courseTerm.Id && x.Status == ConstantHelpers.SYLLABUS_TEACHER.STATUS.PRESENTED).FirstOrDefaultAsync();

                    var grades = await _context.Grades.Where(x => x.StudentSectionId == studentSection.Id).ToListAsync();

                    var academicYear = await _context.AcademicYearCourses.Where(x => x.CourseId == course.Id && x.CurriculumId == student.CurriculumId).Select(x => x.AcademicYear).FirstOrDefaultAsync();

                    var substituteExam = await _context.SubstituteExams.Where(x => x.SectionId == studentSection.SectionId && x.StudentId == studentSection.StudentId && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED).FirstOrDefaultAsync();

                    var modeltpm = new EnrolledCourseViewModel
                    {
                        Syllabus = new CourseSyllabusViewModel
                        {
                            Enabled = false
                        },
                        CurriculumAcademicYear = academicYear,
                        EvaluationByUnits = evaluationByUnits,
                        StudentSection = new StudentSectionViewModel
                        {
                            Id = studentSection.Id,
                            SubstituteExamFinalGrade = substituteExam?.ExamScore,
                            FinalGrade = studentSection.FinalGrade,
                            Observations = studentSection.Observations,
                            MinGradeTerm = term.MinGrade,
                            Try = studentSection.Try,
                            Status = studentSection.Status,
                            Section = new SectionViewModel
                            {
                                Code = section.Code,
                                CourseTerm = new CourseTermViewModel
                                {
                                    Credits = course.Credits,
                                    Course = new CourseViewModel
                                    {
                                        FullName = course.FullName
                                    }
                                }
                            }
                        }
                    };

                    if (syllabusTeacher != null)
                    {
                        modeltpm.Syllabus = new CourseSyllabusViewModel
                        {
                            Enabled = true,
                            SyllabusTeacherId = syllabusTeacher.Id
                            //CourseId = courseTerm.CourseId,
                            //TermId = courseTerm.TermId,
                            //IsDigital = syllabusTeacher.IsDigital,
                            //UrlFile = syllabusTeacher.Url,
                            //CurriculumId = student.CurriculumId
                        };
                    }

                    if (evaluationByUnits)
                    {
                        var courseUnits = await _context.CourseUnits.Where(x => x.CourseSyllabus.CourseId == courseTerm.CourseId && x.CourseSyllabus.TermId == courseTerm.TermId).Include(x => x.Evaluations).ToListAsync();

                        modeltpm.StudentSection.Section.CourseUnits = courseUnits.OrderBy(x => x.Number).Select(x => new CourseUnitViewModel
                        {
                            Name = x.Name,
                            Number = x.Number,
                            AcademicProgressPercentage = x.AcademicProgressPercentage,
                            Evaluations = x.Evaluations.OrderBy(y => y.Week).Select(y => new EvaluationViewModel
                            {
                                Id = y.Id,
                                Description = y.Description,
                                Name = y.Name,
                                Percentage = y.Percentage,
                                Grade = new GradeViewModel
                                {
                                    HasGrade = grades.Any(g => g.EvaluationId == y.Id),
                                    Approved = grades.Where(g => g.EvaluationId == y.Id).Select(g => g.Value).FirstOrDefault() >= term.MinGrade,
                                    Attended = grades.Where(g => g.EvaluationId == y.Id).Select(g => g.Attended).FirstOrDefault(),
                                    Value = grades.Where(g => g.EvaluationId == y.Id).Select(g => g.Value).FirstOrDefault()
                                }
                            }).ToList()
                        }).ToList();


                        if (evaluationsBySection)
                        {
                            var evaluationsBySectionsPercentages = await _context.SectionEvaluations.Where(x => x.SectionId == section.Id).ToListAsync();
                            foreach (var cu in modeltpm.StudentSection.Section.CourseUnits)
                            {
                                cu.Evaluations = cu.Evaluations
                                    .Select(x => new EvaluationViewModel
                                    {
                                        Id = x.Id,
                                        Description = x.Description,
                                        Name = x.Name,
                                        Percentage = evaluationsBySectionsPercentages.Where(y => y.EvaluationId == x.Id).Select(y => y.Percentage).FirstOrDefault(),
                                        Grade = x.Grade
                                    })
                                    .ToList();
                            }

                        }

                        var completionPercentage = 0m;

                        foreach (var cu in modeltpm.StudentSection.Section.CourseUnits)
                        {
                            var unitValue = 0M;

                            if (modeltpm.StudentSection.Section.CourseUnits.All(y => y.AcademicProgressPercentage == 0))
                            {
                                unitValue = 100M / modeltpm.StudentSection.Section.CourseUnits.Count();
                            }
                            else
                            {
                                unitValue = cu.AcademicProgressPercentage;
                            }

                            var evaluations = cu.Evaluations.Where(x => x.Grade.HasGrade).Sum(x => x.Percentage);
                            var totalEvaluation = cu.Evaluations.Sum(x => x.Percentage);

                            if (totalEvaluation == 0)
                                totalEvaluation = 1;

                            if (unitValue == 0)
                                unitValue = 1;

                            var progressByUnit = ((decimal)evaluations / (decimal)totalEvaluation) * 100M;
                            completionPercentage += (progressByUnit * (decimal)unitValue) / 100M;

                            if (cu.Evaluations.All(y => y.Grade.HasGrade))
                            {
                                var maxGrade = cu.Evaluations.Sum(x => x.Percentage) * 0.2M;
                                if (maxGrade == 0M)
                                    maxGrade = 1M;

                                var grade = cu.Evaluations.Sum(x => x.Grade.Value * (decimal)x.Percentage / 100M);
                                var gradeByUnit = 20M * grade / maxGrade;
                                cu.GradeByUnit = Math.Round(gradeByUnit, 2, MidpointRounding.AwayFromZero);
                            }
                        }

                        modeltpm.StudentSection.PercentageProgress = Math.Round(completionPercentage, 2, MidpointRounding.AwayFromZero);

                    }
                    else
                    {
                        var evaluations = await _context.Evaluations.Where(x => x.CourseTermId == courseTerm.Id).ToListAsync();

                        modeltpm.StudentSection.Section.Evaluations = evaluations.Select(x => new EvaluationViewModel
                        {
                            Id = x.Id,
                            Description = x.Description,
                            Name = x.Name,
                            Percentage = x.Percentage,
                            Grade = new GradeViewModel
                            {
                                HasGrade = grades.Any(g => g.EvaluationId == x.Id),
                                Approved = grades.Where(g => g.EvaluationId == x.Id).Select(g => g.Value).FirstOrDefault() >= term.MinGrade,
                                Attended = grades.Where(g => g.EvaluationId == x.Id).Select(g => g.Attended).FirstOrDefault(),
                                Value = grades.Where(g => g.EvaluationId == x.Id).Select(g => g.Value).FirstOrDefault()
                            }
                        }).ToList();



                        if (evaluationsBySection)
                        {
                            var evaluationsBySectionsPercentages = await _context.SectionEvaluations.Where(x => x.SectionId == section.Id).ToListAsync();
                            foreach (var eva in modeltpm.StudentSection.Section.Evaluations)
                            {
                                eva.Percentage = evaluationsBySectionsPercentages.Where(y => y.EvaluationId == eva.Id).Select(y => y.Percentage).FirstOrDefault();
                            }
                        }
                    }

                    courses.Add(modeltpm);

                }

                var model = new AcademicHistoryViewModel()
                {
                    Term = new TermViewModel()
                    {
                        Name = term.Name,
                        MinimumValue = term.MinGrade
                    },
                    EnrolledCourses = courses
                };

                return PartialView("_AcademicHistory", model);
            }
            else
            {
                return BadRequest($"No se pudo encontrar un Periodo Académico con el id {pid}.");
            }
        }

        [HttpGet("descargar-silabo")]
        public async Task<IActionResult> DonwloadSyllabus(Guid syllabusTeacherId, Guid studentSectionId)
        {
            var syllabusTeacher = await _context.SyllabusTeachers.Where(x => x.Id == syllabusTeacherId)
                .Select(x => new
                {
                    x.IsDigital,
                    x.Url
                })
                .FirstOrDefaultAsync();

            var studentSection = await _context.StudentSections.Where(x => x.Id == studentSectionId).FirstOrDefaultAsync();

            if (!studentSection.SyllabusDownloadDate.HasValue)
            {
                studentSection.SyllabusDownloadDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            var section = await _context.StudentSections.Where(x => x.Id == studentSectionId)
                  .Select(x => new
                  {
                      x.Section.CourseTerm.CourseId,
                      x.Section.CourseTerm.TermId,
                      x.Student.CurriculumId,
                      fullNameCourse = $"{x.Section.CourseTerm.Course.Code}-{x.Section.CourseTerm.Course.Name}"
                  })
                  .FirstOrDefaultAsync();

            byte[] fileBytes = null;

            if (syllabusTeacher.IsDigital)
            {
                var model = await GetModel(section.CourseId, section.TermId, section.CurriculumId);

                var settings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 },
                };

                var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Student/Views/Grade/CourseTermSyllabusPdf.cshtml", model);

                var objectSettings = new ObjectSettings()
                {
                    PagesCount = true,
                    HtmlContent = viewToString,
                    WebSettings =
                {
                    DefaultEncoding = "utf-8"
                }
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = settings,
                    Objects = { objectSettings }
                };

                fileBytes = _dinkConverter.Convert(pdf);
            }
            else
            {
                var storage = new CloudStorageService(_storageCredentials);

                using (var mem = new MemoryStream())
                {
                    await storage.TryDownload(mem, "", syllabusTeacher.Url);
                    fileBytes = mem.ToArray();
                }
            }

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            return File(fileBytes, "application/pdf", $"{section.fullNameCourse}.pdf");
        }

        private async Task<CourseTermSyllabusViewModel> GetModel(Guid courseId, Guid termId, Guid curriculumId)
        {
            var courseSyllabus = await _courseSyllabusService.GetIncludingTermAndCourse(courseId, termId);
            var confiSyllabusValidation = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.TeacherManagement.ENABLED_SYLLABUS_VALIDATION);

            var course = courseSyllabus.Course;
            var term = courseSyllabus.Term;

            var teacherSections = await _teacherSectionService.GetTeacherSectionsByTermIdAndCourseId(term.Id, course.Id);
            var classrooms = await _classScheduleService.GetAllBySections(teacherSections.Select(x => x.SectionId).ToList());
            var academicYearCourse = await _academicYearCourseService.GetWithCourseAndCurriculum(courseId, curriculumId);
            var prerequisites = await _academicYearCoursePreRequisiteService.GetAllByFilter(null, academicYearCourse?.Id, null);
            var courseTerm = await _courseTermService.GetByFilters(courseId, termId);
            var courseSyllabusWeeks = await _courseSyllabusWeekService.GetAllByCourseSyllabusId(courseSyllabus.Id);

            //Rango de semanas en base al periodo
            var termClassesRange = new List<DateTime[]>();
            var init = term.ClassStartDate.ToDefaultTimeZone().Date;
            while (init <= term.ClassEndDate.ToDefaultTimeZone().Date)
            {
                var final = init.AddDays(7).Date > term.ClassEndDate.ToDefaultTimeZone().Date ? term.ClassEndDate.ToDefaultTimeZone().Date : init.AddDays(7);
                termClassesRange.Add(new DateTime[]
                {
                    init,final
                });
                init = init.AddDays(7);
            };

            var currentUser = await _userService.GetUserByClaim(User);

            var model = new CourseTermSyllabusViewModel
            {
                CanEdit = User.IsInRole(ConstantHelpers.ROLES.SUPERADMIN)
                        || User.IsInRole(ConstantHelpers.ROLES.TEACHING_MANAGEMENT_ADMIN)
                        || User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR)
                        || courseTerm?.CoordinatorId == currentUser.Id,
                CanValidate = User.IsInRole(ConstantHelpers.ROLES.SUPERADMIN)
                        || User.IsInRole(ConstantHelpers.ROLES.TEACHING_MANAGEMENT_ADMIN)
                        || User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR)
                        || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR),
                IsCoordinator = courseTerm?.CoordinatorId == currentUser.Id,
                CourseSyllabusId = courseSyllabus.Id,
                EnabledSyllabusValidation = bool.Parse(confiSyllabusValidation),
                CourseId = course.Id,
                TermId = term.Id,
                CourseComponentId = course.CourseComponentId,
                CurriculumId = curriculumId,
                GeneralInformation = new CourseTermSyllabusGeneralInformationViewModel
                {
                    Teachers = new List<CourseTermSyllabusTeacherViewModel>(),
                    CourseName = course.Name,
                    CourseCode = course.Code,
                    PracticalHours = $"{course.PracticalHours:00}",
                    TheoreticalHours = $"{course.TheoreticalHours:00}",
                    SeminarHours = $"{course.SeminarHours:00}",
                    //VirtualHours = $"{course.VirtualHours:00}",
                    VirtualHours = $"{courseSyllabus.ListCourseUnit.Sum(y => y.VirtualHours):00}",
                    TotalHours = $"{course.TotalHours:00}",
                    Credits = $"{course.Credits:00}",
                    TermName = term.Name,
                    TermClassEnd = term.ClassEndDate.ToDefaultTimeZone(),
                    TermClassStart = term.ClassStartDate.ToDefaultTimeZone(),
                    TermYear = term.Year,
                    Area = course?.Area?.Name,
                    AreaId = course?.Area?.Id,
                    Cycle = academicYearCourse.AcademicYear,
                    Features = courseSyllabus.Features,
                    Prerequisites = (prerequisites != null && prerequisites.Any()) ? string.Join(", ",
                    prerequisites.Select(x => $"{(x.Course.Code)} - {x.Course.Name}").ToList()) : "Ninguno",
                    TotalWeeks = (int)Math.Floor((term.ClassEndDate - term.ClassStartDate).TotalDays / 7) + 1,
                    //Classroom
                    LearningEnvironment = courseSyllabus.LearningEnvironment,
                    Classrooms = classrooms.GroupBy(x => new { building = x.Classroom.Building.Name, campus = x.Classroom.Building.Campus.Name, classroom = x.Classroom.Description }).Select(x => new CourseTermSyllabusClassroomViewModel
                    {
                        Building = x.Key.classroom == "Sin Asignar" ? x.Key.classroom : x.Key.building,
                        Campus = x.Key.classroom == "Sin Asignar" ? x.Key.classroom : x.Key.campus,
                        Classroom = x.Key.classroom
                    }).ToList()
                },
                Raiting = courseSyllabus.Raiting,
                Summary = courseSyllabus.Summary,
                Competences = courseSyllabus.Competences,
                GraduateProfile = courseSyllabus.GraduateProfile,
                LearningAchievement = courseSyllabus.LearningAchievement,
                CourseSyllabusWeek = courseSyllabusWeeks.Select(x => new CourseSyllabusWeekViewModel
                {
                    EssentialKnowledge = x.EssentialKnowledge,
                    PerformanceCriterion = x.PerformanceCriterion,
                    Id = x.Id,
                    Week = x.Week
                }).ToList(),
                CourseUnits = courseSyllabus.ListCourseUnit.Select(x => new CourseUnitV2ViewModel
                {
                    Id = x.Id,
                    DevelopmentTime = GetDevelopmentTimeString(termClassesRange, x.WeekNumberStart, x.WeekNumberEnd, course.TotalHours),
                    EssentialKnowledge = x.EssentialKnowledge,
                    GradeEntryDate = x.GradeEntryDate.HasValue ? x.GradeEntryDate.ToLocalDateFormat() : "Sin Asignar",
                    LearningAchievements = x.LearningAchievements,
                    Name = x.Name,
                    AcademicProgressPercentage = x.AcademicProgressPercentage,
                    Number = x.Number,
                    PerformanceCriterion = x.PerformanceCriterion,
                    VirtualHours = $"{x.VirtualHours:00}",
                    PerformanceEvidence = x.PerformanceEvidence,
                    Techniques = x.Techniques,
                    Tools = x.Tools,
                    Weighing = x.Weighing,
                    WeekNumberStart = x.WeekNumberStart,
                    WeekNumberEnd = x.WeekNumberEnd
                }).OrderBy(x => x.Number).ToList(),
                MethodologicalStrategies = new MethodologicalStrategiesViewModel
                {
                    Learning = courseSyllabus.MethodologicalLearningStrategies,
                    Research = courseSyllabus.MethodologicalResearchStrategies,
                    SocialResponsability = courseSyllabus.MethodologicalStrategiesOfSocialResponsability,
                    Teaching = courseSyllabus.TeachingMethodologicalStrategies,
                    VirtualTeaching = courseSyllabus.VirtualTeachingMethodologicalStrategies
                },
                DidacticMaterials = courseSyllabus.DidacticMaterials,
                LearningProduct = new LearningProductViewModel
                {
                    PresentationDate = courseSyllabus.LearningProductPresentationDate,
                    Product = courseSyllabus.LearningProductEntity
                },
                BibliographicReferences = new BibliographicReferencesViewModel
                {
                    Basic = courseSyllabus.BasicBibliographicReferences,
                    Complementary = courseSyllabus.ComplementaryBibliographicReferences,
                    Electronic = courseSyllabus.ElectronicBibliographicReferences,
                    IntellectualProduction = courseSyllabus.IntellectualProductionBibliographicReferences
                }
            };

            var directedCourse = await _directedCourseService.GetByFilters(term.Id, null, course.Id);
            if (directedCourse != null)
            {
                var practicalHours = (decimal)course.PracticalHours / 2;
                var seminarHours = (decimal)course.SeminarHours / 2;
                var virtualHours = (decimal)course.VirtualHours / 2;
                var theoreticalHours = (decimal)course.TheoreticalHours / 2;

                model.GeneralInformation.PracticalHours = practicalHours % 1 != 0 ? $"{practicalHours:0.0}" : $"{practicalHours:00}";
                model.GeneralInformation.SeminarHours = virtualHours % 1 != 0 ? $"{virtualHours:0.0}" : $"{virtualHours:00}";
                model.GeneralInformation.VirtualHours = virtualHours % 1 != 0 ? $"{virtualHours:0.0}" : $"{virtualHours:00}";
                model.GeneralInformation.TheoreticalHours = theoreticalHours % 1 != 0 ? $"{theoreticalHours:0.0}" : $"{theoreticalHours:00}";
            }

            if (courseTerm != null)
            {

                var syllabusTeacherRequest = await _syllabusTeacherService.GetByCourseTermId(courseTerm.Id);

                if (syllabusTeacherRequest != null)
                {
                    if (
                        courseTerm?.CoordinatorId == currentUser.Id &&
                        (syllabusTeacherRequest.Status == ConstantHelpers.SYLLABUS_TEACHER.STATUS.IN_VALIDATION || syllabusTeacherRequest.Status == ConstantHelpers.SYLLABUS_TEACHER.STATUS.PRESENTED)
                        )
                    {
                        model.CanEdit = false;
                    }

                    model.SyllabusTeacherRequest = new SyllabusTeacherRequest
                    {
                        Id = syllabusTeacherRequest.Id,
                        Status = syllabusTeacherRequest.Status,
                        PresentationDate = syllabusTeacherRequest.PresentationDate.ToLocalDateTimeFormat()
                    };
                }
            }

            if (course.CareerId.HasValue)
            {
                var career = await _careerService.Get(course.CareerId.Value);
                var faculty = await _facultyService.Get(career.FacultyId);

                model.GeneralInformation.Career = career?.Name;
                model.GeneralInformation.Faculty = faculty?.Name;

                if (course.AcademicProgramId.HasValue)
                {
                    var academicProgram = await _academicProgramService.Get(course.AcademicProgramId.Value);

                    model.GeneralInformation.AcademicProgram = academicProgram.Name;
                }
            }

            #region -- Asignación Docente --

            var teacherTempDataBySyllabus = await _courseSyllabusTeacherService.GetByCourseSyllabusId(courseSyllabus.Id);

            if (teacherSections.Any())
            {
                //Otros docentes
                var teacherSectionFilter = teacherSections.GroupBy(x => x.TeacherId).Select(x => new
                {
                    teacherId = x.Key
                }).ToList();

                foreach (var teacherSection in teacherSectionFilter)
                {
                    var teacherDetail = await _teacherService.GetTeacherWithData(teacherSection.teacherId);
                    var teacherTempData = teacherTempDataBySyllabus.Where(x => x.TeacherId == teacherSection.teacherId).FirstOrDefault();

                    if (teacherDetail != null)
                    {
                        var teacherModel = new CourseTermSyllabusTeacherViewModel
                        {
                            TeacherId = teacherDetail.UserId,

                            Name = teacherDetail.User?.FullName,
                            IsTemporalName = false,

                            //TeacherCondition = teacherDetail.User?.WorkerLaborInformation?.WorkerLaborCondition?.Name,
                            TeacherCondition = string.Empty,
                            IsTemporalCondition = false,

                            //TeacherSpeciality = string.Join("-", teacherDetail.User?.WorkerProfessionalTitles.Select(y => y.Specialty).ToArray()),
                            TeacherSpeciality = string.Empty,
                            IsTemporalSpeciality = false,

                            IsCoordinator = teacherDetail.UserId == courseTerm?.CoordinatorId
                        };

                        if (string.IsNullOrEmpty(teacherModel.TeacherCondition))
                        {
                            teacherModel.TeacherCondition = teacherTempData?.Condition;

                            if (string.IsNullOrEmpty(teacherTempData?.Condition))
                                teacherModel.TeacherCondition = teacherDetail.User?.WorkerLaborInformation?.WorkerLaborCondition?.Name;

                            teacherModel.IsTemporalCondition = true;
                        }

                        if (string.IsNullOrEmpty(teacherModel.TeacherSpeciality))
                        {
                            teacherModel.TeacherSpeciality = teacherTempData?.Speciality;

                            if (string.IsNullOrEmpty(teacherTempData?.Speciality))
                                teacherModel.TeacherSpeciality = string.Join("-", teacherDetail.User?.WorkerProfessionalTitles.Select(y => y.Specialty).ToArray());

                            teacherModel.IsTemporalSpeciality = true;
                        }

                        model.GeneralInformation.Teachers.Add(teacherModel);
                    }
                }

                model.GeneralInformation.Teachers = model.GeneralInformation.Teachers.OrderByDescending(x => x.IsCoordinator).ToList();

            }
            else
            {
                var tempData = teacherTempDataBySyllabus.Where(x => string.IsNullOrEmpty(x.TeacherId)).FirstOrDefault();

                var teacherModel = new CourseTermSyllabusTeacherViewModel
                {
                    Name = tempData?.TempTeacherName,
                    IsTemporalName = true,

                    TeacherCondition = tempData?.Condition,
                    IsTemporalCondition = true,

                    TeacherSpeciality = tempData?.Speciality,
                    IsTemporalSpeciality = true,
                };

                model.GeneralInformation.Teachers.Add(teacherModel);
            }

            #endregion

            return model;
        }

        private string GetDevelopmentTimeString(List<DateTime[]> termClassesRange, int weekNumberStart, int weekNumberEnd, int totalHoursByWeek)
        {
            var weekNumberStartTpm = weekNumberStart == 0 ? 0 : weekNumberStart - 1;
            var weekNumberEndTpm = weekNumberEnd == 0 ? 0 : weekNumberEnd - 1;

            if (weekNumberStart == 0 && weekNumberEnd == 0)
                return $"Tiempo de desarrollo no especificado";


            var startDate = termClassesRange.ElementAtOrDefault(weekNumberStartTpm)?[0];
            var endDate = termClassesRange.ElementAtOrDefault(weekNumberEndTpm)?[1];
            var totalWeeks = weekNumberEnd - (weekNumberStart == 0 ? 0 : (weekNumberStart - 1));

            if (startDate == null || endDate == null)
                return $"De la semana {weekNumberStart} a la semana {weekNumberStart}";

            var result = $"Del {startDate.Value.Day} de {ConstantHelpers.MONTHS.VALUES[startDate.Value.Month]} al {endDate.Value.Day} de {ConstantHelpers.MONTHS.VALUES[endDate.Value.Month]} del {endDate.Value.Year} (Total {totalWeeks * totalHoursByWeek} horas)";
            return result;
        }

        [HttpGet("ficha-matricula")]
        [HttpGet("ficha-matricula/{termId}")]
        public async Task<IActionResult> EnrollmentReport(Guid termId)
        {
            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);

            var url = string.Empty;
            if (ConstantHelpers.Solution.Routes.Keys.Contains(ConstantHelpers.GENERAL.Institution.Value))
            {
                var baseUrl = ConstantHelpers.Solution.Routes[ConstantHelpers.GENERAL.Institution.Value][CORE.Helpers.ConstantHelpers.Solution.Enrollment];
                url = $"{baseUrl}admin/matricula/alumno/{student.Id}/detalle-cursos-matriculados";
            }

            var template = await _studentSectionService.GetEnrollmentReportTemplate(student.Id, termId, qrUrl: url);

            template.Image = Path.Combine(_hostingEnvironment.WebRootPath, @"images\themes\" + CORE.Helpers.GeneralHelpers.GetTheme() + "/logo-report.png");

            var storage = new CloudStorageService(_storageCredentials);

            var signatureImageUrl = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IMAGE_CERTIFICATE_SIGNATURE);
            var signatureBase64 = string.Empty;

            if (!string.IsNullOrEmpty(signatureImageUrl))
            {
                using (var mem = new MemoryStream())
                {
                    await storage.TryDownload(mem, ConstantHelpers.CONTAINER_NAMES.GENERAL_INFORMATION, signatureImageUrl);
                    signatureBase64 = $"data:image/png;base64, {Convert.ToBase64String(mem.ToArray())}";
                }
            }

            template.SignatuareImgBase64 = signatureBase64;

            var orientation = DinkToPdf.Orientation.Portrait;
            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG)
                orientation = DinkToPdf.Orientation.Portrait;

            var margin = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 15, Right = 15 };
            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG)
                margin = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 };
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = orientation,
                PaperSize = DinkToPdf.PaperKind.Letter,
                Margins = margin,
                DPI = 290
            };

            var objectSettings = new DinkToPdf.ObjectSettings();

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG)
            {
                template.PrintingDate = DateTime.UtcNow.ToDefaultTimeZone().ToString("dddd, dd MMMM yyyy", new CultureInfo("es-PE"));

                var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/StudentInformation/Pdf/EnrollmentReportUNJBG.cshtml", template);

                objectSettings = new DinkToPdf.ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = viewToString,
                    WebSettings = { DefaultEncoding = "utf-8" },
                    FooterSettings = {
                        FontName = "Arial",
                        FontSize = 9,
                        Line = false,
                        Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                        Center = "",
                        Right = "Pág: [page]/[toPage]"
                    }
                };
            }
            else
            {
                var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Academic/Views/StudentInformation/Pdf/EnrollmentReport.cshtml", template);
                var cssPtah = Path.Combine(_hostingEnvironment.WebRootPath, @"css/areas/academic/studentinformation/enrollmentreport.css");

                objectSettings = new DinkToPdf.ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = viewToString,
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = cssPtah },
                    FooterSettings = {
                        FontName = "Arial",
                        FontSize = 9,
                        Line = false,
                        Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                        Center = "",
                        Right = "Pág: [page]/[toPage]"
                    }
                };
            }

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var fileByte = _dinkConverter.Convert(pdf);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            //return File(fileByte, "application/pdf", "Reporte matricula.pdf");
            return File(fileByte, "application/pdf", $"{template.StudentCode}-Matricula {template.Semester}.pdf");
        }

        [HttpGet("constancia-fichasocioeconomica")]
        public async Task<IActionResult> PrintSocioEconomicRecord()
        {
            var userId = _userService.GetUserIdByClaim(User);
            var student = await _studentService.GetStudentByUser(userId);

            var data = await _studentService.GetStudentLastConstancy(student.Id);

            if (data == null)
                return BadRequest("No se ha encontrado una constancia");

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/InstitutionalWelfare/PDF/SocioEconomicConstancy.cshtml", data);

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 0, Left = 10, Right = 10 }
            };

            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8" },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileByte = _dinkConverter.Convert(pdf);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            HttpContext.Response.Headers["Content-Disposition"] = $"attachment; filename= {data.UserName}-constancia.pdf";
            return File(fileByte, "application/octet-stream");
        }
    }
}
