using Microsoft.AspNetCore.Http;
using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.GradeCorrectionViewModels
{
    public class CorrectionViewModel
    {
        public Guid Id { get; set; }

        public bool Status { get; set; }
    }
}
