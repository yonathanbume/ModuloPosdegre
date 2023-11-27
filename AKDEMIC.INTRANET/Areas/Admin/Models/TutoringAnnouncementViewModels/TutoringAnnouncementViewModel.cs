using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Overrides;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.TutoringAnnouncementViewModels
{
    public class TutoringAnnouncementViewModel
    {
        public Guid? Id { get; set; }

        public bool AllRoles { get; set; }

        public bool AllCareers { get; set; }

        [Display(Name = "Roles", Prompt = "Roles")]
        public IEnumerable<string> TutoringAnnouncementRoleIds { get; set; }

        [Display(Name = "Escuelas Profesionales", Prompt = "Escuelas Profesionales")]
        public IEnumerable<Guid> TutoringAnnouncementCareerIds { get; set; }

        [Display(Name = "Fecha de Publicación", Prompt = "Fecha de Publicación")]
        public string DisplayTime { get; set; }

        [Display(Name = "Fecha fin de Publicación", Prompt = "Fecha fin de Publicación")]
        public string EndTime { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Asunto", Prompt = "Asunto")]
        public string Title { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Mensaje", Prompt = "Mensaje")]
        public string Message { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        [DataType(DataType.Upload)]
        [Extensions(ConstantHelpers.DOCUMENTS.FILE_EXTENSION_GROUP.PDF_IMG_DOCUMENTS, ErrorMessage = "La extension del archivo no es valida")]
        public IFormFile File { get; set; }

    }
}
