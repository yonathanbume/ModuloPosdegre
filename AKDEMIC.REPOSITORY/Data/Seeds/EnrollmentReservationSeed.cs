using AKDEMIC.ENTITIES.Models.Enrollment;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class EnrollmentReservationSeed
    {
        public static EnrollmentReservation[] Seed(AkdemicContext context)
        {
            var result = new EnrollmentReservation[0];

            //    var userProcedures = context.UserProcedures
            //        .Where(x => x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.ACCEPTED || x.Procedure.StaticType == ConstantHelpers.PROCEDURES.STATIC_TYPE.ENROLLMENT_RESERVATION)
            //        .ToList();

            //    var result = new List<EnrollmentReservation>();

            //    for (var i = 0; i < userProcedures.Count; i++)
            //    {
            //        var userProcedure = userProcedures[i];
            //        var enrollmentReservation = new EnrollmentReservation
            //        {
            //            UserProcedureId = userProcedure.Id
            //        };

            //        result.Add(enrollmentReservation);
            //    }

            return result.ToArray();
        }
    }
}
