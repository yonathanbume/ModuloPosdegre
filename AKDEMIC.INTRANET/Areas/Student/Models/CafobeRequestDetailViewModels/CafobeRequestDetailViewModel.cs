// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.INTRANET.Areas.Student.Models.CafobeRequestDetailViewModels
{
    public class CafobeRequestDetailViewModel
    {
        public Guid CafobeRequestId { get; set; }

        public int Status { get; set; } // AKDEMIC.CORE.Helpers.ConstantHelpers.CAFOBE_REQUEST_DETAIL.STATUS;

        [Display(Name = "Estado", Prompt = "Sin enviar solicitud")]
        public string StatusText { get; set; }
        [Display(Name = "Comentario", Prompt = "Sin ninguna observación")]
        public string Comentary { get; set; }
        public string FileDetailUrl { get; set; }
        [Required(ErrorMessage = "El Archivo es obligatorio")]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile FileDetailFile { get; set; }
    }
}
