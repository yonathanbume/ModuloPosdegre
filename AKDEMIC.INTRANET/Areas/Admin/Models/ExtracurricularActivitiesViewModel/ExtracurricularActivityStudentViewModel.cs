using AKDEMIC.CORE.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ExtracurricularActivitiesViewModel
{
    public class ExtracurricularActivityStudentViewModel
    {
        public Guid? Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Estudiante", Prompt = "Estudiante")]
        public Guid StudentId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Actividad Extracurricular", Prompt = "Actividad Extracurricular")]
        public Guid ExtracurricularActivityId { get; set; }
        [Display(Name = "Nota", Prompt = "Nota")]
        public int? Grade { get; set; }
        [Display(Name = "Resolución", Prompt = "Resolución")]
        public string Resolution { get; set; }
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string EvaluationReportDate { get; set; }
    }
}
