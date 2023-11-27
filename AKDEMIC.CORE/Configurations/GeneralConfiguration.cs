using System.Collections.Generic;

namespace AKDEMIC.CORE.Configurations
{
    public class GeneralConfiguration
    {
        public static class General {
            public static class Database
            {
                public const int DATABASE = DatabaseConfiguration.SQL;

                public const bool FULLTEXT_ENABLED = false;

                public static class ConnectionStrings
                {
                    public static int CONNECTION_STRING = DatabaseConfiguration.ConnectionStrings.Sql.DEFAULT;
                }

                public static class Version
                {
                    public static int VERSION = DatabaseConfiguration.Versions.MySql.V5726;
                }
            }

            public static class Institution
            {
                public const int Value = GeneralConfiguration.Institution.AKDEMIC;
            }

            public static class Theme
            {
                public const int Value = GeneralConfiguration.Institution.AKDEMIC;
            }

            public static class FileStorage
            {
                public const int STORAGE_MODE = GeneralConfiguration.FileStorage.Mode.BLOB_STORAGE_MODE;

                //public const string PATH = "/var/www/files";
                public const string PATH = "D:/akdemic";
            }
        }
                       
        public static class Institution
        {
            public const int AKDEMIC = 1;
            public const int UNAM = 2;
            public const int UNAMAD = 3;
            public const int UNICA = 4;
            public const int UNJBG = 5;
            public const int UNICA_POST = 7;
            public const int UNFV = 8;

            public static Dictionary<int, string> Abbreviations = new Dictionary<int, string>()
            {
                { AKDEMIC, "AKDEMIC" },
                { UNAM, "UNAM" },
                { UNAMAD, "UNAMAD" },
                { UNICA, "UNICA" },
                { UNJBG, "UNJBG" },
                { UNICA_POST, "UNICA" },
                { UNFV, "UNFV" },
            };

            public static Dictionary<int, string> Locations = new Dictionary<int, string>()
            {
                { AKDEMIC, "Lima" },
                { UNAM, "Moquegua" },
                { UNAMAD, "Madre de Dios" },
                { UNICA, "Ica" },
                { UNJBG, "Tacna" },
                { UNICA_POST, "Ica" },
                { UNFV, "Lima" },
            };

            public static Dictionary<int, string> Codes = new Dictionary<int, string>()
            {
                { AKDEMIC, "" },
                { UNAM, "035" },
                { UNAMAD, "029" },
                { UNICA, "009" },
                { UNJBG, "032" },
                { UNICA_POST, "009" },
            };

            public static Dictionary<int, string> Repositories = new Dictionary<int, string>()
            {
                { AKDEMIC, "" },
                { UNAM, "http://repositorio.unam.edu.pe/handle/UNAM/" },
                { UNAMAD, "http://repositorio.unamad.edu.pe/handle/UNAMAD/" },
                { UNICA, "http://repositorio.unica.edu.pe/handle/UNICA/" },
                { UNJBG, "http://repositorio.unjbg.edu.pe/handle/UNJBG/" },
                { UNICA_POST, "http://repositorio.unica.edu.pe/handle/UNICA/" },
            };

            public static Dictionary<int, string> Names = new Dictionary<int, string>()
            {
                { AKDEMIC, "AKDEMIC" },
                { UNAM, "Universidad Nacional de Moquegua" },
                { UNAMAD, "Universidad Nacional Amazónica de Madre de Dios" },
                { UNICA, "Universidad Nacional \"San Luis Gonzaga\" de Ica" },
                { UNJBG, "Universidad Nacional Jorge Basadre Grohmann" },
                { UNICA_POST, "Universidad Nacional \"San Luis Gonzaga\" de Ica" },
                { UNFV, "Universidad Nacional Federico Villarreal" },
            };

            public static Dictionary<int, string> SupportEmail = new Dictionary<int, string>()
            {
                { AKDEMIC, "enchufatest@gmail.com" },
                { UNAM, "enchufatest@gmail.com" },
                { UNAMAD, "enchufatest@gmail.com" },
                { UNICA, "enchufatest@gmail.com" },
                { UNJBG, "enchufatest@gmail.com" },
                { UNICA_POST, "enchufatest@gmail.com" },
                { UNFV, "enchufatest@gmail.com" },
            };

            public static Dictionary<int, string> SupportEmailName = new Dictionary<int, string>()
            {
                { AKDEMIC, "enchufatest" },
                { UNAM, "enchufatest" },
                { UNAMAD, "enchufatest" },
                { UNICA, "enchufatest" },
                { UNJBG, "enchufatest" },
                { UNICA_POST, "enchufatest" },
                { UNFV, "enchufatest" },
            };

            public static Dictionary<int, string> SupportEmailPassword = new Dictionary<int, string>()
            {
                { AKDEMIC, "EnchufateWindow" },
                { UNAM, "EnchufateWindow" },
                { UNAMAD, "EnchufateWindow" },
                { UNICA, "EnchufateWindow" },
                { UNJBG, "EnchufateWindow" },
                { UNICA_POST, "EnchufateWindow" },
                { UNFV, "EnchufateWindow" },
            };
        }

        public static class FileStorage
        {
            public class Mode
            {
                public const int BLOB_STORAGE_MODE = 1;
                public const int SERVER_STORAGE_MODE = 2;
            }

            public class SystemFolder
            {
                public const string INTRANET = "intranet";
                public const string ENROLLMENT = "enrollment";
                public const string SCALE = "scale";
                public const string DOCUMENTARY_PROCEDURE = "documentaryprocedure";
                public const string INDICATORS = "indicators";
                public const string INVESTIGATION = "investigation";
                public const string ADMISSION = "admission";
                public const string EVALUATION = "evaluation";
                public const string ACADEMIC_EXCHANGE = "academicexchange";
                public const string INTEREST_GROUP = "interestgroup";
                public const string TEACHING_MANAGEMENT = "teachingmanagement";
                public const string LAURASSIA = "laurassia";
                public const string TRANSPARENCY_PORTAL = "transparencyportal";
                public const string LANGUAGE_CENTER = "languagecenter";
                public const string KARDEX = "kardex";
                public const string RESERVATIONS = "reservations";
                public const string JOBEXCHANGE = "jobexchange";
                public const string TUTORING = "tutoring";
                public const string PREUNIVERSITARY = "preuniversitary";
                public const string RESOLUTIVE_ACTS = "resolutiveacts";
                public const string SISCO = "sisco";
                public const string DEGREE = "degree";
                public const string ECONOMICMANAGEMENT = "economicmanagement";
                public const string COMPUTERMANAGEMENT = "computermanagement";
            }
        }

        public static class Language
        {
            public const string English = "en-US";
            public const string Spanish = "es-PE";
        }

        public static class Theme
        {
            public const int Default = 0;
            public const int Akdemic = 1;
            public const int UNAM = 2;
            public const int UNAMAD = 3;
            public const int UNICA = 4;
            public const int UNJBG = 5;
            public const int UNICA_POST = 7;
            public const int UNFV = 8;

            public static Dictionary<int, string> Values = new Dictionary<int, string>()
            {
                { Default, "" },
                { Akdemic, "akdemic" },
                { UNAM, "unam" },
                { UNAMAD, "unamad" },
                { UNICA, "unica" },
                { UNJBG, "unjbg" },
                { UNICA_POST, "unica_post"},
                { UNFV, "unfv"},
            };
        }

        public static class TimeZoneInfo
        {
            public const bool DISABLE_DAYLIGHT_SAVING_TIME = true;
            public const int GMT = -5;
        }

        public const string LINUX_TIMEZONE_ID = "America/Bogota";
        public const string OSX_TIMEZONE_ID = "America/Cayman";
        public const string WINDOWS_TIMEZONE_ID = "SA Pacific Standard Time";
        public const string PASSWORDQUIBUK = "@ApiQuibuk";

        public static class Seeder
        {
            public const bool Enabled = false;
            public const string IdentityRole = "ApplicationRoleSeed";
            public const string UserRole = "UserRoleSeed";
            public static readonly List<string> Priorities = new List<string>()
            {
                "ApplicationRoleSeed",              "UserRoleSeed",                     "DependencySeed",               "UserDependencySeed",                   "CountrySeed",                      "DepartmentSeed",                   "ProvinceSeed",
                "DistrictSeed",                     "ClassifierSeed",                   "SimulationSubjectSeed",        "SimulationSubjectAreaSeed",            "SimulationQuestionSeed",           "SimulationAnswerSeed",             "PsychologyCategorySeed",
                "PsychologyTestQuestionSeed",       "AnnouncementSeed",                 "RolAnnouncementSeed",          "EventTypeSeed",                        "EventSeed",                        /*"SurveySeed",                       "SurveyUserSeed",
                "QuestionSeed",                     "AnswerSeed",                       "AnswerByUserSeed",*/             "AdmissionSubjectSeed",                 "AdmissionSubjectAreaSeed",       "AdmissionQuestionSeed",            "AdmissionAnswerSeed",
                "FacultySeed",                      "CurriculumAreaSeed",               "FacultyCurriculumAreaSeed",    "AcademicDepartmentSeed",               "DeanSeed",                         "CareerSeed",
                "SimulationExamRegistrationSeed",   "CourseComponentSeed",              "AccountingPlanSeed",           "CurrentAccountSeed",
                "TermSeed",                         "AcademicCalendarDateSeed",         "CurriculumSeed",               "NewSeed",                              "ManualSeed",                       "FrequentQuestionSeed",             "PsychologicalDiagnosticSeed",
                "ConsultTypeSeed",                  "ForumSeed",                        /*"BankSeed",*/                 "UITSeed",                              "RecordSubjectTypeSeed",            /*"PaymentMethodSeed",*/            "ConfigurationSeed",
                "DocumentaryRecordTypeSeed",        "SettlementSeed",                   "IndicatorsSeed",               "TransparencyPortalGeneralSeed",        "DocumentTypeSeed",                 "ConceptSeed",                      "ExternalUserSeed",
                "WorkerCapPositionSeed",            "WorkerLaborRegimeSeed",            "WorkerLaborCategorySeed",      "WorkerLaborConditionSeed",             "WorkerPositionClassificationSeed", "AdmissionTypeSeed",                "AdmissionTypeDescountSeed",
                "ApplicationTermSeed",              "CourseTypeSeed",                   "AreaSeed",                     "AcademicYearSeed",                     "AdmissionExamSeed",                "AdmissionExamAdmissionTypeSeed",   "AdmissionExamSubjectAreaSeed",
                "StudentSeed",                      "EnrollmentTurnSeed",               "StudentFamilySeed",            "SpecialtySeed",                        "DoctorSeed",                       "MedicalAppointmentSeed",           "TeacherDedicationSeed",
                "TeacherSeed",                      "CourseSeed",                       "AcademicYearCourseSeed",       "AcademicYearCoursePreRequisiteSeed",   "ElectiveCourseSeed",               "CourseSyllabusSeed",               "CourseUnitSeed",
                "UnitActivitySeed",                 "UnitResourceSeed",                 "CourseTermSeed",               "GroupSeed",                            "SectionSeed",                      "StudentSectionSeed",               "AcademicHistorySeed",
                "TeacherSectionSeed",               "AcademicSummarySeed",              "CampusSeed",                   "BuildingSeed",                         "ClassroomTypeSeed",                "ClassroomSeed",                    "ClassScheduleSeed",
                "ClassSeed",                        "EvaluationSeed",                   "GradeSeed",                    "GradeCorrectionSeed",                  "PostulantSeed",                    "AdmissionResultSeed",              "SimulationExamSeed",
                "SimulationExamSubjectAreaSeed",    "ClassStudentSeed",                 "ClassRescheduleSeed",          "TutorialSeed",                         "TeacherScheduleSeed",              "TutorialStudentSeed",              "ScaleSectionSeed",
                "ScaleResolutionTypeSeed",          "ScaleSectionResolutionTypeSeed",   "InternalProcedureSeed",        "InternalProcedureFileSeed",            "InternalProcedureReferenceSeed",   "ExternalProcedureSeed",            "UserExternalProcedureSeed",
                "UserInternalProcedureSeed",        "ProcedureCategorySeed",            "ProcedureSubcategorySeed",     "ProcedureSeed",                        "ProcedureResolutionSeed",          "ProcedureRoleSeed",                "ProcedureRequirementSeed",
                "ProcedureDependencySeed",          "UserProcedureSeed",                "WorkerManagementPositionSeed", /*"EnrollmentReservationSeed",*/            "LanguageCourseSeed",               "LanguageLevelSeed",                "LanguageSectionSeed",
                "LanguageQualificationSeed",        "LanguageSectionScheduleSeed",
                // ECONOMIC MANAGEMENT
                "SupplierSeed",                     "OrderSeed",                        /*"RequirementSeed",              "RequirementSupplierSeed",*/
                // PAYROLL
                "BankSeed",                         "PaymentMethodSeed",                "PayrollTypeSeed",              "PayrollClassSeed",                     "WorkAreaSeed",                     "WorkerOcupationSeed",              "WorkerSeed",
                "WorkerHistorySeed",
                // TUTORING
                "TutorSeed",                        "TutoringStudentSeed",              "TutoringCoordinatorSeed",      "TutorTutoringStudentSeed",             "TutoringAnnouncementSeed",         "SupportOfficeSeed",                "SupportOfficeUserSeed",
                // JOB EXCHANGE            
                "SectorSeed",
                 //"CompanySeed",
                "AbilitySeed",                  "LanguageSeed",                         /*"JobOfferSeed",*/
                // PREUNIVERSITARY
                "PreuniversitaryTermSeed",          "PreuniversitaryCourseSeed",        "PreuniversitaryGroupSeed",     "PreuniversitaryScheduleSeed",          "PreuniversitaryUserGroupSeed",
                // ?
                "SupplierSeed",                     /*"RequirementSeed"*/
                //RESOLUTIVE ACTS
                "SorterSeed",                       "ResolutionCategorySeed"
            };
        }

        public static class CivilStatus
        {
            public const byte SINGLE = 0;
            public const byte MARRIED = 1;
            public const byte WIDOWER = 2;
            public const byte COHABITED = 3;
            public const byte DIVORCED = 4;
            public const byte SEPARATED = 5;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { SINGLE, "Soltero(a)" },
                { MARRIED, "Casado(a)" },
                { WIDOWER, "Viudo(a)" },
                { COHABITED, "Conviviente(a)" },
                { DIVORCED, "Divorciado(a)" },
                { SEPARATED, "Separado(a)" }
            };
        }

        public static class Containers
        {
            public const string ACADEMICHISTORYDOCUMENT = "academichistorydocument";
            public const string ADMISSIONTYPE = "admissiontypepdf";
            public const string ANNOUNCEMENT_FILES = "announcementpicture";
            public const string DEPENDENCY_SIGNATURE = "dependencysignature";
            public const string EVENT_PICTURE = "eventpicture";
            public const string FORUM_FILES = "forumpicture";
            public const string INTERNAL_PROCEDURE_DOCUMENT = "internalproceduredocument";
            public const string NONACTIVITIES_FILES = "nonactivitiesfile";
            public const string POSITIONS_FILES = "positionsfile";
            public const string RESOLUTIONS = "resolutions";
            public const string APROBERESOLUTIONS = "aproberesolutions";
            public const string STUDENT_ABSENCE_JUSTIFICATION_FILE = "studentabsencejustificationfile";
            public const string TEMARY = "temarypdf";
            public const string TEACHER_NONACTIVITY_HISTORIAL = "teachernonactivityhistorial";
            public const string USER_ABSENCE_JUSTIFICATION_FILE = "userabsencejustificationfile";
            public const string USER_PROFILE_PICTURE = "userprofilepicture";
            public const string USER_PROCEDURE_DERIVATION_DOCUMENT = "userprocedurederivationdocument";
            public const string USER_INTERNAL_PROCEDURE_DERIVATION_DOCUMENT = "userinternalprocedurederivationdocument";
            public const string USER_PERSONAL_DOCUMENTS = "scaleworkerpersonaldocuments";
            public const string USER_PROCEDURE_RECORD_DOCUMENTS = "userprocedurerecorddocuments";
            public const string USER_REQUIREMENT_DEGREE = "userrequirementdegree";
            public const string WORKER_RESOLUTIONS = "scaleworkerresolutions";
            public const string WORKER_RESOLUTIONS_ANNEXES = "scaleworkerresolutionsannexes";
            public const string WORKER_VACATIONS_LICENSES = "scaleworkervacationslicenses";
            public const string POSTULANT_REQUIREMENTS = "postulantrequirements";
            public const string POSTULANT_PHOTOS = "postulantphotos";
            public const string RESERVATION_ENVIRONMENT = "reservationenvironment";
            public const string INVESTIGATION = "investigation";
            public const string EVALUATION = "evaluation";
            public const string RESEARCH = "research";
            public const string INSTITUTIONAL_AGENDA = "agendainstitucional";
            public const string SUBACTIVITY = "subactivity";
            public const string CURRICULUM = "curriculumvitaes";
            public const string COMPANY = "companies";
            public const string COMPANY_IMAGES = "companiesimages";
            public const string AGREEMENT_JOB_EXCHANGE = "agreementdocuments";
            public const string FINANCIAL_EXECUTIONS = "financialexecutions";
            public const string INSTITUTIONAL_ACTIVITIES = "institutionalactivities";
            public const string PUBLIC_INFORMATION = "publicinformation";
            public const string RESEARCH_PROJECTS = "researchprojects";
            public const string SESSION_RECORD = "sessionrecord";
            public const string FINANCIAL_STATEMENT = "financialstatement";
            public const string RESOLUTIVE_ACTS = "resolutiveacts";
            public const string INSTITUTIONAL_INFORMATION = "institutionalinformation";
            public const string HELP_DESK_FILES = "helpdeskfiles";
            public const string ADMIN_ENROLLMENT = "adminenrollment";
            public const string TRANSPARENCY_PORTAL_REGULATIONS = "transparencyportalregulations";
            public const string ORDER_CHANGED_FILES = "orderchangedfiles";
            public const string ENROLLMENT_RESERVATION = "enrollmentreservation";
            public const string LIVING_COST = "livingcostpdf";
            public const string DIGITIZED_SIGNATURES = "digitizedsignatures";
            public const string JOB_OFFERS = "jobofferspdf";

            //Portal
            public const string COMPETITION_FILES = "competitionfiles";

            //sisco
            public const string NORM = "norm";
            public const string AGREEMENT_PUBLICATION_FILES = "agreementpublicationfiles";
            public const string INCUBATION_CALL_FILES = "incubationcallfiles";
            public const string TUTORING_PLANS = "tutoringplans";
            public const string TUTORING_PROBLEMS = "tutoringproblems";
            public const string INTEREST_GROUPS_FILES = "interestgroups";
            public const string SISCO = "sisco";

            //EnrollmentManagement
            public const string PURCHASE_ORDER_DOCUMENTS = "purchaseorderdocuments";

            //EconomicManagement
            public const string USER_REQUIREMENT_DOCUMENTS = "userrequirementdocuments";

            //AcademicExchange
            public const string ACADEMIC_EXCHANGE_FILES = "academicexchangefiles";
            public const string SCHOLARSHIPS_FILES = "scholarshipsfiles";
            public const string SCHOLARSHIP_GALLERY = "scholarshipgalleryfiles";

            //internshipRequests
            public const string INTERNSHIPREQUESTS_FILES = "internshiprequests";

            public const string PICTURE_GROUP = "picturegroup";
            //computermanagement
            public const string COMPUTER_CONDITION_FILES = "computerconditionfiles";
        }

        public static class Colors
        {
            public static class Badge
            {
                public static class State
                {
                    public const int NONE = 0;
                    public const int SUCCESS = 1;
                    public const int WARNING = 2;
                    public const int DANGER = 3;
                    public const int INFO = 4;
                    public const int PRIMARY = 5;
                    public const int SECONDARY = 6;
                    public const int BRAND = 7;
                    public const int ACCENT = 8;
                    public const int FOCUS = 9;
                    public const int METAL = 10;
                    public const int LIGHT = 11;

                    public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "NONE", NONE },
                    { "SUCCESS", SUCCESS },
                    { "WARNING", WARNING },
                    { "DANGER", DANGER },
                    { "INFO", INFO },
                    { "PRIMARY", PRIMARY },
                    { "SECONDARY", SECONDARY },
                    { "BRAND", BRAND },
                    { "ACCENT", ACCENT },
                    { "FOCUS", FOCUS },
                    { "METAL", METAL },
                    { "LIGHT", LIGHT }
                };

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NONE, "" },
                    { SUCCESS, "m-badge--success" },
                    { WARNING, "m-badge--warning" },
                    { DANGER, "m-badge--danger" },
                    { INFO, "m-badge--info" },
                    { PRIMARY, "m-badge--primary" },
                    { SECONDARY, "m-badge--secondary" },
                    { BRAND, "m-badge--brand" },
                    { ACCENT, "m-badge--accent" },
                    { FOCUS, "m-badge--focus" },
                    { METAL, "m-badge--metal" },
                    { LIGHT, "m-badge--light" }
                };
                }
            }
        }

        public static class Datatable
        {
            public static class ServerSide
            {
                public static class Default
                {
                    public const string ORDER_DIRECTION = "DESC";
                }

                public static class SentParameters
                {
                    public const string DRAW_COUNTER = "draw";
                    public const string PAGING_FIRST_RECORD = "start";
                    public const string RECORDS_PER_DRAW = "length";
                    public const string SEARCH_VALUE = "search[value]";
                    public const string SEARCH_REGEX = "search[regex]";
                    public const string ORDER_COLUMN = "order[0][column]";
                    public const string ORDER_DIRECTION = "order[0][dir]";
                }
            }
        }




        public static class Hubs
        {
            public static class Akdemic
            {
                public static class ClientProxy
                {
                    public static class Methods
                    {
                        public static class Chat
                        {
                            public const string NEWMESSAGE = "akdemic-chat-new-message";
                            public const string SUCCESSENDMESSAGE = "akdemic-chat-success-send-message";
                        }

                        public static class Notification
                        {
                            public const string RECIEVE = "RecieveNotification";
                        }
                    }

                    public const string METHOD = "akdemic-notification";
                    public const string URL = "/hubs/akdemic";
                }

                public static class Database
                {
                    public static class Table
                    {
                        public const string NOTIFICATION = "Generals.Notifications";
                        public const string USER_NOTIFICATION = "Generals.UserNotifications";
                    }
                }
            }
        }

        public static class Messages
        {
            public static class Error
            {
                public const string MESSAGE = "Ocurrió un problema al procesar su consulta";
                public const string TITLE = "Error";
            }

            public static class Info
            {
                public const string MESSAGE = "Mensaje informativo";
                public const string TITLE = "Info";
            }

            public static class Success
            {
                public const string MESSAGE = "Tarea realizada satisfactoriamente";
                public const string TITLE = "Éxito";
            }

            public static class Validation
            {
                public const string COMPARE = "El campo '{0}' no coincide con '{1}'";
                public const string EMAIL_ADDRESS = "El campo '{0}' no es un correo electrónico válido";
                public const string RANGE = "El campo '{0}' debe tener un valor entre {1}-{2}";
                public const string REGULAR_EXPRESSION = "El campo '{0}' no es válido";
                public const string REQUIRED = "El campo '{0}' es obligatorio";
                public const string STRING_LENGTH = "El campo '{0}' debe tener {1}-{2} caracteres";
                public const string NOT_VALID = "El campo '{0}' no es válido'";
                public const string FILE_EXTENSIONS = "El campo '{0}' solo acepta archivos con los formatos: {1}";
            }

            public static class ValidationLanguages
            {
                public const string COMPARE = "ValidationMessageCompare";
                public const string EMAIL_ADDRESS = "ValidationMessageEmail";
                public const string RANGE = "ValidationMessageRange";
                public const string REGULAR_EXPRESSION = "ValidationMessageRegularExpression";
                public const string REQUIRED = "ValidationMessageRequired";
                public const string STRING_LENGTH = "ValidationMessageStringLength";
                public const string URL = "ValidationMessageStringUrl";
                public const string STRING_UNIQUE_LENGTH = "ValidationMessageStringUniqueLength";
                public const string NOT_VALID = "ValidationMessageNotValid";
                public const string FILE_EXTENSIONS = "ValidationMessageFileExtensions";
                public const string POSITIVE_INTEGER = "ValidationMessagePositiveInteger";
                public const string POSITIVE_INTEGER_OR_ZERO = "ValidationMessagePositiveIntegerOrZero";
            }

            public static class Warning
            {
                public const string MESSAGE = "Mensaje de advertencia";
                public const string TITLE = "Advertencia";
            }
        }

        public static class Months
        {
            public const int JANUARY = 1;
            public const int FEBRAURY = 2;
            public const int MARCH = 3;
            public const int APRIL = 4;
            public const int MAY = 5;
            public const int JUNE = 6;
            public const int JULY = 7;
            public const int AUGUST = 8;
            public const int SEPTEMBER = 9;
            public const int OCTOBER = 10;
            public const int NOVEMBER = 11;
            public const int DECEMBER = 12;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { JANUARY, "Enero" },
                { FEBRAURY, "Febrero" },
                { MARCH, "Marzo" },
                { APRIL, "Abril" },
                { MAY, "Mayo" },
                { JUNE, "Junio" },
                { JULY, "Julio" },
                { AUGUST, "Agosto" },
                { SEPTEMBER, "Setiembre" },
                { OCTOBER, "Octubre" },
                { NOVEMBER, "Noviembre" },
                { DECEMBER, "Diciembre" }
            };
        }

        public static class Notifications
        {
            public const int DEFAULT_PAGE_SIZE = 10;

            public static class Colors
            {
                public const int NONE = 0;
                public const int SUCCESS = 1;
                public const int WARNING = 2;
                public const int DANGER = 3;
                public const int INFO = 4;
                public const int PRIMARY = 5;
                public const int SECONDARY = 6;
                public const int BRAND = 7;
                public const int ACCENT = 8;
                public const int FOCUS = 9;
                public const int METAL = 10;

                public static Dictionary<int, string> LABELS = new Dictionary<int, string>()
                {
                    { NONE, "" },
                    { SUCCESS, "m-badge--success" },
                    { WARNING, "m-badge--warning" },
                    { DANGER, "m-badge--danger" },
                    { INFO, "m-badge--info" },
                    { PRIMARY, "m-badge--primary" },
                    { SECONDARY, "m-badge--secondary" },
                    { BRAND, "m-badge--brand" },
                    { ACCENT, "m-badge--accent" },
                    { FOCUS, "m-badge--focus" },
                    { METAL, "m-badge--metal" },
                };
            }
        }




        public static class EntityEntries
        {
            public static class PropertyName
            {
                public const string CODE_NUMBER = "CodeNumber";
                public const string CODE_TEXT = "CodeText";
                public const string CREATED_AT = "CreatedAt";
                public const string DELETED_AT = "DeletedAt";
                public const string UPDATED_AT = "UpdatedAt";

                public const string CREATED_BY = "CreatedBy";
                public const string DELETED_BY = "DeletedBy";
                public const string UPDATED_BY = "UpdatedBy";
            }
        }

        public static class EntityFramework
        {
            public const int CHANGES_LIMIT = 100;
            public const int RECORD_LIMIT = 1000;
        }

        public static class EntityModels
        {
            public static class Dbo
            {
                public const string ASPNETUSERS = "dbo.AspNetUsers";
            }

            public static class Generals
            {
                public const string STUDENTS = "Generals.Students";
            }

            public static class Intranet
            {
                public const string ACADEMIC_HISTORY = "Intranet.AcademicHistories";
                public const string ACADEMIC_SUMMARY = "Intranet.AcademicSummaries";
                public const string SURVEY_USER = "Intranet.SurveyUsers";

                public static class AcademicSummaries
                {
                    public const string MERIT_ORDER = "MeritOrder";
                }
            }
            public static class TeachingManagement
            {
                public const string TEACHER_SURVEY = "TeachingManagement.TeacherSurveys";
            }
        }

        public static class Formats
        {
            public const string DATE = "dd/MM/yyyy";
            public const string DURATION = "{0}h {1}m";
            public const string TIME = "h:mm tt";
            public const string DATETIME = "dd/MM/yyyy h:mm tt";
        }

        public static class InternalProcedures
        {
            public static class Priority
            {
                public const int LOW = 1;
                public const int MEDIUM = 2;
                public const int HIGH = 3;

                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "LOW", LOW },
                    { "MEDIUM", MEDIUM },
                    { "HIGH", HIGH }
                };

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { LOW, "Normal" },
                    { MEDIUM, "Medio" },
                    { HIGH, "Alto" }
                };
            }

            public static class StaticType
            {
                public const int COMPLAINTS_BOOK = 1;

                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "COMPLAINTS_BOOK", COMPLAINTS_BOOK }
                };

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { COMPLAINTS_BOOK, "Libro de Reclamaciones" },
                };
            }
        }
    }
}
