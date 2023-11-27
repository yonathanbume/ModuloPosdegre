using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    //Formacion Academica en Bolsa
    public class StudentCertificate
    {
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        //Titulo adquirido - Grado Adquirido
        public string Description { get; set; }

        public string Institution { get; set; }

        public int Type { get; set; } = ConstantHelpers.STUDENTCERTIFICATE.TYPE.NOTSPECIFIED;
        public int Level { get; set; } = ConstantHelpers.STUDENTCERTIFICATE.LEVEL.NOTSPECIFIED;
        public int Merit { get; set; } = ConstantHelpers.STUDENTCERTIFICATE.MERIT.NONE;
        public DateTime? CertificateDate { get; set; }
        public string CertificateFilePath { get; set; }
        public string MeritFilePath { get; set; }

        public string File { get; set; }

        public Student Student { get; set; }
    }
}
