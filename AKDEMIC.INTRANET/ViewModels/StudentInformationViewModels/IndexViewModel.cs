using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.ViewModels.StudentInformationViewModels
{
    public class StudentFamilyViewModel
    {
        public string UserNameParameter { get; set; }
        public Guid Id { get; set; }
        public Student Student { get; set; }
        public Guid StudentId { get; set; }
        public string Name { get; set; }
        public string PaternalName { get; set; }
        public string MaternalName { get; set; }
        public string Birthday { get; set; }
        public string Relationship { get; set; }
        public string CivilStatus { get; set; }
        public string DegreeInstruction { get; set; }
        public string Certificated { get; set; }
        public string Occupation { get; set; }
        public string WorkCenter { get; set; }
        public string Location { get; set; }
        public byte RelationshipInt { get; set; }
        public byte CivilStatusInt { get; set; }
        public byte DegreeInstructionInt { get; set; }
        public bool IsSick { get; set; }
        public bool IsSick2 { get; set; }
        public string DiseaseType { get; set; }
        public bool SurgicalIntervention { get; set; }
        public bool SurgicalIntervention2 { get; set; }


        public string Community { get; set; }
        public string Partiality { get; set; }
        public string PopulatedCenter { get; set; }
        public string SecondOccupation { get; set; }
        public bool LivingRuralArea { get; set; }

        public Guid StudentFamilityDistrictId { get; set; }
    }
    public class SectionResolutionViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<QuestionResolutionViewModel> Questions { get; set; }
    }
    public class QuestionResolutionViewModel
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Response { get; set; }
        public bool Disabled { get; set; }
        public List<AnswerResolutionViewModel> Answers { get; set; }
        public List<Guid> Selection { get; set; }
    }
    public class AnswerResolutionViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
    }
    public class IndexViewModel
    {
        public byte SisfohClasification { get; set; }
        public IFormFile SisfohConstancy { get; set; }
        public bool IsStudentInformationResolved { get; set; }
        public string YearInformation { get; set; }
        //configuration 
        public bool ConfigStudentInformationVisibility { get; set; }
        public bool ConfigSurveyVisibility { get; set; }

        public List<StudentFamily> ListStudentFamilyOnlyRead { get; set; }
        public List<SectionResolutionViewModel> Sections { get; set; }
        public Guid Id { get; set; }
        public string UserNameParameter { get; set; }
        public string List { get; set; }

        #region General
        public string Term { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string DNI { get; set; }
        public string UserName { get; set; }
        public string Career { get; set; }
        public string QrImage { get; set; }
        public string BirthDate { get; set; }
        public int Sex { get; set; }
        public string Email { get; set; }

        public Guid? CurrentDistrictId { get; set; }
        public Guid? CurrentProvinceId { get; set; }
        public Guid? CurrentDepartmentId { get; set; }

        public Guid? OriginDistrictId { get; set; }
        public Guid? OriginProvinceId { get; set; }
        public Guid? OriginDepartmentId { get; set; }

        public Guid? PlaceOriginDistrictId { get; set; }
        public Guid? PlaceOriginProvinceId { get; set; }
        public Guid? PlaceOriginDepartmentId { get; set; }

        public string PlaceOriginDistrictDescription { get; set; }
        public string PlaceOriginProvinceDescription { get; set; }
        public string PlaceOriginDepartmentDescription { get; set; }

        public string CurrentDistrictDescription { get; set; }
        public string CurrentProvinceDescription { get; set; }
        public string CurrentDepartmentDescription { get; set; }



        #endregion

        #region Personal Information        

        public string Age { get; set; }

        public byte CivilStatus { get; set; }

        public string OriginAddress { get; set; }

        public string CurrentAddress { get; set; }

        public string OriginPhoneNumber { get; set; }

        public string CurrentPhoneNumber { get; set; }

        public string FullNameExternalPerson { get; set; }

        public string AddressExternalPerson { get; set; }

        public string EmailExternalPerson { get; set; }

        public string PhoneExternalPerson { get; set; }


        #endregion

        #region Academic Background


        public string OriginSchool { get; set; }

        public string OriginSchoolPlace { get; set; }

        public byte SchoolType { get; set; }

        public int UniversityPreparationId { get; set; }

        #endregion

        #region Health

        public byte IsSick { get; set; }

        public string TypeParentIllness { get; set; }

        public byte HaveInsurance { get; set; }

        public byte InsuranceDescription { get; set; }

        #endregion

        #region Feeding

        public bool BreakfastHome { get; set; }

        public bool BreakfastPension { get; set; }

        public bool BreakfastRelativeHome { get; set; }

        public bool BreakfastOther { get; set; }

        public bool LunchHome { get; set; }

        public bool LunchPension { get; set; }

        public bool LunchRelativeHome { get; set; }

        public bool LunchOther { get; set; }

        public bool DinnerHome { get; set; }

        public bool DinnerPension { get; set; }

        public bool DinnerRelativeHome { get; set; }

        public bool DinnerOther { get; set; }

        #endregion

        #region Economy

        public int PrincipalPerson { get; set; }

        public int EconomicMethodFatherTutor { get; set; }

        public int DSectorFatherTutor { get; set; }

        public int DWorkConditionFatherTutor { get; set; }

        public string DEspecificActivityFatherTutor { get; set; }

        public int DBusyFatherTutor { get; set; }

        public int ISectorFatherTutor { get; set; }

        public int IWorkConditionFatherTutor { get; set; }

        public string IEspecificActivityFatherTutor { get; set; }

        public int EconomicMethodMother { get; set; }

        public int DSectorMother { get; set; }

        public int DWorkConditionMother { get; set; }

        public string DEspecificActivityMother { get; set; }

        public int DBusyMother { get; set; }

        public int ISectorMother { get; set; }

        public int IWorkConditionMother { get; set; }

        public string IEspecificActivityMother { get; set; }

        public int EconomicExpensesFeeding { get; set; }

        public int EconomicExpensesBasicServices { get; set; }

        public int EconomicExpensesEducation { get; set; }

        public int EconomicExpensesOthers { get; set; }

        public int FatherRemuneration { get; set; }

        public int MotherRemuneration { get; set; }

        public int StudentRemuneration { get; set; }

        public int OtherRemuneration { get; set; }

        public int TotalRemuneration { get; set; }

        public int StudentDependency { get; set; }

        public int StudentCoexistence { get; set; }

        public int FamilyRisk { get; set; }

        public int StudentWorkDedication { get; set; }

        public string StudentWorkDescription { get; set; }

        public int StudentWorkCondition { get; set; }

        public bool AuthorizeCheck { get; set; }

        public string AuthorizedPersonFullName { get; set; }

        public string AuthorizedPersonAddress { get; set; }

        public string AuthorizedPersonPhone { get; set; }

        #endregion


        #region Living Place

        public byte Tenure { get; set; }

        public byte ContructionType { get; set; }

        public byte ZoneType { get; set; }

        public byte BuildType { get; set; }

        public string OtherTypeLivingPlace { get; set; }

        public byte NumberFloors { get; set; }

        public byte NumberRooms { get; set; }

        public byte NumberKitchen { get; set; }

        public byte NumberBathroom { get; set; }

        public byte NumberLivingRoom { get; set; }

        public byte NumberDinningRoom { get; set; }

        public bool Water { get; set; }

        public bool Drain { get; set; }

        public bool LivingPlacePhone { get; set; }

        public bool Light { get; set; }

        public bool Internet { get; set; }

        public bool TV { get; set; }

        public bool HasPhone { get; set; }

        public bool Radio { get; set; }

        public bool Stereo { get; set; }

        public bool Iron { get; set; }

        public bool EquipPhone { get; set; }

        public bool Laptop { get; set; }

        public bool Closet { get; set; }

        public bool Fridge { get; set; }

        public bool PersonalLibrary { get; set; }

        public bool EquipComputer { get; set; }
        public int CurrentAcademicYear { get; set; }
        public string Faculty { get; set; }

        #endregion

    }

}
