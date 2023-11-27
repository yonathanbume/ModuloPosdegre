using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.HolidayViewModels
{
    public class HolidayViewModel
    {
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        [Display(Name = "Nombre", Prompt = "Ingrese nombre")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Fecha", Prompt = "Seleccione fecha")]
        public string Date { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Tipo", Prompt = "Seleccione tipo")]
        public byte Type { get; set; }
        public bool NeedReschedule { get; set; }
    }
}
