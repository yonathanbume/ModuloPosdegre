using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class AcademicSummarySeed
    {
        public static AcademicSummary[] Seed(AkdemicContext context)
        {
            var academicYearCourses = context.AcademicYearCourses
                .Include(x => x.Curriculum)
                .ToList();
            var terms = context.Terms.ToList();
            var random = new Random();

            var result = context.StudentSections
                //.AsEnumerable() // TODO: PLEASE CHANGE THIS OR UPDATE THE SEEDER !!! // NVM !!!
                .ToList();

            var result2 = result
                .GroupBy(x => new
                {
                    x.Section.CourseTerm.TermId,
                    x.StudentId,
                    x.Student.CareerId
                })
                .Select(x => new AcademicSummary
                {
                    CareerId = x.Key.CareerId,
                    TotalCredits = x.Sum(y => y.Section.CourseTerm.Course.Credits),
                    WeightedAverageGrade = (x.Sum(y => y.FinalGrade * y.Section.CourseTerm.Course.Credits) * 1.00M) / (x.Sum(y => y.Section.CourseTerm.Course.Credits) * 1.00M),
                    WeightedAverageCumulative = (x.Sum(y => y.FinalGrade * y.Section.CourseTerm.Course.Credits) * 1.00M) / (x.Sum(y => y.Section.CourseTerm.Course.Credits) * 1.00M),
                    StudentId = x.Key.StudentId,
                    TermId = x.Key.TermId,
                    MeritOrder = 0,
                    MeritType = ConstantHelpers.ACADEMIC_ORDER.NONE,
                    StudentAcademicYear = x.Min(y => academicYearCourses.First(z => z.CourseId == y.Section.CourseTerm.CourseId).AcademicYear),
                    ApprovedCredits = x.Where(y => y.FinalGrade >= terms.First(z => z.Id == x.Key.TermId).MinGrade).Sum(y => y.Section.CourseTerm.Course.Credits),
                    TermHasFinished = x.All(y => y.Status != 0)
                })
                .ToList();

            result2.GroupBy(x => new
            {
                x.CareerId,
                x.TermId
            }).ToList().ForEach(x =>
            {
                var count = 1;
                var subTotal = x.Count();
                x.OrderByDescending(y => y.WeightedAverageGrade)
                    .ToList().ForEach(y =>
                    {
                        y.MeritOrder = count++;
                        y.MeritType = (count <= subTotal * 0.1)
                            ? ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH
                            :
                            (count > subTotal * 0.1 && count <= subTotal * 0.2)
                                ?
                                ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH
                                : (count > subTotal * 0.2 && count <= subTotal * 0.33)
                                    ? ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD
                                    : ConstantHelpers.ACADEMIC_ORDER.NONE;
                    });
            });

            return result2.ToArray();
        }
    }
}
