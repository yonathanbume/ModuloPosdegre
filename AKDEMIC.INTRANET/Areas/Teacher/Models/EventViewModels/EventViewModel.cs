using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.EventViewModels
{
    public class EventViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Nombre", Prompt = "Nombre del evento")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Descripción", Prompt = "Descripción del evento")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Lugar", Prompt = "Lugar del evento")]
        public string Place { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Costo", Prompt = "Costo del evento")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Tipo", Prompt = "Tipo de evento")]
        public Guid Type { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Fecha del evento", Prompt = "Fecha del evento")]
        public string EventDate { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Fecha de inscripción", Prompt = "Fecha de inscripción")]
        public string RegistrationStartDate { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es obligatorio")]
        [Display(Name = "Fecha de finalización", Prompt = "Fecha de finalización")]
        public string RegistrationEndDate { get; set; }

        [Display(Name = "Archivo adjunto", Prompt = "Archivo adjunto")]
        public IFormFile File { get; set; } 

        public ICollection<EventType> EventTypes { get; set; }
    }
}
