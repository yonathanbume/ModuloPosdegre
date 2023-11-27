using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.ViewModels.DocumentVerifierViewModels
{
    public class EvaluationReportViewModel
    {
        [Display(Name = "Tipo de Acta")]
        public string Type { get; set; }
        [Display(Name = "Facultad")]
        public string Faculty { get; set; }
        [Display(Name = "Carrera")]
        public string Career { get; set; }
        [Display(Name = "Profesor")]
        public string Teacher { get; set; }
        [Display(Name = "Creditos")]
        public string Credits { get; set; }
        [Display(Name = "Periodo")]
        public string Term { get; set; }
        [Display(Name = "Secciòn")]
        public string Section { get; set; }
        [Display(Name = "Identificador")]
        public string RelationalId { get; set; }
        [Display(Name = "Fecha de Recepción")]
        public string ReceptionDate { get; set; }
        [Display(Name = "Fecha de Generación")]
        public string GeneratedDate { get; set; }
    }
}
