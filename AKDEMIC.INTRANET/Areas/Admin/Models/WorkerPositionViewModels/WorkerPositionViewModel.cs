using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.WorkerPositionViewModels
{
    public class WorkerPositionViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe rellenar los campos señalados")]
        [Display(Name = "Descripción", Prompt = "Description")]
        [StringLength(maximumLength: 100, MinimumLength = 4, ErrorMessage = "El texto ingresado debe tener como mínimo 4 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Debe rellenar los campos señalados")]
        [Display(Name = "Edad", Prompt = "Age")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Debe rellenar los campos señalados")]
        [Display(Name = "Categoría", Prompt = "Category")]
        [StringLength(maximumLength: 100, MinimumLength = 4, ErrorMessage = "El texto ingresado debe tener como mínimo 4 caracteres")]
        public string Category { get; set; }


        [Required(ErrorMessage = "Debe rellenar los campos señalados")]
        [Display(Name = "Dedicación", Prompt = "Dedication")]
        [StringLength(maximumLength: 100, MinimumLength = 4, ErrorMessage = "El texto ingresado debe tener como mínimo 4 caracteres")]
        public string Dedication { get; set; }


        [Required(ErrorMessage = "Debe rellenar los campos señalados")]
        [Display(Name = "Grado Académico", Prompt = "AcademicDegree")]
        [StringLength(maximumLength: 100, MinimumLength = 4, ErrorMessage = "El texto ingresado debe tener como mínimo 4 caracteres")]
        public string AcademicDegree { get; set; }


        [Required(ErrorMessage = "Debe rellenar los campos señalados")]
        [Display(Name = "Título profesional", Prompt = "JobTitle")]
        [StringLength(maximumLength: 100, MinimumLength = 4, ErrorMessage = "El texto ingresado debe tener como mínimo 4 caracteres")]
        public string JobTitle { get; set; }


        [Required(ErrorMessage = "Debe rellenar los campos señalados")]
        [Display(Name = "Documento de norma", Prompt = "Document")]
        [StringLength(maximumLength: 100, MinimumLength = 4, ErrorMessage = "El texto ingresado debe tener como mínimo 4 caracteres")]
        public string Document { get; set; }

    }
}
