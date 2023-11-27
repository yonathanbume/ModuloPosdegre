using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{

    public class ExternalUserRepository : Repository<ExternalUser>, IExternalUserRepository
    {
        public ExternalUserRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<ExternalUser>> GetExternalUsersDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<ExternalUser, ExternalUser>> selectPredicate = null, Expression<Func<ExternalUser, dynamic>> orderByPredicate = null, Func<ExternalUser, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.ExternalUsers.AsNoTracking();
            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<ExternalUser>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        private async Task<Select2Structs.ResponseParameters> GetExternalUsersSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<ExternalUser, Select2Structs.Result>> selectPredicate = null, Func<ExternalUser, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.ExternalUsers.AsNoTracking();
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

        private async Task<Select2Structs.ResponseParameters> GetUserExternalProcedureExternalUsersSelect2(Select2Structs.RequestParameters requestParameters, string userId, Expression<Func<ExternalUser, Select2Structs.Result>> selectPredicate = null, Func<ExternalUser, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.ExternalUsers
                .Where(x => _context.UserExternalProcedures.Any(y => y.ExternalUserId == x.Id))
                .AsNoTracking();

            if (userId != null)
            {
                query = query.Where(x => _context.UserExternalProcedures.Any(y => _context.UserDependencies.Any(z => z.DependencyId == y.DependencyId && z.UserId == userId) && y.ExternalUserId == x.Id));
            }

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

        public async Task<bool> AnyExternalUserByDni(string dni)
        {
            var query = _context.ExternalUsers.Where(x => x.DocumentNumber == dni);

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<ExternalUser>> GetExternalUsers()
        {
            var query = _context.ExternalUsers.SelectExternalUser();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExternalUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, DateTime? startDateCreatedAt = null, DateTime? endDateCreatedAt = null, int? documentType = null, bool onlyPublicSector = false)
        {
            Expression<Func<ExternalUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.FullName;
                    break;
                //case "1":
                //    orderByPredicate = (x) => x.DocumentType;
                //    break;
                case "1":
                    orderByPredicate = (x) => x.DocumentNumber;
                    break;
                case "2":
                    orderByPredicate = (x) => x.BusinessName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.PhoneNumber;

                    break;
                case "4":
                    orderByPredicate = (x) => x.Email;
                    break;
                default:
                    break;
            }

            var query = _context.ExternalUsers
              .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.FullName.ToUpper().Contains(searchValue.ToUpper())
                || x.DocumentNumber.ToUpper().Contains(searchValue.ToUpper()));

            if (startDateCreatedAt.HasValue)
                query = query.Where(x => x.CreatedAt >= startDateCreatedAt.Value.ToUtcDateTime());

            if (endDateCreatedAt.HasValue)
                query = query.Where(x => x.CreatedAt <= endDateCreatedAt.Value.ToUtcDateTime());

            if (documentType.HasValue && documentType > 0)
            {
                query = query.Where(x => x.DocumentType == documentType);

                if (documentType == ConstantHelpers.DOCUMENT_TYPES.RUC && onlyPublicSector)
                    query = query.Where(x => x.IsPublicSector);
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    CreatedAt = x.CreatedAt.ToLocalDateFormat(),
                    id = x.Id,
                    fullname = x.FullName,
                    name = x.Name,
                    paternalSurname = x.PaternalSurname,
                    maternalSurname = x.MaternalSurname,
                    documentType = x.DocumentType,
                    document = x.DocumentNumber,
                    phone = x.PhoneNumber,
                    email = x.Email,
                    birthDate = x.BirthDate.HasValue ? x.BirthDate.ToLocalDateFormat() : "",
                    bussiness = x.BusinessName,
                    workPosition = x.WorkPosition,
                    hasProcedures = x.UserExternalProcedures.Any(),
                    address = x.Address,
                    isPublicSector = x.IsPublicSector
                })
                .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<Select2Structs.ResponseParameters> GetExternalUsersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetExternalUsersSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.FullName
            }, (x) => new[] { x.DocumentNumber, x.FullName }, searchValue);
        }

        public async Task<object> GetExternalUsersByTermSelect2(string term, byte? documentType = null)
        {
            var query = _context.ExternalUsers.AsNoTracking();

            if (documentType.HasValue)
                query = query.Where(x => x.DocumentType == documentType);

            var users = await query
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.FullName,
                    x.BusinessName
                }, term).ToListAsync();

            var data = users
               .Select(x => new
               {
                   id = x.Id,
                   text = $"{x.DocumentNumber} - {(!string.IsNullOrEmpty(x.BusinessName) ? x.BusinessName : x.FullName)}"
               })
               .ToList();

            return data;
        }

        public Select2Structs.ResponseParameters GetReniecExternalUsersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return new Select2Structs.ResponseParameters();
        }

        public async Task<Select2Structs.ResponseParameters> GetUserExternalProcedureExternalUsersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetUserExternalProcedureExternalUsersSelect2(requestParameters, null, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.FullName
            }, (x) => new[] { x.DocumentNumber, x.FullName }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetUserExternalProcedureExternalUsersSelect2(Select2Structs.RequestParameters requestParameters, string userId, string searchValue = null)
        {
            return await GetUserExternalProcedureExternalUsersSelect2(requestParameters, userId, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.FullName
            }, (x) => new[] { x.DocumentNumber, x.FullName }, searchValue);
        }

        #endregion

        public async Task<bool> HasAnyUserExternalProcedure(Guid externalUserId)
        {
            var externalProcedures = await _context.UserExternalProcedures.AnyAsync(x => x.ExternalUserId == externalUserId);
            var payments = await _context.Payments.AnyAsync(x => x.ExternalUserId == externalUserId);
            return externalProcedures || payments;
        }

        public async Task<bool> IsDniDuplicated(string dni)
        {
            return await _context.ExternalUsers.AnyAsync(x => x.DocumentNumber == dni);
        }

        public async Task<ExternalUser> GetExternalUserByUserId(string userId)
            => await _context.ExternalUsers.Where(x => x.UserId == userId).FirstOrDefaultAsync();

        public async Task<bool> IsDniDuplicated(string dni, Guid externalUserId)
        {
            return await _context.ExternalUsers.AnyAsync(x => x.Id != externalUserId && x.DocumentNumber == dni);
        }

        public async Task<Tuple<int, List<ExternalUser>>> GetExternalUsers(DataTablesStructs.SentParameters sentParameters)
        {
            var query = _context.ExternalUsers
                .Where(x => string.IsNullOrWhiteSpace(sentParameters.SearchValue) || x.Name.Contains(sentParameters.SearchValue) ||
                            x.PaternalSurname.Contains(sentParameters.SearchValue) || x.MaternalSurname.Contains(sentParameters.SearchValue) ||
                            x.DocumentNumber.Contains(sentParameters.SearchValue))
                .AsQueryable();

            var records = await query.CountAsync();

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.PaternalSurname) : query.OrderBy(q => q.PaternalSurname);
                    break;
                case "1":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.DocumentNumber) : query.OrderBy(q => q.DocumentNumber);
                    break;
                case "2":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.WorkPosition) : query.OrderBy(q => q.WorkPosition);
                    break;
                case "3":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.BirthDate) : query.OrderBy(q => q.BirthDate);
                    break;
                case "4":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.PhoneNumber) : query.OrderBy(q => q.PhoneNumber);
                    break;
                default:
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.PaternalSurname) : query.OrderBy(q => q.PaternalSurname);
                    break;
            }

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .Select(x => new ExternalUser
                {
                    Id = x.Id,
                    Name = x.Name,
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname,
                    DocumentNumber = x.DocumentNumber,
                    PhoneNumber = x.PhoneNumber,
                    WorkPosition = x.WorkPosition,
                    BirthDate = x.BirthDate,
                    HasRelatedUserExternalProcedures = x.UserExternalProcedures.Any(),
                    FullName = x.FullName
                })
                .AsNoTracking()
                .ToListAsync();

            return new Tuple<int, List<ExternalUser>>(records, pagedList);
        }

        public async Task<List<ExternalUser>> GetExternalUsersBySearchValue(string searchValue)
        {
            if (!string.IsNullOrWhiteSpace(searchValue))
                searchValue = searchValue.ToLower();

            var query = _context.ExternalUsers
                .Where(x => string.IsNullOrWhiteSpace(searchValue) ||
                            string.Concat(string.IsNullOrWhiteSpace(x.PaternalSurname) ? "" : x.PaternalSurname, " ", string.IsNullOrWhiteSpace(x.MaternalSurname) ? "" : x.MaternalSurname, " ", x.Name).ToLower().Contains(searchValue) ||
                            x.DocumentNumber.Contains(searchValue))
                .Select(x => new ExternalUser
                {
                    Id = x.Id,
                    DocumentNumber = x.DocumentNumber,
                    Name = x.Name,
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname
                })
                .OrderBy(x => x.PaternalSurname)
                .AsNoTracking()
                .AsQueryable();

            return await query.ToListAsync();


        }
        public async Task<object> GetExternalUsersOrStudentBySearchValue(string searchValue, int type)
        {
            if (type == ConstantHelpers.SEARCH_USER.STUDENT)
            {

                var query = _context.Students.AsNoTracking();

                if (!string.IsNullOrEmpty(searchValue))
                    query = query
                    .Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                            || x.User.Dni.ToUpper().Contains(searchValue.ToUpper()));

                query = query
                    .AsQueryable();

                var data = await query
                    .Select(x => new
                    {
                        id = x.Id,
                        documentNumber = x.User.Dni,
                        fullName = x.User.FullName,
                        text = $"DNI: {x.User.Dni} - {x.User.FullName}"
                    })
                    .OrderBy(x => x.fullName)
                    .ToListAsync();

                return data;
            }
            else
            {
                var query = _context.ExternalUsers.AsNoTracking();

                if (!string.IsNullOrEmpty(searchValue))
                    query = query
                    .Where(x => x.FullName.ToUpper().Contains(searchValue.ToUpper())
                            || x.DocumentNumber.ToUpper().Contains(searchValue.ToUpper()));

                query = query
                    .AsQueryable();

                var data = await query
                    .Select(x => new
                    {
                        id = x.Id,
                        documentNumber = x.DocumentNumber,
                        fullName = x.FullName,
                        text = $"DNI: {x.DocumentNumber} - {x.FullName}"
                    })
                    .OrderBy(x => x.fullName)
                    .ToListAsync();

                return data;

            }

        }

        public async Task<ExternalUser> GetByDni(string dni)
            => await _context.ExternalUsers.Where(x => x.DocumentNumber == dni).FirstOrDefaultAsync();

        public async Task<ExternalUser> GetByDocument(int documentType, string documentNumber)
        {
            var externalUser = await _context.ExternalUsers
                .Where(x => x.DocumentType == documentType && x.DocumentNumber == documentNumber)
                .FirstOrDefaultAsync();

            return externalUser;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }

}
