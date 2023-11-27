using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class AdminEnrollmentRepository : Repository<AdminEnrollment>, IAdminEnrollmentRepository
    {
        public AdminEnrollmentRepository(AkdemicContext context) : base(context)
        {
        }
        public async Task<object> GetDataDatatableClientSide(Guid studentId, Guid termId)
        {
            var query = _context.AdminEnrollments
                .Where(s => s.StudentId == studentId && s.TermId == termId && s.IsRectification)
                .AsQueryable();

            var result = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(s => new
                {
                    user = s.User.UserName,
                    description = s.Observations,
                    hasFile = !string.IsNullOrEmpty(s.FileUrl),
                    date = s.CreatedAt.ToLocalDateTimeFormat(),
                    id = s.Id,
                    log = s.ActivityLog
                })                
                .ToListAsync();

            var filterRecords = result.Count;

            return new
            {
                draw = ConstantHelpers.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.DRAW_COUNTER,
                recordsTotal = filterRecords,
                recordsFiltered = filterRecords,
                data = result
            };
        }
        public async Task<AdminEnrollment> FindById(Guid id) => await _context.AdminEnrollments.FindAsync(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? termId = null, Guid? careerId = null, Guid? facultyId = null, ClaimsPrincipal user = null)
        {
            Expression<Func<AdminEnrollment, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Student.Career.Faculty.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    sentParameters.OrderDirection = ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION;
                    break;
            }

            var query = _context.AdminEnrollments
                .Where(x => x.IsRectification)
                .AsNoTracking();

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
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Student.CareerId == careerId);

                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == facultyId);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                || x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()));
            }

            if (termId.HasValue && termId != Guid.Empty)
            {
                query = query.Where(x => x.TermId == termId);
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    term = x.Term.Name,
                    code = x.Student.User.UserName,
                    name = x.Student.User.FullName,                   
                    career = x.Student.Career.Name,                   
                    faculty = x.Student.Career.Faculty.Name,                   
                    date = x.CreatedAt.ToLocalDateFormat(),
                    hasFile = !string.IsNullOrEmpty(x.FileUrl),
                    observations = x.ActivityLog
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<AdminEnrollment> GetAdminEnrollmentRectification(Guid studentId, Guid termId)
            => await _context.AdminEnrollments.Where(x => x.IsRectification == true && x.StudentId == studentId && x.TermId == termId).FirstOrDefaultAsync();
        
        public async Task<AdminEnrollment> GetLastAdminEnrollmentByStudent(Guid studentId, Guid termId)
            => await _context.AdminEnrollments.Where(x => x.StudentId == studentId && x.TermId == termId).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();

        public async Task<AdminEnrollment> GetAdminPendingEnrollmentRectification(Guid studentId, string userId, Guid termId)
           => await _context.AdminEnrollments.Where(x => x.IsRectification && !x.WasApplied && x.StudentId == studentId && x.TermId == termId && x.UserId == userId).FirstOrDefaultAsync();
    }
}
