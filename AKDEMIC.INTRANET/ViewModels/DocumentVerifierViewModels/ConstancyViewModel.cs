using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.ViewModels.DocumentVerifierViewModels
{
    public class ConstancyViewModel
    {
        [Display(Name = "Tipo de Documento")]
        public string Type { get; set; }
        [Display(Name = "Estudiante")]
        public string Student { get; set; }
        [Display(Name = "Escuela Profesional")]
        public string Career { get; set; }
        [Display(Name = "Facultad")]
        public string Faculty { get; set; }
        [Display(Name = "Cod. Trámite")]
        public string UserProcedureCode { get; set; }
        [Display(Name = "Fec. Solicitud")]
        public string RequestDate { get; set; }
    }
}
