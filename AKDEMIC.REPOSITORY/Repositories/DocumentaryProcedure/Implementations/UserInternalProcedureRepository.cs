using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.CORE.Extensions;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using static AKDEMIC.CORE.Configurations.GeneralConfiguration;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class UserInternalProcedureRepository : Repository<UserInternalProcedure>, IUserInternalProcedureRepository
    {
        public UserInternalProcedureRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<IEnumerable<Dependency>> GetDependenciesByUserDependencyUser(string userDependencyUserId)
        {
            var query = _context.UserDependencies
                .Where(x => x.UserId == userDependencyUserId)
                .Select(x => new Dependency
                {
                    Id = x.DependencyId
                });

            return await query.ToListAsync();
        }

        private async Task<IEnumerable<UserInternalProcedure>> GetChildUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid? internalProcedureDependencyId = null, DateTime? startCreatedAt = null, DateTime? endCreatedAt = null, List<UserInternalProcedure> result = null)
        {
            if (result == null)
            {
                result = new List<UserInternalProcedure>();
            }

            var userInternalProcedures = _context.UserInternalProcedures
                .Include(x => x.InternalProcedure)
                .Where(x => x.InternalProcedure.InternalProcedureParentId == internalProcedureId);

            if (internalProcedureDependencyId != null)
            {
                userInternalProcedures = userInternalProcedures.Where(x => x.InternalProcedure.DependencyId == internalProcedureDependencyId);
            }

            if (startCreatedAt != null)
            {
                userInternalProcedures = userInternalProcedures.Where(x => x.CreatedAt >= startCreatedAt);
            }

            if (endCreatedAt != null)
            {
                userInternalProcedures = userInternalProcedures.Where(x => x.CreatedAt <= endCreatedAt);
            }

            var userInternalProcedures2 = await userInternalProcedures
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserInternalProcedure()
                .ToListAsync();

            result.AddRange(userInternalProcedures2);

            for (var i = 0; i < userInternalProcedures2.Count; i++)
            {
                await GetChildUserInternalProceduresByInternalProcedure(userInternalProcedures2[i].InternalProcedureId, internalProcedureDependencyId, startCreatedAt, endCreatedAt, result);
            }

            return result;
        }


        #endregion

        #region PUBLIC

        public async Task<UserInternalProcedure> GetUserInternalProcedure(Guid id)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Id == id)
                .SelectUserInternalProcedure();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyClaimId, string search = null)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ACCEPTED
                || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.NOT_APPLICABLE)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.InternalProcedure.DependencyId == dependencyClaimId)
                .SelectUserInternalProcedure()
                .OrderByDescending(x => x.GeneratedId);

            var result = await query.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();

                result = result.Where(x =>
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Code) && x.InternalProcedure.Code.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Subject) && x.InternalProcedure.Subject.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && x.InternalProcedure.User != null && !string.IsNullOrEmpty(x.InternalProcedure.User.FullName) && x.InternalProcedure.User.FullName.ToUpper().Trim().Contains(search)) ||
                    (!string.IsNullOrEmpty(x.UserExternalProcedureCode) && x.UserExternalProcedureCode.Contains(search)) ||
                    (x.Dependency != null && !string.IsNullOrEmpty(x.Dependency.Name) && x.Dependency.Name.ToUpper().Trim().Contains(search))
                ).ToList();
            }

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, Guid dependencyClaimId, string search = null)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ACCEPTED
                || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.NOT_APPLICABLE)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.DependencyId == dependencyId)
                .Where(x => x.DependencyId == dependencyClaimId)
                .SelectUserInternalProcedure()
                .OrderByDescending(x => x.GeneratedId);

            var result = await query.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();

                result = result.Where(x =>
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Code) && x.InternalProcedure.Code.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Subject) && x.InternalProcedure.Subject.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && x.InternalProcedure.User != null && !string.IsNullOrEmpty(x.InternalProcedure.User.FullName) && x.InternalProcedure.User.FullName.ToUpper().Trim().Contains(search)) ||
                    (!string.IsNullOrEmpty(x.UserExternalProcedureCode) && x.UserExternalProcedureCode.Contains(search)) ||
                    (x.Dependency != null && !string.IsNullOrEmpty(x.Dependency.Name) && x.Dependency.Name.ToUpper().Trim().Contains(search))
                ).ToList();
            }

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, string userId, Guid dependencyClaimId, string search = null)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ACCEPTED
                || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.NOT_APPLICABLE)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.UserId == userId)
                .Where(x => x.DependencyId == dependencyClaimId)
                .SelectUserInternalProcedure()
                .OrderByDescending(x => x.GeneratedId);

            var result = await query.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();

                result = result.Where(x =>
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Code) && x.InternalProcedure.Code.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Subject) && x.InternalProcedure.Subject.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && x.InternalProcedure.User != null && !string.IsNullOrEmpty(x.InternalProcedure.User.FullName) && x.InternalProcedure.User.FullName.ToUpper().Trim().Contains(search)) ||
                    (!string.IsNullOrEmpty(x.UserExternalProcedureCode) && x.UserExternalProcedureCode.Contains(search)) ||
                    (x.Dependency != null && !string.IsNullOrEmpty(x.Dependency.Name) && x.Dependency.Name.ToUpper().Trim().Contains(search))
                ).ToList();
            }

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, string userId, Guid dependencyClaimId, string search = null)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ACCEPTED
                || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.NOT_APPLICABLE)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.DependencyId == dependencyId)
                .Where(x => x.UserId == userId)
                .Where(x => x.DependencyId == dependencyClaimId)
                .SelectUserInternalProcedure()
                .OrderByDescending(x => x.GeneratedId);

            var result = await query.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();

                result = result.Where(x =>
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Code) && x.InternalProcedure.Code.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Subject) && x.InternalProcedure.Subject.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && x.InternalProcedure.User != null && !string.IsNullOrEmpty(x.InternalProcedure.User.FullName) && x.InternalProcedure.User.FullName.ToUpper().Trim().Contains(search)) ||
                    (!string.IsNullOrEmpty(x.UserExternalProcedureCode) && x.UserExternalProcedureCode.Contains(search)) ||
                    (x.Dependency != null && !string.IsNullOrEmpty(x.Dependency.Name) && x.Dependency.Name.ToUpper().Trim().Contains(search))
                ).ToList();
            }

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ACCEPTED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ACCEPTED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.DependencyId == dependencyId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, string userId)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ACCEPTED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.UserId == userId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetAcceptedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId, string userId)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ACCEPTED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.DependencyId == dependencyId)
                .Where(x => x.UserId == userId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }
        private int GetTime()
        {
            var time = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1));
            return (int)(time.TotalMilliseconds + 0.5);
        }
        private TimeSpan GetTime(DateTime date)
        {
            var st = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var t = (date.ToUniversalTime() - st);
            return t;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetPastProcedure(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<UserInternalProcedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.InternalProcedure.Number;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Status;
                    break;
                case "3":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "4":
                    orderByPredicate = (x) => x.UpdatedAt;
                    break;
                //case "5":
                //    orderByPredicate = (x) => Math.Round( Convert.ToDecimal((GetTime(DateTime.Now) - GetTime(x.CreatedAt.Value))) / (1000 * 60 * 60 * 24) );
                //    break;
                default:
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
            }


            var query = _context.UserInternalProcedures.Include(x => x.InternalProcedure).Where(x => x.UpdatedAt < DateTime.UtcNow)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(q => q.Dependency.Name.ToLower().Contains(search.Trim().ToLower())
                    /*|| q.InternalProcedure.Code.ToUpper().Contains(search.Trim().ToLower())*/);

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    depencendy = x.Dependency.Name,
                    code = x.InternalProcedure.Code,
                    status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.VALUES[x.Status],
                    parsedCreatedAt = x.ParsedCreatedAt,
                    parsedUpdatedAt = x.UpdatedAt.ToLocalDateFormat(),
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
        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyClaimId, int? status = null, string search = null, Guid? procedureFolderId = null)
        {
            var query = _context.UserInternalProcedures
                                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.NOT_APPLICABLE)
                                .Where(x => x.UserId == null || x.UserId == internalProcedureUserId)
                                .Where(x => x.DependencyId == dependencyClaimId).AsNoTracking();

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            if (procedureFolderId.HasValue)
                query = query.Where(x => x.ProcedureFolderId == procedureFolderId);

            query = query
                .SelectUserInternalProcedure()
                .OrderByDescending(x => x.GeneratedId);

            var result = await query.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();

                result = result.Where(x =>
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Code) && x.InternalProcedure.Code.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Subject) && x.InternalProcedure.Subject.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && x.InternalProcedure.User != null && !string.IsNullOrEmpty(x.InternalProcedure.User.FullName) && x.InternalProcedure.User.FullName.ToUpper().Trim().Contains(search)) ||
                    (!string.IsNullOrEmpty(x.UserExternalProcedureCode) && x.UserExternalProcedureCode.Contains(search)) ||
                    (x.Dependency != null && !string.IsNullOrEmpty(x.Dependency.Name) && x.Dependency.Name.ToUpper().Trim().Contains(search))
                ).ToList();
            }

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, Guid dependencyClaimId, int? status = null, string search = null, Guid? procedureFolderId = null)
        {
            var query = _context.UserInternalProcedures
                                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.NOT_APPLICABLE)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.DependencyId == dependencyId)
                .Where(x => x.DependencyId == dependencyClaimId);

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            if (procedureFolderId.HasValue)
                query = query.Where(x => x.ProcedureFolderId == procedureFolderId);

            query = query
                .SelectUserInternalProcedure()
                .OrderByDescending(x => x.GeneratedId);

            var result = await query.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();

                result = result.Where(x =>
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Code) && x.InternalProcedure.Code.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Subject) && x.InternalProcedure.Subject.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && x.InternalProcedure.User != null && !string.IsNullOrEmpty(x.InternalProcedure.User.FullName) && x.InternalProcedure.User.FullName.ToUpper().Trim().Contains(search)) ||
                    (!string.IsNullOrEmpty(x.UserExternalProcedureCode) && x.UserExternalProcedureCode.Contains(search)) ||
                    (x.Dependency != null && !string.IsNullOrEmpty(x.Dependency.Name) && x.Dependency.Name.ToUpper().Trim().Contains(search))
                ).ToList();
            }

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, string userId, Guid dependencyClaimId, int? status = null, string search = null, Guid? procedureFolderId = null)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.NOT_APPLICABLE)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.UserId == userId)
                .Where(x => x.DependencyId == dependencyClaimId);

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            if (procedureFolderId.HasValue)
                query = query.Where(x => x.ProcedureFolderId == procedureFolderId);

            query = query
                .SelectUserInternalProcedure()
                .OrderByDescending(x => x.GeneratedId);

            var result = await query.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();

                result = result.Where(x =>
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Code) && x.InternalProcedure.Code.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Subject) && x.InternalProcedure.Subject.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && x.InternalProcedure.User != null && !string.IsNullOrEmpty(x.InternalProcedure.User.FullName) && x.InternalProcedure.User.FullName.ToUpper().Trim().Contains(search)) ||
                    (!string.IsNullOrEmpty(x.UserExternalProcedureCode) && x.UserExternalProcedureCode.Contains(search)) ||
                    (x.Dependency != null && !string.IsNullOrEmpty(x.Dependency.Name) && x.Dependency.Name.ToUpper().Trim().Contains(search))
                ).ToList();
            }

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, string userId, Guid dependencyClaimId, int? status = null, string search = null, Guid? procedureFolderId = null)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.NOT_APPLICABLE)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.DependencyId == dependencyId)
                .Where(x => x.UserId == userId)
                .Where(x => x.DependencyId == dependencyClaimId);

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            if (procedureFolderId.HasValue)
                query = query.Where(x => x.ProcedureFolderId == procedureFolderId);

            query = query
                .SelectUserInternalProcedure()
                .OrderByDescending(x => x.GeneratedId);

            var result = await query.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();

                result = result.Where(x =>
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Code) && x.InternalProcedure.Code.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Subject) && x.InternalProcedure.Subject.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && x.InternalProcedure.User != null && !string.IsNullOrEmpty(x.InternalProcedure.User.FullName) && x.InternalProcedure.User.FullName.ToUpper().Trim().Contains(search)) ||
                    (!string.IsNullOrEmpty(x.UserExternalProcedureCode) && x.UserExternalProcedureCode.Contains(search)) ||
                    (x.Dependency != null && !string.IsNullOrEmpty(x.Dependency.Name) && x.Dependency.Name.ToUpper().Trim().Contains(search))
                ).ToList();
            }

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.DependencyId == dependencyId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, string userId)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.UserId == userId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetArchivedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId, string userId)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED || x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.DependencyId == dependencyId)
                .Where(x => x.UserId == userId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetChildUserInternalProceduresByInternalProcedure(Guid internalProcedureId)
        {
            var result = await GetChildUserInternalProceduresByInternalProcedure(internalProcedureId);

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetChildUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid internalProcedureDependencyId)
        {
            var result = await GetChildUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId);

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetChildUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid internalProcedureDependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            var result = await GetChildUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId, startCreatedAt, endCreatedAt);

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetChildUserInternalProceduresByInternalProcedure(Guid internalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            var result = await GetChildUserInternalProceduresByInternalProcedure(internalProcedureId, null, startCreatedAt, endCreatedAt);

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyClaimId, string search = null)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.DISPATCHED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.InternalProcedure.DependencyId == dependencyClaimId)
                .SelectUserInternalProcedure()
                .OrderByDescending(x => x.GeneratedId);

            var result = await query.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();

                result = result.Where(x =>
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Code) && x.InternalProcedure.Code.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Subject) && x.InternalProcedure.Subject.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && x.InternalProcedure.User != null && !string.IsNullOrEmpty(x.InternalProcedure.User.FullName) && x.InternalProcedure.User.FullName.ToUpper().Trim().Contains(search)) ||
                    (!string.IsNullOrEmpty(x.UserExternalProcedureCode) && x.UserExternalProcedureCode.Contains(search)) ||
                    (x.Dependency != null && !string.IsNullOrEmpty(x.Dependency.Name) && x.Dependency.Name.ToUpper().Trim().Contains(search))
                ).ToList();
            }

            return result; 
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, Guid dependencyClaimId, string search = null)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.DISPATCHED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.DependencyId == dependencyId)
                .Where(x => x.InternalProcedure.DependencyId == dependencyClaimId)
                .SelectUserInternalProcedure()
                .OrderByDescending(x => x.GeneratedId);

            var result = await query.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();

                result = result.Where(x =>
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Code) && x.InternalProcedure.Code.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Subject) && x.InternalProcedure.Subject.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && x.InternalProcedure.User != null && !string.IsNullOrEmpty(x.InternalProcedure.User.FullName) && x.InternalProcedure.User.FullName.ToUpper().Trim().Contains(search)) ||
                    (!string.IsNullOrEmpty(x.UserExternalProcedureCode) && x.UserExternalProcedureCode.Contains(search)) ||
                    (x.Dependency != null && !string.IsNullOrEmpty(x.Dependency.Name) && x.Dependency.Name.ToUpper().Trim().Contains(search))
                ).ToList();
            }

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, string userId, Guid dependencyClaimId, string search = null)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.DISPATCHED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.UserId == userId)
                .Where(x => x.InternalProcedure.DependencyId == dependencyClaimId)
                .SelectUserInternalProcedure()
                .OrderByDescending(x => x.GeneratedId);

            var result = await query.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();

                result = result.Where(x =>
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Code) && x.InternalProcedure.Code.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Subject) && x.InternalProcedure.Subject.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && x.InternalProcedure.User != null && !string.IsNullOrEmpty(x.InternalProcedure.User.FullName) && x.InternalProcedure.User.FullName.ToUpper().Trim().Contains(search)) ||
                    (!string.IsNullOrEmpty(x.UserExternalProcedureCode) && x.UserExternalProcedureCode.Contains(search)) ||
                    (x.Dependency != null && !string.IsNullOrEmpty(x.Dependency.Name) && x.Dependency.Name.ToUpper().Trim().Contains(search))
                ).ToList();
            }

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresByInternalProcedureUser(string internalProcedureUserId, Guid dependencyId, string userId, Guid dependencyClaimId, string search = null)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.DISPATCHED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.DependencyId == dependencyId)
                .Where(x => x.UserId == userId)
                .Where(x => x.InternalProcedure.DependencyId == dependencyClaimId)
                .SelectUserInternalProcedure()
                .OrderByDescending(x => x.GeneratedId);

            var result = await query.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();

                result = result.Where(x =>
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Code) && x.InternalProcedure.Code.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Subject) && x.InternalProcedure.Subject.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && x.InternalProcedure.User != null && !string.IsNullOrEmpty(x.InternalProcedure.User.FullName) && x.InternalProcedure.User.FullName.ToUpper().Trim().Contains(search)) ||
                    (!string.IsNullOrEmpty(x.UserExternalProcedureCode) && x.UserExternalProcedureCode.Contains(search)) ||
                    (x.Dependency != null && !string.IsNullOrEmpty(x.Dependency.Name) && x.Dependency.Name.ToUpper().Trim().Contains(search))
                ).ToList();
            }

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.DISPATCHED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.DISPATCHED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.DependencyId == dependencyId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, string userId)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.DISPATCHED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.UserId == userId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetDispatchedUserInternalProceduresDatatableByInternalProcedureUser(DataTablesStructs.SentParameters sentParameters, string internalProcedureUserId, Guid dependencyId, string userId)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.DISPATCHED)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Where(x => x.DependencyId == dependencyId)
                .Where(x => x.UserId == userId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresByInternalProcedure(Guid internalProcedureId)
        {
            var result = await GetParentUserInternalProceduresByInternalProcedure(internalProcedureId, null, null, null);

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid? internalProcedureDependencyId)
        {
            var result = await GetParentUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId, null, null);

            return result;
        }

        //public async Task<DataTablesStructs.ReturnedData<object>> GetParentUserInternalProceduresByInternalProcedureDatatble(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Guid? dependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        //{
        //    var result = new List<UserInternalProcedure>();
        //    var internalProcedure = await _context.InternalProcedures
        //        .Where(x => x.Id == internalProcedureId)
        //        .FirstOrDefaultAsync();

        //    var internalProcedureId2 = internalProcedure?.InternalProcedureParentId;

        //    if (internalProcedureId2 != null)
        //    {
        //        do
        //        {
        //            internalProcedure = await _context.InternalProcedures
        //                .Where(x => x.Id == internalProcedureId2)
        //                .SelectInternalProcedure()
        //                .FirstOrDefaultAsync();

        //            if (internalProcedure != null)
        //            {
        //                var parentInternalProcedure = await _context.InternalProcedures
        //                    .Where(x => x.Id == internalProcedure.InternalProcedureParentId)
        //                    .FirstOrDefaultAsync();


        //                var userInternalProcedureAny = await _context.UserInternalProcedures.AnyAsync(x => x.InternalProcedureId == internalProcedureId);
        //                internalProcedureId2 = internalProcedure.InternalProcedureParentId ?? Guid.Empty;

        //                if (userInternalProcedureAny)
        //                {
        //                    var userInternalProcedures = _context.UserInternalProcedures.Where(x => x.InternalProcedureId == internalProcedure.Id);

        //                    if (dependencyId != null)
        //                    {
        //                        userInternalProcedures = userInternalProcedures.Where(x => x.InternalProcedure.DependencyId == dependencyId);
        //                    }

        //                    if (startCreatedAt != null)
        //                    {
        //                        userInternalProcedures = userInternalProcedures.Where(x => x.CreatedAt >= startCreatedAt);
        //                    }

        //                    if (endCreatedAt != null)
        //                    {
        //                        userInternalProcedures = userInternalProcedures.Where(x => x.CreatedAt <= endCreatedAt);
        //                    }

        //                    var userInternalProcedures2 = await userInternalProcedures
        //                        .SelectUserInternalProcedure()
        //                        .OrderByDescending(x => x.GeneratedId)
        //                        .ToListAsync();

        //                    query.InsertRange(0, userInternalProcedures2);
        //                }
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //        while (internalProcedureId2 != Guid.Empty);
        //    }

        //    Expression<Func<UserInternalProcedure, dynamic>> orderByPredicate = null;

        //    switch (sentParameters.OrderColumn)
        //    {
        //        default:
        //            orderByPredicate = (x) => x.ParsedCreatedAt;
        //            break;
        //    }

        //    var query = _context.InternalProcedures
        //        .Where(x => x.Id == internalProcedureId).AsNoTracking();

        //    switch (sentParameters.OrderColumn)
        //    {
        //        default:
        //            orderByPredicate = (x) => x.ParsedCreatedAt;
        //            break;
        //    }

        //    var query = _context.InternalProcedures
        //        .Where(x => x.Id == internalProcedureId).AsNoTracking();

        //                    var userInternalProcedures2 = await userInternalProcedures
        //                        .SelectUserInternalProcedure()
        //                        .OrderByDescending(x => x.GeneratedId)
        //                        .ToListAsync();

        //                    query.InsertRange(0, userInternalProcedures2);
        //                }
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //        while (internalProcedureId2 != Guid.Empty);
        //    }

        //    Expression<Func<UserInternalProcedure, dynamic>> orderByPredicate = null;

        //    switch (sentParameters.OrderColumn)
        //    {
        //        default:
        //            orderByPredicate = (x) => x.ParsedCreatedAt;
        //            break;
        //    }

        //    var query = _context.InternalProcedures
        //        .Where(x => x.Id == internalProcedureId).AsNoTracking();

        //    var data = await query
        //        .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
        //        .Skip(sentParameters.PagingFirstRecord)
        //        .Take(sentParameters.RecordsPerDraw)
        //        .Select(x => new
        //        {
        //            id =  x.Id,
        //            from = x.InternalProcedure.User.FullName,
        //            to = x.User.FullName,
        //            fromdependency = x.InternalProcedure.Dependency.Name,
        //            todependency = x.Dependency.Name,
        //            documenttype = x.InternalProcedure.DocumentType.Name,
        //            date = x.ParsedCreatedAt,
        //            status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.VALUES[x.Status],
        //            observation = x.Observation
        //        })
        //        .ToListAsync();

        //    var recordsFiltered = data.Count;

        //    var result = new DataTablesStructs.ReturnedData<object>
        //    {
        //        Data = data,
        //        DrawCounter = sentParameters.DrawCounter,
        //        RecordsFiltered = recordsFiltered,
        //        RecordsTotal = recordsTotal,
        //    };

        //}
        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresByInternalProcedure(Guid internalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            var result = await GetParentUserInternalProceduresByInternalProcedure(internalProcedureId, null, startCreatedAt, endCreatedAt);

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId)
        {
            List<UserInternalProcedure> result = new List<UserInternalProcedure>();
            var userInternalProcedureParents = await GetParentUserInternalProceduresByInternalProcedure(internalProcedureId);

            result.AddRange(userInternalProcedureParents);

            var result2 = result.Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);

            return result2;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Guid internalProcedureDependencyId)
        {
            List<UserInternalProcedure> result = new List<UserInternalProcedure>();
            var userInternalProcedureParents = await GetParentUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId);

            result.AddRange(userInternalProcedureParents);

            var result2 = result.Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);

            return result2;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Guid internalProcedureDependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            List<UserInternalProcedure> result = new List<UserInternalProcedure>();
            var userInternalProcedureParents = await GetParentUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId, startCreatedAt, endCreatedAt);

            result.AddRange(userInternalProcedureParents);

            var result2 = result.Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);

            return result2;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            List<UserInternalProcedure> result = new List<UserInternalProcedure>();
            var userInternalProcedureParents = await GetParentUserInternalProceduresByInternalProcedure(internalProcedureId, null, startCreatedAt, endCreatedAt);

            result.AddRange(userInternalProcedureParents);

            var result2 = result.Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);

            return result2;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresByUserDependencyUser(string userDependencyUserId, Guid dependencyId, string search = null, int? status = null)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserInternalProcedures.AsNoTracking();

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            var data = query
                .Where(x => x.DependencyId == dependencyId)
                .Where(x => x.Status != ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED && x.Status != ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED && x.Status != ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.NOT_APPLICABLE)
                .Where(x => dependencies.Contains(x.Dependency) && x.UserId == null || x.UserId == userDependencyUserId)
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserInternalProcedure();

            var result = await data.ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();

                result = result.Where(x => 
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Code) && x.InternalProcedure.Code.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && !string.IsNullOrEmpty(x.InternalProcedure.Subject) && x.InternalProcedure.Subject.ToUpper().Trim().Contains(search)) ||
                    (x.InternalProcedure != null && x.InternalProcedure.User != null && !string.IsNullOrEmpty(x.InternalProcedure.User.FullName) && x.InternalProcedure.User.FullName.ToUpper().Trim().Contains(search)) ||
                    (!string.IsNullOrEmpty(x.UserExternalProcedureCode) && x.UserExternalProcedureCode.Contains(search)) ||
                    (x.Dependency != null && !string.IsNullOrEmpty(x.Dependency.Name) && x.Dependency.Name.ToUpper().Trim().Contains(search))
                ).ToList();
            }

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresByUserDependencyUser(string userDependencyUserId, Guid internalProcedureDependencyId, Guid dependencyId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserInternalProcedures
                .Where(x => !x.IsDerived && x.DependencyId == dependencyId)
                .Where(x => dependencies.Contains(x.Dependency) && x.UserId != null ? x.UserId == userDependencyUserId : false)
                .Where(x => x.InternalProcedure.DependencyId == internalProcedureDependencyId)
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresByUserDependencyUser(string userDependencyUserId, string internalProcedureUserId, Guid dependencyId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserInternalProcedures
                .Where(x => !x.IsDerived && x.DependencyId == dependencyId)
                .Where(x => x.Status != ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED && x.Status != ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED)
                .Where(x => dependencies.Contains(x.Dependency) && x.UserId != null ? x.UserId == userDependencyUserId : false)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresByUserDependencyUser(string userDependencyUserId, Guid internalProcedureDependencyId, string internalProcedureUserId, Guid dependencyId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserInternalProcedures
                .Where(x => !x.IsDerived && x.DependencyId == dependencyId)
                .Where(x => x.Status != ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED && x.Status != ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED)
                .Where(x => dependencies.Contains(x.Dependency) && x.UserId != null ? x.UserId == userDependencyUserId : false)
                .Where(x => x.InternalProcedure.DependencyId == internalProcedureDependencyId)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, Guid dependencyId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserInternalProcedures
                .Where(x => !x.IsDerived && x.DependencyId == dependencyId)
                .Where(x => x.Status != ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED && x.Status != ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED)
                .Where(x => dependencies.Contains(x.Dependency) && x.UserId != null ? x.UserId == userDependencyUserId : false)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, Guid internalProcedureDependencyId, Guid dependencyId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserInternalProcedures
                .Where(x => !x.IsDerived && x.DependencyId == dependencyId)
                .Where(x => x.Status != ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED && x.Status != ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED)
                .Where(x => dependencies.Contains(x.Dependency) && x.UserId != null ? x.UserId == userDependencyUserId : false)
                .Where(x => x.InternalProcedure.DependencyId == internalProcedureDependencyId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, string internalProcedureUserId, Guid dependencyId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserInternalProcedures
                .Where(x => !x.IsDerived && x.DependencyId == dependencyId)
                .Where(x => x.Status != ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED && x.Status != ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED)
                .Where(x => dependencies.Contains(x.Dependency) && x.UserId != null ? x.UserId == userDependencyUserId : false)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRecievedUserInternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, Guid internalProcedureDependencyId, string internalProcedureUserId, Guid dependencyId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserInternalProcedures
                .Where(x => !x.IsDerived && x.DependencyId == dependencyId)
                .Where(x => x.Status != ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ARCHIVED && x.Status != ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.FINALIZED)
                .Where(x => dependencies.Contains(x.Dependency) && x.UserId != null ? x.UserId == userDependencyUserId : false)
                .Where(x => x.InternalProcedure.DependencyId == internalProcedureDependencyId)
                .Where(x => x.InternalProcedure.UserId == internalProcedureUserId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                //.GroupBy(x => new { x.DependencyId, x.InternalProcedure.SearchTree })
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        //public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresByInternalProcedure(Guid internalProcedureId)
        //{
        //    List<UserInternalProcedure> result = new List<UserInternalProcedure>();
        //    var userInternalProcedure = await _context.UserInternalProcedures
        //        .Where(x => x.InternalProcedureId == internalProcedureId)
        //        .SelectUserInternalProcedure()
        //        .FirstOrDefaultAsync();
        //    var userInternalProcedureChilds = await GetChildUserInternalProceduresByInternalProcedure(internalProcedureId);
        //    var userInternalProcedureParents = await GetParentUserInternalProceduresByInternalProcedure(internalProcedureId);

        //    result.AddRange(userInternalProcedureChilds);
        //    result.Add(userInternalProcedure);
        //    result.AddRange(userInternalProcedureParents);

        //    return result;
        //}

        //public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid internalProcedureDependencyId)
        //{
        //    List<UserInternalProcedure> result = new List<UserInternalProcedure>();
        //    var userInternalProcedure = await _context.UserInternalProcedures
        //        .Where(x => x.InternalProcedureId == internalProcedureId)
        //        .SelectUserInternalProcedure()
        //        .FirstOrDefaultAsync();
        //    var userInternalProcedureChilds = await GetChildUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId);
        //    var userInternalProcedureParents = await GetParentUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId);

        //    result.AddRange(userInternalProcedureChilds);
        //    result.Add(userInternalProcedure);
        //    result.AddRange(userInternalProcedureParents);

        //    return result;
        //}

        //public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid internalProcedureDependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        //{
        //    List<UserInternalProcedure> result = new List<UserInternalProcedure>();
        //    var userInternalProcedure = await _context.UserInternalProcedures
        //        .Where(x => x.InternalProcedureId == internalProcedureId)
        //        .SelectUserInternalProcedure()
        //        .FirstOrDefaultAsync();
        //    var userInternalProcedureChilds = await GetChildUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId, startCreatedAt, endCreatedAt);
        //    var userInternalProcedureParents = await GetParentUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId, startCreatedAt, endCreatedAt);

        //    result.AddRange(userInternalProcedureChilds);
        //    result.Add(userInternalProcedure);
        //    result.AddRange(userInternalProcedureParents);

        //    return result;
        //}

        public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresByInternalProcedure(Guid internalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt, Guid userProcedureId)
        {
            List<UserInternalProcedure> result = new List<UserInternalProcedure>();

            //var userInternalProcedure = await _context.UserInternalProcedures
            //    .Where(x => x.Id == userProcedureId)
            //    .SelectUserInternalProcedure()
            //    .FirstOrDefaultAsync();

            var userInternalProcedures = await _context.UserInternalProcedures
                .Where(x => x.InternalProcedureId == internalProcedureId)
                .SelectUserInternalProcedure()
                .ToListAsync();

            var userInternalProcedureChilds = await GetChildUserInternalProceduresByInternalProcedure(internalProcedureId, null, startCreatedAt, endCreatedAt);
            var userInternalProcedureParents = await GetParentUserInternalProceduresByInternalProcedure(internalProcedureId, null, startCreatedAt, endCreatedAt);

            result.AddRange(userInternalProcedureChilds);
            result.AddRange(userInternalProcedures);
            result.AddRange(userInternalProcedureParents);

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresByUserInternalProcedure(Guid userInternalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            List<UserInternalProcedure> result = new List<UserInternalProcedure>();

            var userInternalProcedure = await _context.UserInternalProcedures
                .Where(x => x.Id == userInternalProcedureId)
                .SelectUserInternalProcedure()
                .FirstOrDefaultAsync();

            //children

            //Current
            result.Add(userInternalProcedure);

            //parents

            return result;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId)
        {
            List<UserInternalProcedure> result = new List<UserInternalProcedure>();
            var userInternalProcedure = await _context.UserInternalProcedures
                .Where(x => x.InternalProcedureId == internalProcedureId)
                .SelectUserInternalProcedure()
                .FirstOrDefaultAsync();
            var userInternalProcedureChilds = await GetChildUserInternalProceduresByInternalProcedure(internalProcedureId);
            var userInternalProcedureParents = await GetParentUserInternalProceduresByInternalProcedure(internalProcedureId);

            result.AddRange(userInternalProcedureChilds);
            result.Add(userInternalProcedure);
            result.AddRange(userInternalProcedureParents);

            var result2 = result
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);

            return result2;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Guid internalProcedureDependencyId)
        {
            List<UserInternalProcedure> result = new List<UserInternalProcedure>();
            var userInternalProcedure = await _context.UserInternalProcedures
                .Where(x => x.InternalProcedureId == internalProcedureId)
                .SelectUserInternalProcedure()
                .FirstOrDefaultAsync();
            var userInternalProcedureChilds = await GetChildUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId);
            var userInternalProcedureParents = await GetParentUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId);

            result.AddRange(userInternalProcedureChilds);
            result.Add(userInternalProcedure);
            result.AddRange(userInternalProcedureParents);

            var result2 = result
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);

            return result2;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, Guid internalProcedureDependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            List<UserInternalProcedure> result = new List<UserInternalProcedure>();
            var userInternalProcedure = await _context.UserInternalProcedures
                .Where(x => x.InternalProcedureId == internalProcedureId)
                .SelectUserInternalProcedure()
                .FirstOrDefaultAsync();
            var userInternalProcedureChilds = await GetChildUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId, startCreatedAt, endCreatedAt);
            var userInternalProcedureParents = await GetParentUserInternalProceduresByInternalProcedure(internalProcedureId, internalProcedureDependencyId, startCreatedAt, endCreatedAt);

            result.AddRange(userInternalProcedureChilds);
            result.Add(userInternalProcedure);
            result.AddRange(userInternalProcedureParents);

            var result2 = result
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);

            return result2;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetRelatedUserInternalProceduresDatatableByInternalProcedure(DataTablesStructs.SentParameters sentParameters, Guid internalProcedureId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            List<UserInternalProcedure> result = new List<UserInternalProcedure>();
            var userInternalProcedure = await _context.UserInternalProcedures
                .Where(x => x.InternalProcedureId == internalProcedureId)
                .SelectUserInternalProcedure()
                .FirstOrDefaultAsync();
            var userInternalProcedureChilds = await GetChildUserInternalProceduresByInternalProcedure(internalProcedureId, null, startCreatedAt, endCreatedAt);
            var userInternalProcedureParents = await GetParentUserInternalProceduresByInternalProcedure(internalProcedureId, null, startCreatedAt, endCreatedAt);

            result.AddRange(userInternalProcedureChilds);
            result.Add(userInternalProcedure);
            result.AddRange(userInternalProcedureParents);

            var result2 = result
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);

            return result2;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProcedures()
        {
            var query = _context.UserInternalProcedures
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByDate(DateTime start, DateTime end)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.CreatedAt > start && x.CreatedAt < end)
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByDependency(Guid dependencyId, byte? status = null)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.DependencyId == dependencyId)
                .AsNoTracking();

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            query = query
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByEndDate(DateTime end)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.CreatedAt < end)
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByEndDate(DateTime end, int searchTree)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.CreatedAt <= end)
                .Where(x => x.InternalProcedure.SearchTree == searchTree)
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByInternalProcedureSearchTree(int searchTree)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.InternalProcedure.SearchTree == searchTree)
                .SelectUserInternalProcedure()
                .OrderByDescending(x => x.GeneratedId);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByStartDate(DateTime start)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.CreatedAt > start)
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetUserInternalProceduresByStartDate(DateTime start, int searchTree)
        {
            var query = _context.UserInternalProcedures
                .Where(x => x.CreatedAt >= start)
                .Where(x => x.InternalProcedure.SearchTree == searchTree)
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserInternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetUserInternalProcedureByDependencies()
        {
            var query = await _context.UserInternalProcedures
                .Include(x => x.Dependency)
                .Where(x => x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.DISPATCHED)
                .ToListAsync();

            var result = query
                .GroupBy(x => x.Dependency)
                .Take(5)
                .ToDictionary(x => x.Key.Name, x => x.Count());

            return result;
        }

        public async Task<object> GetUserInternalProcedureReportByMonthChart()
        {
            var today = DateTime.UtcNow;

            var query = await _context.UserInternalProcedures
                .Include(x => x.InternalProcedure.DocumentType)
                .Where(x => x.CreatedAt.Value.Month == today.Month && x.CreatedAt.Value.Year == today.Year)
                .ToListAsync();

            var result = query
                .GroupBy(x => x.InternalProcedure.DocumentType)
                .Select(x => new
                {
                    name = x.Key.Name,
                    y = x.Count()
                })
                .ToList();

            return result;
        }

        public async Task<Guid> GetInternalProcedureAnswer(Guid uid, string userId)
        {
            var userInternalProcedure = _context.UserInternalProcedures.Where(x => x.Id == uid).FirstOrDefault();

            var answer = await _context.InternalProcedures
                .Where(x=>
                x.DependencyId == userInternalProcedure.DependencyId &&
                x.InternalProcedureParentId == userInternalProcedure.InternalProcedureId && 
                x.AnswerType == ConstantHelpers.USER_INTERNAL_PROCEDURES.TYPE.ANSWER)
                .Select(y => y.Id)
                .FirstOrDefaultAsync();

            //var query = await _context.InternalProcedures.Where(x => x.InternalProcedureParentId == userInternalProcedure.InternalProcedureId && x.AnswerType == ConstantHelpers.USER_INTERNAL_PROCEDURES.TYPE.ANSWER).Select(y => y.Id).FirstOrDefaultAsync();
            
            //var query = await _context.UserInternalProcedures
            //    .Where(x => x.InternalProcedure.InternalProcedureParentId == iid && x.UserId == userId && !x.IsDerived).Select(y => y.InternalProcedureId).FirstOrDefaultAsync();

            return answer;
        }

        public async Task<UserInternalProcedure> GetByUserExternalProcedure(Guid userExternalProcedureId)
        {
            var userInternalProcedure = _context.UserInternalProcedures.Include(x => x.InternalProcedure).ThenInclude(x => x.UserExternalProcedures).Where(x => x.InternalProcedure.FromExternal).AsNoTracking();
            var data = await userInternalProcedure.Where(y => y.InternalProcedure.UserExternalProcedures.Any(e => e.Id == userExternalProcedureId)).FirstOrDefaultAsync();
            return data;
        }

        public async Task<IEnumerable<UserInternalProcedure>> GetParentUserInternalProceduresByInternalProcedure(Guid internalProcedureId, Guid? internalProcedureDependencyId, DateTime? startCreatedAt, DateTime? endCreatedAt)
        {
            var result = new List<UserInternalProcedure>();
            var internalProcedure = await _context.InternalProcedures
                .Where(x => x.Id == internalProcedureId)
                .FirstOrDefaultAsync();
            var internalProcedureId2 = internalProcedure?.InternalProcedureParentId;

            if (internalProcedureId2 != null)
            {
                do
                {
                    internalProcedure = await _context.InternalProcedures
                        .Where(x => x.Id == internalProcedureId2)
                        .SelectInternalProcedure()
                        .FirstOrDefaultAsync();

                    if (internalProcedure != null)
                    {
                        var parentInternalProcedure = await _context.InternalProcedures
                            .Where(x => x.Id == internalProcedure.InternalProcedureParentId)
                            .FirstOrDefaultAsync();


                        var userInternalProcedureAny = await _context.UserInternalProcedures.AnyAsync(x => x.InternalProcedureId == internalProcedureId);
                        internalProcedureId2 = internalProcedure.InternalProcedureParentId ?? Guid.Empty;

                        if (userInternalProcedureAny)
                        {
                            var userInternalProcedures = _context.UserInternalProcedures.Where(x => x.InternalProcedureId == internalProcedure.Id);

                            if (internalProcedureDependencyId != null)
                            {
                                userInternalProcedures = userInternalProcedures.Where(x => x.InternalProcedure.DependencyId == internalProcedureDependencyId);
                            }

                            if (startCreatedAt != null)
                            {
                                userInternalProcedures = userInternalProcedures.Where(x => x.CreatedAt >= startCreatedAt);
                            }

                            if (endCreatedAt != null)
                            {
                                userInternalProcedures = userInternalProcedures.Where(x => x.CreatedAt <= endCreatedAt);
                            }

                            var userInternalProcedures2 = await userInternalProcedures
                                .SelectUserInternalProcedure()
                                .OrderByDescending(x => x.GeneratedId)
                                .ToListAsync();

                            result.InsertRange(0, userInternalProcedures2);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                while (internalProcedureId2 != Guid.Empty);
            }

            return result;
        }

        //public async Task<DataTablesStructs<object>> 

        #endregion
    }
}
