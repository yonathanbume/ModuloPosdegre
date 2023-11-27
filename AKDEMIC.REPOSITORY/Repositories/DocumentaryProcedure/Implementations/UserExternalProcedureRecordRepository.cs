using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class UserExternalProcedureRecordRepository : Repository<UserExternalProcedureRecord>, IUserExternalProcedureRecordRepository
    {
        public UserExternalProcedureRecordRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? userExternalProcedureId = null, Expression<Func<UserExternalProcedureRecord, UserExternalProcedureRecord>> selectPredicate = null, Expression<Func<UserExternalProcedureRecord, dynamic>> orderByPredicate = null, Func<UserExternalProcedureRecord, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.UserExternalProcedureRecords.AsNoTracking();

            if (userExternalProcedureId != null)
            {
                query = query.Where(x => x.UserExternalProcedureId == userExternalProcedureId);
            }

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<UserExternalProcedureRecord>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        #endregion

        #region PUBLIC

        public async Task<int> CountByUserExternalProcedureId(Guid userExternalProcedureId)
        {
            return await _context.UserExternalProcedureRecords.CountAsync(x => x.UserExternalProcedureId == userExternalProcedureId);
        }

        public async Task<UserExternalProcedureRecord> GetUserExternalProcedureRecordByUserExternalProcedure(Guid userExternalProcedureId)
        {
            var query = _context.UserExternalProcedureRecords
                .Where(x => x.UserExternalProcedureId == userExternalProcedureId)
                .SelectUserExternalProcedureRecord()
                .AsQueryable();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecords()
        {
            var query = _context.UserExternalProcedureRecords
                .SelectUserExternalProcedureRecord()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsByDate(DateTime start, DateTime end)
        {
            var query = _context.UserExternalProcedureRecords
                .Where(x => x.CreatedAt > start && x.CreatedAt < end)
                .SelectUserExternalProcedureRecord()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsByEndDate(DateTime end)
        {
            var query = _context.UserExternalProcedureRecords
                .Where(x => x.CreatedAt < end)
                .SelectUserExternalProcedureRecord()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsBySearchValue(string searchValue)
        {
            var query = _context.UserExternalProcedureRecords
                .Where(x => x.FullRecordNumber == searchValue || x.UserExternalProcedure.ExternalUser.DocumentNumber == searchValue)
                .SelectUserExternalProcedureRecord()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsByStartDate(DateTime start)
        {
            var query = _context.UserExternalProcedureRecords
                .Where(x => x.CreatedAt > start)
                .SelectUserExternalProcedureRecord()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsByUserExternalProcedure(Guid userExternalProcedureId)
        {
            var query = _context.UserExternalProcedureRecords
                .Where(x => x.UserExternalProcedureId == userExternalProcedureId)
                .SelectUserExternalProcedureRecord()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<UserExternalProcedureRecord>> GetUserExternalProcedureRecordsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<UserExternalProcedureRecord, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Subject);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.RecordNumber);

                    break;
                default:
                    orderByPredicate = ((x) => x.Subject);

                    break;
            }

            return await GetUserExternalProcedureRecordsDatatable(sentParameters, null, ExpressionHelpers.SelectUserExternalProcedureRecord(), orderByPredicate, (x) => new[] { x.Subject, x.RecordNumber + "" }, searchValue);
        }

        #endregion






















        public async Task<Tuple<int, List<UserExternalProcedureRecord>>> GetUserExternalProcedureRecords(DateTime? beginDate, DateTime? endDate, DataTablesStructs.SentParameters sentParameters, Guid? dependenyId)
        {
            var query = _context.UserExternalProcedureRecords
                .Where(x => string.IsNullOrWhiteSpace(sentParameters.SearchValue) || x.FullRecordNumber.Contains(sentParameters.SearchValue) ||
                            x.UserExternalProcedure.ExternalUser.DocumentNumber.Contains(sentParameters.SearchValue) ||
                            x.UserExternalProcedure.ExternalProcedure.Name.Contains(sentParameters.SearchValue))
                .AsQueryable();

            if (dependenyId.HasValue)
                query = query.Where(x => x.UserExternalProcedure.DependencyId == dependenyId);

            if (beginDate.HasValue && endDate.HasValue)
                query = query.Where(x => x.EntryDate.Date >= beginDate.Value.Date && x.EntryDate.Date <= endDate.Value.Date);
            else if (beginDate.HasValue)
                query = query.Where(x => x.EntryDate.Date >= beginDate.Value.Date);
            else if (endDate.HasValue)
                query = query.Where(x => x.EntryDate.Date <= endDate.Value.Date);

            var records = await query.CountAsync();

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.FullRecordNumber) : query.OrderBy(q => q.FullRecordNumber);
                    break;
                case "1":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.UserExternalProcedure.ExternalUser.PaternalSurname) : query.OrderBy(q => q.UserExternalProcedure.ExternalUser.PaternalSurname);
                    break;
                case "2":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.UserExternalProcedure.ExternalProcedure.Name) : query.OrderBy(q => q.UserExternalProcedure.ExternalProcedure.Name);
                    break;
                case "3":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.EntryDate) : query.OrderBy(q => q.EntryDate);
                    break;
                default:
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.FullRecordNumber) : query.OrderBy(q => q.FullRecordNumber);
                    break;
            }

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .Select(x => new UserExternalProcedureRecord
                {
                    Id = x.Id,
                    FullRecordNumber = x.FullRecordNumber,
                    EntryDate = x.EntryDate,
                    Pages = x.Pages,
                    UserExternalProcedure = new UserExternalProcedure
                    {
                        Id = x.UserExternalProcedure.Id,
                        ExternalProcedure = new ExternalProcedure
                        {
                            Code = x.UserExternalProcedure.ExternalProcedure.Code,
                            Name = x.UserExternalProcedure.ExternalProcedure.Name
                        },
                        DependencyId = x.UserExternalProcedure.DependencyId,
                        Dependency = new Dependency
                        {
                            Id = x.UserExternalProcedure.DependencyId,
                            Name = x.UserExternalProcedure.Dependency.Name
                        },
                        ExternalUser = new ExternalUser
                        {
                            Name = x.UserExternalProcedure.ExternalUser.Name,
                            PaternalSurname = x.UserExternalProcedure.ExternalUser.PaternalSurname,
                            MaternalSurname = x.UserExternalProcedure.ExternalUser.MaternalSurname,
                            DocumentNumber = x.UserExternalProcedure.ExternalUser.DocumentNumber,
                            FullName = x.UserExternalProcedure.ExternalUser.FullName
                        }
                    }
                })
                .AsNoTracking()
                .ToListAsync();

            return new Tuple<int, List<UserExternalProcedureRecord>>(records, pagedList);
        }

        public async Task<int> GetNextRecordNumberByDocumentaryRecordTypeId(Guid documentaryRecordTypeId)
        {
            var result = await _context.UserExternalProcedureRecords
                .Where(x => x.DocumentaryRecordTypeId == documentaryRecordTypeId)
                .OrderByDescending(x => x.RecordNumber)
                .FirstOrDefaultAsync();

            if (result == null)
                return 1;

            return result.RecordNumber + 1;
        }
    }
}
