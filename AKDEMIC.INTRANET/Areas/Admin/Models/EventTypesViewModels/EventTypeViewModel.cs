using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.EventTypesViewModels
{
    public class EventTypeViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="El campo '{0}' es obligatorio")]
        [Display(Name="Nombre", Prompt ="Nombre del tipo de evento")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Color", Prompt = "Color del tipo de evento")]
        public string Color { get; set; }
    }
}
