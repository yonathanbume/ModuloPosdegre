using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.SurveyViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.INTRANET.Hubs;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ConstantHelpers = AKDEMIC.CORE.Helpers.ConstantHelpers;
using ConvertHelpers = AKDEMIC.CORE.Helpers.ConvertHelpers;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," + ConstantHelpers.ROLES.INTRANET_ADMIN)]
    [Area("Admin")]
    [Route("admin/gestion-encuestas")]
    public class SurveyManagementController : BaseController
    {
        private readonly IConfigurationService _configurationService;
        private readonly ISurveyService _surveyService;
        private readonly ISurveyUserService _surveyUserService;
        private readonly ISurveyItemService _surveyItemService;
        private readonly IAnswerByUserService _answerByUserService;
        private readonly IAnswerService _answerService;
        private readonly IQuestionService _questionService;

        public SurveyManagementController(
            IConfigurationService configurationService,
            ISurveyService surveyService,
            ISurveyUserService surveyUserService,
            ISurveyItemService surveyItemService,
            IAnswerByUserService answerByUserService,
            IAnswerService answerService,
            IQuestionService questionService,
            IDataTablesService dataTablesService,
            IConfiguration configuration,
            IUserService userService,
            IHubContext<AkdemicHub> hubContext
        ) : base(configuration, hubContext, dataTablesService, userService)
        {
            _surveyService = surveyService;
            _surveyUserService = surveyUserService;
            _surveyItemService = surveyItemService;
            _answerByUserService = answerByUserService;
            _questionService = questionService;
            _answerService = answerService;
            _configurationService = configurationService;
        }

        /// <summary>
        /// Obtiene la vista inicial de las encuestas generales
        /// </summary>
        /// <returns>Retorna una vista</returns>
        public async Task<IActionResult> Index()
        {
            var configurationSurvey = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.IntranetManagement.SURVEY_ENFORCE_REQUIRED);
            bool requiredEnabled = false;
            if (configurationSurvey != null)
            {
                requiredEnabled = bool.Parse(configurationSurvey.Value);
            }
            ViewBag.requiredEnabled = requiredEnabled;
            return View();
        }

        /// <summary>
        /// Obtiene todas las encuestas generales creadas en el sistema de Intranet
        /// </summary>
        /// <param name="searchValue">Texto de busqueda</param>
        /// <returns>Retorna el objeto que contiene la estructura de la tabla</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetSurvies(string searchValue)
        {

            var sentParameters = _dataTablesService.GetSentParameters();
            var system = ConstantHelpers.Solution.Intranet;
            var result = await _surveyService.GetIntranetGeneralSurveysDatatable(sentParameters, system, null, null, null, searchValue);

            return Ok(result);
        }

        #region Survey 
        /// <summary>
        /// Crea una nueva encuesta general para el sistema de intranet
        /// </summary>
        /// <param name="model">Modelo que contiene los parametros para crear la nueva encuesta</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("registrar/post")]
        public async Task<IActionResult> AddSurvey(SurveyViewModel model)
        {
            var type = ConstantHelpers.TYPE_SURVEY.GENERAL;
            var system = ConstantHelpers.Solution.Intranet;

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _surveyService.ExistSurveyCode(type, system, model.Code)) return BadRequest("El código de encuesta ya existe");
            if (await _surveyService.AnySurveyByName(type, system, model.Name)) return BadRequest("Ya existe una encuesta con el mismo nombre.");

            var survey = new Survey
            {
                Name = model.Name,
                Description = model.Description,
                Code = model.Code,
                CreatedDate = DateTime.UtcNow,
                Type = (byte)type,
                IsRequired = model.IsRequired,
                IsAnonymous = model.IsAnonymous,
                System = system,
                State = ConstantHelpers.SURVEY_STATES.NOTSENT,
                PublicationDate = (ConvertHelpers.DatepickerToDatetime(model.PublicationDate)).AddHours(5),
                FinishDate = (ConvertHelpers.DatepickerToDatetime(model.FinishDate)).AddHours(5)
            };
            await _surveyService.Insert(survey);
            return Ok();
        }

        /// <summary>
        /// Edita solo las fechas de una encuesta
        /// </summary>
        /// <param name="model">Modelo que contiene las fechas a editar</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("editar/cambiarfechas")]
        public async Task<IActionResult> EditDateSurvey(SurveyEditSendedViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var survey = await _surveyService.Get(model.Id);

            var startDate = ConvertHelpers.DatepickerToUtcDateTime(model.PublicationDate);
            var endDate = ConvertHelpers.DatepickerToUtcDateTime(model.FinishDate);

            if (startDate > endDate)
                return BadRequest("La fecha de inicio de la encuesta no puede ser mayor a la fecha de fin");

            survey.PublicationDate = (ConvertHelpers.DatepickerToDatetime(model.PublicationDate)).AddHours(5);
            survey.FinishDate = (ConvertHelpers.DatepickerToDatetime(model.FinishDate)).AddHours(5);

            await _surveyService.Update(survey);
            return Ok();
        }

        /// <summary>
        /// Edita la mayoria de los campos de una encuesta general
        /// </summary>
        /// <param name="model">Modelo que contiene todos los campos a editar de una encuesta</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("editar/post")]
        public async Task<IActionResult> EditSurvey(SurveyEditViewModel model)
        {
            var type = ConstantHelpers.TYPE_SURVEY.GENERAL;
            var system = ConstantHelpers.Solution.Intranet;

            var configurationSurvey = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.IntranetManagement.SURVEY_ENFORCE_REQUIRED);
            bool requiredEnabled = false;
            if (configurationSurvey != null)
            {
                requiredEnabled = bool.Parse(configurationSurvey.Value);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var survey = await _surveyService.Get(model.Id);

            if (survey.State != ConstantHelpers.SURVEY_STATES.NOTSENT)
            {
                return BadRequest("La encuesta no puede ser editada.");
            }

            if (model.Name.ToUpper() != survey.Name.ToUpper())
            {
                if (await _surveyService.AnySurveyByName(type, system, model.Name)) return BadRequest("Ya existe una encuesta con el mismo nombre.");
            }
            var startDate = ConvertHelpers.DatepickerToUtcDateTime(model.PublicationDate);
            var endDate = ConvertHelpers.DatepickerToUtcDateTime(model.FinishDate);

            if (startDate > endDate)
                return BadRequest("La fecha de inicio de la encuesta no puede ser mayor a la fecha de fin");

            survey.Name = model.Name;
            survey.Description = model.Description;

            //en la vista esta bloqueado la edicion del codigo
            //survey.Code = model.Code;

            if (requiredEnabled)
            {
                survey.IsRequired = model.IsRequired;
            }

            survey.IsAnonymous = model.IsAnonymous;

            survey.PublicationDate = (ConvertHelpers.DatepickerToDatetime(model.PublicationDate)).AddHours(5);
            survey.FinishDate = (ConvertHelpers.DatepickerToDatetime(model.FinishDate)).AddHours(5);

            await _surveyService.Update(survey);
            return Ok();
        }

        /// <summary>
        /// Elimina una encuesta en especifico
        /// </summary>
        /// <param name="id">Identificador de la encuesta</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _surveyService.DeleteById(id);
            return Ok();
        }

        /// <summary>
        /// Obtiene la vista para editar una encuesta
        /// </summary>
        /// <param name="id">Identificador de la encuesta</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            ViewBag.SurveyId = id;
            bool sended = false;

            var survey = await _surveyService.Get(id);
            if (survey.State != ConstantHelpers.SURVEY_STATES.NOTSENT)
                sended = true;

            var configurationSurvey = await _configurationService.FirstOrDefaultByKey(ConstantHelpers.Configuration.IntranetManagement.SURVEY_ENFORCE_REQUIRED);
            bool requiredEnabled = false;
            if (configurationSurvey != null)
            {
                requiredEnabled = bool.Parse(configurationSurvey.Value);
            }

            ViewBag.RequiredEnabled = requiredEnabled;
            ViewBag.Sended = sended;
            return View();
        }

        /// <summary>
        /// Obtiene la informacion de una encuesta
        /// </summary>
        /// <param name="id">Identifiacor de una encuesta</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetSurvey(Guid id)
        {
            var survey = await _surveyService.GetSurveyDetail(id);
            return Ok(survey);
        }

        /// <summary>
        /// Valida si se puede enviar la encuesta, esto valida que tenga una pregunta como minimo
        /// </summary>
        /// <param name="id">Identificador de la encuesta</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("general/enviar/validate")]
        public async Task<IActionResult> ValidateSendSurvey(Guid id)
        {
            var isValid = await _surveyService.ValidateSurvey(id);
            if (isValid)
                return Ok();
            else
                return BadRequest();
        }

        /// <summary>
        /// Crea la vista inicial para enviar la encuesta
        /// </summary>
        /// <param name="id">Identificador de la encuesta</param>
        /// <returns>Retorna una vsita</returns>
        [HttpGet("enviar/{id}")]
        public IActionResult Send(Guid id)
        {
            ViewBag.SurveyId = id;
            return View();
        }
        #endregion

        /// <summary>
        /// Obtiene los usuarios que han respondido una encuesta
        /// </summary>
        /// <param name="surveyId">Identificador de una encuesta</param>
        /// <returns>Retorna el objeto que contiene la estructura de la tabla</returns>
        [HttpGet("getsurveyusers/{surveyId}")]
        public async Task<IActionResult> GetSurveyUsers(Guid surveyId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _answerByUserService.AnsweredSurveyUserDatatable(sentParameters, surveyId);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene una lista de usuarios para enviar la encuesta segun los filtros
        /// </summary>
        /// <param name="rol">Rol del usuario</param>
        /// <param name="onlyEnrolled">Solo Matriculados en caso de alumnos</param>
        /// <param name="careerId">Identificador de escuela en caso de alumnos</param>
        /// <param name="facultyId">Identificador de facultad en caso de alumnos</param>
        /// <param name="specialtyId">Identificador de especialidad en caso de alumnos</param>
        /// <param name="academicDepartmentId">Identificador de departamento academico en caso de docentes</param>
        /// <returns>Retorna el objeto que contiene la estructura de la tabla</returns>
        [HttpGet("getusers")]
        public async Task<IActionResult> GetUsers(string rol, string academicYears, bool onlyEnrolled = false, Guid? careerId = null, Guid? facultyId = null, Guid? specialtyId = null, Guid? academicDepartmentId = null)
        {

            var academicYearSelected = JsonConvert.DeserializeObject<List<int>>(academicYears);

            if (rol == "0")
                rol = "";
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _userService.GetSurveyIntranetUsersDatatable(sentParameters, rol, academicYearSelected, onlyEnrolled, careerId, facultyId, specialtyId, academicDepartmentId);

            return Ok(result);
        }

        /// <summary>
        /// Cuenta la cantidad de usuarios a los que se enviara la encuesta, si esta supera las 1000 lo enviara como un "job"
        /// </summary>
        /// <param name="rol">Rol del usuario</param>
        /// <param name="onlyEnrolled">Solo Matriculados en caso de alumnos</param>
        /// <param name="careerId">Identificador de escuela en caso de alumnos</param>
        /// <param name="facultyId">Identificador de facultad en caso de alumnos</param>
        /// <param name="specialtyId">Identificador de especialidad en caso de alumnos</param>
        /// <param name="academicDepartmentId">Identificador de departamento academico en caso de docentes</param>
        /// <returns>Retorna la cantidad de usuarios a los que se enviaria la encuesta</returns>
        [HttpPost("enviar/validar")]
        public async Task<IActionResult> SendValidate(string rol, string academicYears, bool onlyEnrolled = false, Guid? careerId = null, Guid? facultyId = null, Guid? specialtyId = null, Guid? academicDepartmentId = null)
        {
            var academicYearSelected = JsonConvert.DeserializeObject<List<int>>(academicYears);

            if (rol == "0")
                rol = "";
            var count = await _userService.CountSurveyIntranetUsers(rol, academicYearSelected, onlyEnrolled, careerId, facultyId, specialtyId, academicDepartmentId);
            return Ok(count);
        }

        /// <summary>
        /// Envia a los usuarios la encuesta
        /// </summary>
        /// <param name="surveyId">Identificador de la encuesta</param>
        /// <param name="rol">Rol del usuario</param>
        /// <param name="onlyEnrolled">Solo Matriculados en caso de alumnos</param>
        /// <param name="careerId">Identificador de escuela en caso de alumnos</param>
        /// <param name="facultyId">Identificador de facultad en caso de alumnos</param>
        /// <param name="specialtyId">Identificador de especialidad en caso de alumnos</param>
        /// <param name="academicDepartmentId">Identificador de departamento academico en caso de docentes</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("nojob/enviar/usuarios/{surveyId}")]
        public async Task<IActionResult> NotJobSurvey(Guid surveyId, string rol, string academicYears, bool onlyEnrolled = false, Guid? careerId = null, Guid? facultyId = null, Guid? specialtyId = null, Guid? academicDepartmentId = null)
        {
            var academicYearSelected = JsonConvert.DeserializeObject<List<int>>(academicYears);

            var survey = await _surveyService.Get(surveyId);
            if (survey == null)
                return BadRequest("Esta encuesta no existe");
            if (survey.State == ConstantHelpers.SURVEY_STATES.SENT)
                return BadRequest("Esta encuesta ya fue enviada");
            if (rol == "0")
                rol = "";
            var query = await _userService.GetSurveyIntranetUsers(rol, academicYearSelected, onlyEnrolled, careerId, facultyId, specialtyId, academicDepartmentId);

            var result = query
                .Select(x => new
                {
                    id = x.Id,
                    user = x.UserName
                }).ToList();

            List<SurveyUser> surveyUsers = new List<SurveyUser>();
            for (int i = 0; i < result.Count; i++)
            {
                var surveyUser = new SurveyUser()
                {
                    UserId = result[i].id,
                    SurveyId = surveyId,
                    IsGraduated = false,
                    SectionId = null
                };
                surveyUsers.Add(surveyUser);
            }

            await _surveyUserService.AddRange(surveyUsers);
            survey.State = ConstantHelpers.SURVEY_STATES.SENT;
            await _surveyService.Update(survey);
            return Ok();
        }

        /// <summary>
        /// Envia a los usuarios la encuesta, este metodo esta hecho para enviar a mas de 1000 usuarios
        /// </summary>
        /// <param name="surveyId">Identificador de la encuesta</param>
        /// <param name="rol">Rol del usuario</param>
        /// <param name="onlyEnrolled">Solo Matriculados en caso de alumnos</param>
        /// <param name="careerId">Identificador de escuela en caso de alumnos</param>
        /// <param name="facultyId">Identificador de facultad en caso de alumnos</param>
        /// <param name="specialtyId">Identificador de especialidad en caso de alumnos</param>
        /// <param name="academicDepartmentId">Identificador de departamento academico en caso de docentes</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("job/enviar/usuarios/{surveyId}")]
        public async Task<IActionResult> JobSurvey(Guid surveyId, string rol, string academicYears, bool onlyEnrolled = false, Guid? careerId = null, Guid? facultyId = null, Guid? specialtyId = null, Guid? academicDepartmentId = null)
        {
            var academicYearSelected = JsonConvert.DeserializeObject<List<int>>(academicYears);

            var survey = await _surveyService.Get(surveyId);
            if (survey == null)
                return BadRequest("Esta encuesta no existe");
            if (survey.State == ConstantHelpers.SURVEY_STATES.SENT)
                return BadRequest("Esta encuesta ya fue enviada");
            if (rol == "0")
                rol = "";
            var query = await _userService.GetSurveyIntranetUsers(rol, academicYearSelected, onlyEnrolled, careerId, facultyId, specialtyId, academicDepartmentId);

            survey.State = AKDEMIC.CORE.Helpers.ConstantHelpers.SURVEY_STATES.INPROCESS;
            await _surveyService.Update(survey);

            var result = query
                .Select(x => new
                {
                    id = x.Id,
                    user = x.UserName
                }).ToList();


            var connectionString = _configuration.GetConnectionString(ConstantHelpers.DATABASES.CONNECTION_STRINGS.VALUES[new Tuple<int, int>(ConstantHelpers.GENERAL.DATABASES.DATABASE, ConstantHelpers.GENERAL.DATABASES.CONNECTION_STRINGS.CONNECTION_STRING)]);

            var table = ConstantHelpers.ENTITY_MODELS.INTRANET.SURVEY_USER;

            if (ConstantHelpers.GENERAL.DATABASES.DATABASE == ConstantHelpers.DATABASES.MYSQL)
            {
                table = table.Replace(".", "_");

                using (var mySqlConnection = new MySqlConnection(connectionString))
                {
                    await mySqlConnection.OpenAsync();
                    using (var mySqlTransaction = mySqlConnection.BeginTransaction())
                    {
                        using (var mySqlCommand = mySqlConnection.CreateCommand())
                        {
                            mySqlCommand.CommandText = $"INSERT INTO {table} " +
                                $"(Id , CreatedAt , SurveyId , UserId , IsGraduated)" +
                                $"VALUES( @Id , @CreatedAt, @SurveyId, @UserId, @IsGraduated)";
                            mySqlCommand.Transaction = mySqlTransaction;

                            mySqlCommand.Parameters.Add("@Id", DbType.String);
                            mySqlCommand.Parameters.Add("@SurveyId", DbType.String);
                            mySqlCommand.Parameters.Add("@UserId", DbType.String);
                            mySqlCommand.Parameters.Add("@CreatedAt", DbType.DateTime);
                            mySqlCommand.Parameters.Add("@IsGraduated", DbType.Boolean);
                            mySqlCommand.Prepare();

                            for (int i = 0; i < result.Count(); i++)
                            {
                                mySqlCommand.Parameters["@Id"].Value = Guid.NewGuid();
                                mySqlCommand.Parameters["@SurveyId"].Value = surveyId;
                                mySqlCommand.Parameters["@UserId"].Value = result[i].id;
                                mySqlCommand.Parameters["@CreatedAt"].Value = DateTime.UtcNow;
                                mySqlCommand.Parameters["@IsGraduated"].Value = 0;
                                await mySqlCommand.ExecuteNonQueryAsync();
                            }
                            mySqlTransaction.Commit();
                        }
                    }
                }
            }
            else
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();
                    using (var sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (var sqlCommand = sqlConnection.CreateCommand())
                        {
                            sqlCommand.CommandText = $"INSERT INTO {ConstantHelpers.ENTITY_MODELS.INTRANET.SURVEY_USER} " +
                                $"(Id , CreatedAt , SurveyId , UserId , IsGraduated)" +
                                $"VALUES( @Id , @CreatedAt, @SurveyId, @UserId, @IsGraduated)";
                            sqlCommand.Transaction = sqlTransaction;

                            sqlCommand.Parameters.Add("@Id", SqlDbType.UniqueIdentifier);
                            sqlCommand.Parameters.Add("@SurveyId", SqlDbType.UniqueIdentifier);
                            sqlCommand.Parameters.Add("@UserId", SqlDbType.NVarChar, 450);
                            sqlCommand.Parameters.Add("@CreatedAt", SqlDbType.DateTime2, 7);
                            sqlCommand.Parameters.Add("@IsGraduated", SqlDbType.Bit);
                            sqlCommand.Prepare();

                            for (int i = 0; i < result.Count; i++)
                            {
                                sqlCommand.Parameters["@Id"].Value = Guid.NewGuid();
                                sqlCommand.Parameters["@SurveyId"].Value = surveyId;
                                sqlCommand.Parameters["@UserId"].Value = result[i].id;
                                sqlCommand.Parameters["@CreatedAt"].Value = DateTime.UtcNow;
                                sqlCommand.Parameters["@IsGraduated"].Value = 0;
                                await sqlCommand.ExecuteNonQueryAsync();
                            }
                            sqlTransaction.Commit();
                        }
                    }
                }
            }
            survey.State = ConstantHelpers.SURVEY_STATES.SENT;
            await _surveyService.Update(survey);
            return Ok();
        }

        /// <summary>
        /// Envia la encuesta a los usuarios en la lista
        /// </summary>
        /// <param name="users">Lista de usuarios</param>
        /// <param name="surveyId">Identificador de encuesta</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("sendSurveys")]
        public async Task<IActionResult> SendSurveys(List<string> users, Guid surveyId)
        {
            var survey = await _surveyService.Get(surveyId);
            var surveyUsers = new List<SurveyUser>();

            foreach (var user in users)
            {
                var surveyUser = new SurveyUser
                {
                    SurveyId = surveyId,
                    UserId = user
                };
                surveyUsers.Add(surveyUser);
            }

            await _surveyUserService.AddRange(surveyUsers);
            survey.State = ConstantHelpers.SURVEY_STATES.SENT;
            await _surveyService.Update(survey);
            return Ok();
        }

        /// <summary>
        /// Obtiene la vista de respuestas para la encuesta
        /// </summary>
        /// <param name="id">Identificador de la encuesta</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("respuestas/{id}")]
        public IActionResult Answer(Guid id)
        {
            ViewBag.SurveyId = id;
            return View();
        }

        /// <summary>
        /// Obtiene la vista de las respuestas para encuesta de un usuario en especifico
        /// </summary>
        /// <param name="surveyUserId">Identificador de la encuesta del usuario</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("respuestas/detalle/{surveyUserId}")]
        public async Task<IActionResult> Detail(Guid surveyUserId)
        {
            var survey = await _surveyUserService.GetIncludeFirstLevel(surveyUserId);

            ViewBag.SurveyId = survey.SurveyId;
            ViewBag.FullName = $"{survey.User.UserName} - {survey.User.FullName}";
            ViewBag.SurveyUserId = surveyUserId;
            return View();
        }



        #region preguntas
        /// <summary>
        /// Crea una pregunta dentro de un item de la encuesta
        /// </summary>
        /// <param name="model">Modelo que contiene los parametros para la creacion de la pregunta con su respuestas</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("registrar/pregunta/post")]
        public async Task<IActionResult> AddQuestion(QuestionCreateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var surveyItem = await _surveyItemService.Get(model.SurveyItemId);

            var question = new Question
            {
                SurveyItemId = surveyItem.Id,
                Type = model.Type,
                Description = model.Description,
                Answers = new List<Answer>()
            };
            var answersQuantity = 0;

            if (model.Answers != null) answersQuantity = model.Answers.Count();

            if (model.Type == ConstantHelpers.SURVEY.UNIQUE_SELECTION_QUESTION || model.Type == ConstantHelpers.SURVEY.MULTIPLE_SELECTION_QUESTION)
            {
                if (answersQuantity < 2) return BadRequest("Agregue por lo menos dos respuestas");
                foreach (var answer in model.Answers)
                {
                    var newAnswer = new Answer
                    {
                        Description = answer.Description
                    };

                    question.Answers.Add(newAnswer);
                }
            }
            await _questionService.Insert(question);
            return Ok();
        }

        /// <summary>
        /// Crea una pregunta de tipo liker dentro de un item de la encuesta
        /// </summary>
        /// <param name="model">Modelo que contiene los parametros para la creacion de la pregunta con su respuestas</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("registrar/pregunta-likert/post")]
        public async Task<IActionResult> AddLikertQuestion(LikertQuestionCreateViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var surveyItem = await _surveyItemService.Get(model.SurveyItemId);

            if (!surveyItem.IsLikert)
            {
                return BadRequest("Esta sección no permite preguntas en Escala de Likert");
            }

            var question = new Question
            {
                SurveyItemId = surveyItem.Id,
                Type = ConstantHelpers.SURVEY.LIKERT_QUESTION,
                Description = model.Description
            };

            await _questionService.Insert(question);
            return Ok();
        }

        /// <summary>
        /// Editar una pregunta de tipo liker dentro de un item de la encuesta
        /// </summary>
        /// <param name="model">Modelo que contiene los parametros para la edición de la pregunta con su respuestas</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("editar/pregunta-likert/post")]
        public async Task<IActionResult> EditLikertQuestion(LikertQuestionEditViewModel model)
        {
            var question = await _questionService.Get(model.QuestionId);

            if (question == null)
            {
                return BadRequest("Sucedio un error");
            }

            question.Description = model.Description;

            await _questionService.Update(question);
            return Ok();
        }

        /// <summary>
        /// Obtiene la información de si la sección es Likert o no
        /// </summary>
        /// <param name="surveyItemId">Identificador de la sección de la encuesta</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("seccion/{surveyItemId}/validar-likert")]
        public async Task<IActionResult> SurveyItemInformation(Guid surveyItemId)
        {
            var surveyItem = await _surveyItemService.Get(surveyItemId);

            if (surveyItem == null)
                return BadRequest("No existe la sección");

            var result = new
            {
                surveyItem.Id,
                surveyItem.IsLikert
            };

            return Ok(result);
        }

        /// <summary>
        /// Obtiene la informacion de la pregunta, para saber si es de tipo likert
        /// </summary>
        /// <param name="questionId">Identificador de la pregunta</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("pregunta/{questionId}/validar-likert")]
        public async Task<IActionResult> QuestionInformation(Guid questionId)
        {
            var question = await _questionService.Get(questionId);

            if (question == null)
                return BadRequest("No existe la pregunta");

            var surveyItem = await _surveyItemService.Get(question.SurveyItemId);

            var result = new
            {
                QuestionId = question.Id,
                IsLikert = surveyItem.IsLikert
            };

            return Ok(result);
        }

        /// <summary>
        /// Obtiene una lista de las respuestas de los usuarios a una pregunta de tipo texto
        /// </summary>
        /// <param name="questionId">Identificador de pregunta</param>
        /// <returns>Retorna el objeto que contiene la estructura de la tabla</returns>
        [HttpGet("pregunta/{questionId}/respuestas")]
        public async Task<IActionResult> AnswersUserByQuestionTable(Guid questionId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _answerByUserService.GetAnswersFromTextQuesitonDataTable(sentParameters, questionId);

            return Ok(result);
        }

        /// <summary>
        /// Edita una pregunta
        /// </summary>
        /// <param name="model">Contiene el modelo con los parametros de la pregunta a editar</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("editar/pregunta/post")]
        public async Task<IActionResult> EditQuestion(QuestionEditViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var question = await _questionService.GetIncludeAnswers(model.Id);

            question.Type = model.Type;
            question.Description = model.Description;

            await _answerService.DeleteRange(question.Answers);

            var answersQuantity = 0;

            if (model.Answers != null) answersQuantity = model.Answers.Count();

            if (model.Type == ConstantHelpers.SURVEY.UNIQUE_SELECTION_QUESTION || model.Type == ConstantHelpers.SURVEY.MULTIPLE_SELECTION_QUESTION)
            {
                if (answersQuantity < 2) return BadRequest("Agregue por lo menos dos respuestas");
                foreach (var answer in model.Answers)
                {
                    var newAnswer = new Answer
                    {
                        Description = answer.Description
                    };

                    question.Answers.Add(newAnswer);
                }
            }
            await _questionService.Update(question);

            return Ok();
        }

        /// <summary>
        /// Elimina la pregunta
        /// </summary>
        /// <param name="id">Identificador de la pregunta</param>
        /// <returns></returns>
        [HttpPost("preguntas/eliminar")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            await _questionService.DeleteById(id);
            return Ok();
        }

        /// <summary>
        /// Obtiene la pregunta
        /// </summary>
        /// <param name="id">Identificador de la pregunta</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("pregunta/get/{id}")]
        public async Task<IActionResult> GetQuestion(Guid id)
        {
            var query = await _questionService.GetIncludeAnswers(id);

            var question = new
            {
                query.Id,
                query.SurveyItemId,
                query.Description,
                query.Type,
                Answers = query.Answers
                    .Select(x => new
                    {
                        x.Id,
                        x.Description
                    }).ToList()
            };

            return Ok(question);
        }

        /// <summary>
        /// Obtiene una vista parcial de las preguntas de la encuesta
        /// </summary>
        /// <param name="surveyUserId">Identificador del usuario y encuesta</param>
        /// <returns>Retorna una vista parcial</returns>
        [HttpGet("encuestas/{surveyUserId}/preguntas")]
        public async Task<IActionResult> _Questions(Guid surveyUserId)
        {
            var surveyUser = await _surveyUserService.GetSurveyUserTemplate(surveyUserId);

            return PartialView(surveyUser);
        }
        #endregion

        #region Items

        /// <summary>
        /// Crea un item/seccion dentro de la encuesta
        /// </summary>
        /// <param name="model">Modelo que contiene la informacion del nuevo item</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("item/crear")]
        public async Task<IActionResult> AddSection(SurveyItemViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Sucedio un Error");

            var surveyItem = new SurveyItem
            {
                SurveyId = model.Id,
                Title = model.Title,
                IsLikert = model.IsLikert
            };
            await _surveyItemService.Insert(surveyItem);
            return Ok();
        }

        /// <summary>
        /// Obtiene la seccion especificada de la encuesta
        /// </summary>
        /// <param name="id">Identificador del item</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("encuesta-seccion/{id}/get")]
        public async Task<IActionResult> GetSection(Guid id)
        {
            var surveyItem = await _surveyItemService.Get(id);
            var result = new
            {
                surveyItem.Title,
                surveyItem.Id,
                surveyItem.IsLikert
            };

            return Ok(result);
        }

        /// <summary>
        /// Edita la seccion especificada
        /// </summary>
        /// <param name="model">Modelo que contiene los parametros a editar de la seccion</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("encuesta-seccion/editar")]
        public async Task<IActionResult> EditSection(SurveyItemViewModel model)
        {
            var surveyItem = await _surveyItemService.Get(model.Id);

            if (surveyItem == null) return BadRequest("Sucedio un error");

            if (!ModelState.IsValid) return BadRequest("Sucedio un Error");

            if (string.IsNullOrWhiteSpace(model.Title))
                return BadRequest("Debe escribir un Título para la sección");

            //Si vamos a cambiar el tipo de seccion, debemos asegurarnos que no existan preguntas
            if (surveyItem.IsLikert != model.IsLikert)
            {
                if (await _surveyItemService.HasQuestions(surveyItem.Id))
                {
                    return BadRequest("No se puede cambiar el tipo de sección (Escala Likert), ya que tiene preguntas. Debe eliminar primero las preguntas y intentarlo otra vez");
                }
            }

            surveyItem.Title = model.Title;
            surveyItem.IsLikert = model.IsLikert;

            await _surveyItemService.Update(surveyItem);
            return Ok();
        }

        /// <summary>
        /// Obtiene una vista parcial de todos los items de una encuesta
        /// </summary>
        /// <param name="surveyId">Identificador de la encuesta</param>
        /// <returns>Retorna una vista parcial</returns>
        [HttpGet("items/get/{surveyId}")]
        public async Task<IActionResult> GetItems(Guid surveyId)
        {
            var survey = await _surveyService.Get(surveyId);
            bool sended = false;
            if (survey.State != ConstantHelpers.SURVEY_STATES.NOTSENT) sended = true;
            var query = await _surveyItemService.GetBySurvey(surveyId);

            var model = query
                 .Select(x => new SurveyItemQuestionViewModel
                 {
                     Id = x.Id,
                     Title = x.Title,
                     Sended = sended,
                     IsLikert = x.IsLikert,
                     Questions = x.Questions.Select(y => new QuestionViewModel
                     {
                         Id = y.Id,
                         Type = y.Type,
                         Description = y.Description,
                         Answers = y.Answers.Select(z => new AnswerViewModel
                         {
                             Id = z.Id,
                             Description = z.Description
                         }).ToList()
                     }).ToList()
                 }).ToList();

            return PartialView("_ItemsList", model);
        }

        /// <summary>
        /// Elimina un item especifico
        /// </summary>
        /// <param name="id">Identificador del item</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("item/eliminar")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            await _surveyItemService.DeleteById(id);
            return Ok();
        }
        #endregion
    }
}
