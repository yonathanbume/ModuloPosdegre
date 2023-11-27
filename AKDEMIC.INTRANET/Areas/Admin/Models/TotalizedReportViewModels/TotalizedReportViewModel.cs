using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.TotalizedReportViewModels
{
    public class TotalizedReportViewModel
    {
        public List<TotalizedManWomanViewModel> LstResult_Totalized_Man_Woman { get; set; }
        public List<TotalizedStudentUniversityPreparationViewModel> LstResult_Totalized_Student_UniversityPreparation { get; set; }
        public List<TotalizedStudentAdmissionTypeViewModel> LstResult_Totalized_Student_Admission { get; set; }
        public List<TotalizedStudentCareerViewModel> LstResult_Totalized_Student_Career { get; set; }
        public List<TotalizedStudentSchoolTypeViewModel> LstResult_Totalized_Student_SchoolType { get; set; }
        public List<TotalizedStudentLevelEducationViewModel> LstResult_Totalized_Student_LevelEducation { get; set; }
        public List<TotalizedStudentRangeAge> LstResult_Totalized_Student_Range { get; set; }
        public List<TotalizedStudentDependencyEconomicViewModel> LstResult_Totalized_Student_Dependency_Economic { get; set; }
        public List<TotalizedStudentCivilStatusViewModel> LstResult_Totalized_Student_Civil_Status { get; set; }
        public List<TotalizedStudentPrincipalPersonViewModel> LstResult_Totalized_Student_Principal_Person { get; set; }
        public List<TotalizedStudentCoexistenceViewModel> LstResult_Totalized_Student_Coexistence { get; set; }
        public List<TotalizedStudentFamilyRiskViewModel> LstResult_Totalized_Student_Family_Risk { get; set; }
        public List<TotalizedStudentTenureViewModel> LstResult_Totalized_Student_Tenure { get; set; }
        public List<TotalizedStudentConstructionTypeViewModel> LstResult_Totalized_Student_Construction_Type { get; set; }
        public List<TotalizedStudenZoneTypeViewModel> LstResult_Totalized_Student_ZoneType { get; set; }
        public List<TotalizedStudenBuildTypeViewModel> LstResult_Totalized_Student_BuildType { get; set; }
        public List<TotalizedStudentConstructionConditionViewModel> LstResult_Totalized_Student_Construction_Condition { get; set; }
        public List<TotalizedStudentTotalRemunerationViewModel> LstResult_Totalized_Student_RemunerationTotal { get; set; }
    }
}
