using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.Report_surveyViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Survey;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        ConstantHelpers.ROLES.CAREER_DIRECTOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_COORDINATOR + "," +
        ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR + "," +
        ConstantHelpers.ROLES.REPORT_QUERIES)]
    [Area("Admin")]
    [Route("admin/reporte_encuesta")]
    public class ReportSurveyController : BaseController
    {
        private readonly ICareerService _careerService;
        private readonly ISurveyService _surveyService;
        private readonly ISurveyItemService _surveyItemService;
        private readonly ISurveyUserService _surveyUserService;
        private readonly IAnswerByUserService _answerByUserService;
        private readonly ISelect2Service _select2Service;

        public ReportSurveyController(UserManager<ApplicationUser> userManager,
            IDataTablesService dataTablesService,
            ISelect2Service select2Service,
            ICareerService careerService,
            AkdemicContext context,
            ISurveyService surveyService,
            ISurveyItemService surveyItemService,
            ISurveyUserService surveyUserService,
            IAnswerByUserService answerByUserService
            ) : base(context, userManager, dataTablesService)
        {
            _select2Service = select2Service;
            _careerService = careerService;
            _surveyService = surveyService;
            _surveyItemService = surveyItemService;
            _surveyUserService = surveyUserService;
            _answerByUserService = answerByUserService;
        }

        /// <summary>
        /// Obtiene la vista inicial del reporte de encuesta
        /// </summary>
        /// <returns>Retorna una vista</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene una lista de todas las encuestas creadas en intranet de tipo general
        /// </summary>
        /// <param name="search">texto de busqueda</param>
        /// <returns>Retorna el objeto que contiene la estructura de la tabla</returns>
        [HttpGet("listar")]
        public async Task<IActionResult> GetAllSurveys(string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            int system = ConstantHelpers.Solution.Intranet;
            int type = ConstantHelpers.TYPE_SURVEY.GENERAL;
            var result = await _surveyService.GetIntranetReportSurveyDatatable(sentParameters, system, type, search);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene la vista de detalle de una encuesta
        /// </summary>
        /// <param name="surveyId">Identificador de encuesta</param>
        /// <returns>Retorna una vista</returns>
        [HttpGet("{surveyId}/detalle")]
        public async Task<IActionResult> GetGeneralSurveyDetail(Guid surveyId)
        {
            ViewBag.SurveyId = surveyId;
            decimal progressValue = 0.0M;

            try
            {
                progressValue = decimal.Round(await _surveyService.GetSurveyProgressPercentage(surveyId), 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception e)
            {
                //NothingHappens
            };

            ViewBag.ProgressValue = progressValue;
            return View();
        }

        [HttpGet("{surveyId}/reporte-excel-consolidado")]
        public async Task<IActionResult> GetExcelReportAdvanced(Guid surveyId)
        {
            var survey = await _context.Survey
                .Where(x => x.Id == surveyId)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync();

            if (survey == null) return BadRequest("Sucedio un error");

            var surveyItemsData = await _context.SurveyItems
                .Where(x => x.SurveyId == surveyId)
                .Select(x => new
                {
                    x.Id,
                    x.IsLikert,
                    x.Title,
                    Questions = x.Questions
                        .Where(x => x.Type != ConstantHelpers.SURVEY.TEXT_QUESTION)
                        .Select(y => new
                        {
                            y.Id,
                            y.Description,
                            y.Type,
                            Answers = y.Answers.Select(z => new
                            {
                                z.Id,
                                z.Description
                            })
                            .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();

            var answersByUsersData = await _context.AnswerByUsers
                .Where(x => x.SurveyUser.SurveyId == surveyId)
                .Select(x => new
                {
                    x.SurveyUser.User.UserName,
                    x.SurveyUser.UserId,
                    x.SurveyUser.SurveyId,
                    x.QuestionId,
                    x.AnswerId,
                    x.Description
                })
                .ToListAsync();

            var surveyItems = new List<SurveyExcelReportViewModel>();

            for (int i = 0; i < surveyItemsData.Count; i++)
            {
                //Informacion de la seccion
                var surveyItem = new SurveyExcelReportViewModel
                {
                    SurveyItemTitle = surveyItemsData[i].Title,
                    IsLikert = surveyItemsData[i].IsLikert
                };

                var questions = new List<SurveyExcelQuestionsReportViewModel>();

                //Preguntas cantidad y %s
                for (int j = 0; j < surveyItemsData[i].Questions.Count; j++)
                {
                    var question = new SurveyExcelQuestionsReportViewModel
                    {
                        QuestionDescription = surveyItemsData[i].Questions[j].Description,
                        TotalAnswers = answersByUsersData.Where(x => x.QuestionId == surveyItemsData[i].Questions[j].Id).Count(),
                        Type = surveyItemsData[i].Questions[j].Type,
                        TypeText = ConstantHelpers.SURVEY.TYPE_QUESTION.ContainsKey(surveyItemsData[i].Questions[j].Type) ?
                            ConstantHelpers.SURVEY.TYPE_QUESTION[surveyItemsData[i].Questions[j].Type] : "",
                    };

                    var answers = new List<SurveyExcelAnswersReportViewModel>();

                    if (surveyItemsData[i].Questions[j].Type == ConstantHelpers.SURVEY.LIKERT_QUESTION)
                    {
                        foreach (var item in ConstantHelpers.SURVEY_LIKERT.RATING_SCALE.LIKERT)
                        {
                            var answer = new SurveyExcelAnswersReportViewModel
                            {
                                AnswerText = item.Value,
                                Quantity = answersByUsersData.Where(x => x.QuestionId == surveyItemsData[i].Questions[j].Id && x.Description == item.Value).Count()
                            };

                            if (question.TotalAnswers == 0)
                            {
                                answer.Percentage = 0.0m;
                            }
                            else
                            {
                                answer.Percentage = (answer.Quantity * 100.0m) / question.TotalAnswers * 1.0m;
                            }
                            answers.Add(answer);
                        }
                    }
                    else if (surveyItemsData[i].Questions[j].Type == ConstantHelpers.SURVEY.MULTIPLE_SELECTION_QUESTION ||
                            surveyItemsData[i].Questions[j].Type == ConstantHelpers.SURVEY.UNIQUE_SELECTION_QUESTION)
                    {
                        for (int k = 0; k < surveyItemsData[i].Questions[j].Answers.Count; k++)
                        {
                            var answer = new SurveyExcelAnswersReportViewModel
                            {
                                AnswerText = surveyItemsData[i].Questions[j].Answers[k].Description,
                                Quantity = answersByUsersData.Where(x => x.AnswerId == surveyItemsData[i].Questions[j].Answers[k].Id).Count()
                            };

                            if (question.TotalAnswers == 0)
                            {
                                answer.Percentage = 0.0m;
                            }
                            else
                            {
                                answer.Percentage = (answer.Quantity * 100.0m) / question.TotalAnswers * 1.0m;
                            }


                            answers.Add(answer);
                        }
                    }

                    question.AnswersReport = answers;
                    questions.Add(question);
                }
                surveyItem.QuestionReport = questions;
                surveyItems.Add(surveyItem);
            }

            string fileName = $"Reporte_Encuesta.xlsx";
            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Consolidado de Encuesta");

                #region formato
                int currentRow = 2;
                int currentCol = 2;


                ws.Cell(currentRow, currentCol).Value = $"{survey.Name.ToUpper()}";
                ws.Cell(currentRow, currentCol).Style.Font.FontSize = 14;
                ws.Cell(currentRow, currentCol).Style.Font.Bold = true;
                ws.Cell(currentRow, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(currentRow, currentCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range(currentRow, currentCol, currentRow, currentCol + 12).Merge();

                ws.Column(1).Width = 8;
                ws.Column(2).Width = 15;
                ws.Column(3).Width = 15;
                ws.Column(4).Width = 15;
                ws.Column(5).Width = 25;
                ws.Column(6).Width = 25;
                ws.Column(7).Width = 25;
                ws.Column(8).Width = 25;
                ws.Column(9).Width = 25;
                ws.Column(10).Width = 25;
                ws.Column(11).Width = 25;
                ws.Column(12).Width = 25;
                ws.Column(13).Width = 25;
                ws.Column(14).Width = 25;
                ws.Column(15).Width = 25;
                ws.Column(16).Width = 25;

                //Bajamos
                currentRow = currentRow + 3;

                //Cabecera de la tabla
                for (int i = 0; i < surveyItems.Count; i++)
                {
                    //SurveyItem - Sección de la encuesta
                    ws.Cell(currentRow, currentCol).Value = $"{i + 1}.- SECCIÓN: {surveyItems[i].SurveyItemTitle.ToUpper()}";
                    ws.Cell(currentRow, currentCol).Style.Font.FontSize = 12;
                    ws.Cell(currentRow, currentCol).Style.Font.Bold = true;
                    ws.Cell(currentRow, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(currentRow, currentCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range(currentRow, currentCol, currentRow + 1, currentCol + 5).Merge();
                    ws.Range(currentRow, currentCol, currentRow + 1, currentCol + 5).Style.Fill.PatternType = XLFillPatternValues.Solid;
                    ws.Range(currentRow, currentCol, currentRow + 1, currentCol + 5).Style.Fill.BackgroundColor = XLColor.Yellow;


                    currentRow = currentRow + 2;
                    //Preguntas
                    for (int j = 0; j < surveyItems[i].QuestionReport.Count; j++)
                    {
                        var tmpQuestionCurrentCol = currentCol;

                        ws.Cell(currentRow, currentCol).Value = $"{j + 1}.- {surveyItems[i].QuestionReport[j].QuestionDescription.ToUpper()}";
                        ws.Cell(currentRow, currentCol).Style.Font.FontSize = 10;
                        ws.Cell(currentRow, currentCol).Style.Font.Bold = true;
                        ws.Cell(currentRow, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        ws.Cell(currentRow, currentCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                        ws.Range(currentRow, currentCol, currentRow + 2, currentCol + 2).Merge();
                        //Borders
                        ws.Range(currentRow, currentCol, currentRow + 2, currentCol + 2).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        ws.Range(currentRow, currentCol, currentRow + 2, currentCol + 2).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        ws.Range(currentRow, currentCol, currentRow + 2, currentCol + 2).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        ws.Range(currentRow, currentCol, currentRow + 2, currentCol + 2).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        ws.Range(currentRow, currentCol, currentRow + 2, currentCol + 2).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        ws.Range(currentRow, currentCol, currentRow + 2, currentCol + 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        currentCol = currentCol + 3;

                        //Alternativas
                        for (int k = 0; k < surveyItems[i].QuestionReport[j].AnswersReport.Count; k++)
                        {

                            //Respuesta
                            ws.Cell(currentRow, currentCol).Value = $"{surveyItems[i].QuestionReport[j].AnswersReport[k].AnswerText.ToUpper()}";
                            ws.Cell(currentRow, currentCol).Style.Font.FontSize = 10;
                            ws.Cell(currentRow, currentCol).Style.Font.Bold = true;
                            ws.Cell(currentRow, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            ws.Cell(currentRow, currentCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                            ws.Range(currentRow, currentCol, currentRow, currentCol + 1).Merge();
                            ws.Range(currentRow, currentCol, currentRow + 2, currentCol + 1).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            ws.Range(currentRow, currentCol, currentRow + 2, currentCol + 1).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            ws.Range(currentRow, currentCol, currentRow + 2, currentCol + 1).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            ws.Range(currentRow, currentCol, currentRow + 2, currentCol + 1).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            ws.Range(currentRow, currentCol, currentRow + 2, currentCol + 1).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            ws.Range(currentRow, currentCol, currentRow + 2, currentCol + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            currentRow = currentRow + 1;
                            //Cantidad
                            ws.Cell(currentRow, currentCol).Value = $"Cant.";
                            ws.Cell(currentRow, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(currentRow, currentCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            ws.Cell(currentRow + 1, currentCol).Value = $"{surveyItems[i].QuestionReport[j].AnswersReport[k].Quantity}";
                            ws.Cell(currentRow + 1, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            ws.Cell(currentRow + 1, currentCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

                            currentCol = currentCol + 1;
                            //Porcetanje
                            ws.Cell(currentRow, currentCol).Value = $"%";
                            ws.Cell(currentRow, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(currentRow, currentCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            ws.Cell(currentRow + 1, currentCol).Value = $"{surveyItems[i].QuestionReport[j].AnswersReport[k].Percentage}";
                            ws.Cell(currentRow + 1, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            ws.Cell(currentRow + 1, currentCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;


                            currentCol = currentCol + 1;
                            currentRow = currentRow - 1;
                        }
                        currentRow = currentRow + 3;

                        //Reset currentCol 
                        currentCol = tmpQuestionCurrentCol;
                    }

                    currentRow = currentRow + 1;
                }


                #endregion


                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //Return xlsx Excel File  
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        /// <summary>
        /// Obtiene una lista de usuarios a los que se le enviaron una encuesta
        /// </summary>
        /// <param name="surveyId">Identificador de la encuesta</param>
        /// <param name="searchValue">Texto de busqueda</param>
        /// <param name="answered">Indicador de si la encuesta fue completada, pendiente o ambos casos</param>
        /// <returns>Retorna el objeto que contiene la estructura de la tabla</returns>
        [HttpGet("usuarios/listar")]
        public async Task<IActionResult> GetUsersInGeneralSurvey(Guid surveyId, string searchValue = null, int answered = 0)
        {
            bool? answeredQuery;
            if (answered == 0) //Todos
            {
                answeredQuery = null;
            }
            else if (answered == 1)//Completado
            {
                answeredQuery = true;
            }
            else //Pendiente
            {
                answeredQuery = false;
            }
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _surveyUserService.GetUsersInGeneralSurveyDatatable(sentParameters, surveyId, searchValue, answeredQuery);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene una vista parcial de las respuestas de la encuesta, con sus preguntas y secciones respectivamente
        /// </summary>
        /// <param name="surveyId">Identificador de la encuesta</param>
        /// <returns>Retorna una vista parcial</returns>
        [HttpGet("{surveyId}/preguntas")]
        public async Task<IActionResult> _GetReportGeneralSurvey(Guid surveyId)
        {
            var result = await _surveyItemService.GetSurveyItemTemplate(surveyId);
            return PartialView(result);
        }

        /// <summary>
        /// Obtiene la información de las respuestas de la encuesta, con sus preguntas y secciones respectivamente
        /// </summary>
        /// <param name="surveyId">Identificador de la encuesta</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400 con la información solicitada</returns>
        [HttpGet("seccion-encuesta/{surveyId}/preguntas")]
        public async Task<IActionResult> GetSurveyItemData(Guid surveyId)
        {

            var result = await _context.SurveyItems
                .Where(x => x.SurveyId == surveyId)
                .Select(x => new SurveyItemReportTemplate
                {
                    Title = x.Title,
                    Reportes = x.Questions
                        .Select(y => new QuestionReportTemplate
                        {
                            Id = y.Id,
                            Name = y.Description,
                            Type = y.Type,
                            Alternatives = y.Answers
                                .Select(z => new AlternativeTemplate
                                {
                                    Description = z.Description,
                                    Count = z.AnswerByUsers
                                        .Where(au => au.Question.Type != ConstantHelpers.SURVEY.TEXT_QUESTION)
                                        .GroupBy(au => au.AnswerId)
                                        .Select(au => au.Count()).FirstOrDefault()
                                }).ToList()
                        }).ToList()
                })
                .ToListAsync();

            for (int i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < result[i].Reportes.Count; j++)
                {
                    if (result[i].Reportes[j].Type == ConstantHelpers.SURVEY.LIKERT_QUESTION)
                    {
                        result[i].Reportes[j].Alternatives = new List<AlternativeTemplate>();

                        foreach (var item in ConstantHelpers.SURVEY_LIKERT.RATING_SCALE.LIKERT)
                        {
                            var alternative = new AlternativeTemplate
                            {
                                Description = item.Value,
                                Count = await _context.AnswerByUsers
                                .Where(x => x.QuestionId == result[i].Reportes[j].Id && x.Description == item.Value)
                                .CountAsync()
                            };

                            result[i].Reportes[j].Alternatives.Add(alternative);
                        }
                    }
                }
            }

            return Ok(result);
        }

        /// <summary>
        /// Reporte excel de la encuesta y las respuestas de cada usuario
        /// </summary>
        /// <param name="surveyId">Identificador de la encuesta</param>
        /// <returns>Retorna un archivo excel</returns>
        [HttpGet("{surveyId}/reporte-excel")]
        public async Task<IActionResult> Excel(Guid surveyId)
        {
            var survey = await _surveyService.Get(surveyId);

            if (survey.IsAnonymous)
            {
                return BadRequest("Esta encuesta esta configurada como anónima");
            }

            if (survey.Type != ConstantHelpers.TYPE_SURVEY.GENERAL && survey.System != ConstantHelpers.Solution.Intranet)
            {
                return BadRequest();
            }
            var dt = new DataTable
            {
                TableName = "Reporte"
            };
            //Datos de alumno
            dt.Columns.Add("Usuario");
            dt.Columns.Add("NombreCompleto");
            //dt.Columns.Add("Docente/Alumno");
            //dt.Columns.Add("Dni");
            //dt.Columns.Add("EscuelaProfesional");
            //dt.Columns.Add("Ciclo");
            //Respuestas de los participantes a la encuesta
            var questionFromSurvey = await _surveyService.GetQuestions(surveyId);
            var surveyUserAnswers = await _answerByUserService.GetUserAnswersBySurvey(surveyId);
            foreach (var question in questionFromSurvey)
            {
                dt.Columns.Add(question.Question);
            }

            foreach (var surveyUser in surveyUserAnswers)
            {
                DataRow dr = dt.NewRow();
                dr["Usuario"] = surveyUser.UserName;
                dr["NombreCompleto"] = surveyUser.FullName;
                //dr["Docente/Alumno"] = surveyUser.Role;
                //dr["Dni"] = surveyUser.Dni;
                //dr["EscuelaProfesional"] = surveyUser.Career;
                //dr["Ciclo"] = surveyUser.CurrentAcademicYear;
                foreach (var question in questionFromSurvey)
                {
                    var answer = string.Join(", ", surveyUser.AnswersQuestions.Where(x => x.QuestionId == question.QuestionId).Select(x => x.Answer).ToList());
                    dr[question.Question] = answer;
                }
                dt.Rows.Add(dr);
            }

            dt.AcceptChanges();

            string fileName = $"{survey.Name}.xlsx";
            using (var wb = new XLWorkbook())
            {
                //Add DataTable in worksheet  
                wb.Worksheets.Add(dt);
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //Return xlsx Excel File  
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        /// <summary>
        /// Obtiene un excel de un reporte general
        /// </summary>
        /// <returns>Retorna un archivo</returns>
        [HttpGet("reporte")]
        public async Task<IActionResult> GetGeneralSurveysReport()
        {
            int system = ConstantHelpers.Solution.Intranet;
            int type = ConstantHelpers.TYPE_SURVEY.GENERAL;
            var data = await _surveyService.GetReportBySystemAndTypeData(system, type);
            var dt = new DataTable();

            dt.TableName = "Reporte";

            dt.Columns.Add("Código");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Fecha de Publicación");
            dt.Columns.Add("Cantidad de Encuestados");

            foreach (var item in data)
            {
                dt.Rows.Add(item.Code, item.Name, item.PublishDate, item.SurveyuserCount);
            }

            dt.AcceptChanges();

            string fileName = "Reporte encuesta general.xlsx";
            using (var wb = new XLWorkbook())
            {
                //Add DataTable in worksheet  
                wb.Worksheets.Add(dt);
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //Return xlsx Excel File  
                    Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
