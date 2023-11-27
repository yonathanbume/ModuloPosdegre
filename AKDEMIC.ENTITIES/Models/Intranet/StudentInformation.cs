using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class StudentInformation : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid? TermId { get; set; }
        public Term Term { get; set; }
        public Guid? StudentId { get; set; }

        [InverseProperty("StudentInformations")]
        public Student Student { get; set; }

        #region Personal Information        

        public int Age { get; set; }        

        public string OriginAddress { get; set; }

        public string OriginPhoneNumber { get; set; }

        public string CurrentPhoneNumber { get; set; }

        public Guid? PlaceOriginDistrictId { get; set; }

        public District PlaceOriginDistrict { get; set; }

        public Guid? OriginDistrictId { get; set; }

        public District OriginDistrict { get; set; }

        public string FullNameExternalPerson { get; set; }

        public string AddressExternalPerson { get; set; }

        public string EmailExternalPerson { get; set; }

        public string PhoneExternalPerson { get; set; }



        public bool CurrentLivingRuralArea { get; set; }

        public string CurrentCommunity { get; set; }

        public string CurrentPartiality { get; set; }

        public string CurrentPopulatedCenter { get; set; }


        public bool LivingRuralAreaExternalPerson { get; set; }

        public string CommunityExternalPerson { get; set; }

        public string PartialityExternalPerson { get; set; }

        public string PopulatedCenterExternalPerson { get; set; }


        #endregion

        #region Academic Background

        [StringLength(100)]
        public string OriginSchool { get; set; }

        public string OriginSchoolPlace { get; set; }

        public byte SchoolType { get; set; }

        [StringLength(100)]
        public string UniversityPreparation { get; set; }

        public int UniversityPreparationId { get; set; } = 1;


        public int YearCompletion { get; set; }
        public int ApplicationAttempts { get; set; }
        public bool StudyingOtherProfessionalSchool { get; set; }

        public byte UniversityChoose { get; set; }
        public byte ProfessionalSchoolChoose { get; set; }
        public bool StudyingOtherUniversity { get; set; }

        public bool Aymara { get; set; }
        public bool AymaraTalk { get; set; }
        public bool AymaraRead { get; set; }
        public bool AymaraWrite { get; set; }

        public bool Quechua { get; set; }
        public bool QuechuaTalk { get; set; }
        public bool QuechuaRead { get; set; }
        public bool QuechuaWrite { get; set; }

        public bool English { get; set; }
        public bool EnglishTalk { get; set; }
        public bool EnglishRead { get; set; }
        public bool EnglishWrite { get; set; }

        public bool German { get; set; }
        public bool GermanTalk { get; set; }
        public bool GermanRead { get; set; }
        public bool GermanWrite { get; set; }

        public bool French { get; set; }
        public bool FrenchTalk { get; set; }
        public bool FrenchRead { get; set; }
        public bool FrenchWrite { get; set; }

        public bool Italian { get; set; }
        public bool ItalianTalk { get; set; }
        public bool ItalianRead { get; set; }
        public bool ItalianWrite { get; set; }

        public bool Portuguese { get; set; }
        public bool PortugueseTalk { get; set; }
        public bool PortugueseRead { get; set; }
        public bool PortugueseWrite { get; set; }

        #endregion

        #region Health

        public byte IsSick { get; set; }

        [StringLength(400)]
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

        [StringLength(100)]
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


        public byte Cohabitant { get; set; }

        public byte Tenure2 { get; set; }

        public byte ContructionType2 { get; set; }

        public byte ZoneType2 { get; set; }

        public byte BuildType2 { get; set; }

        public string OtherTypeLivingPlace2 { get; set; }

        public byte NumberFloors2 { get; set; }

        public byte NumberRooms2 { get; set; }

        public byte NumberKitchen2 { get; set; }

        public byte NumberBathroom2 { get; set; }

        public byte NumberLivingRoom2 { get; set; }

        public byte NumberDinningRoom2 { get; set; }

        public bool Water2 { get; set; }

        public bool Drain2 { get; set; }

        public bool LivingPlacePhone2 { get; set; }

        public bool Light2 { get; set; }

        public bool Internet2 { get; set; }

        public bool TV2 { get; set; }

        public bool HasPhone2 { get; set; }

        public bool Radio2 { get; set; }

        public bool Stereo2 { get; set; }

        public bool Iron2 { get; set; }

        public bool EquipPhone2 { get; set; }

        public bool Laptop2 { get; set; }

        public bool Closet2 { get; set; }

        public bool Fridge2 { get; set; }

        public bool PersonalLibrary2 { get; set; }

        public bool EquipComputer2 { get; set; }

        #endregion
    }

}
