using System.Collections.Generic;

namespace AKDEMIC.CORE.Configurations
{
    public class ProjectConfiguration
    {
        #region Intranet
        public static class Intranet
        {

            public static class AcademicOrder
            {
                public const int NONE = 1;
                public const int UPPER_THIRD = 2;
                public const int UPPER_FIFTH = 3;
                public const int UPPER_TENTH = 4;
                public const int UPPER_HALF = 5;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NONE, "-" },
                    { UPPER_THIRD, "Tercio Superior" },
                    { UPPER_FIFTH, "Quinto Superior" },
                    { UPPER_TENTH, "Décimo Superior" },
                    { UPPER_HALF, "Medio Superior" }
                };

                public static Dictionary<int, string> SHORT_VALUES = new Dictionary<int, string>()
                {
                    { NONE, "-" },
                    { UPPER_THIRD, "Tercio" },
                    { UPPER_FIFTH, "Quinto" },
                    { UPPER_TENTH, "Décimo" },
                    { UPPER_HALF, "M. Superior" }
                };
            }

            public static class AssistanceState
            {
                public const int ABSENCE = 0;
                public const int ASSISTED = 1;
                //public const int LATE = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
            {
                { ABSENCE, "Inasistencias" },
                { ASSISTED, "Asistencias" }
            };

                public static Dictionary<int, bool> INVERSE_VALUES = new Dictionary<int, bool>()
            {
                { ABSENCE, true },
                { ASSISTED, false }
            };
            }

            public static class SubstituteExamStatus
            {
                public const byte REGISTERED = 0;
                public const byte EVALUATED = 1;
                public const byte DELETED = 2;

                public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
            {
                { REGISTERED, "Registrado" },
                { EVALUATED, "Evaluado" },
                { DELETED, "Eliminado" }
            };
            }
        }
        #endregion

        #region Admission
        public static class Admission
        {
            public class Postulant
            {
                public class AdmissionState
                {
                    public const int PENDING = 0;
                    public const int ADMITTED = 1;
                    public const int NOT_ADMITTED = 2;
                    public const int MANUAL_APPROVAL = 3;

                    public static Dictionary<int, string> ROMAN_NUMERALS = new Dictionary<int, string>()
                {
                    {PENDING, "Pendiente" },
                    {ADMITTED, "Admitido" },
                    {NOT_ADMITTED, "No admitido" },
                    {MANUAL_APPROVAL, "Aprobación manu" },
                };
                }
            }
        }
        #endregion
    }
}
