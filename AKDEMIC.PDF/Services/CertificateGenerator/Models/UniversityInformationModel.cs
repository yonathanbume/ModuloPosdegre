using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.CertificateGenerator.Models
{
    public class UniversityInformationModel
    {
        public string LogoImgPath { get; set; }
        public string HeaderText { get; set; }
        public string SubHeaderText { get; set; }
        public string AcademicRecordSigned { get; set; }
        public string SignatuareImgBase64 { get; set; }
        public string BossPositionRecordSigning { get; set; }
        public byte HeaderType { get; set; }
        public string Office { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string WebSite { get; set; }
    }
}
