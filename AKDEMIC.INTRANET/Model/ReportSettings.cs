// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Model
{
    public class ReportSettings
    {
        #region AcademicRecord/GradeReportController
        //registrosacademicos/informes-de-grados/generar-constancia-grado/{gradeReportId}
        public string BachelorsDegreeHeaderText { get; set; } = "VICERRECTORADO ACADÉMICO";
        #endregion

        #region AcademicRecord/TitleReportController
        //registrosacademicos/informes-de-titulos/generar-constancia-titulo/{titlereportId}
        public string JobTitleHeaderText { get; set; } = "VICERRECTORADO ACADÉMICO";
        #endregion

        #region Academic/ConstancyController
        //academico/constancias/notas/{studentid}/{termid}
        public string ReportGradesHeaderText { get; set; } = "VICERRECTORADO ACADÉMICO";
        #endregion

        #region Academic/StudentInformationController
        //academico/alumnos/informacion/{id}/matriculas/reporte
        //academico/alumnos/informacion/{id}/matriculas/reporte/{termId}
        public string EnrollmentReportHeaderText { get; set; } = "VICERRECTORADO ACADÉMICO";
        //academico/alumnos/informacion/{id}/grado-bachiller
        public string BachelorsDegreeInfoHeaderText { get; set; } = "VICERRECTORADO ACADÉMICO";
        #endregion

        #region Admin/AcademicSituationController
        //admin/situacion-academica/
        //private function ReportFormat1
        public string AcademicSituationReportFormat1HeaderText { get; set; } = "VICERRECTORADO ACADÉMICO";
        #endregion

        #region Teacher/ClassScheduleController
        //profesor/horario-ciclo/carga-academica
        public string AcademicChargeHeaderText { get; set; } = "VICERRECTORADO ACADÉMICO";
        #endregion


        #region Teacher/GradeController
        //profesor/notas/acta-final/{sectionId}
        public string EvaluationReportHeaderText { get; set; } = "VICERRECTORADO ACADÉMICO";
        #endregion

        #region Teacher/SectionController
        //profesor/secciones/{sectionId}/matriculados/pdf
        public string SectionEnrolledReportPDF { get; set; } = "VICERRECTORADO ACADÉMICO";
        #endregion




    }
}
