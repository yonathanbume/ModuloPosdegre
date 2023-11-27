// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Report.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)]
    [Area("Report")]
    [Route("reporte/estudiantes-por-egresar")]
    public class FutureGraduatedStudentController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IDataTablesService _dataTablesService;
        private readonly ICurriculumService _curriculumService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IAcademicHistoryService _academicHistoryService;
        private readonly IAcademicYearCourseService _academicYearCourseService;
        private readonly ITermService _termService;

        public FutureGraduatedStudentController(
            IStudentService studentService,
            IDataTablesService dataTablesService,
            ICurriculumService curriculumService,
            IStudentSectionService studentSectionService,
            IAcademicHistoryService academicHistoryService,
            IAcademicYearCourseService academicYearCourseService,
            ITermService termService
            )
        {
            _studentService = studentService;
            _dataTablesService = dataTablesService;
            _curriculumService = curriculumService;
            _studentSectionService = studentSectionService;
            _academicHistoryService = academicHistoryService;
            _academicYearCourseService = academicYearCourseService;
            _termService = termService;
        }

        /// <summary>
        /// Vista donde se listan los estudiantes por egresar
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
            => View();

        /// <summary>
        /// Obtiene el listado de estudiantes por egresar
        /// </summary>
        /// <param name="careerId">identificador de la escuela profesional</param>
        /// <param name="curriculumId">Identificador del plan de estudio</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Listado de estudiantes</returns>
        [HttpGet("get")]
        public async Task<IActionResult> GetFutureGradutedStudentDatatable(Guid? careerId, Guid? curriculumId, string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetFutureGraduatedStudentDatatable(parameters, careerId, curriculumId, search, User);
            return Ok(result);
        }

        [HttpGet("get-excel")]
        public async Task<IActionResult> GetFutureGradutedStudentExcel(Guid? careerId, Guid? curriculumId)
        {
            var result = await _studentService.GetFutureGraduatedStudentsTemplate(careerId, curriculumId, User);

            var dt = new DataTable
            {
                TableName = "Reporte"
            };

            dt.Columns.Add("Usuario");
            dt.Columns.Add("Nombre Completo");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Créditos Aprobados");
            dt.Columns.Add("Créditos Matriculados");

            foreach (var item in result)
                dt.Rows.Add(item.UserName, item.FullName, item.Career, item.ApprovedCredits, item.EnrolledCredits);

            dt.AcceptChanges();

            string fileName = $"Reporte de estudiantes por egresar.xlsx";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);
                ws.AddHeaderToWorkSheet("Estudiantes por egresar", null);

                ws.Rows().AdjustToContents();
                ws.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        /// <summary>
        /// Obtiene el historial académico del alumno
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <returns>Objeto que contiene el listado de cursos del plan académico</returns>
        [HttpGet("historial-academico/alumno")]
        public async Task<IActionResult> GetCurriculumCourses(Guid studentId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _academicYearCourseService.GetCurriculumCoursesDatatable(sentParameters, studentId);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene los cursos matriculados del estudiante
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <returns>Objeto que contiene el listado de cursos del plan académico</returns>
        [HttpGet("cursos-matriculados/alumno")]
        public async Task<IActionResult> GetStudentSections(Guid studentId)
        {
            var term = await _termService.GetActiveTerm();
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentSectionService.GetCoursesReportCardDatatable(sentParameters, studentId, term.Id);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene la cantidad de créditos aprobados por el alumno
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <returns>Objeto que contiene la cantidad de créditos</returns>
        [HttpGet("creditos-aprobados/alumno")]
        public async Task<IActionResult> GetApprovedCredits(Guid studentId)
        {
            var credits = await _studentService.GetApprovedCreditsByStudentId(studentId);
            return Ok(credits);
        }

        /// <summary>
        /// Obtiene la cantidad de créditos matriculados por el alumno
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <returns>Objeto que contiene la cantidad de créditos matriculados</returns>
        [HttpGet("creditos-matriculados/alumno")]
        public async Task<IActionResult> GetEnrollmendCredits(Guid studentId)
        {
            var credits = await _studentService.GetEnrolledCreditsByStudentId(studentId/*, ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS*/);
            return Ok(credits);
        }
    }
}
