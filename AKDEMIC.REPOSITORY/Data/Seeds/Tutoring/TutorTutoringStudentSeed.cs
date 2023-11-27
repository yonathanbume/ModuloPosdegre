using AKDEMIC.ENTITIES.Models.Tutoring;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds.Tutoring
{
    public class TutorTutoringStudentSeed
    {
        public static TutorTutoringStudent[] Seed(AkdemicContext context)
        {
            var tutors = context.Tutors.ToList();
            var tutoringStudents = context.TutoringStudents.ToList();

            var result = new List<TutorTutoringStudent>()
            {
                new TutorTutoringStudent { TutorId = tutors[0].UserId, TutoringStudentStudentId = tutoringStudents[0].StudentId, TutoringStudentTermId = tutoringStudents[0].TermId },
                new TutorTutoringStudent { TutorId = tutors[0].UserId, TutoringStudentStudentId = tutoringStudents[1].StudentId, TutoringStudentTermId = tutoringStudents[1].TermId },
                new TutorTutoringStudent { TutorId = tutors[1].UserId, TutoringStudentStudentId = tutoringStudents[2].StudentId, TutoringStudentTermId = tutoringStudents[2].TermId }
            };

            return result.ToArray();
        }
    }
}
