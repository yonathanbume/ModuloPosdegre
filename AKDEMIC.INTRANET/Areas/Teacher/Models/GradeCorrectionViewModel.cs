using Microsoft.AspNetCore.Http;
using PdfSharp.Pdf.Advanced;
using System;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models
{
    public class GradeCorrectionViewModel
    {
        public Guid Id { get; set; }

        public Guid GradeId { get; set; }

        public decimal NewGrade { get; set; }

        public int State { get; set; } // 1: Pending 2: Approbed 3: Declined

        public string TeacherId { get; set; }

        public string Observations { get; set; }

        public bool NotTaken { get; set; }

        public IFormFile File { get; set; }
    }
}
