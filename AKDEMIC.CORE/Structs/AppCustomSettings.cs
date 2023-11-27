using AKDEMIC.CORE.Helpers;

namespace AKDEMIC.CORE.Structs
{
    public class AppCustomSettings
    {
        public AppCustomSettings()
        {
            //valores por defecto de los formatos

            //intranet
            EvaluationReport = "StandardFormat";
            AcademicSituationRecordReport = "StandardFormat";
            CertificatePDF = "StandardFormat";
            BlockEvaluationReport = "StandardBlockReport";
            GradeDetails = "StandardDetail";

            //teaching management
            AcademicChargeReport = ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNJBG || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.Akdemic ? "ReportFormat1" :  "StandardFormat";
            //AcademicChargeReport = "ReportFormat1";
            SaveCourse = "SaveStandard";

            //economic Management
            PaymentMethod = "LoadBatchMethodStandard";
        }
        //intranet
        public string EvaluationReport { get; set; }
        public string GradeDetails { get; set; }
        public string AcademicSituationRecordReport { get; set; }
        public string CertificatePDF { get; set; }
        public string BlockEvaluationReport { get; set; }


        //teaching management
        public string SaveCourse { get; set; }
        public string AcademicChargeReport { get; set; }

        //economic Managemente
        public string PaymentMethod { get; set; }
    }
}
