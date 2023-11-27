using System;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.UpdateStudentGradeViewModels
{
    public class UpdateGradeViewModel
    {
        public Guid AcademicHistoryId { get; set; }
        public Guid Id { get; set; }
        public decimal Grade { get; set; }
        public IFormFile Resolution { get; set; }
        public string PIN { get; set; }
    }
}
