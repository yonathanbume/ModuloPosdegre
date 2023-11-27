using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates
{
    public static class EVALUATIONHelpers
    {
        public static class RUBRIC
        {
            public const byte INSCRIPTION = 0;
            public const byte ADVANCE = 1;
            public const byte FINAL = 2;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { INSCRIPTION, "Inscripción" },
                { ADVANCE, "Avance" },
                { FINAL, "Entrega Final" }
            };
        }
        public static class MODALITY
        {
            public const byte SOCIAL_PROYECTION = 0;
            public const byte UNIVERSITY_EXTENSION = 1;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>()
            {
                { SOCIAL_PROYECTION, "Proyección Social" },
                { UNIVERSITY_EXTENSION, "Extensión Universitaria" }
            };
        }
    }
}
