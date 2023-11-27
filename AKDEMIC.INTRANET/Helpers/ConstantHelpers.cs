using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Helpers
{
    public class ConstantHelpers : CORE.Helpers.ConstantHelpers
    {
        public class Project
        {
            public const string NAME = "Intranet";
            public const bool NOTIFICATION = true;
            public const string URLBASE = "170.80.80.23:98";
            //public const string URLBASE = "http://localhost:50975/";
        }

        //public class STUDENT_FAMILY
        // {

        //     public static class CIVIL_STATUS
        //     {
        //         public const int SINGLE = 1;
        //         public const int DIVORCED = 2;
        //         public const int MARRIED = 3;
        //         public const int WIDOWED = 4;

        //         public static Dictionary<int, string> TYPE = new Dictionary<int, string>()
        //             {
        //                 { SINGLE, "Soltero(a)" },
        //                 { DIVORCED, "Divorciado(a)" },
        //                 { MARRIED, "Casado(a)" },
        //                 { WIDOWED, "Viudo(a)" },
        //             };
        //     }

        //     public static class RELATIONSHIPS
        //     {
        //         public const int FATHER = 1;
        //         public const int MOTHER = 2;
        //         public const int SON = 3;
        //         public const int DAUGHTER = 4;
        //         public const int OTHER = 5;

        //         public static Dictionary<int, string> TYPE = new Dictionary<int, string>()
        //             {
        //                 { FATHER, "Padre" },
        //                 { MOTHER, "Madre" },
        //                 { SON, "Hijo" },
        //                 { DAUGHTER, "Hija" },
        //                 { OTHER, "Otros" }
        //             };
        //     }

        //     public static class CERTIFICATES
        //     {
        //         public const int COMPLETE_PRIMARY = 1;
        //         public const int INCOMPLETE_PRIMARY = 2;
        //         public const int COMPLETE_SECONDARY = 3;
        //         public const int INCOMPLETE_SECONDARY = 4;
        //         public const int SUPERIOR_COMPLETE_TECHNIQUE = 5;
        //         public const int SUPERIOR_INCOMPLETE_TECHNIQUE = 6;
        //         public const int COMPLETE_UNIVERSITY = 7;
        //         public const int INCOMPLETE_UNIVERSITY = 8;
        //         public const int POSTGRADUATE = 9;
        //         public const int NO_STUDIES = 10;



        //         public static Dictionary<int, string> TYPE = new Dictionary<int, string>()
        //             {
        //                 { COMPLETE_PRIMARY , "Primaria completa" },
        //                 { INCOMPLETE_PRIMARY , "Primaria incompleta"},
        //                 { COMPLETE_SECONDARY , "Secundaria completa"},
        //                 { INCOMPLETE_SECONDARY , "Secundaria incompleta"},
        //                 { SUPERIOR_COMPLETE_TECHNIQUE, "Superior técnica completa"},
        //                 { SUPERIOR_INCOMPLETE_TECHNIQUE , "Superior técnica incompleta"},
        //                 { COMPLETE_UNIVERSITY , "Universitario completo"},
        //                 { INCOMPLETE_UNIVERSITY , "Universitario incompleta"},
        //                 { POSTGRADUATE , "Posgrado"},
        //                 { NO_STUDIES , "Sin nivel"}
        //              };

        //     }
        // }    

        public static class NUTRITIONAL_RECORD
        {

            public static class PHYSICAL_ACTIVITY
            {
                public const bool NO = false;
                public const bool YES = true;
                public static Dictionary<bool, string> ACTIVITY = new Dictionary<bool, string>()
                    {
                        { NO, "No" },
                        { YES, "Si"}
                    };
            }

            public static class COLATIONS
            {
                public const bool NO = false;
                public const bool YES = true;
                public static Dictionary<bool, string> OPTIONS = new Dictionary<bool, string>()
                    {
                        { NO, "No" },
                        { YES, "Si"}
                    };
            }
            public static class ALIMENTATION
            {
                public const int NO = 1;
                public const int YES = 2;
                public const int OCCASIONALLY = 3;
                public static Dictionary<int, string> OPTIONS = new Dictionary<int, string>()
                    {
                        { NO, "No" },
                        { YES, "Si"},
                        { OCCASIONALLY, "Ocasionalmente"},
                    };
            }
        }

        public static class COURSES
        {
            public const int ATTENDANCE_MIN_PERCENTAGE = 70;
        }


        public static class STUDENT_INFORMATION
        {
            public static class PERSONAL_INFORMATION
            {


                public static class PERSONAL_INFORMATION_TYPE_SCHOOL
                {
                    public const int STATE_SCHOOL = 0;
                    public const int PRIVATE_SCHOOL = 1;

                    public static Dictionary<int, string> TYPE_SCHOOL = new Dictionary<int, string>()
                    {
                        { STATE_SCHOOL,"Estatal"},
                        { PRIVATE_SCHOOL,"Particular"}
                    };
                }
                public static class PERSONAL_INFORMATION_LEVEL_EDUCATION
                {
                    public const int PRIMARY = 0;
                    public const int SECONDARY = 1;

                    public static Dictionary<int, string> LEVEL_EDUCATION = new Dictionary<int, string>()
                    {
                        { PRIMARY,"Primaria"},
                        { SECONDARY,"Secundaria"}
                    };
                }
                public static class PERSONAL_INFORMATION_CIVIL_STATUS
                {
                    public const int SINGLE = 0;
                    public const int DIVORCED = 1;
                    public const int MARRIED = 2;
                    public const int WIDOWED = 3;

                    public static Dictionary<int, string> CIVIL_STATUS = new Dictionary<int, string>()
                    {
                        { SINGLE, "Soltero(a)" },
                        { DIVORCED, "Divorciado(a)" },
                        { MARRIED, "Casado(a)" },
                        { WIDOWED, "Viudo(a)" },
                    };
                }

                public static class PERSONAL_INFORMATION_INSURANCE
                {
                    public const bool NO = false;
                    public const bool YES = true;
                    public static Dictionary<bool, string> INSURANCE = new Dictionary<bool, string>()
                    {
                        { NO, "No" },
                        { YES, "Si"}
                    };
                }
                public static class PERSONAL_INFORMATION_UNIVERSITY_PREPARATION
                {
                    public const int CEPRE_TYPE_1 = 0;
                    public const int CEPRE_TYPE_2 = 1;
                    public const int CEPRE_TYPE_3 = 2;
                    public const int CEPRE_TYPE_4 = 3;
                    public const int CEPRE_TYPE_5 = 4;
                    public const int CEPRE_TYPE_6 = 5;
                    public const int CEPRE_TYPE_7 = 6;
                    public const int CEPRE_TYPE_8 = 7;
                    public const int PARTICULAR_ACADEMY_1 = 8;
                    public const int PARTICULAR_ACADEMY_2 = 9;
                    public const int PARTICULAR_ACADEMY_3 = 10;

                    public const int PARTICULAR_ACADEMY_4 = 11;
                    public const int MYSELF_1 = 12;
                    public const int MYSELF_2 = 13;
                    public const int PARTICULAR_TEACHER = 14;
                    public const int NONE = 15;
                    public static Dictionary<int, string> UNIVERSITY_PREPARATION = new Dictionary<int, string>()
                    {
                        { CEPRE_TYPE_1, $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]}, Academica particular, Autopreparación y profesor" },
                        { CEPRE_TYPE_2, $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]}, Academica particular, Autopreparación" },
                        { CEPRE_TYPE_3,  $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]}, Academica particular y profesor particular, círculo de estudios" },
                        { CEPRE_TYPE_4,  $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]} y Academica particular" },
                        { CEPRE_TYPE_5,  $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]}, Autopreparación y profesor particular , círculo de estudios" },
                        { CEPRE_TYPE_6,  $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]} y Autopreparación " },
                        { CEPRE_TYPE_7,  $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]} y profesor particular, círculo de estudios" },
                        { CEPRE_TYPE_8,  $"CEPRE-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]} solamente" },
                        { PARTICULAR_ACADEMY_1,  "Academia particular, Autoreparación y profesor particular" },
                        { PARTICULAR_ACADEMY_2,  "Academia particular y Autoreparación " },
                        { PARTICULAR_ACADEMY_3,  "Academia particular y profesor particular, círculo de estudios " },
                        { PARTICULAR_ACADEMY_4,  "Academia particular" },
                        { MYSELF_1,  "Autopreparación y profesor particular, círculo de estudios" },
                        { MYSELF_2,  "Autopreparación" },
                        { PARTICULAR_TEACHER,  "Profesor particular, círculo de estudios" },
                        { NONE,  "No tuvo preparación alguna" },
                    };
                }

                public static class PERSONAL_INFORMATION_UNIVERISITY_CHOOSE
                {
                    public const byte CHOOSE_1 = 0;
                    public const byte CHOOSE_2 = 1;
                    public const byte CHOOSE_3 = 2;
                    public const byte CHOOSE_4 = 3;
                    public const byte CHOOSE_5 = 4;

                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        { CHOOSE_1, "Por ser estatal" },
                        { CHOOSE_2, "Por tener prestigio"},
                        { CHOOSE_3, "Por la cercanía"},
                        { CHOOSE_4, "Por ser la única en el departamento"},
                        { CHOOSE_5, "Por ser la única que ofrece dicha carrera"},

                    };
                }


                public static class PERSONAL_INFORMATION_PROFESSIONAL_SCHOOL_CHOOSE
                {
                    public const byte PROFESSIONAL_SCHOOL_CHOOSE_1 = 0;
                    public const byte PROFESSIONAL_SCHOOL_CHOOSE_2 = 1;
                    public const byte PROFESSIONAL_SCHOOL_CHOOSE_3 = 2;
                    public const byte PROFESSIONAL_SCHOOL_CHOOSE_4 = 3;
                    public const byte PROFESSIONAL_SCHOOL_CHOOSE_5 = 4;

                    public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
                    {
                        { PROFESSIONAL_SCHOOL_CHOOSE_1, "Por vocación (Me Gusta)" },
                        { PROFESSIONAL_SCHOOL_CHOOSE_2, "Me parece buena profesión"},
                        { PROFESSIONAL_SCHOOL_CHOOSE_3, "Me obligaron"},
                        { PROFESSIONAL_SCHOOL_CHOOSE_4, "Escogi al azar"},
                        { PROFESSIONAL_SCHOOL_CHOOSE_5, "Para asegurar el ingreso"},

                    };
                }

            }


            public static class HEALTH
            {
                public static class HEALTH_HABITS
                {
                    public const int NO = 0;
                    public const int YES = 1;
                    public const int SOMETIMES = 2;

                    public static Dictionary<int, string> HABITS = new Dictionary<int, string>()
                    {
                        { NO, "No" },
                        { YES, "Si" },
                        { SOMETIMES, "A Veces" }
                    };
                }
                public static class TYPE_INSURANCE
                {
                    public const int PRIVATE = 1;
                    public const int SIS = 2;
                    public const int ESSALUD = 3;
                    public const int OTHER = 4;
                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { PRIVATE, "Privado" },
                        { SIS, "SIS"},
                        { ESSALUD, "ESSALUD" },
                        { OTHER ,"Otro"}
                    };
                }
                public static class HEALTH_INSURANCE
                {
                    public const int NO = 0;
                    public const int YES = 1;

                    public static Dictionary<int, string> INSURANCE = new Dictionary<int, string>()
                    {
                        { NO, "No" },
                        { YES, "Si" }
                    };
                }
                public static class HEALTH_PARENT_SICK
                {
                    public const int NO = 0;
                    public const int YES = 1;

                    public static Dictionary<int, string> PARENT_SICK = new Dictionary<int, string>()
                    {
                        { NO, "No" },
                        { YES, "Si" }
                    };
                }
                public static class HEALTH_PARENT_PARENT_SICK
                {
                    public const int NONE = 0;
                    public const int FATHER = 1;
                    public const int MOM = 2;
                    public const int BROTHER = 3;
                    public const int ANOTHERPARENT = 4;

                    public static Dictionary<int, string> PARENT_SICK_WHO = new Dictionary<int, string>()
                    {
                        { NONE, "---" },
                        { FATHER, "Padre" },
                        { MOM, "Madre" },
                        { BROTHER, "Hermano" },
                        { ANOTHERPARENT, "Otro familiar" }
                    };
                }

                public static class HEALTH_BLOODTYPE
                {
                    public const int AB_POSITIVE = 0;
                    public const int AB_NEGATIVE = 1;
                    public const int A_POSITIVE = 2;
                    public const int A_NEGATIVE = 3;
                    public const int B_POSITIVE = 4;
                    public const int B_NEGATIVE = 5;
                    public const int O_POSITIVE = 6;
                    public const int O_NEGATIVE = 7;

                    public static Dictionary<int, string> BLOODTYPE = new Dictionary<int, string>()
                    {
                        { AB_POSITIVE, "AB+" },
                        { AB_NEGATIVE, "AB-" },
                        { A_POSITIVE, "A+" },
                        { A_NEGATIVE, "A-" },
                        { B_POSITIVE, "B+" },
                        { B_NEGATIVE, "B-" },
                        { O_POSITIVE, "O+" },
                        { O_NEGATIVE, "O-" }
                    };
                }
            }

            public static class ECONOMY
            {
                public static class ECONOMY_WORK_CONDITION
                {
                    public const int CONTRACT_FULL_TIME = 1;
                    public const int CONTRACT_PARTIAL_TIME = 2;
                    public const int CONTRACT_TEMPORAL = 3;
                    public const int CONTRACT_INDEF = 4;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { CONTRACT_FULL_TIME, "Contrato a tiempo total" },
                        { CONTRACT_PARTIAL_TIME, "Contrato a tiempo parcial"},
                        { CONTRACT_TEMPORAL, "Contrato temporal" },
                        { CONTRACT_INDEF, "Contrato indefinido" }
                    };
                }

                public static class ECONOMY_WORK_DEDICATION
                {
                    public const int COMPLETE = 1;
                    public const int PARTIAL = 2;
                    public const int FOR_HOURS = 3;


                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { COMPLETE, "Tiempo completo" },
                        { PARTIAL, "Tiempo parcial"},
                        { FOR_HOURS, "Por horas" }
                    };
                }


                public static class ECONOMY_PRINCIPAL_PERSON
                {
                    public const int FATHER_MOTHER = 0;
                    public const int FATHER = 1;
                    public const int MOTHER = 2;
                    public const int TUTOR = 3;
                    public const int MYSELF = 4;
                    public static Dictionary<int, string> PRINCIPAL_PERSON = new Dictionary<int, string>()
                    {
                        { FATHER_MOTHER, "Padre y madre" },
                        { FATHER, "Padre"},
                        { MOTHER, "Madre" },
                        { TUTOR, "Familiar tutor" },
                        { MYSELF, "El mismo estudiante "}
                    };
                }
                public static class ECONOMY_ECONOMIC_METHOD
                {
                    public const int MONTHLY = 0;
                    public const int BIWEEKLY = 1;
                    public const int WEEKLY = 2;
                    public const int DAILY = 3;
                    public static Dictionary<int, string> ECONOMIC_METHOD = new Dictionary<int, string>()
                    {
                        { MONTHLY, "Mensual" },
                        { BIWEEKLY, "Quincenal"},
                        { WEEKLY, "Semanal" },
                        { DAILY, "Diario" }
                    };
                }
                public static class ECONOMY_IS_WORK
                {
                    public const int YES = 0;
                    public const int NO = 1;
                    public static Dictionary<int, string> IS_WORK = new Dictionary<int, string>()
                    {
                        { YES, "Si" },
                        { NO, "No"}
                    };
                }
                public static class ECONOMY_D_SECTOR
                {
                    public const int NONE = 0;
                    public const int PUBLIC = 1;
                    public const int PRIVATE = 2;
                    public static Dictionary<int, string> D_SECTOR = new Dictionary<int, string>()
                    {
                        { NONE ,"---"},
                        { PUBLIC, "Público" },
                        { PRIVATE, "Privado"}
                    };
                }
                public static class ECONOMY_D_WORK_CONDITION
                {
                    public const int NONE = 0;
                    public const int APPOINTED = 1;
                    public const int HIRED = 2;
                    public static Dictionary<int, string> D_WORK_CONDITION = new Dictionary<int, string>()
                    {
                        { NONE ,"---"},
                        { APPOINTED, "Nombrado" },
                        { HIRED, "Contratado"}
                    };
                }
                public static class ECONOMY_BUSY
                {
                    public const int NONE = 0;
                    public const int YES = 1;
                    public const int NO = 2;
                    public static Dictionary<int, string> BUSY = new Dictionary<int, string>()
                    {
                        { NONE, "---" },
                        { YES, "Sí" },
                        { NO, "No"}
                    };
                }
                public static class ECONOMY_I_SECTOR
                {
                    public const int NONE = 0;
                    public const int FORMAL_TRADE = 1;
                    public const int AMBULATORY_TRADE = 2;
                    public static Dictionary<int, string> I_SECTOR = new Dictionary<int, string>()
                    {
                        { NONE, "---" },
                        { FORMAL_TRADE, "Comercio formal" },
                        { AMBULATORY_TRADE, "Comercio ambulatorio"}
                    };
                }
                public static class ECONOMY_I_WORK_CONDITION
                {
                    public const int NONE = 0;
                    public const int BY_SERVICES = 1;
                    public const int OTHERS = 2;
                    public static Dictionary<int, string> I_WORK_CONDITION = new Dictionary<int, string>()
                    {
                        { NONE, "---" },
                        { BY_SERVICES, "Por servicios" },
                        { OTHERS, "Otros"}
                    };
                }
                public static class ECONOMY_STUDENT_DEPENDENCY
                {
                    public const int BY_PARENTS = 0;
                    public const int BY_MOTHER = 1;
                    public const int BY_FATHER = 2;
                    public const int UNIQUEPARENT = 3;
                    public const int MYSELF = 4;
                    public static Dictionary<int, string> STUDENT_DEPENDENCY = new Dictionary<int, string>()
                    {
                        { BY_PARENTS, "Depende de ambos padres" },
                        { BY_MOTHER, "Sólo de madre"},
                        { BY_FATHER, "Sólo de padre" },
                        { UNIQUEPARENT, "De un familiar"},
                        { MYSELF, "De si mismo"}
                    };
                }
                public static class ECONOMY_STUDENT_COEXISTENCE
                {
                    public const int WITH_PARENTS = 0;
                    public const int ONE_PARENT = 1;
                    public const int HOSTED = 2;
                    public const int CAUTION = 3;
                    public const int RENTED_ROOM = 4;
                    public static Dictionary<int, string> STUDENT_COEXISTENCE = new Dictionary<int, string>()
                    {
                        { WITH_PARENTS, "Con sus padres" },
                        { ONE_PARENT, "Con uno de sus padres"},
                        { HOSTED, "Como alojado" },
                        { CAUTION, "Como cuidante"},
                        { RENTED_ROOM, "Cuarto alquilado"}
                    };
                }
                public static class ECONOMY_STUDENT_RISK
                {
                    public const int LIVING_PARENTS = 0;
                    public const int DEAD_FATHER = 1;
                    public const int DEAD_MOTHER = 2;
                    public const int DEAD_FATHER_MOTHER = 3;
                    public const int ALONE = 4;
                    public static Dictionary<int, string> STUDENT_RISK = new Dictionary<int, string>()
                    {
                        { LIVING_PARENTS, "Hijo de padres vivos" },
                        { DEAD_FATHER, "Huérfano de padre"},
                        { DEAD_MOTHER, "Huérfano de madre" },
                        { DEAD_FATHER_MOTHER, "Huérfano de padre y madre"},
                        { ALONE, "Vive solo"}
                    };
                }

            }

            public static class LIVING_PLACE
            {
                public static class LIVING_PLACE_ZONE
                {
                    public const int RESIDENTIAL = 0;
                    public const int URBAN = 1;
                    public const int HUMAN_SETTLEMENTS = 2;
                    public const int INVASION = 3;

                    public static Dictionary<int, string> ZONE = new Dictionary<int, string>()
                    {
                        { RESIDENTIAL, "Residencial" },
                        { URBAN, "Urbana" },
                        { HUMAN_SETTLEMENTS, "AA.HH" },
                        { INVASION, "Invasión" }
                    };
                }

                public static class LIVING_PLACE_TYPE
                {
                    public const int HOME = 0;
                    public const int HOME_CONDOMINIUM = 1;
                    public const int APARTMENT_IN_BUILDING = 2;
                    public const int FIFTH_APARTMENT = 3;
                    public const int ROOM = 4;

                    public static Dictionary<int, string> TYPE = new Dictionary<int, string>()
                    {
                        { HOME, "Casa" },
                        { HOME_CONDOMINIUM, "Casa en condominio" },
                        { APARTMENT_IN_BUILDING, "Dpto. en edificio" },
                        { FIFTH_APARTMENT, "Dpto. en quinta" },
                        { ROOM, "Cuarto" }
                    };
                }

                public static class LIVING_PLACE_TENURE
                {
                    public const int OWN = 0;
                    public const int RENTAL_SALE = 1;
                    public const int RENTED = 2;
                    public const int BORROWED = 3;
                    public const int SHARED = 4;

                    public static Dictionary<int, string> TENURE = new Dictionary<int, string>()
                    {
                        { OWN, "Propia" },
                        { RENTAL_SALE, "Alquiler-Venta" },
                        { RENTED, "Alquilada" },
                        { BORROWED, "Prestada" },
                        { SHARED, "Compartida" }
                    };
                }

                public static class LIVINGPLACE_TYPEBUILD
                {
                    public const int BRICK_CONCRETE = 0;
                    public const int ADOBE = 1;
                    public const int QUINCHA = 2;
                    public const int WOOD = 3;
                    public const int MAT = 4;

                    public static Dictionary<int, string> TYPEBUILD = new Dictionary<int, string>()
                    {
                        { BRICK_CONCRETE, "Ladrillo y Concreto" },
                        { ADOBE, "Adobe" },
                        { QUINCHA, "Quincha" },
                        { WOOD, "Madera" },
                        { MAT, "Estera" }
                    };
                }

                public static class LIVINGPLACE_CONSTRUCTION_CONDITION
                {
                    public const int IN_CONSTRUCTION = 0;
                    public const int FINISHED = 1;

                    public static Dictionary<int, string> CONSTRUCTION_CONDITION = new Dictionary<int, string>()
                    {
                        { IN_CONSTRUCTION, "En Construcción" },
                        { FINISHED, "Acabada" }
                    };
                }

                public static class LIVINGPLACE_CONSTRUCTION_TYPE
                {
                    public const int NOBLE = 0;
                    public const int MIXED = 1;
                    public const int RUSTIC = 2;
                    public const int PRECARIUS = 3;

                    public static Dictionary<int, string> CONSTRUCTION_TYPE = new Dictionary<int, string>()
                    {
                        { NOBLE, "Noble" },
                        { MIXED, "Mixto" },
                        { RUSTIC, "Rústico" },
                        { PRECARIUS, "Precario" }
                    };

                }

                public static class LIVINGPLACE_COHABITANT
                {
                    public const int BROTHER = 0;
                    public const int FATHER_MOTHER = 1;
                    public const int FATHER = 2;
                    public const int MOTHER = 3;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { BROTHER , "Hermano" },
                        { FATHER_MOTHER, "Padre y Madre" },
                        { FATHER, "Padre" },
                        { MOTHER, "Madre" }
                    };
                }
            }
            public static class RELIGION
            {
                public static class RELIGION_TYPE
                {
                    public const int CATOLIC_RELIGION = 0;
                    public const int EVANGELICAL_RELIGION = 1;
                    public const int JEHOVAHS_WITNESS = 2;
                    public const int OTHER_RELIGION = 3;

                    public static Dictionary<int, string> TYPE = new Dictionary<int, string>()
                    {
                        { CATOLIC_RELIGION, "Católica" },
                        { EVANGELICAL_RELIGION, "Evangélica" },
                        { JEHOVAHS_WITNESS, "Testigo de Jehová" },
                        { OTHER_RELIGION, "Otra" },
                    };
                }
            }
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

        public static class STUDENTSECTION_STATES
        {
            public const int IN_PROCESS = 0;
            public const int WITHDRAWN = 3;
            public const int DPI = 4;
            public const int APPROVED = 1;
            public const int DISAPPROVED = 2;
        }

        public static class ACADEMIC_HISTORIES_STATES
        {
            public const byte DISAPPROVED = 0;
            public const byte APPROVED = 1;
            public const byte WITHDRAWN = 2;
        }

        //public static class ACADEMIC_YEAR
        //{
        //    public const int FIRST = 1;
        //    public const int SECOND = 2;
        //    public const int THIRD = 3;
        //    public const int QUARTER = 4;
        //    public const int FIFTH = 5;
        //    public const int SIXTH = 6;
        //    public const int SEVENTH= 7;
        //    public const int EIGHTH= 8;
        //    public const int NINETH = 9;
        //    public const int TENTH = 10;

        //    public static Dictionary<int, string> TEXT = new Dictionary<int, string>()
        //        {
        //            {FIRST, "Primer" },
        //            {SECOND, "Segundo" },
        //            {THIRD, "Tercero" },
        //            {QUARTER, "Cuarto" },
        //            {FIFTH, "Quinto" },
        //            {SIXTH, "Sexto" },
        //            {SEVENTH, "Septimo" },
        //            {EIGHTH, "Octavo" },
        //            {NINETH, "Noveno" },
        //            {TENTH, "Decimo" },

        //        };
        //}


        public static class TREASURY
        {
            public static class INVOICE_PADLEFT
            {
                public const int SERIAL = 3;
                public const int CORRELATIVE = 8;
            }
        }

        public static class SEQUENCEORDER
        {
            public const byte FIRST = 1;
            public const byte SECOND = 2;
            public const byte THIRD = 3;
            public const byte FOURTH = 4;
            public const byte FIFTH = 5;
            public const byte SIXTH = 6;
            public const byte SEVENTH = 7;
            public const byte EIGHTH = 8;
            public const byte NINETH = 9;
            public const byte TENTH = 10;

            public static Dictionary<int, string> NAMES = new Dictionary<int, string>
            {
                { FIRST, "PRIMER" },
                { SECOND, "SEGUNDO" },
                { THIRD, "TERCER" },
                { FOURTH, "CUARTO" },
                { FIFTH, "QUINTO" },
                { SIXTH, "SEXTO" },
                { SEVENTH, "SEPTIMO" },
                { EIGHTH, "OCTAVO" },
                { NINETH, "NOVENO" },
                { TENTH, "DECIMO" },
            };
        }

        public static class BLOOD_TYPE
        {
            public static class TYPE_1
            {
                public const int O = 1;
                public const int A = 2;
                public const int B = 3;
                public const int AB = 4;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    {O, "O" },
                    {A, "A" },
                    {B, "B" },
                    {AB, "AB" }
                };
            }

        }

        public static class CONDITION_BOOLEAN
        {
            public static class TYPE_1
            {
                public const bool YES = true;
                public const bool NO = false;
                public static Dictionary<bool, string> VALUES = new Dictionary<bool, string>()
                {
                    {YES, "Sí" },
                    {NO, "No" }
                };
            }

        }


        public static class CONDITION_INT
        {
            public static class TYPE_1
            {
                public const int YES = 1;
                public const int NO = 2;
                public const int SOMETIMES = 3;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    {YES, "Sí" },
                    {NO, "No" },
                    {SOMETIMES, "A veces" }
                };
            }

            public static class TYPE_2
            {
                public const int POSITIVE = 1;
                public const int NEGATIVE = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    {POSITIVE, "Positivo" },
                    {NEGATIVE, "Negativo" }
                };
            }

        }


        public static class EVALUATION
        {
            public static class SDS
            {
                public const int NEVER = 1;
                public const int OCCASIONALLY = 2;
                public const int QUITE_OFTEN = 3;
                public const int ALWAYS = 4;
                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    {NEVER, "Nunca o muy pocas veces" },
                    {OCCASIONALLY, "De vez en cuando" },
                    {QUITE_OFTEN, "Con bastante frecuencia" },
                    {ALWAYS, "Siempre o casi siempre" }
                };
            }

        }
    }
}
