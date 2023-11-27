using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.CertificateGenerator.Models
{
    public class CertificateModel
    {
        public string GeneratedBy { get; set; }
        public string UserProcedureCode { get; set; }
        public string ImageQRBase64 { get; set; }
        public byte RecordType { get; set; }
        public TermModel CurrentTerm { get; set; }
        public StudentModel Student { get; set; }
        public UniversityInformationModel University { get; set; }
        public DocumentFormatModel DocumentFormat { get; set; }
        public PaymentModel Payment { get; set; }
    }
}
