using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.INTRANET.Services.EvaluationReportGenerator;
using AKDEMIC.PDF.Services.AcademicRecordGenerator;
using AKDEMIC.PDF.Services.CertificateGenerator;
using AKDEMIC.PDF.Services.CertificateMeritOrderGenerator;
using AKDEMIC.PDF.Services.CertificateOfStudiesGenerator;
using AKDEMIC.PDF.Services.CompleteCurriculumGenerator;
using AKDEMIC.PDF.Services.ReportCardGenerator;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Degree.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Degree.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Geo.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Geo.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Implementatios;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Server.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Server.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Implementations;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Degree.Implementations;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Implementations;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment;
using AKDEMIC.SERVICE.Services.Enrollment.Implementations;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals;
using AKDEMIC.SERVICE.Services.Generals.Implementations;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Geo.Implementations;
using AKDEMIC.SERVICE.Services.Geo.Interfaces;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Implementations;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.PosDegree.Implementations;
using AKDEMIC.SERVICE.Services.Scale.Implementations;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Server.Implementations;
using AKDEMIC.SERVICE.Services.Server.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Implementations;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Implementations;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeacherService = AKDEMIC.SERVICE.Services.Generals.Implementations.TeacherService;

namespace AKDEMIC.INTRANET
{
    public static class ConfigureContainerExtensions
    {
        public static void AddRepository(this IServiceCollection serviceCollection)
        {
            // Base
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // General
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            serviceCollection.AddScoped(typeof(IRecordHistoryRepository), typeof(RecordHistoryRepository));
            serviceCollection.AddScoped(typeof(IStudentRepository), typeof(StudentRepository));
            serviceCollection.AddScoped(typeof(ITermRepository), typeof(TermRepository));
            serviceCollection.AddScoped(typeof(ICareerRepository), typeof(CareerRepository));
            serviceCollection.AddScoped(typeof(IUserNotificationRepository), typeof(UserNotificationRepository));
            serviceCollection.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            serviceCollection.AddScoped(typeof(IRoleRepository), typeof(RoleRepository));
            serviceCollection.AddScoped(typeof(IAcademicProgramRepository), typeof(AcademicProgramRepository));
            serviceCollection.AddScoped(typeof(IStudentRepository), typeof(StudentRepository));
            serviceCollection.AddScoped(typeof(ITeacherRepository), typeof(REPOSITORY.Repositories.Generals.Implementations.TeacherRepository));
            serviceCollection.AddScoped(typeof(ICountryRepository), typeof(CountryRepository));
            serviceCollection.AddScoped(typeof(IDepartmentRepository), typeof(DepartmentRepository));
            serviceCollection.AddScoped(typeof(IProvinceRepository), typeof(ProvinceRepository));
            serviceCollection.AddScoped(typeof(IDistrictRepository), typeof(DistrictRepository));
            serviceCollection.AddScoped(typeof(IDeanRepository), typeof(DeanRepository));
            serviceCollection.AddScoped(typeof(IRecordHistoryObservationRepository), typeof(RecordHistoryObservationRepository));
            serviceCollection.AddScoped(typeof(IHolidayRepository), typeof(HolidayRepository));
            serviceCollection.AddScoped(typeof(IBeginningAnnouncementRepository), typeof(BeginningAnnouncementRepository));
            serviceCollection.AddScoped(typeof(IUserAnnouncementRepository), typeof(UserAnnouncementRepository));
            serviceCollection.AddScoped(typeof(IEmailManagementRepository), typeof(EmailManagementRepository));
            serviceCollection.AddScoped(typeof(ICareerAccreditationRepository), typeof(CareerAccreditationRepository));
            serviceCollection.AddScoped(typeof(IYearInformationRepository), typeof(YearInformationRepository));
            serviceCollection.AddScoped(typeof(IExternalUserRepository), typeof(ExternalUserRepository));
            serviceCollection.AddScoped(typeof(IStudentScaleRepository), typeof(StudentScaleRepository));
            serviceCollection.AddScoped(typeof(IEnrollmentFeeRepository), typeof(EnrollmentFeeRepository));

            // Documentary Procedure
            serviceCollection.AddScoped(typeof(IProcedureRepository), typeof(ProcedureRepository));
            serviceCollection.AddScoped(typeof(IProcedureDependencyRepository), typeof(ProcedureDependencyRepository));
            serviceCollection.AddScoped(typeof(IProcedureRequirementRepository), typeof(ProcedureRequirementRepository));
            serviceCollection.AddScoped(typeof(IUITRepository), typeof(UITRepository));
            serviceCollection.AddScoped(typeof(IUserProcedureRepository), typeof(UserProcedureRepository));
            serviceCollection.AddScoped(typeof(IUserProcedureDerivationRepository), typeof(UserProcedureDerivationRepository));
            serviceCollection.AddScoped(typeof(IUserDependencyRepository), typeof(UserDependencyRepository));
            serviceCollection.AddScoped(typeof(IDependencyRepository), typeof(DependencyRepository));
            serviceCollection.AddScoped(typeof(IDocumentTypeRepository), typeof(DocumentTypeRepository));
            serviceCollection.AddScoped(typeof(IInternalProcedureRepository), typeof(InternalProcedureRepository));
            serviceCollection.AddScoped(typeof(IUserInternalProcedureRepository), typeof(UserInternalProcedureRepository));
            serviceCollection.AddScoped(typeof(IUserProcedureFileRepository), typeof(UserProcedureFileRepository));
            serviceCollection.AddScoped(typeof(IUserProcedureDerivationFileRepository), typeof(UserProcedureDerivationFileRepository));
            serviceCollection.AddScoped(typeof(IProcedureTaskRepository), typeof(ProcedureTaskRepository));
            serviceCollection.AddScoped(typeof(IStudentUserProcedureRepository), typeof(StudentUserProcedureRepository));

            // Economic Management
            serviceCollection.AddScoped(typeof(IPaymentRepository), typeof(PaymentRepository));
            serviceCollection.AddScoped(typeof(IInvoiceRepository), typeof(InvoiceRepository));
            serviceCollection.AddScoped(typeof(IInvoiceDetailsRepository), typeof(InvoiceDetailsRepository));
            serviceCollection.AddScoped(typeof(IConceptRepository), typeof(ConceptRepository));
            serviceCollection.AddScoped(typeof(IClassifierRepository), typeof(ClassifierRepository));

            // Enrollment
            serviceCollection.AddScoped(typeof(ICampusRepository), typeof(CampusRepository));
            serviceCollection.AddScoped(typeof(ICourseRepository), typeof(CourseRepository));
            serviceCollection.AddScoped(typeof(IEvaluationRepository), typeof(EvaluationRepository));
            serviceCollection.AddScoped(typeof(ISectionRepository), typeof(SectionRepository));
            serviceCollection.AddScoped(typeof(IFacultyRepository), typeof(FacultyRepository));
            serviceCollection.AddScoped(typeof(IStudentSectionRepository), typeof(StudentSectionRepository));
            serviceCollection.AddScoped(typeof(IResolutionRepository), typeof(ResolutionRepository));
            serviceCollection.AddScoped(typeof(IAcademicYearCourseRepository), typeof(AcademicYearCourseRepository));
            serviceCollection.AddScoped(typeof(IStudentSectionRepository), typeof(StudentSectionRepository));
            serviceCollection.AddScoped(typeof(IAcademicYearCourseRepository), typeof(AcademicYearCourseRepository));
            serviceCollection.AddScoped(typeof(IEnrollmentReservationRepository), typeof(EnrollmentReservationRepository));
            serviceCollection.AddScoped(typeof(IClassScheduleRepository), typeof(ClassScheduleRepository));
            serviceCollection.AddScoped(typeof(IAdmissionTypeRepository), typeof(AdmissionTypeRepository));
            serviceCollection.AddScoped(typeof(IPostulantRepository), typeof(PostulantRepository));
            serviceCollection.AddScoped(typeof(IEnrollmentReservationRepository), typeof(EnrollmentReservationRepository));
            serviceCollection.AddScoped(typeof(IWorkingDayRepository), typeof(WorkingDayRepository));
            serviceCollection.AddScoped(typeof(ICurriculumRepository), typeof(CurriculumRepository));
            serviceCollection.AddScoped(typeof(ITeacherSectionRepository), typeof(TeacherSectionRepository));
            serviceCollection.AddScoped(typeof(ICourseUnitRepository), typeof(CourseUnitRepository));
            serviceCollection.AddScoped(typeof(IAdmissionTypeRepository), typeof(AdmissionTypeRepository));
            serviceCollection.AddScoped(typeof(IBuildingRepository), typeof(BuildingRepository));
            serviceCollection.AddScoped(typeof(IAreaRepository), typeof(AreaRepository));
            serviceCollection.AddScoped(typeof(IClassroomRepository), typeof(ClassroomRepository));
            serviceCollection.AddScoped(typeof(IAcademicYearCreditRepository), typeof(AcademicYearCreditRepository));
            serviceCollection.AddScoped(typeof(IStudentCourseCertificateRepository), typeof(StudentCourseCertificateRepository));
            serviceCollection.AddScoped(typeof(ICourseCertificateRepository), typeof(CourseCertificateRepository));
            serviceCollection.AddScoped(typeof(IEnrollmentShiftRepository), typeof(EnrollmentShiftRepository));
            serviceCollection.AddScoped(typeof(ICareerEnrollmentShiftRepository), typeof(CareerEnrollmentShiftRepository));
            serviceCollection.AddScoped(typeof(IEnrollmentConceptRepository), typeof(EnrollmentConceptRepository));

            // Intranet
            serviceCollection.AddScoped(typeof(IAcademicHistoryDocumentRepository), typeof(AcademicHistoryDocumentRepository));
            serviceCollection.AddScoped(typeof(IAcademicRecordDepartmentRepository), typeof(AcademicRecordDepartmentRepository));
            serviceCollection.AddScoped(typeof(IAcademicCalendarDateRepository), typeof(AcademicCalendarDateRepository));
            serviceCollection.AddScoped(typeof(IAcademicHistoryRepository), typeof(AcademicHistoryRepository));
            serviceCollection.AddScoped(typeof(IAcademicSummariesRepository), typeof(AcademicSummariesRepository));
            serviceCollection.AddScoped(typeof(IWorkingDayRepository), typeof(WorkingDayRepository));
            serviceCollection.AddScoped(typeof(ISurveyRepository), typeof(SurveyRepository));
            serviceCollection.AddScoped(typeof(ISurveyUserRepository), typeof(SurveyUserRepository));
            serviceCollection.AddScoped(typeof(ISurveyItemRepository), typeof(SurveyItemRepository));
            serviceCollection.AddScoped(typeof(IAnswerByUserRepository), typeof(AnswerByUserRepository));
            serviceCollection.AddScoped(typeof(IAnswerRepository), typeof(AnswerRepository));
            serviceCollection.AddScoped(typeof(IQuestionRepository), typeof(QuestionRepository));
            serviceCollection.AddScoped(typeof(IStudentAbsenceJustificationRepository), typeof(StudentAbsenceJustificationRepository));
            serviceCollection.AddScoped(typeof(IUserAbsenceJustificationRepository), typeof(UserAbsenceJustificationRepository));
            serviceCollection.AddScoped(typeof(IClassRepository), typeof(ClassRepository));
            serviceCollection.AddScoped(typeof(IClassStudentRepository), typeof(ClassStudentRepository));
            serviceCollection.AddScoped(typeof(ICourseTermRepository), typeof(CourseTermRepository));
            serviceCollection.AddScoped(typeof(IClassScheduleRepository), typeof(ClassScheduleRepository));
            serviceCollection.AddScoped(typeof(IClassRescheduleRepository), typeof(ClassRescheduleRepository));
            serviceCollection.AddScoped(typeof(IClassStudentRepository), typeof(ClassStudentRepository));
            serviceCollection.AddScoped(typeof(IStudentObservationRepository), typeof(StudentObservationRepository));
            serviceCollection.AddScoped(typeof(ITutorialRepository), typeof(TutorialRepository));
            serviceCollection.AddScoped(typeof(ITutorialStudentRepository), typeof(TutorialStudentRepository));
            serviceCollection.AddScoped(typeof(IEventRepository), typeof(EventRepository));
            serviceCollection.AddScoped(typeof(IEventTypeRepository), typeof(EventTypeRepository));
            serviceCollection.AddScoped(typeof(IEventRoleRepository), typeof(EventRoleRepository));
            serviceCollection.AddScoped(typeof(IUserEventRepository), typeof(UserEventRepository));
            serviceCollection.AddScoped(typeof(IEvaluationReportRepository), typeof(EvaluationReportRepository));
            serviceCollection.AddScoped(typeof(IExtracurricularCourseRepository), typeof(ExtracurricularCourseRepository));
            serviceCollection.AddScoped(typeof(IExtracurricularCourseGroupRepository), typeof(ExtracurricularCourseGroupRepository));
            serviceCollection.AddScoped(typeof(IExtracurricularCourseGroupStudentRepository), typeof(ExtracurricularCourseGroupStudentRepository));
            serviceCollection.AddScoped(typeof(IExtracurricularCourseGroupAssistanceRepository), typeof(ExtracurricularCourseGroupAssistanceRepository));
            serviceCollection.AddScoped(typeof(IExtracurricularCourseGroupAssistanceStudentRepository), typeof(ExtracurricularCourseGroupAssistanceStudentRepository));
            serviceCollection.AddScoped(typeof(IExtracurricularActivityRepository), typeof(ExtracurricularActivityRepository));
            serviceCollection.AddScoped(typeof(IExtracurricularActivityStudentRepository), typeof(ExtracurricularActivityStudentRepository));
            serviceCollection.AddScoped(typeof(IExtracurricularAreaRepository), typeof(ExtracurricularAreaRepository));
            serviceCollection.AddScoped(typeof(IGradeRepository), typeof(GradeRepository));
            serviceCollection.AddScoped(typeof(IGradeCorrectionRepository), typeof(GradeCorrectionRepository));
            serviceCollection.AddScoped(typeof(IGradeRegistrationRepository), typeof(GradeRegistrationRepository));
            serviceCollection.AddScoped(typeof(IInstitutionalAlertRepository), typeof(InstitutionalAlertRepository));
            serviceCollection.AddScoped(typeof(IPsychologicalDiagnosticRepository), typeof(PsychologicalDiagnosticRepository));
            serviceCollection.AddScoped(typeof(IAnnouncementRepository), typeof(AnnouncementRepository));
            serviceCollection.AddScoped(typeof(IRolAnnouncementRepository), typeof(RolAnnouncementRepository));
            serviceCollection.AddScoped(typeof(IForumRepository), typeof(ForumRepository));
            serviceCollection.AddScoped(typeof(IPostRepository), typeof(PostRepository));
            serviceCollection.AddScoped(typeof(ISubstituteExamRepository), typeof(SubstituteExamRepository));
            serviceCollection.AddScoped(typeof(IClassRepository), typeof(ClassRepository));
            serviceCollection.AddScoped(typeof(IUserEventRepository), typeof(UserEventRepository));
            serviceCollection.AddScoped(typeof(IInstitutionalAlertRepository), typeof(InstitutionalAlertRepository));
            serviceCollection.AddScoped(typeof(IConfigurationRepository), typeof(ConfigurationRepository));
            serviceCollection.AddScoped(typeof(IConnectionRepository), typeof(ConnectionRepository));
            serviceCollection.AddScoped(typeof(ITopicRepository), typeof(TopicRepository));
            serviceCollection.AddScoped(typeof(IDirectedCourseRepository), typeof(DirectedCourseRepository));
            serviceCollection.AddScoped(typeof(IDirectedCourseStudentRepository), typeof(DirectedCourseStudentRepository));
            serviceCollection.AddScoped(typeof(IGradeReportRepository), typeof(GradeReportRepository));
            serviceCollection.AddScoped(typeof(IDigitizedSignatureRepository), typeof(DigitizedSignatureRepository));
            serviceCollection.AddScoped(typeof(IUniversityAuthorityRepository), typeof(UniversityAuthorityRepository));
            serviceCollection.AddScoped(typeof(IWelfareCategoryRepository), typeof(WelfareCategoryRepository));
            serviceCollection.AddScoped(typeof(ISuggestionsAndTipRepository), typeof(SuggestionsAndTipRepository));
            serviceCollection.AddScoped(typeof(IDeanFacultyRepository), typeof(DeanFacultyRepository));
            serviceCollection.AddScoped(typeof(IEnrollmentTurnRepository), typeof(EnrollmentTurnRepository));
            serviceCollection.AddScoped(typeof(ISubstituteExamDetailRepository), typeof(SubstituteExamDetailRepository));
            serviceCollection.AddScoped(typeof(IExamWeekRepository), typeof(ExamWeekRepository));
            serviceCollection.AddScoped(typeof(IStudentInformationRepository), typeof(StudentInformationRepository));

            serviceCollection.AddScoped(typeof(IPsychologicalRecordRepository), typeof(PsychologicalRecordRepository));
            serviceCollection.AddScoped(typeof(INutritionalRecordRepository), typeof(NutritionalRecordRepository));
            serviceCollection.AddScoped(typeof(IStudentFamilyRepository), typeof(StudentFamilyRepository));
            serviceCollection.AddScoped(typeof(IGradeReportRequirementRepository), typeof(GradeReportRequirementRepository));
            serviceCollection.AddScoped(typeof(IRecordsConceptRepository), typeof(RecordsConceptRepository));
            serviceCollection.AddScoped(typeof(ISectionsDuplicateContentRepository), typeof(SectionsDuplicateContentRepository));
            serviceCollection.AddScoped(typeof(IExtraordinaryEvaluationStudentRepository), typeof(ExtraordinaryEvaluationStudentRepository));
            serviceCollection.AddScoped(typeof(IGradeRecoveryExamRepository), typeof(GradeRecoveryExamRepository));
            serviceCollection.AddScoped(typeof(IGradeRecoveryRepository), typeof(GradeRecoveryRepository));
            serviceCollection.AddScoped(typeof(IStudentIncomeScoreRepository), typeof(StudentIncomeScoreRepository));
            serviceCollection.AddScoped(typeof(IExtraordinaryEvaluationRepository), typeof(ExtraordinaryEvaluationRepository));
            serviceCollection.AddScoped(typeof(IExtraordinaryEvaluationCommitteeRepository), typeof(ExtraordinaryEvaluationCommitteeRepository));
            serviceCollection.AddScoped(typeof(IWeeklyAttendanceReportRepository), typeof(WeeklyAttendanceReportRepository));
            serviceCollection.AddScoped(typeof(IGradeRectificationRepository), typeof(GradeRectificationRepository));
            serviceCollection.AddScoped(typeof(ISubstituteExamCorrectionRepository), typeof(SubstituteExamCorrectionRepository));
            serviceCollection.AddScoped(typeof(IEventFileRepository), typeof(EventFileRepository));

            serviceCollection.AddScoped(typeof(IDeferredExamRepository), typeof(DeferredExamRepository));
            serviceCollection.AddScoped(typeof(IDeferredExamStudentRepository), typeof(DeferredExamStudentRepository));
            serviceCollection.AddScoped(typeof(IStudentPortfolioRepository), typeof(StudentPortfolioRepository));
            serviceCollection.AddScoped(typeof(IDocumentFormatRepository), typeof(DocumentFormatRepository));

            serviceCollection.AddScoped(typeof(ISectionGroupRepository), typeof(SectionGroupRepository));
            serviceCollection.AddScoped(typeof(IStudentPortfolioTypeRepository), typeof(StudentPortfolioTypeRepository));

            serviceCollection.AddScoped(typeof(ICorrectionExamRepository), typeof(CorrectionExamRepository));

            serviceCollection.AddScoped(typeof(ICafobeRequestRepository), typeof(CafobeRequestRepository));
            serviceCollection.AddScoped(typeof(ICafobeRequestDetailRepository), typeof(CafobeRequestDetailRepository));
            serviceCollection.AddScoped(typeof(ISectionEvaluationRepository), typeof(SectionEvaluationRepository));


            //TeachingManagement
            serviceCollection.AddScoped(typeof(ICourseComponentRepository), typeof(CourseComponentRepository));
            serviceCollection.AddScoped(typeof(IPerformanceEvaluationRepository), typeof(PerformanceEvaluationRepository));
            serviceCollection.AddScoped(typeof(IPerformanceEvaluationTemplateRepository), typeof(PerformanceEvaluationTemplateRepository));
            serviceCollection.AddScoped(typeof(IPerformanceEvaluationUserRepository), typeof(PerformanceEvaluationUserRepository));
            serviceCollection.AddScoped(typeof(IScoreInputScheduleRepository), typeof(ScoreInputScheduleRepository));
            serviceCollection.AddScoped(typeof(ITeacherScheduleRepository), typeof(TeacherScheduleRepository));
            serviceCollection.AddScoped(typeof(INonTeachingLoadScheduleRepositoy), typeof(NonTeachingLoadScheduleRepositoy));
            serviceCollection.AddScoped(typeof(INonTeachingLoadRepository), typeof(NonTeachingLoadRepository));
            serviceCollection.AddScoped(typeof(IPerformanceEvaluationCriterionRepository), typeof(PerformanceEvaluationCriterionRepository));
            serviceCollection.AddScoped(typeof(IEvaluationTypeRepository), typeof(EvaluationTypeRepository));
            serviceCollection.AddScoped(typeof(IExtraTeachingLoadRepository), typeof(ExtraTeachingLoadRepository));
            serviceCollection.AddScoped(typeof(IAcademicYearCoursePreRequisiteRepository), typeof(AcademicYearCoursePreRequisiteRepository));
            serviceCollection.AddScoped(typeof(ICourseSyllabusRepository), typeof(CourseSyllabusRepository));
            serviceCollection.AddScoped(typeof(ICourseSyllabusWeekRepository), typeof(CourseSyllabusWeekRepository));
            serviceCollection.AddScoped(typeof(ISyllabusTeacherRepository), typeof(SyllabusTeacherRepository));
            serviceCollection.AddScoped(typeof(ICourseSyllabusTeacherRepository), typeof(CourseSyllabusTeacherRepository));
            serviceCollection.AddScoped(typeof(IPerformanceEvaluationRatingScaleRepository), typeof(PerformanceEvaluationRatingScaleRepository));

            //Geo
            serviceCollection.AddScoped(typeof(ILaboratoryRequestRepository), typeof(LaboratoryRequestRepository));

            //Degree
            serviceCollection.AddScoped(typeof(IRegistryPatternRepository), typeof(RegistryPatternRepository));

            serviceCollection.AddScoped(typeof(IAcademicDepartmentRepository), typeof(AcademicDepartmentRepository));
            serviceCollection.AddScoped(typeof(IDegreeRequirementRepository), typeof(DegreeRequirementRepository));

            serviceCollection.AddScoped(typeof(IEnrollmentTurnRepository), typeof(EnrollmentTurnRepository));
            serviceCollection.AddScoped(typeof(ITemporalGradeRepository), typeof(TemporalGradeRepository));
            serviceCollection.AddScoped(typeof(IInstitutionalWelfareRecordRepository), typeof(InstitutionalWelfareRecordRepository));
            serviceCollection.AddScoped(typeof(IInstitutionalWelfareSectionRepository), typeof(InstitutionalWelfareSectionRepository));
            serviceCollection.AddScoped(typeof(IInstitutionalRecordCategorizationByStudentRepository), typeof(InstitutionalRecordCategorizationByStudentRepository));
            serviceCollection.AddScoped(typeof(IInstitutionalWelfareAnswerByStudentRepository), typeof(InstitutionalWelfareAnswerByStudentRepository));
            serviceCollection.AddScoped(typeof(IInstitutionalWelfareAnswerRepository), typeof(InstitutionalWelfareAnswerRepository));

            serviceCollection.AddScoped(typeof(ITutoringAnnouncementRepository), typeof(TutoringAnnouncementRepository));
            serviceCollection.AddScoped<ITutoringAnnouncementRoleRepository, TutoringAnnouncementRoleRepository>();
            serviceCollection.AddScoped<ITutoringAnnouncementCareerRepository, TutoringAnnouncementCareerRepository>();
            //PosDregre
            serviceCollection.AddScoped(typeof(IMasterRepository), typeof(MasterRepository));
            //Scale
            serviceCollection.AddScoped<IWorkerLaborConditionRepository, WorkerLaborConditionRepository>();
            serviceCollection.AddScoped<IWorkerLaborInformationRepository, WorkerLaborInformationRepository>();
            serviceCollection.AddScoped<IWorkerLaborCategoryRepository, WorkerLaborCategoryRepository>();
            serviceCollection.AddScoped<ITutoringStudentRepository, TutoringStudentRepository>();
            serviceCollection.AddScoped<IScaleExtraPerformanceEvaluationFieldRepository, ScaleExtraPerformanceEvaluationFieldRepository>();

            //Tutoria
            serviceCollection.AddScoped(typeof(ITutoringStudentRepository), typeof(TutoringStudentRepository));
            serviceCollection.AddScoped(typeof(ITutoringMessageRepository), typeof(TutoringMessageRepository));

            serviceCollection.AddScoped<IGeneralLinkRepository, GeneralLinkRepository>();

        }

        public static void AddTransientServices(this IServiceCollection serviceCollection)
        {
            //General
            serviceCollection.AddTransient<ICareerService, CareerService>();
            serviceCollection.AddTransient<IRecordHistoryService, RecordHistoryService>();
            serviceCollection.AddTransient<IStudentService, StudentService>();
            serviceCollection.AddTransient<ITermService, TermService>();
            serviceCollection.AddTransient<ICareerService, CareerService>();
            serviceCollection.AddTransient<IUserNotificationService, UserNotificationService>();
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IRoleService, RoleService>();
            serviceCollection.AddTransient<IAcademicProgramService, AcademicProgramService>();
            serviceCollection.AddTransient<IStudentService, StudentService>();
            serviceCollection.AddTransient<ITeacherService, TeacherService>();
            serviceCollection.AddTransient<ICountryService, CountryService>();
            serviceCollection.AddTransient<IDepartmentService, DepartmentService>();
            serviceCollection.AddTransient<IProvinceService, ProvinceService>();
            serviceCollection.AddTransient<IDistrictService, DistrictService>();
            serviceCollection.AddTransient<IDeanService, DeanService>();
            serviceCollection.AddTransient<IRecordHistoryObservationService, RecordHistoryObservationService>();
            serviceCollection.AddTransient<IHolidayService, HolidayService>();
            serviceCollection.AddTransient<IBeginningAnnouncementService, BeginningAnnouncementService>();
            serviceCollection.AddTransient<IUserAnnouncementService, UserAnnouncementService>();
            serviceCollection.AddTransient<IEmailManagementService, EmailManagementService>();
            serviceCollection.AddTransient<ICareerAccreditationService, CareerAccreditationService>();
            serviceCollection.AddTransient<IYearInformationService, YearInformationService>();
            serviceCollection.AddTransient<IExternalUserService, ExternalUserService>();
            serviceCollection.AddTransient<IStudentScaleService, StudentScaleService>();
            serviceCollection.AddTransient<IEnrollmentFeeService, EnrollmentFeeService>();

            // Documentary Procedure
            serviceCollection.AddTransient<IProcedureService, ProcedureService>();
            serviceCollection.AddTransient<IProcedureDependencyService, ProcedureDependencyService>();
            serviceCollection.AddTransient<IProcedureRequirementService, ProcedureRequirementService>();
            serviceCollection.AddTransient<IUITService, UITService>();
            serviceCollection.AddTransient<IUserProcedureService, UserProcedureService>();
            serviceCollection.AddTransient<IUserProcedureDerivationService, UserProcedureDerivationService>();
            serviceCollection.AddTransient<IUserDependencyService, UserDependencyService>();
            serviceCollection.AddTransient<IDependencyService, DependencyService>();
            serviceCollection.AddTransient<IDocumentTypeService, DocumentTypeService>();
            serviceCollection.AddTransient<IInternalProcedureService, InternalProcedureService>();
            serviceCollection.AddTransient<IUserInternalProcedureService, UserInternalProcedureService>();
            serviceCollection.AddTransient<IUserProcedureFileService, UserProcedureFileService>();
            serviceCollection.AddTransient<IUserProcedureDerivationFileService, UserProcedureDerivationFileService>();
            serviceCollection.AddTransient<IProcedureTaskService, ProcedureTaskService>();
            serviceCollection.AddTransient<IStudentUserProcedureService, StudentUserProcedureService>();

            // Economic Management
            serviceCollection.AddTransient<IPaymentService, PaymentService>();
            serviceCollection.AddTransient<IInvoiceService, InvoiceService>();
            serviceCollection.AddTransient<IInvoiceDetailsService, InvoiceDetailsService>();
            serviceCollection.AddTransient<IConceptService, ConceptService>();
            serviceCollection.AddTransient<IClassifierService, ClassifierService>();

            // Enrollment
            serviceCollection.AddTransient<ICampusService, CampusService>();
            serviceCollection.AddTransient<IEvaluationService, EvaluationService>();
            serviceCollection.AddTransient<ISectionService, SectionService>();
            serviceCollection.AddTransient<IFacultyService, FacultyService>();
            serviceCollection.AddTransient<ICourseService, CourseService>();
            serviceCollection.AddTransient<IStudentSectionService, StudentSectionService>();
            serviceCollection.AddTransient<IResolutionService, ResolutionService>();
            serviceCollection.AddTransient<IAcademicYearCourseService, AcademicYearCourseService>();
            serviceCollection.AddTransient<IWorkingDayService, WorkingDayService>();
            serviceCollection.AddTransient<IAcademicYearCourseService, AcademicYearCourseService>();
            serviceCollection.AddTransient<IStudentSectionService, StudentSectionService>();
            serviceCollection.AddTransient<IResolutionService, ResolutionService>();
            serviceCollection.AddTransient<IAcademicYearCourseService, AcademicYearCourseService>();
            serviceCollection.AddTransient<IClassScheduleService, ClassScheduleService>();
            serviceCollection.AddTransient<IResolutionService, ResolutionService>();
            serviceCollection.AddTransient<IPostulantService, PostulantService>();
            serviceCollection.AddTransient<IEnrollmentReservationService, EnrollmentReservationService>();
            serviceCollection.AddTransient<IWorkingDayService, WorkingDayService>();
            serviceCollection.AddTransient<IAdmissionTypeService, AdmissionTypeService>();
            serviceCollection.AddTransient<IAreaService, AreaService>();
            serviceCollection.AddTransient<IBuildingService, BuildingService>();
            serviceCollection.AddTransient<ICurriculumService, CurriculumService>();
            serviceCollection.AddTransient<ITeacherSectionService, TeacherSectionService>();
            serviceCollection.AddTransient<IAdmissionTypeService, AdmissionTypeService>();
            serviceCollection.AddTransient<IAreaService, AreaService>();
            serviceCollection.AddTransient<IClassroomService, ClassroomService>();
            serviceCollection.AddTransient<ICourseUnitService, CourseUnitService>();
            serviceCollection.AddTransient<IAcademicYearCreditService, AcademicYearCreditService>();
            serviceCollection.AddTransient<IStudentCourseCertificateService, StudentCourseCertificateService>();
            serviceCollection.AddTransient<ICourseCertificateService, CourseCertificateService>();
            serviceCollection.AddTransient<IEnrollmentShiftService, EnrollmentShiftService>();
            serviceCollection.AddTransient<ICareerEnrollmentShiftService, CareerEnrollmentShiftService>();
            serviceCollection.AddTransient<IEnrollmentConceptService, EnrollmentConceptService>();

            // Intranet
            //serviceCollection.AddTransient<IWorkingDayService, WorkingDayService>();
            serviceCollection.AddTransient<IAcademicHistoryDocumentService, AcademicHistoryDocumentService>();
            serviceCollection.AddTransient<IAcademicRecordDepartmentService, AcademicRecordDepartmentService>();
            serviceCollection.AddTransient<IAcademicCalendarDateService, AcademicCalendarDateService>();
            serviceCollection.AddTransient<IAcademicHistoryService, AcademicHistoryService>();
            serviceCollection.AddTransient<IAcademicSummariesService, AcademicSummariesService>();
            serviceCollection.AddTransient<ISurveyService, SurveyService>();
            serviceCollection.AddTransient<ISurveyUserService, SurveyUserService>();
            serviceCollection.AddTransient<ISurveyItemService, SurveyItemService>();
            serviceCollection.AddTransient<IAnswerByUserService, AnswerByUserService>();
            serviceCollection.AddTransient<IAnswerService, AnswerService>();
            serviceCollection.AddTransient<IQuestionService, QuestionService>();
            serviceCollection.AddTransient<ICourseTermService, CourseTermService>();
            serviceCollection.AddTransient<IClassService, ClassService>();
            serviceCollection.AddTransient<IClassScheduleService, ClassScheduleService>();
            serviceCollection.AddTransient<IClassStudentService, ClassStudentService>();
            serviceCollection.AddTransient<IClassRescheduleService, ClassRescheduleService>();
            serviceCollection.AddTransient<IQuestionService, QuestionService>();
            serviceCollection.AddTransient<IUserAbsenceJustificationService, UserAbsenceJustificationService>();
            serviceCollection.AddTransient<IStudentAbsenceJustificationService, StudentAbsenceJustificationService>();
            serviceCollection.AddTransient<IEvaluationReportService, EvaluationReportService>();
            serviceCollection.AddTransient<IExtracurricularCourseService, ExtracurricularCourseService>();
            serviceCollection.AddTransient<IExtracurricularCourseGroupService, ExtracurricularCourseGroupService>();
            serviceCollection.AddTransient<IExtracurricularCourseGroupStudentService, ExtracurricularCourseGroupStudentService>();
            serviceCollection.AddTransient<IStudentObservationService, StudentObservationService>();
            serviceCollection.AddTransient<IExtracurricularCourseGroupAssistanceService, ExtracurricularCourseGroupAssistanceService>();
            serviceCollection.AddTransient<IExtracurricularCourseGroupAssistanceStudentService, ExtracurricularCourseGroupAssistanceStudentService>();
            serviceCollection.AddTransient<IExtracurricularActivityService, ExtracurricularActivityService>();
            serviceCollection.AddTransient<IExtracurricularActivityStudentService, ExtracurricularActivityStudentService>();
            serviceCollection.AddTransient<IExtracurricularAreaService, ExtracurricularAreaService>();
            serviceCollection.AddTransient<IGradeService, GradeService>();
            serviceCollection.AddTransient<IGradeCorrectionService, GradeCorrectionService>();
            serviceCollection.AddTransient<IGradeRegistrationService, GradeRegistrationService>();
            serviceCollection.AddTransient<IInstitutionalAlertService, InstitutionalAlertService>();
            serviceCollection.AddTransient<IUserEventService, UserEventService>();
            serviceCollection.AddTransient<ITutorialService, TutorialService>();
            serviceCollection.AddTransient<ITutorialStudentService, TutorialStudentService>();
            serviceCollection.AddTransient<IEventService, EventService>();
            serviceCollection.AddTransient<IEventTypeService, EventTypeService>();
            serviceCollection.AddTransient<IEventRoleService, EventRoleService>();
            serviceCollection.AddTransient<IPsychologicalDiagnosticService, PsychologicalDiagnosticService>();
            serviceCollection.AddTransient<IAnnouncementService, AnnouncementService>();
            serviceCollection.AddTransient<IRolAnnouncementService, RolAnnouncementService>();
            serviceCollection.AddTransient<ISubstituteExamService, SubstituteExamService>();
            serviceCollection.AddTransient<IClassService, ClassService>();
            serviceCollection.AddTransient<IForumService, ForumService>();
            serviceCollection.AddTransient<IPostService, PostService>();
            serviceCollection.AddTransient<IUserEventService, UserEventService>();
            serviceCollection.AddTransient<IInstitutionalAlertService, InstitutionalAlertService>();
            serviceCollection.AddTransient<IConfigurationService, ConfigurationService>();
            serviceCollection.AddTransient<IConnectionService, ConnectionService>();
            serviceCollection.AddTransient<ITopicService, TopicService>();
            serviceCollection.AddTransient<IDirectedCourseService, DirectedCourseService>();
            serviceCollection.AddTransient<IDirectedCourseStudentService, DirectedCourseStudentService>();
            serviceCollection.AddTransient<IGradeReportService, GradeReportService>();
            serviceCollection.AddTransient<IDigitizedSignatureService, DigitizedSignatureService>();
            serviceCollection.AddTransient<IUniversityAuthorityService, UniversityAuthorityService>();
            serviceCollection.AddTransient<ISuggestionsAndTipService, SuggestionsAndTipService>();
            serviceCollection.AddTransient<IWelfareCategoryService, WelfareCategoryService>();
            serviceCollection.AddTransient<ISubstituteExamDetailService, SubstituteExamDetailService>();
            serviceCollection.AddTransient<IExamWeekService, ExamWeekService>();
            serviceCollection.AddTransient<IStudentInformationService, StudentInformationService>();

            serviceCollection.AddTransient<ICategorizationLevelService, CategorizationLevelService>();

            serviceCollection.AddTransient<IDeanFacultyService, DeanFacultyService>();
            serviceCollection.AddTransient<IPsychologicalRecordService, PsychologicalRecordService>();
            serviceCollection.AddTransient<INutritionalRecordService, NutritionalRecordService>();
            serviceCollection.AddTransient<IStudentFamilyService, StudentFamilyService>();
            serviceCollection.AddTransient<IGradeReportRequirementService, GradeReportRequirementService>();
            serviceCollection.AddTransient<IRecordsConceptService, RecordsConceptService>();
            serviceCollection.AddTransient<IEnrollmentTurnService, EnrollmentTurnService>();
            serviceCollection.AddTransient<ISectionsDuplicateContentService, SectionsDuplicateContentService>();
            serviceCollection.AddTransient<IExtraordinaryEvaluationStudentService, ExtraordinaryEvaluationStudentService>();
            serviceCollection.AddTransient<IGradeRecoveryService, GradeRecoveryService>();
            serviceCollection.AddTransient<IGradeRecoveryExamService, GradeRecoveryExamService>();
            serviceCollection.AddTransient<IStudentIncomeScoreService, StudentIncomeScoreService>();
            serviceCollection.AddTransient<IExtraordinaryEvaluationService, ExtraordinaryEvaluationService>();
            serviceCollection.AddTransient<IExtraordinaryEvaluationCommitteeService, ExtraordinaryEvaluationCommitteeService>();
            serviceCollection.AddTransient<IWeeklyAttendanceReportService, WeeklyAttendanceReportService>();
            serviceCollection.AddTransient<IGradeRectificationService, GradeRectificationService>();
            serviceCollection.AddTransient<ISubstituteExamCorrectionService, SubstituteExamCorrectionService>();
            serviceCollection.AddTransient<IEventFileService, EventFileService>();

            serviceCollection.AddTransient<IStudentPortfolioService, StudentPortfolioService>();
            serviceCollection.AddTransient<IDeferredExamService, DeferredExamService>();
            serviceCollection.AddTransient<IDeferredExamStudentService, DeferredExamStudentService>();
            serviceCollection.AddTransient<IDocumentFormatService, DocumentFormatService>();

            serviceCollection.AddTransient<ISectionGroupService, SectionGroupService>();
            serviceCollection.AddTransient<IStudentPortfolioTypeService, StudentPortfolioTypeService>();

            serviceCollection.AddTransient<ICorrectionExamService, CorrectionExamService>();
            serviceCollection.AddTransient<ICafobeRequestService, CafobeRequestService>();
            serviceCollection.AddTransient<ICafobeRequestDetailService, CafobeRequestDetailService>();
            serviceCollection.AddTransient<ISectionEvaluationService, SectionEvaluationService>();


            //TeachingManagement
            serviceCollection.AddTransient<ICourseComponentService, CourseComponentService>();
            serviceCollection.AddTransient<IPerformanceEvaluationService, PerformanceEvaluationService>();
            serviceCollection.AddTransient<IPerformanceEvaluationTemplateService, PerformanceEvaluationTemplateService>();
            serviceCollection.AddTransient<IPerformanceEvaluationUserService, PerformanceEvaluationUserService>();
            serviceCollection.AddTransient<IScoreInputScheduleService, ScoreInputScheduleService>();
            serviceCollection.AddTransient<ITeacherScheduleService, TeacherScheduleService>();
            serviceCollection.AddTransient<INonTeachingLoadScheduleService, NonTeachingLoadScheduleService>();
            serviceCollection.AddTransient<INonTeachingLoadService, NonTeachingLoadService>();
            serviceCollection.AddTransient<IPerformanceEvaluationCriterionService, PerformanceEvaluationCriterionService>();
            serviceCollection.AddTransient<IEvaluationTypeService, EvaluationTypeService>();
            serviceCollection.AddTransient<IExtraTeachingLoadService, ExtraTeachingLoadService>();
            serviceCollection.AddTransient<IAcademicYearCoursePreRequisiteService, AcademicYearCoursePreRequisiteService>();
            serviceCollection.AddTransient<ICourseSyllabusService, CourseSyllabusService>();
            serviceCollection.AddTransient<ICourseSyllabusWeekService, CourseSyllabusWeekService>();
            serviceCollection.AddTransient<ISyllabusTeacherService, SyllabusTeacherService>();
            serviceCollection.AddTransient<ICourseSyllabusTeacherService, CourseSyllabusTeacherService>();
            serviceCollection.AddTransient<IPerformanceEvaluationRatingScaleService, PerformanceEvaluationRatingScaleService>();

            //Geo
            serviceCollection.AddTransient<ILaboratoryRequestService, LaboratoryRequestService>();

            //Degree
            serviceCollection.AddTransient<IRegistryPatternService, RegistryPatternService>();

            serviceCollection.AddTransient<IAcademicDepartmentService, AcademicDepartmentService>();
            serviceCollection.AddTransient<IDegreeRequirementService, DegreeRequirementService>();

            serviceCollection.AddTransient<IEnrollmentTurnService, EnrollmentTurnService>();
            serviceCollection.AddTransient<ITemporalGradeService, TemporalGradeService>();
            serviceCollection.AddTransient<IInstitutionalWelfareRecordService, InstitutionalWelfareRecordService>();
            serviceCollection.AddTransient<IInstitutionalWelfareSectionService, InstitutionalWelfareSectionService>();
            serviceCollection.AddTransient<IInstitutionalRecordCategorizationByStudentService, InstitutionalRecordCategorizationByStudentService>();
            serviceCollection.AddTransient<IInstitutionalWelfareAnswerByStudentService, InstitutionalWelfareAnswerByStudentService>();
            serviceCollection.AddTransient<IInstitutionalWelfareAnswerService, InstitutionalWelfareAnswerService>();

            serviceCollection.AddTransient<ITutoringAnnouncementService, TutoringAnnouncementService>();
            serviceCollection.AddTransient<ITutoringAnnouncementRoleService, TutoringAnnouncementRoleService>();
            serviceCollection.AddTransient<ITutoringAnnouncementCareerService, TutoringAnnouncementCareerService>();
            //PosDegree
            serviceCollection.AddTransient<IMasterService, MasterService>();
            //Scale
            serviceCollection.AddTransient<IWorkerLaborConditionService, WorkerLaborConditionService>();
            serviceCollection.AddTransient<IWorkerLaborInformationService, WorkerLaborInformationService>();
            serviceCollection.AddTransient<IWorkerLaborCategoryService, WorkerLaborCategoryService>();

            serviceCollection.AddTransient<ITutoringStudentService, TutoringStudentService>();

            serviceCollection.AddTransient<IEvaluationReportGeneratorService, EvaluationReportGeneratorService>();

            serviceCollection.AddTransient<ICompleteCurriculumGeneratorService, CompleteCurriculumGeneratorService>();
            serviceCollection.AddTransient<IReportCardGeneratorService, ReportCardGeneratorService>();
            serviceCollection.AddTransient<ICertificateOfStudiesGeneratorService, CertificateOfStudiesGeneratorService>();

            serviceCollection.AddTransient<ICertificateGeneratorService, CertificateGeneratorService>();
            serviceCollection.AddTransient<ICertificateMeritOrderGeneratorService, CertificateMeritOrderGeneratorService>();
            serviceCollection.AddTransient<IAcademicRecordGeneratorService, AcademicRecordGeneratorService>();


            serviceCollection.AddTransient<IScaleExtraPerformanceEvaluationFieldService, ScaleExtraPerformanceEvaluationFieldService>();

            //Tutoria
            serviceCollection.AddTransient<ITutoringStudentService, TutoringStudentService>();
            serviceCollection.AddTransient<ITutoringMessageService, TutoringMessageService>();

            serviceCollection.AddTransient<IGeneralLinkService, GeneralLinkService>();

        }

        private static string GetDataConnectionStringFromConfig(IConfiguration configuration)
        {
            //return new DatabaseConfiguration().GetDataConnectionString();
            switch (ConstantHelpers.GENERAL.DATABASES.DATABASE)
            {
                case ConstantHelpers.DATABASES.MYSQL:
                default:
                    return configuration.GetConnectionString("MySqlDefaultConnection");
                case ConstantHelpers.DATABASES.SQL:
                    return configuration.GetConnectionString("SqlDefaultConnection");
            }
        }
    }
}
