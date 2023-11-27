using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.TeachingManagement;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class TeacherDedicationSeed
    {
        public static TeacherDedication[] Seed(AkdemicContext context)
        {
            var workerLaborRegimes = context.WorkerLaborRegimes.ToList();

            var result = new List<TeacherDedication>()
            {
                new TeacherDedication { WorkerLaborRegimeId = workerLaborRegimes[0].Id, MaxLessonHours = 20, MaxNoLessonHours = 6, MinLessonHours = 16, MinNoLessonHours = 3, Name = "Tiempo Parcial", Status = 1 },
                new TeacherDedication { WorkerLaborRegimeId = workerLaborRegimes[1].Id, MaxLessonHours = 40, MaxNoLessonHours = 12, MinLessonHours = 35, MinNoLessonHours = 9, Name = "Tiempo Completo", Status = 1 }
            };

            return result.ToArray();
        }
    }
}
