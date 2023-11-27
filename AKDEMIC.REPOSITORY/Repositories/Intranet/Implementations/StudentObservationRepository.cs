using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class StudentObservationRepository : Repository<StudentObservation>, IStudentObservationRepository
    {
        public StudentObservationRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<StudentObservation>> GetStudentObservationDatatable(DataTablesStructs.SentParameters sentParameters
            , Expression<Func<StudentObservation, StudentObservation>> selectPredicate = null, Expression<Func<StudentObservation, dynamic>> orderByPredicate = null, Func<StudentObservation, string[]> searchValuePredicate = null,
            Guid? studentId = null, string searchValue = null)
        {
            var query = _context.StudentObservations
                .WhereSearchValue(searchValuePredicate, searchValue)
                .AsQueryable();

            if (studentId.HasValue)
                query = query.Where(x => x.StudentId == studentId);

            query = query.AsNoTracking();

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC

        public async Task<DataTablesStructs.ReturnedData<StudentObservation>> GetStudentObservationDatatable(DataTablesStructs.SentParameters sentParameters, Guid? studentId = null, string searchValue = null)
        {
            Expression<Func<StudentObservation, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Observation);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
                default:
                    orderByPredicate = ((x) => x.Observation);
                    break;
            }

            return await GetStudentObservationDatatable(sentParameters, (x) => new StudentObservation
            {
                Id = x.Id,
                Observation = x.Observation,
                User = new ApplicationUser
                {
                    UserName = string.IsNullOrEmpty(x.UserId) ? "-" : x.User.UserName
                },
                CreatedFormattedDate = x.CreatedAt.ToLocalDateFormat()
            }, orderByPredicate, (x) => new[] { x.User.UserName }, studentId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetObservationsDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId)
        {
            Expression<Func<StudentObservation, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Observation);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
                default:
                    orderByPredicate = ((x) => x.Observation);
                    break;
            }

            var query = _context.StudentObservations
                .Where(x => x.StudentId == studentId)
                .AsTracking();

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                //.Skip(parameters.PagingFirstRecord)
                //.Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    observation = x.Observation,
                    user = string.IsNullOrEmpty(x.UserId) ? "---" : x.User.UserName,
                    date = x.CreatedAt.ToLocalDateTimeFormat(),
                    fileUrl = x.File,
                    type = ConstantHelpers.OBSERVATION_TYPES.VALUES[x.Type]
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<StudentObservation> GetByStudentId(Guid studentId)
            => await _context.StudentObservations.Where(x => x.StudentId == studentId).FirstOrDefaultAsync();

        public async Task<IEnumerable<StudentObservation>> GetAllByType(byte type)
        {
            var data = await _context.StudentObservations
                .Where(x => x.Type == type)
                .ToListAsync();

            return data;
        }

        public async Task<List<StudentObservation>> GetStudentObservations(Guid termId, Guid studentId, byte type)
            => await _context.StudentObservations
            .Where(x => x.TermId == termId && x.StudentId == studentId && x.Type == type)
            .Include(x => x.User)
            .Include(x => x.Term)
            .ToListAsync();

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetResignatedStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            Expression<Func<StudentObservation, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Term.Name);
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
                case "5":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                default:
                    break;
            }

            var query = _context.StudentObservations
                .Where(x => x.Type == ConstantHelpers.OBSERVATION_TYPES.RESIGNATION)
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
                if (careerId.HasValue && careerId != Guid.Empty)
                    query = query.Where(q => q.Student.CareerId == careerId.Value);

                if (facultyId.HasValue && facultyId != Guid.Empty)
                    query = query.Where(q => q.Student.Career.FacultyId == facultyId.Value);
            }

            if (termId.HasValue) query = query.Where(q => q.TermId == termId.Value);

            var recordsTotal = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Student.User.UserName,
                    name = x.Student.User.FullName,
                    career = x.Student.Career.Name,
                    faculty = x.Student.Career.Faculty.Name,
                    date = x.CreatedAt.ToLocalDateFormat(),
                    term = x.Term.Name,
                    file = x.File
                })
                .ToListAsync();

            var recordsFiltered = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                //RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
            };
        }

        #endregion
    }
}
