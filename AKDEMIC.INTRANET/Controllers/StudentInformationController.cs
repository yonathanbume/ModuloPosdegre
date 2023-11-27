using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.ViewModels.StudentInformationViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using DinkToPdf.Contracts;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Options;
using AKDEMIC.INTRANET.Filters.Permission;
using AKDEMIC.SERVICE.Services.Intranet.Implementations;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.STUDENTS)]
    [Route("ficha-socioeconomica")]
    public class StudentInformationController : BaseController
    {
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IStudentInformationService _studentInformationService;
        private readonly IConverter _dinkConverter;
        private readonly IInstitutionalWelfareRecordService _recordService;
        private readonly IInstitutionalWelfareSectionService _recordSectionService;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUserService _userService;
        private readonly IStudentService _studentService;
        private readonly IStudentFamilyService _studentFamilyService;
        private readonly IEnrollmentTurnService _enrollmentTurnService;
        private readonly IInstitutionalRecordCategorizationByStudentService _institutionalRecordCategorizationByStudentService;
        private readonly IInstitutionalWelfareAnswerByStudentService _recordAnswerByStudentService;
        private readonly IInstitutionalWelfareAnswerService _recordAnswerService;
        private readonly IConfigurationService _configurationService;
        private readonly ITermService _termService;
        private readonly IYearInformationService _yearInformationService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public StudentInformationController(UserManager<ApplicationUser> userManager, IConverter dinkConverter,
            IViewRenderService viewRenderService,
            IWebHostEnvironment environment,
            IStudentInformationService studentInformationService,
            IInstitutionalWelfareRecordService recordService,
            IInstitutionalWelfareSectionService recordSectionService,
            IUserService userService,
            IStudentService studentService,
            IStudentFamilyService studentFamilyService,
            IEnrollmentTurnService enrollmentTurnService,
            IInstitutionalWelfareAnswerService recordAnswerService,
            IConfigurationService configurationService,
            IInstitutionalWelfareAnswerByStudentService recordAnswerByStudentService,
            IOptions<CloudStorageCredentials> storageCredentials,
            IInstitutionalRecordCategorizationByStudentService institutionalRecordCategorizationByStudentService,
            IYearInformationService yearInformationService,
            IDataTablesService dataTablesService,
            ITermService termService) : base(userManager, dataTablesService)
        {
            _hostingEnvironment = environment;
            _dinkConverter = dinkConverter;
            _recordService = recordService;
            _viewRenderService = viewRenderService;
            _userService = userService;
            _studentService = studentService;
            _studentFamilyService = studentFamilyService;
            _enrollmentTurnService = enrollmentTurnService;
            _recordSectionService = recordSectionService;
            _termService = termService;
            _recordAnswerByStudentService = recordAnswerByStudentService;
            _institutionalRecordCategorizationByStudentService = institutionalRecordCategorizationByStudentService;
            _recordAnswerService = recordAnswerService;
            _configurationService = configurationService;
            _yearInformationService = yearInformationService;
            _storageCredentials = storageCredentials;
            _studentInformationService = studentInformationService;
        }

        /// <summary>
        /// Obtiene la vista de la ficha socioeconomica
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var term = await _termService.GetActiveTerm();
            var userId = _userManager.GetUserId(User);

            #region configuraciones
            //La visibilidad de la ficha socioeconomica, se vera afectada de los dos siguientes parametros, todos los estudiantes, solo ingresantes y reiterativa
            var boolConfigStudentInformationVisibility = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.InstitutionalWelfareManagement.STUDENT_INFORMATION_VISIBILITY));
            var boolConfigSurveyVisibility = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.InstitutionalWelfareManagement.INSTITUTIONAL_WELFARE_SURVEY_VISIBILITY));

            var boolConfigStudentInformationReiterative = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.InstitutionalWelfareManagement.STUDENT_INFORMATION_REITERATIVE));
            var boolconfigStudentInformationAllStudent = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.InstitutionalWelfareManagement.STUDENT_INFORMATION_ALLSTUDENT));
            #endregion

            var student = await _studentService.GetStudentDataByUserId(userId);

            var studentInformationTerm = await _studentInformationService.GetByStudentAndTerm(student.StudentId, term.Id);
            var hasStudentInformation = await _studentInformationService.HasStudentInformation(student.StudentId);//Si lleno la ficha en cualquier otro periodo

            var model = new IndexViewModel
            {
                //Datos Del Estudiante
                UserName = student.UserName,
                UserNameParameter = student.UserName,
                Name = student.Name,
                PaternalSurname = student.PaternalSurname,
                MaternalSurname = student.MaternalSurname,
                DNI = student.DNI,
                Sex = student.Sex,
                Email = student.Email,
                CivilStatus = student.CivilStatus,
                Career = student.Career,
                Faculty = student.Faculty,
                CurrentAcademicYear = student.CurrentAcademicYear,
                BirthDate = student.BirthDate.ToString("dd/MM/yyyy"),
                Age = (DateTime.UtcNow.Year - student.BirthDate.Year).ToString(),
                CurrentAddress = student.CurrentAddress,
                CurrentPhoneNumber = student.CurrentPhoneNumber,
                CurrentDepartmentId = student.CurrentDepartmentId,
                CurrentDepartmentDescription = student.CurrentDepartmentDescription,
                CurrentProvinceId = student.CurrentProvinceId,
                CurrentProvinceDescription = student.CurrentProvinceDescription,
                CurrentDistrictId = student.CurrentDistrictId,
                CurrentDistrictDescription = student.CurrentDistrictDescription,
                ConfigSurveyVisibility = boolConfigSurveyVisibility,
                ConfigStudentInformationVisibility = boolConfigStudentInformationVisibility
            };


            #region Validaciones para la fichasocioeconomica


            //Si esta activo la ficha socioeconomica
            if (model.ConfigStudentInformationVisibility)
            {
                //Dirigido a todos los estudiantes
                if (boolconfigStudentInformationAllStudent)
                {
                    //Si esta configurada como reiterativa
                    if (boolConfigStudentInformationReiterative)
                    {
                        //Si ya tiene ficha socioeconomica en este periodo
                        if (studentInformationTerm != null)
                        {
                            model.ConfigStudentInformationVisibility = false;
                        }
                    }
                    else
                    {
                        //Si ya tiene ficha socioeconomica cualquier periodo
                        if (hasStudentInformation)
                        {
                            model.ConfigStudentInformationVisibility = false;
                        }
                    }
                }
                else //Solo ingresantes
                {
                    if (student.Status != ConstantHelpers.Student.States.ENTRANT)
                    {
                        //Si el alumno es ingresante, entonces no puede ver la ficha socioeconomica a pesar que este activa
                        model.ConfigStudentInformationVisibility = false;
                    }
                    else
                    {
                        //Si esta configurada como reiterativa
                        if (boolConfigStudentInformationReiterative)
                        {
                            //Si ya tiene ficha socioeconomica en este periodo
                            if (studentInformationTerm != null)
                            {
                                model.ConfigStudentInformationVisibility = false;
                            }
                        }
                        else
                        {
                            //Si ya tiene ficha socioeconomica cualquier periodo
                            if (hasStudentInformation)
                            {
                                model.ConfigStudentInformationVisibility = false;
                            }
                        }
                    }
                }
            }
            #endregion

            if (studentInformationTerm != null)
            {
                model.IsStudentInformationResolved = true;
            }


            model.OriginAddress = studentInformationTerm != null ? studentInformationTerm.OriginAddress : "";
            model.OriginPhoneNumber = studentInformationTerm != null ? studentInformationTerm.OriginPhoneNumber : "";
            model.FullNameExternalPerson = studentInformationTerm != null ? studentInformationTerm.FullNameExternalPerson : "";
            model.AddressExternalPerson = studentInformationTerm != null ? studentInformationTerm.AddressExternalPerson : "";
            model.EmailExternalPerson = studentInformationTerm != null ? studentInformationTerm.EmailExternalPerson : "";
            model.PhoneExternalPerson =  studentInformationTerm != null ?  studentInformationTerm.PhoneExternalPerson : "";
            model.OriginSchool = studentInformationTerm != null ? studentInformationTerm.OriginSchool : "";
            model.OriginSchoolPlace = studentInformationTerm != null ? studentInformationTerm.OriginSchoolPlace : "";
            model.SchoolType = studentInformationTerm != null ? studentInformationTerm.SchoolType : byte.MinValue;
            model.UniversityPreparationId = studentInformationTerm != null ? studentInformationTerm.UniversityPreparationId : 0;


            model.PrincipalPerson = studentInformationTerm != null ? studentInformationTerm.PrincipalPerson : 0;
            model.EconomicMethodFatherTutor = studentInformationTerm != null ? studentInformationTerm.EconomicMethodFatherTutor : 0;
            model.DSectorFatherTutor = studentInformationTerm != null ? studentInformationTerm.DSectorFatherTutor : 0;
            model.DWorkConditionFatherTutor = studentInformationTerm != null ? studentInformationTerm.DWorkConditionFatherTutor : 0;
            model.DEspecificActivityFatherTutor = studentInformationTerm != null ? studentInformationTerm.DEspecificActivityFatherTutor : "";
            model.DBusyFatherTutor = studentInformationTerm != null ? studentInformationTerm.DBusyFatherTutor : 0;
            model.ISectorFatherTutor = studentInformationTerm != null ? studentInformationTerm.ISectorFatherTutor : 0;
            model.IWorkConditionFatherTutor = studentInformationTerm != null ? studentInformationTerm.IWorkConditionFatherTutor : 0;
            model.IEspecificActivityFatherTutor = studentInformationTerm != null ? studentInformationTerm.IEspecificActivityFatherTutor : "";
            model.EconomicMethodMother = studentInformationTerm != null ? studentInformationTerm.EconomicMethodMother : 0;
            model.DSectorMother = studentInformationTerm != null ? studentInformationTerm.DSectorMother : 0;
            model.DWorkConditionMother = studentInformationTerm != null ? studentInformationTerm.DWorkConditionMother : 0;
            model.DEspecificActivityMother = studentInformationTerm != null ? studentInformationTerm.DEspecificActivityMother : "";
            model.DBusyMother = studentInformationTerm != null ? studentInformationTerm.DBusyMother : 0;
            model.ISectorMother = studentInformationTerm != null ? studentInformationTerm.ISectorMother : 0;
            model.IWorkConditionMother = studentInformationTerm != null ? studentInformationTerm.IWorkConditionMother : 0;
            model.IEspecificActivityMother = studentInformationTerm != null ? studentInformationTerm.IEspecificActivityMother : "";
            model.EconomicExpensesFeeding = studentInformationTerm != null ? studentInformationTerm.EconomicExpensesFeeding : 0;
            model.EconomicExpensesBasicServices = studentInformationTerm != null ? studentInformationTerm.EconomicExpensesBasicServices : 0;
            model.EconomicExpensesEducation = studentInformationTerm != null ? studentInformationTerm.EconomicExpensesEducation : 0;
            model.EconomicExpensesOthers = studentInformationTerm != null ? studentInformationTerm.EconomicExpensesOthers : 0;
            model.FatherRemuneration = studentInformationTerm != null ? studentInformationTerm.FatherRemuneration : 0;
            model.MotherRemuneration = studentInformationTerm != null ? studentInformationTerm.MotherRemuneration : 0;
            model.StudentRemuneration = studentInformationTerm != null ? studentInformationTerm.StudentRemuneration : 0;
            model.OtherRemuneration = studentInformationTerm != null ? studentInformationTerm.OtherRemuneration : 0;
            model.TotalRemuneration = studentInformationTerm != null ? studentInformationTerm.TotalRemuneration : 0;
            model.StudentDependency = studentInformationTerm != null ? studentInformationTerm.StudentDependency : 0;
            model.StudentCoexistence = studentInformationTerm != null ? studentInformationTerm.StudentCoexistence : 0;
            model.FamilyRisk = studentInformationTerm != null ? studentInformationTerm.FamilyRisk : 0;
            model.StudentWorkDedication = studentInformationTerm != null ? studentInformationTerm.StudentWorkDedication : 0;
            model.StudentWorkDescription = studentInformationTerm != null ? studentInformationTerm.StudentWorkDescription : "";
            model.StudentWorkCondition = studentInformationTerm != null ? studentInformationTerm.StudentWorkCondition : 1;
            model.AuthorizeCheck = studentInformationTerm != null ? studentInformationTerm.AuthorizeCheck : false;
            model.AuthorizedPersonFullName = studentInformationTerm != null ? studentInformationTerm.AuthorizedPersonFullName : "";
            model.AuthorizedPersonAddress = studentInformationTerm != null ? studentInformationTerm.AuthorizedPersonAddress : "";
            model.AuthorizedPersonPhone = studentInformationTerm != null ? studentInformationTerm.AuthorizedPersonPhone : "";




            model.IsSick = studentInformationTerm != null ? studentInformationTerm.IsSick : byte.MinValue;
            model.TypeParentIllness = studentInformationTerm != null ? studentInformationTerm.TypeParentIllness : "";
            model.HaveInsurance = studentInformationTerm != null ? studentInformationTerm.HaveInsurance : byte.MinValue;
            model.InsuranceDescription = studentInformationTerm != null ? studentInformationTerm.InsuranceDescription : byte.MinValue;

            model.BreakfastHome = studentInformationTerm != null ? studentInformationTerm.BreakfastHome : false;
            model.BreakfastPension = studentInformationTerm != null ? studentInformationTerm.BreakfastPension : false;
            model.BreakfastRelativeHome = studentInformationTerm != null ? studentInformationTerm.BreakfastRelativeHome : false;
            model.BreakfastOther = studentInformationTerm != null ? studentInformationTerm.BreakfastOther : false;
            model.LunchHome = studentInformationTerm != null ? studentInformationTerm.LunchHome : false;
            model.LunchPension = studentInformationTerm != null ? studentInformationTerm.LunchPension : false;
            model.LunchRelativeHome = studentInformationTerm != null ? studentInformationTerm.LunchRelativeHome : false;
            model.LunchOther = studentInformationTerm != null ? studentInformationTerm.LunchOther : false;
            model.DinnerHome = studentInformationTerm != null ? studentInformationTerm.DinnerHome : false;
            model.DinnerPension = studentInformationTerm != null ? studentInformationTerm.DinnerPension : false;
            model.DinnerRelativeHome = studentInformationTerm != null ? studentInformationTerm.DinnerRelativeHome : false;
            model.DinnerOther = studentInformationTerm != null ? studentInformationTerm.DinnerOther : false;

            model.Tenure = studentInformationTerm != null ? studentInformationTerm.Tenure : byte.MinValue;
            model.ContructionType = studentInformationTerm != null ? studentInformationTerm.ContructionType : byte.MinValue;
            model.ZoneType = studentInformationTerm != null ? studentInformationTerm.ZoneType : byte.MinValue;
            model.BuildType = studentInformationTerm != null ? studentInformationTerm.BuildType : byte.MinValue;
            model.OtherTypeLivingPlace = studentInformationTerm != null ? studentInformationTerm.OtherTypeLivingPlace : "";
            model.NumberFloors = studentInformationTerm != null ? studentInformationTerm.NumberFloors : byte.MinValue;
            model.NumberRooms = studentInformationTerm != null ? studentInformationTerm.NumberRooms : byte.MinValue;
            model.NumberKitchen = studentInformationTerm != null ? studentInformationTerm.NumberKitchen : byte.MinValue;
            model.NumberBathroom = studentInformationTerm != null ? studentInformationTerm.NumberBathroom : byte.MinValue;
            model.NumberLivingRoom = studentInformationTerm != null ? studentInformationTerm.NumberLivingRoom : byte.MinValue;
            model.NumberDinningRoom = studentInformationTerm != null ? studentInformationTerm.NumberDinningRoom : byte.MinValue;
            model.Water = studentInformationTerm != null ? studentInformationTerm.Water : false;
            model.Drain = studentInformationTerm != null ? studentInformationTerm.Drain : false;
            model.LivingPlacePhone = studentInformationTerm != null ? studentInformationTerm.LivingPlacePhone : false;
            model.Light = studentInformationTerm != null ? studentInformationTerm.Light : false;
            model.Internet = studentInformationTerm != null ? studentInformationTerm.Internet : false;
            model.TV = studentInformationTerm != null ? studentInformationTerm.TV : false;
            model.HasPhone = studentInformationTerm != null ? studentInformationTerm.HasPhone : false;
            model.Radio = studentInformationTerm != null ? studentInformationTerm.Radio : false;
            model.Stereo = studentInformationTerm != null ? studentInformationTerm.Stereo : false;
            model.Iron = studentInformationTerm != null ? studentInformationTerm.Iron : false;
            model.EquipPhone = studentInformationTerm != null ? studentInformationTerm.EquipPhone : false;
            model.Laptop = studentInformationTerm != null ? studentInformationTerm.Laptop : false;
            model.Closet = studentInformationTerm != null ? studentInformationTerm.Closet : false;
            model.Fridge = studentInformationTerm != null ? studentInformationTerm.Fridge : false;
            model.PersonalLibrary = studentInformationTerm != null ? studentInformationTerm.PersonalLibrary : false;
            model.EquipComputer = studentInformationTerm != null ? studentInformationTerm.EquipComputer : false;

            if (student.StudentInformationData != null)
            {
                model.PlaceOriginDepartmentId = student.StudentInformationData.PlaceOriginDepartmentId;
                model.PlaceOriginProvinceId = student.StudentInformationData.PlaceOriginProvinceId;
                model.PlaceOriginDistrictId = student.StudentInformationData.PlaceOriginDistrictId;

                model.OriginDepartmentId = student.StudentInformationData.OriginDepartmentId;
                model.OriginProvinceId = student.StudentInformationData.OriginProvinceId;
                model.OriginDistrictId = student.StudentInformationData.OriginDistrictId;
            }

            return View(model);
        }

        /// <summary>
        /// Obtiene la ficha de evaluacion socioeconomica
        /// </summary>
        /// <param name="username">Usuario</param>
        /// <returns>Retorna una vista parcial</returns>
        [HttpGet("obtener-ficha-evaluacion")]
        public async Task<IActionResult> GetRecordForEvaluation(string username)
        {
            var term = await _termService.GetActiveTerm();
            var record = await _recordService.GetActive();

            if (record != null)
            {
                bool wasAnsweredByuser = await _recordAnswerByStudentService.ExistAnswerByUserName(record.Id, username, term.Id);
                var sections = await _recordSectionService.GetDetailsByRecordId(record.Id);
                var model = sections
                    .Select(x => new SectionViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        RecordId = x.InstitutionalWelfareRecordId,
                        CanActive = wasAnsweredByuser,
                        Questions = x.InstitutionalWelfareQuestions.Select(y => new QuestionViewModel
                        {
                            Id = y.Id,
                            Description = y.Description,
                            DescriptionType = y.DescriptionType,
                            Type = y.Type,
                            InstitutionalWelfareSectionId = y.InstitutionalWelfareSectionId,
                            Answers = y.InstitutionalWelfareAnswers.Select(z => new AnswerViewModel
                            {
                                Id = z.Id,
                                Description = z.Description
                            })
                            .ToList()
                        })
                       .ToList()
                    })
                   .ToList();
                return PartialView("Partials/_Record", model);
            }
            else
            {
                return PartialView("Partials/_Record", new List<SectionViewModel>());
            }
        }

        /// <summary>
        /// Guarda toda la información de la ficha socio economica
        /// </summary>
        /// <param name="model">Modelo que contiene los parametros de la ficha socioeconomica</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("guardar/post")]
        public async Task<IActionResult> SaveAll(IndexViewModel model)
        {
            var hasAnyTextQuestion = false;
            string userId = _userManager.GetUserId(User);
            var currentTerm = await _termService.GetActiveTerm();


            var student = await _studentService.GetStudentIncludeInformationUser(userId);
            var hasStudentInformation = await _studentInformationService.HasStudentInformation(student.Id);//Si lleno la ficha en cualquier otro periodo

            var configStudentInformationVisibility = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.InstitutionalWelfareManagement.STUDENT_INFORMATION_VISIBILITY));
            var configSurveyVisibility = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.InstitutionalWelfareManagement.INSTITUTIONAL_WELFARE_SURVEY_VISIBILITY));

            #region Validaciones para la fichasocioeconomica
            var boolConfigStudentInformationReiterative = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.InstitutionalWelfareManagement.STUDENT_INFORMATION_REITERATIVE));
            var boolconfigStudentInformationAllStudent = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.InstitutionalWelfareManagement.STUDENT_INFORMATION_ALLSTUDENT));

            //Si esta activo la ficha socioeconomica
            if (configStudentInformationVisibility)
            {
                //Dirigido a todos los estudiantes
                if (boolconfigStudentInformationAllStudent)
                {
                    //Si esta configurada como reiterativa
                    if (boolConfigStudentInformationReiterative)
                    {
                        //Si ya tiene ficha socioeconomica en este periodo
                        if (model.IsStudentInformationResolved)
                        {
                            model.ConfigStudentInformationVisibility = false;
                        }

                    }
                    else
                    {
                        //Si ya tiene ficha socioeconomica cualquier periodo
                        if (hasStudentInformation)
                        {
                            model.ConfigStudentInformationVisibility = false;
                        }
                    }
                }
                else //Solo ingresantes
                {
                    if (student.Status != ConstantHelpers.Student.States.ENTRANT)
                    {
                        //Si el alumno es ingresante, entonces no puede ver la ficha socioeconomica a pesar que este activa
                        configStudentInformationVisibility = false;
                    }
                    else
                    {
                        //Si esta configurada como reiterativa
                        if (boolConfigStudentInformationReiterative)
                        {
                            //Si ya tiene ficha socioeconomica en este periodo
                            if (model.IsStudentInformationResolved)
                            {
                                model.ConfigStudentInformationVisibility = false;
                            }
                        }
                        else
                        {
                            //Si ya tiene ficha socioeconomica cualquier periodo
                            if (hasStudentInformation)
                            {
                                model.ConfigStudentInformationVisibility = false;
                            }
                        }
                    }
                }
            }
            #endregion      

            if (configSurveyVisibility)
            {
                #region InstituionalWelfareSurvey
                var record = await _recordService.GetActive();
                var answers = new List<InstitutionalWelfareAnswerByStudent>();
                var scores = new List<int>();
                bool wasAnsweredByuser = await _recordAnswerByStudentService.ExistAnswerByStudent(record.Id, student.Id, currentTerm.Id);
                if (!wasAnsweredByuser)
                {
                    foreach (var item in model.Sections)
                    {
                        var section = await _recordSectionService.Get(item.Id);
                        hasAnyTextQuestion = !item.Questions.Any(x => x.Type == ConstantHelpers.QUESTIONNAIRE.TEXT_QUESTION);
                        foreach (var question in item.Questions)
                        {


                            if (question.Type == ConstantHelpers.QUESTIONNAIRE.TEXT_QUESTION)
                            {
                                var answerUser = new InstitutionalWelfareAnswerByStudent
                                {
                                    AnswerDescription = question.Description,
                                    InstitutionalWelfareQuestionId = question.Id,
                                    StudentId = student.Id,
                                    TermId = currentTerm.Id
                                };
                                //scores.Add(getqueston.Score);
                                answers.Add(answerUser);
                            }
                            else
                            {
                                if (question.Selection != null)
                                {
                                    foreach (var selection in question.Selection)
                                    {
                                        var getanswer = await _recordAnswerService.Get(selection);
                                        var answerUser = new InstitutionalWelfareAnswerByStudent
                                        {
                                            InstitutionalWelfareQuestionId = question.Id,
                                            InstitutionalWelfareAnswerId = selection,
                                            StudentId = student.Id,
                                            TermId = currentTerm.Id
                                        };
                                        scores.Add(getanswer.Score);
                                        answers.Add(answerUser);
                                    }
                                }
                            }
                        }
                    }

                    var sum = scores.Sum(x => x);

                    CategorizationLevel currentConfigurationLevel = null;
                    foreach (var item in record.CategorizationLevelHeader.CategorizationLevels)
                    {
                        if (item.Min <= sum && sum <= item.Max)
                        {
                            currentConfigurationLevel = item;
                            break;
                        }
                    }
                    if (currentConfigurationLevel == null)
                    {
                        return BadRequest("No se encontró una categoría para el puntaje total obtenido");
                    }
                    var institutionalRecordCategorizationByStudent = new InstitutionalRecordCategorizationByStudent
                    {
                        CategorizationLevelId = currentConfigurationLevel.Id,
                        InstitutionalWelfareRecordId = record.Id,
                        StudentId = student.Id,
                        StudentScore = sum,
                        TermId = currentTerm.Id,
                        IsEvaluated = hasAnyTextQuestion
                    };
                    if (model.SisfohConstancy != null)
                    {
                        var documentFile = model.SisfohConstancy;

                        if (documentFile.Length > ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.APPLICATION.GENERIC)
                        {
                            return BadRequest($"El tamaño del archivo '{documentFile.FileName}' excede el límite de {ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.APPLICATION.GENERIC / 1024 / 1024}MB");
                        }

                        if (!documentFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.GENERIC))
                        {
                            return BadRequest($"El contenido del archivo '{documentFile.FileName}' es inválido");
                        }

                        var cloudStorageService = new CloudStorageService(_storageCredentials);
                        try
                        {
                            var uploadFilePath = await cloudStorageService.UploadFile(documentFile.OpenReadStream(),
                                ConstantHelpers.CONTAINER_NAMES.SISFOH_DOCUMENTS, Path.GetExtension(documentFile.FileName),
                                ConstantHelpers.FileStorage.SystemFolder.INSTITUTIONALWELFARE);
                            institutionalRecordCategorizationByStudent.SisfohConstancy = uploadFilePath;
                        }
                        catch (Exception)
                        {
                            return BadRequest($"Hubo un problema al subir el archivo '{documentFile.FileName}'");
                        }
                    }
                    institutionalRecordCategorizationByStudent.SisfohClasification = model.SisfohClasification;
                    await _institutionalRecordCategorizationByStudentService.Insert(institutionalRecordCategorizationByStudent);
                    await _recordAnswerByStudentService.InsertRange(answers);
                }

                #endregion

                if (configStudentInformationVisibility)
                {
                    #region StudentInformation

                    var studentInformation = new StudentInformation
                    {
                        StudentId = student.Id,
                        TermId = currentTerm.Id,
                        Age = Convert.ToInt32(model.Age),
                        OriginAddress = model.OriginAddress,
                        OriginPhoneNumber = model.OriginPhoneNumber,
                        FullNameExternalPerson = model.FullNameExternalPerson,
                        AddressExternalPerson = model.AddressExternalPerson,
                        EmailExternalPerson = model.EmailExternalPerson,
                        PhoneExternalPerson = model.PhoneExternalPerson,
                        OriginSchool = model.OriginSchool,
                        OriginSchoolPlace = model.OriginSchoolPlace,
                        SchoolType = model.SchoolType,
                        UniversityPreparationId = model.UniversityPreparationId,
                        PrincipalPerson = model.PrincipalPerson,
                        EconomicMethodFatherTutor = model.EconomicMethodFatherTutor,
                        DSectorFatherTutor = model.DSectorFatherTutor,
                        DWorkConditionFatherTutor = model.DWorkConditionFatherTutor,
                        DEspecificActivityFatherTutor = model.DEspecificActivityFatherTutor,
                        DBusyFatherTutor = model.DBusyFatherTutor,
                        ISectorFatherTutor = model.ISectorFatherTutor,
                        IWorkConditionFatherTutor = model.IWorkConditionFatherTutor,
                        IEspecificActivityFatherTutor = model.IEspecificActivityFatherTutor,
                        EconomicMethodMother = model.EconomicMethodMother,
                        DSectorMother = model.DSectorMother,
                        DWorkConditionMother = model.DWorkConditionMother,
                        DEspecificActivityMother = model.DEspecificActivityMother,
                        DBusyMother = model.DBusyMother,
                        ISectorMother = model.ISectorMother,
                        IWorkConditionMother = model.IWorkConditionMother,
                        IEspecificActivityMother = model.IEspecificActivityMother,
                        EconomicExpensesFeeding = model.EconomicExpensesFeeding,
                        EconomicExpensesBasicServices = model.EconomicExpensesBasicServices,
                        EconomicExpensesEducation = model.EconomicExpensesEducation,
                        EconomicExpensesOthers = model.EconomicExpensesOthers,
                        FatherRemuneration = model.FatherRemuneration,
                        MotherRemuneration = model.MotherRemuneration,
                        StudentRemuneration = model.StudentRemuneration,
                        OtherRemuneration = model.OtherRemuneration,
                        TotalRemuneration = model.TotalRemuneration,
                        StudentDependency = model.StudentDependency,
                        StudentCoexistence = model.StudentCoexistence,
                        FamilyRisk = model.FamilyRisk,
                        StudentWorkDedication = model.StudentWorkDedication,
                        StudentWorkDescription = model.StudentWorkDescription,
                        StudentWorkCondition = model.StudentWorkCondition,
                        AuthorizeCheck = model.AuthorizeCheck,
                        AuthorizedPersonFullName = model.AuthorizedPersonFullName,
                        AuthorizedPersonAddress = model.AuthorizedPersonAddress,
                        AuthorizedPersonPhone = model.AuthorizedPersonPhone,
                        IsSick = model.IsSick,
                        TypeParentIllness = model.TypeParentIllness,
                        HaveInsurance = model.HaveInsurance,
                        InsuranceDescription = model.InsuranceDescription,
                        BreakfastHome = model.BreakfastHome,
                        BreakfastPension = model.BreakfastPension,
                        BreakfastRelativeHome = model.BreakfastRelativeHome,
                        BreakfastOther = model.BreakfastOther,
                        LunchHome = model.LunchHome,
                        LunchPension = model.LunchPension,
                        LunchRelativeHome = model.LunchRelativeHome,
                        LunchOther = model.LunchOther,
                        DinnerHome = model.DinnerHome,
                        DinnerPension = model.DinnerPension,
                        DinnerRelativeHome = model.DinnerRelativeHome,
                        DinnerOther = model.DinnerOther,
                        Tenure = model.Tenure,
                        ContructionType = model.ContructionType,
                        ZoneType = model.ZoneType,
                        BuildType = model.BuildType,
                        OtherTypeLivingPlace = model.OtherTypeLivingPlace,
                        NumberFloors = model.NumberFloors,
                        NumberRooms = model.NumberRooms,
                        NumberKitchen = model.NumberKitchen,
                        NumberBathroom = model.NumberBathroom,
                        NumberLivingRoom = model.NumberLivingRoom,
                        NumberDinningRoom = model.NumberDinningRoom,
                        Water = model.Water,
                        Drain = model.Drain,
                        LivingPlacePhone = model.LivingPlacePhone,
                        Light = model.Light,
                        Internet = model.Internet,
                        TV = model.TV,
                        HasPhone = model.HasPhone,
                        Radio = model.Radio,
                        Stereo = model.Stereo,
                        Iron = model.Iron,
                        EquipPhone = model.EquipPhone,
                        Laptop = model.Laptop,
                        Closet = model.Closet,
                        Fridge = model.Fridge,
                        PersonalLibrary = model.PersonalLibrary,
                        EquipComputer = model.EquipComputer,
                    };

                    if (model.OriginDistrictId.HasValue)
                    {
                        studentInformation.OriginDistrictId = model.OriginDistrictId.Value;
                    }
                    if (model.PlaceOriginDistrictId.HasValue)
                    {
                        studentInformation.PlaceOriginDistrictId = model.PlaceOriginDistrictId.Value;

                    }

                    student.User.Address = model.CurrentAddress;
                    student.User.PhoneNumber = model.CurrentPhoneNumber;
                    if (model.CurrentDistrictId.HasValue)
                    {

                        student.User.DistrictId = model.CurrentDistrictId.Value;
                    }
                    if (model.CurrentProvinceId.HasValue)
                    {
                        student.User.ProvinceId = model.CurrentProvinceId.Value;
                    }
                    if (model.CurrentDepartmentId.HasValue)
                    {
                        student.User.DepartmentId = model.CurrentDepartmentId.Value;

                    }

                    student.StudentInformation = studentInformation;

                    await _studentService.Update(student);
                    #endregion
                }

            }
            else
            {
                if (configStudentInformationVisibility)
                {
                    #region StudentInformation

                    var studentInformation = new StudentInformation
                    {
                        TermId = currentTerm.Id,
                        StudentId = student.Id,
                        Age = Convert.ToInt32(model.Age),
                        OriginAddress = model.OriginAddress,
                        OriginPhoneNumber = model.OriginPhoneNumber,
                        FullNameExternalPerson = model.FullNameExternalPerson,
                        AddressExternalPerson = model.AddressExternalPerson,
                        EmailExternalPerson = model.EmailExternalPerson,
                        PhoneExternalPerson = model.PhoneExternalPerson,
                        OriginSchool = model.OriginSchool,
                        OriginSchoolPlace = model.OriginSchoolPlace,
                        SchoolType = model.SchoolType,
                        UniversityPreparationId = model.UniversityPreparationId,
                        PrincipalPerson = model.PrincipalPerson,
                        EconomicMethodFatherTutor = model.EconomicMethodFatherTutor,
                        DSectorFatherTutor = model.DSectorFatherTutor,
                        DWorkConditionFatherTutor = model.DWorkConditionFatherTutor,
                        DEspecificActivityFatherTutor = model.DEspecificActivityFatherTutor,
                        DBusyFatherTutor = model.DBusyFatherTutor,
                        ISectorFatherTutor = model.ISectorFatherTutor,
                        IWorkConditionFatherTutor = model.IWorkConditionFatherTutor,
                        IEspecificActivityFatherTutor = model.IEspecificActivityFatherTutor,
                        EconomicMethodMother = model.EconomicMethodMother,
                        DSectorMother = model.DSectorMother,
                        DWorkConditionMother = model.DWorkConditionMother,
                        DEspecificActivityMother = model.DEspecificActivityMother,
                        DBusyMother = model.DBusyMother,
                        ISectorMother = model.ISectorMother,
                        IWorkConditionMother = model.IWorkConditionMother,
                        IEspecificActivityMother = model.IEspecificActivityMother,
                        EconomicExpensesFeeding = model.EconomicExpensesFeeding,
                        EconomicExpensesBasicServices = model.EconomicExpensesBasicServices,
                        EconomicExpensesEducation = model.EconomicExpensesEducation,
                        EconomicExpensesOthers = model.EconomicExpensesOthers,
                        FatherRemuneration = model.FatherRemuneration,
                        MotherRemuneration = model.MotherRemuneration,
                        StudentRemuneration = model.StudentRemuneration,
                        OtherRemuneration = model.OtherRemuneration,
                        TotalRemuneration = model.TotalRemuneration,
                        StudentDependency = model.StudentDependency,
                        StudentCoexistence = model.StudentCoexistence,
                        FamilyRisk = model.FamilyRisk,
                        StudentWorkDedication = model.StudentWorkDedication,
                        StudentWorkDescription = model.StudentWorkDescription,
                        StudentWorkCondition = model.StudentWorkCondition,
                        AuthorizeCheck = model.AuthorizeCheck,
                        AuthorizedPersonFullName = model.AuthorizedPersonFullName,
                        AuthorizedPersonAddress = model.AuthorizedPersonAddress,
                        AuthorizedPersonPhone = model.AuthorizedPersonPhone,
                        IsSick = model.IsSick,
                        TypeParentIllness = model.TypeParentIllness,
                        HaveInsurance = model.HaveInsurance,
                        InsuranceDescription = model.InsuranceDescription,
                        BreakfastHome = model.BreakfastHome,
                        BreakfastPension = model.BreakfastPension,
                        BreakfastRelativeHome = model.BreakfastRelativeHome,
                        BreakfastOther = model.BreakfastOther,
                        LunchHome = model.LunchHome,
                        LunchPension = model.LunchPension,
                        LunchRelativeHome = model.LunchRelativeHome,
                        LunchOther = model.LunchOther,
                        DinnerHome = model.DinnerHome,
                        DinnerPension = model.DinnerPension,
                        DinnerRelativeHome = model.DinnerRelativeHome,
                        DinnerOther = model.DinnerOther,
                        Tenure = model.Tenure,
                        ContructionType = model.ContructionType,
                        ZoneType = model.ZoneType,
                        BuildType = model.BuildType,
                        OtherTypeLivingPlace = model.OtherTypeLivingPlace,
                        NumberFloors = model.NumberFloors,
                        NumberRooms = model.NumberRooms,
                        NumberKitchen = model.NumberKitchen,
                        NumberBathroom = model.NumberBathroom,
                        NumberLivingRoom = model.NumberLivingRoom,
                        NumberDinningRoom = model.NumberDinningRoom,
                        Water = model.Water,
                        Drain = model.Drain,
                        LivingPlacePhone = model.LivingPlacePhone,
                        Light = model.Light,
                        Internet = model.Internet,
                        TV = model.TV,
                        HasPhone = model.HasPhone,
                        Radio = model.Radio,
                        Stereo = model.Stereo,
                        Iron = model.Iron,
                        EquipPhone = model.EquipPhone,
                        Laptop = model.Laptop,
                        Closet = model.Closet,
                        Fridge = model.Fridge,
                        PersonalLibrary = model.PersonalLibrary,
                        EquipComputer = model.EquipComputer,
                    };

                    if (model.OriginDistrictId.HasValue)
                    {
                        studentInformation.OriginDistrictId = model.OriginDistrictId.Value;
                    }
                    if (model.PlaceOriginDistrictId.HasValue)
                    {
                        studentInformation.PlaceOriginDistrictId = model.PlaceOriginDistrictId.Value;

                    }

                    student.User.Address = model.CurrentAddress;
                    student.User.PhoneNumber = model.CurrentPhoneNumber;
                    if (model.CurrentDistrictId.HasValue)
                    {

                        student.User.DistrictId = model.CurrentDistrictId.Value;
                    }
                    if (model.CurrentProvinceId.HasValue)
                    {
                        student.User.ProvinceId = model.CurrentProvinceId.Value;
                    }
                    if (model.CurrentDepartmentId.HasValue)
                    {
                        student.User.DepartmentId = model.CurrentDepartmentId.Value;

                    }

                    student.StudentInformation = studentInformation;

                    await _studentService.Update(student);
                    #endregion
                }
            }

            var enrollmentTurn = await _enrollmentTurnService.GetByStudentIdAndTerm(student.Id, currentTerm.Id);

            if (enrollmentTurn != null)
            {
                if (enrollmentTurn.EnrollmentTurnHistories == null)
                    enrollmentTurn.EnrollmentTurnHistories = new List<ENTITIES.Models.Enrollment.EnrollmentTurnHistory>();

                enrollmentTurn.EnrollmentTurnHistories.Add(new ENTITIES.Models.Enrollment.EnrollmentTurnHistory
                {
                    CreatedAt = DateTime.UtcNow,
                    ConfirmationDate = enrollmentTurn.ConfirmationDate,
                    CreditsLimit = enrollmentTurn.CreditsLimit,
                    FileUrl = enrollmentTurn.FileUrl,
                    IsConfirmed = enrollmentTurn.IsConfirmed,
                    IsReceived = enrollmentTurn.IsReceived,
                    IsRectificationActive = enrollmentTurn.IsRectificationActive,
                    Observations = enrollmentTurn.Observations,
                    SpecialEnrollment = enrollmentTurn.SpecialEnrollment,
                });

                enrollmentTurn.WasStudentInformationUpdated = true;

                await _enrollmentTurnService.Update(enrollmentTurn);
            }

            var url = Url.Action("PrintSocioEconomicRecord", "StudentInformation", new { studentId = student.Id });
            return Ok(url);
        }

        /// <summary>
        /// Obtiene la constancia de la ficha socioeconomica del estudiante
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <returns>Retorna un archivo PDF</returns>
        [HttpGet("imprimir/{studentId}")]
        public async Task<IActionResult> PrintSocioEconomicRecord(Guid studentId)
        {
            var user = await _userManager.GetUserAsync(User);

            var student = await _studentService.GetStudentIncludeInformationGeoCareer2(studentId);

            if (user.Id != student.UserId)
                return BadRequest("Sucedio un error");

            var LstStudentFamily = await _studentFamilyService.GetStudentFamilyByStudentId(student.Id);
            var term = await _termService.GetActiveTerm();
            var year = await _yearInformationService.GetNameByYear(DateTime.Now.Year);
            var model = GetModel(student, LstStudentFamily, term, year);
            var viewToString = "";
            var cssPath = "";
            viewToString = await _viewRenderService.RenderToStringAsync("/Views/StudentInformation/PrintSocioEconomicRecord2.cshtml", model);
            cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/studentinformation/printsocioeconomicrecord.css");


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
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = cssPath },
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
            HttpContext.Response.Headers["Content-Disposition"] = $"attachment; filename= {model.UserName}.pdf";
            return File(fileByte, "application/octet-stream");
        }
        /// <summary>
        /// Obtiene la vista de la ficha socioeconomica del estudiante
        /// </summary>
        /// <param name="studentid">Identificador del estudiante</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("ver/{studentId}")]
        public async Task<IActionResult> GetSocioEconomicRecordView(Guid studentId)
        {
            var student = await _studentService.GetStudentIncludeInformationGeoCareer2(studentId);

            var LstStudentFamily = await _studentFamilyService.GetStudentFamilyByStudentId(student.Id);
            var term = await _termService.GetActiveTerm();
            var year = await _yearInformationService.GetNameByYear(DateTime.Now.Year);

            var model = GetModel(student, LstStudentFamily, term, year);
            return View("/Views/StudentInformation/SocioEconomicRecord.cshtml", model);
        }

        private IndexViewModel GetModel(ENTITIES.Models.Generals.Student student, List<StudentFamily> LstStudentFamily, ENTITIES.Models.Enrollment.Term term, string year)
        {
            return new IndexViewModel()
            {
                YearInformation = year,
                QrImage = GetImageQR(student.Id),
                UserName = student.User.UserName,
                Career = student.Career.Name,
                Faculty = student.Career.Faculty.Name,
                Term = term.Name,
                CurrentAcademicYear = student.CurrentAcademicYear,
                DNI = student.User.Dni,
                Email = student.User.Email,
                Name = student.User.FullName,
                Sex = student.User.Sex,
                BirthDate = student.User.BirthDate.ToString("dd/MM/yyyy"),
                Age = student.StudentInformation.Age.ToString(),
                CivilStatus = student.User.CivilStatus,
                PlaceOriginDepartmentDescription = student.StudentInformation?.PlaceOriginDistrict?.Province?.Department?.Name,
                PlaceOriginProvinceDescription = student.StudentInformation?.PlaceOriginDistrict?.Province?.Name,
                PlaceOriginDistrictDescription = student.StudentInformation?.PlaceOriginDistrict?.Name,
                CurrentAddress = student.User.Address,
                CurrentPhoneNumber = student.User.PhoneNumber,
                CurrentDistrictDescription = student.User.District.Name,
                CurrentProvinceDescription = student.User.District.Province.Name,
                CurrentDepartmentDescription = student.User.District.Province.Department.Name,
                FullNameExternalPerson = student.StudentInformation.FullNameExternalPerson,
                AddressExternalPerson = student.StudentInformation.AddressExternalPerson,
                PhoneExternalPerson = student.StudentInformation.PhoneExternalPerson,
                EmailExternalPerson = student.StudentInformation.EmailExternalPerson,
                OriginSchool = student.StudentInformation.OriginSchool,
                OriginSchoolPlace = student.StudentInformation.OriginSchoolPlace,
                SchoolType = student.StudentInformation.SchoolType,
                UniversityPreparationId = student.StudentInformation.UniversityPreparationId,
                ListStudentFamilyOnlyRead = LstStudentFamily,

                PrincipalPerson = student.StudentInformation.PrincipalPerson,
                EconomicMethodFatherTutor = student.StudentInformation.EconomicMethodFatherTutor,
                DSectorFatherTutor = student.StudentInformation.DSectorFatherTutor,
                DWorkConditionFatherTutor = student.StudentInformation.DWorkConditionFatherTutor,
                DEspecificActivityFatherTutor = student.StudentInformation.DEspecificActivityFatherTutor,
                DBusyFatherTutor = student.StudentInformation.DBusyFatherTutor,
                ISectorFatherTutor = student.StudentInformation.ISectorFatherTutor,
                IWorkConditionFatherTutor = student.StudentInformation.IWorkConditionFatherTutor,
                IEspecificActivityFatherTutor = student.StudentInformation.IEspecificActivityFatherTutor,
                EconomicMethodMother = student.StudentInformation.EconomicMethodMother,
                DSectorMother = student.StudentInformation.DSectorMother,
                DWorkConditionMother = student.StudentInformation.DWorkConditionMother,
                DEspecificActivityMother = student.StudentInformation.DEspecificActivityMother,
                DBusyMother = student.StudentInformation.DBusyMother,
                ISectorMother = student.StudentInformation.ISectorMother,
                IWorkConditionMother = student.StudentInformation.IWorkConditionMother,
                IEspecificActivityMother = student.StudentInformation.IEspecificActivityMother,
                EconomicExpensesFeeding = student.StudentInformation.EconomicExpensesFeeding,
                EconomicExpensesBasicServices = student.StudentInformation.EconomicExpensesBasicServices,
                EconomicExpensesEducation = student.StudentInformation.EconomicExpensesEducation,
                EconomicExpensesOthers = student.StudentInformation.EconomicExpensesOthers,
                FatherRemuneration = student.StudentInformation.FatherRemuneration,
                MotherRemuneration = student.StudentInformation.MotherRemuneration,
                StudentRemuneration = student.StudentInformation.StudentRemuneration,
                OtherRemuneration = student.StudentInformation.OtherRemuneration,
                TotalRemuneration = student.StudentInformation.TotalRemuneration,
                StudentDependency = student.StudentInformation.StudentDependency,
                StudentCoexistence = student.StudentInformation.StudentCoexistence,
                FamilyRisk = student.StudentInformation.FamilyRisk,
                StudentWorkDedication = student.StudentInformation.StudentWorkDedication,
                StudentWorkDescription = student.StudentInformation.StudentWorkDescription,
                StudentWorkCondition = student.StudentInformation.StudentWorkCondition,
                AuthorizeCheck = student.StudentInformation.AuthorizeCheck,
                AuthorizedPersonFullName = student.StudentInformation.AuthorizedPersonFullName,
                AuthorizedPersonAddress = student.StudentInformation.AuthorizedPersonAddress,
                AuthorizedPersonPhone = student.StudentInformation.AuthorizedPersonPhone,
                Tenure = student.StudentInformation.Tenure,
                ContructionType = student.StudentInformation.ContructionType,
                ZoneType = student.StudentInformation.ZoneType,
                BuildType = student.StudentInformation.BuildType,
                OtherTypeLivingPlace = student.StudentInformation.OtherTypeLivingPlace,
                NumberFloors = student.StudentInformation.NumberFloors,
                NumberRooms = student.StudentInformation.NumberRooms,
                NumberKitchen = student.StudentInformation.NumberKitchen,
                NumberBathroom = student.StudentInformation.NumberBathroom,
                NumberLivingRoom = student.StudentInformation.NumberLivingRoom,
                NumberDinningRoom = student.StudentInformation.NumberDinningRoom,
                Water = student.StudentInformation.Water,
                Drain = student.StudentInformation.Drain,
                LivingPlacePhone = student.StudentInformation.LivingPlacePhone,
                Light = student.StudentInformation.Light,
                Internet = student.StudentInformation.Internet,
                TV = student.StudentInformation.TV,
                HasPhone = student.StudentInformation.HasPhone,
                Radio = student.StudentInformation.Radio,
                Stereo = student.StudentInformation.Stereo,
                Iron = student.StudentInformation.Iron,
                EquipPhone = student.StudentInformation.EquipPhone,
                Laptop = student.StudentInformation.Laptop,
                Closet = student.StudentInformation.Closet,
                Fridge = student.StudentInformation.Fridge,
                PersonalLibrary = student.StudentInformation.PersonalLibrary,
                EquipComputer = student.StudentInformation.EquipComputer,

                BreakfastHome = student.StudentInformation.BreakfastHome,
                BreakfastPension = student.StudentInformation.BreakfastPension,
                BreakfastRelativeHome = student.StudentInformation.BreakfastRelativeHome,
                BreakfastOther = student.StudentInformation.BreakfastOther,
                LunchHome = student.StudentInformation.LunchHome,
                LunchPension = student.StudentInformation.LunchPension,
                LunchRelativeHome = student.StudentInformation.LunchRelativeHome,
                LunchOther = student.StudentInformation.LunchOther,
                DinnerHome = student.StudentInformation.DinnerHome,
                DinnerPension = student.StudentInformation.DinnerPension,
                DinnerRelativeHome = student.StudentInformation.DinnerRelativeHome,
                DinnerOther = student.StudentInformation.DinnerOther,

                IsSick = student.StudentInformation.IsSick,
                TypeParentIllness = student.StudentInformation.TypeParentIllness,
                HaveInsurance = student.StudentInformation.HaveInsurance,
                InsuranceDescription = student.StudentInformation.InsuranceDescription
            };
        }



        [HttpGet("composicion-familiar/estudiante/datatable")]
        public async Task<IActionResult> GetStudentFamilyDatatableGet()
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _studentService.GetStudentByUserName(user.UserName);

            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentFamilyService.GetAllFamilyMembersFromStudentDatatable(sentParameters, student.Id);
            return Ok(result);
        }
        /// <summary>
        /// Obtiene la información familiar del estudiante
        /// </summary>
        /// <param name="UserNameParameter">Usuario del estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("studentfamily/{UserNameParameter}")]
        public async Task<IActionResult> GetStudentFamilybyStudentInformation(string UserNameParameter)
        {
            var student = await _studentService.GetStudentByUserName(UserNameParameter);
            var result = (await _studentFamilyService.GetStudentFamilyByStudentId(student.Id))
                .Select(X => new
                {
                    id = X.Id,
                    name = X.Name,
                    paternalname = X.PaternalName,
                    maternalname = X.MaternalName,
                    birthday = X.Birthday.ToLocalDateFormat(),
                    relationship = ConstantHelpers.STUDENT_FAMILY.RELATIONSHIPS.TYPE[X.RelationshipInt],
                    civilstatus = ConstantHelpers.CIVIL_STATUS.VALUES[X.CivilStatusInt],
                    degreeinstruction = ConstantHelpers.STUDENT_FAMILY.CERTIFICATES.TYPE[X.DegreeInstructionInt],
                    certificated = X.Certificated,
                    occupation = X.Occupation,
                    workcenter = X.WorkCenter,
                    location = X.Location,
                    isSick = X.IsSick,
                    diseaseType = X.DiseaseType,
                    surgicalIntervention = X.SurgicalIntervention
                }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Elimina la información familiar del estudiante
        /// </summary>
        /// <param name="id">Identificador de la información familiar del estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("eliminar/studentFamily/{id}")]
        public async Task<IActionResult> DeleteStudentFamily(Guid id)
        {
            var result = await _studentFamilyService.GetStudentFamilyById(id);
            await _studentFamilyService.DeleteStudentFamily(result);
            return Ok();
        }

        /// <summary>
        /// Agrega un registro de familiar estudiante
        /// </summary>
        /// <param name="model">Modelo que contiene el familiar estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("agregar/studentFamily")]
        public async Task<IActionResult> AddStudentFamily(StudentFamilyViewModel model)
        {
            var student = await _studentService.GetStudentByUserName(model.UserNameParameter);

            if (!ModelState.IsValid) return BadRequest();
            var result = new StudentFamily
            {
                StudentId = student.Id,
                Name = model.Name,
                PaternalName = model.PaternalName,
                MaternalName = model.MaternalName,
                Birthday = ConvertHelpers.DatepickerToUtcDateTime(model.Birthday),
                RelationshipInt = model.RelationshipInt,
                CivilStatusInt = model.CivilStatusInt,
                DegreeInstructionInt = model.DegreeInstructionInt,
                Certificated = model.Certificated,
                Occupation = model.Occupation,
                WorkCenter = model.WorkCenter,
                Location = model.Location
            };

            await _studentFamilyService.InsertStudentFamily(result);
            return Ok();
        }

        /// <summary>
        /// Edita un registro de familiar estudiante
        /// </summary>
        /// <param name="model">Modelo que contiene el familiar estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("editar/studentFamily")]
        public async Task<IActionResult> EditStudentFamily(StudentFamilyViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var studentFamily = await _studentFamilyService.GetStudentFamilyByUsername(model.UserNameParameter);

            studentFamily.Name = model.Name;
            studentFamily.PaternalName = model.PaternalName;
            studentFamily.MaternalName = model.MaternalName;
            studentFamily.Birthday = ConvertHelpers.DatepickerToUtcDateTime(model.Birthday);
            studentFamily.RelationshipInt = model.RelationshipInt;
            studentFamily.CivilStatusInt = model.CivilStatusInt;
            studentFamily.DegreeInstructionInt = model.DegreeInstructionInt;
            studentFamily.Certificated = model.Certificated;
            studentFamily.Occupation = model.Occupation;
            studentFamily.WorkCenter = model.WorkCenter;
            studentFamily.Location = model.Location;

            await _studentFamilyService.UpdateStudentFamily(studentFamily);
            return Ok();
        }

        /// <summary>
        /// Agrega un registro de salud familiar estudiante
        /// </summary>
        /// <param name="model">Modelo que contiene la salud familiar estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("editar/studentFamilyHealth")]
        public async Task<IActionResult> EditStudentFamilyHealth(StudentFamilyViewModel model)
        {
            var studentFamily = await _studentFamilyService.GetStudentFamilyById(model.Id);

            studentFamily.IsSick = model.IsSick2;
            studentFamily.SurgicalIntervention = model.SurgicalIntervention2;
            studentFamily.DiseaseType = model.DiseaseType;

            await _studentFamilyService.UpdateStudentFamily(studentFamily);

            return Ok();
        }

        /// <summary>
        /// Obtiene la información del familiar del estudiante
        /// </summary>
        /// <param name="id">Identificador del familiar del estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("studentfamily/get/{id}")]
        public async Task<IActionResult> GetStudentFamily(Guid id)
        {
            var result = await _studentFamilyService.GetStudentFamilySelectById(id);
            return Ok(result);
        }

        private byte[] GenerarQv2(Guid studentId)
        {
            QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
            var URLAbsolute = HttpContext.Request.Host + "/ficha-socioeconomica/ver/" + studentId;
            QRCoder.QRCodeData qrCodeData = qrGenerator.CreateQrCode(URLAbsolute, QRCoder.QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCoder.PngByteQRCode(qrCodeData);
            //var stream = new MemoryStream();
            //qrCode.GetGraphic(5).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //return stream.ToArray();
            return qrCode.GetGraphic(5);
        }
        private string GetImageQR(Guid studentId)
        {
            var bitMap = GenerarQv2(studentId);
            var finalQR = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(bitMap));
            return finalQR;
        }
    }
}
