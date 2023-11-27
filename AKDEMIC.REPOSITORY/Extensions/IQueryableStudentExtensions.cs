using AKDEMIC.ENTITIES.Models.Generals;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Extensions
{
    public static class IQueryableStudentExtensions
    {
        public static IQueryable<Student> FilterActiveStudents(this IQueryable<Student> students)
        {
            return students
                .Where(s => 
                s.Status == CORE.Helpers.ConstantHelpers.Student.States.ENTRANT ||
                s.Status == CORE.Helpers.ConstantHelpers.Student.States.REGULAR ||
                s.Status == CORE.Helpers.ConstantHelpers.Student.States.TRANSFER ||
                s.Status == CORE.Helpers.ConstantHelpers.Student.States.IRREGULAR ||
                s.Status == CORE.Helpers.ConstantHelpers.Student.States.REPEATER ||
                s.Status == CORE.Helpers.ConstantHelpers.Student.States.UNBEATEN ||
                s.Status == CORE.Helpers.ConstantHelpers.Student.States.HIGH_PERFORMANCE ||
                s.Status == CORE.Helpers.ConstantHelpers.Student.States.OBSERVED).AsQueryable();
        }
    }
}
