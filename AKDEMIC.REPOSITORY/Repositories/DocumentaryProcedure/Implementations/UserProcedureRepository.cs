using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Template;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class UserProcedureRepository : Repository<UserProcedure>, IUserProcedureRepository
    {
        public UserProcedureRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<IEnumerable<Dependency>> GetDependenciesByUserDependencyUser(string userDependencyUserId)
        {
            var query = _context.UserDependencies
                .Where(x => x.UserId == userDependencyUserId)
                .Select(x => new Dependency
                {
                    Id = x.DependencyId
                })
                .AsQueryable();

            return await query.ToListAsync();
        }

        private async Task<DataTablesStructs.ReturnedData<UserProcedure>> GetUserProcedureDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<UserProcedure, UserProcedure>> selectPredicate = null, Expression<Func<UserProcedure, dynamic>> orderByPredicate = null, Func<UserProcedure, string[]> searchValuePredicate = null,
            string userId = null, string searchValue = null)
        {
            var query = _context.UserProcedures
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .WhereSearchValue(searchValuePredicate, searchValue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(x => x.UserId == userId);

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC

        public async Task<string> GetNextCorrelative(Guid procedureId)
        {
            var currentYear = DateTime.UtcNow.ToDefaultTimeZone().Year;
            var procedure = await _context.Procedures.Where(x => x.Id == procedureId).FirstOrDefaultAsync();
            var userProceduresByProcedureCount = await _context.UserProcedures.Where(x => x.ProcedureId == procedureId && x.CreatedAt.HasValue && x.CreatedAt.Value.Year == currentYear).CountAsync();
            return $"{procedure.Code}-{currentYear}-{(userProceduresByProcedureCount + 1):00000000}";
        }
        public async Task<DataTablesStructs.ReturnedData<UserProcedure>> GetUserProcedureDatatable(DataTablesStructs.SentParameters sentParameters, string userId = null, string searchValue = null)
        {
            Expression<Func<UserProcedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Procedure.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.TermId);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Status);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.DependencyId);
                    break;
                default:
                    orderByPredicate = ((x) => x.Code);
                    break;
            }

            return await GetUserProcedureDatatable(sentParameters, (x) => new UserProcedure
            {
                Id = x.Id,
                CreatedFormattedDate = x.CreatedAt.ToLocalDateTimeFormat(),
                CreatedAt = x.CreatedAt,
                Number = x.Number,
                Procedure = new Procedure
                {
                    Code = x.Procedure.Code,
                    Name = x.Procedure.Name,

                },
                User = new ENTITIES.Models.Generals.ApplicationUser
                {
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname
                },
                Term = new ENTITIES.Models.Enrollment.Term
                {
                    Name = x.TermId.HasValue ? x.Term.Name : "-"
                },
                StatusString = ConstantHelpers.USER_PROCEDURES.STATUS.VALUES[x.Status],
                Dependency = new Dependency
                {
                    Name = x.DependencyId.HasValue ? x.Dependency.Name : "-"
                }
            }, orderByPredicate, (x) => new[] { x.User.PaternalSurname, x.User.MaternalSurname, x.User.Name }, userId, searchValue);
        }

        public async Task<UserProcedure> GetUserProcedure(Guid id)
        {
            var query = _context.UserProcedures
                .Where(x => x.Id == id)
                .SelectUserProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserProcedure>> GetActiveEnrollmentReservations()
        {
            var query = _context.UserProcedures
                .Where(x => x.Procedure.StaticType == ConstantHelpers.PROCEDURES.STATIC_TYPE.ENROLLMENT_RESERVATION && x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED || x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS)
                .SelectUserProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserProcedure>> GetActiveUserProceduresBySearchValue(string searchValue)
        {
            var query = _context.UserProcedures
                .Where(x => (string.IsNullOrWhiteSpace(searchValue) ||
                            x.GeneratedId.ToString().Equals(searchValue) ||
                            x.User.Dni.Equals(searchValue)) &&
                            x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED ||
                            x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS)
                .SelectUserProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<UserProcedure>> GetFinishByUserProcedure(Guid id)
        {
            var query = _context.UserProcedures
                .Where(x => x.Id == id && x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED)
                .SelectUserProcedure();

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<UserProcedure>> GetActiveUserProceduresByUser(string userId, string search)
        {
            var query = _context.UserProcedures
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(q => q.Procedure.Name.Contains(search) || q.User.FullName.Contains(search));
            }

            var result = await query
                                .Where(x => x.Status != ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED && x.UserId == userId)
                                .SelectUserProcedure()
                                .OrderByDescending(x => x.GeneratedId)
                                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<UserProcedure>> GetActiveUserProceduresByUserDependencyUser(string userDependencyUserId, string search)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserProcedures
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(q => q.Procedure.Name.Contains(search) || q.User.FullName.Contains(search));
            }

            var result = await query
                        .Where(x => x.Status != ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED)
                        .Where(x => x.DependencyId == null || dependencies.Contains(x.Dependency))
                        .SelectUserProcedure()
                        .OrderByDescending(x => x.GeneratedId)
                        .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<UserProcedure>> GetActiveUserProceduresByUserDependencyUser(string userDependencyUserId, string userId, string search)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserProcedures
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(q => q.Procedure.Name.Contains(search) || q.User.FullName.Contains(search));
            }

            var result = await query
                .Where(x => x.Status != ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED)
                .Where(x => x.DependencyId == null || dependencies.Contains(x.Dependency))
                .Where(x => x.UserId == userId)
                .SelectUserProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetActiveUserProceduresByUserDependencyUserDatatable(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId = null, string userId = null, string search = null, Guid? dependencyId = null, int? status = null)
        {
            Expression<Func<UserProcedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            var query = _context.UserProcedures
                .Where(x => x.DependencyId == dependencyId)
                .Where(x => x.Status != ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED)
                .AsNoTracking();


            if (!string.IsNullOrEmpty(search))
                query = query.Where(q => q.Procedure.Name.Contains(search) || q.User.FullName.Contains(search));

            if (!string.IsNullOrEmpty(userId)) query = query.Where(x => x.UserId == userId);

            if (status.HasValue && status != 0) query = query.Where(q => q.Status == status);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    generatedId = x.GeneratedId,
                    producreName = x.Procedure.Name,
                    nameUser = x.User.FullName,
                    dependency = x.Dependency,
                    dependencyName = x.Dependency.Name,
                    duration = x.Procedure.Duration,
                    status = ConstantHelpers.USER_PROCEDURES.STATUS.VALUES[x.Status],
                    createdAt = x.CreatedAt,
                    parsedCreatedAt = x.ParsedCreatedAt,
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetDerivationUserProceduresByUserDependencyUserDatatable(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId = null, string userId = null, string search = null)
        {
            Expression<Func<UserProcedureDerivation, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            //var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserProcedureDerivations.Where(x => x.UserId == userDependencyUserId)
                .AsNoTracking();
            //var query = _context.UserProcedures.Where(x => x.DependencyId == null || dependencies.Contains(x.Dependency))
            //    .AsQueryable();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(q => q.UserProcedure.Procedure.Name.Contains(search) || q.User.FullName.Contains(search));
            }

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(x => x.UserId == userId);

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    userProcedurId = x.UserProcedureId,
                    generatedId = x.UserProcedure.GeneratedId,
                    producreName = x.UserProcedure.Procedure.Name,
                    nameUser = x.User.FullName,
                    dependency = x.Dependency,
                    dependencyName = x.Dependency.Name,
                    duration = x.UserProcedure.Procedure.Duration,
                    status = ConstantHelpers.USER_PROCEDURES.STATUS.VALUES[x.UserProcedure.Status],
                    createdAt = x.CreatedAt,
                    parsedCreatedAt = x.ParsedCreatedAt,
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
        public async Task<DataTablesStructs.ReturnedData<object>> GetHistoricUserProceduresByUserDependencyUserDatatable(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId = null, string userId = null, string search = null)
        {
            Expression<Func<UserProcedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            //var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserProcedures.Where(x => x.DependencyId == null || x.Dependency.UserDependencies.Any(y => y.UserId == userDependencyUserId))
                .AsNoTracking();


            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(q => q.Procedure.Name.Contains(search) || q.User.FullName.Contains(search));
            }

            //query = query.Where(x => x.DependencyId == null || x.UserProcedureDerivations.Count > 0 && x.UserProcedureDerivations.Any(y => dependencies.Contains(y.Dependency)));

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(x => x.UserId == userId);

            query = query.Where(x => x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED || x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.ARCHIVED);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                //|| !dependencies.Any(y => y.Id == x.DependencyId) && x.UserProcedureDerivations.Count > 0 && x.UserProcedureDerivations.Any(y => dependencies.Contains(y.Dependency)))            
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    generatedId = x.GeneratedId,
                    producreName = x.Procedure.Name,
                    nameUser = x.User.FullName,
                    dependency = x.Dependency,
                    dependencyName = x.Dependency.Name,
                    duration = x.Procedure.Duration,
                    status = ConstantHelpers.USER_PROCEDURES.STATUS.VALUES[x.Status],
                    createdAt = x.CreatedAt,
                    parsedCreatedAt = x.ParsedCreatedAt,
                    parsedUpdatedAt = x.ParsedUpdatedAt
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
        public async Task<IEnumerable<UserProcedure>> GetHistoricUserProceduresByUser(string userId, string search)
        {
            var query = _context.UserProcedures
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(q => q.Procedure.Name.Contains(search) || q.User.FullName.Contains(search));
            }

            var result = await query
                                .Where(x => x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED && x.UserId == userId)
                                .SelectUserProcedure()
                                .OrderByDescending(x => x.GeneratedId)
                                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<UserProcedure>> GetHistoricUserProceduresByUserDependencyUser(string userDependencyUserId, string search)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserProcedures
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(q => q.Procedure.Name.Contains(search) || q.User.FullName.Contains(search));
            }

            var result = await query
                                .Where(x => x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED || !dependencies.Any(y => y.Id == x.DependencyId) && x.UserProcedureDerivations.Count > 0 && x.UserProcedureDerivations.Any(y => dependencies.Contains(y.Dependency)))
                                .Where(x => x.DependencyId == null || x.UserProcedureDerivations.Count > 0 && x.UserProcedureDerivations.Any(y => dependencies.Contains(y.Dependency)))
                                .SelectUserProcedure()
                                .OrderByDescending(x => x.GeneratedId)
                                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<UserProcedure>> GetHistoricUserProceduresByUserDependencyUser(string userDependencyUserId, string userId, string search)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserProcedures
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(q => q.Procedure.Name.Contains(search) || q.User.FullName.Contains(search));
            }

            var result = await query
                .Where(x => x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED || !dependencies.Any(y => y.Id == x.DependencyId) && x.UserProcedureDerivations.Count > 0 && x.UserProcedureDerivations.Any(y => dependencies.Contains(y.Dependency)))
                .Where(x => x.DependencyId == null || x.UserProcedureDerivations.Count > 0 && x.UserProcedureDerivations.Any(y => dependencies.Contains(y.Dependency)))
                .Where(x => x.UserId == userId)
                .SelectUserProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProcedures()
        {
            var query = _context.UserProcedures
                .SelectUserProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresByDate(DateTime start, DateTime end)
        {
            var query = _context.UserProcedures
                .Where(x => x.CreatedAt > start && x.CreatedAt < end)
                .SelectUserProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresByDependency(Guid dependencyId)
        {
            var query = _context.UserProcedures
                .Where(x => x.DependencyId == dependencyId)
                .SelectUserProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresByEndDate(DateTime end)
        {
            var query = _context.UserProcedures
                .Where(x => x.CreatedAt < end)
                .SelectUserProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresByProcedure(Guid procedureId)
        {
            var query = _context.UserProcedures
                .Where(x => x.ProcedureId == procedureId)
                .SelectUserProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresBySearchValue(string searchValue)
        {
            var query = _context.UserProcedures
                .Where(x => x.User.Dni == searchValue || x.UserProcedureRecords.Any(y => y.FullRecordNumber == searchValue && y.UserProcedureId == x.Id))
                .SelectUserProcedure();

            var result = (IEnumerable<UserProcedure>)await query.ToListAsync();

            if (!result.Any())
            {
                result = query
                    .AsEnumerable()
                    .Where(x => x.Code == searchValue);
            }

            return result;
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresByStartDate(DateTime start)
        {
            var query = _context.UserProcedures
                .Where(x => x.CreatedAt > start)
                .SelectUserProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresByUser(string userId)
        {
            var query = _context.UserProcedures
                .Where(x => x.UserId == userId)
                .SelectUserProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserProcedure>> GetUserProceduresByUser(string userId, string searchValue)
        {
            if (!string.IsNullOrWhiteSpace(searchValue))
                searchValue = searchValue.ToLower();

            var query = _context.UserProcedures
                .Where(x => (string.IsNullOrWhiteSpace(searchValue) ||
                            x.GeneratedId.ToString().Equals(searchValue) ||
                            x.Procedure.Name.Contains(searchValue)) &&
                            x.UserId == userId &&
                            (x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED ||
                            x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS))
                .SelectUserProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDirectedCoursesUserProcedureDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query = _context.UserProcedures
                .Where(x => x.Procedure.StaticType == ConstantHelpers.PROCEDURES.STATIC_TYPE.DIRECTED_COURSE_INSCRIPTION
                && x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED || x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
              .Skip(sentParameters.PagingFirstRecord)
              .Take(sentParameters.RecordsPerDraw)
              .Select(x => new
              {
                  id = x.Id,
                  name = x.User.FullName,
                  code = x.User.UserName,
                  term = x.Term.Name,
                  date = x.ParsedCreatedAt,
                  student = _context.Students.Where(s => s.UserId == x.UserId).Select(s => s.Id).FirstOrDefault()
              })
                .OrderBy(x => x.date)
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrollmentReservationRequestsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, string searchValue = null)
        {
            Expression<Func<UserProcedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
                default:
                    //orderByPredicate = ((x) => x.User.UserName);
                    break;
            }

            int years = int.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.RESERVATION_TIME_LIMIT));
            var procedureId = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.RESERVATION_PROCEDURE));

            var query = _context.UserProcedures
            .Where(x => x.ProcedureId == procedureId && x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED
            || x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS).AsNoTracking();

            //if (careerId.HasValue)
            //    query = query.Where(q => q.User.Student.CareerId == careerId.Value);

            //if (facultyId.HasValue)
            //    query = query.Where(q => q.Student.Career.FacultyId == facultyId.Value);

            var recordsFiltered = query.CountAsync();

            var data = query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.User.UserName,
                    name = x.User.FullName,
                    startDate = $"{x.CreatedAt:dd/MM/yyyy}",
                    endDate = $"{x.CreatedAt.Value.AddYears(years):dd/MM/yyyy}"
                });

            return await data.ToDataTables<object>(sentParameters);
        }

        public async Task<object> GetExtemporaneousEnrollmentDatatable(ClaimsPrincipal user = null)
        {
            var students = new HashSet<string>();

            var query = _context.Students.AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));

                    students = query.Select(x => x.UserId).ToHashSet();
                }
            }

            var procedureId = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.RESERVATION_PROCEDURE));

            var queryProcedures = _context.UserProcedures
             .Where(x => x.ProcedureId == procedureId
             && x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED || x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS)
             .AsNoTracking();

            if (students.Any())
            {
                queryProcedures = queryProcedures.Where(x => students.Contains(x.UserId));
            }

            var result = await queryProcedures
            .Select(x => new
            {
                id = x.Id,
                name = x.User.FullName,
                code = x.User.UserName,
                term = x.Term.Name,
                date = x.ParsedCreatedAt
            })
            .OrderBy(x => x.date)
            .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExtemporaneousEnrollmentDatatableServerSide(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string search = null)
        {
            var procedureConfig = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.EXTEMPORANEOUS_ENROLLMENT_SURCHARGE_PROCEDURE);

            if (string.IsNullOrEmpty(procedureConfig))
            {
                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = new List<string>(),
                    DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = 0,
                    RecordsTotal = 0
                };
            }

            Expression<Func<UserProcedure, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Term.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "2":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "3":
                    orderByPredicate = ((x) => x.ParsedCreatedAt); break;
                default:
                    orderByPredicate = ((x) => x.Term.Name); break;
            }

            var students = new HashSet<string>();

            var query = _context.Students.AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));

                    students = query.Select(x => x.UserId).ToHashSet();
                }
            }

            var procedureId = Guid.Parse(procedureConfig);

            var queryProcedures = _context.UserProcedures
             .Where(x => x.ProcedureId == procedureId
             && x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED || x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS)
             .AsNoTracking();

            if (students.Any()) queryProcedures = queryProcedures.Where(x => students.Contains(x.UserId));
            if (!string.IsNullOrEmpty(search)) queryProcedures = queryProcedures.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper()) || x.User.FullName.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await queryProcedures.CountAsync();
            var data = await queryProcedures
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.User.FullName,
                    code = x.User.UserName,
                    term = x.Term.Name,
                    date = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateTimeFormat() : ""
                })
                //.OrderBy(x => x.date)
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<Guid?>> GetIdUserProcedures(List<Guid> procedureDependecies)
        {
            var userProcedures = await _context.UserProcedures.AsNoTracking().Where(x => procedureDependecies.Contains(x.ProcedureId)).Select(x => x.PaymentId).ToListAsync();

            return userProcedures;
        }
        public async Task<UserProcedure> GetUserProcedureByPaymentId(Guid id)
            => await _context.UserProcedures.FirstOrDefaultAsync(x => x.PaymentId == id);


        public async Task UpdateExtemporaneousEnrollment(Guid studentId)
        {
            var student = await _context.Students.FindAsync(studentId);

            var procedureId = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.RESERVATION_PROCEDURE));

            var procedures = await _context.UserProcedures
                  .Where(x => x.ProcedureId == procedureId && x.UserId == student.UserId
                  && x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED || x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS)
                  .ToListAsync();

            foreach (var item in procedures)
            {
                item.Status = ConstantHelpers.USER_PROCEDURES.STATUS.ACCEPTED;
            };

            await _context.SaveChangesAsync();

            //return procedures;

            //await _context.SaveChangesAsync();
            //return true;
        }
        public async Task<List<UserProcedure>> Get(string userId, Guid termId, Guid irregularProcedureId, Guid disaprovedCourseProcedureId, Guid regularProcedureId)
        {
            return await _context.UserProcedures
                    .Where(x => x.TermId == termId && x.UserId == userId
                    && (x.ProcedureId == irregularProcedureId
                    || x.ProcedureId == disaprovedCourseProcedureId
                    || x.ProcedureId == regularProcedureId))
                    .ToListAsync();
        }

        public async Task<UserProcedure> GetUserProcedureByStaticType(string userId, int staticType)
        {
            var result = await _context.UserProcedures.Include(x => x.Procedure)
                .Where(x => x.UserId == userId && x.Procedure.StaticType == staticType).FirstOrDefaultAsync();
            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRequerimentProcedure(DataTablesStructs.SentParameters sentParameters, Guid id, int? status = null)
        {
            Expression<Func<UserProcedureFile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.FileName); break;
                case "1":
                    orderByPredicate = ((x) => x.Size); break;
                default:
                    orderByPredicate = ((x) => x.FileName); break;
            }

            var students = new HashSet<string>();

            var query = _context.UserProcedureFiles.Where(x => x.UserProcedureId == id && (x.Status == CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.STATUS.SENT || x.Status == CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.STATUS.OBSERVED)).AsNoTracking();


            if (status != null)
                query = query.Where(x => x.Status == status);

            var query2 = _context.UserProcedureDerivationFiles
                .Where(x => x.UserProcedureDerivation.UserProcedureId == id)
                .AsQueryable();


            var query3 = _context.UserProcedureFiles.Where(x => x.UserProcedureId == id && x.Status == CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.STATUS.RESOLVED).AsNoTracking();

            var lstFiles = new List<FileTemplate>();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new FileTemplate
                {
                    Id = x.Id,
                    RequirementName = x.ProcedureRequirement.Name,
                    RequirementCode=  x.ProcedureRequirement.Code,
                    Filename = x.FileName,
                    Filesize = x.Size,
                    Path = x.Path,
                    Dependency = CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.TYPE.VALUES[1],
                    Type = CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.TYPE.EXTERNAL_USER,
                    Title = CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.TITLE.REQUIREMENT,
                    Status = x.Status
                })
                .ToListAsync();

            var data2 = await query2
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new FileTemplate
                {
                    Id = x.Id,
                    RequirementName = "Derivado",
                    RequirementCode = "-",
                    Filename = x.FileName,
                    Filesize = x.Size,
                    Path = x.Path,
                    Dependency = x.UserProcedureDerivation.DependencyFrom.Name,
                    Type = CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.TYPE.DEPENDENCY,
                    Title = CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.TITLE.ANEXANNEXED
                })
                .ToListAsync();

            var data3 = await query3
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new FileTemplate
                {
                    Id = x.Id,
                    RequirementName = x.ProcedureRequirement.Name,
                    RequirementCode = x.ProcedureRequirement.Code,
                    Filename = x.FileName,
                    Filesize = x.Size,
                    Path = x.Path,
                    Dependency = $"Finalizado por la dependencia {x.UserProcedure.Dependency.Name}",
                    Type = CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.TYPE.FINALY,
                    Title = CORE.Helpers.ConstantHelpers.USER_PROCEDURES.FILE.TITLE.FINALY,
                    Status = x.Status

                })
                .ToListAsync();

            foreach (var item in data)
            {
                var file = new FileTemplate()
                {
                    Id = item.Id,
                    RequirementName = item.RequirementName,
                    RequirementCode = item.RequirementCode,
                    Filename = item.Filename,
                    Filesize = item.Filesize,
                    Path = item.Path,
                    Dependency = item.Dependency,
                    Type = item.Type,
                    Title = item.Title,
                    Status = item.Status

                };
                lstFiles.Add(file);
            }

            foreach (var item in data2)
            {
                var file = new FileTemplate()
                {
                    Id = item.Id,
                    RequirementName = item.RequirementName,
                    RequirementCode = item.RequirementCode,
                    Filename = item.Filename,
                    Filesize = item.Filesize,
                    Path = item.Path,
                    Dependency = item.Dependency,
                    Type = item.Type,
                    Title = item.Title,
                    Status = item.Status

                };
                lstFiles.Add(file);
            }

            foreach (var item in data3)
            {
                var file = new FileTemplate()
                {
                    Id = item.Id,
                    RequirementName = item.RequirementName,
                    RequirementCode = item.RequirementCode,
                    Filename = item.Filename,
                    Filesize = item.Filesize,
                    Path = item.Path,
                    Dependency = item.Dependency,
                    Type = item.Type,
                    Title = item.Title,
                    Status = item.Status

                };
                lstFiles.Add(file);
            }

            var result = lstFiles
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            int recordsTotal = lstFiles.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = result,
                DrawCounter = sentParameters.DrawCounter,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsTotal
            };
        }

        public async Task<IEnumerable<UserProcedureFile>> GetRequerimentProcedureList(Guid id, int? status = null)
        {
            var query = _context.UserProcedureFiles.Include(x => x.UserProcedure).ThenInclude(x => x.Dependency)
                .Where(x => x.UserProcedureId == id && x.Status == status)
                .Include(x=>x.ProcedureRequirement)
                .AsQueryable();

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<UserProcedureFile>> GetRequerimentProcedureListPt2(Guid id, int? status = null)
        {
            var query = _context.UserProcedureFiles
                .Where(x => x.UserProcedureId == id && x.Status == status)
                .AsQueryable();

            return await query.ToListAsync();
        }
        public async Task<Guid?> GetNextByGeneratedId(int generatedId, string userDependencyUserId, string userId, Guid dependencyId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var result = await _context.UserProcedures
                .Where(x => x.DependencyId == dependencyId && x.Status != ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED)
                .OrderBy(x => x.GeneratedId)
                .Where(x => x.GeneratedId > generatedId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            if (result == Guid.Empty)
                return null;

            return result;
        }

        public async Task<Guid?> GetPreviousByGeneratedId(int generatedId, string userDependencyUserId, string userId, Guid dependencyId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var result = await _context.UserProcedures
                .Where(x => x.DependencyId == dependencyId && x.Status != ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED)
                .OrderByDescending(x => x.GeneratedId)
                .Where(x => x.GeneratedId < generatedId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            if (result == Guid.Empty)
                return null;

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserProceduresDatatable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, bool? completed, string search)
        {
            var query = _context.UserProcedures
                .OrderBy(x => x.CreatedAt)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                query = query.Where(x => x.UserId == userId);
            }

            if (completed.HasValue)
            {
                if (completed.Value)
                    query = query.Where(x => x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED);
                else
                    query = query.Where(x => x.Status != ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED);
            }

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Procedure.Name.ToLower().Trim().Contains(search.ToLower().Trim()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    createdAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    procedure = x.Procedure.Name,
                    status = ConstantHelpers.USER_PROCEDURES.STATUS.VALUES[x.Status],
                    x.RecordHistoryId,
                    RecordHistoryFileUrl = x.RecordHistory.FileURL,
                    Total = x.Payment != null ? x.Payment.Total : 0
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }


        public async Task<bool> AnyUserProcedurePending(string userId, Guid procedureId)
        {
            return await _context.UserProcedures.AnyAsync(x => x.UserId == userId && x.ProcedureId == procedureId && x.Status != ConstantHelpers.USER_PROCEDURES.STATUS.FINALIZED && x.Status != ConstantHelpers.USER_PROCEDURES.STATUS.NOT_APPLICABLE);
        }

        public async Task<List<UserProcedure>> GetStudentUserProcedures(Guid studentId, Guid? termId)
            => await _context.UserProcedures.Where(x => x.StudentUserProcedureId.HasValue && x.StudentUserProcedure.StudentId == studentId && x.StudentUserProcedure.TermId == termId).Include(x => x.StudentUserProcedure).ThenInclude(x=>x.StudentUserProcedureDetails).ToListAsync();

        public async Task<List<UserProcedure>> GetUserProceduresByUserId(string userId, Guid? termId, byte? status = null, Guid? procedureId = null)
        {
            var query = _context.UserProcedures.Where(x => x.UserId == userId && x.TermId == termId).AsNoTracking();

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            if (procedureId.HasValue)
                query = query.Where(x => x.ProcedureId == procedureId);

            return await query.ToListAsync();
        }

        #endregion
    }
}
