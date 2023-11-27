using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.EnrollmentReservation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class EnrollmentReservationRepository : Repository<EnrollmentReservation>, IEnrollmentReservationRepository
    {
        public EnrollmentReservationRepository(AkdemicContext context) : base(context) { }

        public async Task<List<EnrollmentReservation>> GetEnrollmentReservations()
        {
            var query = _context.EnrollmentReservations
                .SelectEnrollmentReservation()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<EnrollmentReservation>> GetEnrollmentReservationsByStudent(Guid studentId, Guid? termId = null)
        {
            var query = _context.EnrollmentReservations
              .Where(x => x.StudentId == studentId)
              .AsNoTracking();

            if (termId.HasValue && termId != Guid.Empty)
            {
                query = query.Where(x => x.TermId == termId);
            }

            var reservations = await query
                .Include(x => x.Term)
                .ToListAsync();

            return reservations;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrollmentReservationsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            Expression<Func<EnrollmentReservation, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.Student.User.UserName);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.Student.User.FullName);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.Student.Career.Name);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.Student.Career.Faculty.Name);

                    break;
                case "5":
                    orderByPredicate = ((x) => x.CreatedAt);

                    break;
                case "6":
                    orderByPredicate = ((x) => x.ExpirationDate);

                    break;
                case "7":
                    orderByPredicate = ((x) => x.Term.Name);

                    break;
                default:
                    break;
            }

            var query = _context.EnrollmentReservations.AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                    if (careerId.HasValue && careerId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (careerId.HasValue && careerId != Guid.Empty)
                    query = query.Where(q => q.Student.CareerId == careerId.Value);

                if (facultyId.HasValue && facultyId != Guid.Empty)
                    query = query.Where(q => q.Student.Career.FacultyId == facultyId.Value);
            }

            if (termId.HasValue)
                query = query.Where(q => q.TermId == termId.Value);

            var data = query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Student.User.UserName,
                    name = x.Student.User.FullName,
                    career = x.Student.Career.Name,
                    faculty = x.Student.Career.Faculty.Name,
                    startDate = x.CreatedAt.ToLocalDateFormat(),
                    endDate = x.ExpirationDate.ToLocalDateFormat(),
                    term = x.Term.Name,
                    file = x.FileURL
                });

            return await data.ToDataTables<object>(sentParameters);
        }

        public async Task<bool> ValidateStudentReservationExist(Guid termId, Guid studentId)
            => await _context.EnrollmentReservations.AnyAsync(x => x.TermId == termId && x.StudentId == studentId);

        public async Task<IEnumerable<ReservationExcelTemplate>> GetEnrollmentReservationsExcel(Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            var query = _context.EnrollmentReservations.AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                    if (careerId.HasValue && careerId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (careerId.HasValue && careerId != Guid.Empty)
                    query = query.Where(q => q.Student.CareerId == careerId.Value);

                if (facultyId.HasValue && facultyId != Guid.Empty)
                    query = query.Where(q => q.Student.Career.FacultyId == facultyId.Value);
            }

            if (termId.HasValue)
                query = query.Where(q => q.TermId == termId.Value);

            var data = await query
                .Select(x => new
                {
                    x.StudentId,
                    UserName = x.Student.User.UserName,
                    FullName = x.Student.User.FullName,
                    Career = x.Student.Career.Name,
                    Faculty = x.Student.Career.Faculty.Name,
                    StartDate = x.CreatedAt,
                    EndDate = x.ExpirationDate,
                    Term = x.Term.Name,
                    x.TermId
                }).ToListAsync();

            var enrollmentTurns = await _context.EnrollmentTurns
                .Where(x => x.ConfirmationDate.HasValue && x.Student.EnrollmentReservations.Any(y => y.TermId == x.TermId))
                .Select(x => new
                {
                    x.StudentId,
                    x.TermId,
                    x.ConfirmationDate
                }).ToListAsync();

            var adminEnrollments = await _context.AdminEnrollments
                .Where(x => x.CreatedAt.HasValue && x.Student.EnrollmentReservations.Any(y => y.TermId == x.TermId))
                .Select(x => new
                {
                    x.StudentId,
                    x.TermId,
                    x.CreatedAt
                }).ToListAsync();

            var result = new List<ReservationExcelTemplate>();

            foreach (var item in data)
            {
                var date = (DateTime?)null;

                var turn = enrollmentTurns.FirstOrDefault(x => x.TermId == item.TermId && x.StudentId == item.StudentId);
                if (turn != null) date = turn.ConfirmationDate;
                else
                {
                    var admin = adminEnrollments.FirstOrDefault(x => x.TermId == item.TermId && x.StudentId == item.StudentId);
                    if (admin != null) date = admin.CreatedAt;
                }

                result.Add(new ReservationExcelTemplate
                {
                    UserName = item.UserName,
                    FullName = item.FullName,
                    Career = item.Career,
                    Faculty = item.Faculty,
                    StartDate = item.StartDate.ToDefaultTimeZone().ToLocalDateFormat(),
                    EndDate = item.EndDate.ToDefaultTimeZone().ToLocalDateFormat(),
                    Term = item.Term,
                    EnrollmentDate = date.HasValue ? date.ToDefaultTimeZone().ToLocalDateFormat() : ""
                });
            }

            return result;
        }

    }
}
