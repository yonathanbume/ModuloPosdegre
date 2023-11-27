using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.CertificateGenerator.Models
{
    public class PostulantModel
    {
        public int Place { get; set; }
        public decimal Score { get; set; }
        public int TotalStudents { get; set; }
        public string Modality { get; set; }
    }
}
