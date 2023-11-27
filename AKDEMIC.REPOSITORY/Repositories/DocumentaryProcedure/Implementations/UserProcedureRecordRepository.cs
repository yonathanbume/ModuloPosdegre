using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class UserProcedureRecordRepository : Repository<UserProcedureRecord>, IUserProcedureRecordRepository
    {
        public UserProcedureRecordRepository(AkdemicContext context) : base(context) { }

        public async Task<int> CountByUserProcedureId(Guid userProcedureId)
        {
            return await _context.UserProcedureRecords.CountAsync(x => x.UserProcedureId == userProcedureId);
        }

        public async Task<Tuple<int, List<UserProcedureRecord>>> GetUserProcedureRecords(DateTime? beginDate, DateTime? endDate, DataTablesStructs.SentParameters sentParameters)
        {
            var query = _context.UserProcedureRecords
                .Where(x => string.IsNullOrWhiteSpace(sentParameters.SearchValue) || x.FullRecordNumber.Contains(sentParameters.SearchValue) ||
                            x.UserProcedure.User.Dni.Contains(sentParameters.SearchValue) ||
                            x.UserProcedure.User.PaternalSurname.Contains(sentParameters.SearchValue) ||
                            x.UserProcedure.User.MaternalSurname.Contains(sentParameters.SearchValue) ||
                            x.UserProcedure.User.Name.Contains(sentParameters.SearchValue) ||
                            x.UserProcedure.Procedure.Name.Contains(sentParameters.SearchValue))
                .AsQueryable();

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
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.UserProcedure.User.PaternalSurname) : query.OrderBy(q => q.UserProcedure.User.PaternalSurname);
                    break;
                case "2":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.UserProcedure.Procedure.Name) : query.OrderBy(q => q.UserProcedure.Procedure.Name);
                    break;
                case "3":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.EntryDate) : query.OrderBy(q => q.EntryDate);
                    break;
                default:
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.FullRecordNumber) : query.OrderBy(q => q.FullRecordNumber);
                    break;
            }

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .Select(x => new UserProcedureRecord
                {
                    Id = x.Id,
                    FullRecordNumber = x.FullRecordNumber,
                    EntryDate = x.EntryDate,
                    Pages = x.Pages,
                    UserProcedure = new UserProcedure
                    {
                        Id = x.UserProcedure.Id,
                        Procedure = new Procedure
                        {
                            Id = x.UserProcedure.ProcedureId,
                            Name = x.UserProcedure.Procedure.Name
                        },
                        User = new ApplicationUser
                        {
                            Dni = x.UserProcedure.User.Dni,
                            Name = x.UserProcedure.User.Name,
                            PaternalSurname = x.UserProcedure.User.PaternalSurname,
                            MaternalSurname = x.UserProcedure.User.MaternalSurname,
                            FullName = x.UserProcedure.User.FullName
                        },
                        Term = new ENTITIES.Models.Enrollment.Term
                        {
                            Name = x.UserProcedure.Term.Name
                        }
                    }
                })
                .AsNoTracking()
                .ToListAsync();

            return new Tuple<int, List<UserProcedureRecord>>(records, pagedList);
        }

        public async Task<Tuple<int, List<UserProcedureRecord>>> GetUserProcedureRecordsByDniOrRecordNumber(DataTablesStructs.SentParameters sentParameters)
        {
            var query = _context.UserProcedureRecords
                .Where(x => x.FullRecordNumber.Equals(sentParameters.SearchValue) ||
                            x.UserProcedure.User.Dni.Equals(sentParameters.SearchValue))
                .AsQueryable();

            var records = await query.CountAsync();

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.UserProcedure.User.PaternalSurname) : query.OrderBy(q => q.UserProcedure.User.PaternalSurname);
                    break;
                case "1":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.UserProcedure.User.Dni) : query.OrderBy(q => q.UserProcedure.User.Dni);
                    break;
                case "2":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.FullRecordNumber) : query.OrderBy(q => q.FullRecordNumber);
                    break;
                case "3":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.EntryDate) : query.OrderBy(q => q.EntryDate);
                    break;
                default:
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.UserProcedure.User.PaternalSurname) : query.OrderBy(q => q.UserProcedure.User.PaternalSurname);
                    break;
            }

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .Select(x => new UserProcedureRecord
                {
                    Id = x.Id,
                    FullRecordNumber = x.FullRecordNumber,
                    EntryDate = x.EntryDate,
                    Pages = x.Pages,
                    UserProcedure = new UserProcedure
                    {
                        Id = x.UserProcedure.Id,
                        User = new ApplicationUser
                        {
                            Name = x.UserProcedure.User.Name,
                            PaternalSurname = x.UserProcedure.User.PaternalSurname,
                            MaternalSurname = x.UserProcedure.User.MaternalSurname,
                            Dni = x.UserProcedure.User.Dni,
                            FullName = x.UserProcedure.User.FullName
                        }
                    }
                })
                .AsNoTracking()
                .ToListAsync();

            return new Tuple<int, List<UserProcedureRecord>>(records, pagedList);
        }

        public async Task<int> GetNextRecordNumberByDocumentaryRecordTypeId(Guid documentaryRecordTypeId)
        {
            var result = await _context.UserProcedureRecords
                .Where(x => x.DocumentaryRecordTypeId == documentaryRecordTypeId)
                .OrderByDescending(x => x.RecordNumber)
                .FirstOrDefaultAsync();

            if (result == null)
                return 1;

            return result.RecordNumber + 1;
        }

        public async Task<UserProcedureRecord> GetUserProcedureRecordByUserProcedure(Guid userProcedureId)
        {
            var query = _context.UserProcedureRecords
                .Where(x => x.UserProcedureId == userProcedureId)
                .SelectUserProcedureRecord()
                .AsQueryable();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecords()
        {
            var query = _context.UserProcedureRecords
                .SelectUserProcedureRecord()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecordsByDate(DateTime start, DateTime end)
        {
            var query = _context.UserProcedureRecords
                .Where(x => x.CreatedAt > start && x.CreatedAt < end)
                .SelectUserProcedureRecord()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecordsByEndDate(DateTime end)
        {
            var query = _context.UserProcedureRecords
                .Where(x => x.CreatedAt < end)
                .SelectUserProcedureRecord()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecordsBySearchValue(string searchValue)
        {
            var query = _context.UserProcedureRecords
                .Where(x => x.FullRecordNumber == searchValue || x.UserProcedure.User.Dni == searchValue)
                .SelectUserProcedureRecord()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecordsByStartDate(DateTime start)
        {
            var query = _context.UserProcedureRecords
                .Where(x => x.CreatedAt > start)
                .SelectUserProcedureRecord()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<UserProcedureRecord>> GetUserProcedureRecordsByUserProcedure(Guid userProcedureId)
        {
            var query = _context.UserProcedureRecords
                .Where(x => x.UserProcedureId == userProcedureId)
                .SelectUserProcedureRecord()
                .AsQueryable();

            return await query.ToListAsync();
        }
    }
}
