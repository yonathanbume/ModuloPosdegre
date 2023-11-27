using System;
using System.ComponentModel.DataAnnotations;


namespace AKDEMIC.INTRANET.Areas.Admin.Models.SurveyViewModels
{
    public class SurveyEditViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Es Obligatoria")]
        public bool IsRequired { get; set; }

        [Display(Name = "Es Anónima")]
        public bool IsAnonymous { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Fecha de publicación")]
        public string PublicationDate { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Fecha de finalización")]
        public string FinishDate { get; set; }
    }

    public class SurveyEditSendedViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Fecha de publicación")]
        public string PublicationDate { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Fecha de finalización")]
        public string FinishDate { get; set; }
    }
}
