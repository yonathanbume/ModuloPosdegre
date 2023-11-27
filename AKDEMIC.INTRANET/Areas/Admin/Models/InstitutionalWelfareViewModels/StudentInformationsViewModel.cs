using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.InstitutionalWelfareViewModels
{
    public class StudentInformationsViewModel
    {
        public Guid Id { get; set; }      
        
        #region General
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string DNI { get; set; }
        public string UserName { get; set; }
        public string Career { get; set; }                
        public string Birthdate { get; set; }        
        public int Sex { get; set; }
        public string Email { get; set; }
      
        public Guid? CurrentDistrictId { get; set; }
        public Guid? CurrentProvinceId { get; set; }
        public Guid? CurrentDepartmentId { get; set; }

        public Guid? OriginDistrictId { get; set; }
        public Guid? OriginProvinceId { get; set; }
        public Guid? OriginDepartmentId { get; set; }

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
        public int CurrentAcademicYear { get; internal set; }

        #endregion

    }

    public class StudentInformationTermViewModel
    {
        public Guid StudentId { get; set; }
        public Guid TermId { get; set; }

        public Guid? CurrentDepartmentIdDefault { get; set; }
        public Guid? CurrentProvinceIdDefault { get; set; }
        public Guid? CurrentDistrictIdDefault { get; set; }

        public Guid? OriginDistrictIdDefault { get; set; }
        public Guid? OriginDepartmentIdDefault { get; set; }
        public Guid? OriginProvinceIdDefault { get; set; }
    }

    public class PersonalInformationViewModel : StudentInformationTermViewModel
    {
        public string UserName { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string Dni { get; set; }
        public string CareerName { get; set; }
        public string FacultyName { get; set; }
        public string BirthDate { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public int CurrentAcademicYear { get; set; }
        public int CivilStatus { get; set; }
        public string TermName { get; set; }

        public int Sex { get; set; }
        public string CurrentAddress { get; set; }
        public string CurrentPhoneNumber { get; set; }
        public Guid? CurrentDepartmentId { get; set; }
        public Guid? CurrentProvinceId { get; set; }
        public Guid? CurrentDistrictId { get; set; }
        public Guid? OriginDistrictId { get; set; }
        public Guid? OriginDepartmentId { get; set; }
        public Guid? OriginProvinceId { get; set; }
        public string OriginPhoneNumber { get; set; }
        public string OriginAddress { get; set; }
        public string FullNameExternalPerson { get; set; }
        public string AddressExternalPerson { get; set; }
        public string EmailExternalPerson { get; set; }
        public string PhoneExternalPerson { get; set; }
    }

    public class AcademicBackgroundViewModel : StudentInformationTermViewModel
    {
        public string OriginSchool { get; set; }
        public string OriginSchoolPlace { get; set; }
        public int SchoolType { get; set; }
        public int UniversityPreparationId { get; set; }
    }

    public class EconomyViewModel : StudentInformationTermViewModel
    {
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
    }

    public class HealthViewModel : StudentInformationTermViewModel
    {
        public byte IsSick { get; set; }

        public string TypeParentIllness { get; set; }

        public byte HaveInsurance { get; set; }

        public byte InsuranceDescription { get; set; }
    }

    public class FeedViewModel : StudentInformationTermViewModel
    {
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
    }

    public class LivingPlaceViewModel : StudentInformationTermViewModel
    {
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
        public int CurrentAcademicYear { get; internal set; }
    }
}
