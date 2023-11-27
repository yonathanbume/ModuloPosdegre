using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.ExamWeekViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN)]
    [Route("admin/semana-examenes")]
    public class ExamWeekController : BaseController
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IExamWeekService _examWeekService;
        private readonly ITermService _termService;
        private readonly AkdemicContext _context;

        public ExamWeekController(
            IDataTablesService dataTablesService,
            IExamWeekService examWeekService,
            ITermService termService,
            AkdemicContext context
            )
        {
            _dataTablesService = dataTablesService;
            _examWeekService = examWeekService;
            _termService = termService;
            _context = context;
        }

        /// <summary>
        /// Vista donde se gestiona la semana de exámanes
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de semanas de examanes para ser usado en tablas
        /// </summary>
        /// <returns>Objeto que tiene el listado de semanas de exámanes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _examWeekService.GetExamWeekDatatable(parameters);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el detalle de la semana de examen
        /// </summary>
        /// <param name="termId">Identificador del periodo académico</param>
        /// <returns>Objeto que contiene los datos de la semana de examen</returns>
        [HttpGet("get-semanas-detalle")]
        public async Task<IActionResult> GetWeekDetails(Guid termId)
        {
            var term = await _termService.Get(termId);
            var startClass = term.ClassStartDate.Date;
            var endClass = term.ClassEndDate.Date;
            var result = new List<WeekViewModel>();
            var tpmClassDay = startClass.Date;

            var tpmWeek = 1;

            while (tpmClassDay < endClass)
            {
                var rest = (endClass - tpmClassDay).Days;
                if (rest < 7)
                {
                    result.Add(new WeekViewModel
                    {
                        Text = $"Semana {tpmWeek} ({tpmClassDay} - {tpmClassDay.AddDays(rest)})",
                        Id = tpmWeek
                    });

                    tpmClassDay = tpmClassDay.AddDays(rest);
                }
                else
                {
                    result.Add(new WeekViewModel
                    {
                        Text = $"Semana {tpmWeek} ({tpmClassDay} - {tpmClassDay.AddDays(7)})",
                        Id = tpmWeek
                    });

                    tpmClassDay = tpmClassDay.AddDays(7);
                }
                tpmWeek++;
            }

            return Ok(result);

        }

        /// <summary>
        /// Método para agregar una semana de examen
        /// </summary>
        /// <param name="model">Objeto que contiene los datos de la nueva semana de examen</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("agregar")]
        public async Task<IActionResult> Add(ExamWeekViewModel model)
        {
            if (await _examWeekService.AnyByTermAndType(model.TermId, model.Type))
                return BadRequest($"Ya existe una semana de tipo '{ConstantHelpers.EXAM_WEEK_TYPE.VALUES[model.Type]}' para el periodo seleccionado.");

            var term = await _termService.Get(model.TermId);

            var classes = await _context.Classes.Where(x => x.ClassSchedule.Section.CourseTerm.TermId == model.TermId && x.WeekNumber == model.Week).ToListAsync();
            if (classes.Any(y => y.IsDictated))
                return BadRequest($"Existen clases dictadas en la semana {model.Week}");

            if (classes.Any(y => y.IsRescheduled))
                return BadRequest($"Se han encontrado clases reprogramadas en la semana {model.Week}");

            var classWeek = new List<ClassWeekViewModel>();
            var startClass = term.ClassStartDate.Date;
            var endClass = term.ClassEndDate.Date;
            var tpmClassDay = startClass.Date;
            var tpmWeek = 1;

            while (tpmClassDay < endClass)
            {
                var rest = (endClass - tpmClassDay).Days;
                if (rest < 7)
                {
                    classWeek.Add(new ClassWeekViewModel
                    {
                        Week = tpmWeek,
                        Start = tpmClassDay,
                        End = tpmClassDay.AddDays(rest)
                    });

                    tpmClassDay = tpmClassDay.AddDays(rest);
                }
                else
                {
                    classWeek.Add(new ClassWeekViewModel
                    {
                        Week = tpmWeek,
                        End = tpmClassDay.AddDays(7),
                        Start = tpmClassDay
                    });

                    tpmClassDay = tpmClassDay.AddDays(7);
                }
                tpmWeek++;
            }

            //_context.Classes.RemoveRange(classes);

            var entity = new ExamWeek
            {
                TermId = model.TermId,
                Type = model.Type,
                Week = model.Week,
                WeekStart = classWeek.Where(x => x.Week == model.Week).Select(x => x.Start).FirstOrDefault(),
                WeekEnd = classWeek.Where(x => x.Week == model.Week).Select(x => x.End).FirstOrDefault()
            };

            await _examWeekService.Insert(entity);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
