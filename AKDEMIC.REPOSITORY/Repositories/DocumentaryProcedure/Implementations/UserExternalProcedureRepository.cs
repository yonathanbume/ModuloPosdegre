using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class UserExternalProcedureRepository : Repository<UserExternalProcedure>, IUserExternalProcedureRepository
    {
        public UserExternalProcedureRepository(AkdemicContext context) : base(context) { }

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

        private async Task<UserExternalProcedure> LastYearlyUserExternalProcedure(Guid? dependencyId = null)
        {
            var query = _context.UserExternalProcedures.AsQueryable();

            if (dependencyId != null)
            {
                query = query.Where(x => x.DependencyId == dependencyId);
            }

            query = query
                .Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Year == DateTime.UtcNow.Year)
                .OrderByDescending(x => x.Number)
                .AsQueryable();

            return await query.FirstOrDefaultAsync();
        }

        private async Task<DataTablesStructs.ReturnedData<UserExternalProcedure>> GetUserExternalProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Guid? externalUserId = null, string userDependencyUserId = null, Expression<Func<UserExternalProcedure, UserExternalProcedure>> selectPredicate = null, Expression<Func<UserExternalProcedure, dynamic>> orderByPredicate = null, Func<UserExternalProcedure, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.UserExternalProcedures.AsNoTracking();

            if (externalUserId != null)
            {
                query = query.Where(x => x.ExternalUserId == externalUserId);
            }

            if (userDependencyUserId != null)
            {
                var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
                query = query.Where(x => x.DependencyId == null || dependencies.Contains(x.Dependency));
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x => x.ExternalProcedure.Name.Contains(searchValue) || x.ExternalUser.FullName.Contains(searchValue) || x.ExternalUser.DocumentNumber.Contains(searchValue) || x.UserExternalProcedureRecords.Any(y => y.FullRecordNumber.ToLower().Contains(searchValue)));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(selectPredicate)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<UserExternalProcedure>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        private async Task<Select2Structs.ResponseParameters> GetUserExternalProceduresSelect2(Select2Structs.RequestParameters requestParameters, Guid? dependencyId = null, Expression<Func<UserExternalProcedure, Select2Structs.Result>> selectPredicate = null, Func<UserExternalProcedure, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.UserExternalProcedures.AsNoTracking();

            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            var results = await query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        #endregion

        #region PUBLIC

        public async Task<UserExternalProcedure> GetWithIncludes(Guid id)
        {
            var query = _context.UserExternalProcedures
                .Include(x => x.ExternalProcedure.Concept)
                .Include(x => x.ExternalUser)
                .Include(x => x.Payment)
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<UserExternalProcedure> GetUserExternalProcedure(Guid id)
        {
            var query = _context.UserExternalProcedures
                .Where(x => x.Id == id)
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserExternalProcedure();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetUserExternalProcedures()
        {
            var query = _context.UserExternalProcedures
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserExternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresByDependency(Guid dependencyId)
        {
            var query = _context.UserExternalProcedures
                .Where(x => x.DependencyId == dependencyId)
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserExternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresByExternalUser(Guid externalUserId)
        {
            var query = _context.UserExternalProcedures
                .Where(x => x.ExternalUserId == externalUserId)
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserExternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresByExternalUser(Guid externalUserId, string userDependencyUserId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserExternalProcedures
                .Where(x => x.ExternalUserId == externalUserId)
                .Where(x => x.DependencyId == null || dependencies.Contains(x.Dependency))
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserExternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresBySearchValue(string searchValue)
        {
            var query = _context.UserExternalProcedures
                .Where(x => x.ExternalUser.DocumentNumber == searchValue || x.UserExternalProcedureRecords.Any(y => y.FullRecordNumber == searchValue && y.UserExternalProcedureId == x.Id))
                .SelectUserExternalProcedure();

            var result = (IEnumerable<UserExternalProcedure>)await query.ToListAsync();

            if (!result.Any())
            {
                result = query
                    .AsEnumerable()
                    .Where(x => x.Code == searchValue);
            }

            return result;
        }

        public async Task<IEnumerable<object>> GetUserExternalInternalProcedure(string search, int type)
        {
            var query = _context.UserExternalProcedures.Include(x => x.Dependency).Include(x => x.ExternalProcedure).AsNoTracking();

            if(type == 2)
            {
                query = query.Where(x => x.ExternalUser.DocumentNumber == search);
            }
            else
            {
                query = query.Where(x => x.UserExternalProcedureRecords.Any(y => y.FullRecordNumber == search));
            }

            var userInternalProcedure =  _context.UserInternalProcedures.Include(x => x.InternalProcedure).ThenInclude(x => x.UserExternalProcedures).Where(x => x.InternalProcedure.FromExternal).AsNoTracking();

            var result = (IEnumerable<UserExternalProcedure>)await query.ToListAsync();

            if (!result.Any())
            {
                result = query
                    .AsEnumerable()
                    .Where(x => x.Code == search);
            }

            var data = result.Select(x => new 
            {
                id = x.Id,
                createdAt = x.CreatedAt.ToLocalDateTimeFormat(),
                externalProcedureName = x.ExternalProcedure.Name,
                dependencyName = x.Dependency.Name,
                statusInternal = userInternalProcedure.Any(y => y.InternalProcedure.UserExternalProcedures.Any(e => e.Id == x.Id)) ? 
                                 userInternalProcedure.Where(y => y.InternalProcedure.UserExternalProcedures.Any(e => e.Id == x.Id)).Select(u => u.Status).FirstOrDefault() 
                                 : x.Status,
                status = x.Status
            }).ToList();

            return data;
        }
        public async Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresByUserDependencyUser(string userDependencyUserId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserExternalProcedures
                .Where(x => x.DependencyId == null || dependencies.Contains(x.Dependency))
                .OrderByDescending(x => x.GeneratedId)
                .SelectUserExternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<UserExternalProcedure>> GetUserExternalProceduresDatatableByExternalUser(DataTablesStructs.SentParameters sentParameters, Guid externalUserId, string searchValue = null)
        {
            Expression<Func<UserExternalProcedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ExternalProcedure.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Code);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.Status);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.ExternalUser.FullName);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.CreatedAt);

                    break;
                default:
                    orderByPredicate = ((x) => x.ExternalProcedure.Name);

                    break;
            }

            return await GetUserExternalProceduresDatatable(sentParameters, externalUserId, null, ExpressionHelpers.SelectUserExternalProcedure(), orderByPredicate, (x) => new[] { x.Code, x.ExternalProcedure?.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserExternalProcedure>> GetUserExternalProceduresDatatableByExternalUser(DataTablesStructs.SentParameters sentParameters, Guid externalUserId, string userDependencyUserId, string searchValue = null)
        {
            Expression<Func<UserExternalProcedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ExternalProcedure.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Code);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.Status);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.ExternalUser.FullName);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.CreatedAt);

                    break;
                default:
                    orderByPredicate = ((x) => x.ExternalProcedure.Name);

                    break;
            }

            return await GetUserExternalProceduresDatatable(sentParameters, externalUserId, userDependencyUserId, ExpressionHelpers.SelectUserExternalProcedure(), orderByPredicate, (x) => new[] { x.Code, x.ExternalProcedure.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserExternalProcedure>> GetUserExternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, string searchValue = null)
        {
            Expression<Func<UserExternalProcedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ExternalProcedure.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Code);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.Status);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.ExternalUser.FullName);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.CreatedAt);

                    break;
                default:
                    orderByPredicate = ((x) => x.ExternalProcedure.Name);

                    break;
            }

            return await GetUserExternalProceduresDatatable(sentParameters, null, userDependencyUserId, ExpressionHelpers.SelectUserExternalProcedure(), orderByPredicate, (x) => new[] { x.Code, x.ExternalProcedure?.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetUserExternalProceduresSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetUserExternalProceduresSelect2(requestParameters, null, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.ExternalProcedure.Name
            }, (x) => new[] { x.ExternalProcedure.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetUserExternalProceduresSelect2ByDependency(Select2Structs.RequestParameters requestParameters, Guid dependencyId, string searchValue = null)
        {
            return await GetUserExternalProceduresSelect2(requestParameters, dependencyId, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.ExternalProcedure.Name
            }, (x) => new[] { x.ExternalProcedure.Name }, searchValue);
        }

        public async Task InsertUserExternalProcedure(UserExternalProcedure userExternalProcedure)
        {
            var userExternalProcedure2 = await LastYearlyUserExternalProcedure(userExternalProcedure.DependencyId);
            var number = userExternalProcedure2 != null ? userExternalProcedure2.Number : 0;

            userExternalProcedure.Number = number + 1;

            await Insert(userExternalProcedure);
        }

        public async Task<UserExternalProcedure> GetByInternalProcedure(Guid internalProcedureId)
            => await _context.UserExternalProcedures.Where(x => x.InternalProcedureId == internalProcedureId).SelectUserExternalProcedure().FirstOrDefaultAsync();
        #endregion





        /*
        public async Task<IEnumerable<UserExternalProcedure>> GetUserExternalProceduresBySearchValue2(string searchValue)
        {
            var query = _context.UserExternalProcedures
                .Where(x => x.ExternalUser.Dni == searchValue)
                .SelectUserExternalProcedure()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetActiveUserExternalProcedureByExternalUser(Guid externalUserId)
        {
            var query = _context.UserExternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.REQUESTED || x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.IN_PROCESS)
                .Where(x => x.ExternalUserId == externalUserId)
                .SelectUserExternalProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetActiveUserExternalProceduresByUserDependencyUser(string userDependencyUserId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserExternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.REQUESTED || x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.IN_PROCESS)
                .Where(x => x.DependencyId == null || dependencies.Contains(x.Dependency))
                .SelectUserExternalProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetActiveUserExternalProceduresByUserDependencyUser(string userDependencyUserId, Guid externalUserId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserExternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.REQUESTED || x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.IN_PROCESS)
                .Where(x => x.DependencyId == null || dependencies.Contains(x.Dependency))
                .Where(x => x.ExternalUserId == externalUserId)
                .SelectUserExternalProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetActiveUserExternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, Guid externalUserId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserExternalProcedures
                .Where(x => x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.REQUESTED || x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.IN_PROCESS)
                .Where(x => x.DependencyId == null || dependencies.Contains(x.Dependency))
                .Where(x => x.ExternalUserId == externalUserId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserExternalProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetHistoricUserExternalProceduresByExternalUser(Guid externalUserId)
        {
            var query = _context.UserExternalProcedures
                .Where(x => x.Status != ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.REQUESTED && x.Status != ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.IN_PROCESS)
                .Where(x => x.ExternalUserId == externalUserId)
                .SelectUserExternalProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetHistoricUserExternalProceduresByUserDependencyUser(string userDependencyUserId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserExternalProcedures
                .Where(x => x.Status != ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.REQUESTED && x.Status != ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.IN_PROCESS)
                .Where(x => x.DependencyId == null || dependencies.Contains(x.Dependency))
                .SelectUserExternalProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetHistoricUserExternalProceduresByUserDependencyUser(string userDependencyUserId, Guid externalUserId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserExternalProcedures
                .Where(x => x.Status != ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.REQUESTED && x.Status != ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.IN_PROCESS)
                .Where(x => x.DependencyId == null || dependencies.Contains(x.Dependency))
                .Where(x => x.ExternalUserId == externalUserId)
                .SelectUserExternalProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetHistoricUserExternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameter, string userDependencyUserId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserExternalProcedures
                .Where(x => x.Status != ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.REQUESTED && x.Status != ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.IN_PROCESS)
                .Where(x => x.DependencyId == null || dependencies.Contains(x.Dependency))
                .SelectUserExternalProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedure>> GetHistoricUserExternalProceduresDatatableByUserDependencyUser(DataTablesStructs.SentParameters sentParameters, string userDependencyUserId, Guid externalUserId)
        {
            var dependencies = await GetDependenciesByUserDependencyUser(userDependencyUserId);
            var query = _context.UserExternalProcedures
                .Where(x => x.Status != ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.REQUESTED && x.Status != ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.IN_PROCESS)
                .Where(x => x.DependencyId == null || dependencies.Contains(x.Dependency))
                .Where(x => x.ExternalUserId == externalUserId)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectUserExternalProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }





















        // --

        public async Task<Payment> GetUserExternalProcedurePayment(Guid userExternalProcedureId)
        {
            var userExternalProcedure = await _context.UserExternalProcedures.Include(x => x.Payment)
                .FirstOrDefaultAsync(x => x.Id == userExternalProcedureId);
            return userExternalProcedure.Payment;
        }

        public async Task<bool> IsUserExternalProcedureAllowedByUserDependency(Guid userExternalProcedureId, string userId)
        {
            var dependencies = await _context.UserDependencies
                .Where(x => x.UserId == userId)
                .Select(x => x.Dependency)
                .ToListAsync();

            return await _context.UserExternalProcedures.AnyAsync(x =>
                x.Id == userExternalProcedureId &&
                dependencies.Contains(x.Dependency));
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetActiveUserExternalProceduresByUserDependency(string userId, DataTablesStructs.SentParameters sentParameters)
        {
            var dependencies = await _context.UserDependencies
                .Where(x => x.UserId == userId)
                .Select(x => x.Dependency)
                .ToListAsync();

            var query = _context.UserExternalProcedures
                .Where(x => (string.IsNullOrWhiteSpace(sentParameters.SearchValue) || 
                             x.GeneratedId.ToString().Contains(sentParameters.SearchValue) ||
                             x.ExternalProcedure.Name.Contains(sentParameters.SearchValue)) && 
                            (x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.REQUESTED || 
                            x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.IN_PROCESS) &&
                            dependencies.Contains(x.Dependency))
                .AsQueryable();

            var records = await query.CountAsync();

            query = GetSortedUserExternalProcedures(query, sentParameters);

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .SelectUserExternalProcedure()
                .ToListAsync();

            return new Tuple<int, List<UserExternalProcedure>>(records, pagedList);
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetActiveUserExternalProceduresByUserDependency(string userId, Guid externalUserId, DataTablesStructs.SentParameters sentParameters)
        {
            var dependencies = await _context.UserDependencies
                .Where(x => x.UserId == userId)
                .Select(x => x.Dependency)
                .ToListAsync();

            var query = _context.UserExternalProcedures
                .Where(x => (string.IsNullOrWhiteSpace(sentParameters.SearchValue) ||
                             x.GeneratedId.ToString().Contains(sentParameters.SearchValue) ||
                             x.ExternalProcedure.Name.Contains(sentParameters.SearchValue)) &&
                            (x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.REQUESTED ||
                             x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.IN_PROCESS) &&
                            x.ExternalUserId == externalUserId &&
                            dependencies.Contains(x.Dependency))
                .AsQueryable();

            var records = await query.CountAsync();

            query = GetSortedUserExternalProcedures(query, sentParameters);

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .SelectUserExternalProcedure()
                .ToListAsync();

            return new Tuple<int, List<UserExternalProcedure>>(records, pagedList);
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetActiveUserExternalProceduresByUserDependency(string userId, DateTime? beginDate, DateTime? endDate, DataTablesStructs.SentParameters sentParameters)
        {
            var dependencies = await _context.UserDependencies
                .Where(x => x.UserId == userId)
                .Select(x => x.Dependency)
                .ToListAsync();

            var query = _context.UserExternalProcedures
                .Where(x => (string.IsNullOrWhiteSpace(sentParameters.SearchValue) ||
                             x.GeneratedId.ToString().Contains(sentParameters.SearchValue) ||
                             x.ExternalProcedure.Name.Contains(sentParameters.SearchValue)) &&
                            (x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.REQUESTED ||
                             x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.IN_PROCESS) &&
                            dependencies.Contains(x.Dependency))
                .AsQueryable();

            if (beginDate.HasValue && endDate.HasValue)
                query = query.Where(x => x.CreatedAt.Value.Date >= beginDate.Value.Date && x.CreatedAt.Value.Date <= endDate.Value.Date);
            else if (beginDate.HasValue)
                query = query.Where(x => x.CreatedAt.Value.Date >= beginDate.Value.Date);
            else if (endDate.HasValue)
                query = query.Where(x => x.CreatedAt.Value.Date <= endDate.Value.Date);

            var records = await query.CountAsync();

            query = GetSortedUserExternalProcedures(query, sentParameters);

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .SelectUserExternalProcedure()
                .ToListAsync();

            return new Tuple<int, List<UserExternalProcedure>>(records, pagedList);
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetActiveUserExternalProceduresByUserDependency(string userId, DateTime? beginDate, DateTime? endDate, Guid externalUserId, DataTablesStructs.SentParameters sentParameters)
        {
            var dependencies = await _context.UserDependencies
                .Where(x => x.UserId == userId)
                .Select(x => x.Dependency)
                .ToListAsync();

            var query = _context.UserExternalProcedures
                .Where(x => (string.IsNullOrWhiteSpace(sentParameters.SearchValue) ||
                             x.GeneratedId.ToString().Contains(sentParameters.SearchValue) ||
                             x.ExternalProcedure.Name.Contains(sentParameters.SearchValue)) &&
                            (x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.REQUESTED ||
                             x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.IN_PROCESS) &&
                            x.ExternalUserId == externalUserId &&
                            dependencies.Contains(x.Dependency))
                .AsQueryable();

            if (beginDate.HasValue && endDate.HasValue)
                query = query.Where(x => x.CreatedAt.Value.Date >= beginDate.Value.Date && x.CreatedAt.Value.Date <= endDate.Value.Date);
            else if (beginDate.HasValue)
                query = query.Where(x => x.CreatedAt.Value.Date >= beginDate.Value.Date);
            else if (endDate.HasValue)
                query = query.Where(x => x.CreatedAt.Value.Date <= endDate.Value.Date);

            var records = await query.CountAsync();

            query = GetSortedUserExternalProcedures(query, sentParameters);

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .SelectUserExternalProcedure()
                .ToListAsync();

            return new Tuple<int, List<UserExternalProcedure>>(records, pagedList);
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetHistoricUserExternalProceduresUserDependency(string userId, DataTablesStructs.SentParameters sentParameters)
        {
            var dependencies = await _context.UserDependencies
                .Where(x => x.UserId == userId)
                .Select(x => x.Dependency)
                .ToListAsync();

            var query = _context.UserExternalProcedures
                .Where(x => (string.IsNullOrWhiteSpace(sentParameters.SearchValue) ||
                             x.GeneratedId.ToString().Contains(sentParameters.SearchValue) ||
                             x.ExternalProcedure.Name.Contains(sentParameters.SearchValue)) &&
                            (x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.ACCEPTED ||
                            x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.NOT_APPLICABLE) &&
                            dependencies.Contains(x.Dependency))
                .AsQueryable();

            var records = await query.CountAsync();

            query = GetSortedUserExternalProcedures(query, sentParameters);

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .SelectUserExternalProcedure()
                .ToListAsync();

            return new Tuple<int, List<UserExternalProcedure>>(records, pagedList);
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetHistoricUserExternalProceduresUserDependency(string userId, Guid externalUserId, DataTablesStructs.SentParameters sentParameters)
        {
            var dependencies = await _context.UserDependencies
                .Where(x => x.UserId == userId)
                .Select(x => x.Dependency)
                .ToListAsync();

            var query = _context.UserExternalProcedures
                .Where(x => (string.IsNullOrWhiteSpace(sentParameters.SearchValue) ||
                             x.GeneratedId.ToString().Contains(sentParameters.SearchValue) ||
                             x.ExternalProcedure.Name.Contains(sentParameters.SearchValue)) &&
                            (x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.ACCEPTED ||
                             x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.NOT_APPLICABLE) &&
                            x.ExternalUserId == externalUserId &&
                            dependencies.Contains(x.Dependency))
                .AsQueryable();

            var records = await query.CountAsync();

            query = GetSortedUserExternalProcedures(query, sentParameters);

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .SelectUserExternalProcedure()
                .ToListAsync();

            return new Tuple<int, List<UserExternalProcedure>>(records, pagedList);
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetHistoricUserExternalProceduresUserDependency(string userId, DateTime? beginDate, DateTime? endDate, DataTablesStructs.SentParameters sentParameters)
        {
            var dependencies = await _context.UserDependencies
                .Where(x => x.UserId == userId)
                .Select(x => x.Dependency)
                .ToListAsync();

            var query = _context.UserExternalProcedures
                .Where(x => (string.IsNullOrWhiteSpace(sentParameters.SearchValue) ||
                             x.GeneratedId.ToString().Contains(sentParameters.SearchValue) ||
                             x.ExternalProcedure.Name.Contains(sentParameters.SearchValue)) &&
                            (x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.ACCEPTED ||
                             x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.NOT_APPLICABLE) &&
                            dependencies.Contains(x.Dependency))
                .AsQueryable();

            if (beginDate.HasValue && endDate.HasValue)
                query = query.Where(x => x.CreatedAt.Value.Date >= beginDate.Value.Date && x.CreatedAt.Value.Date <= endDate.Value.Date);
            else if (beginDate.HasValue)
                query = query.Where(x => x.CreatedAt.Value.Date >= beginDate.Value.Date);
            else if (endDate.HasValue)
                query = query.Where(x => x.CreatedAt.Value.Date <= endDate.Value.Date);

            var records = await query.CountAsync();

            query = GetSortedUserExternalProcedures(query, sentParameters);

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .SelectUserExternalProcedure()
                .ToListAsync();

            return new Tuple<int, List<UserExternalProcedure>>(records, pagedList);
        }

        public async Task<Tuple<int, List<UserExternalProcedure>>> GetHistoricUserExternalProceduresUserDependency(string userId, DateTime? beginDate, DateTime? endDate, Guid externalUserId, DataTablesStructs.SentParameters sentParameters)
        {
            var dependencies = await _context.UserDependencies
                .Where(x => x.UserId == userId)
                .Select(x => x.Dependency)
                .ToListAsync();

            var query = _context.UserExternalProcedures
                .Where(x => (string.IsNullOrWhiteSpace(sentParameters.SearchValue) ||
                             x.GeneratedId.ToString().Contains(sentParameters.SearchValue) ||
                             x.ExternalProcedure.Name.Contains(sentParameters.SearchValue)) &&
                            (x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.ACCEPTED ||
                             x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.NOT_APPLICABLE) &&
                            x.ExternalUserId == externalUserId &&
                            dependencies.Contains(x.Dependency))
                .AsQueryable();

            if (beginDate.HasValue && endDate.HasValue)
                query = query.Where(x => x.CreatedAt.Value.Date >= beginDate.Value.Date && x.CreatedAt.Value.Date <= endDate.Value.Date);
            else if (beginDate.HasValue)
                query = query.Where(x => x.CreatedAt.Value.Date >= beginDate.Value.Date);
            else if (endDate.HasValue)
                query = query.Where(x => x.CreatedAt.Value.Date <= endDate.Value.Date);

            var records = await query.CountAsync();

            query = GetSortedUserExternalProcedures(query, sentParameters);

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .SelectUserExternalProcedure()
                .ToListAsync();

            return new Tuple<int, List<UserExternalProcedure>>(records, pagedList);
        }

        public async Task<List<UserExternalProcedure>> GetUserExternalProceduresBySearchValue(string searchValue)
        {
            searchValue = searchValue?.ToLower();

            var query = _context.UserExternalProcedures
                .Where(x => string.IsNullOrWhiteSpace(searchValue) ||
                            x.GeneratedId.ToString().Equals(searchValue) ||
                            x.ExternalUser.Dni.Equals(searchValue))
                .SelectUserExternalProcedure()
                .OrderBy(x => x.CreatedAt)
                .AsQueryable();

            return await query.ToListAsync();
        }*/

        public async Task<List<UserExternalProcedure>> GetUserExternalProceduresBySearchValue(Guid externalUserId, string searchValue)
        {
            if (!string.IsNullOrWhiteSpace(searchValue))
                searchValue = searchValue.ToLower();

            var query = _context.UserExternalProcedures
                .Where(x => (string.IsNullOrWhiteSpace(searchValue) ||
                            x.GeneratedId.ToString().Equals(searchValue) ||
                             x.ExternalProcedure.Name.Contains(searchValue)) &&
                            x.ExternalUserId == externalUserId &&
                            (x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.REQUESTED ||
                             x.Status == ConstantHelpers.USER_EXTERNAL_PROCEDURES.STATUS.IN_PROCESS))
                .SelectUserExternalProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }
        /*
        private static IQueryable<UserExternalProcedure> GetSortedUserExternalProcedures(IQueryable<UserExternalProcedure> query, DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.GeneratedId) : query.OrderBy(q => q.GeneratedId);
                case "1":
                    return sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.ExternalProcedure.Name) : query.OrderBy(q => q.ExternalProcedure.Name);
                case "2":
                    return sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.ExternalUser.FullName) : query.OrderBy(q => q.ExternalUser.FullName);
                case "3":
                    return sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Status) : query.OrderBy(q => q.Status);
                case "4":
                    return sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Dependency.Name) : query.OrderBy(q => q.Dependency.Name);
                case "5":
                    return sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.ExternalProcedure.CreatedAt) : query.OrderBy(q => q.ExternalProcedure.CreatedAt);
                default:
                    return sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.GeneratedId) : query.OrderBy(q => q.GeneratedId);
            }
        }
        */
    }
}
