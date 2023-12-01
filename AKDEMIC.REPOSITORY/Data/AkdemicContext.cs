using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.ENTITIES.Models.Server;
using AKDEMIC.ENTITIES.Models.ComputerCenter;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Geo;
using AKDEMIC.ENTITIES.Models.HelpDesk;
using AKDEMIC.ENTITIES.Models.Indicators;
using AKDEMIC.ENTITIES.Models.InstitutionalAgenda;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.ENTITIES.Models.InternationalCooperationManagement;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.ENTITIES.Models.Kardex;
using AKDEMIC.ENTITIES.Models.LanguageCenter;
using AKDEMIC.ENTITIES.Models.Laurassia;
using AKDEMIC.ENTITIES.Models.Matricula;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.ENTITIES.Models.Permission;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.ENTITIES.Models.Quibuk;
using AKDEMIC.ENTITIES.Models.Reservations;
using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.ENTITIES.Models.Scale.Maps;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.ENTITIES.Models.VirtualDirectory;
using AKDEMIC.ENTITIES.Models.VisitManagement;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using OpenIddict.EntityFrameworkCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.PosDegree;

namespace AKDEMIC.REPOSITORY.Data
{
    public class AkdemicContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly List<AuditEntry> _temporaryPropertyAuditEntries;

        public AkdemicContext(DbContextOptions<AkdemicContext> options) : base(options)
        {
            _httpContextAccessor = new HttpContextAccessor();
            _temporaryPropertyAuditEntries = new List<AuditEntry>();

            if (ConstantHelpers.GENERAL.DATABASES.DATABASE != ConstantHelpers.DATABASES.PSQL)
                Database.SetCommandTimeout(Int32.MaxValue);
        }

        public AkdemicContext(DbContextOptions<AkdemicContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _temporaryPropertyAuditEntries = new List<AuditEntry>();

            if (ConstantHelpers.GENERAL.DATABASES.DATABASE != ConstantHelpers.DATABASES.PSQL)
                Database.SetCommandTimeout(Int32.MaxValue);
        }

        #region TABLES

        #region ACADEMIC EXCHANGE
        public DbSet<AcademicAgreement> AcademicAgreements { get; set; }
        public DbSet<AgreementType> AgreementTypes { get; set; }
        public DbSet<AcademicAgreementType> AcademicAgreementTypes { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<QuestionnaireSection> QuestionnaireSections { get; set; }
        public DbSet<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }
        public DbSet<Scholarship> Scholarships { get; set; }
        public DbSet<ScholarshipFile> ScholarshipFiles { get; set; }
        public DbSet<QuestionnaireAnswer> QuestionnaireAnswers { get; set; }
        public DbSet<QuestionnaireAnswerByUser> QuestionnaireAnswerByUsers { get; set; }
        public DbSet<Postulation> Postulations { get; set; }
        public DbSet<PostulationFile> PostulationFiles { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<PostulationInformation> PostulationInformations { get; set; }
        public DbSet<AcademicExchangeNews> AcademicExchangeNews { get; set; }
        public DbSet<AeGeneralFile> AeGeneralFiles { get; set; }
        public DbSet<AeContactMessage> AeContactMessages { get; set; }


        #endregion

        #region ADMISSION
        public DbSet<ApplicationTermSurvey> ApplicationTermSurveys { get; set; }
        public DbSet<ApplicationTermSurveyQuestion> ApplicationTermSurveyQuestions { get; set; }
        public DbSet<ApplicationTermSurveyAnswer> ApplicationTermSurveyAnswers { get; set; }
        public DbSet<ApplicationTermSurveyUser> ApplicationTermSurveyUsers { get; set; }
        public DbSet<ApplicationTermSurveyAnswerByUser> ApplicationTermSurveyAnswerByUsers { get; set; }

        public DbSet<AdmissionExamClassroomTeacher> AdmissionExamClassroomTeachers { get; set; }
        public DbSet<AdmissionExamClassroomCareer> AdmissionExamClassroomCareers { get; set; }
        public DbSet<ApplicationTermAdmissionType> ApplicationTermAdmissionTypes { get; set; }
        public DbSet<ApplicationTermCampus> ApplicationTermCampuses { get; set; }
        public DbSet<AdmissionExamPostulantGrade> AdmissionExamPostulantGrades { get; set; }
        public DbSet<ApplicationTerm> ApplicationTerms { get; set; }
        public DbSet<CareerApplicationTerm> CareerApplicationTerms { get; set; }
        public DbSet<AdmissionExamApplicationTerm> AdmissionExamApplicationTerms { get; set; }
        public DbSet<AdmissionExamChannel> AdmissionExamChannels { get; set; }
        public DbSet<PostulantCardSection> PostulantCardSections { get; set; }
        public DbSet<Prospect> Prospects { get; set; }
        public DbSet<VocationalTest> VocationalTests { get; set; }
        public DbSet<VocationalTestAnswer> VocationalTestAnswers { get; set; }
        public DbSet<VocationalTestQuestion> VocationalTestQuestions { get; set; }
        public DbSet<VocationalTestAnswerCareer> VocationalTestAnswerCareers { get; set; }
        public DbSet<VocationalTestAnswerCareerPostulant> VocationalTestAnswerCareerPostulants { get; set; }
        public DbSet<ApplicationTermManager> ApplicationTermManagers { get; set; }
        public DbSet<EntityLoadFormat> EntityLoadFormats { get; set; }
        public DbSet<AdmissionExam> AdmissionExams { get; set; }
        public DbSet<AdmissionExamClassroom> AdmissionExamClassrooms { get; set; }
        public DbSet<AdmissionExamClassroomPostulant> AdmissionExamClassroomPostulants { get; set; }
        public DbSet<AdmissionResult> AdmissionResults { get; set; }
        public DbSet<AdmissionRequirement> AdmissionRequirements { get; set; }
        public DbSet<AdmissionType> AdmissionTypes { get; set; }
        public DbSet<AdmissionTypeDescount> AdmissionTypeDescounts { get; set; }
        public DbSet<Postulant> Postulants { get; set; }
        public DbSet<PostulantAdmissionRequirement> PostulantAdmissionRequirements { get; set; }
        public DbSet<PostulantFamily> PostulantFamilies { get; set; }
        public DbSet<Vacant> Vacants { get; set; }
        public DbSet<SpecialCase> SpecialCases { get; set; }
        public DbSet<ApplicationTermAdmissionFile> ApplicationTermAdmissionFiles { get; set; }
        public DbSet<PostulantOriginalLanguage> PostulantOriginalLanguages { get; set; }
        public DbSet<SanctionedPostulant> SanctionedPostulants { get; set; }
        public DbSet<School> Schools { get; set; }

        #endregion

        #region CAFETERIA

        public DbSet<UnitMeasurement> UnitMeasurements { get; set; }
        public DbSet<CafeteriaServiceTerm> CafeteriaServiceTerms { get; set; }
        public DbSet<CafeteriaStock> CafeteriaStocks { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<SupplyOrder> SupplyOrders { get; set; }
        public DbSet<SupplyOrderDetail> SupplyOrderDetails { get; set; }
        public DbSet<UserCafeteriaServiceTerm> UserCafeteriaServiceTerms { get; set; }
        public DbSet<UserCafeteriaDailyAssistance> UserCafeteriaDailyAssistances { get; set; }
        public DbSet<CafeteriaPostulation> CafeteriaPostulations { get; set; }
        public DbSet<CafeteriaServiceTermSchedule> CafeteriaServiceTermSchedules { get; set; }
        public DbSet<CafeteriaWeeklySchedule> CafeteriaWeeklySchedules { get; set; }
        public DbSet<MenuPlate> MenuPlates { get; set; }
        public DbSet<MenuPlateSupply> MenuPlateSupplies { get; set; }
        public DbSet<CafeteriaKardex> CafeteriaKardexes { get; set; }
        public DbSet<CafeteriaWeeklyScheduleTurnDetail> CafeteriaWeeklyScheduleTurnDetails { get; set; }
        public DbSet<SupplyPackage> SupplyPackages { get; set; }
        public DbSet<ProviderSupply> ProviderSupplies { get; set; }

        #endregion

        #region COMPUTER CENTER

        public DbSet<ComActivity> ComActivities { get; set; }
        public DbSet<ComClass> ComClasses { get; set; }
        public DbSet<ComClassSchedule> ComClassSchedules { get; set; }
        public DbSet<ComClassUser> ComClassUsers { get; set; }
        public DbSet<ComCourse> ComCourses { get; set; }
        public DbSet<ComCourseModule> ComCourseModules { get; set; }
        public DbSet<ComEvaluationCriteria> ComEvaluationCriterias { get; set; }
        public DbSet<ComGrades> ComGrades { get; set; }
        public DbSet<ComGroup> ComGroups { get; set; }
        public DbSet<ComPayment> ComPayments { get; set; }
        public DbSet<ComUserGroup> ComUserGroups { get; set; }

        #endregion

        #region COMPUTER MANAGEMENT

        public DbSet<Computer> Computers { get; set; }
        public DbSet<ComputerConditionFile> ComputerConditionFiles { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<SoftwareType> SoftwareType { get; set; }
        public DbSet<SoftwareSubType> SoftwareSubType { get; set; }
        public DbSet<Hardware> Hardwares { get; set; }
        public DbSet<HardwareType> HardwareTypes { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<ComputerSupplier> ComputerSuppliers { get; set; }
        public DbSet<ComputerState> ComputerState { get; set; }
        public DbSet<ComputerType> ComputerType { get; set; }
        public DbSet<InstitutionalStrategicPlan> InstitutionalStrategicPlans { get; set; }


        #endregion

        #region DEGREE


        public DbSet<RegistryPattern> RegistryPatterns { get; set; }
        public DbSet<ForeignUniversityOrigin> ForeignUniversityOrigins { get; set; }
        public DbSet<HistoricalRegistryPattern> HistoricalRegistryPatterns { get; set; }
        public DbSet<DegreeRequirement> DegreeRequirements { get; set; }
        public DbSet<DegreeProjectDirector> DegreeProjectDirectors { get; set; }


        #endregion
        #region POSDEGREE
        public DbSet<Master> Masters { get; set; }
        public DbSet<PosdegreeDetailsPayment> PosdegreeDetailsPayments { get; set; }
        public DbSet<PosdegreeStudent> PosdegreeStudents { get; set; }
        #endregion


        #region DOCUMENTARY PROCEDURE

        public DbSet<Dependency> Dependencies { get; set; }
        public DbSet<DocumentaryRecordType> DocumentaryRecordTypes { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<ExternalProcedure> ExternalProcedures { get; set; }
        public DbSet<InternalProcedure> InternalProcedures { get; set; }
        public DbSet<InternalProcedureFile> InternalProcedureFiles { get; set; }
        public DbSet<InternalProcedureReference> InternalProcedureReferences { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<ProcedureCategory> ProcedureCategories { get; set; }
        public DbSet<ProcedureDependency> ProcedureDependencies { get; set; }
        public DbSet<ProcedureRequirement> ProcedureRequirements { get; set; }
        public DbSet<ProcedureResolution> ProcedureResolutions { get; set; }
        public DbSet<ProcedureRole> ProcedureRoles { get; set; }
        public DbSet<ProcedureAdmissionType> ProcedureAdmissionTypes { get; set; }
        public DbSet<ProcedureSubcategory> ProcedureSubcategories { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<RecordSubjectType> RecordSubjectTypes { get; set; }
        public DbSet<UserDependency> UserDependencies { get; set; }
        public DbSet<UserExternalProcedure> UserExternalProcedures { get; set; }
        public DbSet<UserExternalProcedureRecord> UserExternalProcedureRecords { get; set; }
        public DbSet<UserExternalProcedureRecordDocument> UserExternalProcedureRecordDocuments { get; set; }
        public DbSet<UserInternalProcedure> UserInternalProcedures { get; set; }
        public DbSet<UserProcedure> UserProcedures { get; set; }
        public DbSet<UserProcedureDerivation> UserProcedureDerivations { get; set; }
        public DbSet<UserProcedureDerivationFile> UserProcedureDerivationFiles { get; set; }
        public DbSet<UserProcedureRecord> UserProcedureRecords { get; set; }
        public DbSet<UserProcedureRecordDocument> UserProcedureRecordDocuments { get; set; }
        public DbSet<UserProcedureRecordRequirement> UserProcedureRecordRequirements { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<ComplaintFile> ComplaintFiles { get; set; }
        public DbSet<ComplaintType> ComplaintTypes { get; set; }
        public DbSet<UserProcedureFile> UserProcedureFiles { get; set; }
        public DbSet<ActivityProcedure> ActivityProcedures { get; set; }
        public DbSet<UserExternalProcedureFile> UserExternalProcedureFiles { get; set; }
        public DbSet<SignedDocument> SignedDocuments { get; set; }
        public DbSet<DocumentTemplate> DocumentTemplates { get; set; }
        public DbSet<ProcedureFolder> ProcedureFolders { get; set; }
        public DbSet<ProcedureTask> ProcedureTasks { get; set; }
        public DbSet<StudentUserProcedure> StudentUserProcedures { get; set; }
        public DbSet<StudentUserProcedureDetail> StudentUserProcedureDetails { get; set; }


        #endregion

        #region ECONOMIC MANAGEMENT

        public DbSet<BalanceTransfer> BalanceTransfers { get; set; }
        public DbSet<CashierDependency> CashierDependencies { get; set; }
        public DbSet<Classifier> Classifiers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<CreditNote> CreditNotes { get; set; }
        public DbSet<CreditNoteDetail> CreditNoteDetails { get; set; }
        public DbSet<Concept> Concepts { get; set; }
        public DbSet<ConceptHistory> ConceptHistories { get; set; }
        public DbSet<ConceptDistribution> ConceptDistributions { get; set; }
        public DbSet<ConceptDistributionDetail> ConceptDistributionDetails { get; set; }
        public DbSet<Debt> Debts { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenditureProvision> ExpenditureProvisions { get; set; }
        public DbSet<ExpenseOutput> ExpenseOutputs { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderChangeHistory> OrderChanges { get; set; }
        public DbSet<OrderChangeFileHistory> OrderChangeFiles { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<RequirementFile> RequirementFiles { get; set; }
        public DbSet<RequirementSupplier> RequirementSuppliers { get; set; }
        public DbSet<SerialNumber> SerialNumbers { get; set; }
        public DbSet<Settlement> Settlements { get; set; }
        public DbSet<SettlementDetail> SettlementDetails { get; set; }
        public DbSet<SiafExpense> SiafExpenses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierCategory> SupplierCategories { get; set; }
        public DbSet<UserRequirement> UserRequirements { get; set; }
        public DbSet<UserRequirementFile> UserRequirementFiles { get; set; }
        public DbSet<StructureForExpense> StructureForExpenses { get; set; }
        public DbSet<StructureForIncome> StructureForIncomes { get; set; }
        public DbSet<BudgetFramework> BudgetFrameworks { get; set; }
        public DbSet<BudgetBalance> BudgetBalances { get; set; }
        public DbSet<PhaseRequirement> PhaseRequirements { get; set; }
        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CatalogGoal> CatalogGoals { get; set; }
        public DbSet<UserRequirementItem> UserRequirementItems { get; set; }
        public DbSet<CatalogItemGoal> CatalogItemGoals { get; set; }
        public DbSet<AccountingPlan> AccountingPlans { get; set; }
        public DbSet<CurrentAccount> CurrentAccounts { get; set; }
        public DbSet<ExecuteObservation> ExecuteObservations { get; set; }
        public DbSet<BankDeposit> BankDeposits { get; set; }
        public DbSet<PettyCashBook> PettyCashBooks { get; set; }
        public DbSet<CatalogActivity> CatalogActivities { get; set; }
        public DbSet<BankTemporalPayment> BankTemporalPayments { get; set; }
        public DbSet<UserCurrentAccount> UserCurrentAccounts { get; set; }
        public DbSet<MonthlyBalance> MonthlyBalances { get; set; }
        public DbSet<CardPaymentHeader> CardPaymentHeaders { get; set; }
        public DbSet<CardPaymentDetail> CardPaymentDetails { get; set; }
        public DbSet<ExoneratePayment> ExoneratePayments { get; set; }
        public DbSet<ReceivedOrder> ReceivedOrders { get; set; }
        public DbSet<ReceivedOrderHistory> ReceivedOrderHistories { get; set; }
        public DbSet<InternalOutput> InternalOutputs { get; set; }
        public DbSet<InternalOutputItem> InternalOutputItems { get; set; }
        public DbSet<Heritage> Heritages { get; set; }
        public DbSet<PaymentToValidate> PaymentToValidates { get; set; }
        public DbSet<ConceptGroup> ConceptGroups { get; set; }
        public DbSet<ConceptGroupDetail> ConceptGroupDetails { get; set; }

        #endregion

        #region ENROLLMENT

        public DbSet<SupportChat> SupportChats { get; set; }
        public DbSet<SupportMessage> SupportMessages { get; set; }
        public DbSet<EnrollmentSurveyStudent> EnrollmentSurveyStudents { get; set; }
        public DbSet<AcademicCalendarDate> AcademicCalendarDates { get; set; }
        public DbSet<AcademicYearCourse> AcademicYearCourses { get; set; }
        public DbSet<AcademicYearCoursePreRequisite> AcademicYearCoursePreRequisites { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<CalificationCriteria> CalificactionCriterias { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<CampusCareer> CampusCareers { get; set; }
        public DbSet<CareerEnrollmentShift> CareerEnrollmentShifts { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<ClassroomType> ClassroomTypes { get; set; }
        public DbSet<CompetenceEvaluation> CompetenceEvaluations { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseEquivalence> CourseEquivalences { get; set; }
        public DbSet<CurriculumEquivalence> CurriculumEquivalences { get; set; }
        public DbSet<CourseRecognition> CoursesRecognition { get; set; }
        public DbSet<CourseSyllabus> CourseSyllabus { get; set; }
        public DbSet<CourseSyllabusObservation> CourseSyllabusObservations { get; set; }
        public DbSet<CourseSyllabusWeek> CourseSyllabusWeeks { get; set; }
        public DbSet<CourseTerm> CourseTerms { get; set; }
        public DbSet<CourseType> CourseTypes { get; set; }
        public DbSet<CourseUnit> CourseUnits { get; set; }
        public DbSet<Curriculum> Curriculums { get; set; }
        public DbSet<CurriculumArea> CurriculumAreas { get; set; }
        public DbSet<ElectiveCourse> ElectiveCourses { get; set; }
        public DbSet<EnrollmentReservation> EnrollmentReservations { get; set; }
        public DbSet<EnrollmentShift> EnrollmentShifts { get; set; }
        public DbSet<EnrollmentTurn> EnrollmentTurns { get; set; }
        public DbSet<EnrollmentTurnHistory> EnrollmentTurnHistories { get; set; }
        public DbSet<EntrantEnrollment> EntrantEnrollments { get; set; }
        public DbSet<FacultyCurriculumArea> FacultyCurriculumAreas { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<SectionEvaluation> SectionEvaluations { get; set; }
        public DbSet<EvaluationType> EvaluationTypes { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Resolution> Resolutions { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<SectionCode> SectionCodes { get; set; }
        public DbSet<StudentSection> StudentSections { get; set; }
        public DbSet<TeacherSection> TeacherSections { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<TmpEnrollment> TmpEnrollments { get; set; }
        public DbSet<WorkingDay> WorkingDays { get; set; }
        public DbSet<EnrollmentConcept> EnrollmentConcepts { get; set; }
        public DbSet<AdminEnrollment> AdminEnrollments { get; set; }
        public DbSet<AcademicProgramCurriculum> AcademicProgramCurriculums { get; set; }
        public DbSet<Recognition> Recognitions { get; set; }
        public DbSet<RecognitionHistory> RecognitionRecognitionHistories { get; set; }
        public DbSet<CareerParallelCourse> CareerParallelCourses { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<ChannelCareer> ChannelCareers { get; set; }
        public DbSet<SectionGroup> SectionGroups { get; set; }
        public DbSet<DisapprovedCourseConcept> DisapprovedCourseConcepts { get; set; }
        public DbSet<AcademicYearCredit> AcademicYearCredits { get; set; }
        public DbSet<CurriculumCredit> CurriculumCredits { get; set; }
        public DbSet<EnrollmentMessage> EnrollmentMessages { get; set; }
        public DbSet<CourseCertificate> CourseCertificates { get; set; }
        public DbSet<AcademicYearCourseCertificate> AcademicYearCourseCertificates { get; set; }
        public DbSet<StudentCourseCertificate> StudentCourseCertificates { get; set; }
        public DbSet<CourseSyllabusTeacher> CourseSyllabusTeachers { get; set; }
        public DbSet<Competencie> Competencies { get; set; }
        public DbSet<CurriculumCompetencie> CurriculumCompetencies { get; set; }
        public DbSet<ExtraCreditConfiguration> ExtraCreditConfigurations { get; set; }
        public DbSet<AverageGradeCreditConfiguration> AverageGradeCreditConfigurations { get; set; }
        public DbSet<EnrollmentFee> EnrollmentFees { get; set; }
        public DbSet<EnrollmentFeeDetail> EnrollmentFeeDetails { get; set; }
        public DbSet<EnrollmentFeeTerm> EnrollmentFeeTerms { get; set; }
        public DbSet<EnrollmentFeeTemplate> EnrollmentFeeTemplates { get; set; }
        public DbSet<EnrollmentFeeDetailTemplate> EnrollmentFeeDetailTemplates { get; set; }
        #endregion

        #region EXTRACURRICULAR

        public DbSet<ExtracurricularArea> ExtracurricularAreas { get; set; }
        public DbSet<ExtracurricularActivity> ExtracurricularActivities { get; set; }
        public DbSet<ExtracurricularActivityStudent> ExtracurricularActivityStudents { get; set; }
        public DbSet<ExtracurricularCourse> ExtracurricularCourses { get; set; }
        public DbSet<ExtracurricularCourseGroup> ExtracurricularCourseGroups { get; set; }
        public DbSet<ExtracurricularCourseGroupAssistance> ExtracurricularCourseGroupAssistances { get; set; }
        public DbSet<ExtracurricularCourseGroupAssistanceStudent> ExtracurricularCourseGroupAssistanceStudents { get; set; }
        public DbSet<ExtracurricularCourseGroupStudent> ExtracurricularCourseGroupStudents { get; set; }

        #endregion

        #region FORUM

        public DbSet<VForum> VForums { get; set; }
        public DbSet<VForumFile> VForumFiles { get; set; }
        public DbSet<VForumChild> VForumChildren { get; set; }
        public DbSet<VForumChildFile> VForumChildFiles { get; set; }

        #endregion

        #region GENERALS
        public DbSet<ExternalUser> ExternalUsers { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<AcademicProgram> AcademicPrograms { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Career> Careers { get; set; }
        public DbSet<CareerHistory> CareerHistories { get; set; }
        public DbSet<Criterion> Criterions { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Dean> Deans { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<District> Districts { get; set; }
        //public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<RecordHistory> RecordHistories { get; set; }
        public DbSet<RecordHistoryObservation> RecordHistoryObservations { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<UIT> UITs { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<LivingCost> LivingCosts { get; set; }
        public DbSet<StudentExtraCareer> StudentExtraCareers { get; set; }
        public DbSet<BeginningAnnouncement> BeginningAnnouncements { get; set; }
        public DbSet<BeginningAnnouncementRole> BeginningAnnouncementRoles { get; set; }
        public DbSet<UserAnnouncement> UserAnnouncements { get; set; }
        public DbSet<EmailManagement> EmailManagements { get; set; }
        public DbSet<CareerAccreditation> CareerAccreditations { get; set; }
        public DbSet<CareerLicensure> CareerLicensures { get; set; }
        public DbSet<YearInformation> YearInformations { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<PortalNew> PortalNews { get; set; }
        public DbSet<UserSuggestion> UserSuggestions { get; set; }
        public DbSet<StudentCondition> StudentConditions { get; set; }
        public DbSet<StudentBenefit> studentBenefits { get; set; }

        public DbSet<ExternalLink> ExternalLinks { get; set; }
        public DbSet<StudentScale> StudentScales { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }


        #endregion

        #region GEO

        public DbSet<Connection> Connections { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<EquipmentReservations> EquipmentReservations { get; set; }
        public DbSet<LaboratoryEquipments> LaboratoryEquipments { get; set; }
        public DbSet<LaboratoryEquipmentLoans> LaboratoryEquipmentLoans { get; set; }
        public DbSet<LaboratoyRequest> LaboratoyRequests { get; set; }
        public DbSet<LaboratoryStudentAssitance> LaboratoryStudentAssitance { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<WorkerPosition> WorkerPositions { get; set; }

        #endregion

        #region HELP DESK

        public DbSet<Incident> Incidents { get; set; }
        public DbSet<IncidentSolution> IncidentSolutions { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<IncidentType> IncidentTypes { get; set; }

        #endregion

        #region INDICATORS

        public DbSet<Indicators> Indicatorses { get; set; }
        public DbSet<ScoreCard> ScoreCards { get; set; }
        public DbSet<IndicatorProcesses> IndicatorProcesses { get; set; }
        public DbSet<IndicatorImprovement> IndicatorImprovements { get; set; }
        public DbSet<ResearchPerYear> ResearchPerYears { get; set; }


        #endregion

        #region INSTITUTIONAL AGENDA

        public DbSet<AgendaEvent> AgendaEvents { get; set; }
        public DbSet<Inscription> Inscriptions { get; set; }
        public DbSet<MenuOption> MenuOptions { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        #endregion

        #region INSTITUTIONAL WELFARE

        public DbSet<InstitutionalWelfareAnswer> InstitutionalWelfareAnswers { get; set; }
        public DbSet<InstitutionalWelfareQuestion> InstitutionalWelfareQuestions { get; set; }
        public DbSet<InstitutionalWelfareRecord> InstitutionalWelfareRecords { get; set; }
        public DbSet<InstitutionalWelfareSection> InstitutionalWelfareSections { get; set; }
        public DbSet<InstitutionalWelfareAnswerByStudent> InstitutionalWelfareAnswerByStudents { get; set; }
        public DbSet<InstitutionalRecordCategorizationByStudent> InstitutionalRecordCategorizationByStudents { get; set; }
        public DbSet<CategorizationLevelHeader> CategorizationLevelHeaders { get; set; }
        public DbSet<MedicalDiagnostic> MedicalDiagnostics { get; set; }
        public DbSet<InstitutionalWelfareUserProduct> InstitutionalWelfareUserProducts { get; set; }
        public DbSet<InstitutionalWelfareProduct> InstitutionalWelfareProducts { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<InstitutionalWelfareScholarshipType> InstitutionalWelfareScholarshipTypes { get; set; }
        public DbSet<InstitutionalWelfareScholarship> InstitutionalWelfareScholarships { get; set; }
        public DbSet<InstitutionalWelfareScholarshipFormat> InstitutionalWelfareScholarshipFormats { get; set; }
        public DbSet<InstitutionalWelfareScholarshipRequirement> InstitutionalWelfareScholarshipRequirements { get; set; }
        public DbSet<ScholarshipStudent> ScholarshipStudents { get; set; }
        public DbSet<ScholarshipStudentRequirement> ScholarshipStudentRequirements { get; set; }

        public DbSet<WelfareConvocation> WelfareConvocations { get; set; }
        public DbSet<WelfareConvocationRequirement> WelfareConvocationRequirements { get; set; }
        public DbSet<WelfareConvocationFormat> WelfareConvocationFormats { get; set; }
        public DbSet<WelfareConvocationPostulant> WelfareConvocationPostulants { get; set; }
        public DbSet<WelfareConvocationPostulantFile> WelfareConvocationPostulantFiles { get; set; }

        public DbSet<CafobeRequest> CafobeRequests { get; set; }
        public DbSet<CafobeRequestDetail> CafobeRequestDetails { get; set; }

        public DbSet<Disability> Disabilities { get; set; }

        public DbSet<WelfareAlert> WelfareAlerts { get; set; }

        #endregion

        #region INTEREST GROUP
        public DbSet<InterestGroup> InterestGroups { get; set; }
        public DbSet<InterestGroupFile> InterestGroupFiles { get; set; }
        public DbSet<InterestGroupUser> InterestGroupUsers { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingFile> MeetingFiles { get; set; }
        public DbSet<MeetingUser> MeetingUsers { get; set; }
        public DbSet<InterestGroupForum> InterestGroupForums { get; set; }
        public DbSet<InterestGroupSurvey> InterestGroupSurveys { get; set; }
        public DbSet<MeetingCriterion> MeetingCriterions { get; set; }
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<ConferenceUser> ConferenceUsers { get; set; }
        public DbSet<InterestGroupExternalUser> InterestGroupExternalUsers { get; set; }
        public DbSet<InterestGroupUserRefreshToken> InterestGroupUserRefreshTokens { get; set; }

        #endregion

        #region INTRANET
        public DbSet<AcademicRecordDepartment> AcademicRecordDepartments { get; set; }
        public DbSet<AcademicHistoryDocument> AcademicHistoryDocuments { get; set; }
        public DbSet<AcademicHistory> AcademicHistories { get; set; }
        public DbSet<AcademicSummary> AcademicSummaries { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<AnswerByUser> AnswerByUsers { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignPerson> CampaignPersons { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassReschedule> ClassReschedules { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; }
        public DbSet<ClassStudent> ClassStudents { get; set; }
        public DbSet<ClinicHistory> ClinicHistories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCareer> EventCareers { get; set; }
        public DbSet<EventCertification> EventCertifications { get; set; }
        public DbSet<EventEvidence> EventEvidences { get; set; }
        public DbSet<EventFile> EventFiles { get; set; }
        public DbSet<EventRole> EventRoles { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<ExternalPerson> ExternalPersons { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<ForumCareer> ForumCareers { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<GradeCorrection> GradeCorrections { get; set; }
        public DbSet<InstitutionalAlert> InstitutionalAlerts { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<InvoiceVoucher> InvoiceVouchers { get; set; }
        public DbSet<MedicalAppointment> MedicalAppointments { get; set; }
        public DbSet<HistoricalReferredAppointment> HistoricalReferredAppointments { get; set; }
        public DbSet<MedicalConsultReason> MedicalConsultReasons { get; set; }
        public DbSet<MedicalDiagnosticImpression> MedicalDiagnosticImpressions { get; set; }
        public DbSet<MedicalFamilyHistory> MedicalFamilyHistories { get; set; }
        public DbSet<MedicalObservation> MedicalObservations { get; set; }
        public DbSet<MedicalPersonalHistory> MedicalPersonalHistories { get; set; }
        public DbSet<NutritionalRecord> NutritionalRecords { get; set; }
        public DbSet<PettyCash> PettyCash { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PsychologicalDiagnostic> PsychologicalDiagnostics { get; set; }
        public DbSet<PsychologicalRecord> PsychologicalRecords { get; set; }
        public DbSet<PsychologicRecordDiagnostic> PsychologicRecordDiagnostics { get; set; }
        public DbSet<PsychologyCategory> PsychologyCategories { get; set; }
        public DbSet<PsychologyTestExam> PsychologyTestExams { get; set; }
        public DbSet<PsychologyTestQuestion> PsychologyTestQuestions { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<RolAnnouncement> RolAnnouncements { get; set; }
        public DbSet<StudentAbsenceJustification> StudentAbsenceJustifications { get; set; }
        public DbSet<StudentFamily> StudentFamilies { get; set; }
        public DbSet<StudentGroup> StudentGroups { get; set; }
        public DbSet<StudentInformation> StudentInformations { get; set; }
        public DbSet<SubstituteExam> SubstituteExams { get; set; }
        public DbSet<SubstituteExamDetail> SubstituteExamDetails { get; set; }
        public DbSet<Survey> Survey { get; set; }
        public DbSet<SurveyItem> SurveyItems { get; set; }
        public DbSet<SurveyUser> SurveyUsers { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<TopicConsult> TopicConsults { get; set; }
        public DbSet<Tutorial> Tutorials { get; set; }
        public DbSet<TutorialStudent> TutorialStudents { get; set; }
        public DbSet<UserAbsenceJustification> UserAbsenceJustifications { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<StudentObservation> StudentObservations { get; set; }
        public DbSet<EvaluationReport> EvaluationReports { get; set; }
        public DbSet<GradeRegistration> GradeRegistrations { get; set; }
        public DbSet<DeferredExam> DeferredExams { get; set; }
        public DbSet<DeferredExamStudent> DeferredExamStudents { get; set; }
        public DbSet<DirectedCourse> DirectedCourses { get; set; }
        public DbSet<DirectedCourseStudent> DirectedCourseStudents { get; set; }
        public DbSet<DisapprovedCourse> DisapprovedCourses { get; set; }
        public DbSet<GradeReport> GradeReports { get; set; }
        public DbSet<DigitizedSignature> DigitizedSignatures { get; set; }
        public DbSet<UniversityAuthority> UniversityAuthorities { get; set; }
        public DbSet<UniversityAuthorityHistory> UniversityAuthorityHistory { get; set; }
        public DbSet<SuggestionAndTip> SuggestionAndTips { get; set; }
        public DbSet<WelfareCategory> WelfareCategories { get; set; }
        public DbSet<CategorizationLevel> CategorizationLevels { get; set; }
        public DbSet<DeanFaculty> DeanFaculties { get; set; }
        public DbSet<GradeReportRequirement> GradeReportRequirements { get; set; }
        public DbSet<RecordConcept> RecordConcepts { get; set; }
        public DbSet<SectionsDuplicateContent> SectionsDuplicateContents { get; set; }
        public DbSet<ExtraordinaryEvaluation> ExtraordinaryEvaluations { get; set; }
        public DbSet<ExtraordinaryEvaluationStudent> ExtraordinaryEvaluationStudents { get; set; }
        public DbSet<ExtraordinaryEvaluationCommittee> ExtraordinaryEvaluationCommittees { get; set; }
        public DbSet<GradeRecovery> GradeRecoveries { get; set; }
        public DbSet<GradeRecoveryExam> GradeRecoveryExams { get; set; }
        public DbSet<StudentIncomeScore> StudentIncomeScores { get; set; }
        public DbSet<TemporalGrade> TemporalGrades { get; set; }
        public DbSet<WeeklyAttendanceReport> WeeklyAttendanceReports { get; set; }
        public DbSet<ExamWeek> ExamWeeks { get; set; }
        //public DbSet<DoctorSpecialty> DoctorSpecialties { get; set; }
        public DbSet<GradeRectification> GradeRectifications { get; set; }
        public DbSet<SubstituteExamCorrection> SubstituteExamCorrections { get; set; }
        public DbSet<StudentPortfolio> StudentPortfolios { get; set; }
        public DbSet<AKDEMIC.ENTITIES.Models.Intranet.DocumentFormat> DocumentFormats { get; set; }
        public DbSet<StudentGradeChangeHistory> StudentGradeChangeHistories { get; set; }
        public DbSet<StudentPortfolioType> StudentPortfolioTypes { get; set; }

        public DbSet<CorrectionExam> CorrectionExams { get; set; }
        public DbSet<CorrectionExamStudent> CorrectionExamStudents { get; set; }
        //seteamos la clase 
        public DbSet<TrainingCurse> TrainingCurses { get; set; }


        #endregion

        #region CONTINUING EDUCATION

        public DbSet<ENTITIES.Models.ContinuingEducation.Activity> FormationActivities { get; set; }
        public DbSet<ENTITIES.Models.ContinuingEducation.ActivityType> FormationActivityTypes { get; set; }
        public DbSet<ENTITIES.Models.ContinuingEducation.Course> FormationCourses { get; set; }
        public DbSet<ENTITIES.Models.ContinuingEducation.Section> FormationSections { get; set; }
        public DbSet<ENTITIES.Models.ContinuingEducation.CourseArea> FormationCourseAreas { get; set; }
        public DbSet<ENTITIES.Models.ContinuingEducation.CourseType> FormationCourseTypes { get; set; }
        public DbSet<ENTITIES.Models.ContinuingEducation.CourseExhibitor> FormationCourseExhibitors { get; set; }
        public DbSet<ENTITIES.Models.ContinuingEducation.RegisterSection> FormationRegisterSections { get; set; }

        #endregion


        #region EVALUATION

        public DbSet<CulturalActivity> CulturalActivities { get; set; }
        public DbSet<CulturalActivityFile> CulturalActivityFiles { get; set; }
        public DbSet<RegisterCourseConference> RegisterCourseConferences { get; set; }
        public DbSet<RegisterCulturalActivity> RegisterCulturalActivities { get; set; }
        public DbSet<ProjectReport> ProjectReports { get; set; }
        public DbSet<ProjectSustainableDevelopmentGoal> ProjectSustainableDevelopmentGoals { get; set; }
        public DbSet<SustainableDevelopmentGoal> SustainableDevelopmentGoals { get; set; }
        public DbSet<ProjectConfiguration> ProjectConfigurations { get; set; }
        public DbSet<ENTITIES.Models.Evaluation.Project> EvaluationProjects { get; set; }
        public DbSet<ENTITIES.Models.Evaluation.Template> EvaluationTemplates { get; set; }
        public DbSet<ENTITIES.Models.Evaluation.CourseConference> CourseConferences { get; set; }
        public DbSet<ENTITIES.Models.Evaluation.CourseConferenceExhibitor> CourseConferenceExhibitors { get; set; }
        public DbSet<ENTITIES.Models.Evaluation.ProjectMember> EvaluationProjectMembers { get; set; }
        public DbSet<ENTITIES.Models.Evaluation.ProjectAdvance> EvaluationProjectAdvances { get; set; }
        public DbSet<ENTITIES.Models.Evaluation.CompanyInterest> EvaluationCompanyInterests { get; set; }
        public DbSet<ENTITIES.Models.Evaluation.ProjectActivity> EvaluationProjectActivities { get; set; }
        public DbSet<ENTITIES.Models.Evaluation.ProjectEvaluator> EvaluationProjectEvaluators { get; set; }
        public DbSet<ENTITIES.Models.Evaluation.SabaticalRequest> EvaluationSabaticalRequests { get; set; }
        public DbSet<ENTITIES.Models.Evaluation.ProjectAdvanceHistoric> EvaluationProjectAdvanceHistorics { get; set; }
        public DbSet<ENTITIES.Models.Evaluation.EvaluationArea> EvaluationAreas { get; set; }
        public DbSet<ENTITIES.Models.Evaluation.CulturalActivityType> CulturalActivityTypes { get; set; }
        public DbSet<ENTITIES.Models.Evaluation.ProjectScheduleHistoric> EvaluationProjectScheduleHistorics { get; set; }

        #endregion

        #region INVESTIGATION
        public DbSet<ENTITIES.Models.Investigation.LockedUser> LockedUsers { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ResearchLineHistoric> ResearchLineHistorics { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ProjectSchedule> ProjectSchedules { get; set; }
        public DbSet<ENTITIES.Models.Investigation.UserResearchLine> UserResearchLines { get; set; }
        public DbSet<ENTITIES.Models.Investigation.UserResearchArea> UserResearchAreas { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ResearchArea> ResearchAreas { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ResearchLine> ResearchLines { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ResearchSubArea> ResearchSubAreas { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ResearchCategory> ResearchCategories { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ResearchDiscipline> ResearchDisciplines { get; set; }
        public DbSet<ENTITIES.Models.Investigation.Project> InvestigationProjects { get; set; }
        public DbSet<ENTITIES.Models.Investigation.Template> InvestigationTemplates { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ProjectRubric> InvestigationProjectRubrics { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ProjectMember> InvestigationProjectMembers { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ProjectAdvance> InvestigationProjectAdvances { get; set; }
        public DbSet<ENTITIES.Models.Investigation.CompanyInterest> InvestigationCompanyInterests { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ProjectActivity> InvestigationProjectActivities { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ProjectItemScore> InvestigationProjectItemScores { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ProjectEvaluator> InvestigationProjectEvaluators { get; set; }
        public DbSet<ENTITIES.Models.Investigation.SabaticalRequest> InvestigationSabaticalRequests { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ProjectRubricItem> InvestigationProjectRubricItems { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ProjectAdvanceHistoric> InvestigationProjectAdvanceHistorics { get; set; }


        public DbSet<ENTITIES.Models.Investigation.Activity> InvestigationActivities { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ActivityFile> InvestigationActivityFiles { get; set; }
        public DbSet<ENTITIES.Models.Investigation.ActivityType> InvestigationActivityTypes { get; set; }
        public DbSet<ENTITIES.Models.Investigation.StudentActivity> InvestigationStudentActivities { get; set; }
        public DbSet<ENTITIES.Models.Investigation.StudentActivityFile> InvestigationStudentActivityFiles { get; set; }
        public DbSet<ENTITIES.Models.Investigation.StudentFeedbackActivity> InvestigationStudentFeedbackActivities { get; set; }
        public DbSet<ENTITIES.Models.Investigation.StudentFeedbackActivityFile> InvestigationStudentFeedbackActivityFiles { get; set; }

        #endregion

        #region JOB EXCHANGE

        public DbSet<Ability> Abilities { get; set; }
        public DbSet<Agreement> Agreements { get; set; }
        public DbSet<ChannelContact> ChannelContacts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<AgreementFormat> AgreementFormats { get; set; }
        public DbSet<AgreementTemplate> AgreementTemplates { get; set; }
        public DbSet<CurriculumVitae> CurriculumVitaes { get; set; }
        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<ForumTopic> ForumTopics { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<JobOfferAbility> JobOfferAbilities { get; set; }
        public DbSet<JobOfferApplication> JobOfferApplications { get; set; }
        public DbSet<JobOfferCareer> JobOfferCareers { get; set; }
        public DbSet<JobOfferLanguage> JobOfferLanguages { get; set; }
        public DbSet<ImageCompany> ImageCompanies { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Sede> Sedes { get; set; }
        public DbSet<StudentAbility> StudentAbilities { get; set; }
        public DbSet<StudentAcademicEducation> StudentAcademicEducations { get; set; }
        public DbSet<StudentCertificate> StudentCertificates { get; set; }
        public DbSet<StudentExperience> StudentExperiences { get; set; }
        public DbSet<StudentLanguage> StudentLanguages { get; set; }
        public DbSet<CoordinatorCareer> CoordinatorCareers { get; set; }
        public DbSet<EmployeeSurvey> EmployeeSurveys { get; set; }
        public DbSet<ENTITIES.Models.JobExchange.InternshipRequest> InternshipRequests { get; set; }
        public DbSet<CompanySize> CompanySizes { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<EconomicActivity> EconomicActivities { get; set; }
        public DbSet<DidacticalMaterial> DidacticalMaterials { get; set; }
        public DbSet<DidacticalMaterialFile> DidacticalMaterialFiles { get; set; }
        public DbSet<StudentComplementaryStudy> StudentComplementaryStudies { get; set; }
        public DbSet<StudentSpecialityCertificate> StudentSpecialityCertificates { get; set; }
        public DbSet<StudentGraduatedSurvey> StudentGraduatedSurveys { get; set; }
        public DbSet<EconomicActivityDivision> EconomicActivityDivisions { get; set; }
        public DbSet<EconomicActivitySection> EconomicActivitySections { get; set; }

        public DbSet<FavoriteCompany> FavoriteCompanies { get; set; }

        #endregion

        #region KARDEX

        public DbSet<Category> Categories { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInput> ProductInputs { get; set; }
        public DbSet<ProductOutput> GetProductOutputs { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        #endregion

        #region LANGUAGE CENTER

        public DbSet<LanguageAcademicHistory> LanguageAcademicHistories { get; set; }
        public DbSet<LanguageCourse> LanguageCourses { get; set; }
        public DbSet<LanguageLevel> LanguageLevels { get; set; }
        public DbSet<LanguagePayment> LanguagePayments { get; set; }
        public DbSet<LanguagePaymentUserProcedure> LanguagePaymentUserProcedures { get; set; }
        public DbSet<LanguageQualification> LanguageQualifications { get; set; }
        public DbSet<LanguageSection> LanguageSections { get; set; }
        public DbSet<LanguageSectionSchedule> LanguageSectionSchedules { get; set; }
        public DbSet<LanguageSectionStudent> LanguageSectionStudents { get; set; }
        public DbSet<LanguageStudentAssistance> LanguageStudentAssistances { get; set; }
        public DbSet<LanguageStudentQualification> LanguageStudentQualifications { get; set; }

        #endregion

        #region LAURASSIA

        #region HOMEWORK

        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<HomeworkStudent> HomeworkStudents { get; set; }
        public DbSet<RubricItem> RubricItems { get; set; }
        public DbSet<RubricItemStudent> RubricItemStudents { get; set; }
        public DbSet<HomeworkFile> HomeworkFiles { get; set; }
        public DbSet<HomeworkStudentFile> HomeworkStudentFiles { get; set; }

        #endregion

        public DbSet<Contest> Contests { get; set; }
        public DbSet<ContestRequirement> ContestRequirements { get; set; }
        public DbSet<ContestStudent> ContestStudents { get; set; }
        public DbSet<ContestStudentRequirement> ContestStudentRequirements { get; set; }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<New> News { get; set; }
        public DbSet<Wiki> Wikis { get; set; }
        //public DbSet<Login> Logins { get; set; }
        public DbSet<Manual> Manuals { get; set; }
        public DbSet<VGroup> VGroups { get; set; }
        public DbSet<Consult> Consults { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Reading> Readings { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<VGroupFile> VGroupFiles { get; set; }
        public DbSet<VGroupUser> VGroupUsers { get; set; }
        public DbSet<ConsultType> ConsultTypes { get; set; }
        public DbSet<CourseGuide> CourseGuides { get; set; }
        //public DbSet<LoginDetail> LoginDetails { get; set; }
        public DbSet<ExamSegment> ExamSegments { get; set; }
        public DbSet<VirtualClass> VirtualClass { get; set; }
        public DbSet<VMessageGroup> VMessageGroups { get; set; }
        public DbSet<SectionSchedule> SectionSchedules { get; set; }
        public DbSet<RubricItemDetail> RubricItemDetails { get; set; }
        public DbSet<QualificationLog> QualificationLogs { get; set; }
        public DbSet<SupervisorCareer> SupervisorCareers { get; set; }
        public DbSet<SupervisorDepartment> SupervisorDepartments { get; set; }
        public DbSet<FrequentQuestion> FrequentQuestions { get; set; }
        public DbSet<VirtualClassDetail> VirtualClassDetails { get; set; }
        public DbSet<OtherQualification> OtherQualifications { get; set; }
        public DbSet<GeneralAnnouncement> GeneralAnnouncements { get; set; }
        public DbSet<SectionAnnouncement> SectionAnnouncements { get; set; }
        public DbSet<VirtualClassStudent> VirtualClassStudents { get; set; }
        public DbSet<FrequentQuestionLink> FrequentQuestionLinks { get; set; }
        public DbSet<VirtualClassRecording> VirtualClassRecordings { get; set; }
        public DbSet<VirtualClassCredential> VirtualClassCredentials { get; set; }
        public DbSet<OtherQualificationStudent> OtherQualificationStudents { get; set; }

        public DbSet<RecoveryHomeworkStudent> RecoveryHomeworkStudents { get; set; }
        public DbSet<HomeworkStudentFeedbackFile> HomeworkStudentFeedbackFiles { get; set; }
        public DbSet<QuestionRubric> QuestionRubrics { get; set; }
        public DbSet<QuestionRubricDetail> QuestionRubricDetails { get; set; }
        public DbSet<QuestionRubricStudent> QuestionRubricStudents { get; set; }
        public DbSet<VirtualClassLog> VirtualClassLogs { get; set; }
        public DbSet<Certificate> Certificates { get; set; }


        #region VEXAM

        public DbSet<Element> Elements { get; set; }
        public DbSet<ExamResolution> ExamResolutions { get; set; }
        public DbSet<VExam> VExams { get; set; }
        public DbSet<VExamDetail> VExamDetails { get; set; }
        public DbSet<VExamFeedback> VExamFeedbacks { get; set; }
        public DbSet<VExamStudent> VExamStudents { get; set; }
        public DbSet<VQuestion> VQuestions { get; set; }
        public DbSet<RecoveryExamStudent> RecoveryExamStudents { get; set; }

        #endregion

        #endregion

        #region OPENID

        //public DbSet<OpenIddictApplication> OpenIddictApplications { get; set; }
        //public DbSet<Application> Applications { get; set; }
        //public DbSet<ApplicationUserAuthorized> ApplicationUserAuthorized { get; set; }
        //public DbSet<OpenIddictScope> OpenIddictScopes { get; set; }
        //public DbSet<OpenIddictToken> OpenIddictTokens { get; set; }

        #endregion

        #region PAYROLL

        public DbSet<Bank> Banks { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PayrollClass> PayrollClasses { get; set; }
        public DbSet<PayrollClassWageItemFormula> PayrollClassWageItemFormulas { get; set; }
        public DbSet<PayrollType> PayrollTypes { get; set; }
        public DbSet<PayrollWorker> PayrollWorkers { get; set; }
        public DbSet<PayrollWorkerWageItem> PayrollWorkerWageItems { get; set; }
        public DbSet<WageItem> WageItems { get; set; }
        public DbSet<WorkArea> WorkAreas { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<WorkerHistory> WorkerHistories { get; set; }
        public DbSet<WorkerOcupation> WorkerOcupations { get; set; }
        public DbSet<WorkingTerm> WorkingTerms { get; set; }
        public DbSet<AdministrativeTable> AdministrativeTables { get; set; }
        public DbSet<PayrollConcept> PayrollConcepts { get; set; }
        public DbSet<WageLevel> WageLevels { get; set; }

        public DbSet<WorkShift> WorkShifts { get; set; }

        public DbSet<RemunerationMaintenance> RemunerationMaintenances { get; set; }

        public DbSet<EmployerMaintenance> EmployerMaintenances { get; set; }

        public DbSet<WorkerRemuneration> WorkerRemunerations { get; set; }

        public DbSet<WorkerTermPayrollDetail> WorkerTermPayrollDetails { get; set; }
        public DbSet<RemunerationPayrollType> RemunerationPayrollTypes { get; set; }

        public DbSet<ConceptType> ConceptTypes { get; set; }

        #endregion

        #region PERMISSION

        public DbSet<RolePermission> RolePermission { get; set; }

        #endregion

        #region PORTAL 

        public DbSet<FinancialExecution> FinancialExecutions { get; set; }
        public DbSet<FinancialExecutionDetail> FinancialExecutionDetails { get; set; }
        public DbSet<InstitutionalActivity> InstitutionalActivities { get; set; }
        public DbSet<InstitutionalActivityFile> InstitutionalActivityFiles { get; set; }
        public DbSet<FinancialStatement> FinancialStatements { get; set; }
        public DbSet<FinancialStatementFile> FinancialStatementFiles { get; set; }
        public DbSet<SessionRecord> SessionRecords { get; set; }
        public DbSet<SessionRecordFile> SessionRecordFiles { get; set; }
        public DbSet<TransparencyCompetition> TransparencyCompetitions { get; set; }
        public DbSet<TransparencyPortalGeneral> TransparencyPortalGenerals { get; set; }
        public DbSet<TransparencyPortalRegulation> TransparencyPortalRegulations { get; set; }
        public DbSet<TransparencyPublicInformation> TransparencyPublicInformation { get; set; }
        public DbSet<TransparencyPublicInformationFile> TransparencyPublicInformationFiles { get; set; }
        public DbSet<TransparencyResearchProject> TransparencyResearchProjects { get; set; }
        public DbSet<TransparencyResearchProjectFile> TransparencyResearchProjectFiles { get; set; }
        public DbSet<TransparencyCompetitionFile> TransparencyCompetitionFiles { get; set; }
        public DbSet<TransparencyConciliationAct> TransparencyConciliationActs { get; set; }
        public DbSet<TransparencySelectionCommittee> TransparencySelectionCommittees { get; set; }
        public DbSet<TransparencyServiceOrder> TransparencyServiceOrders { get; set; }
        public DbSet<TransparencyAppliedPenalty> TransparencyAppliedPenalties { get; set; }
        public DbSet<TransparencyEmployee> TransparencyEmployees { get; set; }
        public DbSet<TransparencyAdvertising> TransparencyAdvertisings { get; set; }
        public DbSet<TransparencyVisitsRecord> TransparencyVisitsRecords { get; set; }
        public DbSet<TransparencyTelephone> TransparencyTelephones { get; set; }
        public DbSet<TransparencyVehicle> TransparencyVehicles { get; set; }
        public DbSet<TransparencyTravelPassage> TransparencyTravelPassages { get; set; }
        public DbSet<TransparencySubMenu> TransparencySubMenus { get; set; }
        public DbSet<TransparencySubMenuFile> TransparencySubMenuFiles { get; set; }
        public DbSet<TransparencyScholarship> TransparencyScholarships { get; set; }
        public DbSet<Directive> Directives { get; set; }
        public DbSet<TransparencyPortalGeneralFile> TransparencyPortalGeneralFiles { get; set; }
        public DbSet<TransparencyManagementDocument> TransparencyManagementDocuments { get; set; }
        public DbSet<TransparencyPortalInterestLink> TransparencyPortalInterestLinks { get; set; }

        #endregion

        #region PREUNIVERSITARY

        public DbSet<PreuniversitaryAssistance> PreuniversitaryAssistances { get; set; }
        public DbSet<PreuniversitaryAssistanceStudent> PreuniversitaryAssistanceStudents { get; set; }
        public DbSet<PreuniversitaryCourse> PreuniversitaryCourses { get; set; }
        public DbSet<PreuniversitaryGroup> PreuniversitaryGroups { get; set; }
        public DbSet<PreuniversitaryPostulant> PreuniversitaryPostulants { get; set; }
        public DbSet<PreuniversitarySchedule> PreuniversitarySchedules { get; set; }
        public DbSet<PreuniversitaryTemary> PreuniversitaryTemaries { get; set; }
        public DbSet<PreuniversitaryTerm> PreuniversitaryTerms { get; set; }
        public DbSet<PreuniversitaryUserGroup> PreuniversitaryUserGroups { get; set; }
        public DbSet<IncubationCall> IncubationCalls { get; set; }
        public DbSet<PreuniversitaryChannel> PreuniversitaryChannels { get; set; }
        public DbSet<PreuniversitaryExam> PreuniversitaryExams { get; set; }
        public DbSet<PreuniversitaryExamClassroom> PreuniversitaryExamClassrooms { get; set; }
        public DbSet<PreuniversitaryExamTeacher> PreuniversitaryExamTeachers { get; set; }
        public DbSet<PreuniversitaryExamClassroomPostulant> PreuniversitaryExamClassroomPostulants { get; set; }


        #endregion

        #region QUESTION

        public DbSet<Alternative> Alternatives { get; set; }
        public DbSet<Calculated> Calculateds { get; set; }
        public DbSet<CalculatedAlternative> CalculatedAlternatives { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<GroupChoice> GroupChoices { get; set; }
        public DbSet<Number> Numbers { get; set; }
        public DbSet<SubImage> SubImages { get; set; }
        public DbSet<SubInput> SubInputs { get; set; }
        public DbSet<SubQuestion> SubQuestions { get; set; }
        public DbSet<Variable> Variables { get; set; }

        #endregion

        #region RESERVATIONS

        public DbSet<ENTITIES.Models.Reservations.Environment> Environments { get; set; }
        public DbSet<EnvironmentSchedule> EnvironmentSchedules { get; set; }
        public DbSet<EnvironmentReservation> EnvironmentReservations { get; set; }
        public DbSet<TeachingHoursRange> TeachingHoursRanges { get; set; }

        #endregion

        #region RESOLUTIVE ACTS

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentFile> DocumentFiles { get; set; }
        public DbSet<Sorter> Sorters { get; set; }
        public DbSet<ResolutionCategory> ResolutionCategories { get; set; }

        #endregion

        #region SCALE

        public DbSet<AcademicDepartment> AcademicDepartments { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<PrivateManagementPensionFund> PrivateManagementPensionFunds { get; set; }
        public DbSet<ScaleExtraBenefitField> ScaleExtraBenefitFields { get; set; }
        public DbSet<BenefitType> BenefitTypes { get; set; }
        public DbSet<ScaleExtraContractField> ScaleExtraContractFields { get; set; }
        public DbSet<ScaleExtraDemeritField> ScaleExtraDemeritFields { get; set; }
        public DbSet<ScaleExtraDisplacementField> ScaleExtraDisplacementFields { get; set; }
        public DbSet<ScaleExtraInstitutionExperienceField> ScaleExtraInstitutionExperienceFields { get; set; }
        public DbSet<ScaleExtraInvestigationField> ScaleExtraInvestigationFields { get; set; }
        public DbSet<InvestigationParticipationType> InvestigationParticipationTypes { get; set; }
        public DbSet<ScaleExtraMeritField> ScaleExtraMeritFields { get; set; }
        public DbSet<ScaleExtraPerformanceEvaluationField> ScaleExtraPerformanceEvaluationFields { get; set; }
        public DbSet<ScaleLicenseAuthorization> ScaleLicenseAuthorizations { get; set; }
        public DbSet<LicenseResolutionType> LicenseResolutionTypes { get; set; }
        public DbSet<ScalePermitAuthorization> ScalePermitAuthorizations { get; set; }
        public DbSet<ScaleResolution> ScaleResolutions { get; set; }
        public DbSet<ScaleResolutionType> ScaleResolutionTypes { get; set; }
        public DbSet<ScaleSection> ScaleSections { get; set; }
        public DbSet<ScaleSectionAnnex> ScaleSectionAnnexes { get; set; }
        public DbSet<ScaleSectionResolutionType> ScaleSectionResolutionTypes { get; set; }
        public DbSet<ScaleVacationAuthorization> ScaleVacationAuthorizations { get; set; }
        public DbSet<WorkerBachelorDegree> WorkerBachelorDegrees { get; set; }
        public DbSet<WorkerCapPosition> WorkerCapPositions { get; set; }
        public DbSet<WorkerDiplomate> WorkerDiplomates { get; set; }
        public DbSet<WorkerDoctoralDegree> WorkerDoctoralDegrees { get; set; }
        public DbSet<WorkerLaborCategory> WorkerLaborCategories { get; set; }
        public DbSet<WorkerLaborCondition> WorkerLaborConditions { get; set; }
        public DbSet<WorkerLaborInformation> WorkerLaborInformation { get; set; }
        public DbSet<WorkerLaborTermInformation> WorkerLaborTermInformations { get; set; }
        public DbSet<WorkerLaborRegime> WorkerLaborRegimes { get; set; }
        public DbSet<WorkerManagementPosition> WorkerManagementPositions { get; set; }
        public DbSet<WorkerMasterDegree> WorkerMasterDegrees { get; set; }
        public DbSet<WorkerOtherStudy> WorkerOtherStudies { get; set; }
        public DbSet<WorkerPersonalDocument> WorkerPersonalDocuments { get; set; }
        public DbSet<WorkerPositionClassification> WorkerPositionClassifications { get; set; }
        public DbSet<WorkerProfessionalSchool> WorkerProfessionalSchools { get; set; }
        public DbSet<WorkerProfessionalTitle> WorkerProfessionalTitles { get; set; }
        public DbSet<WorkerSecondSpecialty> WorkerSecondSpecialties { get; set; }
        public DbSet<WorkerSchoolDegree> WorkerSchoolDegrees { get; set; }
        public DbSet<WorkerTechnicalStudy> WorkerTechnicalStudies { get; set; }
        public DbSet<WorkerBankAccountInformation> WorkerBankAccountInformation { get; set; }
        public DbSet<WorkCertificateRecord> WorkCertificateRecords { get; set; }
        public DbSet<WorkerDina> WorkerDinas { get; set; }
        public DbSet<WorkerDinaSupportExperience> WorkerDinaSupportExperiences { get; set; }
        public DbSet<WorkerRetirementSystemHistory> WorkerRetirementSystemHistories { get; set; }
        public DbSet<WorkerTraining> WorkerTrainings { get; set; }
        public DbSet<ScaleReportProfile> ScaleReportProfiles { get; set; }
        public DbSet<ScaleReportProfileDetail> ScaleReportProfileDetails { get; set; }
        public DbSet<ScaleReportHistory> ScaleReportHistories { get; set; }
        public DbSet<WorkerFamilyInformation> WorkerFamilyInformations { get; set; }


        #endregion

        #region TEACHING HIRING
        public DbSet<ENTITIES.Models.TeacherHiring.Convocation> Convocations { get; set; }
        public DbSet<ConvocationDocument> ConvocationDocuments { get; set; }
        public DbSet<ConvocationAcademicDeparment> ConvocationAcademicDeparments { get; set; }
        public DbSet<ConvocationComitee> ConvocationComitees { get; set; }
        public DbSet<ConvocationCalendar> ConvocationCalendars { get; set; }
        public DbSet<ConvocationSection> ConvocationSections { get; set; }
        public DbSet<ENTITIES.Models.TeacherHiring.ConvocationQuestion> ConvocationQuestions { get; set; }
        public DbSet<ENTITIES.Models.TeacherHiring.ConvocationAnswer> ConvocationAnswers { get; set; }
        public DbSet<ENTITIES.Models.TeacherHiring.ConvocationAnswerByUser> ConvocationAnswerByUsers { get; set; }
        public DbSet<ApplicantTeacher> ApplicantTeachers { get; set; }

        #endregion

        #region TEACHING MANAGEMENT
        public DbSet<AcademicSecretary> AcademicSecretaries { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<CourseComponent> CourseComponents { get; set; }
        public DbSet<DigitalDocument> DigitalDocuments { get; set; }
        public DbSet<DigitalResource> DigitalResources { get; set; }
        public DbSet<DigitalResourceCareer> DigitalResourceCareers { get; set; }
        public DbSet<NonActivity> NonActivities { get; set; }
        public DbSet<ScoreInputSchedule> ScoreInputSchedules { get; set; }
        public DbSet<ScoreInputScheduleDetail> ScoreInputScheduleDetails { get; set; }
        public DbSet<SyllabusRequest> SyllabusRequests { get; set; }
        public DbSet<SyllabusTeacher> SyllabusTeachers { get; set; }
        public DbSet<TeacherAssistance> TeacherAssistance { get; set; }
        public DbSet<TeacherDedication> TeacherDedication { get; set; }
        public DbSet<TeacherExperience> TeacherExperiences { get; set; }
        public DbSet<TeacherInformation> TeacherInformations { get; set; }
        public DbSet<TeacherSchedule> TeacherSchedules { get; set; }
        public DbSet<TeacherTermInform> TeacherTermInforms { get; set; }
        public DbSet<TermInform> TermInforms { get; set; }
        public DbSet<UnitActivity> UnitActivities { get; set; }
        public DbSet<UnitResource> UnitResources { get; set; }
        public DbSet<TeacherSurvey> TeacherSurveys { get; set; }
        public DbSet<NonTeachingLoad> NonTeachingLoads { get; set; }
        public DbSet<TeachingLoadType> TeachingLoadTypes { get; set; }
        public DbSet<TeachingLoadSubType> TeachingLoadSubTypes { get; set; }
        public DbSet<NonTeachingLoadSchedule> NonTeachingLoadSchedules { get; set; }

        public DbSet<NonTeachingLoadDeliverable> NonTeachingLoadDeliverables { get; set; }
        public DbSet<NonTeachingLoadActivity> NonTeachingLoadActivities { get; set; }

        public DbSet<TeacherNonActivityHistorial> TeacherNonActivityHistorials { get; set; }
        public DbSet<TeacherAcademicCharge> TeacherAcademicCharges { get; set; }
        public DbSet<PerformanceEvaluation> PerformanceEvaluations { get; set; }
        public DbSet<PerformanceEvaluationUser> PerformanceEvaluationUsers { get; set; }
        public DbSet<PerformanceEvaluationRatingScale> PerformanceEvaluationRatingScales { get; set; }
        public DbSet<PerformanceEvaluationQuestion> PerformanceEvaluationQuestions { get; set; }
        public DbSet<PerformanceEvaluationResponse> PerformanceEvaluationResponses { get; set; }
        public DbSet<PerformanceEvaluationTemplate> PerformanceEvaluationTemplates { get; set; }
        public DbSet<PerformanceEvaluationRubric> PerformanceEvaluationRubrics { get; set; }
        public DbSet<RelatedPerformanceEvaluationTemplate> RelatedPerformanceEvaluationTemplates { get; set; }
        public DbSet<PerformanceEvaluationCriterion> PerformanceEvaluationCriterions { get; set; }
        public DbSet<ExtraTeachingLoad> ExtraTeachingLoads { get; set; }
        public DbSet<TeacherPortfolio> TeacherPortfolios { get; set; }

        #endregion

        #region TEACHING RESEARCH

        public DbSet<ENTITIES.Models.TeachingResearch.Convocation> TeachingResearchConvocations { get; set; }
        public DbSet<ENTITIES.Models.TeachingResearch.ConvocationAnswer> TeachingResearchConvocationAnswers { get; set; }
        public DbSet<ENTITIES.Models.TeachingResearch.ConvocationAnswerByUser> TeachingResearchConvocationAnswerByUsers { get; set; }
        public DbSet<ENTITIES.Models.TeachingResearch.ConvocationFile> TeachingResearchConvocationFiles { get; set; }
        public DbSet<ENTITIES.Models.TeachingResearch.ConvocationSupervisor> TeachingResearchConvocationSupervisors { get; set; }
        public DbSet<ENTITIES.Models.TeachingResearch.ConvocationPostulant> TeachingResearchPostulants { get; set; }

        #endregion

        #region TUTORING

        public DbSet<SupportOffice> SupportOffices { get; set; }
        public DbSet<SupportOfficeUser> SupportOfficeUsers { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<TutoringAnnouncement> TutoringAnnouncements { get; set; }
        public DbSet<TutoringAnnouncementCareer> TutoringAnnouncementCareers { get; set; }
        public DbSet<TutoringAnnouncementRole> TutoringAnnouncementRoles { get; set; }
        public DbSet<TutoringCoordinator> TutoringCoordinators { get; set; }
        public DbSet<TutoringProblem> TutoringProblems { get; set; }
        public DbSet<TutoringProblemFile> TutoringProblemFiles { get; set; }
        public DbSet<TutoringSession> TutoringSessions { get; set; }
        public DbSet<TutoringSessionStudent> TutoringSessionStudents { get; set; }
        public DbSet<TutoringSessionProblem> TutoringSessionProblems { get; set; }
        public DbSet<TutoringStudent> TutoringStudents { get; set; }
        public DbSet<TutorTutoringStudent> TutorTutoringStudents { get; set; }
        public DbSet<TutoringSuggestion> TutoringSuggestions { get; set; }
        public DbSet<TutorWorkingPlan> TutorWorkingPlans { get; set; }
        public DbSet<TutoringPlan> TutoringPlans { get; set; }
        public DbSet<TutoringPlanHistory> TutoringPlanHistories { get; set; }
        public DbSet<TutoringAttendance> TutoringAttendances { get; set; }
        public DbSet<TutoringAttendanceProblem> TutoringAttendanceProblems { get; set; }
        public DbSet<TutoringMessage> TutoringMessages { get; set; }
        public DbSet<HistoryReferredTutoringStudent> HistoryReferredTutoringStudents { get; set; }
        #endregion

        #region VIRTUAL DIRECTORY      
        public DbSet<DirectoryDependency> DirectoryDependencies { get; set; }
        public DbSet<InstitutionalInformation> InstitutionalInformations { get; set; }

        #endregion

        #region VISIT MANAGEMENT
        public DbSet<Visit> Visits { get; set; }
        public DbSet<VisitorInformation> VisitorInformations { get; set; }
        #endregion

        #region SISCO
        public DbSet<Norm> Norms { get; set; }
        public DbSet<SectionSisco> SectionsSisco { get; set; }
        public DbSet<ENTITIES.Models.Sisco.CourseConference> CoursesConferences { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Novelty> Novelties { get; set; }
        public DbSet<MissionVision> MissionVisions { get; set; }
        public DbSet<Shortcut> Shortcuts { get; set; }
        public DbSet<SubShortcut> SubShortcuts { get; set; }
        #endregion

        #region LIBRARY      
        public DbSet<QuibukAuthorityType> QuibukAuthorityTypes { get; set; }
        public DbSet<QuibukAuthoritie> QuibukAuthorities { get; set; }
        public DbSet<QuibukAuthorityField> QuibukAuthorityFields { get; set; }
        #endregion

        #region PREPROFESIONAL PRACTICE

        public DbSet<ENTITIES.Models.PreprofesionalPractice.InternshipRequest> InternshipValidationRequests { get; set; }
        public DbSet<InternshipDevelopment> InternshipDevelopments { get; set; }
        public DbSet<InternshipAspect> InternshipAspects { get; set; }
        public DbSet<PresentationLetter> PresentationLetters { get; set; }
        public DbSet<WeeklyInternshipProgress> WeeklyInternshipProgress { get; set; }
        public DbSet<WeeklyInternshipProgressDetail> WeeklyInternshipProgressDetails { get; set; }
        public DbSet<InternshipRequestFile> InternshipRequestFiles { get; set; }

        #endregion

        #region SERVER

        public DbSet<GeneralLink> GeneralLinks { get; set; }
        public DbSet<GeneralLinkRole> GeneralLinkRoles { get; set; }

        #endregion

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var databaseType = ConstantHelpers.GENERAL.DATABASES.DATABASE;
            var modelEntityTypes = modelBuilder.Model.GetEntityTypes();

            // https://andrewlock.net/customising-asp-net-core-identity-ef-core-naming-conventions-for-postgresql/
            // https://stackoverflow.com/questions/31534945/change-foreign-key-constraint-naming-convention/
            // https://www.c-sharpcorner.com/article/shadow-properties-in-entity-framework-core/

            foreach (var modelEntityType in modelEntityTypes)
            {
                var propertyClrType = modelEntityType.ClrType;

                if (typeof(ICodeNumber).IsAssignableFrom(propertyClrType))
                {
                    modelBuilder.Invoke(ModelBuilderHelpers.GetMethodInfo("CodeNumberProperty"), propertyClrType);
                    modelBuilder.Invoke(ModelBuilderHelpers.GetMethodInfo("CodeNumberHasAlternateKey"), propertyClrType);
                }

                if (typeof(IKeyNumber).IsAssignableFrom(propertyClrType))
                {
                    modelBuilder.Invoke(ModelBuilderHelpers.GetMethodInfo("KeyNumberProperty"), propertyClrType);
                }

                if (typeof(ISoftDelete).IsAssignableFrom(propertyClrType))
                {
                    modelBuilder.Invoke(ModelBuilderHelpers.GetMethodInfo("SoftDeleteProperty"), propertyClrType);
                    modelBuilder.Invoke(ModelBuilderHelpers.GetMethodInfo("SoftDeleteHasQueryFilter"), propertyClrType);
                }

                if (typeof(ITimestamp).IsAssignableFrom(propertyClrType))
                {
                    modelBuilder.Invoke(ModelBuilderHelpers.GetMethodInfo("TimestampProperty"), propertyClrType);
                }

                if (typeof(ITrackNumber).IsAssignableFrom(propertyClrType))
                {
                    modelBuilder.Invoke(ModelBuilderHelpers.GetMethodInfo("TrackNumberProperty"), propertyClrType);
                }

#pragma warning disable CS0162 // Unreachable code detected
                foreach (var foreignKey in modelEntityType.GetForeignKeys())
                {
                    switch (ConstantHelpers.GENERAL.DATABASES.DATABASE)
                    {
                        case ConstantHelpers.DATABASES.MYSQL:
                        case ConstantHelpers.DATABASES.PSQL:
                            foreignKey.NormalizeRelationalName(64);
                            break;
                        default:
                            foreignKey.NormalizeRelationalName();

                            break;
                    }

                    foreignKey.RestrictDeleteBehavior();
                }

                foreach (var index in modelEntityType.GetIndexes())
                {
                    switch (ConstantHelpers.GENERAL.DATABASES.DATABASE)
                    {
                        case ConstantHelpers.DATABASES.MYSQL:
                        case ConstantHelpers.DATABASES.PSQL:
                            index.NormalizeRelationalName(64);

                            break;
                        default:
                            index.NormalizeRelationalName();

                            break;
                    }
                }

                foreach (var key in modelEntityType.GetKeys())
                {
                    switch (ConstantHelpers.GENERAL.DATABASES.DATABASE)
                    {
                        case ConstantHelpers.DATABASES.MYSQL:
                        case ConstantHelpers.DATABASES.PSQL:
                            key.NormalizeRelationalName(64);

                            break;
                        default:
                            key.NormalizeRelationalName();

                            break;
                    }
                }

                foreach (var property in modelEntityType.GetProperties())
                {
                    switch (ConstantHelpers.GENERAL.DATABASES.DATABASE)
                    {
                        case ConstantHelpers.DATABASES.MYSQL:
                            if (property.PropertyInfo != null)
                            {
                                if (property.PropertyInfo.PropertyType.IsBool())
                                {
                                    modelBuilder.Entity(modelEntityType.ClrType)
                                        .Property(property.Name)
                                        .HasConversion(new BoolToZeroOneConverter<short>());
                                }
                                else if (property.PropertyInfo.PropertyType.IsDateTime())
                                {
                                    modelBuilder.Entity(modelEntityType.ClrType)
                                        .Property(property.Name)
                                        .HasColumnType("datetime");
                                }
                                else if (property.PropertyInfo.PropertyType.IsDecimal())
                                {
                                    modelBuilder.Entity(modelEntityType.ClrType)
                                        .Property(property.Name)
                                        .HasColumnType("decimal(18, 2)");
                                }
                                else if (property.PropertyInfo.PropertyType.IsGuid())
                                {
                                    modelBuilder.Entity(modelEntityType.ClrType)
                                        .Property(property.Name)
                                        .HasColumnType("char(36)");
                                }
                            }

                            break;
                        default:
                            break;
                    }
                }
#pragma warning restore CS0162 // Unreachable code detected
            }

            #region ACADEMIC EXCHANGE

            modelBuilder.Entity<Gallery>(x => x.ToDatabaseTable(databaseType, "Galleries", "AcademicExchange"));
            modelBuilder.Entity<Scholarship>(x => x.ToDatabaseTable(databaseType, "Scholarships", "AcademicExchange"));
            modelBuilder.Entity<Postulation>(x => x.ToDatabaseTable(databaseType, "Postulations", "AcademicExchange"));
            modelBuilder.Entity<Questionnaire>(x => x.ToDatabaseTable(databaseType, "Questionnaires", "AcademicExchange"));
            modelBuilder.Entity<PostulationFile>(x => x.ToDatabaseTable(databaseType, "PostulationFiles", "AcademicExchange"));
            modelBuilder.Entity<ScholarshipFile>(x => x.ToDatabaseTable(databaseType, "ScholarshipFiles", "AcademicExchange"));
            modelBuilder.Entity<AcademicAgreement>(x => x.ToDatabaseTable(databaseType, "AcademicAgreements", "AcademicExchange"));
            modelBuilder.Entity<AgreementType>(x => x.ToDatabaseTable(databaseType, "AgreementTypes", "AcademicExchange"));
            modelBuilder.Entity<QuestionnaireAnswer>(x => x.ToDatabaseTable(databaseType, "QuestionnaireAnswers", "AcademicExchange"));
            modelBuilder.Entity<AcademicExchangeNews>(x => x.ToDatabaseTable(databaseType, "AcademicExchangeNews", "AcademicExchange"));
            modelBuilder.Entity<QuestionnaireSection>(x => x.ToDatabaseTable(databaseType, "QuestionnaireSections", "AcademicExchange"));
            modelBuilder.Entity<QuestionnaireQuestion>(x => x.ToDatabaseTable(databaseType, "QuestionnaireQuestions", "AcademicExchange"));
            modelBuilder.Entity<AcademicAgreementType>(x => x.ToDatabaseTable(databaseType, "AcademicAgreementTypes", "AcademicExchange"));
            modelBuilder.Entity<PostulationInformation>(x => x.ToDatabaseTable(databaseType, "PostulationInformations", "AcademicExchange"));
            modelBuilder.Entity<QuestionnaireAnswerByUser>(x => x.ToDatabaseTable(databaseType, "QuestionnaireAnswerByUsers", "AcademicExchange"));
            modelBuilder.Entity<AeGeneralFile>(x => x.ToDatabaseTable(databaseType, "AeGeneralFiles", "AcademicExchange"));
            modelBuilder.Entity<AeContactMessage>(x => x.ToDatabaseTable(databaseType, "AeContactMessages", "AcademicExchange"));

            #endregion

            #region ADMISSION
            modelBuilder.Entity<ApplicationTermSurvey>(x => x.ToDatabaseTable(databaseType, "ApplicationTermSurveys", "Admission"));
            modelBuilder.Entity<ApplicationTermSurveyQuestion>(x => x.ToDatabaseTable(databaseType, "ApplicationTermSurveyQuestions", "Admission"));
            modelBuilder.Entity<ApplicationTermSurveyAnswer>(x => x.ToDatabaseTable(databaseType, "ApplicationTermSurveyAnswers", "Admission"));
            modelBuilder.Entity<ApplicationTermSurveyUser>(x => x.ToDatabaseTable(databaseType, "ApplicationTermSurveyUsers", "Admission"));
            modelBuilder.Entity<ApplicationTermSurveyAnswerByUser>(x =>
            {
                x.HasOne(y => y.ApplicationTermSurveyQuestion)
                .WithMany(y => y.ApplicationTermSurveyAnswerByUsers)
                .HasForeignKey(y => y.ApplicationTermSurveyQuestionId)
                .HasConstraintName("FK_ApplicationTermSurveyQuestion");
                x.HasIndex(y => y.ApplicationTermSurveyQuestionId).HasDatabaseName("IX_ApplicationTermSurveyQuestion");

                x.HasOne(y => y.ApplicationTermSurveyAnswer)
                .WithMany(y => y.ApplicationTermSurveyAnswerByUsers)
                .HasForeignKey(y => y.ApplicationTermSurveyAnswerId)
                .HasConstraintName("FK_ApplicationTermSurveyAnswer");
                x.HasIndex(y => y.ApplicationTermSurveyAnswerId).HasDatabaseName("IX_ApplicationTermSurveyAnswer");

                x.ToDatabaseTable(databaseType, "ApplicationTermSurveyAnswerByUsers", "Admission");
            });

            modelBuilder.Entity<AdmissionExamClassroomTeacher>(x =>
            {
                //x.HasKey(t => new { t.AdmissionExamClassroomId, t.TeacherId });
                x.ToDatabaseTable(databaseType, "AdmissionExamClassroomTeachers", "Admission");
            });
            modelBuilder.Entity<AdmissionExamApplicationTerm>(x =>
            {
                x.HasKey(t => new { t.AdmissionExamId, t.ApplicationTermId });
                x.ToDatabaseTable(databaseType, "AdmissionExamApplicationTerms", "Admission");
            });
            modelBuilder.Entity<AdmissionExamChannel>(x =>
            {
                x.HasKey(t => new { t.AdmissionExamId, t.ChannelId });
                x.ToDatabaseTable(databaseType, "AdmissionExamChannels", "Admission");
            });
            modelBuilder.Entity<AdmissionExamClassroomCareer>(x =>
            {
                x.HasKey(t => new { t.AdmissionExamClassroomId, t.CareerId });
                x.ToDatabaseTable(databaseType, "AdmissionExamClassroomCareers", "Admission");
            });
            modelBuilder.Entity<PostulantAdmissionRequirement>().HasKey(t => new { t.PostulantId, t.AdmissionRequirementId });
            modelBuilder.Entity<Vacant>(x => x.ToDatabaseTable(databaseType, "Vacants", "Admission"));
            modelBuilder.Entity<Prospect>(x => x.ToDatabaseTable(databaseType, "Prospects", "Admission"));

            modelBuilder.Entity<Postulant>(x =>
            {
                x.ToDatabaseTable(databaseType, "Postulants", "Admission");
                x.Property(y => y.FinalScore).HasColumnType("decimal(18,3)");
            });

            modelBuilder.Entity<AdmissionExam>(x => x.ToDatabaseTable(databaseType, "AdmissionExams", "Admission"));
            modelBuilder.Entity<AdmissionType>(x => x.ToDatabaseTable(databaseType, "AdmissionTypes", "Admission"));
            modelBuilder.Entity<VocationalTest>(x => x.ToDatabaseTable(databaseType, "VocationalTests", "Admission"));
            modelBuilder.Entity<ApplicationTerm>(x => x.ToDatabaseTable(databaseType, "ApplicationTerms", "Admission"));
            modelBuilder.Entity<AdmissionResult>(x => x.ToDatabaseTable(databaseType, "AdmissionResults", "Admission"));
            modelBuilder.Entity<EntityLoadFormat>(x => x.ToDatabaseTable(databaseType, "EntityLoadFormats", "Admission"));
            modelBuilder.Entity<PostulantFamily>(x => x.ToDatabaseTable(databaseType, "PostulantFamilies", "Admission"));
            modelBuilder.Entity<PostulantCardSection>(x => x.ToDatabaseTable(databaseType, "PostulantCardSections", "Admission"));
            modelBuilder.Entity<VocationalTestAnswer>(x => x.ToDatabaseTable(databaseType, "VocationalTestAnswers", "Admission"));
            modelBuilder.Entity<AdmissionRequirement>(x => x.ToDatabaseTable(databaseType, "AdmissionRequirements", "Admission"));
            modelBuilder.Entity<CareerApplicationTerm>(x => x.ToDatabaseTable(databaseType, "CareerApplicationTerms", "Admission"));
            modelBuilder.Entity<AdmissionTypeDescount>(x => x.ToDatabaseTable(databaseType, "AdmissionTypeDescounts", "Admission"));
            modelBuilder.Entity<AdmissionExamClassroom>(x => x.ToDatabaseTable(databaseType, "AdmissionExamClassrooms", "Admission"));
            modelBuilder.Entity<ApplicationTermManager>(x => x.ToDatabaseTable(databaseType, "ApplicationTermManagers", "Admission"));
            modelBuilder.Entity<VocationalTestQuestion>(x => x.ToDatabaseTable(databaseType, "VocationalTestQuestions", "Admission"));
            modelBuilder.Entity<VocationalTestAnswerCareer>(x => x.ToDatabaseTable(databaseType, "VocationalTestAnswerCareers", "Admission"));
            modelBuilder.Entity<AdmissionExamPostulantGrade>(x => x.ToDatabaseTable(databaseType, "AdmissionExamPostulantGrades", "Admission"));
            modelBuilder.Entity<ApplicationTermAdmissionType>(x => x.ToDatabaseTable(databaseType, "ApplicationTermAdmissionTypes", "Admission"));
            modelBuilder.Entity<ApplicationTermCampus>(x => x.ToDatabaseTable(databaseType, "ApplicationTermCampuses", "Admission"));
            modelBuilder.Entity<PostulantAdmissionRequirement>(x => x.ToDatabaseTable(databaseType, "PostulantAdmissionRequirements", "Admission"));
            modelBuilder.Entity<AdmissionExamClassroomPostulant>(x => x.ToDatabaseTable(databaseType, "AdmissionExamClassroomPostulants", "Admission"));
            modelBuilder.Entity<VocationalTestAnswerCareerPostulant>(x => x.ToDatabaseTable(databaseType, "VocationalTestAnswerCareerPostulants", "Admission"));
            modelBuilder.Entity<SpecialCase>(x => x.ToDatabaseTable(databaseType, "SpecialCases", "Admission"));
            modelBuilder.Entity<ApplicationTermAdmissionFile>(x => x.ToDatabaseTable(databaseType, "ApplicationTermAdmissionFiles", "Admission"));
            modelBuilder.Entity<PostulantOriginalLanguage>(x => x.ToDatabaseTable(databaseType, "PostulantOriginalLanguages", "Admission"));
            modelBuilder.Entity<SanctionedPostulant>(x => x.ToDatabaseTable(databaseType, "SanctionedPostulants", "Admission"));
            modelBuilder.Entity<School>(x => x.ToDatabaseTable(databaseType, "Schools", "Admission"));

            #endregion

            #region CAFETERIA

            modelBuilder.Entity<Supply>(x => x.ToDatabaseTable(databaseType, "Supplies", "Cafeteria"));
            modelBuilder.Entity<Provider>(x => x.ToDatabaseTable(databaseType, "Providerss", "Cafeteria"));
            modelBuilder.Entity<MenuPlate>(x => x.ToDatabaseTable(databaseType, "MenuPlates", "Cafeteria"));
            modelBuilder.Entity<SupplyOrder>(x => x.ToDatabaseTable(databaseType, "SupplyOrders", "Cafeteria"));
            modelBuilder.Entity<PurchaseOrder>(x => x.ToDatabaseTable(databaseType, "PurchaseOrders", "Cafeteria"));
            modelBuilder.Entity<SupplyPackage>(x => x.ToDatabaseTable(databaseType, "SupplyPackages", "Cafeteria"));
            modelBuilder.Entity<CafeteriaStock>(x => x.ToDatabaseTable(databaseType, "CafeteriaStocks", "Cafeteria"));
            modelBuilder.Entity<ProviderSupply>(x => x.ToDatabaseTable(databaseType, "ProviderSupplies", "Cafeteria"));
            modelBuilder.Entity<UnitMeasurement>(x => x.ToDatabaseTable(databaseType, "UnitMeasurements", "Cafeteria"));
            modelBuilder.Entity<CafeteriaKardex>(x => x.ToDatabaseTable(databaseType, "CafeteriaKardexes", "Cafeteria"));
            modelBuilder.Entity<MenuPlateSupply>(x => x.ToDatabaseTable(databaseType, "MenuPlateSupplies", "Cafeteria"));
            modelBuilder.Entity<SupplyOrderDetail>(x => x.ToDatabaseTable(databaseType, "SupplyOrderDetails", "Cafeteria"));
            modelBuilder.Entity<PurchaseOrderDetail>(x => x.ToDatabaseTable(databaseType, "PurchaseOrderDetails", "Cafeteria"));
            modelBuilder.Entity<CafeteriaServiceTerm>(x => x.ToDatabaseTable(databaseType, "CafeteriaServiceTerms", "Cafeteria"));
            //// ESTO DE AQUI
            modelBuilder.Entity<CafeteriaPostulation>(x => x.ToDatabaseTable(databaseType, "CafeteriaPostulations", "Cafeteria"));
            ////
            modelBuilder.Entity<CafeteriaWeeklySchedule>(x => x.ToDatabaseTable(databaseType, "CafeteriaWeeklySchedules", "Cafeteria"));
            modelBuilder.Entity<UserCafeteriaServiceTerm>(x => x.ToDatabaseTable(databaseType, "UserCafeteriaServiceTerms", "Cafeteria"));
            modelBuilder.Entity<CafeteriaServiceTermSchedule>(x => x.ToDatabaseTable(databaseType, "CafeteriaServiceTermSchedules", "Cafeteria"));
            modelBuilder.Entity<UserCafeteriaDailyAssistance>(x => x.ToDatabaseTable(databaseType, "UserCafeteriaDailyAssistances", "Cafeteria"));
            modelBuilder.Entity<CafeteriaWeeklyScheduleTurnDetail>(x => x.ToDatabaseTable(databaseType, "CafeteriaWeeklyScheduleTurnDetails", "Cafeteria"));
            #endregion

            #region COMPUTER CENTER

            modelBuilder.Entity<ComGroup>(x => x.ToDatabaseTable(databaseType, "Groups", "Computer"));
            modelBuilder.Entity<ComClass>(x => x.ToDatabaseTable(databaseType, "Classes", "Computer"));
            modelBuilder.Entity<ComGrades>(x => x.ToDatabaseTable(databaseType, "Grades", "Computer"));
            modelBuilder.Entity<ComCourse>(x => x.ToDatabaseTable(databaseType, "Courses", "Computer"));
            modelBuilder.Entity<ComPayment>(x => x.ToDatabaseTable(databaseType, "Payments", "Computer"));
            modelBuilder.Entity<ComActivity>(x => x.ToDatabaseTable(databaseType, "Activities", "Computer"));
            modelBuilder.Entity<ComUserGroup>(x => x.ToDatabaseTable(databaseType, "UserGroups", "Computer"));
            modelBuilder.Entity<ComClassUser>(x => x.ToDatabaseTable(databaseType, "ClassUsers", "Computer"));
            modelBuilder.Entity<ComCourseModule>(x => x.ToDatabaseTable(databaseType, "CourseModules", "Computer"));
            modelBuilder.Entity<ComClassSchedule>(x => x.ToDatabaseTable(databaseType, "ClassSchedules", "Computer"));
            modelBuilder.Entity<ComEvaluationCriteria>(x => x.ToDatabaseTable(databaseType, "EvaluationCriterias", "Computer"));

            #endregion

            #region COMPUTER MANAGEMENT

            modelBuilder.Entity<Computer>(x => x.ToDatabaseTable(databaseType, "Computers", "ComputersManagement"));
            modelBuilder.Entity<Software>(x => x.ToDatabaseTable(databaseType, "Softwares", "ComputersManagement"));
            modelBuilder.Entity<Hardware>(x => x.ToDatabaseTable(databaseType, "Hardwares", "ComputersManagement"));
            modelBuilder.Entity<Equipment>(x => x.ToDatabaseTable(databaseType, "Equipments", "ComputersManagement"));
            modelBuilder.Entity<ComputerType>(x => x.ToDatabaseTable(databaseType, "ComputerType", "ComputersManagement"));
            modelBuilder.Entity<HardwareType>(x => x.ToDatabaseTable(databaseType, "HardwareTypes", "ComputersManagement"));
            modelBuilder.Entity<SoftwareType>(x => x.ToDatabaseTable(databaseType, "SoftwareTypes", "ComputersManagement"));
            modelBuilder.Entity<ComputerState>(x => x.ToDatabaseTable(databaseType, "ComputerState", "ComputersManagement"));
            modelBuilder.Entity<EquipmentType>(x => x.ToDatabaseTable(databaseType, "EquipmentTypes", "ComputersManagement"));
            modelBuilder.Entity<SoftwareSubType>(x => x.ToDatabaseTable(databaseType, "SoftwareSubTypes", "ComputersManagement"));
            modelBuilder.Entity<ComputerSupplier>(x => x.ToDatabaseTable(databaseType, "ComputerSuppliers", "ComputersManagement"));
            modelBuilder.Entity<ComputerConditionFile>(x => x.ToDatabaseTable(databaseType, "ComputerConditionFiles", "ComputersManagement"));
            modelBuilder.Entity<InstitutionalStrategicPlan>(x => x.ToDatabaseTable(databaseType, "InstitutionalStrategicPlans", "ComputersManagement"));


            #endregion

            #region DBO

            modelBuilder.Entity<ApplicationUser>(x =>
            {
                x.HasOne(p => p.WorkerPersonalDocuments)
                    .WithOne(i => i.User)
                    .HasForeignKey<WorkerPersonalDocument>(b => b.UserId)
                    .IsRequired();
                x.HasIndex(p => new { p.FullName });
                x.HasIndex(p => p.UserName);
            });

            //modelBuilder.Entity<ApplicationUserAuthorized>(x =>
            //{
            //    x.HasKey(c => new { c.Id, c.UserId });
            //});

            modelBuilder.Entity<ApplicationUserRole>(x =>
            {
                x.HasKey(ur => new { ur.UserId, ur.RoleId });
                x.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
                x.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            //modelBuilder.Entity<OpenIddictToken>(x =>
            //{
            //    x.HasIndex(u => u.ReferenceId).IsUnique();
            //});

            #endregion

            #region DEGREE

            modelBuilder.Entity<RegistryPattern>(x => x.ToDatabaseTable(databaseType, "RegistryPatterns", "Degree"));
            modelBuilder.Entity<ForeignUniversityOrigin>(x => x.ToDatabaseTable(databaseType, "ForeignUniversityOrigins", "Degree"));
            modelBuilder.Entity<HistoricalRegistryPattern>(x => x.ToDatabaseTable(databaseType, "HistoricalRegistryPatterns", "Degree"));
            modelBuilder.Entity<DegreeRequirement>(x => x.ToDatabaseTable(databaseType, "DegreeRequirements", "Degree"));
            modelBuilder.Entity<DegreeProjectDirector>(x => x.ToDatabaseTable(databaseType, "DegreeProjectDirectors", "Degree"));

            #endregion
            #region POSDEGREE
            modelBuilder.Entity<Master>(x =>  { x.ToDatabaseTable(databaseType, "Masters", "PosDegree"); });
            modelBuilder.Entity<PosdegreeStudent>(x =>{ x.ToDatabaseTable(databaseType, "PosdegreeStudent", "PosDegree"); });
            modelBuilder.Entity<PosdegreeDetailsPayment>(x =>{ x.ToDatabaseTable(databaseType, "PosdegreeDetailsPayments", "PosDegree"); });
            #endregion

            #region DOCUMENTARY PROCEDURE

            modelBuilder.Entity<Dependency>(x =>
            {
                x.HasIndex(p => p.Acronym).IsUnique();
                x.ToDatabaseTable(databaseType, "Dependencies", "DocumentaryProcedure");
            });
            modelBuilder.Entity<DocumentaryRecordType>(x =>
            {
                x.HasIndex(p => p.Code).IsUnique();
                x.ToDatabaseTable(databaseType, "DocumentaryRecordTypes", "DocumentaryProcedure");
            });
            modelBuilder.Entity<DocumentType>(x =>
            {
                x.HasIndex(p => p.Code).IsUnique();
                x.ToDatabaseTable(databaseType, "DocumentTypes", "DocumentaryProcedure");
            });
            modelBuilder.Entity<ProcedureCategory>(x =>
            {
                x.HasIndex(p => p.Name).IsUnique();
                x.ToDatabaseTable(databaseType, "ProcedureCategories", "DocumentaryProcedure");
            });
            modelBuilder.Entity<UserProcedureRecord>(x =>
            {
                x.HasIndex(p => p.FullRecordNumber).IsUnique();
                x.ToDatabaseTable(databaseType, "UserProcedureRecords", "DocumentaryProcedure");
            });
            modelBuilder.Entity<RecordSubjectType>(x =>
            {
                x.HasIndex(p => p.Code).IsUnique();
                x.ToDatabaseTable(databaseType, "RecordSubjectTypes", "DocumentaryProcedure");
            });
            modelBuilder.Entity<Record>(x => x.ToDatabaseTable(databaseType, "Records", "DocumentaryProcedure"));
            modelBuilder.Entity<Complaint>(x =>
            {
                x.Property(r => r.HiredType).HasDefaultValue(ConstantHelpers.COMPLAINT_BOOK.HIRED_TYPE.SERVICE);
                x.ToDatabaseTable(databaseType, "Complaints", "DocumentaryProcedure");
            });
            modelBuilder.Entity<ComplaintType>(x => x.ToDatabaseTable(databaseType, "ComplaintTypes", "DocumentaryProcedure"));
            modelBuilder.Entity<ComplaintFile>(x => x.ToDatabaseTable(databaseType, "ComplaintFiles", "DocumentaryProcedure"));


            modelBuilder.Entity<Procedure>(x =>
            {
                x.Property(r => r.Type).HasDefaultValue(ConstantHelpers.PROCEDURES.TYPE.DEPENDENCY);
                x.Property(r => r.Enabled).HasDefaultValue(true);
                x.ToDatabaseTable(databaseType, "Procedures", "DocumentaryProcedure");
            });

            modelBuilder.Entity<UserProcedure>(x => x.ToDatabaseTable(databaseType, "UserProcedures", "DocumentaryProcedure"));
            modelBuilder.Entity<StudentUserProcedure>(x => x.ToDatabaseTable(databaseType, "StudentUserProcedures", "DocumentaryProcedure"));
            modelBuilder.Entity<StudentUserProcedureDetail>(x => x.ToDatabaseTable(databaseType, "StudentUserProcedureDetails", "DocumentaryProcedure"));
            modelBuilder.Entity<ProcedureRole>(x => x.ToDatabaseTable(databaseType, "ProcedureRoles", "DocumentaryProcedure"));
            modelBuilder.Entity<ProcedureAdmissionType>(x => x.ToDatabaseTable(databaseType, "ProcedureAdmissionTypes", "DocumentaryProcedure"));
            modelBuilder.Entity<UserDependency>(x => x.ToDatabaseTable(databaseType, "UserDependencies", "DocumentaryProcedure"));
            modelBuilder.Entity<InternalProcedure>(x => x.ToDatabaseTable(databaseType, "InternalProcedures", "DocumentaryProcedure"));
            modelBuilder.Entity<ExternalProcedure>(x => x.ToDatabaseTable(databaseType, "ExternalProcedures", "DocumentaryProcedure"));
            modelBuilder.Entity<ProcedureResolution>(x => x.ToDatabaseTable(databaseType, "ProcedureResolutions", "DocumentaryProcedure"));
            modelBuilder.Entity<ProcedureDependency>(x => x.ToDatabaseTable(databaseType, "ProcedureDependencies", "DocumentaryProcedure"));

            modelBuilder.Entity<ProcedureRequirement>(x =>
            {
                x.Property(r => r.Type).HasDefaultValue(ConstantHelpers.PROCEDURE_REQUIREMENTS.TYPE.COST);
                x.ToDatabaseTable(databaseType, "ProcedureRequirements", "DocumentaryProcedure");
            });

            modelBuilder.Entity<ProcedureSubcategory>(x => x.ToDatabaseTable(databaseType, "ProcedureSubcategories", "DocumentaryProcedure"));
            modelBuilder.Entity<InternalProcedureFile>(x => x.ToDatabaseTable(databaseType, "InternalProcedureFiles", "DocumentaryProcedure"));
            modelBuilder.Entity<UserExternalProcedure>(x => x.ToDatabaseTable(databaseType, "UserExternalProcedures", "DocumentaryProcedure"));
            modelBuilder.Entity<UserInternalProcedure>(x => x.ToDatabaseTable(databaseType, "UserInternalProcedures", "DocumentaryProcedure"));
            modelBuilder.Entity<UserProcedureDerivation>(x => x.ToDatabaseTable(databaseType, "UserProcedureDerivations", "DocumentaryProcedure"));
            modelBuilder.Entity<InternalProcedureReference>(x => x.ToDatabaseTable(databaseType, "InternalProcedureReferences", "DocumentaryProcedure"));
            modelBuilder.Entity<UserExternalProcedureRecord>(x => x.ToDatabaseTable(databaseType, "UserExternalProcedureRecords", "DocumentaryProcedure"));
            modelBuilder.Entity<UserProcedureRecordDocument>(x => x.ToDatabaseTable(databaseType, "UserProcedureRecordDocuments", "DocumentaryProcedure"));
            modelBuilder.Entity<UserProcedureDerivationFile>(x => x.ToDatabaseTable(databaseType, "UserProcedureDerivationFiles", "DocumentaryProcedure"));
            modelBuilder.Entity<UserProcedureRecordRequirement>(x => x.ToDatabaseTable(databaseType, "UserProcedureRecordRequirements", "DocumentaryProcedure"));
            modelBuilder.Entity<UserExternalProcedureRecordDocument>(x => x.ToDatabaseTable(databaseType, "UserExternalProcedureRecordDocuments", "DocumentaryProcedure"));
            modelBuilder.Entity<UserProcedureFile>(x => x.ToDatabaseTable(databaseType, "UserProcedureFiles", "DocumentaryProcedure"));
            modelBuilder.Entity<ActivityProcedure>(x => x.ToDatabaseTable(databaseType, "ActivityProcedures", "DocumentaryProcedure"));
            modelBuilder.Entity<UserExternalProcedureFile>(x => x.ToDatabaseTable(databaseType, "UserExternalProcedureFiles", "DocumentaryProcedure"));
            modelBuilder.Entity<SignedDocument>(x => x.ToDatabaseTable(databaseType, "SignedDocuments", "DocumentaryProcedure"));
            modelBuilder.Entity<DocumentTemplate>(x =>
            {
                x.Property(r => r.System).HasDefaultValue(ConstantHelpers.SYSTEMS.DOCUMENTARY_PROCEDURE);
                x.ToDatabaseTable(databaseType, "DocumentTemplates", "DocumentaryProcedure");
            });
            modelBuilder.Entity<ProcedureFolder>(x => x.ToDatabaseTable(databaseType, "ProcedureFolders", "DocumentaryProcedure"));
            modelBuilder.Entity<ProcedureTask>(x =>
            {
                x.Property(r => r.Type).HasDefaultValue(ConstantHelpers.PROCEDURE_TASK.TYPE.DESCRIPTION);
                x.ToDatabaseTable(databaseType, "ProcedureTasks", "DocumentaryProcedure");
            });

            #endregion

            #region ECONOMIC MANAGEMENT 

            modelBuilder.Entity<CashierDependency>(x =>
            {
                x.HasKey(t => new { t.UserId, t.DependencyId });
                x.ToDatabaseTable(databaseType, "CashierDependencies", "EconomicManagement");
            });
            modelBuilder.Entity<Classifier>(x => x.ToDatabaseTable(databaseType, "Classifiers", "EconomicManagement"));
            modelBuilder.Entity<Client>(x => x.ToDatabaseTable(databaseType, "Clients", "EconomicManagement"));
            modelBuilder.Entity<Concept>(x => x.ToDatabaseTable(databaseType, "Concepts", "EconomicManagement"));
            modelBuilder.Entity<ConceptHistory>(x => x.ToDatabaseTable(databaseType, "ConceptHistories", "EconomicManagement"));
            modelBuilder.Entity<ConceptDistribution>(x => x.ToDatabaseTable(databaseType, "ConceptDistributions", "EconomicManagement"));
            modelBuilder.Entity<ConceptDistributionDetail>(x => x.ToDatabaseTable(databaseType, "ConceptDistributionDetails", "EconomicManagement"));
            modelBuilder.Entity<CreditNote>(x => x.ToDatabaseTable(databaseType, "CreditNotes", "EconomicManagement"));
            modelBuilder.Entity<CreditNoteDetail>(x => x.ToDatabaseTable(databaseType, "CreditNoteDetails", "EconomicManagement"));
            modelBuilder.Entity<Debt>(x => x.ToDatabaseTable(databaseType, "Debts", "EconomicManagement"));
            modelBuilder.Entity<ExpenditureProvision>(x => x.ToDatabaseTable(databaseType, "ExpenditureProvisions", "EconomicManagement"));
            modelBuilder.Entity<Expense>(x => x.ToDatabaseTable(databaseType, "Expenses", "EconomicManagement"));
            modelBuilder.Entity<ExpenseOutput>(x => x.ToDatabaseTable(databaseType, "ExpenseOutputs", "EconomicManagement"));

            modelBuilder.Entity<Payment>(x =>
            {
                x.HasAlternateKey(y => y.BankIdentifier);
                x.Property(y => y.BankIdentifier).ValueGeneratedOnAdd();

                //if (ConstantHelpers.GENERAL.DATABASES.DATABASE == ConstantHelpers.DATABASES.MYSQL)
                //    x.Property(y => y.BankIdentifier).UseMySqlIdentityColumn();
                //else
                //    x.Property(y => y.BankIdentifier).UseIdentityColumn();

                x.ToDatabaseTable(databaseType, "Payments", "EconomicManagement");
            });

            modelBuilder.Entity<SerialNumber>(x => x.ToDatabaseTable(databaseType, "SerialNumbers", "EconomicManagement"));
            modelBuilder.Entity<Settlement>(x => x.ToDatabaseTable(databaseType, "Settlements", "EconomicManagement"));
            modelBuilder.Entity<SettlementDetail>(x => x.ToDatabaseTable(databaseType, "SettlementDetails", "EconomicManagement"));
            modelBuilder.Entity<BalanceTransfer>(x => x.ToDatabaseTable(databaseType, "BalanceTransfers", "EconomicManagement"));
            modelBuilder.Entity<Income>(x => x.ToDatabaseTable(databaseType, "Incomes", "EconomicManagement"));
            modelBuilder.Entity<SiafExpense>(x => x.ToDatabaseTable(databaseType, "SiafExpenses", "EconomicManagement"));
            modelBuilder.Entity<Order>(x => x.ToDatabaseTable(databaseType, "Orders", "EconomicManagement"));
            modelBuilder.Entity<OrderChangeHistory>(x => x.ToDatabaseTable(databaseType, "OrderChangeHistories", "EconomicManagement"));
            modelBuilder.Entity<OrderChangeFileHistory>(x => x.ToDatabaseTable(databaseType, "OrderChangeFileHistories", "EconomicManagement"));
            modelBuilder.Entity<Requirement>(x => x.ToDatabaseTable(databaseType, "Requirements", "EconomicManagement"));
            modelBuilder.Entity<RequirementFile>(x => x.ToDatabaseTable(databaseType, "RequirementFiles", "EconomicManagement"));
            modelBuilder.Entity<RequirementSupplier>(x => x.ToDatabaseTable(databaseType, "RequirementSuppliers", "EconomicManagement"));
            modelBuilder.Entity<Supplier>(x => x.ToDatabaseTable(databaseType, "Suppliers", "EconomicManagement"));
            modelBuilder.Entity<SupplierCategory>(x => x.ToDatabaseTable(databaseType, "SupplierCategories", "EconomicManagement"));
            modelBuilder.Entity<UserRequirement>(x => x.ToDatabaseTable(databaseType, "UserRequirements", "EconomicManagement"));
            modelBuilder.Entity<UserRequirementFile>(x => x.ToDatabaseTable(databaseType, "UserRequirementFiles", "EconomicManagement"));
            modelBuilder.Entity<StructureForExpense>(x => x.ToDatabaseTable(databaseType, "StructureForExpenses", "EconomicManagement"));
            modelBuilder.Entity<StructureForIncome>(x => x.ToDatabaseTable(databaseType, "StructureForIncomes", "EconomicManagement"));
            modelBuilder.Entity<BudgetFramework>(x => x.ToDatabaseTable(databaseType, "BudgetFrameworks", "EconomicManagement"));
            modelBuilder.Entity<BudgetBalance>(x => x.ToDatabaseTable(databaseType, "BudgetBalances", "EconomicManagement"));
            modelBuilder.Entity<PhaseRequirement>(x => x.ToDatabaseTable(databaseType, "PhaseRequirements", "EconomicManagement"));
            modelBuilder.Entity<CatalogItem>(x => x.ToDatabaseTable(databaseType, "CatalogItems", "EconomicManagement"));
            modelBuilder.Entity<CatalogGoal>(x => x.ToDatabaseTable(databaseType, "CatalogGoals", "EconomicManagement"));
            modelBuilder.Entity<UserRequirementItem>(x => x.ToDatabaseTable(databaseType, "UserRequirementItems", "EconomicManagement"));
            modelBuilder.Entity<CatalogItemGoal>(x => x.ToDatabaseTable(databaseType, "CatalogItemGoals", "EconomicManagement"));
            modelBuilder.Entity<AccountingPlan>(x => x.ToDatabaseTable(databaseType, "AccountingPlans", "EconomicManagement"));
            modelBuilder.Entity<CurrentAccount>(x => x.ToDatabaseTable(databaseType, "CurrentAccounts", "EconomicManagement"));
            modelBuilder.Entity<ExecuteObservation>(x => x.ToDatabaseTable(databaseType, "ExecuteObservations", "EconomicManagement"));
            modelBuilder.Entity<BankDeposit>(x => x.ToDatabaseTable(databaseType, "BankDeposits", "EconomicManagement"));
            modelBuilder.Entity<PettyCashBook>(x => x.ToDatabaseTable(databaseType, "PettyCashBooks", "EconomicManagement"));
            modelBuilder.Entity<CatalogActivity>(x => x.ToDatabaseTable(databaseType, "CatalogActivities", "EconomicManagement"));
            modelBuilder.Entity<BankTemporalPayment>(x => x.ToDatabaseTable(databaseType, "BankTemporalPayments", "EconomicManagement"));
            modelBuilder.Entity<UserCurrentAccount>(x => x.ToDatabaseTable(databaseType, "UserCurrentAccounts", "EconomicManagement"));
            modelBuilder.Entity<CardPaymentDetail>(x => x.ToDatabaseTable(databaseType, "CardPaymentDetails", "EconomicManagement"));
            modelBuilder.Entity<CardPaymentHeader>(x => x.ToDatabaseTable(databaseType, "CardPaymentHeaders", "EconomicManagement"));
            modelBuilder.Entity<ExoneratePayment>(x => x.ToDatabaseTable(databaseType, "ExoneratePayments", "EconomicManagement"));
            modelBuilder.Entity<ReceivedOrderHistory>(x => x.ToDatabaseTable(databaseType, "ReceivedOrderHistories", "EconomicManagement"));
            modelBuilder.Entity<ReceivedOrder>(x => x.ToDatabaseTable(databaseType, "ReceivedOrders", "EconomicManagement"));

            modelBuilder.Entity<MonthlyBalance>(x =>
            {
                x.HasKey(t => new { t.Year, t.Month });
                x.ToDatabaseTable(databaseType, "MonthlyBalances", "EconomicManagement");
            });
            modelBuilder.Entity<InternalOutput>(x => x.ToDatabaseTable(databaseType, "InternalOutputs", "EconomicManagement"));
            modelBuilder.Entity<InternalOutputItem>(x => x.ToDatabaseTable(databaseType, "InternalOutputItems", "EconomicManagement"));
            modelBuilder.Entity<Heritage>(x =>
            {
                x.HasKey(t => new { t.DependencyId, t.CatalogItemId });
                x.ToDatabaseTable(databaseType, "Heritages", "EconomicManagement");
            });
            modelBuilder.Entity<PaymentToValidate>(x => x.ToDatabaseTable(databaseType, "PaymentToValidates", "EconomicManagement"));

            modelBuilder.Entity<ConceptGroup>(x => x.ToDatabaseTable(databaseType, "ConceptGroups", "EconomicManagement"));
            modelBuilder.Entity<ConceptGroupDetail>(x =>
            {
                x.HasKey(t => new { t.ConceptGroupId, t.ConceptId });
                x.ToDatabaseTable(databaseType, "ConceptGroupDetails", "EconomicManagement");
            });

            #endregion

            #region ENROLLMENT
            modelBuilder.Entity<Area>(x =>
            {
                x.HasIndex(p => p.Name).IsUnique();
                x.ToDatabaseTable(databaseType, "Areas", "Enrollment");
            });
            modelBuilder.Entity<CampusCareer>(x =>
            {
                x.HasKey(t => new { t.CampusId, t.CareerId });
                x.ToDatabaseTable(databaseType, "CampusCareers", "Enrollment");
            });
            modelBuilder.Entity<Classroom>(x =>
            {
                x.HasQueryFilter(x => x.Status == ConstantHelpers.STATES.ACTIVE);
                x.HasQueryFilter(x => x.DeletedAt == null);
                x.HasQueryFilter(x => x.DeletedBy == null);
                x.ToDatabaseTable(databaseType, "Classrooms", "Enrollment");
            });
            modelBuilder.Entity<Recognition>(x => x.ToDatabaseTable(databaseType, "Recognitions", "Enrollment"));
            modelBuilder.Entity<RecognitionHistory>(x => x.ToDatabaseTable(databaseType, "RecognitionHistories", "Enrollment"));
            modelBuilder.Entity<CourseRecognition>(x =>
            {
                x.HasKey(s => new { s.CourseId, s.RecognitionId });
                x.ToDatabaseTable(databaseType, "CoursesRecognition", "Enrollment");
            });
            modelBuilder.Entity<Term>(x =>
            {
                x.HasIndex(p => p.Name).IsUnique();
                x.ToDatabaseTable(databaseType, "Terms", "Enrollment");
            });
            modelBuilder.Entity<AcademicProgramCurriculum>(x =>
            {
                x.HasKey(t => new { t.AcademicProgramId, t.CurriculumId });
                x.ToDatabaseTable(databaseType, "AcademicProgramCurriculums", "Enrollment");
            });
            modelBuilder.Entity<ChannelCareer>(x =>
            {
                x.HasKey(t => new { t.ChannelId, t.CareerId });
                x.ToDatabaseTable(databaseType, "ChannelCareers", "Enrollment");
            });
            modelBuilder.Entity<EnrollmentSurveyStudent>(x => x.ToDatabaseTable(databaseType, "EnrollmentSurveyStudents", "Enrollment"));
            modelBuilder.Entity<SupportMessage>(x => x.ToDatabaseTable(databaseType, "SupportMessages", "Enrollment"));
            modelBuilder.Entity<SupportChat>(x => x.ToDatabaseTable(databaseType, "SupportChats", "Enrollment"));
            modelBuilder.Entity<AcademicYearCourse>(x => x.ToDatabaseTable(databaseType, "AcademicYearCourses", "Enrollment"));
            modelBuilder.Entity<Building>(x => x.ToDatabaseTable(databaseType, "Buildings", "Enrollment"));
            modelBuilder.Entity<Campus>(x => x.ToDatabaseTable(databaseType, "Campuses", "Enrollment"));
            modelBuilder.Entity<ClassroomType>(x => x.ToDatabaseTable(databaseType, "ClassroomTypes", "Enrollment"));
            modelBuilder.Entity<Course>(x => x.ToDatabaseTable(databaseType, "Courses", "Enrollment"));
            modelBuilder.Entity<CourseEquivalence>(x => x.ToDatabaseTable(databaseType, "CourseEquivalences", "Enrollment"));
            modelBuilder.Entity<CurriculumEquivalence>(x =>
            {
                x.HasKey(t => new { t.NewCurriculumId, t.OldCurriculumId });
                x.ToDatabaseTable(databaseType, "CurriculumEquivalences", "Enrollment");
            });
            modelBuilder.Entity<CourseSyllabus>(x => x.ToDatabaseTable(databaseType, "CourseSyllabus", "Enrollment"));
            modelBuilder.Entity<CourseSyllabusObservation>(x => x.ToDatabaseTable(databaseType, "CourseSyllabusObservations", "Enrollment"));
            modelBuilder.Entity<CourseSyllabusWeek>(x => x.ToDatabaseTable(databaseType, "CourseSyllabusWeeks", "Enrollment"));
            modelBuilder.Entity<CourseTerm>(x =>
            {
                x.Property(r => r.Modality).HasDefaultValue(ConstantHelpers.Course.Modality.PRESENTIAL);
                x.ToDatabaseTable(databaseType, "CourseTerms", "Enrollment");
            });
            modelBuilder.Entity<CourseType>(x => x.ToDatabaseTable(databaseType, "CourseTypes", "Enrollment"));
            modelBuilder.Entity<CourseUnit>(x => x.ToDatabaseTable(databaseType, "CourseUnits", "Enrollment"));
            modelBuilder.Entity<Curriculum>(x => x.ToDatabaseTable(databaseType, "Curriculums", "Enrollment"));
            modelBuilder.Entity<CurriculumArea>(x => x.ToDatabaseTable(databaseType, "CurriculumAreas", "Enrollment"));
            modelBuilder.Entity<ElectiveCourse>(x => x.ToDatabaseTable(databaseType, "ElectiveCourses", "Enrollment"));
            modelBuilder.Entity<EnrollmentShift>(x => x.ToDatabaseTable(databaseType, "EnrollmentShifts", "Enrollment"));
            modelBuilder.Entity<EnrollmentTurn>(x => x.ToDatabaseTable(databaseType, "EnrollmentTurns", "Enrollment"));
            modelBuilder.Entity<EnrollmentTurnHistory>(x => x.ToDatabaseTable(databaseType, "EnrollmentTurnHistories", "Enrollment"));
            modelBuilder.Entity<EntrantEnrollment>(x => x.ToDatabaseTable(databaseType, "EntrantEnrollments", "Enrollment"));
            modelBuilder.Entity<CareerParallelCourse>(x => x.ToDatabaseTable(databaseType, "CareerParallelCourses", "Enrollment"));
            modelBuilder.Entity<AcademicCalendarDate>(x => x.ToDatabaseTable(databaseType, "AcademicCalendarDates", "Enrollment"));
            modelBuilder.Entity<CompetenceEvaluation>(x => x.ToDatabaseTable(databaseType, "CompetenceEvaluations", "Enrollment"));
            modelBuilder.Entity<CalificationCriteria>(x => x.ToDatabaseTable(databaseType, "CalificactionCriterias", "Enrollment"));
            modelBuilder.Entity<CareerEnrollmentShift>(x => x.ToDatabaseTable(databaseType, "CareerEnrollmentShifts", "Enrollment"));
            modelBuilder.Entity<EnrollmentReservation>(x => x.ToDatabaseTable(databaseType, "EnrollmentReservations", "Enrollment"));
            modelBuilder.Entity<FacultyCurriculumArea>(x => x.ToDatabaseTable(databaseType, "FacultyCurriculumAreas", "Enrollment"));
            modelBuilder.Entity<AcademicYearCoursePreRequisite>(x => x.ToDatabaseTable(databaseType, "AcademicYearCoursePreRequisites", "Enrollment"));
            modelBuilder.Entity<Group>(x => x.ToDatabaseTable(databaseType, "Groups", "Enrollment"));
            modelBuilder.Entity<Channel>(x => x.ToDatabaseTable(databaseType, "Channels", "Enrollment"));
            modelBuilder.Entity<Section>(x => x.ToDatabaseTable(databaseType, "Sections", "Enrollment"));
            modelBuilder.Entity<Faculty>(x => x.ToDatabaseTable(databaseType, "Faculties", "Enrollment"));
            modelBuilder.Entity<WorkingDay>(x => x.ToDatabaseTable(databaseType, "WorkingDays", "Enrollment"));
            modelBuilder.Entity<Evaluation>(x =>
            {
                x.ToDatabaseTable(databaseType, "Evaluations", "Enrollment");
            });
            modelBuilder.Entity<SectionEvaluation>(x => x.ToDatabaseTable(databaseType, "SectionEvaluations", "Enrollment"));

            modelBuilder.Entity<Resolution>(x => x.ToDatabaseTable(databaseType, "Resolutions", "Enrollment"));
            modelBuilder.Entity<SectionCode>(x => x.ToDatabaseTable(databaseType, "SectionCodes", "Enrollment"));
            modelBuilder.Entity<SectionGroup>(x => x.ToDatabaseTable(databaseType, "SectionGroups", "Enrollment"));
            modelBuilder.Entity<TmpEnrollment>(x => x.ToDatabaseTable(databaseType, "TmpEnrollments", "Enrollment"));
            modelBuilder.Entity<StudentSection>(x => x.ToDatabaseTable(databaseType, "StudentSections", "Enrollment"));
            modelBuilder.Entity<TeacherSection>(x => x.ToDatabaseTable(databaseType, "TeacherSections", "Enrollment"));
            modelBuilder.Entity<EvaluationType>(x =>
            {
                x.Property(r => r.Enabled).HasDefaultValue(true);
                x.ToDatabaseTable(databaseType, "EvaluationTypes", "Enrollment");
            });
            modelBuilder.Entity<AdminEnrollment>(x => x.ToDatabaseTable(databaseType, "AdminEnrollments", "Enrollment"));
            modelBuilder.Entity<EnrollmentConcept>(x => x.ToDatabaseTable(databaseType, "EnrollmentConcepts", "Enrollment"));
            modelBuilder.Entity<DisapprovedCourseConcept>(x => x.ToDatabaseTable(databaseType, "DisapprovedCourseConcepts", "Enrollment"));
            modelBuilder.Entity<CurriculumCredit>(x => x.ToDatabaseTable(databaseType, "CurriculumCredits", "Enrollment"));
            modelBuilder.Entity<EnrollmentMessage>(x => x.ToDatabaseTable(databaseType, "EnrollmentMessages", "Enrollment"));
            modelBuilder.Entity<AcademicYearCredit>(x =>
            {
                x.HasKey(t => new { t.CurriculumId, t.AcademicYear });
                x.ToDatabaseTable(databaseType, "AcademicYearCredits", "Enrollment");
            });
            modelBuilder.Entity<AcademicYearCourseCertificate>(x =>
            {
                x.HasKey(t => new { t.AcademicYearCourseId, t.CourseCertificateId });
                x.ToDatabaseTable(databaseType, "AcademicYearCourseCertificates", "Enrollment");
            });
            modelBuilder.Entity<StudentCourseCertificate>(x =>
            {
                x.HasKey(t => new { t.StudentId, t.CourseCertificateId });
                x.ToDatabaseTable(databaseType, "StudentCourseCertificates", "Enrollment");
            });
            modelBuilder.Entity<CourseCertificate>(x => x.ToDatabaseTable(databaseType, "CourseCertificates", "Enrollment"));
            modelBuilder.Entity<CourseSyllabusTeacher>(x => x.ToDatabaseTable(databaseType, "CourseSyllabusTeachers", "Enrollment"));
            modelBuilder.Entity<Competencie>(x => x.ToDatabaseTable(databaseType, "Competencies", "Enrollment"));
            modelBuilder.Entity<CurriculumCompetencie>(x =>
            {
                x.HasKey(t => new { t.CurriculumId, t.CompetencieId });
                x.ToDatabaseTable(databaseType, "CurriculumCompetencies", "Enrollment");
            });
            modelBuilder.Entity<ExtraCreditConfiguration>(x => x.ToDatabaseTable(databaseType, "ExtraCreditConfigurations", "Enrollment"));
            modelBuilder.Entity<AverageGradeCreditConfiguration>(x => x.ToDatabaseTable(databaseType, "AverageGradeCreditConfigurations", "Enrollment"));

            modelBuilder.Entity<EnrollmentFee>(x => x.ToDatabaseTable(databaseType, "EnrollmentFees", "Enrollment"));
            modelBuilder.Entity<EnrollmentFeeDetail>(x => x.ToDatabaseTable(databaseType, "EnrollmentFeeDetails", "Enrollment"));
            modelBuilder.Entity<EnrollmentFeeTerm>(x => x.ToDatabaseTable(databaseType, "EnrollmentFeeTerms", "Enrollment"));
            modelBuilder.Entity<EnrollmentFeeTemplate>(x => x.ToDatabaseTable(databaseType, "EnrollmentFeeTemplates", "Enrollment"));
            modelBuilder.Entity<EnrollmentFeeDetailTemplate>(x => x.ToDatabaseTable(databaseType, "EnrollmentFeeDetailTemplates", "Enrollment"));


            #endregion

            #region GENERALS
            modelBuilder.Entity<AcademicProgram>(x => x.ToDatabaseTable(databaseType, "AcademicPrograms", "Generals"));
            modelBuilder.Entity<Audit>(x => x.ToDatabaseTable(databaseType, "Audits", "Generals"));
            modelBuilder.Entity<Career>(x =>
            {
                x.ToDatabaseTable(databaseType, "Careers", "Generals");
                x.HasIndex(p => p.Name);
            });
            modelBuilder.Entity<CareerHistory>(x => x.ToDatabaseTable(databaseType, "CareerHistories", "Generals"));

            modelBuilder.Entity<Criterion>(x => x.ToDatabaseTable(databaseType, "Criterions", "Generals"));
            modelBuilder.Entity<Country>(x => x.ToDatabaseTable(databaseType, "Countries", "Generals"));
            modelBuilder.Entity<Dean>(x => x.ToDatabaseTable(databaseType, "Deans", "Generals"));
            modelBuilder.Entity<Department>(x => x.ToDatabaseTable(databaseType, "Departments", "Generals"));
            modelBuilder.Entity<District>(x => x.ToDatabaseTable(databaseType, "Districts", "Generals"));
            //modelBuilder.Entity<Doctor>(x => x.ToDatabaseTable(databaseType, "Doctors", "Generals"));
            modelBuilder.Entity<Holiday>(x => x.ToDatabaseTable(databaseType, "Holidays", "Generals"));
            modelBuilder.Entity<Notification>(x => x.ToDatabaseTable(databaseType, "Notifications", "Generals"));
            modelBuilder.Entity<Province>(x => x.ToDatabaseTable(databaseType, "Provinces", "Generals"));
            modelBuilder.Entity<RecordHistory>(x =>
            {
                x.ToDatabaseTable(databaseType, "RecordHistories", "Generals");
                x.Property(r => r.Status).HasDefaultValue(ConstantHelpers.RECORD_HISTORY_STATUS.GENERATED);
            });
            modelBuilder.Entity<RecordHistoryObservation>(x => x.ToDatabaseTable(databaseType, "RecordHistoryObservations", "Generals"));
            modelBuilder.Entity<Student>(x =>
            {
                x.ToDatabaseTable(databaseType, "Students", "Generals");
                x.Property(r => r.Condition).HasDefaultValue(ConstantHelpers.Student.Condition.REGULAR);
            });
            modelBuilder.Entity<Teacher>(x => x.ToDatabaseTable(databaseType, "Teachers", "Generals"));
            modelBuilder.Entity<UIT>(x =>
            {
                x.HasIndex(p => p.Year).IsUnique();
                x.ToDatabaseTable(databaseType, "UITs", "Generals");
            });
            modelBuilder.Entity<UserNotification>(x => x.ToDatabaseTable(databaseType, "UserNotifications", "Generals"));
            modelBuilder.Entity<LivingCost>(x => x.ToDatabaseTable(databaseType, "LivingCosts", "Generals"));
            modelBuilder.Entity<StudentExtraCareer>(x =>
            {
                x.HasKey(t => new { t.FirstStudentId, t.NewStudentId });
                x.ToDatabaseTable(databaseType, "StudentExtraCareers", "Generals");
            });
            modelBuilder.Entity<BeginningAnnouncement>(x => x.ToDatabaseTable(databaseType, "BeginningAnnouncements", "Generals"));
            modelBuilder.Entity<BeginningAnnouncementRole>(x =>
            {
                x.HasKey(t => new { t.BeginningAnnouncementId, t.RoleId });
                x.ToDatabaseTable(databaseType, "BeginningAnnouncementRoles", "Generals");
            });
            modelBuilder.Entity<UserAnnouncement>(x => x.ToDatabaseTable(databaseType, "UserAnnouncements", "Generals"));
            modelBuilder.Entity<EmailManagement>(x => x.ToDatabaseTable(databaseType, "EmailManagements", "Generals"));
            modelBuilder.Entity<CareerAccreditation>(x => x.ToDatabaseTable(databaseType, "CareerAccreditations", "Generals"));
            modelBuilder.Entity<CareerLicensure>(x => x.ToDatabaseTable(databaseType, "CareerLicensures", "Generals"));
            modelBuilder.Entity<YearInformation>(x =>
            {
                x.HasIndex(p => p.Year).IsUnique();
                x.ToDatabaseTable(databaseType, "YearInformations", "Generals");
            });

            modelBuilder.Entity<ExternalUser>(x => x.ToDatabaseTable(databaseType, "ExternalUsers", "Generals"));
            modelBuilder.Entity<Specialty>(x => x.ToDatabaseTable(databaseType, "Specialties", "Generals"));

            modelBuilder.Entity<UserLogin>(x =>
            {
                x.HasKey(t => new { t.UserId, t.System });
                x.ToDatabaseTable(databaseType, "UserLogins", "Generals");
            });

            modelBuilder.Entity<PortalNew>(x => x.ToDatabaseTable(databaseType, "PortalNews", "Generals"));
            modelBuilder.Entity<UserSuggestion>(x => x.ToDatabaseTable(databaseType, "UserSuggestions", "Generals"));
            modelBuilder.Entity<StudentBenefit>(x => x.ToDatabaseTable(databaseType, "StudentBenefits", "Generals"));
            modelBuilder.Entity<StudentCondition>(x => x.ToDatabaseTable(databaseType, "StudentConditions", "Generals"));

            modelBuilder.Entity<ExternalLink>(x => x.ToDatabaseTable(databaseType, "ExternalLinks", "Generals"));
            modelBuilder.Entity<StudentScale>(x => x.ToDatabaseTable(databaseType, "StudentScales", "Generals"));
            modelBuilder.Entity<ExchangeRate>(x =>
            {
                x.ToDatabaseTable(databaseType, "ExchangeRates", "Generals");
                x.Property(m => m.PurchasePrice).HasPrecision(18, 3);
                x.Property(m => m.SalePrice).HasPrecision(18, 3);
            });

            #endregion

            #region HELP DESK

            modelBuilder.Entity<Incident>(x => x.ToDatabaseTable(databaseType, "Incidents", "HelpDesk"));
            modelBuilder.Entity<Solution>(x => x.ToDatabaseTable(databaseType, "Solutions", "HelpDesk"));
            modelBuilder.Entity<Maintenance>(x => x.ToDatabaseTable(databaseType, "Maintenances", "HelpDesk"));
            modelBuilder.Entity<IncidentType>(x => x.ToDatabaseTable(databaseType, "IncidentTypes", "HelpDesk"));
            modelBuilder.Entity<IncidentSolution>(x => x.ToDatabaseTable(databaseType, "IncidentSolutions", "HelpDesk"));

            #endregion

            #region GEO

            modelBuilder.Entity<LaboratoyRequest>(x => x.ToDatabaseTable(databaseType, "LaboratoyRequests", "GEO"));
            modelBuilder.Entity<LaboratoryEquipments>(x => x.ToDatabaseTable(databaseType, "LaboratoryEquipments", "GEO"));
            modelBuilder.Entity<EquipmentReservations>(x => x.ToDatabaseTable(databaseType, "EquipmentReservations", "GEO"));
            modelBuilder.Entity<LaboratoryEquipmentLoans>(x => x.ToDatabaseTable(databaseType, "LaboratoryEquipmentLoans", "GEO"));
            modelBuilder.Entity<LaboratoryStudentAssitance>(x => x.ToDatabaseTable(databaseType, "LaboratoryStudentAssitance", "GEO"));

            #endregion

            #region INDICATORS

            modelBuilder.Entity<ScoreCard>(x => x.ToDatabaseTable(databaseType, "ScoreCards", "Indicators"));
            modelBuilder.Entity<Indicators>(x => x.ToDatabaseTable(databaseType, "Indicatorses", "Indicators"));
            modelBuilder.Entity<IndicatorProcesses>(x => x.ToDatabaseTable(databaseType, "IndicatorProcesses", "Indicators"));
            modelBuilder.Entity<IndicatorImprovement>(x => x.ToDatabaseTable(databaseType, "IndicatorImprovements", "Indicators"));
            modelBuilder.Entity<ResearchPerYear>(x => x.ToDatabaseTable(databaseType, "ResearchPerYears", "Indicators"));

            #endregion

            #region INTEREST GROUP

            modelBuilder.Entity<InterestGroupUser>(x =>
            {
                x.HasKey(t => new { t.InterestGroupId, t.UserId });
                x.ToDatabaseTable(databaseType, "InterestGroupUsers", "InterestGroup");
            });
            modelBuilder.Entity<Meeting>(x => x.ToDatabaseTable(databaseType, "Meetings", "InterestGroup"));

            modelBuilder.Entity<Conference>(x =>
            {
                x.ToDatabaseTable(databaseType, "Conferences", "InterestGroup");
                x.Property(r => r.Type).HasDefaultValue(ConstantHelpers.INTEREST_GROUP_CONFERENCE.TYPE.HANGOUT);
            });

            modelBuilder.Entity<MeetingFile>(x => x.ToDatabaseTable(databaseType, "MeetingFiles", "InterestGroup"));
            modelBuilder.Entity<MeetingUser>(x => x.ToDatabaseTable(databaseType, "MeetingUsers", "InterestGroup"));
            modelBuilder.Entity<InterestGroup>(x => x.ToDatabaseTable(databaseType, "InterestGroups", "InterestGroup"));
            modelBuilder.Entity<ConferenceUser>(x => x.ToDatabaseTable(databaseType, "ConferenceUsers", "InterestGroup"));
            modelBuilder.Entity<MeetingCriterion>(x => x.ToDatabaseTable(databaseType, "MeetingCriterions", "InterestGroup"));
            modelBuilder.Entity<InterestGroupFile>(x => x.ToDatabaseTable(databaseType, "InterestGroupFiles", "InterestGroup"));
            modelBuilder.Entity<InterestGroupForum>(x => x.ToDatabaseTable(databaseType, "InterestGroupForums", "InterestGroup"));
            modelBuilder.Entity<InterestGroupSurvey>(x =>
            {
                x.ToDatabaseTable(databaseType, "InterestGroupSurveys", "InterestGroup");
                x.Property(r => r.Type).HasDefaultValue(ConstantHelpers.INTEREST_GROUP_SURVEY.TYPE.INTERNAL);
            });

            modelBuilder.Entity<InterestGroupExternalUser>(x => x.ToDatabaseTable(databaseType, "InterestGroupExternalUsers", "InterestGroup"));
            modelBuilder.Entity<InterestGroupUserRefreshToken>(x => x.ToDatabaseTable(databaseType, "InterestGroupUserRefreshTokens", "InterestGroup"));

            #endregion

            #region INTRANET

            modelBuilder.Entity<AcademicRecordDepartment>(x =>
            {
                x.HasKey(t => new { t.UserId, t.AcademicDepartmentId });
                x.ToDatabaseTable(databaseType, "AcademicRecordDepartments", "Intranet");
            });
            modelBuilder.Entity<Event>(x => x.ToDatabaseTable(databaseType, "Events", "Intranet"));
            modelBuilder.Entity<Answer>(x => x.ToDatabaseTable(databaseType, "Answer", "Intranet"));
            modelBuilder.Entity<Class>(x => x.ToDatabaseTable(databaseType, "Classes", "Intranet"));
            modelBuilder.Entity<Campaign>(x => x.ToDatabaseTable(databaseType, "Campaigns", "Intranet"));
            modelBuilder.Entity<EventCareer>(x => x.ToDatabaseTable(databaseType, "EventCareers", "Intranet"));
            modelBuilder.Entity<EventCertification>(x => x.ToDatabaseTable(databaseType, "EventCertifications", "Intranet"));
            modelBuilder.Entity<EventEvidence>(x => x.ToDatabaseTable(databaseType, "EventEvidences", "Intranet"));
            modelBuilder.Entity<EventRole>(x => x.ToDatabaseTable(databaseType, "EventRoles", "Intranet"));
            modelBuilder.Entity<EventFile>(x => x.ToDatabaseTable(databaseType, "EventFiles", "Intranet"));
            modelBuilder.Entity<EventType>(x => x.ToDatabaseTable(databaseType, "EventTypes", "Intranet"));
            modelBuilder.Entity<ClassStudent>(x => x.ToDatabaseTable(databaseType, "ClassStudents", "Intranet"));
            modelBuilder.Entity<Announcement>(x => x.ToDatabaseTable(databaseType, "Announcements", "Intranet"));
            modelBuilder.Entity<AnswerByUser>(x => x.ToDatabaseTable(databaseType, "AnswerByUsers", "Intranet"));
            modelBuilder.Entity<ClassSchedule>(x => x.ToDatabaseTable(databaseType, "ClassSchedules", "Intranet"));
            modelBuilder.Entity<ClinicHistory>(x => x.ToDatabaseTable(databaseType, "ClinicHistories", "Intranet"));
            modelBuilder.Entity<CampaignPerson>(x => x.ToDatabaseTable(databaseType, "CampaignPersons", "Intranet"));
            modelBuilder.Entity<ExternalPerson>(x => x.ToDatabaseTable(databaseType, "ExternalPersons", "Intranet"));
            modelBuilder.Entity<ClassReschedule>(x => x.ToDatabaseTable(databaseType, "ClassReschedules", "Intranet"));
            modelBuilder.Entity<AcademicHistory>(x => x.ToDatabaseTable(databaseType, "AcademicHistories", "Intranet"));
            modelBuilder.Entity<AcademicSummary>(x =>
            {
                x.ToDatabaseTable(databaseType, "AcademicSummaries", "Intranet");
                x.Property(r => r.StudentCondition).HasDefaultValue(ConstantHelpers.Student.Condition.REGULAR);
            });
            modelBuilder.Entity<ExtracurricularCourse>(x => x.ToDatabaseTable(databaseType, "ExtracurricularCourses", "Intranet"));
            modelBuilder.Entity<AcademicHistoryDocument>(x => x.ToDatabaseTable(databaseType, "AcademicHistoryDocuments", "Intranet"));

            modelBuilder.Entity<ExtracurricularArea>(x => x.ToDatabaseTable(databaseType, "ExtracurricularAreas", "Intranet"));
            modelBuilder.Entity<ExtracurricularActivity>(x => x.ToDatabaseTable(databaseType, "ExtracurricularActivities", "Intranet"));
            modelBuilder.Entity<ExtracurricularActivityStudent>(x => x.ToDatabaseTable(databaseType, "ExtracurricularActivityStudents", "Intranet"));
            modelBuilder.Entity<ExtracurricularCourseGroup>(x => x.ToDatabaseTable(databaseType, "ExtracurricularCourseGroups", "Intranet"));
            modelBuilder.Entity<ExtracurricularCourseGroupAssistance>(x => x.ToDatabaseTable(databaseType, "ExtracurricularCourseGroupAssistances", "Intranet"));
            modelBuilder.Entity<ExtracurricularCourseGroupAssistanceStudent>(x =>
            {
                x.ToDatabaseTable(databaseType, "ExtracurricularCourseGroupAssistanceStudents", "Intranet");
                x.HasKey(t => new { t.GroupStudentId, t.GroupAssistanceId });

                x.HasOne(y => y.GroupStudent)
                .WithMany(y => y.GroupAssistanceStudents)
                .HasForeignKey(y => y.GroupStudentId)
                .HasConstraintName("FK_GroupStudent");

                x.HasOne(y => y.GroupAssistance)
                .WithMany(y => y.GroupAssistanceStudents)
                .HasForeignKey(y => y.GroupAssistanceId)
                .HasConstraintName("FK_GroupAssistance");
            });
            modelBuilder.Entity<ExtracurricularCourseGroupStudent>(x => x.ToDatabaseTable(databaseType, "ExtracurricularCourseGroupStudents", "Intranet"));
            modelBuilder.Entity<Forum>(x => x.ToDatabaseTable(databaseType, "Forums", "Intranet"));
            modelBuilder.Entity<ForumCareer>(x => x.ToDatabaseTable(databaseType, "ForumCareers", "Intranet"));
            modelBuilder.Entity<Grade>(x => x.ToDatabaseTable(databaseType, "Grades", "Intranet"));
            modelBuilder.Entity<GradeCorrection>(x => x.ToDatabaseTable(databaseType, "GradeCorrections", "Intranet"));
            modelBuilder.Entity<InstitutionalAlert>(x => x.ToDatabaseTable(databaseType, "InstitutionalAlerts", "Intranet"));
            modelBuilder.Entity<Invoice>(x => x.ToDatabaseTable(databaseType, "Invoices", "Intranet"));
            modelBuilder.Entity<InvoiceDetail>(x => x.ToDatabaseTable(databaseType, "InvoiceDetails", "Intranet"));
            modelBuilder.Entity<InvoiceVoucher>(x => x.ToDatabaseTable(databaseType, "InvoiceVouchers", "Intranet"));

            modelBuilder.Entity<MedicalAppointment>(x =>
            {
                x.ToDatabaseTable(databaseType, "MedicalAppointments", "Intranet");
                x.HasOne(s => s.PsychologicalRecord).WithOne(b => b.MedicalAppointment).HasForeignKey<PsychologicalRecord>(b => b.Id);
                x.HasOne(s => s.TopicConsult).WithOne(b => b.MedicalAppointment).HasForeignKey<TopicConsult>(b => b.Id);
                x.HasOne(s => s.NutritionalRecord).WithOne(b => b.MedicalAppointment).HasForeignKey<NutritionalRecord>(b => b.Id);
            });


            modelBuilder.Entity<MedicalConsultReason>(x => x.ToDatabaseTable(databaseType, "MedicalConsultReasons", "Intranet"));
            modelBuilder.Entity<MedicalDiagnosticImpression>(x => x.ToDatabaseTable(databaseType, "MedicalDiagnosticImpressions", "Intranet"));
            modelBuilder.Entity<MedicalFamilyHistory>(x => x.ToDatabaseTable(databaseType, "MedicalFamilyHistories", "Intranet"));
            modelBuilder.Entity<MedicalObservation>(x => x.ToDatabaseTable(databaseType, "MedicalObservations", "Intranet"));
            modelBuilder.Entity<MedicalPersonalHistory>(x => x.ToDatabaseTable(databaseType, "MedicalPersonalHistories", "Intranet"));
            modelBuilder.Entity<NutritionalRecord>(x => x.ToDatabaseTable(databaseType, "NutritionalRecords", "Intranet"));
            modelBuilder.Entity<PettyCash>(x => x.ToDatabaseTable(databaseType, "PettyCash", "Intranet"));
            modelBuilder.Entity<Post>(x => x.ToDatabaseTable(databaseType, "Posts", "Intranet"));
            modelBuilder.Entity<Prescription>(x => x.ToDatabaseTable(databaseType, "Prescriptions", "Intranet"));
            modelBuilder.Entity<PsychologicalDiagnostic>(x => x.ToDatabaseTable(databaseType, "PsychologicalDiagnostics", "Intranet"));
            modelBuilder.Entity<PsychologicalRecord>(x => x.ToDatabaseTable(databaseType, "PsychologicalRecords", "Intranet"));
            modelBuilder.Entity<PsychologicRecordDiagnostic>(x => x.ToDatabaseTable(databaseType, "PsychologicRecordDiagnostics", "Intranet"));
            modelBuilder.Entity<PsychologyCategory>(x => x.ToDatabaseTable(databaseType, "PsychologyCategories", "Intranet"));
            modelBuilder.Entity<PsychologyTestExam>(x => x.ToDatabaseTable(databaseType, "PsychologyTestExams", "Intranet"));
            modelBuilder.Entity<PsychologyTestQuestion>(x => x.ToDatabaseTable(databaseType, "PsychologyTestQuestions", "Intranet"));
            modelBuilder.Entity<Question>(x => x.ToDatabaseTable(databaseType, "Question", "Intranet"));
            modelBuilder.Entity<RolAnnouncement>(x => x.ToDatabaseTable(databaseType, "RolAnnouncements", "Intranet"));
            //modelBuilder.Entity<DoctorSpecialty>(x => x.ToDatabaseTable(databaseType, "DoctorSpecialties", "Intranet"));
            modelBuilder.Entity<StudentAbsenceJustification>(x => x.ToDatabaseTable(databaseType, "StudentAbsenceJustifications", "Intranet"));
            modelBuilder.Entity<StudentFamily>(x => x.ToDatabaseTable(databaseType, "StudentFamilies", "Intranet"));
            modelBuilder.Entity<StudentGroup>(x =>
            {
                x.HasKey(s => new { s.GroupId, s.StudentId });
                x.ToDatabaseTable(databaseType, "StudentGroups", "Intranet");
            });

            modelBuilder.Entity<StudentInformation>(x =>
            {
                x.ToDatabaseTable(databaseType, "StudentInformations", "Intranet");

                //x.HasOne(y => y.Student)
                //.WithOne(y => y.StudentInformation)
                //.HasForeignKey<Student>(y => y.StudentInformationId);
            });

            modelBuilder.Entity<SubstituteExam>(x => x.ToDatabaseTable(databaseType, "SubstituteExams", "Intranet"));

            modelBuilder.Entity<SubstituteExamDetail>(x => x.ToDatabaseTable(databaseType, "SubstituteExamDetails", "Intranet"));
            modelBuilder.Entity<Survey>(x => x.ToDatabaseTable(databaseType, "Survey", "Intranet"));
            modelBuilder.Entity<SurveyItem>(x => x.ToDatabaseTable(databaseType, "SurveyItems", "Intranet"));
            modelBuilder.Entity<SurveyUser>(x => x.ToDatabaseTable(databaseType, "SurveyUsers", "Intranet"));
            modelBuilder.Entity<Topic>(x => x.ToDatabaseTable(databaseType, "Topics", "Intranet"));
            modelBuilder.Entity<TopicConsult>(x => x.ToDatabaseTable(databaseType, "TopicConsults", "Intranet"));
            modelBuilder.Entity<Tutorial>(x => x.ToDatabaseTable(databaseType, "Tutorials", "Intranet"));
            modelBuilder.Entity<TutorialStudent>(x => x.ToDatabaseTable(databaseType, "TutorialStudents", "Intranet"));
            modelBuilder.Entity<UserAbsenceJustification>(x => x.ToDatabaseTable(databaseType, "UserAbsenceJustifications", "Intranet"));
            modelBuilder.Entity<StudentObservation>(x => x.ToDatabaseTable(databaseType, "StudentObservations", "Intranet"));
            modelBuilder.Entity<UserEvent>(x => x.ToDatabaseTable(databaseType, "UserEvents", "Intranet"));
            modelBuilder.Entity<EvaluationReport>(x => x.ToDatabaseTable(databaseType, "EvaluationReports", "Intranet"));
            modelBuilder.Entity<GradeRegistration>(x => x.ToDatabaseTable(databaseType, "GradeRegistrations", "Intranet"));
            modelBuilder.Entity<DeferredExam>(x => x.ToDatabaseTable(databaseType, "DeferredExams", "Intranet"));
            modelBuilder.Entity<DeferredExamStudent>(x => x.ToDatabaseTable(databaseType, "DeferredExamStudents", "Intranet"));
            modelBuilder.Entity<DirectedCourse>(x => x.ToDatabaseTable(databaseType, "DirectedCourses", "Intranet"));
            modelBuilder.Entity<DirectedCourseStudent>(x => x.ToDatabaseTable(databaseType, "DirectedCourseStudents", "Intranet"));
            modelBuilder.Entity<DisapprovedCourse>(x =>
            {
                x.HasKey(t => new
                {
                    t.StudentId,
                    t.CourseId
                });
                x.ToDatabaseTable(databaseType, "DisapprovedCourses", "Intranet");
            });
            modelBuilder.Entity<GradeReport>(x => x.ToDatabaseTable(databaseType, "GradeReports", "Intranet"));
            modelBuilder.Entity<DigitizedSignature>(x => x.ToDatabaseTable(databaseType, "DigitizedSignatures", "Intranet"));
            modelBuilder.Entity<UniversityAuthority>(x => x.ToDatabaseTable(databaseType, "UniversityAuthorities", "Intranet"));
            modelBuilder.Entity<UniversityAuthorityHistory>(x => x.ToDatabaseTable(databaseType, "UniversityAuthorityHistories", "Intranet"));
            modelBuilder.Entity<WelfareCategory>(x => x.ToDatabaseTable(databaseType, "WelfareCategories", "Intranet"));
            modelBuilder.Entity<SuggestionAndTip>(x => x.ToDatabaseTable(databaseType, "SuggestionAndTips", "Intranet"));

            modelBuilder.Entity<DeanFaculty>(x => x.ToDatabaseTable(databaseType, "DeanFaculties", "Intranet"));
            modelBuilder.Entity<GradeReportRequirement>(x => x.ToDatabaseTable(databaseType, "GradeReportRequirements", "Intranet"));
            modelBuilder.Entity<RecordConcept>(x => x.ToDatabaseTable(databaseType, "RecordConcepts", "Intranet"));
            modelBuilder.Entity<SectionsDuplicateContent>(x =>
            {
                x.HasKey(t => new { t.SectionAId, t.SectionBId });
                x.ToDatabaseTable(databaseType, "SectionsDuplicateContents", "Intranet");
            });

            modelBuilder.Entity<ExtraordinaryEvaluation>(x =>
            {
                x.Property(r => r.Type).HasDefaultValue(ConstantHelpers.Intranet.ExtraordinaryEvaluation.EXTRAORDINARY);
                x.ToDatabaseTable(databaseType, "ExtraordinaryEvaluations", "Intranet");
            });

            modelBuilder.Entity<ExtraordinaryEvaluationStudent>(x => x.ToDatabaseTable(databaseType, "ExtraordinaryEvaluationStudents", "Intranet"));
            modelBuilder.Entity<ExtraordinaryEvaluationCommittee>(x => x.ToDatabaseTable(databaseType, "ExtraordinaryEvaluationCommittees", "Intranet"));
            modelBuilder.Entity<GradeRecovery>(x => x.ToDatabaseTable(databaseType, "GradeRecoveries", "Intranet"));
            modelBuilder.Entity<GradeRecoveryExam>(x => x.ToDatabaseTable(databaseType, "GradeRecoveryExams", "Intranet"));
            modelBuilder.Entity<StudentIncomeScore>(x => x.ToDatabaseTable(databaseType, "StudentIncomeScores", "Intranet"));
            modelBuilder.Entity<TemporalGrade>(x => x.ToDatabaseTable(databaseType, "TemporalGrades", "Intranet"));
            modelBuilder.Entity<WeeklyAttendanceReport>(x =>
            {
                x.HasKey(t => new { t.SectionId, t.Week });
                x.ToDatabaseTable(databaseType, "WeeklyAttendanceReports", "Intranet");
            });
            modelBuilder.Entity<HistoricalReferredAppointment>(x => x.ToDatabaseTable(databaseType, "HistoricalReferredAppointments", "Intranet"));
            modelBuilder.Entity<ExamWeek>(x =>
            {
                x.HasKey(t => new { t.TermId, t.Week });
                x.ToDatabaseTable(databaseType, "ExamWeeks", "Intranet");
            });
            modelBuilder.Entity<GradeRectification>(x => x.ToDatabaseTable(databaseType, "GradeRectifications", "Intranet"));
            modelBuilder.Entity<SubstituteExamCorrection>(x => x.ToDatabaseTable(databaseType, "SubstituteExamCorrections", "Intranet"));

            modelBuilder.Entity<StudentPortfolio>(x =>
            {
                x.HasKey(t => new { t.StudentId, t.StudentPortfolioTypeId });
                x.ToDatabaseTable(databaseType, "StudentPortfolios", "Intranet");
            });

            modelBuilder.Entity<AKDEMIC.ENTITIES.Models.Intranet.DocumentFormat>(x =>
            {
                x.HasKey(t => t.Id);
                x.ToDatabaseTable(databaseType, "DocumentFormats", "Intranet");
            });

            modelBuilder.Entity<StudentGradeChangeHistory>(x => x.ToDatabaseTable(databaseType, "StudentGradeChangeHistories", "Intranet"));
            modelBuilder.Entity<StudentPortfolioType>(x =>
            {
                x.Property(r => r.Type).HasDefaultValue(ConstantHelpers.Intranet.STUDENT_PORTFOLIO_TYPE.GENERAL);
                x.ToDatabaseTable(databaseType, "StudentPortfolioTypes", "Intranet");
            });

            modelBuilder.Entity<CorrectionExam>(x => x.ToDatabaseTable(databaseType, "CorrectionExams", "Intranet"));
            modelBuilder.Entity<CorrectionExamStudent>(x => x.ToDatabaseTable(databaseType, "CorrectionExamStudents", "Intranet"));

            modelBuilder.Entity<TrainingCurse>(x => x.ToDatabaseTable(databaseType,"TrainingCurses","Intranet"));


            #endregion

            #region INSTITUTIONAL AGENDA

            modelBuilder.Entity<AgendaEvent>(x => x.ToDatabaseTable(databaseType, "AgendaEvents", "InstitutionalAgenda"));
            modelBuilder.Entity<Inscription>(x => x.ToDatabaseTable(databaseType, "Inscriptions", "InstitutionalAgenda"));
            modelBuilder.Entity<MenuOption>(x => x.ToDatabaseTable(databaseType, "MenuOptions", "InstitutionalAgenda"));
            modelBuilder.Entity<Subscription>(x => x.ToDatabaseTable(databaseType, "Subscriptions", "InstitutionalAgenda"));

            #endregion

            #region ContinuingEducation
            modelBuilder.Entity<ENTITIES.Models.ContinuingEducation.Activity>(x => x.ToDatabaseTable(databaseType, "FormationActivities", "ContinuingEducation"));
            modelBuilder.Entity<ENTITIES.Models.ContinuingEducation.ActivityType>(x => x.ToDatabaseTable(databaseType, "FormationActivityTypes", "ContinuingEducation"));
            modelBuilder.Entity<ENTITIES.Models.ContinuingEducation.Course>(x => x.ToDatabaseTable(databaseType, "FormationCourses", "ContinuingEducation"));
            modelBuilder.Entity<ENTITIES.Models.ContinuingEducation.Section>(x => x.ToDatabaseTable(databaseType, "FormationSections", "ContinuingEducation"));
            modelBuilder.Entity<ENTITIES.Models.ContinuingEducation.CourseArea>(x => x.ToDatabaseTable(databaseType, "FormationCourseAreas", "ContinuingEducation"));
            modelBuilder.Entity<ENTITIES.Models.ContinuingEducation.CourseExhibitor>(x => x.ToDatabaseTable(databaseType, "FormationCourseExhibitors", "ContinuingEducation"));
            modelBuilder.Entity<ENTITIES.Models.ContinuingEducation.RegisterSection>(x => x.ToDatabaseTable(databaseType, "FormationRegisterSections", "ContinuingEducation"));

            #endregion

            #region EVALUATION

            modelBuilder.Entity<CulturalActivity>(x => x.ToDatabaseTable(databaseType, "CulturalActivities", "Evaluation"));
            modelBuilder.Entity<CulturalActivityFile>(x => x.ToDatabaseTable(databaseType, "CulturalActivityFiles", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.Project>(x => x.ToDatabaseTable(databaseType, "Projects", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.Template>(x => x.ToDatabaseTable(databaseType, "Templates", "Evaluation"));
            modelBuilder.Entity<RegisterCourseConference>(x => x.ToDatabaseTable(databaseType, "RegisterCourseConferences", "Evaluation"));
            modelBuilder.Entity<RegisterCulturalActivity>(x => x.ToDatabaseTable(databaseType, "RegisterCulturalActivities", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.ProjectMember>(x => x.ToDatabaseTable(databaseType, "ProjectMembers", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.EvaluationArea>(x => x.ToDatabaseTable(databaseType, "EvaluationAreas", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.ProjectAdvance>(x => x.ToDatabaseTable(databaseType, "ProjectAdvances", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.ProjectActivity>(x => x.ToDatabaseTable(databaseType, "ProjectActivities", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.CompanyInterest>(x => x.ToDatabaseTable(databaseType, "CompanyInterests", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.CourseConference>(x => x.ToDatabaseTable(databaseType, "CourseConferences", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.SabaticalRequest>(x => x.ToDatabaseTable(databaseType, "SabaticalRequests", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.ProjectEvaluator>(x => x.ToDatabaseTable(databaseType, "ProjectEvaluators", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.CulturalActivityType>(x => x.ToDatabaseTable(databaseType, "CulturalActivityTypes", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.ProjectAdvanceHistoric>(x => x.ToDatabaseTable(databaseType, "ProjectAdvanceHistorics", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.ProjectScheduleHistoric>(x => x.ToDatabaseTable(databaseType, "ProjectScheduleHistorics", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.CourseConferenceExhibitor>(x => x.ToDatabaseTable(databaseType, "CourseConferenceExhibitors", "Evaluation"));

            modelBuilder.Entity<ProjectReport>(x => x.ToDatabaseTable(databaseType, "ProjectReports", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.ProjectConfiguration>(x => x.ToDatabaseTable(databaseType, "ProjectConfigurations", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.ProjectSustainableDevelopmentGoal>(x => x.ToDatabaseTable(databaseType, "ProjectSustainableDevelopmentGoals", "Evaluation"));
            modelBuilder.Entity<ENTITIES.Models.Evaluation.SustainableDevelopmentGoal>(x => x.ToDatabaseTable(databaseType, "SustainableDevelopmentGoals", "Evaluation"));

            #endregion

            #region INVESTIGATION

            modelBuilder.Entity<ENTITIES.Models.Investigation.Project>(x => x.ToDatabaseTable(databaseType, "Projects", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.Template>(x => x.ToDatabaseTable(databaseType, "Templates", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.LockedUser>(x => x.ToDatabaseTable(databaseType, "LockedUsers", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ResearchArea>(x => x.ToDatabaseTable(databaseType, "ResearchAreas", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ResearchLine>(x => x.ToDatabaseTable(databaseType, "ResearchLines", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ProjectRubric>(x => x.ToDatabaseTable(databaseType, "ProjectRubrics", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ProjectMember>(x => x.ToDatabaseTable(databaseType, "ProjectMembers", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ProjectAdvance>(x => x.ToDatabaseTable(databaseType, "ProjectAdvances", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.CompanyInterest>(x => x.ToDatabaseTable(databaseType, "CompanyInterests", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ResearchSubArea>(x => x.ToDatabaseTable(databaseType, "ResearchSubAreas", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ProjectSchedule>(x => x.ToDatabaseTable(databaseType, "ProjectSchedules", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ProjectActivity>(x => x.ToDatabaseTable(databaseType, "ProjectActivities", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.UserResearchArea>(x => x.ToDatabaseTable(databaseType, "UserResearchAreas", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.UserResearchLine>(x => x.ToDatabaseTable(databaseType, "UserResearchLines", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.SabaticalRequest>(x => x.ToDatabaseTable(databaseType, "SabaticalRequests", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ProjectItemScore>(x => x.ToDatabaseTable(databaseType, "ProjectItemScores", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ProjectEvaluator>(x => x.ToDatabaseTable(databaseType, "ProjectEvaluators", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ResearchCategory>(x => x.ToDatabaseTable(databaseType, "ResearchCategories", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ProjectRubricItem>(x => x.ToDatabaseTable(databaseType, "ProjectRubricItems", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ResearchDiscipline>(x => x.ToDatabaseTable(databaseType, "ResearchDisciplines", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ResearchLineHistoric>(x => x.ToDatabaseTable(databaseType, "ResearchLineHistorics", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ProjectAdvanceHistoric>(x => x.ToDatabaseTable(databaseType, "ProjectAdvanceHistorics", "Investigation"));

            modelBuilder.Entity<ENTITIES.Models.Investigation.Activity>(x => x.ToDatabaseTable(databaseType, "InvestigationActivities", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ActivityFile>(x => x.ToDatabaseTable(databaseType, "InvestigationActivityFiles", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.ActivityType>(x => x.ToDatabaseTable(databaseType, "InvestigationActivityTypes", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.StudentActivity>(x => x.ToDatabaseTable(databaseType, "InvestigationStudentActivities", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.StudentActivityFile>(x => x.ToDatabaseTable(databaseType, "InvestigationStudentActivityFiles", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.StudentFeedbackActivity>(x => x.ToDatabaseTable(databaseType, "InvestigationStudentFeedbackActivities", "Investigation"));
            modelBuilder.Entity<ENTITIES.Models.Investigation.StudentFeedbackActivityFile>(x => x.ToDatabaseTable(databaseType, "InvestigationStudentFeedbackActivityFiles", "Investigation"));

            #endregion

            #region JOB EXCHANGE

            modelBuilder.Entity<Ability>(x => x.ToDatabaseTable(databaseType, "Abilities", "JobExchange"));
            modelBuilder.Entity<Company>(x => x.ToDatabaseTable(databaseType, "Companies", "JobExchange"));
            modelBuilder.Entity<AgreementFormat>(x => x.ToDatabaseTable(databaseType, "AgreementFormats", "JobExchange"));
            modelBuilder.Entity<AgreementTemplate>(x => x.ToDatabaseTable(databaseType, "AgreementTemplates", "JobExchange"));
            modelBuilder.Entity<JobOffer>(x => x.ToDatabaseTable(databaseType, "JobOffers", "JobExchange"));
            modelBuilder.Entity<ForumPost>(x => x.ToDatabaseTable(databaseType, "ForumPosts", "JobExchange"));
            modelBuilder.Entity<Agreement>(x => x.ToDatabaseTable(databaseType, "Agreements", "JobExchange"));
            modelBuilder.Entity<ForumTopic>(x => x.ToDatabaseTable(databaseType, "ForumTopics", "JobExchange"));
            modelBuilder.Entity<ImageCompany>(x => x.ToDatabaseTable(databaseType, "ImageCompanies", "JobExchange"));
            modelBuilder.Entity<ChannelContact>(x => x.ToDatabaseTable(databaseType, "ChannelContacts", "JobExchange"));
            modelBuilder.Entity<CurriculumVitae>(x => x.ToDatabaseTable(databaseType, "CurriculumVitaes", "JobExchange"));
            modelBuilder.Entity<JobOfferAbility>(x =>
            {
                x.HasKey(t => new { t.JobOfferId, t.AbilityId });
                x.ToDatabaseTable(databaseType, "JobOfferAbilities", "JobExchange");
            });
            modelBuilder.Entity<JobOfferApplication>(x =>
            {
                x.HasKey(t => new { t.JobOfferId, t.StudentId });
                x.ToDatabaseTable(databaseType, "JobOfferApplications", "JobExchange");
            });
            modelBuilder.Entity<JobOfferCareer>(x =>
            {
                x.HasKey(t => new { t.JobOfferId, t.CareerId });
                x.ToDatabaseTable(databaseType, "JobOfferCareers", "JobExchange");
            });
            modelBuilder.Entity<JobOfferLanguage>(x =>
            {
                x.HasKey(t => new { t.JobOfferId, t.LanguageId });
                x.ToDatabaseTable(databaseType, "JobOfferLanguages", "JobExchange");
            });
            modelBuilder.Entity<StudentAbility>(x =>
            {
                x.ToDatabaseTable(databaseType, "StudentAbilities", "JobExchange");
                x.HasKey(t => new { t.StudentId, t.AbilityId });
            });
            modelBuilder.Entity<StudentLanguage>(x =>
            {
                x.ToDatabaseTable(databaseType, "StudentLanguages", "JobExchange");
                x.HasKey(t => new { t.StudentId, t.LanguageId });
            });
            modelBuilder.Entity<Language>(x => x.ToDatabaseTable(databaseType, "Languages", "JobExchange"));
            modelBuilder.Entity<Sector>(x => x.ToDatabaseTable(databaseType, "Sectors", "JobExchange"));
            modelBuilder.Entity<Sede>(x => x.ToDatabaseTable(databaseType, "Sedes", "JobExchange"));
            modelBuilder.Entity<StudentExperience>(x => x.ToDatabaseTable(databaseType, "StudentExperiences", "JobExchange"));
            modelBuilder.Entity<StudentCertificate>(x => x.ToDatabaseTable(databaseType, "StudentCertificates", "JobExchange"));
            modelBuilder.Entity<StudentAcademicEducation>(x => x.ToDatabaseTable(databaseType, "StudentAcademicEducations", "JobExchange"));
            modelBuilder.Entity<IncubationCall>(x => x.ToDatabaseTable(databaseType, "IncubationCalls", "JobExchange"));
            modelBuilder.Entity<CoordinatorCareer>(x => x.ToDatabaseTable(databaseType, "CoordinatorCareers", "JobExchange"));
            modelBuilder.Entity<EmployeeSurvey>(x => x.ToDatabaseTable(databaseType, "EmployeeSurveys", "JobExchange"));
            modelBuilder.Entity<ENTITIES.Models.JobExchange.InternshipRequest>(x => x.ToDatabaseTable(databaseType, "InternshipRequests", "JobExchange"));
            modelBuilder.Entity<CompanySize>(x => x.ToDatabaseTable(databaseType, "CompanySizes", "JobExchange"));
            modelBuilder.Entity<CompanyType>(x => x.ToDatabaseTable(databaseType, "CompanyTypes", "JobExchange"));
            modelBuilder.Entity<EconomicActivity>(x => x.ToDatabaseTable(databaseType, "EconomicActivities", "JobExchange"));
            modelBuilder.Entity<DidacticalMaterial>(x => x.ToDatabaseTable(databaseType, "DidacticalMaterials", "JobExchange"));
            modelBuilder.Entity<DidacticalMaterialFile>(x => x.ToDatabaseTable(databaseType, "DidacticalMaterialFiles", "JobExchange"));
            modelBuilder.Entity<StudentComplementaryStudy>(x => x.ToDatabaseTable(databaseType, "StudentComplementaryStudies", "JobExchange"));
            modelBuilder.Entity<StudentSpecialityCertificate>(x => x.ToDatabaseTable(databaseType, "StudentSpecialityCertificates", "JobExchange"));
            modelBuilder.Entity<StudentGraduatedSurvey>(x => x.ToDatabaseTable(databaseType, "StudentGraduatedSurveys", "JobExchange"));
            modelBuilder.Entity<EconomicActivityDivision>(x => x.ToDatabaseTable(databaseType, "EconomicActivityDivisions", "JobExchange"));
            modelBuilder.Entity<EconomicActivitySection>(x => x.ToDatabaseTable(databaseType, "EconomicActivitySections", "JobExchange"));

            modelBuilder.Entity<FavoriteCompany>(x =>
            {
                x.HasKey(t => new { t.UserId, t.CompanyId });
                x.ToDatabaseTable(databaseType, "FavoriteCompanies", "JobExchange");
            });

            #endregion

            #region KARDEX

            modelBuilder.Entity<Category>(x => x.ToDatabaseTable(databaseType, "Categories", "Kardex"));
            modelBuilder.Entity<MeasurementUnit>(x => x.ToDatabaseTable(databaseType, "MeasurementUnits", "Kardex"));
            modelBuilder.Entity<Product>(x => x.ToDatabaseTable(databaseType, "Products", "Kardex"));
            modelBuilder.Entity<ProductInput>(x => x.ToDatabaseTable(databaseType, "ProductInputs", "Kardex"));
            modelBuilder.Entity<ProductOutput>(x => x.ToDatabaseTable(databaseType, "ProductOutputs", "Kardex"));
            modelBuilder.Entity<ProductType>(x => x.ToDatabaseTable(databaseType, "ProductTypes", "Kardex"));
            modelBuilder.Entity<SubCategory>(x => x.ToDatabaseTable(databaseType, "SubCategories", "Kardex"));
            modelBuilder.Entity<Warehouse>(x => x.ToDatabaseTable(databaseType, "Warehouses", "Kardex"));

            #endregion

            #region LAURASSIA

            modelBuilder.Entity<Chat>(x => x.ToDatabaseTable(databaseType, "Chats", "Laurassia"));
            modelBuilder.Entity<Choice>(x => x.ToDatabaseTable(databaseType, "Choices", "Laurassia"));
            modelBuilder.Entity<Consult>(x => x.ToDatabaseTable(databaseType, "Consults", "Laurassia"));
            modelBuilder.Entity<Element>(x => x.ToDatabaseTable(databaseType, "Elements", "Laurassia"));
            modelBuilder.Entity<Calendar>(x => x.ToDatabaseTable(databaseType, "Calendars", "Laurassia"));
            modelBuilder.Entity<Calculated>(x => x.ToDatabaseTable(databaseType, "Calculateds", "Laurassia"));
            modelBuilder.Entity<Alternative>(x => x.ToDatabaseTable(databaseType, "Alternatives", "Laurassia"));
            modelBuilder.Entity<ConsultType>(x => x.ToDatabaseTable(databaseType, "ConsultTypes", "Laurassia"));
            modelBuilder.Entity<CourseGuide>(x => x.ToDatabaseTable(databaseType, "CourseGuides", "Laurassia"));
            //modelBuilder.Entity<LoginDetail>(x => x.ToDatabaseTable(databaseType, "LoginDetails", "Laurassia"));
            modelBuilder.Entity<ExamSegment>(x => x.ToDatabaseTable(databaseType, "ExamSegments", "Laurassia"));
            modelBuilder.Entity<HomeworkFile>(x => x.ToDatabaseTable(databaseType, "HomeworkFiles", "Laurassia"));
            modelBuilder.Entity<RubricItemDetail>(x => x.ToDatabaseTable(databaseType, "RubricItemDetails", "Laurassia"));
            modelBuilder.Entity<FrequentQuestion>(x => x.ToDatabaseTable(databaseType, "FrequentQuestions", "Laurassia"));
            modelBuilder.Entity<RecoveryExamStudent>(x => x.ToDatabaseTable(databaseType, "RecoveryExamStudents", "Laurassia"));
            modelBuilder.Entity<HomeworkStudentFile>(x => x.ToDatabaseTable(databaseType, "HomeworkStudentFiles", "Laurassia"));
            modelBuilder.Entity<RecoveryHomeworkStudent>(x => x.ToDatabaseTable(databaseType, "RecoveryHomeworkStudents", "Laurassia"));
            modelBuilder.Entity<GeneralAnnouncement>(x => x.ToDatabaseTable(databaseType, "GeneralAnnouncements", "Laurassia"));
            modelBuilder.Entity<VirtualClassRecording>(x => x.ToDatabaseTable(databaseType, "VirtualClassRecordings", "Laurassia"));
            modelBuilder.Entity<CalculatedAlternative>(x => x.ToDatabaseTable(databaseType, "CalculatedAlternatives", "Laurassia"));
            modelBuilder.Entity<VirtualClassCredential>(x => x.ToDatabaseTable(databaseType, "VirtualClassCredentials", "Laurassia"));

            modelBuilder.Entity<FrequentQuestionLink>(x => x.ToDatabaseTable(databaseType, "FrequentQuestionLinks", "Laurassia"));
            modelBuilder.Entity<OtherQualification>(x => x.ToDatabaseTable(databaseType, "OtherQualifications", "Laurassia"));
            modelBuilder.Entity<OtherQualificationStudent>(x => x.ToDatabaseTable(databaseType, "OtherQualificationStudents", "Laurassia"));

            modelBuilder.Entity<Contest>(x => x.ToDatabaseTable(databaseType, "Contests", "Laurassia"));
            modelBuilder.Entity<ContestStudent>(x => x.ToDatabaseTable(databaseType, "ContestStudents", "Laurassia"));
            modelBuilder.Entity<ContestRequirement>(x => x.ToDatabaseTable(databaseType, "ContestRequirements", "Laurassia"));
            modelBuilder.Entity<ContestStudentRequirement>(x => x.ToDatabaseTable(databaseType, "ContestStudentRequirements", "Laurassia"));

            modelBuilder.Entity<ExamResolution>(x => x.ToDatabaseTable(databaseType, "ExamResolutions", "Laurassia"));
            //modelBuilder.Entity<ExamResolution>(x =>
            //    {
            //        x.HasKey(t => new { t.Id });
            //        x.ToDatabaseTable(databaseType, "ExamResolutions", "Laurassia");
            //    });
            modelBuilder.Entity<GroupChoice>(x => x.ToDatabaseTable(databaseType, "GroupChoices", "Laurassia"));
            //modelBuilder.Entity<GroupChoice>(x =>
            //{
            //    x.HasKey(bc => new { bc.Id });
            //    x.HasMany(bc => bc.Choices)
            //        .WithOne(c => c.GroupChoice)
            //        .HasForeignKey(a => a.GroupChoiceId);
            //    x.HasOne(bc => bc.VQuestion)
            //        .WithMany(b => b.GroupChoices)
            //        .HasForeignKey(bc => bc.VQuestionId);
            //    x.ToDatabaseTable(databaseType, "GroupChoices", "Laurassia");
            //});
            modelBuilder.Entity<Homework>(x => x.ToDatabaseTable(databaseType, "Homeworks", "Laurassia"));
            modelBuilder.Entity<HomeworkStudent>(x => x.ToDatabaseTable(databaseType, "HomeworkStudents", "Laurassia"));
            //modelBuilder.Entity<HomeworkStudent>(x =>
            //{
            //    x.HasOne(pt => pt.Homework)
            //        .WithMany(p => p.HomeworkStudent)
            //        .HasForeignKey(pt => pt.HomeworkId);
            //    x.ToDatabaseTable(databaseType, "HomeworkStudents", "Laurassia");
            //});
            modelBuilder.Entity<New>(x => x.ToDatabaseTable(databaseType, "News", "Laurassia"));
            //modelBuilder.Entity<Login>(x => x.ToDatabaseTable(databaseType, "Logins", "Laurassia"));
            modelBuilder.Entity<Manual>(x => x.ToDatabaseTable(databaseType, "Manuals", "Laurassia"));
            modelBuilder.Entity<Number>(x => x.ToDatabaseTable(databaseType, "Numbers", "Laurassia"));
            modelBuilder.Entity<Message>(x => x.ToDatabaseTable(databaseType, "Messages", "Laurassia"));
            modelBuilder.Entity<Reading>(x => x.ToDatabaseTable(databaseType, "Readings", "Laurassia"));
            modelBuilder.Entity<Schedule>(x => x.ToDatabaseTable(databaseType, "Schedules", "Laurassia"));
            modelBuilder.Entity<RubricItem>(x => x.ToDatabaseTable(databaseType, "RubricItems", "Laurassia"));
            modelBuilder.Entity<RubricItemStudent>(x => x.ToDatabaseTable(databaseType, "RubricItemStudents", "Laurassia"));
            modelBuilder.Entity<SectionAnnouncement>(x => x.ToDatabaseTable(databaseType, "SectionAnnouncements", "Laurassia"));
            //modelBuilder.Entity<SectionSchedule>(x => x.ToDatabaseTable(databaseType, "SectionSchedules", "Laurassia"));
            modelBuilder.Entity<SectionSchedule>(x =>
            {
                x.HasKey(t => new { t.SectionId, t.ScheduleId });
                x.HasOne(pt => pt.Schedule)
                    .WithMany(t => t.SectionSchedule)
                    .HasForeignKey(pt => pt.ScheduleId);
                x.ToDatabaseTable(databaseType, "SectionSchedules", "Laurassia");
            });
            modelBuilder.Entity<Content>(x => x.ToDatabaseTable(databaseType, "Contents", "Laurassia"));
            modelBuilder.Entity<Resource>(x => x.ToDatabaseTable(databaseType, "Resources", "Laurassia"));
            modelBuilder.Entity<SubImage>(x => x.ToDatabaseTable(databaseType, "SubImages", "Laurassia"));
            modelBuilder.Entity<SubInput>(x => x.ToDatabaseTable(databaseType, "SubInputs", "Laurassia"));
            modelBuilder.Entity<SubQuestion>(x => x.ToDatabaseTable(databaseType, "SubQuestions", "Laurassia"));
            modelBuilder.Entity<Variable>(x => x.ToDatabaseTable(databaseType, "Variables", "Laurassia"));
            //modelBuilder.Entity<Variable>(x =>
            //{
            //    x.HasKey(bc => new { bc.Id });
            //    x.ToDatabaseTable(databaseType, "Variables", "Laurassia");
            //});
            modelBuilder.Entity<VExam>(x => x.ToDatabaseTable(databaseType, "VExams", "Laurassia"));
            modelBuilder.Entity<VExamDetail>(x => x.ToDatabaseTable(databaseType, "VExamDetails", "Laurassia"));
            //        modelBuilder.Entity<VExamDetail>(x =>
            //        {
            ////x.HasKey(t => new { t.Id, t.VExamId, t.StudentId, t.VQuestionId }); // Evil lurks around here. DON'T TOUCH
            //            x.ToDatabaseTable(databaseType, "VExamDetails", "Laurassia");
            //        });
            modelBuilder.Entity<VExamFeedback>(x => x.ToDatabaseTable(databaseType, "VExamFeedbacks", "Laurassia"));
            modelBuilder.Entity<VExamStudent>(x => x.ToDatabaseTable(databaseType, "VExamStudents", "Laurassia"));
            //modelBuilder.Entity<VExamStudent>(x =>
            //{
            //    x.HasKey(t => new { t.Id });
            //    x.HasOne(pt => pt.VExam)
            //        .WithMany(p => p.VExamStudent)
            //        .HasForeignKey(pt => pt.VExamId);
            //    x.ToDatabaseTable(databaseType, "VExamStudents", "Laurassia");
            //});
            modelBuilder.Entity<VForum>(x => x.ToDatabaseTable(databaseType, "VForums", "Laurassia"));
            modelBuilder.Entity<VForumFile>(x => x.ToDatabaseTable(databaseType, "VForumFiles", "Laurassia"));
            modelBuilder.Entity<VForumChildFile>(x => x.ToDatabaseTable(databaseType, "VForumChildFiles", "Laurassia"));
            modelBuilder.Entity<VForumChild>(x => x.ToDatabaseTable(databaseType, "VForumChildren", "Laurassia"));
            //modelBuilder.Entity<VForumChild>(x =>
            //{
            //    x.HasKey(t => new { t.Id });
            //    x.HasOne(pt => pt.VForum)
            //        .WithMany(p => p.VForumChild)
            //        .HasForeignKey(pt => pt.VForumId);
            //    x.ToDatabaseTable(databaseType, "VForumChildren", "Laurassia");
            //});
            modelBuilder.Entity<VGroup>(x => x.ToDatabaseTable(databaseType, "VGroups", "Laurassia"));
            modelBuilder.Entity<VGroupFile>(x => x.ToDatabaseTable(databaseType, "VGroupFiles", "Laurassia"));
            //modelBuilder.Entity<VGroupUser>(x => x.ToDatabaseTable(databaseType, "VGroupUsers", "Laurassia"));
            modelBuilder.Entity<VGroupUser>(x =>
            {
                x.HasKey(t => new { t.VGroupId, t.UserId });
                x.HasOne(y => y.VGroup)
                    .WithMany(y => y.VGroupUser)
                    .HasForeignKey(h => h.VGroupId)
                    .OnDelete(DeleteBehavior.Cascade);
                x.ToDatabaseTable(databaseType, "VGroupUsers", "Laurassia");
            });
            modelBuilder.Entity<VirtualClass>(x => x.ToDatabaseTable(databaseType, "VirtualClass", "Laurassia"));
            modelBuilder.Entity<VirtualClassDetail>(x => x.ToDatabaseTable(databaseType, "VirtualClassDetails", "Laurassia"));
            modelBuilder.Entity<VirtualClassStudent>(x => x.ToDatabaseTable(databaseType, "VirtualClassStudents", "Laurassia"));
            modelBuilder.Entity<VMessageGroup>(x => x.ToDatabaseTable(databaseType, "VMessageGroups", "Laurassia"));
            modelBuilder.Entity<VQuestion>(x => x.ToDatabaseTable(databaseType, "VQuestions", "Laurassia"));
            modelBuilder.Entity<Wiki>(x => x.ToDatabaseTable(databaseType, "Wikis", "Laurassia"));
            modelBuilder.Entity<SupervisorCareer>(x => x.ToDatabaseTable(databaseType, "SupervisorCareers", "Laurassia"));
            modelBuilder.Entity<SupervisorDepartment>(x => x.ToDatabaseTable(databaseType, "SupervisorDepartments", "Laurassia"));
            modelBuilder.Entity<QualificationLog>(x => x.ToDatabaseTable(databaseType, "QualificationLogs", "Laurassia"));
            modelBuilder.Entity<HomeworkStudentFeedbackFile>(x => x.ToDatabaseTable(databaseType, "HomeworkStudentFeedbackFiles", "Laurassia"));
            modelBuilder.Entity<QuestionRubric>(x => x.ToDatabaseTable(databaseType, "QuestionRubrics", "Laurassia"));
            modelBuilder.Entity<QuestionRubricDetail>(x => x.ToDatabaseTable(databaseType, "QuestionRubricDetails", "Laurassia"));
            modelBuilder.Entity<QuestionRubricStudent>(x => x.ToDatabaseTable(databaseType, "QuestionRubricStudents", "Laurassia"));
            modelBuilder.Entity<VirtualClassLog>(x => x.ToDatabaseTable(databaseType, "VirtualClassLogs", "Laurassia"));
            modelBuilder.Entity<Certificate>(x => x.ToDatabaseTable(databaseType, "Certificates", "Laurassia"));

            #endregion

            #region LANGUAGE CENTER

            modelBuilder.Entity<LanguageAcademicHistory>(x => x.ToDatabaseTable(databaseType, "LanguageAcademicHistories", "LanguageCenter"));
            modelBuilder.Entity<LanguageCourse>(x => x.ToDatabaseTable(databaseType, "LanguageCourses", "LanguageCenter"));
            modelBuilder.Entity<LanguageLevel>(x => x.ToDatabaseTable(databaseType, "LanguageLevels", "LanguageCenter"));
            modelBuilder.Entity<LanguagePayment>(x => x.ToDatabaseTable(databaseType, "LanguagePayments", "LanguageCenter"));
            modelBuilder.Entity<LanguagePaymentUserProcedure>(x => x.ToDatabaseTable(databaseType, "LanguagePaymentUserProcedures", "LanguageCenter"));
            modelBuilder.Entity<LanguageQualification>(x => x.ToDatabaseTable(databaseType, "LanguageQualifications", "LanguageCenter"));
            modelBuilder.Entity<LanguageSection>(x => x.ToDatabaseTable(databaseType, "LanguageSections", "LanguageCenter"));
            modelBuilder.Entity<LanguageSectionSchedule>(x => x.ToDatabaseTable(databaseType, "LanguageSectionSchedules", "LanguageCenter"));
            modelBuilder.Entity<LanguageSectionStudent>(x => x.ToDatabaseTable(databaseType, "LanguageSectionStudents", "LanguageCenter"));
            modelBuilder.Entity<LanguageStudentAssistance>(x => x.ToDatabaseTable(databaseType, "LanguageStudentAssistances", "LanguageCenter"));
            modelBuilder.Entity<LanguageStudentQualification>(x => x.ToDatabaseTable(databaseType, "LanguageStudentQualifications", "LanguageCenter"));

            #endregion

            #region OPENID

            //modelBuilder.Entity<Application>(x => x.ToDatabaseTable(databaseType, "Applications", "OpenId"));
            //modelBuilder.Entity<ApplicationUserAuthorized>(x => x.ToDatabaseTable(databaseType, "ApplicationUserAuthorized", "OpenId"));
            //modelBuilder.Entity<OpenIddictApplication>(x => x.ToDatabaseTable(databaseType, "OpenIddictApplications", "OpenId"));
            //modelBuilder.Entity<OpenIddictScope>(x => x.ToDatabaseTable(databaseType, "OpenIddictScopes", "OpenId"));
            //modelBuilder.Entity<OpenIddictToken>(x => x.ToDatabaseTable(databaseType, "OpenIddictTokens", "OpenId"));

            #endregion

            #region PAYROLL

            modelBuilder.Entity<Bank>(x => x.ToDatabaseTable(databaseType, "Banks", "Payroll"));
            modelBuilder.Entity<PaymentMethod>(x => x.ToDatabaseTable(databaseType, "PaymentMethods", "Payroll"));
            modelBuilder.Entity<Payroll>(x => x.ToDatabaseTable(databaseType, "Payrolls", "Payroll"));
            modelBuilder.Entity<PayrollClass>(x => x.ToDatabaseTable(databaseType, "PayrollClasses", "Payroll"));
            modelBuilder.Entity<PayrollClassWageItemFormula>(x => x.ToDatabaseTable(databaseType, "PayrollClassWageItemFormulas", "Payroll"));
            modelBuilder.Entity<PayrollType>(x => x.ToDatabaseTable(databaseType, "PayrollTypes", "Payroll"));
            modelBuilder.Entity<PayrollWorker>(x => x.ToDatabaseTable(databaseType, "PayrollWorkers", "Payroll"));
            modelBuilder.Entity<PayrollWorkerWageItem>(x => x.ToDatabaseTable(databaseType, "PayrollWorkerWageItems", "Payroll"));
            modelBuilder.Entity<WageItem>(x => x.ToDatabaseTable(databaseType, "WageItems", "Payroll"));
            modelBuilder.Entity<WorkArea>(x => x.ToDatabaseTable(databaseType, "WorkAreas", "Payroll"));
            modelBuilder.Entity<Worker>(x => x.ToDatabaseTable(databaseType, "Workers", "Payroll"));
            modelBuilder.Entity<WorkerHistory>(x => x.ToDatabaseTable(databaseType, "WorkerHistories", "Payroll"));
            modelBuilder.Entity<WorkerOcupation>(x => x.ToDatabaseTable(databaseType, "WorkerOcupations", "Payroll"));
            modelBuilder.Entity<WorkingTerm>(x => x.ToDatabaseTable(databaseType, "WorkingTerms", "Payroll"));
            modelBuilder.Entity<AdministrativeTable>(x => x.ToDatabaseTable(databaseType, "AdministrativeTables", "Payroll"));
            modelBuilder.Entity<PayrollConcept>(x => x.ToDatabaseTable(databaseType, "PayrollConcepts", "Payroll"));
            modelBuilder.Entity<WageLevel>(x => x.ToDatabaseTable(databaseType, "WageLevels", "Payroll"));
            modelBuilder.Entity<WorkShift>(x =>
            {
                x.HasAlternateKey(y => y.Code);
                x.Property(y => y.Code).ValueGeneratedOnAdd();
                x.ToDatabaseTable(databaseType, "WorkShifts", "Payroll");
            });
            modelBuilder.Entity<EmployerMaintenance>(x => x.ToDatabaseTable(databaseType, "EmployerMaintenances", "Payroll"));
            modelBuilder.Entity<RemunerationMaintenance>(x => x.ToDatabaseTable(databaseType, "RemunerationMaintenances", "Payroll"));
            modelBuilder.Entity<WorkerRemuneration>(x => x.ToDatabaseTable(databaseType, "WorkerRemunerations", "Payroll"));
            modelBuilder.Entity<WorkerTermPayrollDetail>(x => x.ToDatabaseTable(databaseType, "WorkerTermPayrollDetails", "Payroll"));
            modelBuilder.Entity<RemunerationPayrollType>(x => x.ToDatabaseTable(databaseType, "RemunerationPayrollTypes", "Payroll"));
            modelBuilder.Entity<ConceptType>(x => x.ToDatabaseTable(databaseType, "ConceptTypes", "Payroll"));
            
            #endregion

            #region PERMISSION
            modelBuilder.Entity<RolePermission>(x => x.ToDatabaseTable(databaseType, "RolePermission", "Permission"));

            #endregion

            #region PORTAL

            modelBuilder.Entity<FinancialExecution>(x => x.ToDatabaseTable(databaseType, "FinancialExecutions", "Portal"));
            modelBuilder.Entity<FinancialExecutionDetail>(x => x.ToDatabaseTable(databaseType, "FinancialExecutionDetails", "Portal"));
            modelBuilder.Entity<FinancialStatement>(x => x.ToDatabaseTable(databaseType, "FinancialStatements", "Portal"));
            modelBuilder.Entity<FinancialStatementFile>(x => x.ToDatabaseTable(databaseType, "FinancialStatementFiles", "Portal"));
            modelBuilder.Entity<InstitutionalActivity>(x => x.ToDatabaseTable(databaseType, "InstitutionalActivities", "Portal"));
            modelBuilder.Entity<InstitutionalActivityFile>(x => x.ToDatabaseTable(databaseType, "InstitutionalActivityFiles", "Portal"));
            modelBuilder.Entity<SessionRecord>(x => x.ToDatabaseTable(databaseType, "SessionRecords", "Portal"));
            modelBuilder.Entity<SessionRecordFile>(x => x.ToDatabaseTable(databaseType, "SessionRecordFiles", "Portal"));
            modelBuilder.Entity<TransparencyCompetition>(x => x.ToDatabaseTable(databaseType, "TransparencyCompetitions", "Portal"));
            modelBuilder.Entity<TransparencyPortalGeneral>(x => x.ToDatabaseTable(databaseType, "TransparencyPortalGenerals", "Portal"));
            modelBuilder.Entity<TransparencyPortalRegulation>(x => x.ToDatabaseTable(databaseType, "TransparencyPortalRegulations", "Portal"));
            modelBuilder.Entity<TransparencyPublicInformation>(x => x.ToDatabaseTable(databaseType, "TransparencyPublicInformation", "Portal"));
            modelBuilder.Entity<TransparencyPublicInformationFile>(x => x.ToDatabaseTable(databaseType, "TransparencyPublicInformationFiles", "Portal"));
            modelBuilder.Entity<TransparencyResearchProject>(x =>
            {
                x.Property(r => r.Year).HasDefaultValue(DateTime.UtcNow.Year);
                x.ToDatabaseTable(databaseType, "TransparencyResearchProjects", "Portal");
            });
            modelBuilder.Entity<TransparencyResearchProjectFile>(x => x.ToDatabaseTable(databaseType, "TransparencyResearchProjectFiles", "Portal"));
            modelBuilder.Entity<TransparencyCompetitionFile>(x => x.ToDatabaseTable(databaseType, "TransparencyCompetitionFiles", "Portal"));
            modelBuilder.Entity<TransparencyConciliationAct>(x => x.ToDatabaseTable(databaseType, "TransparencyConciliationActs", "Portal"));
            modelBuilder.Entity<TransparencySelectionCommittee>(x => x.ToDatabaseTable(databaseType, "TransparencySelectionCommittees", "Portal"));
            modelBuilder.Entity<TransparencyServiceOrder>(x => x.ToDatabaseTable(databaseType, "TransparencyServiceOrders", "Portal"));
            modelBuilder.Entity<TransparencyAppliedPenalty>(x => x.ToDatabaseTable(databaseType, "TransparencyAppliedPenalties", "Portal"));
            modelBuilder.Entity<TransparencyEmployee>(x => x.ToDatabaseTable(databaseType, "TransparencyEmployees", "Portal"));
            modelBuilder.Entity<TransparencyAdvertising>(x => x.ToDatabaseTable(databaseType, "TransparencyAdvertisings", "Portal"));
            modelBuilder.Entity<TransparencyVisitsRecord>(x => x.ToDatabaseTable(databaseType, "TransparencyVisitsRecords", "Portal"));
            modelBuilder.Entity<TransparencyTelephone>(x => x.ToDatabaseTable(databaseType, "TransparencyTelephones", "Portal"));
            modelBuilder.Entity<TransparencyVehicle>(x => x.ToDatabaseTable(databaseType, "TransparencyVehicles", "Portal"));
            modelBuilder.Entity<TransparencyTravelPassage>(x => x.ToDatabaseTable(databaseType, "TransparencyTravelPassages", "Portal"));
            modelBuilder.Entity<TransparencySubMenu>(x => x.ToDatabaseTable(databaseType, "TransparencySubMenus", "Portal"));
            modelBuilder.Entity<TransparencySubMenuFile>(x => x.ToDatabaseTable(databaseType, "TransparencySubMenuFiles", "Portal"));
            modelBuilder.Entity<Directive>(x => x.ToDatabaseTable(databaseType, "Directives", "Portal"));
            modelBuilder.Entity<TransparencyPortalGeneralFile>(x => x.ToDatabaseTable(databaseType, "TransparencyPortalGeneralFiles", "Portal"));
            modelBuilder.Entity<TransparencyScholarship>(x => x.ToDatabaseTable(databaseType, "TransparencyScholarships", "Portal"));
            modelBuilder.Entity<TransparencyManagementDocument>(x => x.ToDatabaseTable(databaseType, "TransparencyManagementDocuments", "Portal"));
            modelBuilder.Entity<TransparencyPortalInterestLink>(x =>
            {
                x.Property(r => r.Type).HasDefaultValue(ConstantHelpers.TRANSPARENCY_PORTAL_INTEREST_LINK.TYPE.FOOTER);
                x.ToDatabaseTable(databaseType, "TransparencyPortalInterestLinks", "Portal");
            });

            #endregion

            #region PREUNIVERSITARY

            modelBuilder.Entity<PreuniversitaryAssistance>(x => x.ToDatabaseTable(databaseType, "PreuniversitaryAssistances", "Preuniversitary"));
            modelBuilder.Entity<PreuniversitaryAssistanceStudent>(x => x.ToDatabaseTable(databaseType, "PreuniversitaryAssistanceStudents", "Preuniversitary"));
            modelBuilder.Entity<PreuniversitaryCourse>(x => x.ToDatabaseTable(databaseType, "PreuniversitaryCourses", "Preuniversitary"));
            modelBuilder.Entity<PreuniversitaryGroup>(x => x.ToDatabaseTable(databaseType, "PreuniversitaryGroups", "Preuniversitary"));
            modelBuilder.Entity<PreuniversitaryPostulant>(x => x.ToDatabaseTable(databaseType, "PreuniversitaryPostulants", "Preuniversitary"));
            modelBuilder.Entity<PreuniversitarySchedule>(x => x.ToDatabaseTable(databaseType, "PreuniversitarySchedules", "Preuniversitary"));
            modelBuilder.Entity<PreuniversitaryTemary>(x => x.ToDatabaseTable(databaseType, "PreuniversitaryTemaries", "Preuniversitary"));
            modelBuilder.Entity<PreuniversitaryTerm>(x => x.ToDatabaseTable(databaseType, "PreuniversitaryTerms", "Preuniversitary"));
            modelBuilder.Entity<PreuniversitaryUserGroup>(x => x.ToDatabaseTable(databaseType, "PreuniversitaryUserGroups", "Preuniversitary"));
            modelBuilder.Entity<PreuniversitaryChannel>(x => x.ToDatabaseTable(databaseType, "PreuniversitaryChannels", "Preuniversitary"));

            modelBuilder.Entity<PreuniversitaryExam>(x => x.ToDatabaseTable(databaseType, "PreuniversitaryExams", "Preuniversitary"));
            modelBuilder.Entity<PreuniversitaryExamClassroom>(x => x.ToDatabaseTable(databaseType, "PreuniversitaryExamClassrooms", "Preuniversitary"));
            modelBuilder.Entity<PreuniversitaryExamTeacher>(x => x.ToDatabaseTable(databaseType, "PreuniversitaryExamTeachers", "Preuniversitary"));
            modelBuilder.Entity<PreuniversitaryExamClassroomPostulant>(x => x.ToDatabaseTable(databaseType, "PreuniversitaryExamClassroomPostulants", "Preuniversitary"));

            #endregion

            #region RESERVATION

            modelBuilder.Entity<ENTITIES.Models.Reservations.Environment>(x => x.ToDatabaseTable(databaseType, "Environments", "Reservation"));
            modelBuilder.Entity<EnvironmentReservation>(x => x.ToDatabaseTable(databaseType, "EnvironmentReservations", "Reservation"));
            modelBuilder.Entity<EnvironmentSchedule>(x => x.ToDatabaseTable(databaseType, "EnvironmentSchedules", "Reservation"));

            #endregion

            #region RESOLUTIVE ACTS

            modelBuilder.Entity<Document>(x =>
            {
                x.Property(r => r.Type).HasDefaultValue(ConstantHelpers.RESOLUTIVE_ACT.DOCUMENT.TYPE.ACT);
                x.Property(r => r.Status).HasDefaultValue(ConstantHelpers.RESOLUTIVE_ACT.DOCUMENT.STATUS.GENERATED);
                x.ToDatabaseTable(databaseType, "Documents", "ResolutiveAct");
            });
            modelBuilder.Entity<DocumentFile>(x => x.ToDatabaseTable(databaseType, "DocumentFiles", "ResolutiveAct"));
            modelBuilder.Entity<Sorter>(x => x.ToDatabaseTable(databaseType, "Sorters", "ResolutiveAct"));
            modelBuilder.Entity<ResolutionCategory>(x => x.ToDatabaseTable(databaseType, "ResolutionCategories", "ResolutiveAct"));

            #endregion

            #region SCALE

            modelBuilder.Entity<AcademicDepartment>(x => x.ToDatabaseTable(databaseType, "AcademicDepartments", "Scale"));
            modelBuilder.Entity<PrivateManagementPensionFund>(x => x.ToDatabaseTable(databaseType, "PrivateManagementPensionFunds", "Scale"));
            modelBuilder.Entity<Institution>(x => x.ToDatabaseTable(databaseType, "Institutions", "Scale"));
            modelBuilder.Entity<WorkerBankAccountInformation>(x => x.ToDatabaseTable(databaseType, "WorkerBankAccountInformations", "Scale"));
            modelBuilder.Entity<WorkerLaborTermInformation>(x => x.ToDatabaseTable(databaseType, "WorkerLaborTermInformations", "Scale"));
            modelBuilder.Entity<ScaleExtraBenefitField>(x => x.ToDatabaseTable(databaseType, "ScaleExtraBenefitFields", "Scale"));
            modelBuilder.Entity<BenefitType>(x => x.ToDatabaseTable(databaseType, "BenefitTypes", "Scale"));
            modelBuilder.Entity<ScaleExtraContractField>(x => x.ToDatabaseTable(databaseType, "ScaleExtraContractFields", "Scale"));
            modelBuilder.Entity<ScaleExtraDemeritField>(x => x.ToDatabaseTable(databaseType, "ScaleExtraDemeritFields", "Scale"));
            modelBuilder.Entity<ScaleExtraDisplacementField>(x => x.ToDatabaseTable(databaseType, "ScaleExtraDisplacementFields", "Scale"));
            modelBuilder.Entity<ScaleExtraInstitutionExperienceField>(x => x.ToDatabaseTable(databaseType, "ScaleExtraInstitutionExperienceFields", "Scale"));
            modelBuilder.Entity<ScaleExtraInvestigationField>(x => x.ToDatabaseTable(databaseType, "ScaleExtraInvestigationFields", "Scale"));
            modelBuilder.Entity<InvestigationParticipationType>(x => x.ToDatabaseTable(databaseType, "InvestigationParticipationTypes", "Scale"));
            modelBuilder.Entity<ScaleExtraMeritField>(x => x.ToDatabaseTable(databaseType, "ScaleExtraMeritFields", "Scale"));
            modelBuilder.Entity<ScaleExtraPerformanceEvaluationField>(x => x.ToDatabaseTable(databaseType, "ScaleExtraPerformanceEvaluationFields", "Scale"));
            modelBuilder.Entity<ScaleLicenseAuthorization>(x => x.ToDatabaseTable(databaseType, "ScaleLicenseAuthorizations", "Scale"));
            modelBuilder.Entity<LicenseResolutionType>(x => x.ToDatabaseTable(databaseType, "LicenseResolutionTypes", "Scale"));
            modelBuilder.Entity<ScalePermitAuthorization>(x => x.ToDatabaseTable(databaseType, "ScalePermitAuthorizations", "Scale"));
            modelBuilder.Entity<ScaleResolution>(x => x.ToDatabaseTable(databaseType, "ScaleResolutions", "Scale"));
            modelBuilder.Entity<ScaleResolutionType>(x => x.ToDatabaseTable(databaseType, "ScaleResolutionTypes", "Scale"));
            modelBuilder.Entity<ScaleSection>(x => x.ToDatabaseTable(databaseType, "ScaleSections", "Scale"));
            modelBuilder.Entity<ScaleSectionAnnex>(x => x.ToDatabaseTable(databaseType, "ScaleSectionAnnexes", "Scale"));
            modelBuilder.Entity<ScaleSectionResolutionType>(x => x.ToDatabaseTable(databaseType, "ScaleSectionResolutionTypes", "Scale"));
            modelBuilder.Entity<ScaleVacationAuthorization>(x => x.ToDatabaseTable(databaseType, "ScaleVacationAuthorizations", "Scale"));
            modelBuilder.Entity<WorkerBachelorDegree>(x => x.ToDatabaseTable(databaseType, "WorkerBachelorDegrees", "Scale"));
            modelBuilder.Entity<WorkerCapPosition>(x => x.ToDatabaseTable(databaseType, "WorkerCapPositions", "Scale"));
            modelBuilder.Entity<WorkerDiplomate>(x => x.ToDatabaseTable(databaseType, "WorkerDiplomates", "Scale"));
            modelBuilder.Entity<WorkerDoctoralDegree>(x => x.ToDatabaseTable(databaseType, "WorkerDoctoralDegrees", "Scale"));
            modelBuilder.Entity<WorkerLaborCategory>(x => x.ToDatabaseTable(databaseType, "WorkerLaborCategories", "Scale"));
            modelBuilder.Entity<WorkerLaborCondition>(x => x.ToDatabaseTable(databaseType, "WorkerLaborConditions", "Scale"));
            modelBuilder.Entity<WorkerLaborInformation>(x => x.ToDatabaseTable(databaseType, "WorkerLaborInformation", "Scale"));
            modelBuilder.Entity<WorkerLaborRegime>(x => x.ToDatabaseTable(databaseType, "WorkerLaborRegimes", "Scale"));
            modelBuilder.Entity<WorkerManagementPosition>(x => x.ToDatabaseTable(databaseType, "WorkerManagementPositions", "Scale"));
            modelBuilder.Entity<WorkerMasterDegree>(x => x.ToDatabaseTable(databaseType, "WorkerMasterDegrees", "Scale"));
            modelBuilder.Entity<WorkerOtherStudy>(x => x.ToDatabaseTable(databaseType, "WorkerOtherStudies", "Scale"));
            modelBuilder.Entity<WorkerPersonalDocument>(x => x.ToDatabaseTable(databaseType, "WorkerPersonalDocuments", "Scale"));
            modelBuilder.Entity<WorkerPositionClassification>(x => x.ToDatabaseTable(databaseType, "WorkerPositionClassifications", "Scale"));
            modelBuilder.Entity<WorkerProfessionalSchool>(x => x.ToDatabaseTable(databaseType, "WorkerProfessionalSchools", "Scale"));
            modelBuilder.Entity<WorkerProfessionalTitle>(x => x.ToDatabaseTable(databaseType, "WorkerProfessionalTitles", "Scale"));
            modelBuilder.Entity<WorkerSecondSpecialty>(x => x.ToDatabaseTable(databaseType, "WorkerSecondSpecialties", "Scale"));
            modelBuilder.Entity<WorkerSchoolDegree>(x => x.ToDatabaseTable(databaseType, "WorkerSchoolDegrees", "Scale"));
            modelBuilder.Entity<WorkerTechnicalStudy>(x => x.ToDatabaseTable(databaseType, "WorkerTechnicalStudies", "Scale"));
            modelBuilder.Entity<WorkCertificateRecord>(x => x.ToDatabaseTable(databaseType, "WorkCertificateRecords", "Scale"));
            //modelBuilder.Entity<ScaleReportRecord>(x => x.ToDatabaseTable(databaseType, "ScaleReportRecords", "Scale"));
            //modelBuilder.Entity<ScaleReportSectionRecord>(x => x.ToDatabaseTable(databaseType, "ScaleReportSectionRecords", "Scale"));
            modelBuilder.Entity<WorkerDina>(x => x.ToDatabaseTable(databaseType, "WorkerDinas", "Scale"));
            modelBuilder.Entity<WorkerDinaSupportExperience>(x => x.ToDatabaseTable(databaseType, "WorkerDinaSupportExperiences", "Scale"));
            modelBuilder.Entity<WorkerRetirementSystemHistory>(x => x.ToDatabaseTable(databaseType, "WorkerRetirementSystemHistories", "Scale"));
            modelBuilder.Entity<WorkerTraining>(x => x.ToDatabaseTable(databaseType, "WorkerTrainings", "Scale"));
            modelBuilder.Entity<ScaleReportProfile>(x => x.ToDatabaseTable(databaseType, "ScaleReportProfiles", "Scale"));
            modelBuilder.Entity<ScaleReportProfileDetail>(x => x.ToDatabaseTable(databaseType, "ScaleReportProfileDetails", "Scale"));
            modelBuilder.Entity<ScaleReportHistory>(x => x.ToDatabaseTable(databaseType, "ScaleReportHistories", "Scale"));
            modelBuilder.Entity<WorkerFamilyInformation>(x => x.ToDatabaseTable(databaseType, "WorkerFamilyInformations", "Scale"));

            new AcademicDepartmentMap(modelBuilder.Entity<AcademicDepartment>());
            new ScaleExtraBenefitFieldMap(modelBuilder.Entity<ScaleExtraBenefitField>());
            new ScaleExtraContractFieldMap(modelBuilder.Entity<ScaleExtraContractField>());
            new ScaleExtraDemeritFieldMap(modelBuilder.Entity<ScaleExtraDemeritField>());
            new ScaleExtraDisplacementFieldMap(modelBuilder.Entity<ScaleExtraDisplacementField>());
            new ScaleExtraInstitutionExperienceFieldMap(modelBuilder.Entity<ScaleExtraInstitutionExperienceField>());
            new ScaleExtraInvestigationFieldMap(modelBuilder.Entity<ScaleExtraInvestigationField>());
            new ScaleExtraMeritFieldMap(modelBuilder.Entity<ScaleExtraMeritField>());
            new ScaleExtraPerformanceEvaluationFieldMap(modelBuilder.Entity<ScaleExtraPerformanceEvaluationField>());
            new ScaleLicenseAuthorizationMap(modelBuilder.Entity<ScaleLicenseAuthorization>());
            new ScalePermitAuthorizationMap(modelBuilder.Entity<ScalePermitAuthorization>());
            new ScaleResolutionMap(modelBuilder.Entity<ScaleResolution>());
            new ScaleResolutionTypeMap(modelBuilder.Entity<ScaleResolutionType>());
            new ScaleSectionAnnexMap(modelBuilder.Entity<ScaleSectionAnnex>());
            new ScaleSectionMap(modelBuilder.Entity<ScaleSection>());
            //new ScaleSectionResolutionTypeMap(modelBuilder.Entity<ScaleSectionResolutionType>());
            new ScaleVacationAuthorizationMap(modelBuilder.Entity<ScaleVacationAuthorization>());

            #endregion

            #region TEACHER HIRING

            modelBuilder.Entity<ENTITIES.Models.TeacherHiring.Convocation>(x => x.ToDatabaseTable(databaseType, "Convocation", "TeacherHiring"));
            modelBuilder.Entity<ConvocationAcademicDeparment>(x => x.ToDatabaseTable(databaseType, "ConvocationAcademicDeparments", "TeacherHiring"));
            modelBuilder.Entity<ConvocationComitee>(x =>
            {
                x.HasKey(t => new { t.ConvocationId, t.UserId });
                x.ToDatabaseTable(databaseType, "ConvocationComitees", "TeacherHiring");
            });
            modelBuilder.Entity<ConvocationCalendar>(x => x.ToDatabaseTable(databaseType, "ConvocationCalendar", "TeacherHiring"));
            modelBuilder.Entity<ConvocationSection>(x => x.ToDatabaseTable(databaseType, "ConvocationSections", "TeacherHiring"));
            modelBuilder.Entity<ENTITIES.Models.TeacherHiring.ConvocationQuestion>(x => x.ToDatabaseTable(databaseType, "ConvocationQuestions", "TeacherHiring"));
            modelBuilder.Entity<ENTITIES.Models.TeacherHiring.ConvocationAnswer>(x => x.ToDatabaseTable(databaseType, "ConvocationAnswers", "TeacherHiring"));
            modelBuilder.Entity<ENTITIES.Models.TeacherHiring.ConvocationAnswerByUser>(x =>
            {
                x.ToDatabaseTable(databaseType, "ConvocationAnswerByUser", "TeacherHiring");
            });
            modelBuilder.Entity<ApplicantTeacher>(x => x.ToDatabaseTable(databaseType, "ApplicantTeachers", "TeacherHiring"));

            #endregion

            #region TEACHING MANAGEMENT

            modelBuilder.Entity<AcademicSecretary>(x => x.ToDatabaseTable(databaseType, "AcademicSecretaries", "TeachingManagement"));
            modelBuilder.Entity<Activity>(x => x.ToDatabaseTable(databaseType, "Activities", "TeachingManagement"));
            modelBuilder.Entity<CourseComponent>(x => x.ToDatabaseTable(databaseType, "CourseComponents", "TeachingManagement"));
            modelBuilder.Entity<ScoreInputSchedule>(x => x.ToDatabaseTable(databaseType, "ScoreInputSchedules", "TeachingManagement"));
            modelBuilder.Entity<ScoreInputScheduleDetail>(x => x.ToDatabaseTable(databaseType, "ScoreInputScheduleDetails", "TeachingManagement"));
            modelBuilder.Entity<DigitalDocument>(x => x.ToDatabaseTable(databaseType, "DigitalDocuments", "TeachingManagement"));
            modelBuilder.Entity<DigitalResource>(x => x.ToDatabaseTable(databaseType, "DigitalResources", "TeachingManagement"));
            modelBuilder.Entity<DigitalResourceCareer>(x =>
            {
                x.HasKey(t => new { t.DigitalResourceId, t.CareerId });
                x.ToDatabaseTable(databaseType, "DigitalResourceCareers", "TeachingManagement");
            });

            modelBuilder.Entity<NonActivity>(x => x.ToDatabaseTable(databaseType, "NonActivities", "TeachingManagement"));
            modelBuilder.Entity<SupportOffice>(x => x.ToDatabaseTable(databaseType, "SupportOffice", "Tutoring"));
            modelBuilder.Entity<SupportOfficeUser>(x => x.ToDatabaseTable(databaseType, "SupportOfficeUser", "Tutoring"));
            modelBuilder.Entity<SyllabusRequest>(x =>
            {
                x.Property(r => r.Type).HasDefaultValue(ConstantHelpers.SYLLABUS_REQUEST.TYPE.MIXED);
                x.ToDatabaseTable(databaseType, "SyllabusRequests", "TeachingManagement");
            });
            modelBuilder.Entity<SyllabusTeacher>(x =>
            {
                x.Property(r => r.Status).HasDefaultValue(ConstantHelpers.SYLLABUS_TEACHER.STATUS.IN_PROCESS);
                x.ToDatabaseTable(databaseType, "SyllabusTeachers", "TeachingManagement");
            });
            modelBuilder.Entity<TeacherAssistance>(x => x.ToDatabaseTable(databaseType, "TeacherAssistance", "TeachingManagement"));
            modelBuilder.Entity<TeacherSurvey>(x => x.ToDatabaseTable(databaseType, "TeacherSurveys", "TeachingManagement"));
            modelBuilder.Entity<TeacherDedication>(x => x.ToDatabaseTable(databaseType, "TeacherDedication", "TeachingManagement"));
            modelBuilder.Entity<TeacherExperience>(x => x.ToDatabaseTable(databaseType, "TeacherExperiences", "TeachingManagement"));
            modelBuilder.Entity<TeacherInformation>(x => x.ToDatabaseTable(databaseType, "TeacherInformations", "TeachingManagement"));
            modelBuilder.Entity<TeacherNonActivityHistorial>(x => x.ToDatabaseTable(databaseType, "TeacherNonActivityHistorials", "TeachingManagement"));
            modelBuilder.Entity<TeacherSchedule>(x => x.ToDatabaseTable(databaseType, "TeacherSchedules", "TeachingManagement"));
            modelBuilder.Entity<TeachingLoadType>(x =>
            {
                x.Property(r => r.Enabled).HasDefaultValue(true);
                x.ToDatabaseTable(databaseType, "TeachingLoadTypes", "TeachingManagement");
            });
            modelBuilder.Entity<TeachingLoadSubType>(x => x.ToDatabaseTable(databaseType, "TeachingLoadSubTypes", "TeachingManagement"));
            modelBuilder.Entity<NonTeachingLoad>(x => x.ToDatabaseTable(databaseType, "NonTeachingLoads", "TeachingManagement"));
            modelBuilder.Entity<TeacherTermInform>(x => x.ToDatabaseTable(databaseType, "TeacherTermInforms", "TeachingManagement"));
            modelBuilder.Entity<TermInform>(x => x.ToDatabaseTable(databaseType, "TermInforms", "TeachingManagement"));
            modelBuilder.Entity<UnitActivity>(x => x.ToDatabaseTable(databaseType, "UnitActivities", "TeachingManagement"));
            modelBuilder.Entity<UnitResource>(x => x.ToDatabaseTable(databaseType, "UnitResources", "TeachingManagement"));
            modelBuilder.Entity<NonTeachingLoadSchedule>(x => x.ToDatabaseTable(databaseType, "NonTeachingLoadSchedules", "TeachingManagement"));

            modelBuilder.Entity<NonTeachingLoadActivity>(x => x.ToDatabaseTable(databaseType, "NonTeachingLoadActivities", "TeachingManagement"));
            modelBuilder.Entity<NonTeachingLoadDeliverable>(x =>
            {
                x.Property(r => r.Status).HasDefaultValue(ConstantHelpers.NON_TEACHING_LOAD_DELIVERABLE.STATUS.PENDING);
                x.ToDatabaseTable(databaseType, "NonTeachingLoadDeliverables", "TeachingManagement");
            });

            modelBuilder.Entity<TeacherAcademicCharge>(x => x.ToDatabaseTable(databaseType, "TeacherAcademicCharges", "TeachingManagement"));
            modelBuilder.Entity<PerformanceEvaluation>(x =>
            {
                x.Property(r => r.Target).HasDefaultValue(ConstantHelpers.PERFORMANCE_EVALUATION.TARGET.ALL);
                x.ToDatabaseTable(databaseType, "PerformanceEvaluations", "TeachingManagement");
            });
            modelBuilder.Entity<PerformanceEvaluationRatingScale>(x => x.ToDatabaseTable(databaseType, "PerformanceEvaluationRatingScales", "TeachingManagement"));
            modelBuilder.Entity<PerformanceEvaluationUser>(x => x.ToDatabaseTable(databaseType, "PerformanceEvaluationUsers", "TeachingManagement"));
            modelBuilder.Entity<PerformanceEvaluationRubric>(x => x.ToDatabaseTable(databaseType, "PerformanceEvaluationRubrics", "TeachingManagement"));
            modelBuilder.Entity<PerformanceEvaluationQuestion>(x => x.ToDatabaseTable(databaseType, "PerformanceEvaluationQuestions", "TeachingManagement"));
            modelBuilder.Entity<PerformanceEvaluationResponse>(x => x.ToDatabaseTable(databaseType, "PerformanceEvaluationResponses", "TeachingManagement"));
            modelBuilder.Entity<PerformanceEvaluationTemplate>(x => x.ToDatabaseTable(databaseType, "PerformanceEvaluationTemplates", "TeachingManagement"));
            modelBuilder.Entity<RelatedPerformanceEvaluationTemplate>(x =>
            {
                x.HasKey(t => new { t.PerformanceEvaluationTemplateId, t.PerformanceEvaluationId });
                x.HasOne(y => y.PerformanceEvaluation)
                .WithMany(y => y.RelatedPerformanceEvaluationTemplates)
                .HasForeignKey(y => y.PerformanceEvaluationId)
                .HasConstraintName("FK_PerformanceEvaluation");
                x.HasOne(y => y.PerformanceEvaluationTemplate)
                .WithMany(y => y.RelatedPerformanceEvaluationTemplates)
                .HasForeignKey(y => y.PerformanceEvaluationTemplateId)
                .HasConstraintName("FK_PerformanceEvaluationTemplate");
                x.ToDatabaseTable(databaseType, "RelatedPerformanceEvaluationTemplates", "TeachingManagement");
            });
            modelBuilder.Entity<PerformanceEvaluationCriterion>(x => x.ToDatabaseTable(databaseType, "PerformanceEvaluationCriterions", "TeachingManagement"));
            modelBuilder.Entity<TeacherPortfolio>(x => x.ToDatabaseTable(databaseType, "TeacherPortfolios", "TeachingManagement"));

            modelBuilder.Entity<ExtraTeachingLoad>(x =>
            {
                x.HasKey(t => new { t.TeacherId, t.TermId });
                x.ToDatabaseTable(databaseType, "ExtraTeachingLoads", "TeachingManagement");
            });
            #endregion

            #region TEACHING RESEARCH

            modelBuilder.Entity<ENTITIES.Models.TeachingResearch.Convocation>(x => x.ToDatabaseTable(databaseType, "Convocations", "TeachingResearch"));
            modelBuilder.Entity<ENTITIES.Models.TeachingResearch.ConvocationAnswer>(x => x.ToDatabaseTable(databaseType, "ConvocationAnswers", "TeachingResearch"));
            modelBuilder.Entity<ENTITIES.Models.TeachingResearch.ConvocationAnswerByUser>(x =>
            {
                x.ToDatabaseTable(databaseType, "ConvocationAnswerByUsers", "TeachingResearch");

                if (ConstantHelpers.GENERAL.DATABASES.DATABASE == ConstantHelpers.DATABASES.PSQL)
                {
                    x.HasOne(y => y.ConvocationPostulant)
                    .WithMany(y => y.ConvocationAnswerByUsers)
                    .HasForeignKey(y => y.ConvocationPostulantId)
                    .HasConstraintName("FK_ConvocationPostulant");
                    x.HasIndex(y => y.ConvocationPostulantId).HasDatabaseName("IX_ConvocationPostulant");

                    x.HasOne(y => y.ConvocationAnswer)
                    .WithMany(y => y.ConvocationAnswerByUsers)
                    .HasForeignKey(y => y.ConvocationAnswerId)
                    .HasConstraintName("FK_ConvocationAnswer");
                    x.HasIndex(y => y.ConvocationAnswerId).HasDatabaseName("IX_ConvocationAnswer");
                }
            });
            modelBuilder.Entity<ENTITIES.Models.TeachingResearch.ConvocationFile>(x => x.ToDatabaseTable(databaseType, "ConvocationFiles", "TeachingResearch"));
            modelBuilder.Entity<ENTITIES.Models.TeachingResearch.ConvocationSupervisor>(x =>
            {
                x.HasKey(t => new { t.UserId, t.ConvocationId });
                x.ToDatabaseTable(databaseType, "ConvocationSupervisors", "TeachingResearch");
            });
            modelBuilder.Entity<ENTITIES.Models.TeachingResearch.ConvocationPostulant>(x =>
            {
                x.Property(r => r.Status).HasDefaultValue(ConstantHelpers.TEACHING_RESEARCH.POSTULANT_STATUS.PENDING);
                x.ToDatabaseTable(databaseType, "ConvocationPostulant", "TeachingResearch");
            });

            #endregion

            #region TUTORING

            modelBuilder.Entity<Tutor>(x => x.ToDatabaseTable(databaseType, "Tutors", "Tutoring"));
            modelBuilder.Entity<TutoringAnnouncement>(x => x.ToDatabaseTable(databaseType, "TutoringAnnouncements", "Tutoring"));
            modelBuilder.Entity<TutoringAnnouncementCareer>(x =>
            {
                x.HasKey(t => new { t.CareerId, t.TutoringAnnouncementId });
                x.HasOne(t => t.Career)
                    .WithMany(t => t.TutoringAnnouncementCareers)
                    .HasForeignKey(t => t.CareerId)
                    .IsRequired();
                x.HasOne(t => t.TutoringAnnouncement)
                    .WithMany(t => t.TutoringAnnouncementCareers)
                    .HasForeignKey(t => t.TutoringAnnouncementId)
                    .IsRequired();
                x.ToDatabaseTable(databaseType, "TutoringAnnouncementCareers", "Tutoring");
            });

            modelBuilder.Entity<TutoringAnnouncementRole>(x =>
            {
                x.HasKey(t => new { t.RoleId, t.TutoringAnnouncementId });
                x.HasOne(t => t.Role)
                    .WithMany(t => t.TutoringAnnouncementRoles)
                    .HasForeignKey(t => t.RoleId)
                    .IsRequired();
                x.HasOne(t => t.TutoringAnnouncement)
                    .WithMany(t => t.TutoringAnnouncementRoles)
                    .HasForeignKey(t => t.TutoringAnnouncementId)
                    .IsRequired();
                x.ToDatabaseTable(databaseType, "TutoringAnnouncementRoles", "Tutoring");
            });

            modelBuilder.Entity<TutoringCoordinator>(x => x.ToDatabaseTable(databaseType, "TutoringCoordinators", "Tutoring"));


            modelBuilder.Entity<TutoringStudent>(x =>
            {
                x.HasKey(t => new { t.StudentId, t.TermId });
                x.ToDatabaseTable(databaseType, "TutoringStudents", "Tutoring");
            });

            modelBuilder.Entity<TutoringProblem>(x => x.ToDatabaseTable(databaseType, "TutoringProblems", "Tutoring"));
            modelBuilder.Entity<TutoringProblemFile>(x => x.ToDatabaseTable(databaseType, "TutoringProblemFiles", "Tutoring"));
            modelBuilder.Entity<TutoringSession>(x => x.ToDatabaseTable(databaseType, "TutoringSessions", "Tutoring"));
            modelBuilder.Entity<TutoringSessionProblem>(x => x.ToDatabaseTable(databaseType, "TutoringSessionProblems", "Tutoring"));
            modelBuilder.Entity<TutorWorkingPlan>(x => x.ToDatabaseTable(databaseType, "TutorWorkingPlans", "Tutoring"));
            modelBuilder.Entity<TutoringPlan>(x => x.ToDatabaseTable(databaseType, "TutoringPlans", "Tutoring"));
            modelBuilder.Entity<TutoringPlanHistory>(x => x.ToDatabaseTable(databaseType, "TutoringPlanHistories", "Tutoring"));
            modelBuilder.Entity<TutoringAttendance>(x => x.ToDatabaseTable(databaseType, "TutoringAttendances", "Tutoring"));
            modelBuilder.Entity<TutoringAttendanceProblem>(x => x.ToDatabaseTable(databaseType, "TutoringAttendanceProblems", "Tutoring"));
            modelBuilder.Entity<TutoringMessage>(x => x.ToDatabaseTable(databaseType, "TutoringMessages", "Tutoring"));
            modelBuilder.Entity<TutoringSessionStudent>(x => x.ToDatabaseTable(databaseType, "TutoringSessionStudents", "Tutoring"));

            //modelBuilder.Entity<TutoringSessionStudent>(x =>
            //{
            //    x.HasKey(t => new { t.TutoringSessionId, t.TutoringStudentId });
            //    x.HasOne(t => t.TutoringSession)
            //        .WithMany(t => t.TutoringSessionStudents)
            //        .HasForeignKey(t => t.TutoringSessionId)
            //        .IsRequired();
            //    x.HasOne(t => t.TutoringStudent)
            //        .WithMany(t => t.TutoringSessionStudents)
            //        .HasForeignKey(t => t.TutoringStudentId)
            //        .IsRequired();
            //    x.ToDatabaseTable(databaseType, "TutoringSessionStudents", "Tutoring");
            //});

            modelBuilder.Entity<TutorTutoringStudent>(x =>
            {
                x.HasKey(t => new { t.TutorId, t.TutoringStudentStudentId, t.TutoringStudentTermId });
                x.ToDatabaseTable(databaseType, "TutorTutoringStudents", "Tutoring");
            });

            modelBuilder.Entity<HistoryReferredTutoringStudent>(x => x.ToDatabaseTable(databaseType, "HistoryReferredTutoringStudents", "Tutoring"));


            #endregion

            #region VIRTUAL DIRECTORY

            modelBuilder.Entity<DirectoryDependency>(x => x.ToDatabaseTable(databaseType, "DirectoryDependencies", "VirtualDirectory"));
            modelBuilder.Entity<InstitutionalInformation>(x => x.ToDatabaseTable(databaseType, "InstitutionalInformations", "VirtualDirectory"));

            #endregion

            #region VISIT MANAGEMENT

            modelBuilder.Entity<Visit>(x => x.ToDatabaseTable(databaseType, "Visits", "VisitManagement"));
            modelBuilder.Entity<VisitorInformation>(x => x.ToDatabaseTable(databaseType, "VisitorInformations", "VisitManagement"));

            #endregion

            #region SISCO
            modelBuilder.Entity<Norm>(x => x.ToDatabaseTable(databaseType, "Norms", "Sisco"));
            modelBuilder.Entity<SectionSisco>(x => x.ToDatabaseTable(databaseType, "Sections", "Sisco"));
            modelBuilder.Entity<ENTITIES.Models.Sisco.CourseConference>(x => x.ToDatabaseTable(databaseType, "CoursesConferences", "Sisco"));
            modelBuilder.Entity<Contact>(x => x.ToDatabaseTable(databaseType, "Contacts", "Sisco"));
            modelBuilder.Entity<Link>(x => x.ToDatabaseTable(databaseType, "Links", "Sisco"));
            modelBuilder.Entity<Achievement>(x => x.ToDatabaseTable(databaseType, "Achievements", "Sisco"));
            modelBuilder.Entity<Banner>(x => x.ToDatabaseTable(databaseType, "Banners", "Sisco"));
            modelBuilder.Entity<Novelty>(x => x.ToDatabaseTable(databaseType, "Novelties", "Sisco"));
            modelBuilder.Entity<MissionVision>(x => x.ToDatabaseTable(databaseType, "MissionVission", "Sisco"));
            modelBuilder.Entity<Shortcut>(x => x.ToDatabaseTable(databaseType, "Shortcuts", "Sisco"));
            modelBuilder.Entity<SubShortcut>(x => x.ToDatabaseTable(databaseType, "SubShortcuts", "Sisco"));
            #endregion

            #region LIBRARY      

            modelBuilder.Entity<QuibukAuthorityType>(x => x.ToDatabaseTable(databaseType, "QuibukAuthorityTypes", "Library"));
            modelBuilder.Entity<QuibukAuthoritie>(x => x.ToDatabaseTable(databaseType, "QuibukAuthorities", "Library"));
            modelBuilder.Entity<QuibukAuthorityField>(x => x.ToDatabaseTable(databaseType, "QuibukAuthorityFields", "Library"));

            #endregion

            #region INTERNATIONAL COOPERATION MANAGEMENT

            modelBuilder.Entity<ExchangeAgreement>(x => x.ToDatabaseTable(databaseType, "ExchangeAgreements", "InternationalCooperation"));

            #endregion

            #region INSTITUTIONAL WELFARE            
            modelBuilder.Entity<InstitutionalWelfareAnswer>(x => x.ToDatabaseTable(databaseType, "InstitutionalWelfareAnswers", "InstitutionalWelfare"));
            modelBuilder.Entity<InstitutionalWelfareQuestion>(x => x.ToDatabaseTable(databaseType, "InstitutionalWelfareQuestions", "InstitutionalWelfare"));
            modelBuilder.Entity<InstitutionalWelfareRecord>(x => x.ToDatabaseTable(databaseType, "InstitutionalWelfareRecords", "InstitutionalWelfare"));
            modelBuilder.Entity<InstitutionalWelfareSection>(x => x.ToDatabaseTable(databaseType, "InstitutionalWelfareSections", "InstitutionalWelfare"));
            modelBuilder.Entity<InstitutionalWelfareAnswerByStudent>(x =>
            {
                x.ToDatabaseTable(databaseType, "InstitutionalWelfareAnswerByStudents", "InstitutionalWelfare");

                if (ConstantHelpers.GENERAL.DATABASES.DATABASE == ConstantHelpers.DATABASES.PSQL)
                {
                    x.HasOne(y => y.InstitutionalWelfareQuestion)
                    .WithMany(y => y.InstitutionalWelfareAnswerByStudents)
                    .HasForeignKey(y => y.InstitutionalWelfareQuestionId)
                    .HasConstraintName("FK_InstitutionalWelfareQuestion");
                    x.HasIndex(y => y.InstitutionalWelfareQuestionId).HasDatabaseName("IX_InstitutionalWelfareQuestion");

                    x.HasOne(y => y.InstitutionalWelfareAnswer)
                    .WithMany(y => y.InstitutionalWelfareAnswerByStudents)
                    .HasForeignKey(y => y.InstitutionalWelfareAnswerId)
                    .HasConstraintName("FK_InstitutionalWelfareAnswer");
                    x.HasIndex(y => y.InstitutionalWelfareAnswerId).HasDatabaseName("IX_InstitutionalWelfareAnswer");
                }
            });
            modelBuilder.Entity<CategorizationLevel>(x => x.ToDatabaseTable(databaseType, "CategorizationLevels", "InstitutionalWelfare"));
            modelBuilder.Entity<InstitutionalRecordCategorizationByStudent>(x => x.ToDatabaseTable(databaseType, "InstitutionalRecordCategorizationByStudents", "InstitutionalWelfare"));
            modelBuilder.Entity<CategorizationLevelHeader>(x => x.ToDatabaseTable(databaseType, "CategorizationLevelHeaders", "InstitutionalWelfare"));
            modelBuilder.Entity<MedicalDiagnostic>(x => x.ToDatabaseTable(databaseType, "MedicalDiagnostics", "InstitutionalWelfare"));
            modelBuilder.Entity<InstitutionalWelfareProduct>(x => x.ToDatabaseTable(databaseType, "InstitutionalWelfareProducts", "InstitutionalWelfare"));
            modelBuilder.Entity<InstitutionalWelfareUserProduct>(x => x.ToDatabaseTable(databaseType, "InstitutionalWelfareUserProducts", "InstitutionalWelfare"));
            modelBuilder.Entity<MedicalRecord>(x => x.ToDatabaseTable(databaseType, "MedicalRecords", "InstitutionalWelfare"));

            modelBuilder.Entity<InstitutionalWelfareScholarshipType>(x => x.ToDatabaseTable(databaseType, "InstitutionalWelfareScholarshipTypes", "InstitutionalWelfare"));
            modelBuilder.Entity<InstitutionalWelfareScholarship>(x => x.ToDatabaseTable(databaseType, "InstitutionalWelfareScholarships", "InstitutionalWelfare"));
            modelBuilder.Entity<InstitutionalWelfareScholarshipRequirement>(x => x.ToDatabaseTable(databaseType, "InstitutionalWelfareScholarshipRequirements", "InstitutionalWelfare"));
            modelBuilder.Entity<InstitutionalWelfareScholarshipFormat>(x => x.ToDatabaseTable(databaseType, "InstitutionalWelfareScholarshipFormats", "InstitutionalWelfare"));
            modelBuilder.Entity<ScholarshipStudent>(x => x.ToDatabaseTable(databaseType, "ScholarshipStudents", "InstitutionalWelfare"));
            modelBuilder.Entity<ScholarshipStudentRequirement>(x =>
            {
                x.HasKey(t => new { t.ScholarshipStudentId, t.InstitutionalWelfareScholarshipRequirementId });
                x.ToDatabaseTable(databaseType, "ScholarshipStudentRequirements", "InstitutionalWelfare");
            });

            modelBuilder.Entity<WelfareConvocation>(x => x.ToDatabaseTable(databaseType, "WelfareConvocations", "InstitutionalWelfare"));
            modelBuilder.Entity<WelfareConvocationRequirement>(x => x.ToDatabaseTable(databaseType, "WelfareConvocationRequirements", "InstitutionalWelfare"));
            modelBuilder.Entity<WelfareConvocationFormat>(x => x.ToDatabaseTable(databaseType, "WelfareConvocationFormats", "InstitutionalWelfare"));
            modelBuilder.Entity<WelfareConvocationPostulant>(x => x.ToDatabaseTable(databaseType, "WelfareConvocationPostulants", "InstitutionalWelfare"));
            modelBuilder.Entity<WelfareConvocationPostulantFile>(x => x.ToDatabaseTable(databaseType, "WelfareConvocationPostulantFiles", "InstitutionalWelfare"));

            modelBuilder.Entity<CafobeRequest>(x => x.ToDatabaseTable(databaseType, "CafobeRequests", "InstitutionalWelfare"));
            modelBuilder.Entity<CafobeRequestDetail>(x => x.ToDatabaseTable(databaseType, "CafobeRequestDetails", "InstitutionalWelfare"));
            modelBuilder.Entity<Disability>(x => x.ToDatabaseTable(databaseType, "Disabilities", "InstitutionalWelfare"));
            modelBuilder.Entity<WelfareAlert>(x => x.ToDatabaseTable(databaseType, "WelfareAlerts", "InstitutionalWelfare"));

            #endregion

            #region PREPROFESIONAL PRACTICE

            modelBuilder.Entity<ENTITIES.Models.PreprofesionalPractice.InternshipRequest>(x => x.ToDatabaseTable(databaseType, "InternshipRequests", "PreprofesionalPractice"));
            modelBuilder.Entity<InternshipAspect>(x => x.ToDatabaseTable(databaseType, "InternshipAspects", "PreprofesionalPractice"));
            modelBuilder.Entity<InternshipDevelopment>(x =>
            {
                x.HasKey(t => new { t.InternshipAspectId, t.InternshipRequestId });
                x.ToDatabaseTable(databaseType, "InternshipDevelopments", "PreprofesionalPractice");
            });
            modelBuilder.Entity<PresentationLetter>(x => x.ToDatabaseTable(databaseType, "PresentationLetters", "PreprofesionalPractice"));
            modelBuilder.Entity<WeeklyInternshipProgress>(x => x.ToDatabaseTable(databaseType, "WeeklyInternshipProgress", "PreprofesionalPractice"));
            modelBuilder.Entity<WeeklyInternshipProgressDetail>(x => x.ToDatabaseTable(databaseType, "WeeklyInternshipProgressDetails", "PreprofesionalPractice"));
            modelBuilder.Entity<InternshipRequestFile>(x => x.ToDatabaseTable(databaseType, "InternshipRequestFiles", "PreprofesionalPractice"));

            #endregion

            #region SERVER

            modelBuilder.Entity<GeneralLink>(x => x.ToDatabaseTable(databaseType, "GeneralLinks", "Campus"));
            modelBuilder.Entity<GeneralLinkRole>(x =>
            {
                x.HasKey(t => new { t.GeneralLinkId, t.ApplicationRoleId });
                x.ToDatabaseTable(databaseType, "GeneralLinkRoles", "Campus");
            });
            #endregion
        }

        public override int SaveChanges()
        {
            return BaseSaveChanges(true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return BaseSaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await BaseSaveChangesAsync(true);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await BaseSaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public int BaseSaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaveChanges();

            int result = base.SaveChanges(acceptAllChangesOnSuccess);

            OnAfterSaveChanges();

            base.SaveChanges(acceptAllChangesOnSuccess);

            return result;
        }

        public async Task<int> BaseSaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaveChanges();

            int result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            OnAfterSaveChanges();

            await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            return result;
        }

        private bool EntityFilter(AuditEntry entity)
        {

            var table = entity.TableName;

            var filters = new List<string>() {
                "Logins",
                "Messages",
                "Chats"
            };

            return !filters.Any(x => x == table);
        }

        public void OnBeforeSaveChanges()
        {
            var entries = ChangeTracker.Entries();
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in entries)
            {
                if (entry.Entity is ExternalUser externalUser)
                {
                    externalUser.FullName = $"{(string.IsNullOrEmpty(externalUser.PaternalSurname) ? "" : $"{externalUser.PaternalSurname} ")}{(string.IsNullOrEmpty(externalUser.MaternalSurname) ? "" : $"{externalUser.MaternalSurname}")} {(string.IsNullOrEmpty(externalUser.Name) ? "" : $"{externalUser.Name}")}";
                }

                var auditEntry = OnCreateAuditEntry(entry);

                if (auditEntry != null && EntityFilter(auditEntry))
                {
                    auditEntries.Add(auditEntry);
                }

                OnChangeEntityEntryState(entry);
            }

            for (var i = 0; i < auditEntries.Count; i++)
            {
                var auditEntry = auditEntries[i];

                if (auditEntry.HasTemporaryProperties)
                {
                    _temporaryPropertyAuditEntries.Add(auditEntry);
                }
                else
                {
                    OnSaveAuditEntry(auditEntry);
                }
            }
        }

        public void OnAfterSaveChanges()
        {
            OnCreateAuditEntryTemporaryProperty(_temporaryPropertyAuditEntries);
        }

        public AuditEntry OnCreateAuditEntry(EntityEntry entityEntry)
        {
            var userName = GetCurrentUser();

            if (!(entityEntry.Entity is Audit))
            {
                var auditEntry = entityEntry.ToAuditEntry(userName);

                return auditEntry;
            }

            return null;
        }

        private void OnCreateAuditEntryTemporaryProperty(List<AuditEntry> auditEntries)
        {
            if (auditEntries != null)
            {
                for (var i = 0; i < auditEntries.Count; i++)
                {
                    var auditEntry = auditEntries[i];

                    if (auditEntry.HasTemporaryProperties)
                    {
                        var auditEntryTemporaryProperties = auditEntry.TemporaryProperties;

                        for (var j = 0; j < auditEntryTemporaryProperties.Count; j++)
                        {
                            var auditEntryTemporaryProperty = auditEntryTemporaryProperties[j];
                            var propertyMetadata = auditEntryTemporaryProperty.Metadata;

                            if (propertyMetadata.IsPrimaryKey())
                            {
                                auditEntry.KeyValues[propertyMetadata.Name] = auditEntryTemporaryProperty.CurrentValue;
                            }
                            else
                            {
                                auditEntry.NewValues[propertyMetadata.Name] = auditEntryTemporaryProperty.CurrentValue;
                            }
                        }

                        OnSaveAuditEntry(auditEntry);
                    }
                }
            }
        }

        public void OnSaveAuditEntry(AuditEntry auditEntry)
        {
            var absoluteUri = "";

            if (_httpContextAccessor.HttpContext != null)
            {
                absoluteUri = string.Concat(
                    _httpContextAccessor.HttpContext.Request.Scheme,
                    "://",
                    _httpContextAccessor.HttpContext.Request.Host.ToUriComponent(),
                    _httpContextAccessor.HttpContext.Request.PathBase.ToUriComponent(),
                    _httpContextAccessor.HttpContext.Request.Path.ToUriComponent(),
                    _httpContextAccessor.HttpContext.Request.QueryString.ToUriComponent());
            }

            var audit = new Audit
            {
                TableName = auditEntry.TableName,
                UserName = auditEntry.UserName,
                DateTime = DateTime.UtcNow,
                KeyValues = JsonConvert.SerializeObject(auditEntry.KeyValues),
                OldValues = auditEntry.OldValues.Count <= 0 ? null : JsonConvert.SerializeObject(auditEntry.OldValues),
                NewValues = auditEntry.NewValues.Count <= 0 ? null : JsonConvert.SerializeObject(auditEntry.NewValues),
                AbsoluteUri = absoluteUri
            };

            Audits.Add(audit);
        }

        /// <summary>
        /// Metodo base ejecutado al realizar cambios en entidades de base de datos
        /// </summary>
        /// <param name="entityEntry"></param>
        /// <returns></returns>
        public EntityEntry OnChangeEntityEntryState(EntityEntry entityEntry)
        {
            var dateTimeNow = DateTime.UtcNow;
            var userName = GetCurrentUser();

            try
            {
                /// Validacion del tipo de cambio y llenado de campos de auditoria
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        entityEntry.SetCurrentValue(ConstantHelpers.ENTITY_ENTRIES.PROPERTY_NAME.CREATED_AT, dateTimeNow);
                        entityEntry.SetCurrentValue(ConstantHelpers.ENTITY_ENTRIES.PROPERTY_NAME.CREATED_BY, userName);

                        break;
                    case EntityState.Deleted:
                        entityEntry.SetCurrentValue(ConstantHelpers.ENTITY_ENTRIES.PROPERTY_NAME.DELETED_AT, dateTimeNow);
                        entityEntry.SetCurrentValue(ConstantHelpers.ENTITY_ENTRIES.PROPERTY_NAME.DELETED_BY, userName);

                        if (
                            entityEntry.HasPropertyEntry(ConstantHelpers.ENTITY_ENTRIES.PROPERTY_NAME.DELETED_AT) ||
                            entityEntry.HasPropertyEntry(ConstantHelpers.ENTITY_ENTRIES.PROPERTY_NAME.DELETED_BY)
                        )
                        {
                            entityEntry.State = EntityState.Modified;

                            OnChangeEntityEntryState(entityEntry);
                        }

                        break;
                    case EntityState.Modified:
                        entityEntry.SetCurrentValue(ConstantHelpers.ENTITY_ENTRIES.PROPERTY_NAME.UPDATED_AT, dateTimeNow);
                        entityEntry.SetCurrentValue(ConstantHelpers.ENTITY_ENTRIES.PROPERTY_NAME.UPDATED_BY, userName);

                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                // throw ?
            }

            return entityEntry;
        }

        private string GetCurrentUser()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var user = httpContext.User;
                var claim = user.FindFirst(ClaimTypes.Name);

                return claim?.Value;
            }

            return null;
        }

        //private void MappingSqlBulkInstance(SqlBulkCopy sqlBulkCopy)
        //{
        //    sqlBulkCopy.ColumnMappings.Add(nameof(Audit.TableName), nameof(Audit.TableName));
        //    sqlBulkCopy.ColumnMappings.Add(nameof(Audit.DateTime), nameof(Audit.DateTime));
        //    sqlBulkCopy.ColumnMappings.Add(nameof(Audit.KeyValues), nameof(Audit.KeyValues));
        //    sqlBulkCopy.ColumnMappings.Add(nameof(Audit.OldValues), nameof(Audit.OldValues));
        //    sqlBulkCopy.ColumnMappings.Add(nameof(Audit.NewValues), nameof(Audit.NewValues));
        //    sqlBulkCopy.ColumnMappings.Add(nameof(Audit.UserName), nameof(Audit.UserName));
        //}
    }
}