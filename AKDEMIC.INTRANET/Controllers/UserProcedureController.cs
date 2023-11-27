using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.ViewModels.UserProcedureViewModels;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System.Linq;
using System.IO;
using AKDEMIC.CORE.Options;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Extensions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize]
    [Route("tramites/usuarios")]
    public class UserProcedureController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _cloudStorageCredentials;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly IPaymentService _paymentService;
        private readonly IProcedureService _procedureService;
        private readonly IProcedureDependencyService _procedureDependencyService;
        private readonly IProcedureRequirementService _procedureRequirementService;
        private readonly IStudentService _studentService;
        private readonly IUITService _uitService;
        private readonly IUserProcedureService _userProcedureService;
        private readonly IUserProcedureFileService _userProcedureFileService;
        private readonly IConfigurationService _configurationService;
        private readonly ITeacherService _teacherService;
        private readonly IDependencyService _dependencyService;
        private readonly IUserService _userService;
        private readonly IUserProcedureDerivationFileService _userProcedureDerivationFileService;
        private readonly IUserProcedureDerivationService _userProcedureDerivationService;
        private readonly IConceptService _conceptService;
        public UserProcedureController(
            IOptions<CloudStorageCredentials> cloudStorageCredentials,
            UserManager<ApplicationUser> userManager,
            IAcademicHistoryService academicHistoryService,
            IPaymentService paymentService,
            IProcedureService procedureService,
            IProcedureDependencyService procedureDependencyService,
            IProcedureRequirementService procedureRequirementService,
            IStudentService studentService,
            ITermService termService,
            IUITService uitService,
            IUserProcedureService userProcedureService,
            IUserProcedureFileService userProcedureFileService,
            IConfigurationService configurationService,
            ITeacherService teacherService,
            IDependencyService dependencyService,
            IUserService userService,
            IUserProcedureDerivationFileService userProcedureDerivationFileService,
            IUserProcedureDerivationService userProcedureDerivationService,
            IConceptService conceptService
        ) : base(userManager, termService)
        {
            _cloudStorageCredentials = cloudStorageCredentials;
            _academicHistoryService = academicHistoryService;
            _paymentService = paymentService;
            _procedureService = procedureService;
            _procedureDependencyService = procedureDependencyService;
            _procedureRequirementService = procedureRequirementService;
            _studentService = studentService;
            _uitService = uitService;
            _userProcedureService = userProcedureService;
            _userProcedureFileService = userProcedureFileService;
            _configurationService = configurationService;
            _teacherService = teacherService;
            _dependencyService = dependencyService;
            _userService = userService;
            _userProcedureDerivationFileService = userProcedureDerivationFileService;
            _userProcedureDerivationService = userProcedureDerivationService;
            _conceptService = conceptService;
        }



        /// <summary>
        /// Obtiene la vista inicial de tramites del usuario
        /// </summary>
        /// <param name="tab">Tab en el que se encuenta en la vista</param>
        /// <returns>Retorna una vista</returns>
        public async Task<IActionResult> Index(int? tab = null)
        {
            var uit = await _uitService.GetCurrentUIT();
            var userProcedureViewModel = new UserProcedureViewModel
            {
                UITViewModel = (uit != null ? new UITViewModel
                {
                    Value = uit.Value
                } : null),
                UserProcedureStatusValues = ConstantHelpers.USER_PROCEDURES.STATUS.VALUES
            };

            var confi = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.DocumentaryProcedureManagement.TUPA_READONLY));

            ViewBag.Tab = tab;
            ViewBag.InformationTupa = confi;
            return View(userProcedureViewModel);
        }

        /// <summary>
        /// Obtiene los tramites por usuario logeado
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [Route("activos/get")]
        public async Task<IActionResult> GetActiveUserProcedures()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userProcedureService.GetActiveUserProceduresByUser(user.Id, null);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene el historio de los tramites por usuario logeado
        /// </summary>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [Route("historicos/get")]
        public async Task<IActionResult> GetHistoricUserProcedures()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userProcedureService.GetHistoricUserProceduresByUser(user.Id, null);

            return Ok(result);
        }

        /// <summary>
        /// Crea un tramite
        /// </summary>
        /// <param name="model">Modelo que contiene los parametros del tramite</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("crear/post")]
        public async Task<IActionResult> CreateUserProcedure(UserProcedureViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var dateTimeNow = DateTime.Now.ToUniversalTime();
            var procedure = await _procedureService.Get(model.ProcedureId);

            //var procedureDependency = await _procedureDependencyService.FirstProcedureDependencyByProcedure(procedure.Id);
            var term = await _termService.GetTermByDateTime(dateTimeNow);
            var user = await _userManager.GetUserAsync(User);

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
                    var dependencies = await _dependencyService.GetDependenciesByCareer(student.CareerId);

                    if (!dependencies.Any())
                        return BadRequest("La trámite no tiene asignado un área de inicio.");

                    dependencyId = dependencies.Select(x => x.Id).FirstOrDefault();
                }
            }

            var concept = await _conceptService.GetConcept(model.ConceptId.HasValue ? model.ConceptId.Value : Guid.Empty);

            if (model.HasReceipt)
            {
                var datetime = ConvertHelpers.DatepickerToDatetime(model.PaymentReceipt.Datetime);
                if (!await _paymentService.AnyWithDateOperationAndTotal(datetime.Date, model.PaymentReceipt.Sequence, model.PaymentReceipt.Amount))
                    return BadRequest($"No se encuentra los datos de su recibo '{model.PaymentReceipt.Datetime}'  -  N° '{model.PaymentReceipt.Sequence}'  -  S/. '{model.PaymentReceipt.Amount}'");

                if (concept != null)
                {
                    if (model.PaymentReceipt.Amount < concept.Amount)
                        return BadRequest($"No se encuentra los datos de su recibo '{model.PaymentReceipt.Datetime}'  -  N° '{model.PaymentReceipt.Sequence}'  -  S/. '{model.PaymentReceipt.Amount}'");
                }
            }

            switch (procedure.StaticType)
            {
                case ConstantHelpers.PROCEDURES.STATIC_TYPE.IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_CREDITS:
                case ConstantHelpers.PROCEDURES.STATIC_TYPE.IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_WITHOUT_CURRUCULUM_CHANGE:
                case ConstantHelpers.PROCEDURES.STATIC_TYPE.IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_WITH_CURRUCULUM_CHANGE:
                case ConstantHelpers.PROCEDURES.STATIC_TYPE.REGULAR_EXTEMPORANEOUS_ENROLLMENT:
                    if (term.ComplementaryEnrollmentStartDate < dateTimeNow || term.ComplementaryEnrollmentEndDate > dateTimeNow)
                    {
                        return BadRequest("Se encuentra fuera del rango de matrícula");
                    }

                    break;
                case ConstantHelpers.PROCEDURES.STATIC_TYPE.EXTRAORDINARY_EVALUATION:
                    var student = await _studentService.GetStudentByUser(user.Id);

                    if (student != null)
                    {
                        var failedAcademicHistoriesCount = await _academicHistoryService.CountFailedAcademicHistoriesByStudent(student.Id);

                        if (failedAcademicHistoriesCount > 2)
                        {
                            return BadRequest("Posee más de dos asignaturas desaprobadas");
                        }
                    }

                    break;
                default:
                    break;
            }

            var userProcedure = new UserProcedure
            {
                DependencyId = dependencyId,
                ProcedureId = model.ProcedureId,
                TermId = term?.Id,
                UserId = user.Id,
                Comment = model.Comment
            };

            var cloudStorageService = new CloudStorageService(_cloudStorageCredentials);

            if (model.HasPicture)
            {

                if (model.urlCropImg != null)
                {
                    var imgArray1 = model.urlCropImg.Split(";");
                    var imgArray2 = imgArray1[1].Split(",");

                    var newImage = Convert.FromBase64String(imgArray2[1]);
                    var rnd = new Random();
                    var imageName = rnd.Next(10000).ToString() + ".png";

                    using (var stream = new MemoryStream(newImage))
                    {
                        if (!string.IsNullOrEmpty(userProcedure.UrlImage))
                            await cloudStorageService.TryDelete(userProcedure.UrlImage.Split('/').Last(), "tramite");

                        userProcedure.UrlImage = await cloudStorageService.UploadFile(stream, "tramite",
                            System.IO.Path.GetExtension(imageName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.DOCUMENTARY_PROCEDURE);
                    }
                }
            }

            var procedureRequirementsCost = await _procedureRequirementService.GetProcedureRequirementsCostSumByProcedure(model.ProcedureId);
            var igvAmount = procedureRequirementsCost * ConstantHelpers.PAYMENT.IGV;

            var userProcedureExist = await _userProcedureService.Get(model.Id);
            if (userProcedureExist == null)
            {
                await _userProcedureService.Insert(userProcedure);
            }
            else
            {
                userProcedureExist.DependencyId = userProcedure.DependencyId;
                userProcedureExist.ProcedureId = userProcedure.ProcedureId;
                userProcedureExist.TermId = userProcedure.TermId;
                userProcedureExist.UserId = userProcedure.UserId;
                userProcedureExist.Comment = userProcedure.Comment;
                userProcedureExist.UrlImage = userProcedure.UrlImage;
                await _userProcedureService.Update(userProcedure);
            }

            if (model.HasReceipt)
            {
                var datetime = ConvertHelpers.DatepickerToDatetime(model.PaymentReceipt.Datetime);
                var payment = await _paymentService.GetByDateOperationAndTotal(datetime, model.PaymentReceipt.Sequence, model.PaymentReceipt.Amount);
                payment.EntityId = userProcedureExist != null ? userProcedureExist.Id : userProcedure.Id;
                payment.UserId = user.Id;
                payment.Type = ConstantHelpers.PAYMENT.TYPES.PROCEDURE;
                payment.ConceptId = procedure.ConceptId;


                await _paymentService.Update(payment);

                userProcedure.Status = ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED;
                userProcedure.PaymentId = payment.Id;
            }
            else
            {
                if (procedureRequirementsCost != 0)
                {
                    decimal total;
                    if (concept.IsTaxed)
                        total = concept.Amount - igvAmount;
                    else
                        total = concept.Amount;

                    var payment = new Payment
                    {
                        EntityId = userProcedure.Id,
                        UserId = user.Id,
                        ConceptId = procedure.ConceptId,
                        Description = procedure.Name,
                        IgvAmount = igvAmount,
                        SubTotal = total,
                        Total = total,
                        Type = ConstantHelpers.PAYMENT.TYPES.PROCEDURE
                    };

                    await _paymentService.Insert(payment);
                    userProcedure.Status = ConstantHelpers.USER_PROCEDURES.STATUS.PENDING_PAYMENT;

                    userProcedure.PaymentId = payment.Id;
                }
                else
                {
                    userProcedure.Status = ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED;
                }
            }

            if (userProcedureExist == null)
            {
                await _userProcedureService.Update(userProcedure);
            }
            else
            {
                userProcedureExist.PaymentId = userProcedure.PaymentId != null ? userProcedure.PaymentId : Guid.Empty;
                userProcedureExist.Status = userProcedure.Status;
                await _userProcedureService.Update(userProcedure);
            }



            var documentFiles = model.DocumentFiles;

            if (documentFiles != null)
            {
                foreach (var documentFile in documentFiles)
                {
                    var fileName = documentFile.FileName;

                    if (documentFile.Length > ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.TEXT.GENERIC)
                    {
                        return BadRequest($"El tamaño del archivo '{fileName}' excede el límite de {ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.TEXT.GENERIC / 1024 / 1024}MB");
                    }

                    if (!documentFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.GENERIC))
                    {
                        return BadRequest($"El contenido del archivo '{fileName}' es inválido");
                    }
                }

                var createUserProcedureFiles = new List<UserProcedureFile>();

                foreach (var documentFile in documentFiles)
                {
                    try
                    {
                        var uploadFilePath = await cloudStorageService.UploadFile(documentFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.USER_PROCEDURE_FILE,
                            Path.GetExtension(documentFile.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.DOCUMENTARY_PROCEDURE);

                        createUserProcedureFiles.Add(new UserProcedureFile
                        {
                            UserProcedureId = userProcedure.Id,
                            FileName = documentFile.FileName,
                            Path = uploadFilePath,
                            Size = documentFile.Length,
                            Status = CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.STATUS.SENT
                        });
                    }
                    catch (Exception)
                    {
                        return BadRequest($"Hubo un problema al subir el archivo '{documentFile.FileName}'");
                    }
                }

                await _userProcedureFileService.InsertRange(createUserProcedureFiles);
            }

            return Ok();
        }

        /// <summary>
        /// Obtiene el detalle del tramite solicitado
        /// </summary>
        /// <param name="id">Identificador del tramite</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("tramite-solicitado/{id}")]
        public async Task<IActionResult> UserProcedureRequested(Guid id)
        {
            var userProcedure = await _userProcedureService.GetUserProcedure(id);

            var user = await _userManager.GetUserAsync(User);
            var procedureRequeriment = await _procedureRequirementService.GetProcedureRequirementsByProcedure(userProcedure.Procedure.Id);
            var procedureDependency = await _procedureDependencyService.GetProcedureDependenciesByProcedure(userProcedure.Procedure.Id);
            var uit = await _uitService.GetCurrentUIT();

            var result = new UserProcedureViewModel()
            {
                Id = userProcedure.Id,
                ProcedureId = userProcedure.Procedure.Id,
                HasPicture = userProcedure.Procedure.HasPicture,
                UserId = user.Id,
                ConceptId = userProcedure.Procedure.ConceptId,
                UITViewModel = (uit != null ? new UITViewModel
                {
                    Value = uit.Value
                } : null),
                UserProcedureStatusValues = ConstantHelpers.USER_PROCEDURES.STATUS.VALUES,
                NameProcedure = userProcedure.Procedure.Name,
                Cost = userProcedure.Procedure.ProcedureRequirementsCostSum,
                ProcedureRequirement = procedureRequeriment.Select(x => new ProcedureRequirementViewModel
                {
                    Code = x.Code,
                    Cost = x.Cost,
                    Id = x.Id,
                    HasUserProcedureRecordRequirement = x.HasUserProcedureRecordRequirement,
                    Name = x.Name
                }).ToList(),
                CostReq = procedureRequeriment.Sum(x => x.Cost),
                Duration = userProcedure.Procedure.Duration,
                Dependencies = procedureDependency.Select(x => new DependencyViewModel
                {
                    Id = x.Id,
                    Name = x.Dependency.Name
                }).ToList()
            };

            return View(result);
        }

        /// <summary>
        /// Actualiza los datos del tramite solicitado
        /// </summary>
        /// <param name="model">Modelo con los parametros del tramite</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("tramite-solicitado/actualizar-datos")]
        public async Task<IActionResult> UpdateUserProcedure(UserProcedureViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var userProcedure = await _userProcedureService.Get(model.Id);
            var procedure = await _procedureService.Get(model.ProcedureId);
            var procedureDependency = await _procedureDependencyService.FirstProcedureDependencyByProcedure(procedure.Id);
            var dateTimeNow = DateTime.Now.ToUniversalTime();
            var term = await _termService.GetActiveTerm();
            var user = await _userManager.GetUserAsync(User);

            switch (procedure.StaticType)
            {
                case ConstantHelpers.PROCEDURES.STATIC_TYPE.IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_CREDITS:
                case ConstantHelpers.PROCEDURES.STATIC_TYPE.IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_WITHOUT_CURRUCULUM_CHANGE:
                case ConstantHelpers.PROCEDURES.STATIC_TYPE.IRREGULAR_EXTEMPORANEOUS_ENROLLMENT_WITH_CURRUCULUM_CHANGE:
                case ConstantHelpers.PROCEDURES.STATIC_TYPE.REGULAR_EXTEMPORANEOUS_ENROLLMENT:
                    if (term.ComplementaryEnrollmentStartDate < dateTimeNow || term.ComplementaryEnrollmentEndDate > dateTimeNow)
                    {
                        return BadRequest("Se encuentra fuera del rango de matrícula");
                    }

                    break;
                case ConstantHelpers.PROCEDURES.STATIC_TYPE.EXTRAORDINARY_EVALUATION:
                    var student = await _studentService.GetStudentByUser(user.Id);

                    if (student != null)
                    {
                        var failedAcademicHistoriesCount = await _academicHistoryService.CountFailedAcademicHistoriesByStudent(student.Id);

                        if (failedAcademicHistoriesCount > 2)
                        {
                            return BadRequest("Posee más de dos asignaturas desaprobadas");
                        }
                    }

                    break;
                default:
                    break;
            }

            userProcedure.DependencyId = procedureDependency?.DependencyId;
            userProcedure.TermId = term?.Id;
            userProcedure.Comment = model.Comment;
            if (userProcedure.Status == ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED)
                userProcedure.Status = ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS;

            var cloudStorageService = new CloudStorageService(_cloudStorageCredentials);

            if (model.HasPicture)
            {

                if (model.urlCropImg != null)
                {
                    var imgArray1 = model.urlCropImg.Split(";");
                    var imgArray2 = imgArray1[1].Split(",");

                    var newImage = Convert.FromBase64String(imgArray2[1]);
                    var rnd = new Random();
                    var imageName = rnd.Next(10000).ToString() + ".png";

                    using (var stream = new MemoryStream(newImage))
                    {
                        if (!string.IsNullOrEmpty(userProcedure.UrlImage))
                            await cloudStorageService.TryDelete(userProcedure.UrlImage.Split('/').Last(), "tramite");

                        userProcedure.UrlImage = await cloudStorageService.UploadFile(stream, "tramite",
                            System.IO.Path.GetExtension(imageName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.DOCUMENTARY_PROCEDURE);
                    }
                }
            }

            await _userProcedureService.Update(userProcedure);

            var documentFiles = model.DocumentFiles;
            if (documentFiles != null)
            {
                //Se valida que cumplan con los estandares del sistema
                foreach (var documentFile in documentFiles)
                {
                    var fileName = documentFile.FileName;
                    if (documentFile.Length > ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.TEXT.GENERIC)
                        return BadRequest($"El tamaño del archivo '{fileName}' excede el límite de {ConstantHelpers.DOCUMENTS.FILE_SIZE_BYTES.TEXT.GENERIC / 1024 / 1024}MB");

                    if (!documentFile.HasContentType(ConstantHelpers.DOCUMENTS.MIME_TYPE.APPLICATION.GENERIC))
                        return BadRequest($"El contenido del archivo '{fileName}' es inválido");
                }

                var userProcedureFiles = new List<UserProcedureFile>();

                foreach (var documentFile in documentFiles)
                {
                    try
                    {
                        var uploadFilePath = await cloudStorageService.UploadFile(documentFile.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.USER_PROCEDURE_FILE,
                            Path.GetExtension(documentFile.FileName), CORE.Helpers.ConstantHelpers.FileStorage.SystemFolder.DOCUMENTARY_PROCEDURE);

                        userProcedureFiles.Add(new UserProcedureFile
                        {
                            UserProcedureId = userProcedure.Id,
                            FileName = documentFile.FileName,
                            Path = uploadFilePath,
                            Size = documentFile.Length,
                            Status = CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.STATUS.SENT
                        });
                    }
                    catch (Exception)
                    {
                        return BadRequest($"Hubo un problema al subir el archivo '{documentFile.FileName}'");
                    }
                }

                await _userProcedureFileService.InsertRange(userProcedureFiles);
            }

            return Ok();
        }

        /// <summary>
        /// Obtiene los datos para solicitar un tramite
        /// </summary>
        /// <param name="id">Identificador del tramite</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("solicitar-tramite/{id}")]
        public async Task<IActionResult> RequestUserProcedure(Guid id)
        {
            var procedure = await _procedureService.GetProcedure(id);
            var user = await _userManager.GetUserAsync(User);
            var procedureRequeriment = await _procedureRequirementService.GetProcedureRequirementsByProcedure(procedure.Id);
            var procedureDependency = await _procedureDependencyService.GetProcedureDependenciesByProcedure(procedure.Id);
            var uit = await _uitService.GetCurrentUIT();
            var student = await _studentService.GetStudentByUser(user.Id);

            var hasReceipt = bool.Parse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.DocumentaryProcedureManagement.PAYMENT_CONFIGURATION));
            var result = new UserProcedureViewModel()
            {
                ProcedureId = procedure.Id,
                HasPicture = procedure.HasPicture,
                HasReceipt = hasReceipt,
                UserId = user.Id,
                ConceptId = procedure.ConceptId,
                StartDependency = "",
                UITViewModel = (uit != null ? new UITViewModel
                {
                    Value = uit.Value
                } : null),
                UserProcedureStatusValues = ConstantHelpers.USER_PROCEDURES.STATUS.VALUES,
                NameProcedure = procedure.Name,
                Cost = procedure.ProcedureRequirementsCostSum,
                ProcedureRequirement = procedureRequeriment.Select(x => new ProcedureRequirementViewModel
                {
                    Code = x.Code,
                    Cost = x.Cost,
                    Id = x.Id,
                    HasUserProcedureRecordRequirement = x.HasUserProcedureRecordRequirement,
                    Name = x.Name
                }).ToList(),
                CostReq = procedureRequeriment.Sum(x => x.Cost),
                Duration = procedure.Duration,
                Dependencies = procedureDependency.Select(x => new DependencyViewModel
                {
                    Id = x.Id,
                    Name = x.Dependency.Name
                }).ToList()
            };

            if (procedure.StartDependencyId.HasValue)
            {
                var dependency = await _dependencyService.Get(procedure.StartDependencyId.Value);
                result.StartDependency = dependency?.Name;
            }
            else
            {
                if (student != null)
                {
                    var dependency = await _dependencyService.GetDependenciesByCareer(student.CareerId);
                    result.StartDependency = dependency.Select(x => x.Name).FirstOrDefault();
                }
            }

            var confi = bool.Parse(await _configurationService.GetValueByKey(ConstantHelpers.Configuration.DocumentaryProcedureManagement.TUPA_READONLY));

            ViewBag.InformationTupa = confi;
            return View(result);
        }

        /// <summary>
        /// Obtiene la vista con los datos del tramite a continuar
        /// </summary>
        /// <param name="id">Identificador del tramite</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("continuar-tramite/{id}")]
        public async Task<IActionResult> ContinueUserProcedure(Guid id)
        {
            var userProcedure = await _userProcedureService.GetUserProcedure(id);

            var user = await _userManager.GetUserAsync(User);
            var procedureRequeriment = await _procedureRequirementService.GetProcedureRequirementsByProcedure(userProcedure.Procedure.Id);
            var procedureDependency = await _procedureDependencyService.GetProcedureDependenciesByProcedure(userProcedure.Procedure.Id);
            var uit = await _uitService.GetCurrentUIT();

            var hasReceipt = bool.Parse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.DocumentaryProcedureManagement.PAYMENT_CONFIGURATION));

            var result = new UserProcedureViewModel()
            {
                ProcedureId = userProcedure.Procedure.Id,
                Id = userProcedure.Id,
                HasPicture = userProcedure.Procedure.HasPicture,
                HasReceipt = hasReceipt,
                UserId = user.Id,
                ConceptId = userProcedure.Procedure.ConceptId,
                UITViewModel = (uit != null ? new UITViewModel
                {
                    Value = uit.Value
                } : null),
                UserProcedureStatusValues = ConstantHelpers.USER_PROCEDURES.STATUS.VALUES,
                NameProcedure = userProcedure.Procedure.Name,
                Cost = userProcedure.Procedure.ProcedureRequirementsCostSum,
                ProcedureRequirement = procedureRequeriment.Select(x => new ProcedureRequirementViewModel
                {
                    Code = x.Code,
                    Cost = x.Cost,
                    Id = x.Id,
                    HasUserProcedureRecordRequirement = x.HasUserProcedureRecordRequirement,
                    Name = x.Name
                }).ToList(),
                CostReq = procedureRequeriment.Sum(x => x.Cost),
                Duration = userProcedure.Procedure.Duration,
                Dependencies = procedureDependency.Select(x => new DependencyViewModel
                {
                    Id = x.Id,
                    Name = x.Dependency.Name
                }).ToList()
            };

            return View(result);
        }

        /// <summary>
        /// Valida el recibo del pago del tramite
        /// </summary>
        /// <param name="model">Modelo que contiene el tramite del usuario</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("validar-recibo")]
        public async Task<IActionResult> ValidateReceipt(UserProcedureViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var datetime = ConvertHelpers.DatepickerToDatetime(model.PaymentReceipt.Datetime);
            if (!await _paymentService.AnyWithDateOperationAndTotal(datetime.Date, model.PaymentReceipt.Sequence, model.PaymentReceipt.Amount))
                return BadRequest($"No se encuentra los datos de su recibo '{model.PaymentReceipt.Datetime}'  -  N° '{model.PaymentReceipt.Sequence}'  -  S/. '{model.PaymentReceipt.Amount}'");

            var concept = await _conceptService.GetConcept(model.ConceptId.HasValue ? model.ConceptId.Value : Guid.Empty);



            if (model.HasReceipt)
            {
                if (concept != null)
                {
                    if (model.PaymentReceipt.Amount < concept.Amount)
                        return BadRequest($"El monto del concepto es mayor al del recibo.");
                }
            }

            return Ok();
        }

        /// <summary>
        /// Obtiene el detalle para el tramite del usuario
        /// </summary>
        /// <param name="id">Identificador del tramite del usuario</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("detalle-tramite/{id}")]
        public async Task<IActionResult> DetailUserProcedure(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var userProcedure = await _userProcedureService.GetUserProcedure(id);
            var student = await _studentService.GetStudentByUser(userProcedure.User.Id);
            var teacher = await _teacherService.GetByUserId(userProcedure.User.Id);
            var administrative = await _userService.GetAdminitrativeByUserId(userProcedure.User.Id);

            var hasReceipt = bool.Parse(await _configurationService.GetValueByKey(CORE.Helpers.ConstantHelpers.Configuration.DocumentaryProcedureManagement.PAYMENT_CONFIGURATION));

            var firstProcedureDependency = await _procedureDependencyService.FirstProcedureDependencyByProcedure(userProcedure.ProcedureId);
            var lastProcedureDependency = await _procedureDependencyService.LastProcedureDependencyByProcedure(userProcedure.ProcedureId);

            var userProcedureViewModel = new UserProcedureDetailViewModel
            {
                Id = userProcedure.Id,
                DependencyId = userProcedure.DependencyId,
                PaymentId = userProcedure.PaymentId,
                ProcedureId = userProcedure.ProcedureId,
                TermId = userProcedure.TermId,
                UserId = userProcedure.UserId,
                GeneratedId = userProcedure.GeneratedId,
                DNI = userProcedure.DNI,
                HasPicture = userProcedure.Procedure.HasPicture,
                Status = userProcedure.Status,
                Comment = userProcedure.Comment,
                UrlImage = userProcedure.UrlImage,
                Observation = userProcedure.ObservationStatus,
                HasReceipt = hasReceipt,
                DependencyViewModel = (userProcedure.Dependency != null ? new DependencyDetailViewModel
                {
                    Name = userProcedure.Dependency.Name
                } : null),
                PaymentViewModel = userProcedure.Payment != null ? new PaymentViewModel
                {
                    OperationCodeB = userProcedure.Payment.OperationCodeB,
                    PayDateTime = userProcedure.Payment.PaymentDate.HasValue ? userProcedure.Payment.PaymentDate.Value.ToLocalDateFormat() : "",
                    Discount = userProcedure.Payment.Discount,
                    IgvAmount = userProcedure.Payment.IgvAmount,
                    Quantity = userProcedure.Payment.Quantity,
                    SubTotal = userProcedure.Payment.SubTotal,
                    Total = userProcedure.Payment.Total
                } : null,
                ProcedureViewModel = (userProcedure.Procedure != null ? new ProcedureViewModel
                {
                    Name = userProcedure.Procedure.Name
                } : null),
                UserViewModel = (userProcedure.User != null ? new UserViewModel
                {
                    FullNameCode = $"{userProcedure.User.UserName} - {userProcedure.User.FullName}",
                    FullName = userProcedure.User.FullName,
                    Type = student != null ? 1 : (teacher != null ? 2 : (administrative != null ? 3 : 0)),
                    Dni = userProcedure.User.Dni,
                    Email = userProcedure.User.Email,
                    Phone = userProcedure.User.PhoneNumber,
                    Username = userProcedure.User.UserName
                } : null),
            };

            if (student != null)
            {
                userProcedureViewModel.UserViewModel.Faculty = student.Career.Faculty.Name;
                userProcedureViewModel.UserViewModel.Career = student.Career.Name;
            }

            if (teacher != null)
                userProcedureViewModel.UserViewModel.DepartmentAcademic = teacher.AcademicDepartment.Name;

            if (administrative != null)
            {
                var model = administrative.UserDependencies.Where(x => x.UserId == userProcedure.User.Id).ToList();
                var list = "";
                foreach (var item in model)
                {
                    if (model.IndexOf(item) == model.Count - 1)
                        list = list + item.Dependency.Name;
                    else
                        list = list + item.Dependency.Name + ", ";
                }
                userProcedureViewModel.UserViewModel.Dependency = list;
            }

            return View(userProcedureViewModel);
        }

        /// <summary>
        /// Obtiene una lista de los archivos de tramite del usuario
        /// </summary>
        /// <param name="id">Identificador del tramite del usuario</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("get-observation-file/{id}")]
        public async Task<IActionResult> GetObservationFile(Guid id)
        {
            var result = await _userProcedureService.GetRequerimentProcedureListPt2(id, CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.STATUS.RESOLVED);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene una lista de los archivos de tramite del usuario derivados
        /// </summary>
        /// <param name="upid">Identificador del tramite del usuario</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("archivos/{upid}/get")]
        public async Task<IActionResult> GetUserProcedureDerivationFilesByUserProcedure(Guid upid)
        {
            var result = await _userProcedureDerivationFileService.GetUserProcedureDerivationFilesByUserProcedurept2(upid);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene una lista de los archivos de tramite del usuario finalizados
        /// </summary>
        /// <param name="upid">Identificador del tramite del usuario</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("finalizado/{upid}/get")]
        public async Task<IActionResult> GetUserProcedureByUserProcedureFinish(Guid upid)
        {
            var result = await _userProcedureService.GetFinishByUserProcedure(upid);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene la cantidad de archivos finalizados y derivados por tramite del usuario
        /// </summary>
        /// <param name="upid">Identificador del tramite del usuario</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("derivaciones/todos/{upid}/get")]
        public async Task<IActionResult> GetUserProcedureByUserProcedureAll(Guid upid)
        {
            var resultIsFinishedList = await _userProcedureService.GetFinishByUserProcedure(upid);
            var resultAllDerivations = await _userProcedureDerivationService.GetUserProcedureDerivationsByUserProcedure(upid);

            return Ok(new { itsFinished = resultIsFinishedList.Count() > 0, derivationFinishedList = resultIsFinishedList, derivationList = resultAllDerivations });
        }
    }
}
