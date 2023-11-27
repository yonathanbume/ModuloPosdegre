using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.SurveyViewModels
{
    public class SurveyViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Es Obligatoria")]
        public bool IsRequired { get; set; }

        [Display(Name = "Es Anónima")]
        public bool IsAnonymous { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Código")]
        public string Code { get; set; }

        [Display(Name = "Fecha de publicación")]
        public string PublicationDate { get; set; }

        [Display(Name = "Fecha de finalización")]
        public string FinishDate { get; set; }

        [Display(Name = "Sección")]
        public string Title { get; set; }


    }
}