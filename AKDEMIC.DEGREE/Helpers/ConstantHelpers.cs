using System.Collections.Generic;

namespace AKDEMIC.DEGREE.Helpers
{
    public class ConstantHelpers : CORE.Helpers.ConstantHelpers
    {
        public class Project
        {
            public const string NAME = "Grados y Títulos";
            public const bool NOTIFICATION = false;
        }


        public static class GRADEREQUIREMENTSTATUS
        {
            public const int PENDING = 0;
            public const int PRESENTED = 1;
            public const int APPROBED = 2;
            public const int OBSERVATIONS = 3;
        }
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

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { IN_PROCESS, "En Proceso" },
                    { ACCEPTED, "Aceptado" },
                    { NOT_APPLICABLE, "No Procede" }
                };
            }
        }

        public static class COURSES
        {
            public const int ATTENDANCE_MIN_PERCENTAGE = 70;
        }

        public static class GRADECORRECTION_STATES
        {
            public const int PENDING = 1;
            public const int APPROBED = 2;
            public const int DECLINED = 3;
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
                public static class PERSONAL_INFORMATION_SEX
                {
                    public const int MAN = 0;
                    public const int WOMAN = 1;
                    public static Dictionary<int, string> SEX = new Dictionary<int, string>()
                    {
                        { MAN, "Masculino" },
                        { WOMAN, "Femenino"}
                    };
                }
                public static class PERSONAL_INFORMATION_INSURANCE
                {
                    public const int NO = 0;
                    public const int YES = 1;
                    public static Dictionary<int, string> INSURANCE = new Dictionary<int, string>()
                    {
                        { NO, "No" },
                        { YES, "Si"}
                    };
                }
                public static class PERSONAL_INFORMATION_UNIVERSITY_PREPARATION
                {
                    public const int PARTICULAR_TEACHER = 0;
                    public const int PARTICULAR_ACADEMY = 1;
                    public const int CEPRE = 2;
                    public const int MYSELF = 3;
                    public static Dictionary<int, string> UNIVERSITY_PREPARATION = new Dictionary<int, string>()
                    {
                        { PARTICULAR_TEACHER, "Profesor particular" },
                        { PARTICULAR_ACADEMY, "Academia particular"},
                        { CEPRE, "CEPRE" },
                        { MYSELF, "Por su cuenta"}
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
                public static class ECONOMY_PRINCIPAL_PERSON
                {
                    public const int FATHER_MOTHER = 0;
                    public const int FATHER = 1;
                    public const int MOTHER = 2;
                    public const int TUTOR = 3;
                    public const int MYSELF = 4;
                    public static Dictionary<int, string> PRINCIPAL_PERSON = new Dictionary<int, string>()
                    {
                        { FATHER_MOTHER, "Padre madre" },
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
                        { YES, "Si" },
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
            public const int FINISHED = 2;
            public const int IN_PROGRESS = 1;
            public const int WITHDRAWN = 3;
        }

        public static class TREASURY
        {
            public static class INVOICE_PADLEFT
            {
                public const int SERIAL = 3;
                public const int CORRELATIVE = 8;
            }
        }

        public static class CONDITIONAL_ANSWER
        {
            public const bool NO = false;
            public const bool YES = true;
            public static Dictionary<bool, string> VALUES = new Dictionary<bool, string>()
                    {
                        { NO, "No" },
                        { YES, "Sí"}
                    };
        }

        public static class THESIS_SUPPORT_MODALITY
        {
            public const int PRESENTIAL = 1;
            public const int VIRTUAL = 2;


            public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { PRESENTIAL, "Presencial" },
                { VIRTUAL, "Virtual" }
            };
        }
    }
}
