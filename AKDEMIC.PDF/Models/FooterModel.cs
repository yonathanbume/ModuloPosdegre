using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Models
{
    public class FooterModel
    {
        public string GeneratedBy { get; set; }
        public string ImageQR { get; set; }
        public string AcademicRecordSigned { get; set; }
        public string SignatuareImgBase64 { get; set; }
    }
}
