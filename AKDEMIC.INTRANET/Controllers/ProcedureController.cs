using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.CORE.Helpers;
using System;
using AKDEMIC.INTRANET.ViewModels.ProcedureViewModels;
using System.Linq;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System.Collections.Generic;
using AKDEMIC.CORE.Extensions;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using DinkToPdf.Contracts;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.ENTITIES.Models.Enrollment;
using Newtonsoft.Json;
using static AKDEMIC.CORE.Helpers.ConstantHelpers;
using Microsoft.AspNetCore.Http;
using static OpenIddict.Abstractions.OpenIddictConstants;
using SixLabors.ImageSharp.ColorSpaces;
using static AKDEMIC.INTRANET.Helpers.ConstantHelpers;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize]
    [Route("tramites")]
    public class ProcedureController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly ISectionService _sectionService;
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;
        private readonly AkdemicContext _context;
        private readonly IProcedureService _procedureService;
        private readonly IProcedureRequirementService _procedureRequirementService;
        private readonly IConfigurationService _configurationService;
        private readonly ITermService _termService;
        private readonly IDependencyService _dependencyService;
        private readonly IStudentService _studentService;
        private readonly IConceptService _conceptService;
        private readonly IUserProcedureService _userProcedureService;
        private readonly ICourseTermService _courseTermService;
        private readonly IPaymentService _paymentService;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly ICareerService _careerService;
        private readonly ICurriculumService _curriculumService;
        private readonly IAcademicProgramService _academicProgramService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRecordHistoryService _recordHistoryService;
        private readonly IProcedureTaskService _procedureTaskService;
        private readonly IEnrollmentTurnService _enrollmentTurnService;
        private readonly IStudentUserProcedureService _studentUserProcedureService;
        private readonly IConverter _converter;
        private readonly IViewRenderService _viewRenderService;
        private readonly ICareerEnrollmentShiftService _careerEnrollmentShiftService;
        private readonly IUserProcedureFileService _userProcedureFileService;

        public ProcedureController(
            IDataTablesService dataTablesService,
            ISectionService sectionService,
            ICourseService courseService,
            IUserService userService,
            AkdemicContext context,
            IProcedureService procedureService,
            IProcedureRequirementService procedureRequirementService,
            IConfigurationService configurationService,
            ITermService termService,
            IDependencyService dependencyService,
            IStudentService studentService,
            IConceptService conceptService,
            IUserProcedureService userProcedureService,
            ICourseTermService courseTermService,
            IPaymentService paymentService,
            ICloudStorageService cloudStorageService,
            ICareerService careerService,
            ICurriculumService curriculumService,
            IAcademicProgramService academicProgramService,
            IStudentSectionService studentSectionService,
            IWebHostEnvironment webHostEnvironment,
            IRecordHistoryService recordHistoryService,
            IProcedureTaskService procedureTaskService,
            IEnrollmentTurnService enrollmentTurnService,
            IStudentUserProcedureService studentUserProcedureService,
            IConverter converter,
            IViewRenderService viewRenderService,
            ICareerEnrollmentShiftService careerEnrollmentShiftService,
            IUserProcedureFileService userProcedureFileService
            )
        {
            _dataTablesService = dataTablesService;
            _sectionService = sectionService;
            _courseService = courseService;
            _userService = userService;
            _context = context;
            _procedureService = procedureService;
            _procedureRequirementService = procedureRequirementService;
            _configurationService = configurationService;
            _termService = termService;
            _dependencyService = dependencyService;
            _studentService = studentService;
            _conceptService = conceptService;
            _userProcedureService = userProcedureService;
            _courseTermService = courseTermService;
            _paymentService = paymentService;
            _cloudStorageService = cloudStorageService;
            _careerService = careerService;
            _curriculumService = curriculumService;
            _academicProgramService = academicProgramService;
            _studentSectionService = studentSectionService;
            _webHostEnvironment = webHostEnvironment;
            _recordHistoryService = recordHistoryService;
            _procedureTaskService = procedureTaskService;
            _enrollmentTurnService = enrollmentTurnService;
            _studentUserProcedureService = studentUserProcedureService;
            _converter = converter;
            _viewRenderService = viewRenderService;
            _careerEnrollmentShiftService = careerEnrollmentShiftService;
            _userProcedureFileService = userProcedureFileService;
        }


        #region TO DELETE

        /// <summary>
        /// Obtiene los tramites del estudiante logeado
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [Route("get")]
        public async Task<IActionResult> GetProcedures()
        {
            var user = await _userService.GetUserByClaim(User);
            var result = await _procedureService.GetProceduresByUser(user.Id);

            return Ok(result);
        }

        #endregion

        [HttpGet("get-tramites-disponibles/datatable")]
        public async Task<IActionResult> GetAvailableProceduresDatatable(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _procedureService.GetAvailableProceduresByUserDatatable(parameters, User, search);
            return Ok(result);
        }

        [HttpGet("get-tramites-en-proceso/datatable")]
        public async Task<IActionResult> GetProceduresInProcessDatatable(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _userProcedureService.GetUserProceduresDatatable(parameters, User, false, search);
            return Ok(result);
        }

        [HttpGet("get-tramites-finalizados/datatable")]
        public async Task<IActionResult> GetCompletedProceduresDatatable(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _userProcedureService.GetUserProceduresDatatable(parameters, User, true, search);
            return Ok(result);
        }

        public async Task<IActionResult> Index(int? tab = null)
        {
            var confiEnableTupa = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.DocumentaryProcedureManagement.ENABLE_TUPA);

            if (!bool.Parse(confiEnableTupa))
            {
                ErrorToastMessage("Los trámites no se encuentran activos");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }


            var confiTupaReadOnly = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.DocumentaryProcedureManagement.TUPA_READONLY);
            ViewBag.Tab = tab;
            ViewBag.TupaReadonly = bool.Parse(confiTupaReadOnly);

            return View();
        }

        [HttpGet("detalles/{id:Guid}")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var user = await _userService.GetUserByClaim(User);
            var procedure = await _procedureService.GetProcedure(id);
            var requirements = await _procedureRequirementService.GetProcedureRequirementsByProcedure(id);
            var task = await _procedureTaskService.GetProcedureTasks(procedure.Id);

            var confiEnabledTupa = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.DocumentaryProcedureManagement.ENABLE_TUPA);
            var confiTupaReadOnly = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.DocumentaryProcedureManagement.TUPA_READONLY);
            var confiPaymentType = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.DocumentaryProcedureManagement.PROCEDURE_PAYMENT_TYPE);
            var confiPreviousPaymentType = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.DocumentaryProcedureManagement.PROCEDURE_PREVIOUS_PAYMENT_TYPE);

            var tryParseConfiPaymentType = byte.TryParse(confiPaymentType, out var paymentType);
            var tryParseConfiPreviousPaymentType = byte.TryParse(confiPreviousPaymentType, out var previousPaymentType);

            var pendingPaymentTitle = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_PROFORMA_TITLE_TEXT);

            var model = new ProcedureViewModel
            {
                Id = procedure.Id,
                Code = procedure.Code,
                ConceptId = procedure.ConceptId,
                Concept = procedure.Concept?.Code,
                ConceptAmount = procedure.Concept?.Amount,
                StartDependencyEqualToDepedency = procedure.Dependency?.Id == procedure.StartDependency?.Id,
                Dependency = procedure.Dependency?.Name,
                StartDependency = procedure.StartDependency?.Name,
                Description = procedure.Description,
                Name = procedure.Name,
                LegalBase = procedure.LegalBase,
                Duration = procedure.Duration,
                NeedImage = procedure.HasPicture,
                PendingPaymentTitle = pendingPaymentTitle,
                Configuration = new ConfigurationViewmodel
                {
                    EnableTupa = bool.Parse(confiEnabledTupa),
                    TupaReadOnly = bool.Parse(confiTupaReadOnly),
                    PaymentType = tryParseConfiPaymentType ? paymentType : (byte?)null,
                    PreviousPaymentType = tryParseConfiPreviousPaymentType ? previousPaymentType : (byte?)null
                },
                Requirements = requirements.Select(x => new RequirementViewModel
                {
                    Code = x.Code,
                    Cost = x.Type == ConstantHelpers.PROCEDURE_REQUIREMENTS.TYPE.COST ? x.Cost : (decimal?)null,
                    Name = x.Name,
                    SystemValidationType = x.SystemValidationType,
                    Id = x.Id,
                    Type = x.Type
                }).ToList()
            };

            if (procedure.Score == ConstantHelpers.PROCEDURES.SCORE.AUTOMATIC || procedure.Score == ConstantHelpers.PROCEDURES.SCORE.SEMIAUTOMATIC)
            {
                var automaticTask = task.Where(x => x.Type != ConstantHelpers.PROCEDURE_TASK.TYPE.DESCRIPTION).FirstOrDefault();

                if (automaticTask != null)
                {
                    if (automaticTask.Type == ConstantHelpers.PROCEDURE_TASK.TYPE.RECORD_HISTORY)
                    {
                        model.RecordHistory = new RecordHistoryViewModel
                        {
                            Type = automaticTask.RecordHistoryType
                        };
                    }
                    else if (automaticTask.Type == ConstantHelpers.PROCEDURE_TASK.TYPE.ACTIVITY)
                    {
                        model.Activity = new ActivityViewModel
                        {
                            Type = automaticTask.ActivityType
                        };
                    }
                }
            }

            if (User.IsInRole(ConstantHelpers.ROLES.STUDENTS) && !procedure.StartDependencyId.HasValue)
            {
                var student = await _studentService.GetStudentByUser(user.Id);

                if (procedure.Type == ConstantHelpers.PROCEDURES.TYPE.CAREER)
                {
                    var dependencies = await _dependencyService.GetDependenciesByCareer(student.CareerId);

                    if (!dependencies.Any())
                        return BadRequest("El trámite no tiene asignado un área de inicio.");

                    model.StartDependency = dependencies.Select(x => x.Name).FirstOrDefault();
                }
                else if (procedure.Type == ConstantHelpers.PROCEDURES.TYPE.FACULTY)
                {
                    var career = await _careerService.Get(student.CareerId);
                    var dependencies = await _dependencyService.GetDependenciesByFaculty(career.FacultyId);

                    if (!dependencies.Any())
                        return BadRequest("El trámite no tiene asignado un área de inicio.");

                    model.StartDependency = dependencies.Select(x => x.Name).FirstOrDefault();
                }
            }

            return View(model);
        }

        [HttpPost("iniciar-tramite")]
        public async Task<IActionResult> StartProcedure(ProcedureViewModel model)
        {
            var user = await _userService.GetUserByClaim(User);
            var procedure = await _procedureService.Get(model.Id);
            var term = await _termService.GetActiveTerm();
            var task = await _procedureTaskService.GetProcedureTasks(procedure.Id);

            var confiPaymentType = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.DocumentaryProcedureManagement.PROCEDURE_PAYMENT_TYPE);
            var confiPreviousPaymentType = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.DocumentaryProcedureManagement.PROCEDURE_PREVIOUS_PAYMENT_TYPE);
            var confiMaxRequestCourseWithdrawal = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.MAX_COURSE_WITHDRAWAL);

            var tryParseConfiPaymentType = byte.TryParse(confiPaymentType, out var paymentType);
            var tryParseConfiPreviousPaymentType = byte.TryParse(confiPreviousPaymentType, out var previousPaymentType);
            var tryParseConfiMaxRequestCourseWithdrawal = byte.TryParse(confiMaxRequestCourseWithdrawal, out var maxRequestCourseWithdrawal);

            //if (await _userProcedureService.AnyUserProcedurePending(user.Id, procedure.Id))
            //    return BadRequest($"Se encontró un trámite pendiente.");

            if (procedure.MaximumRequestByTerm.HasValue)
            {
                var userProceduresRequested = await _userProcedureService.GetUserProceduresByUserId(user.Id, term.Id, null, procedure.Id);

                if (userProceduresRequested.Count() >= procedure.MaximumRequestByTerm)
                    return BadRequest($"El límite de solicitudes del trámite son {procedure.MaximumRequestByTerm}");
            }

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNICA)
            {
                var userProcedureUnschCount = await _context.UserProcedures
                    .Where(x => x.UserId == user.Id && x.StudentUserProcedureId.HasValue && (x.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.GRADE_RECOVERY || x.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.EXTRAORDINARY_EVALUATION))
                    .CountAsync();

                if (userProcedureUnschCount >= 3)
                    return BadRequest("Solo puede solicitar como máximo 3 trámites entre sustitutorio y aplazados.");
            }

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSCH)
            {
                var userProcedureUnschCount = await _context.UserProcedures
                    .Where(x => x.UserId == user.Id && x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE && x.StudentUserProcedureId.HasValue && (x.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.GRADE_RECOVERY || x.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.EXTRAORDINARY_EVALUATION))
                    .CountAsync();

                if (userProcedureUnschCount >= 3)
                    return BadRequest("Solo puede solicitar como máximo 3 trámites entre evaluación extraordinaria y recuperación de notas.");
            }

            var validateSystemRequirements = await _procedureService.ValidateSystemRequirements(procedure.Id, User);

            if (!validateSystemRequirements.Success)
            {
                return BadRequest(validateSystemRequirements.Message);
            }

            Guid? dependencyId = null;

            if (procedure.StartDependencyId.HasValue)
            {
                var dependency = await _dependencyService.Get(procedure.StartDependencyId.Value);
                dependencyId = dependency.Id;
            }
            else
            {
                if (User.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                {
                    var student = await _studentService.GetStudentByUser(user.Id);

                    if (procedure.Type == ConstantHelpers.PROCEDURES.TYPE.CAREER)
                    {
                        var dependencies = await _dependencyService.GetDependenciesByCareer(student.CareerId);

                        if (!dependencies.Any())
                            return BadRequest("El trámite no tiene asignado un área de inicio.");

                        dependencyId = dependencies.Select(x => x.Id).FirstOrDefault();
                    }
                    else if (procedure.Type == ConstantHelpers.PROCEDURES.TYPE.FACULTY)
                    {
                        var career = await _careerService.Get(student.CareerId);
                        var dependencies = await _dependencyService.GetDependenciesByFaculty(career.FacultyId);

                        if (!dependencies.Any())
                            return BadRequest("El trámite no tiene asignado un área de inicio.");

                        dependencyId = dependencies.Select(x => x.Id).FirstOrDefault();
                    }
                    else
                    {
                        return BadRequest("El trámite no tiene asignado un área de inicio.");
                    }
                }
            }

            if (procedure.HasPicture && string.IsNullOrEmpty(model.UserCroppedImage))
            {
                return BadRequest("Es obligatorio adjuntar una foto.");
            }

            var userProcedureFiles = new List<UserProcedureFile>();

            if (model.Requirements != null)
            {
                foreach (var requirement in model.Requirements)
                {
                    try
                    {
                        var uploadFilePath = await _cloudStorageService.UploadFile(requirement.File.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.USER_PROCEDURE_FILE,
                            Path.GetExtension(requirement.File.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.DOCUMENTARY_PROCEDURE);

                        userProcedureFiles.Add(new UserProcedureFile
                        {
                            FileName = requirement.File.FileName,
                            Path = uploadFilePath,
                            Size = requirement.File.Length,
                            ProcedureRequirementId = requirement.Id,
                            Status = CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.STATUS.SENT
                        });
                    }
                    catch (Exception)
                    {
                        return BadRequest($"Hubo un problema al subir el archivo '{requirement.File.FileName}'");
                    }
                }
            }

            var correlative = await _userProcedureService.GetNextCorrelative(procedure.Id);

            var userProcedure = new UserProcedure
            {
                DependencyId = dependencyId,
                ProcedureId = procedure.Id,
                Correlative = correlative,
                TermId = term?.Id,
                UserId = user.Id,
                Comment = model.StudentComment,
                Status = ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED,
                UserProcedureFiles = userProcedureFiles
            };

            if (procedure.HasPicture)
            {
                try
                {
                    var pictureBase64Data = Regex.Match(model.UserCroppedImage, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                    var pictureBinData = Convert.FromBase64String(pictureBase64Data);
                    var pictureStream = new MemoryStream(pictureBinData);
                    userProcedure.UrlImage = await _cloudStorageService.UploadFile(pictureStream, ConstantHelpers.CONTAINER_NAMES.USER_PROCEDURE_FILE, ".png", ConstantHelpers.FileStorage.SystemFolder.DOCUMENTARY_PROCEDURE);
                }
                catch (Exception)
                {
                    return BadRequest("Error al guardar la foto adjunta.");
                }
            }

            await _userProcedureService.AddAsync(userProcedure);

            if (procedure.ConceptId.HasValue)
            {
                var concept = await _conceptService.GetConcept(procedure.ConceptId.Value);

                if (concept.Amount > 0)
                {
                    if (paymentType == ConstantHelpers.PROCEDURES.CONFIGURATION.PAYMENT_TYPE.PREVIOUS_PAYMENT)
                    {
                        if (model.Payment is null)
                            return BadRequest("Es obligatorio seleccionar el pago.");
                        var payment = await _paymentService.Get(model.Payment.Id);

                        if (payment is null)
                            return BadRequest("No se encontró el pago seleccionado.");

                        userProcedure.PaymentId = payment.Id;
                        payment.EntityId = userProcedure.Id;
                        payment.UserId = user.Id;
                        payment.WasBankPaymentUsed = true;
                    }
                    else if (paymentType == ConstantHelpers.PROCEDURES.CONFIGURATION.PAYMENT_TYPE.LATER_PAYMENT)
                    {
                        var total = concept.Amount;

                        if (User.IsInRole(ConstantHelpers.ROLES.STUDENTS) && (procedure.Score == ConstantHelpers.PROCEDURES.SCORE.AUTOMATIC || procedure.Score == ConstantHelpers.PROCEDURES.SCORE.SEMIAUTOMATIC))
                        {
                            if (task.Any(x => x.Type != ConstantHelpers.PROCEDURE_TASK.TYPE.DESCRIPTION))
                            {
                                var automaticTask = task.Where(x => x.Type != ConstantHelpers.PROCEDURE_TASK.TYPE.DESCRIPTION).FirstOrDefault();
                                if (automaticTask.Type == ConstantHelpers.PROCEDURE_TASK.TYPE.RECORD_HISTORY && automaticTask.RecordHistoryType == ConstantHelpers.RECORDS.CERTIFICATEOFSTUDIESPARTIAL)
                                {
                                    var countAcademicYearSelected = JsonConvert.DeserializeObject<object[]>(model.RecordHistory.JsonAcademicYears);
                                    total = total * countAcademicYearSelected.Count();
                                }

                                if (automaticTask.Type == ConstantHelpers.PROCEDURE_TASK.TYPE.ACTIVITY && automaticTask.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.COURSE_WITHDRAWAL_MASSIVE)
                                {
                                    var countCoursesSelected = JsonConvert.DeserializeObject<object[]>(model.Activity.JsonStudentSectionIds);
                                    total = total * countCoursesSelected.Count();
                                }
                            }
                        }


                        var subtotal = total;
                        var igvAmount = 0.00M;

                        if (concept.IsTaxed)
                        {
                            subtotal = total / (1.00M + ConstantHelpers.PAYMENT.IGV);
                            igvAmount = total - subtotal;
                        }

                        var payment = new Payment
                        {
                            Description = procedure.Name,
                            EntityId = userProcedure.Id,
                            Type = ConstantHelpers.PAYMENT.TYPES.PROCEDURE,
                            UserId = user.Id,
                            SubTotal = subtotal,
                            IgvAmount = igvAmount,
                            Discount = 0.00M,
                            Total = total,
                            ConceptId = concept.Id,
                            TermId = term?.Id,
                        };

                        if (model.Activity != null)
                        {
                            if (model.Activity.StudentSectionId.HasValue)
                            {
                                var courseName = await _context.StudentSections
                                    .Where(x => x.Id == model.Activity.StudentSectionId)
                                    .Select(x => $"{x.Section.CourseTerm.Course.Code}-{x.Section.CourseTerm.Course.Name}")
                                    .FirstOrDefaultAsync();

                                payment.Description = $"{procedure.Name} {courseName}";
                            }

                            if (model.Activity.CourseId.HasValue)
                            {
                                var courseName = await _context.Courses
                                    .Where(x => x.Id == model.Activity.CourseId)
                                    .Select(x => $"{x.Code}-{x.Name}")
                                    .FirstOrDefaultAsync();

                                payment.Description = $"{procedure.Name} {courseName}";
                            }
                        }

                        userProcedure.Payment = payment;
                        userProcedure.Status = ConstantHelpers.USER_PROCEDURES.STATUS.PENDING_PAYMENT;
                    }
                }
            }

            if (User.IsInRole(ConstantHelpers.ROLES.STUDENTS) && (procedure.Score == ConstantHelpers.PROCEDURES.SCORE.AUTOMATIC || procedure.Score == ConstantHelpers.PROCEDURES.SCORE.SEMIAUTOMATIC))
            {
                if (task.Any(x => x.Type != ConstantHelpers.PROCEDURE_TASK.TYPE.DESCRIPTION))
                {
                    var automaticTask = task.Where(x => x.Type != ConstantHelpers.PROCEDURE_TASK.TYPE.DESCRIPTION).FirstOrDefault();

                    var student = await _studentService.GetStudentByUser(user.Id);

                    if (automaticTask.Type == ConstantHelpers.PROCEDURE_TASK.TYPE.RECORD_HISTORY)
                    {
                        var recordHistory = new RecordHistory
                        {
                            Type = automaticTask.RecordHistoryType.Value,
                            Date = DateTime.UtcNow,
                            Number = await _recordHistoryService.GetLatestRecordNumberByType(automaticTask.RecordHistoryType.Value, DateTime.UtcNow.Year) + 1,
                            StudentId = student.Id,
                            Status = ConstantHelpers.RECORD_HISTORY_STATUS.FINALIZED
                        };

                        if (automaticTask.RecordHistoryType == ConstantHelpers.RECORDS.CERTIFICATEOFSTUDIESPARTIAL)
                        {
                            recordHistory.JsonAcademicYears = model.RecordHistory.JsonAcademicYears;
                        }

                        if (
                            automaticTask.RecordHistoryType == ConstantHelpers.RECORDS.STUDYRECORD ||
                            automaticTask.RecordHistoryType == ConstantHelpers.RECORDS.REGULARSTUDIES ||
                            automaticTask.RecordHistoryType == ConstantHelpers.RECORDS.REPORT_CARD ||

                            automaticTask.RecordHistoryType == ConstantHelpers.RECORDS.UPPERTHIRD ||
                            automaticTask.RecordHistoryType == ConstantHelpers.RECORDS.UPPERFIFTH ||
                            automaticTask.RecordHistoryType == ConstantHelpers.RECORDS.TENTH_HIGHER ||
                            automaticTask.RecordHistoryType == ConstantHelpers.RECORDS.UPPER_MIDDLE ||

                            automaticTask.RecordHistoryType == ConstantHelpers.RECORDS.ENROLLMENT_REPORT
                            )
                        {

                            if (!model.RecordHistory.TermId.HasValue)
                                return BadRequest("Es necesario seleccionar el periodo.");

                            if (automaticTask.RecordHistoryType == ConstantHelpers.RECORDS.REPORT_CARD && term != null && term.Id == model.RecordHistory.TermId)
                                return BadRequest("No se puede solicitar el documento para el periodo activo.");

                            recordHistory.RecordTermId = model.RecordHistory.TermId;
                        }

                        userProcedure.RecordHistory = recordHistory;
                    }
                    else if (automaticTask.Type == ConstantHelpers.PROCEDURE_TASK.TYPE.ACTIVITY)
                    {
                        var studentUserProcedures = await _userProcedureService.GetStudentUserProcedures(student.Id, term?.Id);
                        var studentUserProceduresPending = studentUserProcedures.Where(x => x.Status != ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED).ToList();

                        var studentUserProcedure = new StudentUserProcedure
                        {
                            AcademicProgramId = model.Activity.AcademicProgramId,
                            CareerId = model.Activity.CareerId,
                            CurriculumId = model.Activity.CurriculumId,
                            StudentSectionId = model.Activity.StudentSectionId,
                            StudentId = student.Id,
                            TermId = term?.Id,
                            ActivityType = automaticTask.ActivityType.Value,
                            CourseId = model.Activity.CourseId
                        };

                        switch (automaticTask.ActivityType)
                        {
                            case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.CAREER_TRANSFER:
                                if (!model.Activity.CareerId.HasValue)
                                    return BadRequest("Es obligatorio seleccionar la escuela profesional.");

                                if (!model.Activity.CurriculumId.HasValue)
                                    return BadRequest("Es obligatorio seleccionar el plan de estudio.");

                                if (studentUserProceduresPending
                                    .Any(y => y.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.CAREER_TRANSFER
                                    ))
                                {
                                    return BadRequest("Se encontró una solicitud de Traslado de Escuela pendiente.");
                                }

                                break;

                            case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.ACADEMIC_YEAR_WITHDRAWAL:
                                if (studentUserProceduresPending
                                    .Any(y => y.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.ACADEMIC_YEAR_WITHDRAWAL
                                    ))
                                {
                                    return BadRequest("Se encontró una solicitud de Retiro de Ciclo pendiente.");
                                }
                                break;

                            case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.COURSE_WITHDRAWAL:
                                if (!model.Activity.StudentSectionId.HasValue)
                                    return BadRequest("Es obligatorio seleccionar el curso a retirar.");

                                if (studentUserProceduresPending
                                   .Any(y => y.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.COURSE_WITHDRAWAL
                                   && y.StudentUserProcedure.StudentSectionId == model.Activity.StudentSectionId
                                   ))
                                {
                                    return BadRequest("Se encontró una solicitud de retiro de asignatura pendiente.");
                                }

                                break;

                            case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.COURSE_WITHDRAWAL_MASSIVE:
                                if (string.IsNullOrEmpty(model.Activity.JsonStudentSectionIds))
                                    return BadRequest("Es obligatorio seleccionar los cursos a retirar.");

                                var studentSectionsIds = JsonConvert.DeserializeObject<List<Guid>>(model.Activity.JsonStudentSectionIds);

                                var requestCourseWithdrawalMassive = studentUserProceduresPending.Where(x => x.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.COURSE_WITHDRAWAL_MASSIVE).SelectMany(x => x.StudentUserProcedure.StudentUserProcedureDetails.Select(y => y.StudentSectionId)).ToList();
                                var requestCourseWithdrawal = studentUserProceduresPending.Where(x => x.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.COURSE_WITHDRAWAL).Select(x => x.StudentUserProcedure.StudentSectionId).ToList();

                                if (requestCourseWithdrawalMassive.Where(y => y.HasValue).Any(y => studentSectionsIds.Contains(y.Value)))
                                {
                                    var sectionsWithRequest = requestCourseWithdrawalMassive.Where(x => x.HasValue && studentSectionsIds.Contains(x.Value)).ToList();
                                    var coursesRepeat = await _context.StudentSections.Where(x => sectionsWithRequest.Contains(x.Id))
                                        .Select(x => x.Section.CourseTerm.Course.Name).ToListAsync();
                                    if (coursesRepeat.Any())
                                        return BadRequest($"Se encontroraron solicitudes de retiro {(coursesRepeat.Count() == 1 ? "del curso" : "de los cursos")} {string.Join("; ", coursesRepeat)}");
                                }

                                if (requestCourseWithdrawal.Where(y => y.HasValue).Any(y => studentSectionsIds.Contains(y.Value)))
                                {
                                    var sectionsWithRequest = requestCourseWithdrawal.Where(x => x.HasValue && studentSectionsIds.Contains(x.Value)).ToList();
                                    var coursesRepeat = await _context.StudentSections.Where(x => sectionsWithRequest.Contains(x.Id))
                                   .Select(x => x.Section.CourseTerm.Course.Name).ToListAsync();

                                    if (coursesRepeat.Any())
                                        return BadRequest($"Se encontroraron solicitudes de retiro {(coursesRepeat.Count() == 1 ? "del curso" : "de los cursos")} {string.Join("; ", coursesRepeat)}");
                                }

                                if (tryParseConfiMaxRequestCourseWithdrawal)
                                {
                                    var studentUserProceduresDetails = studentUserProcedures.SelectMany(x => x.StudentUserProcedure.StudentUserProcedureDetails).ToList();
                                    if ((studentUserProceduresDetails.Count() + studentSectionsIds.Count()) > maxRequestCourseWithdrawal)
                                    {
                                        return BadRequest($"Solo puede solicitar el retiro de {maxRequestCourseWithdrawal} cursos como máximo.");
                                    }
                                }

                                studentUserProcedure.StudentUserProcedureDetails = studentSectionsIds
                                    .Select(x => new StudentUserProcedureDetail
                                    {
                                        StudentSectionId = x
                                    })
                                    .ToList();

                                break;

                            case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.RESIGNATION:
                                if (studentUserProceduresPending
                                  .Any(y => y.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.RESIGNATION
                                  ))
                                {
                                    return BadRequest("Se encontró una solicitud de Renuncia como estudiante pendiente.");
                                }
                                break;

                            case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.REENTRY:
                                if (studentUserProceduresPending
                                .Any(y => y.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.REENTRY
                                ))
                                {
                                    return BadRequest("Se encontró una solicitud de Reingreso como estudiante pendiente.");
                                }
                                break;

                            case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.REGISTRATION_RESERVATION:
                                if (studentUserProceduresPending
                                .Any(y => y.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.REGISTRATION_RESERVATION
                                ))
                                {
                                    return BadRequest("Se encontró una solicitud de Reserva de matrícula pendiente.");
                                }
                                break;

                            case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.CHANGE_ACADEMIC_PROGRAM:
                                if (!model.Activity.AcademicProgramId.HasValue)
                                    return BadRequest("Es obligatorio seleccionar la nueva especialidad.");

                                if (studentUserProceduresPending
                                   .Any(y => y.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.CHANGE_ACADEMIC_PROGRAM
                                   ))
                                {
                                    return BadRequest("Se encontró una solicitud de Cambio de especialidad pendiente.");
                                }

                                break;

                            case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.EXONERATED_COURSE:
                                if (!model.Activity.CourseId.HasValue)
                                    return BadRequest("Es obligatorio seleccionar el curso.");

                                if (studentUserProceduresPending
                                   .Any(y => y.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.EXONERATED_COURSE
                                   && y.StudentUserProcedure.CourseId == model.Activity.CourseId
                                   ))
                                {
                                    return BadRequest("Se encontró una solicitud de Exoneración de curso pendiente.");
                                }

                                var coursesAvailableForExoneratedCourse = await _studentService.GetCoursesAvailableForExoneratedCourse(student.Id);

                                if (!coursesAvailableForExoneratedCourse.Any(y => y.CourseId == model.Activity.CourseId.Value))
                                    return BadRequest("El curso seleccionado no se encuentra disponible para el trámite seleccionado.");

                                break;

                            case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.EXTRAORDINARY_EVALUATION:
                                if (!model.Activity.CourseId.HasValue)
                                    return BadRequest("Es obligatorio seleccionar el curso.");

                                if (studentUserProceduresPending
                                 .Any(y => y.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.EXTRAORDINARY_EVALUATION
                                 && y.StudentUserProcedure.CourseId == model.Activity.CourseId
                                 ))
                                {
                                    return BadRequest("Se encontró una solicitud de Evaluación Extraordinaria pendiente.");
                                }

                                var coursesAvailableForExtraordinaryEvaluation = await _studentService.GetCoursesAvailableForExtraordinaryEvaluation(student.Id);

                                if (!coursesAvailableForExtraordinaryEvaluation.Any(y => y.CourseId == model.Activity.CourseId.Value))
                                    return BadRequest("El curso seleccionado no se encuentra disponible para el trámite seleccionado.");


                                break;

                            case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.SUBSTITUTE_EXAM:
                                {
                                    if (!model.Activity.StudentSectionId.HasValue)
                                        return BadRequest("Es obligatorio seleccionar el curso.");

                                    var studentSection = await _context.StudentSections.Where(x => x.Id == model.Activity.StudentSectionId).FirstOrDefaultAsync();
                                    var section = await _context.Sections.Where(x => x.Id == studentSection.SectionId).FirstOrDefaultAsync();

                                    if (studentUserProceduresPending
                                       .Any(y => y.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.SUBSTITUTE_EXAM
                                       && y.StudentUserProcedure.StudentSectionId == model.Activity.StudentSectionId
                                       ))
                                    {
                                        return BadRequest("Se encontró una solicitud de examen sustitutorio pendiente.");
                                    }

                                    var enrolledCoursesAvailableToSubstitueExam = await _studentService.GetEnrolledCoursesAvailableToSubstitueExam(student.Id);

                                    if (!enrolledCoursesAvailableToSubstitueExam.Any(y => y.StudentSectionId == model.Activity.StudentSectionId.Value))
                                        return BadRequest("El curso seleccionado no se encuentra disponible para el trámite seleccionado.");

                                    if (await _context.SubstituteExams.Where(x => x.StudentId == student.Id && x.SectionId == section.Id && (x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED || x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED)).AnyAsync())
                                        return BadRequest("Ya se encuentra registrado en un examen sustitutorio");
                                }

                                break;

                            case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.GRADE_RECOVERY:
                                {
                                    if (!model.Activity.StudentSectionId.HasValue)
                                        return BadRequest("Es obligatorio seleccionar el curso.");

                                    var studentSection = await _context.StudentSections.Where(x => x.Id == model.Activity.StudentSectionId).FirstOrDefaultAsync();
                                    var section = await _context.Sections.Where(x => x.Id == studentSection.SectionId).FirstOrDefaultAsync();

                                    if (studentUserProceduresPending
                                       .Any(y => y.StudentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.GRADE_RECOVERY
                                       && y.StudentUserProcedure.StudentSectionId == model.Activity.StudentSectionId
                                       ))
                                    {
                                        return BadRequest("Se encontró una solicitud de recuperación de nota pendiente.");
                                    }

                                    var enrolledCoursesToGradeRecovery = await _studentService.GetEnrolledCoursesToGradeRecovery(student.Id);

                                    if (!enrolledCoursesToGradeRecovery.Any(y => y.StudentSectionId == model.Activity.StudentSectionId.Value))
                                        return BadRequest("El curso seleccionado no se encuentra disponible para el trámite seleccionado.");

                                    var anySubstituteExams = await _context.SubstituteExams.Where(x => x.SectionId == studentSection.SectionId && x.StudentId == studentSection.StudentId && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED).AnyAsync();

                                    if (anySubstituteExams)
                                        return BadRequest("El curso seleccionado tiene un examen sustitutorio pendiente.");

                                    if (await _context.GradeRecoveries.Where(x => x.StudentId == student.Id && x.CourseTermId == section.CourseTermId).AnyAsync())
                                        return BadRequest("Ya se encuentra registrado en un examen de recuperación de nota.");
                                }

                                break;
                        }

                        userProcedure.StudentUserProcedure = studentUserProcedure;

                        if (userProcedure.Status != ConstantHelpers.USER_PROCEDURES.STATUS.PENDING_PAYMENT && procedure.Score != ConstantHelpers.PROCEDURES.SCORE.SEMIAUTOMATIC)
                        {
                            var result = await _studentService.ExecuteProcedureActivity(User, userProcedure, studentUserProcedure);

                            if (!result.Succeeded)
                                return BadRequest(result.Message);
                        }
                    }
                }

                if (userProcedure.Status != ConstantHelpers.USER_PROCEDURES.STATUS.PENDING_PAYMENT && procedure.Score != ConstantHelpers.PROCEDURES.SCORE.SEMIAUTOMATIC)
                    userProcedure.Status = ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED;
            }

            await _userProcedureService.Update(userProcedure);

            var okResult = new
            {
                id = userProcedure.Id,
                hasPreviousPayment = userProcedure.Status == ConstantHelpers.USER_PROCEDURES.STATUS.PENDING_PAYMENT
            };

            return Ok(okResult);
        }

        [HttpGet("detalles-solicitud/{id:Guid}")]
        public async Task<IActionResult> UserProcedureDetail(Guid id)
        {
            var userProcedure = await _userProcedureService.GetUserProcedure(id);
            var procedure = await _procedureService.GetProcedure(userProcedure.ProcedureId);
            var userProcedureFiles = await _userProcedureFileService.GetUserProcedureFiles(userProcedure.Id);

            var model = new UserProcedureViewModel
            {
                CreatedAt = userProcedure.CreatedAt.ToLocalDateTimeFormat(),
                Id = userProcedure.Id,
                Term = userProcedure.Term?.Name,
                Comment = userProcedure.Comment,
                Dependency = userProcedure.Dependency?.Name,
                UrlImage = userProcedure.UrlImage,
                Status = userProcedure.Status,
                StatusStr = ConstantHelpers.USER_PROCEDURES.STATUS.VALUES[userProcedure.Status],
                Payment = userProcedure.Payment != null ? new PaymentViewModel
                {
                    Id = userProcedure.Payment.Id,
                    Concept = userProcedure.Payment.Concept.Code,
                    Discount = userProcedure.Payment.Discount,
                    IgvAmount = userProcedure.Payment.IgvAmount,
                    SubTotal = userProcedure.Payment.SubTotal,
                    Total = userProcedure.Payment.Total,
                    StatusStr = ConstantHelpers.PAYMENT.STATUS.VALUES[userProcedure.Payment.Status]
                } : null,
                Procedure = new ProcedureViewModel
                {
                    Code = procedure.Code,
                    Description = procedure.Description,
                    Duration = procedure.Duration,
                    LegalBase = procedure.LegalBase,
                    Name = procedure.Name
                },
                UserRequirementFiles = userProcedureFiles.Select(x => new RequirementViewModel
                {
                    Id = x.Id,
                    Name = x.ProcedureRequirement?.Name,
                    Status = x.Status,
                    Code = x.ProcedureRequirement?.Code,
                    Cost = x.ProcedureRequirement?.Cost,
                    FileName = x.FileName,
                    Url = x.Path
                }).ToList()
            };

            if (userProcedure.RecordHistoryId.HasValue)
            {
                var recordHistory = await _recordHistoryService.Get(userProcedure.RecordHistoryId.Value);
                model.RecordHistoryId = recordHistory.Id;
                model.RecordHistoryType = recordHistory.Type;
                model.RecordHistoryFileUrl = recordHistory.FileURL;

                if (recordHistory.StartTermId.HasValue && recordHistory.EndTermId.HasValue)
                {
                    var startTerm = await _termService.Get(recordHistory.StartTermId.Value);
                    var endTerm = await _termService.Get(recordHistory.EndTermId.Value);
                    model.RecordHistoryStartTerm = startTerm.Name;
                    model.RecordHistoryEndTerm = endTerm.Name;
                }

                if (recordHistory.StartAcademicYear.HasValue && recordHistory.EndAcademicYear.HasValue)
                {
                    model.RecordHistoryStartAcademicYear = recordHistory.StartAcademicYear;
                    model.RecordHistoryEndAcademicYear = recordHistory.EndAcademicYear;
                }

                if (recordHistory.RecordTermId.HasValue)
                {
                    var recordTerm = await _termService.Get(recordHistory.RecordTermId.Value);
                    model.RecordHistoryTerm = recordTerm.Name;
                }
            }

            if (userProcedure.StudentUserProcedureId.HasValue)
            {
                var studentUserProcedure = await _studentUserProcedureService.Get(userProcedure.StudentUserProcedureId.Value);

                if (studentUserProcedure != null)
                {
                    model.Activity = new ActivityViewModel
                    {
                        Type = studentUserProcedure.ActivityType,
                    };

                    if (studentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.CAREER_TRANSFER)
                    {
                        var career = await _careerService.Get(studentUserProcedure.CareerId.Value);
                        var curriculum = await _curriculumService.Get(studentUserProcedure.CurriculumId.Value);

                        model.Activity.Career = career.Name;
                        model.Activity.Curriculum = curriculum.Code;

                    }
                    else if (studentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.COURSE_WITHDRAWAL)
                    {
                        var studentSection = await _studentSectionService.Get(studentUserProcedure.StudentSectionId.Value);
                        var section = await _sectionService.Get(studentSection.SectionId);
                        var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
                        var course = await _courseService.GetAsync(courseTerm.CourseId);

                        model.Activity.StudentSection = $"{course.Name} - {section.Code}";
                    }
                    else if (studentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.CHANGE_ACADEMIC_PROGRAM)
                    {
                        var academicProgam = await _academicProgramService.Get(studentUserProcedure.AcademicProgramId.Value);

                        model.Activity.AcademicProgram = academicProgam.Name;
                    }
                    else if (studentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.EXTRAORDINARY_EVALUATION)
                    {
                        var course = await _courseService.GetAsync(studentUserProcedure.CourseId.Value);

                        model.Activity.Course = $"{course.Code} - {course.Name}";
                    }
                    else if (studentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.EXONERATED_COURSE)
                    {
                        var course = await _courseService.GetAsync(studentUserProcedure.CourseId.Value);

                        model.Activity.Course = $"{course.Code} - {course.Name}";
                    }
                    else if (studentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.SUBSTITUTE_EXAM)
                    {
                        var studentSection = await _studentSectionService.Get(studentUserProcedure.StudentSectionId.Value);
                        var section = await _sectionService.Get(studentSection.SectionId);
                        var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
                        var course = await _courseService.GetAsync(courseTerm.CourseId);

                        model.Activity.StudentSection = $"{course.Name} - {section.Code}";
                    }
                    else if (studentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.GRADE_RECOVERY)
                    {
                        var studentSection = await _studentSectionService.Get(studentUserProcedure.StudentSectionId.Value);
                        var section = await _sectionService.Get(studentSection.SectionId);
                        var courseTerm = await _courseTermService.GetAsync(section.CourseTermId);
                        var course = await _courseService.GetAsync(courseTerm.CourseId);

                        model.Activity.StudentSection = $"{course.Name} - {section.Code}";
                    }
                    else if (studentUserProcedure.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.COURSE_WITHDRAWAL_MASSIVE)
                    {
                        model.Activity.StudentSectionsDescription = await _context.StudentUserProcedureDetails.Where(x => x.StudentUserProcedureId == studentUserProcedure.Id)
                            .Select(x => $"{x.StudentSection.Section.CourseTerm.Course.Code} - {x.StudentSection.Section.CourseTerm.Course.Name}").ToListAsync();
                    }
                }
            }

            return View(model);
        }

        [HttpGet("esquela-pago-pdf/{id:Guid}")]
        public async Task<IActionResult> GetPedingPaymentPdf(Guid id)
        {
            var userProcedure = await _userProcedureService.GetUserProcedure(id);
            var procedure = await _procedureService.Get(userProcedure.ProcedureId);
            var task = await _procedureTaskService.GetProcedureTasks(procedure.Id);

            var user = await _userService.Get(userProcedure.UserId);
            var student = await _studentService.GetStudentByUser(user.Id);


            var title = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_PROFORMA_TITLE_TEXT);
            var footer = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_PROFORMA_FOOTER_TEXT);

            var model = new PendingPaymentPdfViewModel
            {
                //ConceptName = userProcedure.Payment.Description,
                //Career = career.Name,
                //Amount = userProcedure.Payment.Total,
                Total = userProcedure.Payment.Total,
                FooterText = footer,
                FullName = user.FullName,
                Img = Path.Combine(_webHostEnvironment.WebRootPath, @"images\themes\" + GeneralHelpers.GetTheme() + "/logo-report.png"),
                Term = userProcedure.Term.Name,
                Title = title,
                UserName = user.UserName,
                Concepts = new List<PendingPaymentPdfConceptViewModel>
                {
                    new PendingPaymentPdfConceptViewModel
                    {
                        ConceptName = userProcedure.Payment.Description,
                        Amount = userProcedure.Payment.Total
                    }
                }
            };

            var automaticTask = task.Where(x => x.Type != ConstantHelpers.PROCEDURE_TASK.TYPE.DESCRIPTION).FirstOrDefault();

            if (automaticTask != null && automaticTask.ActivityType == ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.COURSE_WITHDRAWAL_MASSIVE)
            {
                model.Concepts = await _context.StudentUserProcedureDetails.Where(x => x.StudentUserProcedureId == userProcedure.StudentUserProcedureId)
                    .Select(x => new PendingPaymentPdfConceptViewModel
                    {
                        ConceptName = $"{x.StudentSection.Section.CourseTerm.Course.Code} - {x.StudentSection.Section.CourseTerm.Course.Name}"
                    })
                    .ToListAsync();

                model.Concepts.ForEach(x => x.Amount = Math.Round((model.Total / model.Concepts.Count()), 2, MidpointRounding.AwayFromZero));
            }

            if (student != null)
            {
                var career = await _careerService.Get(student.CareerId);
                model.Career = career.Name;
            }

            var htmlContent = await _viewRenderService.RenderToStringAsync("/Views/Procedure/Pdf/PendingPaymentPdf.cshtml", model);

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = new DinkToPdf.GlobalSettings
                {
                    ColorMode = DinkToPdf.ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = DinkToPdf.PaperKind.A4,
                    Margins = new DinkToPdf.MarginSettings { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                    DocumentTitle = "Acta de notas"
                },
                Objects = {
                    new DinkToPdf.ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = htmlContent,
                        WebSettings = {
                            DefaultEncoding = "utf-8"
                        }
                    }
                }
            };

            var fileByte = _converter.Convert(pdf);

            return File(fileByte, "application/pdf", $"{title}.pdf");
        }

        [HttpPost("eliminar-tramite")]
        public async Task<IActionResult> DeleteProcedure(Guid id)
        {
            var userProcedure = await _userProcedureService.Get(id);
            if (userProcedure == null)
                return BadRequest("No se encontró el trámite seleccionado");

            if (userProcedure.Status == ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED)
                return BadRequest("El trámite se encuentra finalizado.");

            if (userProcedure.PaymentId.HasValue)
            {
                var payment = await _paymentService.Get(userProcedure.PaymentId.Value);
                if (payment.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                {
                    return BadRequest("El trámite cuenta con pago realizado.");
                }

                _context.Payments.Remove(payment);
            }

            if (userProcedure.StudentUserProcedureId.HasValue)
            {
                var studentUserProcedure = await _studentUserProcedureService.Get(userProcedure.StudentUserProcedureId.Value);
                var studentUserProcedureDetails = await _context.StudentUserProcedureDetails.Where(x => x.StudentUserProcedureId == userProcedure.StudentUserProcedureId.Value).ToListAsync();
                _context.StudentUserProcedureDetails.RemoveRange(studentUserProcedureDetails);
                _context.StudentUserProcedures.Remove(studentUserProcedure);
            }

            _context.UserProcedures.Remove(userProcedure);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("get-archivos-usuario-tramite")]
        public async Task<IActionResult> GetUserRequirementFiles(Guid userProcedureId)
        {
            var userProcedureFiles = await _userProcedureFileService.GetUserProcedureFiles(userProcedureId);
            var result = userProcedureFiles.Select(x => new RequirementViewModel
            {
                Id = x.Id,
                Name = x.ProcedureRequirement?.Name,
                Status = x.Status,
                Code = x.ProcedureRequirement?.Code,
                Cost = x.ProcedureRequirement?.Cost,
                FileName = x.FileName,
                Url = x.Path
            }).ToList();

            return Ok(result);
        }

        [HttpPost("archivo-usuario-tramite/actualizar")]
        public async Task<IActionResult> UpdateUserProcedureFile(Guid id, IFormFile file)
        {
            var userProcedureFile = await _userProcedureFileService.Get(id);

            try
            {
                var uploadFilePath = await _cloudStorageService.UploadFile(file.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.USER_PROCEDURE_FILE,
                    Path.GetExtension(file.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.DOCUMENTARY_PROCEDURE);

                userProcedureFile.Status = CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.STATUS.SENT;
                userProcedureFile.FileName = file.FileName;
                userProcedureFile.Path = uploadFilePath;
                userProcedureFile.Size = file.Length;
                await _userProcedureFileService.Update(userProcedureFile);

            }
            catch (Exception)
            {
                return BadRequest($"Hubo un problema al subir el archivo");
            }

            return Ok();
        }
    }
}
