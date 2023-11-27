using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class SerialNumberRepository : Repository<SerialNumber>, ISerialNumberRepository
    {
        public SerialNumberRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<SerialNumber>> GetUserSerialNumbers(string id) =>
            await _context.SerialNumbers.Where(x => x.UserId == id).ToListAsync();

        public async Task<bool> ValidateSerialNumber(string userId, int number) =>
            await _context.SerialNumbers.AnyAsync(sn => sn.UserId == userId); // && sn.Number == number

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string term)
        {
            Expression<Func<SerialNumber, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.DocumentType;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Series;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                default:
                    orderByPredicate = (x) => x.DocumentType;
                    break;
            }

            var query = _context.SerialNumbers.AsNoTracking();
            if (!string.IsNullOrEmpty(term))
                query = query.Where(x => x.Series.Contains(term) || x.User.UserName.Contains(term));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    id = x.Id,
                    series = x.Series,
                    //correlative = x.Correlative,
                    type = x.DocumentType,
                    userId = x.UserId,
                    username = x.UserId != null ? x.User.UserName : "",
                    user = x.UserId != null ? x.User.FullName.ToUpper() : "",
                    isBankSerial = x.IsBankSerialNumber
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

        public async Task<bool> isRepeatedAsync(string series, byte documentType)
            => await  _context.SerialNumbers.AnyAsync(x =>
                x.Series == series && x.DocumentType == documentType);

        public async Task<bool> haveOneAlreadyAsync(byte documentType, string userId, Guid? id = null)
            => await _context.SerialNumbers.AnyAsync(x => x.DocumentType == documentType && x.UserId == userId && x.Id != id);
        
        public async Task<List<SerialNumber>> GetSerialNumberIncludeUser(Guid id)
        {
            var result = await _context.SerialNumbers.Include(x => x.User)
                .Where(x => x.Id == id).ToListAsync();

            return result;
        }
        public async Task<bool> isRepeatedAsyncById(Guid id, string series, byte documentType)
        {
            var isRepeatedAsync = await _context.SerialNumbers.AnyAsync(x =>
                x.Id != id &&
                x.Series == series && x.DocumentType == documentType);

            return isRepeatedAsync;
        }

        public async Task<SerialNumber> GetUserSerialNumbersByUserIdAndDocumentType(string userId, int documentType)
            => await _context.SerialNumbers.Where(x => x.UserId == userId
                && x.DocumentType == documentType).FirstOrDefaultAsync();

        public async Task RemovePreviousBankSerialNumbers(Guid? newSeriesId)
        {
            var query = _context.SerialNumbers
                .Where(x => x.IsBankSerialNumber)
                .AsQueryable();

            if (newSeriesId.HasValue && newSeriesId != Guid.Empty)
                query = query.Where(x => x.Id != newSeriesId.Value);

            var serials = await query.ToListAsync();

            foreach (var item in serials)
            {
                item.IsBankSerialNumber = false;
            }

            await _context.SaveChangesAsync();
        }
    }
}
