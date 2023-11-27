using AKDEMIC.ENTITIES.Models.LanguageCenter;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class LanguageQualificationSeed
    {
        public static LanguageQualification[] Seed(AkdemicContext _context)
        {
            List<LanguageSection> sections = _context.LanguageSections.ToList();

            List<LanguageCourse> courses = _context.LanguageCourses.ToList();
            for (int i = 0; i < courses.Count; i++)
            {
                List<LanguageLevel> levels = _context.LanguageLevels.Where(x => x.LanguageCourseId == courses[i].Id).ToList();
                for (int j = 1; j < levels.Count; j++)
                {
                    levels[j].PreLanguageLevelId = levels[j - 1].Id;
                }
            }
            _context.SaveChanges();

            List<LanguageQualification> result = new List<LanguageQualification>()
            {
                new LanguageQualification{ LanguageSectionId = sections.ElementAt(0).Id, Name = "Oral", Qualification = 35 },
                new LanguageQualification{ LanguageSectionId = sections.ElementAt(0).Id, Name = "Escrito", Qualification = 35 },
                new LanguageQualification{ LanguageSectionId = sections.ElementAt(0).Id, Name = "Audio", Qualification = 30 },

                new LanguageQualification{ LanguageSectionId = sections.ElementAt(1).Id, Name = "Oral", Qualification = 35 },
                new LanguageQualification{ LanguageSectionId = sections.ElementAt(1).Id, Name = "Escrito", Qualification = 35 },
                new LanguageQualification{ LanguageSectionId = sections.ElementAt(1).Id, Name = "Audio", Qualification = 30 },

                new LanguageQualification{ LanguageSectionId = sections.ElementAt(2).Id, Name = "Oral", Qualification = 35 },
                new LanguageQualification{ LanguageSectionId = sections.ElementAt(2).Id, Name = "Escrito", Qualification = 35 },
                new LanguageQualification{ LanguageSectionId = sections.ElementAt(2).Id, Name = "Audio", Qualification = 30 },

                new LanguageQualification{ LanguageSectionId = sections.ElementAt(3).Id, Name = "Oral", Qualification = 35 },
                new LanguageQualification{ LanguageSectionId = sections.ElementAt(3).Id, Name = "Escrito", Qualification = 35 },
                new LanguageQualification{ LanguageSectionId = sections.ElementAt(3).Id, Name = "Audio", Qualification = 30 },
            };
            return result.ToArray();
        }
    }
}
