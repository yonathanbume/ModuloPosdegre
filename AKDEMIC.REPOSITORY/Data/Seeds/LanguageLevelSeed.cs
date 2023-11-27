using AKDEMIC.ENTITIES.Models.LanguageCenter;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class LanguageLevelSeed
    {
        public static LanguageLevel[] Seed(AkdemicContext _context)
        {
            var courses = _context.LanguageCourses.ToList();
            List<LanguageLevel> result = new List<LanguageLevel>()
            {
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(0).Id, Name = "Basico 01"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(0).Id, Name = "Basico 02"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(0).Id, Name = "Basico 03"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(0).Id, Name = "Intermedio 01"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(0).Id, Name = "Intermedio 02"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(0).Id, Name = "Intermedio 03"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(0).Id, Name = "Avanzado 01"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(0).Id, Name = "Avanzado 02"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(0).Id, Name = "Avanzado 03"},

                new LanguageLevel{ LanguageCourseId = courses.ElementAt(1).Id, Name = "Basico 01"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(1).Id, Name = "Basico 02"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(1).Id, Name = "Basico 03"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(1).Id, Name = "Intermedio 01"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(1).Id, Name = "Intermedio 02"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(1).Id, Name = "Intermedio 03"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(1).Id, Name = "Avanzado 01"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(1).Id, Name = "Avanzado 02"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(1).Id, Name = "Avanzado 03"},

                new LanguageLevel{ LanguageCourseId = courses.ElementAt(2).Id, Name = "Basico 01"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(2).Id, Name = "Basico 02"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(2).Id, Name = "Basico 03"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(2).Id, Name = "Intermedio 01"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(2).Id, Name = "Intermedio 02"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(2).Id, Name = "Intermedio 03"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(2).Id, Name = "Avanzado 01"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(2).Id, Name = "Avanzado 02"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(2).Id, Name = "Avanzado 03"},

                new LanguageLevel{ LanguageCourseId = courses.ElementAt(3).Id, Name = "Basico 01"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(3).Id, Name = "Basico 02"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(3).Id, Name = "Basico 03"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(3).Id, Name = "Intermedio 01"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(3).Id, Name = "Intermedio 02"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(3).Id, Name = "Intermedio 03"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(3).Id, Name = "Avanzado 01"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(3).Id, Name = "Avanzado 02"},
                new LanguageLevel{ LanguageCourseId = courses.ElementAt(3).Id, Name = "Avanzado 03"}
            };
            return result.ToArray();
        }
    }
}
