using Amazon.Runtime.Internal.Transform;
using System.Collections.Generic;

namespace AKDEMIC.CORE.Helpers
{
    public partial class ConstantHelpers
    {
        public static class EVENT
        {
            public const byte INTRANET = 1;
            public const byte INSTITUTIONALWELFARE = 2;
            public const byte JOBEXCHANGE = 3;

            public static Dictionary<int, string> SYSTEM = new Dictionary<int, string>()
                {
                    { INTRANET, "Intranet" },
                    { INSTITUTIONALWELFARE, "Bienestar" },
                    { JOBEXCHANGE, "Bolsa de Trabajo" }
                };
        }

        #region ACADEMIC_EXCHANGE
        public static class ACADEMIC_EXCHANGE
        {
            public static class POSTULATION_STATE
            {
                public const byte PENDING = 0;
                public const byte OBSERVED = 1;
                public const byte APPROVED = 2;
                public const byte DENIED = 3;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { PENDING, "Pendiente" },
                    { OBSERVED, "Observado" },
                    { APPROVED, "Aprobado" },
                    { DENIED, "Rechazado" },
                };
            }

            public static class AE_GENERALFILE_STATUS
            {
                public const byte ACTIVE = 1;
                public const byte EXPIRED = 2;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { ACTIVE, "Activo" },
                    { EXPIRED, "Caducado" },
                };
            }

        }
        #endregion

        #region CONTINUING_EDUCATION
        public static class CONTINUING_EDUCATION
        {
            public static class COURSES_ORGANIZER_ENTITY
            {
                public const int FACULTY = 0;
                public const int CAREER = 1;
                public const int ACADEMICDEPARTMENT = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { FACULTY, "Facultad" },
                    { CAREER, "Escuela Profesional" },
                    { ACADEMICDEPARTMENT, "Departamento Académico" },
                };
            }

        }
        #endregion

        #region TEACHING MANAGEMENT

        public static class TEACHING_LOAD
        {
            public static class CATEGORY
            {
                public const int ACTIVITIES = 1;
                public const int COMPLEMENTARY = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { ACTIVITIES, "Actividades" },
                    { COMPLEMENTARY, "Complementario" },
                };
            }
        }

        public static class WORKING_DAY
        {
            public static class TEACHER_ASSISTANCE_FORMAT
            {
                public const byte GENERAL = 1;
                public const byte UNJBG_FORMAT = 2;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { GENERAL, "Formato 1"},
                    { UNJBG_FORMAT, "Formato 2"}
                };
            }
            public static class STATUS
            {
                public const byte NORMAL = 1;
                public const byte LATE = 2;
                public const byte ABSENT = 3;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { NORMAL, "Asistió" },
                    { LATE, "Tardanza" },
                    { ABSENT, "Ausente" },
                };
            }
        }

        public static class SYLLABUS_REQUEST
        {
            public static class TYPE
            {
                public const byte DIGITAL = 1;
                public const byte FILE = 2;
                public const byte MIXED = 3;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { DIGITAL, "Digital" },
                    { FILE, "Archivo" },
                    { MIXED, "Mixto (Digital o Archivo)" },
                };
            }
        }

        public static class SYLLABUS_TEACHER
        {
            public static class STATUS
            {
                public const byte IN_PROCESS = 1;
                public const byte PRESENTED = 2;

                public const byte OBSERVED = 3;
                public const byte IN_VALIDATION = 4;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { IN_PROCESS, "En Proceso" },
                    { OBSERVED, "Con Observaciones" },
                    { IN_VALIDATION, "En validación" },
                    { PRESENTED, "Presentado" },
                };

                public static Dictionary<byte,string> GetValuesByConfiguration(bool enabledSyllabusValidation)
                {
                    if (enabledSyllabusValidation)
                    {
                        return new Dictionary<byte, string>()
                        {
                            { IN_PROCESS, "En Proceso" },
                            { OBSERVED, "Con Observaciones" },
                            { IN_VALIDATION, "En validación" },
                            { PRESENTED, "Presentado" }
                        };
                    }
                    else
                    {
                        return new Dictionary<byte, string>()
                        {
                            { IN_PROCESS, "En Proceso" },
                            { PRESENTED, "Presentado" }
                        };
                    }
                }
            }
        }

        public class PERFORMANCE_EVALUATION
        {
            public static class RATING_SCALE
            {
                public static string GET_NAME(int max, byte value)
                {
                    switch (max)
                    {
                        case 4:
                            return ConstantHelpers.PERFORMANCE_EVALUATION.RATING_SCALE.OTHER[value];
                        case 5:
                            return ConstantHelpers.PERFORMANCE_EVALUATION.RATING_SCALE.LIKERT[value];
                        default:
                            return "-";
                    }
                }

                public static Dictionary<byte, string> LIKERT = new Dictionary<byte, string>()
                {
                    { 1, "Nunca" },
                    { 2, "Casi nunca" },
                    { 3, "A veces" },
                    { 4, "Casi siempre" },
                    { 5, "Siempre" },
                };

                public static Dictionary<byte, string> OTHER = new Dictionary<byte, string>()
                {
                    { 1, "Muy insatisfactorio" },
                    { 2, "Insatisfactorio" },
                    { 3, "Satisfactorio" },
                    { 4, "Muy satisfactorio" },
                };
            }

            public static class TARGET
            {
                public static byte ALL = 1;
                public static byte STUDENTS = 2;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { ALL, "Todos" },
                    { STUDENTS, "Solo Estudiantes" },
                };
            }
        }

        public class TERM_INFORM
        {
            public static class TYPE
            {
                public const byte HALFTERM = 1;
                public const byte ENDTERM = 2;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { HALFTERM, "Medio ciclo" },
                    { ENDTERM, "Fin de ciclo" },
                };
            }

            public static class REQUEST_TYPE
            {
                public const byte BYTEACHER = 1;
                public const byte BYSECTION = 2;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { BYTEACHER, "Por profesor" },
                    { BYSECTION, "Por sección" },
                };
            }
        }

        public static class NON_TEACHING_LOAD_DELIVERABLE
        {
            public static class STATUS
            {
                public const byte PENDING = 1;
                public const byte APPROVED = 2;
                public const byte REJECTED = 3;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { PENDING, "Pendiente" },
                    { APPROVED, "Aprobado" },
                    { REJECTED, "Rechazado" }
                };
            }
        }
        #endregion

        #region ENROLLMENT
        public class QUESTIONNAIRE
        {
            public const int TEXT_QUESTION = 1;
            public const int MULTIPLE_SELECTION_QUESTION = 2;
            public const int UNIQUE_SELECTION_QUESTION = 3;
            //public const byte ESCALE_SELECTION_QUESTION = 4;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                {TEXT_QUESTION,"Pregunta de Texto" },
                {MULTIPLE_SELECTION_QUESTION,"Pregunta de selección múltiple" },
                {UNIQUE_SELECTION_QUESTION,"Pregunta de selección única" },
                //{ESCALE_SELECTION_QUESTION,"Pregunta de selección de escala" }
            };
        }

        public class COMPETENCIE
        {
            public const byte GENERAL = 1;
            public const byte SPECIFIC = 2;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                {GENERAL,"General" },
                {SPECIFIC,"Específica" },
            };

        }

        #endregion

        #region ADMISSION
        public static class PROOF_OF_INCOME_STATUS
        {
            public const byte NOT_GENERATED = 0;
            public const byte GENERATED = 1;
            public const byte NON_PICKED_UP = 2;
            public const byte PICKED_UP = 3;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { NOT_GENERATED, "No Generada" },
                { GENERATED, "Generada" },
                { NON_PICKED_UP, "No se presentó" },
                { PICKED_UP, "Entregada" }
            };
        }
        public static class ADMISSION_MODE
        {
            public const byte NONE = 0;
            public const byte ORDINARY = 1;
            public const byte EXTRAORDINARY = 2;
            public const byte AGREEMENT = 3;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { NONE, "No especificado" },
                { ORDINARY, "Ordinaria" },
                { EXTRAORDINARY, "Extraordinaria" }
            };
        }
        public static class POSTULANT_CARD
        {
            public static class SECTION
            {
                public const int HIGHER_EDUCATION = 1;
                public const int ADDITIONAL_INFORMATION = 2;
                public const int QUALIFIED_ATHLETES = 3;
                public const int EXTERNAL_TRANSFER = 4;
                public const int GRADUATES = 5;
                public const int INTERNAL_TRANSFER = 6;
                public const int FFAA_PNP_GRADUATES = 7;
                public const int UNIVERSITY_PREPARATION = 8;
                public const int ORIGINAL_LANGUAGE = 9;
                public const int RACIAL_IDENTITY = 10;
                public const int EXAM_CAMPUS = 11;
                public const int PREUNIVERSITY_CENTER = 12;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { HIGHER_EDUCATION, "Estudios Superiores" },
                    { ADDITIONAL_INFORMATION, "Información Adicional" },
                    { QUALIFIED_ATHLETES, "Deportistas Calificados" },
                    { EXTERNAL_TRANSFER, "Traslado Externo" },
                    { GRADUATES, "Graduados o Titulados" },
                    { INTERNAL_TRANSFER, "Taslado interno" },
                    { FFAA_PNP_GRADUATES, "Graduados de las FF.AA y P.N.P" },
                    { UNIVERSITY_PREPARATION, "Preparación Universitaria" },
                    { ORIGINAL_LANGUAGE, "Lenguas Originarias" },
                    { RACIAL_IDENTITY, "Autoidentificación Étnica" },
                    { EXAM_CAMPUS, "Sede para Examen" },
                    { PREUNIVERSITY_CENTER, "Centro Preuniversitario" },
                };
            }
        }
        public static class CLIENT_FOR_PAYMENT
        {
            public const string DOCUMENT = "99999999";
            public const string BUSSINES_NAME = "Cliente Admisión";
            public const string PATSURNAME = "Admisión";
            public const string MATSURNAME = "Admisión";
            public const string NAME = "Cliente";
            public const string EMAIL = "Cliente_Admision@enchufate.pe";
            public const string PHONE = "99999999";
            public static string DOCUMENT_TYPE = ConstantHelpers.DOCUMENT_TYPES.VALUES[ConstantHelpers.DOCUMENT_TYPES.DNI];
            public const string CONTACT_NAME = "Cliente Admisión";
        }
        public static class PLACE_OF_PAYMENT
        {
            public const int BANK = 1;
            public const int CASH = 2;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { BANK, "Banco" },
                    { CASH, "Caja" },
            };
        }

        public static class ADMISSION_FILE
        {
            public const byte ORIGINAL = 1;
            public const byte FORMATTED_FILE = 2;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { ORIGINAL, "Archivo Original" },
                { FORMATTED_FILE, "Archivo Formateado" },
            };
        }

        public static class ADMISSION_APPLICATION_TERM_SURVEY
        {
            public static class APPLICATION_TERM_QUESTION_TYPE
            {
                public const byte TEXT_QUESTION = 1;
                public const byte SELECT_QUESTION = 2;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { TEXT_QUESTION, "Pregunta de Texto" },
                    { SELECT_QUESTION, "Pregunta de Selección Multiple" },
                };
            }
        }

        #endregion
        
        #region ENROLLMENT
        public static class MARITAL_STATUS
        {
            public const int SINGLE = 1;
            public const int MARRIED = 2;
            public const int DIVORCED = 3;
            public const int WIDOWED = 4;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { SINGLE, "Soltero" },
                { MARRIED, "Casado" },
                { DIVORCED, "Divorciado" },
                { WIDOWED, "Viudo" }
            };
        }

        public static class REPRESENTATIVE_TYPE
        {
            public const int NONE = 1;
            public const int MOTHER = 2;
            public const int FATHER = 3;
            public const int OTHER = 4;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { NONE, "Ninguno" },
                { MOTHER, "Madre" },
                { FATHER, "Padre" },
                { OTHER, "Otro" }
            };
        }
        public static class SECONDARY_EDUCATION_TYPE
        {
            public const int PUBLIC = 1;
            public const int PRIVATE = 2;
            public const int PAROCHIAL = 3;
            public const int OTHER = 4;
            public const int FOREIGN = 5;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                {PUBLIC, "Público"},
                {PRIVATE, "Privado"},
                {PAROCHIAL, "Parroquial"},
                {OTHER, "Otros"},
                {FOREIGN, "Extranjero"}
            };
        }
        public class FINISHED_SECONDARY_EDUCATION
        {
            public const int YES = 1;
            public const int NOT_YET = 2;
            public const int ANOTHER_CASE = 3;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                {YES, "Sí"},
                {NOT_YET, "No, cursando 5º año"},
                {ANOTHER_CASE, "Otros Casos"},
            };
        }
        public class BROADCAST_MEDIUM
        {
            public const int INTERNET = 1;
            public const int SOCIAL_NETWORKS = 2;
            public const int FAMILY_AND_FRIENDS = 3;
            public const int OTHER = 4;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                {INTERNET, "Internet"},
                {SOCIAL_NETWORKS, "Redes Sociales"},
                {FAMILY_AND_FRIENDS, "Familia y/o amigos"},
                {OTHER, "Otro"}
            };
        }
        /* ENROLLMENT */
        public static class EnrollmentConcept
        {
            public static class Type
            {
                public const byte ADMISSION_ADDITIONAL_CONCEPT = 1;
                public const byte REGULAR_ADDITIONAL_CONCEPT = 2;

                public const byte ADMISSION_ENROLLMENT_CONCEPT = 3;
                public const byte REGULAR_ENROLLMENT_CONCEPT = 4;
                public const byte EXTRA_ENROLLMENT_CONCEPT = 5;
                public const byte EXONERATED_COURSE_CONCEPT = 6;
                public const byte EXTEMPORANEOUS_ENROLLMENT_CONCEPT = 7;
                public const byte LESS_THAN_TWELVE_CREDITS_ENROLLMENT = 8;
                public const byte UNBEATEN_ENROLLMENT_CONCEPT = 9;

                public const byte ADMISSION_ADDITIONAL_CONCEPT_REPLACEMENT = 10;
                public const byte REGULAR_ADDITIONAL_CONCEPT_REPLACEMENT = 11;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { ADMISSION_ADDITIONAL_CONCEPT, "Ingresante" },
                        { REGULAR_ADDITIONAL_CONCEPT, "Regular"},
                        { ADMISSION_ENROLLMENT_CONCEPT, "Ingresante"},
                        { REGULAR_ENROLLMENT_CONCEPT, "Regular"},
                        { EXTRA_ENROLLMENT_CONCEPT, "Permanencia"},
                        { EXONERATED_COURSE_CONCEPT, "Curso Exonerado"},
                        { EXTEMPORANEOUS_ENROLLMENT_CONCEPT, "Extemporáneo"},
                        { LESS_THAN_TWELVE_CREDITS_ENROLLMENT, "12 Créditos"},
                        { UNBEATEN_ENROLLMENT_CONCEPT, "Invicto"},
                        { ADMISSION_ADDITIONAL_CONCEPT_REPLACEMENT, "Ingresante"},
                        { REGULAR_ADDITIONAL_CONCEPT_REPLACEMENT, "Regular"},
                    };
            }
        }
        public static class COURSES
        {
            public const int ATTENDANCE_MIN_PERCENTAGE = 70;
        }

        public static class ENROLLMENT_SURVEY
        {
            public static class INTERNET_CONNECTION_TYPE
            {
                public const byte DATAPLAN = 1;
                public const byte ADSL = 2;
                public const byte HFC = 3;
                public const byte WIRELESS_INTERNET = 4;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { DATAPLAN, "Plan de datos desde el celular"},
                    { ADSL, "ADSL instalado en casa"},
                    { HFC, "HFC instalado en casa"},
                    { WIRELESS_INTERNET, "Internet inalámbrico"}
                };
            }
        }

        #endregion
        
        #region Biblioteca

        public static class QUIBUKAPIS
        {
            public const int Akdemic = ConstantHelpers.Institution.Akdemic;
            public const int UNAM = ConstantHelpers.Institution.UNAM;
            public const int UNAMAD = ConstantHelpers.Institution.UNAMAD;
            public const int UNICA = ConstantHelpers.Institution.UNICA;
            public const int UNJBG = ConstantHelpers.Institution.UNJBG;
            public const int UNICA_POST = ConstantHelpers.Institution.UNICA_POST;
            public const int UNSCH = ConstantHelpers.Institution.UNSCH;
            public const int UNAB = ConstantHelpers.Institution.UNAB;
            public const int UNAH = ConstantHelpers.Institution.UNAH;
            public const int UNCP = ConstantHelpers.Institution.UNCP;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { Akdemic, "" },
                { UNAM, "" },
                { UNAMAD, "" },
                { UNICA, "" },
                { UNJBG, "" },
                { UNSCH, "" },
                { UNAB, "" },
                { UNAH, "" },
                { UNICA_POST, ""},
                { UNCP, "" },
            };
        }

        #endregion
        
        #region Scale
        public static class SCALEPDFSECTIONS
        {
            public const int PERSONALINFO = 1;
            public const int STUDY = 2;
            public const int WORKEXPERIENCE = 3;
            public const int WORKSITUATION = 4;
            public const int CONTRACT = 5;
            public const int NOMINATION = 6;
            public const int PROMOTION = 7;
            public const int DISPLACEMENT = 8;
            public const int CHARGE = 9;
            public const int MERITDEMERIT = 10;
            public const int PAYEDLICENSE = 11;
            public const int LICENSE = 12;
            public const int COMMISSION = 13;
            public const int ADMBORROWING = 14;
            public const int FAMILYBONO = 15;
            public const int OTHERBONO = 16;
            public const int SCHOLARSHIP = 17;
            public const int CESSATION = 18;
            public const int INVESTIGATION = 19;
            public const int QUINQUENIUM = 20;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { PERSONALINFO, "Datos Personales" },
                { STUDY, "Estudios" },
                { WORKEXPERIENCE, "Trayectoria Laboral" },
                { WORKSITUATION, "Situación Laboral" },
                { CONTRACT, "Contratos" },
                { NOMINATION, "Nombramiento" },
                { PROMOTION , "Ascensos" },
                { DISPLACEMENT , "Desplazamiento" },
                { CHARGE , "Encargaturas" },
                { MERITDEMERIT , "Meritos y Demeritos" },
                { PAYEDLICENSE , "Licencia con goce de haber" },
                { LICENSE , "Licencia sin goce de haber" },
                { COMMISSION , "Comisiones" },
                { ADMBORROWING , "Prestamos Administrativos "},
                { FAMILYBONO , "Remuneración Familiar" },
                { OTHERBONO , "Otras Bonificaciones" },
                { SCHOLARSHIP, "Becas Y/O Capacitaciones" },
                { CESSATION, "Ceses"},
                { INVESTIGATION , "Investigaciones" },
                { QUINQUENIUM , "Quinquenios" }
            };
        };

        public static class RESOLUTION_SECTIONS
        {
            public const byte CONTRACTS = 1;
            public const byte PERFORMANCE_EVALUATION = 2;
            public const byte DISPLACEMENT = 3;
            public const byte VACATIONS = 4;
            public const byte MERIT = 5;
            public const byte PROFESSIONAL_EXPERIENCE = 6;
            public const byte BENEFITS = 7;
            public const byte OTHER_DOCUMENTATIONS = 8;
            public const byte INVESTIGATION = 9;
            public const byte DEMERIT = 10;
        }

        public static class SCALERESOLUTION_DOCUMENT_TYPE
        {
            public const byte RESOLUTION = 1;
            public const byte MEMORANDUM = 2;
            public const byte BALLOT = 3;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { RESOLUTION, "Resolución" },
                { MEMORANDUM, "Memorándum" },
                { BALLOT, "Papeleta" },
            };
        };

        public static class STUDIES_LEVEL
        {
            public const int DOCTORAL = 1;
            public const int MASTER = 2;
            public const int PROFESSIONAL = 3;
            public const int BACHELOR = 4;
            public const int TECHNICAL = 5;
            public const int ELEMENTARYSCHOOL = 6;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { DOCTORAL, "Doctorado" },
                { MASTER, "Maestría" },
                { PROFESSIONAL, "Profesional" },
                { BACHELOR, "Bachiller" },
                { TECHNICAL, "Técnico" },
                { ELEMENTARYSCHOOL, "Primaria-Secundaria" }
            };
        };

        public static class WORKERSTATUS
        {
            public const int ACTIVE = 1;
            public const int UNEMPLOYED = 2;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { ACTIVE, "Activo" },
                { UNEMPLOYED, "Cesante" },
            };
        }

        public static class RESIDENCETYPE
        {
            public const int HOUSE = 1;
            public const int APARTMENT = 2;
            public const int CONDOMINIUM = 3;


            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { HOUSE, "Casa" },
                { APARTMENT, "Departamento" },
                { CONDOMINIUM, "Condominio" }
            };
        }

        public static class DISCAPACITYCARNETTYPE
        {
            public const int ESSALUD = 1;
            public const int CONADIS = 2;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { ESSALUD, "EsSalud" },
                { CONADIS, "Conadis" }
            };
        }

        public static class WORKER_TRAINING_TYPE
        {
            public const byte SEMINAR = 1;
            public const byte CONFERENCE = 2;
            public const byte COURSE = 3;
            public const byte OTHER = 4;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { SEMINAR, "Seminario"},
                { CONFERENCE, "Conferencia"},
                { COURSE, "Curso/Taller"},
                { OTHER, "Otros"},
            };
        }

        public static class STUDY_TYPES
        {
            public const byte ELEMENTARY_SCHOOL = 1;
            public const byte HIGH_SCHOOL = 2;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { ELEMENTARY_SCHOOL, "Primaria" },
                { HIGH_SCHOOL, "Secundaria" }
            };
        };

        public static class INSTITUTION_TYPES
        {
            public const byte UNKNOW = 0;
            public const byte PUBLIC = 1;
            public const byte PRIVATE = 2;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { UNKNOW, "No Especifica"},
                { PUBLIC, "Público" },
                { PRIVATE, "Privado" }
            };
        };

        public static class WORKERFAMILYINFORMATION_RELATIONSHIP
        {
            public const int FATHER = 1;
            public const int MOTHER = 2;
            public const int CHILD = 3;
            public const int REPRESENTATIVE = 4;
            public const int OTHER = 5;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { FATHER, "Padre" },
                { MOTHER, "Madre" },
                { CHILD, "Hijo" },
                { REPRESENTATIVE, "Apoderado" },
                { OTHER, "Otros" }
            };
        }

        #endregion

        #region SISCO

        public class Norms
        {
            public class Status
            {
                public const byte REPEALED = 1;
                public const byte VALID = 2;
                //public const byte MANUAL = 3;

                public static Dictionary<int, string> STATUS = new Dictionary<int, string>
                {
                    { REPEALED, "Derogada" },
                    { VALID, "Vigente" },
                    //{ MANUAL, "Manual" }
                };
            }

            public class Type
            {
                public const byte LAW = 1;
                public const byte RESOLUTION = 2;
                public const byte CIRCULAR = 3;
                public const byte MULTIPLE_OFFICE = 4;
                public const byte CIRCULAR_CARD = 5;
                public const byte CIRCULAR_OFFICE = 6;

                public static Dictionary<int, string> TYPE = new Dictionary<int, string>
                {
                    { LAW, "Ley" },
                    { RESOLUTION, "Resolución" },
                    { CIRCULAR, "Circular" },
                    { MULTIPLE_OFFICE, "Oficio multiple" },
                    { CIRCULAR_CARD, "Carta Circular" },
                    { CIRCULAR_OFFICE, "Oficio Circular" }
                };
            }
        }

        public class Galery
        {
            public class Status
            {
                public const byte ACTIVO = 1;
                public const byte OCULTO = 2;

                public static Dictionary<int, string> STATUS = new Dictionary<int, string>
                {
                    { ACTIVO, "Activo" },
                    { OCULTO, "Oculto" }
                };
            }

            public class SequenceOrder2
            {
                public const byte NINGUNO = 0;
                public const byte PRIMERO = 1;
                public const byte SEGUNDO = 2;
                public const byte TERCERO = 3;
                public const byte CUARTO = 4;
                public const byte QUINTO = 5;
                public const byte SEXTO = 6;
                public const byte SEPTIMO = 7;
                public const byte OCTAVO = 8;
                public const byte NOVENO = 9;
                public const byte DECIMO = 10;
                public const byte UNDECIMO = 11;
                public const byte DUODECIMO = 12;
                public const byte DECIMOTERCERO = 13;
                public const byte DECIMOCUARTO = 14;
                public const byte DECIMOQUINTO = 15;
                public const byte DECIMOSEXTO = 16;
                public const byte DECIMOSEPTIMO = 17;
                public const byte DECIMOOCTAVO = 18;
                public const byte DECIMONOVENO = 19;
                public const byte VIGESIMO = 20;
                public const byte VIGESIMOPRIMERO = 21;
                public const byte VIGESIMOSEGUNDO = 22;
                public const byte VIGESIMOTERCERO = 23;
                public const byte VIGESIMOCUARTO = 24;
                public const byte VIGESIMOQUINTO = 25;
                public const byte VIGESIMOSEXTO = 26;
                public const byte VIGESIMOSEPTIMO = 27;
                public const byte VIGESIMOOCTAVO = 28;
                public const byte VIGESIMONOVENO = 29;
                public const byte TRIGESIMO = 30;

                public static Dictionary<int, string> SEQUENCEORDER = new Dictionary<int, string>
                {
                    { NINGUNO, "" },
                    { PRIMERO, "PRIMERO" },
                    { SEGUNDO, "SEGUNDO" },
                    { TERCERO, "TERCERO" },
                    { CUARTO, "CUARTO" },
                    { QUINTO, "QUINTO" },
                    { SEXTO, "SEXTO" },
                    { SEPTIMO, "SÉPTIMO" },
                    { OCTAVO, "OCTAVO" },
                    { NOVENO, "NOVENO" },
                    { DECIMO, "DÉCIMO" },
                    { UNDECIMO, "UNDÉCIMO" },
                    { DUODECIMO, "DUODÉCIMO" },
                    { DECIMOTERCERO, "DÉCIMOTERCERO" },
                    { DECIMOCUARTO, "DÉCIMOCUARTO" },
                    { DECIMOQUINTO, "DÉCIMOQUINTO" },
                    { DECIMOSEXTO, "DÉCIMOSEXTO" },
                    { DECIMOSEPTIMO, "DÉCIMOSEPTIMO" },
                    { DECIMONOVENO, "DÉCIMONOVENO" },
                    { VIGESIMO, "VIGÉSIMO" },
                    { VIGESIMOPRIMERO, "VIGÉSIMOPRIMERO" },
                    { VIGESIMOSEGUNDO, "VIGÉSIMOSEGUNDO" },
                    { VIGESIMOTERCERO, "VIGÉSIMOTERCERO" },
                    { VIGESIMOCUARTO, "VIGÉSIMOCUARTO" },
                    { VIGESIMOQUINTO, "VIGÉSIMOQUINTO" },
                    { VIGESIMOSEXTO, "VIGÉSIMOSEXTO" },
                    { VIGESIMOSEPTIMO, "VIGÉSIMOSEPTIMO" },
                    { VIGESIMOOCTAVO, "VIGÉSIMOOCTAVO" },
                    { VIGESIMONOVENO, "VIGÉSIMONOVENO" },
                    { TRIGESIMO, "TRIGÉSIMO" },
                };
            }

            public class SequenceOrder
            {
                public const byte NINGUNO = 0;
                public const byte PRIMERO = 1;
                public const byte SEGUNDO = 2;
                public const byte TERCERO = 3;
                public const byte CUARTO = 4;
                public const byte QUINTO = 5;
                public const byte SEXTO = 6;
                public const byte SEPTIMO = 7;
                public const byte OCTAVO = 8;
                public const byte NOVENO = 9;
                public const byte DECIMO = 10;
                public const byte UNDECIMO = 11;
                public const byte DUODECIMO = 12;
                public const byte DECIMOTERCERO = 13;
                public const byte DECIMOCUARTO = 14;
                public const byte DECIMOQUINTO = 15;
                public const byte DECIMOSEXTO = 16;
                public const byte DECIMOSEPTIMO = 17;
                public const byte DECIMOOCTAVO = 18;
                public const byte DECIMONOVENO = 19;
                public const byte VIGESIMO = 20;
                public const byte VIGESIMOPRIMERO = 21;
                public const byte VIGESIMOSEGUNDO = 22;
                public const byte VIGESIMOTERCERO = 23;
                public const byte VIGESIMOCUARTO = 24;
                public const byte VIGESIMOQUINTO = 25;
                public const byte VIGESIMOSEXTO = 26;
                public const byte VIGESIMOSEPTIMO = 27;
                public const byte VIGESIMOOCTAVO = 28;
                public const byte VIGESIMONOVENO = 29;
                public const byte TRIGESIMO = 30;

                public static Dictionary<int, string> SEQUENCEORDER = new Dictionary<int, string>
                {
                    { NINGUNO, "" },
                    { PRIMERO, "PRIMERO" },
                    { SEGUNDO, "SEGUNDO" },
                    { TERCERO, "TERCERO" },
                    { CUARTO, "CUARTO" },
                    { QUINTO, "QUINTO" },
                    { SEXTO, "SEXTO" },
                    { SEPTIMO, "SÉPTIMO" },
                    { OCTAVO, "OCTAVO" },
                    { NOVENO, "NOVENO" },
                    { DECIMO, "DÉCIMO" },
                    { UNDECIMO, "UNDÉCIMO" },
                    { DUODECIMO, "DUODÉCIMO" },
                    { DECIMOTERCERO, "DÉCIMOTERCERO" },
                    { DECIMOCUARTO, "DÉCIMOCUARTO" },
                    { DECIMOQUINTO, "DÉCIMOQUINTO" },
                    { DECIMOSEXTO, "DÉCIMOSEXTO" },
                    { DECIMOSEPTIMO, "DÉCIMOSEPTIMO" },
                    { DECIMONOVENO, "DÉCIMONOVENO" },
                    { VIGESIMO, "VIGÉSIMO" },
                    { VIGESIMOPRIMERO, "VIGÉSIMOPRIMERO" },
                    { VIGESIMOSEGUNDO, "VIGÉSIMOSEGUNDO" },
                    { VIGESIMOTERCERO, "VIGÉSIMOTERCERO" },
                    { VIGESIMOCUARTO, "VIGÉSIMOCUARTO" },
                    { VIGESIMOQUINTO, "VIGÉSIMOQUINTO" },
                    { VIGESIMOSEXTO, "VIGÉSIMOSEXTO" },
                    { VIGESIMOSEPTIMO, "VIGÉSIMOSEPTIMO" },
                    { VIGESIMOOCTAVO, "VIGÉSIMOOCTAVO" },
                    { VIGESIMONOVENO, "VIGÉSIMONOVENO" },
                    { TRIGESIMO, "TRIGÉSIMO" },
                };
            }

        }

        public class Section
        {
            public class Status
            {
                public const byte ACTIVO = 1;
                public const byte OCULTO = 2;

                public static Dictionary<int, string> STATUS = new Dictionary<int, string>
                {
                    { ACTIVO, "Activo" },
                    { OCULTO, "Oculto" }
                };
            }

            public class SequenceOrder
            {
                public const byte SECTION2 = 1;
                public const byte SECTION3 = 2;
                public const byte SECTION5 = 3;
                public const byte SECTION6 = 4;

                public static Dictionary<int, string> SEQUENCEORDER = new Dictionary<int, string>
                {
                    { SECTION2, "SECCIÓN 2" },
                    { SECTION3, "SECCIÓN 3" },
                    { SECTION5, "SECCIÓN 5" },
                    { SECTION6, "SECCIÓN 6" }
                };
            }
        }

        public class Link
        {
            public const byte FACEBOOK = 1;
            public const byte YOUTUBE = 2;
            public const byte TWITTER = 3;
            public const byte INSTAGRAM = 4;
            public const byte LINKEDIN = 5;
            public const byte OTRO = 6;

            public static Dictionary<int, string> TITLE = new Dictionary<int, string>
                {
                    { FACEBOOK, "Facebook" },
                    { YOUTUBE, "Youtube" },
                    { TWITTER, "Twitter" },
                    { INSTAGRAM, "Instagram" },
                    { LINKEDIN, "Linkedin" },
                    { OTRO, "Otro" }
                };
        }

        #endregion

        #region INTRANET

        public static class DOCUMENT_FORMAT
        {
            public static class VARIABLES
            {
                public const string STUDENT_FULLNAME = "Estudiante_NombreCompleto";
                public const string STUDENT_DNI = "Estudiante_DNI";
                public const string STUDENT_FACULTY = "Estudiante_Facultad";
                public const string STUDENT_CAREER = "Estudiante_Escuela";
                public const string STUDENT_TERM_SPECIFIC = "Periodo_Especifico";
                public const string WEIGHTED_AVERAGE = "Promedio_Ponderado";
                public const string ADMISSION_TERM = "Periodo_Ingreso";
                public const string GRADUATION_TERM = "Periodo_Egreso";
                public const string FIRST_ENROLLMENT_TERM = "Periodo_Primera_Matricula";
                public const string APPROVED_CREDITS = "Creditos_Aprobados";
                public const string STUDENT_STATUS = "Estado_Estudiante";
                public const string STUDENT_CURRICULUM = "Estudiante_Plan";
                public const string STUDENT_USERNAME = "Estudiante_Codigo";
                public const string WEIGHTED_AVERAGE_TERM_SPECIFIC = "Promedio_Ponderado_Periodo_Especifico";
                public const string FIRST_ENROLLMENT_DATE = "Fecha_Primera_Matricula";
                public const string FIRST_ENROLLMENT_DATE_TEXT = "Fecha_Primera_Matricula_Texto";
                public const string CURRENT_ACADEMIC_YEAR_STUDENT = "Ciclo_Estudiante";
                public const string ACADEMIC_YEAR_STUDENT_TERM_SPECIFIC = "Ciclo_Estudiante_Periodo";

                public static List<string> VALUES = new List<string>
                {
                    STUDENT_FULLNAME,
                    STUDENT_DNI,
                    STUDENT_FACULTY,
                    STUDENT_CAREER,
                    STUDENT_TERM_SPECIFIC,
                    WEIGHTED_AVERAGE,
                    ADMISSION_TERM,
                    GRADUATION_TERM,
                    FIRST_ENROLLMENT_TERM,
                    APPROVED_CREDITS,
                    STUDENT_STATUS,
                    STUDENT_CURRICULUM,
                    STUDENT_USERNAME,
                    WEIGHTED_AVERAGE_TERM_SPECIFIC,
                    FIRST_ENROLLMENT_DATE,
                    FIRST_ENROLLMENT_DATE_TEXT,
                    CURRENT_ACADEMIC_YEAR_STUDENT,
                    ACADEMIC_YEAR_STUDENT_TERM_SPECIFIC
                };

                public static List<string> CONSTANT_VALUES = new List<string>
                {
                    STUDENT_FULLNAME,
                    STUDENT_DNI,
                    STUDENT_FACULTY,
                    STUDENT_CAREER,
                    WEIGHTED_AVERAGE,
                    ADMISSION_TERM,
                    GRADUATION_TERM,
                    FIRST_ENROLLMENT_TERM,
                    APPROVED_CREDITS,
                    STUDENT_STATUS,
                    STUDENT_CURRICULUM,
                    STUDENT_USERNAME,
                    FIRST_ENROLLMENT_DATE,
                    FIRST_ENROLLMENT_DATE_TEXT,
                    CURRENT_ACADEMIC_YEAR_STUDENT,
                };

                public static List<string> SPECIFIC_VALUES = new List<string>
                {
                    STUDENT_TERM_SPECIFIC,
                    WEIGHTED_AVERAGE_TERM_SPECIFIC,
                    ACADEMIC_YEAR_STUDENT_TERM_SPECIFIC
                };
            }
        }

        public static class CLASS_RESCHEDULE
        {
            public static class STATUS
            {
                public const int IN_PROCESS = 1;
                public const int ACCEPTED = 2;
                public const int NOT_APPLICABLE = 3;

                public static Dictionary<string, int> INDICES = new Dictionary<string, int>()
                {
                    { "IN_PROCESS", IN_PROCESS },
                    { "ACCEPTED", ACCEPTED },
                    { "NOT_APPLICABLE", NOT_APPLICABLE }
                };
                public static Dictionary<string, string> VALUES2 = new Dictionary<string, string>()
                {
                    { $"{IN_PROCESS}", "En Proceso" },
                    { $"{ACCEPTED}", "Aceptado" },
                    { $"{NOT_APPLICABLE}", "No Procede" }
                };
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { IN_PROCESS, "En Proceso" },
                    { ACCEPTED, "Aceptado" },
                    { NOT_APPLICABLE, "No Procede" }
                };
            }
        }
        public static class GRADECORRECTION_STATES
        {
            public const int PENDING = 1;
            public const int APPROBED = 2;
            public const int DECLINED = 3;
            public const int STUDENT_REQUEST = 4;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { PENDING, "Pendiente" },
                { APPROBED, "Aprobado" },
                { DECLINED, "Rechazado" },
                { STUDENT_REQUEST, "Solicitado por estudiante" },
            };
        }

        public static class ANNOUNCEMENT
        {
            public static class TYPE
            {
                public const byte INFO = 1;
                public const byte WARNING = 2;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    {INFO, "Informativo" },
                    {WARNING, "Advertencia" },
                };
            }
            public static class IMAGE_OR_VIDEO
            {
                public const byte IMAGE = 1;
                public const byte VIDEO = 2;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    {IMAGE, "Imágen" },
                    {VIDEO, "Video" },
                };
            }
            public static class APPEARS_IN
            {
                public const byte HOME = 1;
                public const byte LOGIN = 2;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    {HOME, "Inicio" },
                    {LOGIN, "Login" },
                };
            }
            public static class SYSTEM
            {
                public const byte LAURASSIA = 1;
                public const byte INTRANET = 2;
                public const byte ENROLLMENT = 3;
                public const byte TEACHING_MANAGEMENT = 4;
                public const byte JOBEXCHANGE = 5;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    {LAURASSIA, "Aula Virtual" },
                    {INTRANET, "Intranet" },
                    {ENROLLMENT, "Matrícula" },
                    {TEACHING_MANAGEMENT, "Gestión Docente" },
                    {JOBEXCHANGE, "Sistema de Inserción Laboral" },
                };
            }
        }
        public static class OBSERVATION_TYPES
        {
            public const byte OBSERVATION = 1;
            public const byte EXPULSION = 2;
            public const byte EQUIVALENCE = 3;
            public const byte LOCK_OUT = 4;
            public const byte UNLOCK = 5;
            public const byte AMNESTY = 6;
            public const byte INFO_UPDATE = 7;
            public const byte ACADEMICYEAR_UPDATE = 8;
            public const byte ENROLLMENTTURN_UPDATE = 9;
            public const byte SANCTIONED = 10;
            public const byte WITHDRAWN = 11;
            public const byte RESIGNATION = 12;
            public const byte CHANGE_CURRICULUM = 13;
            public const byte CANCELLATION = 14;
            public const byte CHANGE_CAMPUS = 15;
            public const byte RECOGNITION = 16;
            public const byte ABANDONMENT = 17;

            public const byte HISTORY = 98;
            public const byte EMAIL_INSTITUTIONAL = 99;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { OBSERVATION, "Observación" },
                { EXPULSION, "Expulsión" },
                { EQUIVALENCE, "Equivalencia" },
                { LOCK_OUT, "Bloqueo de Usuario" },
                { UNLOCK, "Desbloqueo de Usuario" },
                { AMNESTY, "Amnistía" },
                { INFO_UPDATE, "Actualización de Datos" },
                { HISTORY, "Historial" },
                { ACADEMICYEAR_UPDATE, "Ciclo" },
                { ENROLLMENTTURN_UPDATE, "Turno Matrícula" },
                { EMAIL_INSTITUTIONAL , "Clave Correo Institucional" },
                { SANCTIONED , "Sancionado" },
                { CHANGE_CURRICULUM , "Cambio de plan de estudio" },
                { WITHDRAWN , "Retiro de ciclo" },
                { RESIGNATION , "Renuncia" },
                { CANCELLATION , "Ingreso anulado" },
                { RECOGNITION , "Convalidación" },
                { ABANDONMENT , "Abandono" },
            };
        }

        public static class INSTITUTIONAL_ALERT
        {
            public static class ALERT_TYPES
            {
                public const int GREEN = 1;
                public const int YELLOW = 2;
                public const int RED = 3;
                public static Dictionary<int, string> TYPES = new Dictionary<int, string>()
                {
                    {GREEN, "Verde" },
                    {YELLOW, "Amarillo" },
                    {RED, "Rojo" }
                };
            }
            public static class ALERT_STATUS
            {
                public const string ACTIVE = "Activo";
                public const string INACTIVE = "Inactivo";
            }
        }
        public static class SUGGESTION_AND_TIP
        {

            public const int SUGGESTION = 1;
            public const int TIP = 2;
            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                {SUGGESTION, "Sugerencia" },
                {TIP, "Tip" },
            };
        }

        public static class GRADE_RECOVERY_EXAM_STATUS
        {
            public const byte PENDING = 1;
            public const byte CONFIRMED = 2;
            public const byte EXECUTED = 3;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                {PENDING, "Pendiente" },
                {CONFIRMED, "Confirmado" },
                {EXECUTED, "Realizado" }
            };
        }

        public static class EXAM_WEEK_TYPE
        {
            public const byte MIDTERM_EXAM = 1;
            public const byte FINAL_EXAM = 2;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                {MIDTERM_EXAM, "Examen Parcial" },
                {FINAL_EXAM, "Examen Final" },
            };
        }

        #endregion

        #region JOB_EXCHANGE

        public static class GRADUATED_UPLOAD
        {
            public static class FORMAT
            {
                public const byte GENERAL = 2;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { GENERAL, "Formato 2"}
                };
            }
        }
        public static class INTERNSHIPREQUEST
        {
            public static class Type
            {
                public const int PRE_PROFESIONAL = 1;
                public const int PRO_PROFESIONAL = 2;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
               {
                   { PRE_PROFESIONAL, "Pre-profesional" },
                   { PRO_PROFESIONAL, "Profesional" },
               };
            }

            public static class Status
            {
                public const int PENDING = 0;
                public const int INPROCESS = 1;
                public const int ACCEPTED = 2;
                public const int REJECTED = 3;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
               {
                   { PENDING, "Pendiente" },
                   { INPROCESS, "En proceso" },
                   { ACCEPTED, "Aceptado" },
                   { REJECTED, "Rechazado" },
               };
            }
        }
        public static class JobOffer
        {

            public static class Type
            {
                public const int PREPROFESIONAL_PRACTICIONER = 1;
                public const int PROFESIONAL_PRACTICIONER = 2;
                public const int GRADUATED = 3;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { PREPROFESIONAL_PRACTICIONER, "Practicante Pre-profesional" },
                    { PROFESIONAL_PRACTICIONER, "Practicante Profesional" },
                    { GRADUATED, "Egresado" }
                };
            }

            public static class WorkType
            {
                public const int FULL_TIME = 1;
                public const int PART_TIME = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { FULL_TIME, "Tiempo completo" },
                    { PART_TIME, "Tiempo parcial" }
                };
            }

            public static class MinimumFormationRequired
            {
                public const byte BACHELOR = 1;
                public const byte TITLED = 2;
                public const byte MASTER = 3;
                public const byte DOCTOR = 4;
                public const byte NOT_REQUIRED = 5;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { BACHELOR, "Bachiller" },
                    { TITLED, "Titulado" },
                    { MASTER, "Maestría" },
                    { DOCTOR, "Doctorado" },
                    { NOT_REQUIRED, "No requiere" },
                };
            }

            public static class Status
            {
                public const int ACTIVE = 1;
                public const int CANCELED = 2;
                public const int SELECTED_PERSONAL = 3;
                public const int NON_SELECTED_PERSONAL = 4;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { ACTIVE, "Activo" },
                    { CANCELED, "Cancelado" },
                    { SELECTED_PERSONAL, "Personal seleccionado" },
                    { NON_SELECTED_PERSONAL, "Personal no seleccionado" }
                };
            }
        }

        public static class CURRICULUM_VITAE
        {
            public static class TIUTIONSTATUS
            {
                public const int NOTPRESENT = 0;
                public const int ACTIVE = 1;
                public const int INACTIVE = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NOTPRESENT, "No presenta" },
                    { ACTIVE, "Activo" },
                    { INACTIVE, "Inactivo" }
                };
            }
        }

        public static class STUDENTCOMPLEMENTARYSTUDY
        {
            public static class TYPE
            {
                public const int NOTSPECIFIED = 0;
                public const int COURSE = 1;
                public const int CONGRESS = 2;
                public const int WORKSHOP = 3;
                public const int CONFERENCE = 4;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NOTSPECIFIED, "No especificado" },
                    { COURSE, "Curso" },
                    { CONGRESS, "Congreso" },
                    { WORKSHOP, "Taller" },
                    { CONFERENCE, "Conferencia" },
                };
            }
        }

        public static class STUDENTEXPERIENCE
        {
            public static class TYPE
            {
                public const int NOTSPECIFIED = 0;
                public const int EMPLOYEE = 1;
                public const int PREPROFESSIONALPRACTICES = 2;
                public const int PROFESSIONALPRACTICES = 3;


                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NOTSPECIFIED, "No especificado" },
                    { EMPLOYEE, "Empleo" },
                    { PREPROFESSIONALPRACTICES, "Prácticas Preprofesionales" },
                    { PROFESSIONALPRACTICES, "Prácticas Profesionales" },
                };
            }

            public static class TERMINATIONREASON
            {
                public const int NOTSPECIFIED = 0;
                public const int FIRED = 1;
                public const int RESIGNATION = 2;
                public const int ENDOFCONTRACT = 3;
                public const int STILLWORKING = 4;
                public const int OTHER = 5;


                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NOTSPECIFIED, "No especificado" },
                    { FIRED, "Despido" },
                    { RESIGNATION, "Renuncia" },
                    { ENDOFCONTRACT, "Fin de contrato" },
                    { STILLWORKING, "No aplica-aun laborando" },
                    { OTHER, "Otros" },
                };
            }

            public static class LEVEL
            {
                public const int NOTSPECIFIED = 0;
                public const int ASSISTANT = 1;
                public const int ANALYST = 2;
                public const int SPECIALIST = 3;
                public const int HEADOFAREA = 4;
                public const int MANAGER = 5;
                public const int SUPERVISOR = 6;
                

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NOTSPECIFIED, "No especificado" },
                    { ASSISTANT, "Auxiliar o asistente" },
                    { ANALYST, "Analista" },
                    { SPECIALIST, "Especialista" },
                    { HEADOFAREA, "Jefe de área o departamento" },
                    { MANAGER, "Gerente o director" },
                    { SUPERVISOR, "Supervisor/Coordinador" },
                };
            }

            public static class CONTRACTTYPE
            {
                public const int NOTSPECIFIED = 0;
                public const int INDETERMINATE = 1;
                public const int FIXEDTERMCONTRACT = 2;
                public const int PARTIALTERMCONTRACT = 3;
                public const int DEC1056 = 4;
                public const int DEC276 = 5;
                public const int DEC728 = 6;
                public const int RECEIPTFORFEES = 7;
                public const int OTHERREGIMES = 8;


                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NOTSPECIFIED, "No especificado" },
                    { INDETERMINATE, "Indeterminado" },
                    { FIXEDTERMCONTRACT, "Contrato a plazo fijo" },
                    { PARTIALTERMCONTRACT, "Contrato a plazo parcial" },
                    { DEC1056, "DEC. LEG. 1057.CAS" },
                    { DEC276, "DEC. LEG 276" },
                    { DEC728, "DEC. LEG. 728" },
                    { RECEIPTFORFEES, "Recibo por Honorarios" },
                    { OTHERREGIMES, "Otros regímenes laborales" }
                };
            }


            public static class EMPLOYMENTSITUATION
            {
                public const int NAMED = 1;
                public const int HIRED = 2;
                public const int WITHOUTCONTRACT = 3;
                public const int EVENTUAL = 4;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NAMED, "Nombrado" },
                    { HIRED, "Contratado" },
                    { WITHOUTCONTRACT, "Sin Contrato" },
                    { EVENTUAL, "Eventuales" },
                };
            }
        }

        public static class STUDENTCERTIFICATE
        {
            public static class TYPE
            {
                public const int NOTSPECIFIED = 0;
                public const int TECH = 1;
                public const int UNIVERSITARY = 2;
                public const int MASTER = 3;
                public const int DOCTOR = 4;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NOTSPECIFIED, "No especificado" },
                    { TECH, "Técnico Superior" },
                    { UNIVERSITARY, "Universitario" },
                    { MASTER, "Maestría" },
                    { DOCTOR, "Doctorado" },
                };
            }

            public static class LEVEL
            {
                public const int NOTSPECIFIED = 0;
                public const int STUDENT = 1;
                public const int GRADUATED = 2;
                public const int QUALIFIED = 3;


                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NOTSPECIFIED, "No especificado" },
                    { STUDENT, "Estudiante" },
                    { GRADUATED, "Egresado" },
                    { QUALIFIED, "Titulado" },
                };
            }


            public static class MERIT
            {
                public const int NONE = 0;
                public const int UPPERMIDDLE = 1;
                public const int UPPERTHIRD = 2;
                public const int HIGHERFIFTH = 3;
                public const int TENTHHIGHER = 4;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NONE, "Ninguno" },
                    { UPPERMIDDLE, "Medio Superior" },
                    { UPPERTHIRD, "Tercio Superior" },
                    { HIGHERFIFTH, "Quinto Superior" },
                    { TENTHHIGHER, "Décimo Superior" },
                };
            }
        }

        public static class STUDENTABILITYLANGUAGE
        {
            public static class TYPE
            {
                public const int SOFTWARE = 0;
                public const int LANGUAGE = 1;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { SOFTWARE, "Informáticos" },
                    { LANGUAGE, "Dialectos o idiomas" }
                };
            }
        }

        

        #endregion
       
        #region CAFETERIA

        public static class CAFETERIA_POSTULATION
        {
            public const byte Pending = 1;
            public const byte Approved = 2;
            public const byte Denied = 3;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { Pending, "Pendiente" },
                { Approved, "Aprobado" },
                { Denied, "Denegado"}

            };
        }
        public static class TURN_TYPE
        {
            public const byte Breakfast = 1;
            public const byte Lunch = 2;
            public const byte Dinner = 3;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { Breakfast, "Desayuno" },
                    { Lunch, "Almuerzo" },
                    { Dinner, "Cena" },
                };
        }

        public static class SCHOLARSHIP_TYPE
        {
            public const byte TYPE_A = 1;
            public const byte TYPE_B = 2;
            public const byte TYPE_C = 3;
            public const byte NONE = 4;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { TYPE_A, "BECA A" },
                { TYPE_B, "BECA B" },
                { TYPE_C, "BECA C" },
                { NONE, "NO ESPECIFICA" }
            };
        }
        public static class GROUP
        {
            public const bool ACEPTED = true;
            public const bool NO_ACEPTED = false;


            public static Dictionary<bool, string> VALUES = new Dictionary<bool, string>()
            {
                { ACEPTED, "APTO" },
                { NO_ACEPTED, "NO APTO" }
            };
        }

        //public static class COINTYPE
        //{
        //    public const byte DOLAR = 1;
        //    public const byte SOLES = 2;


        //    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
        //    {
        //        { DOLAR, "DOLAR" },
        //        { SOLES, "SOLES" }
        //    };
        //}

        public static class SUPPLY_CATEGORY
        {
            public const byte CATEGORY_A = 1;
            public const byte CATEGORY_B = 2;
            public const byte NONE = 3;


            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { CATEGORY_A, "CATEGORÍA A" },
                { CATEGORY_B, "CATEGORÍA B" },
                { NONE, "NO ESPECIFICA" }
            };
        }
        #endregion

        #region COMPUTER_MANAGEMENT

        public static class CURRENT_COMPUTERS_TYPES
        {
            public const int TABLET = 1;
            public const int SERVER = 2;
            public const int COMPUTER = 3;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { TABLET, "Tablet" },
                { SERVER, "Servidor" },
                { COMPUTER, "Computador" }
            };
        }

        public static class CURRENT_STATES
        {
            public const int NEW = 1;
            public const int OLD = 2;
            public const int MALOGRAPHED = 3;
            public const int LOST = 4;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { NEW, "Nuevo" },
                { OLD, "Antiguo" },
                { MALOGRAPHED, "Malogrado" },
                { LOST, "Perdido" },
            };
        }
        #endregion

        #region TRANSPARENCY PORTAL

        public static class TRANSPARENCY_PORTAL_INTEREST_LINK
        {
            public static class TYPE
            {
                public const byte FOOTER = 1;
                public const byte HEADER = 2;
            }
        }

        public static class TEMPLATES
        {
            public static class FINANCIALSOURCE
            {
                public const string ORDINARYRESOURCE = "00";
                public const string RESOURCESDIRECTLYCOLLECTED = "09";
                public const string INTERNALCREDIT = "11";
                public const string EXTERNALCREDIT = "12";
                public const string CREDIT = "19";
                public const string DONATIONS = "13";
                public const string ROYALTIES = "01";
                public const string ADUANAINCOME = "03";
                public const string FUNDSCONTRIBUTION = "04";
                public const string MUNICIPALCOMPENSATION = "07";
                public const string MUNICIPALTAXES = "08";
                public const string ADUANAINCOMEANDPARTICIPATIONS = "18";
                public static Dictionary<string, string> VALUES = new Dictionary<string, string>()
                {
                    { ORDINARYRESOURCE, "Recursos Ordinarios" },
                    { RESOURCESDIRECTLYCOLLECTED, "Recursos Directamente Recaudados"},
                    { INTERNALCREDIT, "Recursos por Operaciones Oficiales de Crédito Interno" },
                    { EXTERNALCREDIT, "Recursos por Operaciones Oficiales de Crédito Externo"},
                    { CREDIT, "Recursos por Operaciones Oficiales de Crédito" },
                    { DONATIONS, "Donaciones y Transferencias"},
                    { ROYALTIES, "Conan, Sobrecanon, Regalías y Participaciones" },
                    { ADUANAINCOME, "Participación en Renta de Aduanas"},
                    { FUNDSCONTRIBUTION, "Contribuciones a Fondos" },
                    { MUNICIPALCOMPENSATION, "Fondo de Compensación Municipal"},
                    { MUNICIPALTAXES, "Impuestos Municipales"},
                    { ADUANAINCOMEANDPARTICIPATIONS, "Conan y Sobrecanon, Regalías, Renta de Aduanas y Participaciones" }
                };
            }
            public static class EXPENSETYPE
            {
                public const string GOODS = "01";
                public const string SERVICE = "02";
                public static Dictionary<string, string> VALUES = new Dictionary<string, string>()
                {
                    { GOODS, "Bienes" },
                    { SERVICE, "Servicios"}
                };
            }
            public static class ORDERTYPE
            {
                public const int PURCHASE = 1;
                public const int SERVICE = 2;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { PURCHASE, "Compra" },
                    { SERVICE, "Servicio"}
                };
            }
            public static class TELEPHONETYPE
            {
                public const int MOBILE = 1;
                public const int LANDLINE = 2;
                public const int INTERNET = 3;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { MOBILE, "Movil" },
                    { LANDLINE, "Fijo"},
                    { INTERNET, "Internet"}
                };
            }
            public static class TRAVELTYPE
            {
                public const int NATIONAL = 1;
                public const int INTERNATIONAL = 2;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NATIONAL, "Nacional" },
                    { INTERNATIONAL, "Internacional"},
                };
            }
            public static class TRAVELMODALITY
            {
                public const int AIR = 1;
                public const int LAND = 2;
                public const int RIVER = 3;
                public const int AIRLAND = 4;
                public const int AIRRIVER = 5;
                public const int LANDRIVER = 6;
                public const int AIRLANDRIVER = 7;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { AIR, "Aereo" },
                    { LAND, "Terrestre"},
                    { RIVER, "Fluvial"},
                    { AIRLAND, "Aereo - Terrestre" },
                    { AIRRIVER , "Aereo - Fluvial"},
                    { LANDRIVER , "Terrestre - Fluvial"},
                    { AIRLANDRIVER , "Aereo - Terrestre - Fluvial"}
                };
            }
            public static class VEHICLECLASS
            {
                public const int OFICIAL = 1;
                public const int SECURITY = 2;
                public const int POOL = 3;
                public const int OPERATIVE = 4;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { OFICIAL, "Oficial" },
                    { SECURITY, "Seguridad"},
                    { POOL, "Pool"},
                    { OPERATIVE, "Operativo"}
                };
            }
        }
        public static class COMPETITION
        {
            public static class Type
            {
                public const int UNIVERSITY_TEACHER = 1;
                public const int PRACTICE_CHIEF = 2;
                public const int NOMINATION = 3;
                public const int INTERNAL_COMPETITION = 4;
                public const int PUBLIC_COMPETITION_FIXED_TERM_CONTRACT = 5;
                public const int SUBSTITUTE_CONTRACT = 6;
                public const int CAS = 7;
                public const int LOCATION_OF_SERVICES = 8;
                public const int SELECTION_PROCESS = 9;
                public const int TIP = 10;
                public const int QUOTATION_REQUEST = 11;
                public const int SCIENTIFIC_AND_TECHNOLOGICAL_RESEARCH_COMPETITION = 12;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    {UNIVERSITY_TEACHER, "DOCENTES UNIVERSITARIOS" },
                    {PRACTICE_CHIEF, "JEFES DE PRÁCTICAS" },
                    {NOMINATION, "NOMBRAMIENTO" },
                    {INTERNAL_COMPETITION , "CONCURSO INTERNO"},
                    {PUBLIC_COMPETITION_FIXED_TERM_CONTRACT , "CONCURSO PÚBLICO CONTRATO A PLAZO FIJO" },
                    {SUBSTITUTE_CONTRACT , "CONTRATOS POR SUPLENCIA" },
                    {CAS , "CAS" },
                    {LOCATION_OF_SERVICES , "LOCACIÓN DE SERVICIOS" },
                    {SELECTION_PROCESS , "PROCESO DE SELECCIÓN" },
                    {TIP , "PROPINAS" },
                    {QUOTATION_REQUEST ,"SOLICITUD DE COTIZACIONES" },
                    {SCIENTIFIC_AND_TECHNOLOGICAL_RESEARCH_COMPETITION , "CONCURSO DE INVESTIGACIÓN CIENTÍFICA Y TECNOLÓGICA" }
                };
            }
            public static class State
            {
                public const int OPEN = 1;
                public const int CLOSE = 2;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    {OPEN, "Abierto" },
                    {CLOSE, "Cerrado" }
                };
            }
            public static class FileType
            {
                public const int BASE1 = 1;
                public const int ACT2 = 2;
                public const int ACT3 = 3;
                public const int ACT4 = 4;
                public const int ACT5 = 5;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    {BASE1, "BASES 1/" },
                    {ACT2, "ACTA 2/" },
                    {ACT3, "ACTA 3/" },
                    {ACT4, "ACTA 4/" },
                    {ACT5, "ACTA 5/" }
                };
            }
        }

        public static class TRANSPARENCYPORTAL_MENU
        {
            public const byte MISIONANDVISION = 1;
            public const byte ADMISION = 2;
            public const byte ADMISION_RESULTS = 3;
            public const byte FACULTIES = 4;
            public const byte ACADEMIC_REGULATION = 5;
            public const byte INFRASTRUCTURE = 6;
            public const byte TRANSPARENCY = 7;
            public const byte TUPA = 8;
            public const byte TEACHING_PLAN = 9;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { MISIONANDVISION , "Misión y Visión"},
                { ADMISION , "Admisión"},
                { ADMISION_RESULTS , "Resultados de Admisión"},
                { FACULTIES , "Facultades"},
                { ACADEMIC_REGULATION , "Reglamento Académico"},
                { INFRASTRUCTURE , "Infraestructura"},
                { TRANSPARENCY , "Transparencia"},
                { TUPA , "TUPA"},
                { TEACHING_PLAN , "Plana Docente"}
            };
        }
        #endregion

        #region INSTITUTIONAL_WELFARE

        public static class WELFARE_ALERT
        {
            public static class STATUS
            {
                public const int PENDING = 0; //Pendiente
                public const int SOLVED = 1; //Resuelto

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { PENDING, "Pendiente" },
                    { SOLVED, "Resuelto" },
                };
            }
        }

        public static class CAFOBE_REQUEST
        {
            public static class TYPE
            {
                public const int HIGH_PERFORMANCE = 1; //Alto rendimiento
                public const int MATERNITY = 2; //Maternidad
                public const int HEALTH = 3; //Salud
                public const int FAMILY_DEATH = 4; //Defuncion
                public const int OPHTHALMOLOGICAL = 5; //Oftalmologico
                public const int STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN = 6; //Estímulo

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { HIGH_PERFORMANCE, "Alto rendimiento" },
                    { MATERNITY, "Maternidad" },
                    { HEALTH, "Salud" },
                    { FAMILY_DEATH, "Defunción" },
                    { OPHTHALMOLOGICAL, "Oftalmológico" },
                    { STIMULUSSCHOLARSHIP_OR_OUTSTANDING_SPORTSMAN, "Beca de estimulo o Deportista destacado" },
                };
            }

            public static class STATUS
            {
                public const int PENDING = 0; //Pendiente
                public const int APPROVED = 1; //Aprobado
                public const int DENIED = 2; //Denegado
                public const int OBSERVED = 3; //Observado
                public const int CORRECTED = 4; //Subsanado

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { PENDING, "Pendiente" },
                    { APPROVED, "Aprobado" },
                    { DENIED, "Denegado" },
                    { OBSERVED, "Observado" },
                    { CORRECTED, "Subsanado" },
                };
            }
        }

        public static class CAFOBE_REQUEST_DETAIL
        {
            public static class STATUS
            {
                public const int PENDING = 0; //Pendiente
                public const int APPROVED = 1; //Aprobado
                public const int DENIED = 2; //Denegado
                public const int OBSERVED = 3; //Observado
                public const int CORRECTED = 4; //Subsanado

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { PENDING, "Pendiente" },
                    { APPROVED, "Aprobado" },
                    { DENIED, "Denegado" },
                    { OBSERVED, "Observado" },
                    { CORRECTED, "Subsanado" },
                };
            }
        }

        public static class INSTITUTIONAL_WELFARE_SURVEY
        {
            public static class SISFOH
            {
                public const byte WITHOUT_CLASIFICATION = 1;
                public const byte EXTREME_POOR = 2;
                public const byte POOR = 3;
                public const byte NOT_POOR = 4;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { WITHOUT_CLASIFICATION, "Sin clasificación" },
                    { EXTREME_POOR, "Pobre extremo" },
                    { POOR, "Pobre" },
                    { NOT_POOR, "No pobre" }
                };
            }
        }
        #endregion

        #region INTEREST GROUP

        public static class INTEREST_GROUP_CONFERENCE
        {
            public static class TYPE
            {
                public const byte HANGOUT = 1;
                public const byte EXTERNAL = 2;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { HANGOUT, "Hangout" },
                    { EXTERNAL, "Externo" },
                };
            }
        }

        public static class INTEREST_GROUP_SURVEY
        {
            public static class TYPE
            {
                public const byte INTERNAL = 1;
                public const byte EXTERNAL = 2;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { INTERNAL, "Interno" },
                    { EXTERNAL, "Externo" },
                };
            }
        }

        #endregion

        #region TEACHING_RESEARCH

        public static class TEACHING_RESEARCH
        {
            public static class POSTULANT_STATUS
            {
                public const byte PENDING = 1;
                public const byte WITH_OBSERVATIONS = 2;
                public const byte ACCEPTED = 3;
                public const byte REJECTED = 4;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { PENDING, "Pendiente" },
                    { WITH_OBSERVATIONS, "Con Observaciones" },
                    { ACCEPTED, "Aceptado" },
                    { REJECTED, "Rechazado" },
                };
            }
        }

        #endregion

        #region CAMPUS

        public static class CAMPUS_GENERAL_LINK
        {
            public static class TYPE
            {
                public const byte TOPBAR = 1;
                public const byte SYSTEM = 2;
                public const byte SLIDER = 3;
            }
        }

        #endregion

        #region GENERALS

        public static class EXTERNAL_LINK
        {
            public static class TYPE
            {
                public const int NOTDEFINED = 0;
                public const int JOBOFFER = 1;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NOTDEFINED, "No definido" },
                    { JOBOFFER, "Oferta Laboral" },
                };
            }
        }
        #endregion

        #region PAYROLL
        public static class ADMINISTRATIVETABLE_TYPE
        {
            public const int SERVERTYPE = 1;
            public const int SITUATION = 2;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { SERVERTYPE, "Tipo de Servidor" },
                { SITUATION, "Situación" },
            };
        };

        public static class WORKINGTERM_STATUS
        {
            public const int INACTIVE = 0;
            public const int ACTIVE = 1;
            public const int FINISHED = 2;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { INACTIVE, "Inactivo" },
                { ACTIVE, "Activo" },
                { FINISHED, "Finalizado" },
            };
        };

        public static class PAYROLLCONCEPT_TYPE
        {
            public const int NOT = 1;
            public const int INCOME = 2;
            public const int DISCOUNT = 3;
            public const int CONTRIBUTION = 4;
            public const int REFUND = 5;
            public const int COMISSION = 6;
            public const int OTHER_INCOME = 7;
            public const int NET = 8;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { NOT, "No tiene" },
                { INCOME, "Ingresos" },
                { DISCOUNT, "Descuentos" },
                { CONTRIBUTION, "Aportes" },
                { COMISSION, "Encargaturas" },
                { OTHER_INCOME, "Otros Ingresos" },
                { NET, "Neto" },
            };
        }


        #endregion

        public static class THEME_PARAMETERS
        {
            public static Dictionary<string, string> COLORS = new Dictionary<string, string>()
            {
                { "AKDEMIC", "#0757ad" },
                { "UNAM", "#1c3d6c" },
                { "UNAMAD", "#072d3e" },
                { "UNICA", "#333333" },
                { "UNJBG", "#a40100" },
            };
        }

        public static class GRADE_INFORM
        {
            public static class DegreeType
            {
                public const int BACHELOR = 1;
                public const int PROFESIONAL_TITLE = 2;
                public const int GRADUATED = 3;


                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { BACHELOR, "Bachiller" },
                    { PROFESIONAL_TITLE, "Título Profesional" },
                    { GRADUATED, "Egresado"}
                };
            }

            public static class STATUS
            {
                public const int IN_PROCESS = 0;
                public const int FINALIZED = 1;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { IN_PROCESS, "En proceso" },
                    { FINALIZED, "Finalizado" },
                };
            }
        }


        public static class REGISTRY_PATTERN
        {
            public static class STATUS
            {
                public const int GENERATED = 1;
                public const int DENIED = 2;
                public const int APPROVED = 3;


                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { GENERATED, "Generado" },
                    { DENIED, "Denegado" },
                    { APPROVED, "Aprobado" },

                };
            }


            public static class CLASIFICATION
            {
                public const int ALL = 0;
                public const int GENERATED = 1;
                public const int PENDING = 2;


                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { ALL, "Todos" },
                    { GENERATED, "Generados" },
                    { PENDING, "Pendientes" },

                };
            }
            public static class DIPLOMA_DELIVERY
            {
                public const int PENDING = 0;
                public const int SIGNED = 1;
                public const int DELIVERED = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { PENDING, "Pendiente" },
                    { SIGNED, "Firmado" },
                    { DELIVERED, "Entregado" },
                };
            }
        }




        public static class UNIVERSITY_AUTHORITY
        {
            public static class TYPE
            {
                public const int RECTOR = 1;
                public const int VICERRECTOR = 3;
                public const int GENERAL_SECRETARY = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { RECTOR, "Rector" },
                    { VICERRECTOR, "Vicerrector" },
                    { GENERAL_SECRETARY, "Secretario General" },
                };
            }
        }

        public static class UNIVERSITY_COUNCIL_TYPE
        {
            public static class STATUS
            {
                public const string type_1 = "Ordinaria";
                public const string type_2 = "Extraordinaria";

                public static Dictionary<string, string> VALUES = new Dictionary<string, string>()
                {
                    { type_1, type_1 },
                    { type_2, type_2 }
                };
            }
        }
        public static class VirtualDirectory
        {
            public class PORTAL
            {
                public class FILTERS
                {
                    public const byte AREA = 1;
                    public const byte TEACHER = 2;
                    public const byte STAFF = 3;

                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        {AREA, "ÁREAS" },
                        {TEACHER, "DOCENTES" },
                        {STAFF, "ADMNISTRATIVO" }
                    };
                }
            }

            public static class DirectoryDependency
            {
                public static class Charge
                {
                    public const byte RESPONSIBLE = 1;
                    public const byte SECRETARY = 2;

                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                {
                    { RESPONSIBLE, "Responsable de Area" },
                    { SECRETARY, "Secretaria" }
                };
                }
            }

            public static class InstitutionalInformation
            {
                public static class Type
                {
                    public const byte ACTIVITY = 1;
                    public const byte REGULATION = 2;
                    public const byte NEWS = 3;

                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        { ACTIVITY, "Actividad" },
                        { REGULATION, "Reglamento" },
                        { NEWS, "Noticias" },
                    };
                }
            }
        }
    }
}
