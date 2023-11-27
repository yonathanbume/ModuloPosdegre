using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(AkdemicContext context) : base(context) { }

        public async Task<Country> GetCountryByCode(string code)
        {
            return await _context.Countries.FirstOrDefaultAsync(x => x.Code == code);
        }

        async Task<object> ICountryRepository.GetAllAsSelect2ClientSide()
            => await _context.Countries.Select(
                x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();

        Task<Country> ICountryRepository.GetCountryByCode(string code)
            => _context.Countries.FirstOrDefaultAsync(x => x.Code == code);

        public async Task<object> GetCountryJson(string q)
        {
            var result = _context.Countries
                .Select(c => new
                {
                    id = c.Code,
                    text = c.Name
                }).AsQueryable();

            if (!string.IsNullOrEmpty(q))
                result = result.Where(c => c.text.Contains(q));

            var model = await result.OrderBy(x => x.text).ToListAsync();

            return model;
        }

        public async Task<Country> GetByCode(string cell)
        {
            var country = await _context.Countries.Where(x => x.Code == cell).FirstOrDefaultAsync();

            return country;
        }

        public async Task<Guid> GetGuidByCode(string countryCode)
        {
            var result = (await _context.Countries.FirstOrDefaultAsync(x => x.Code.Equals(countryCode))).Id;

            return result;
        }

        public async Task<Guid> GetGuidFirst()
        {
            var result = (await _context.Countries.FirstOrDefaultAsync()).Id;

            return result;
        }

        public async Task<bool> AnyByCode(string code, Guid? id = null)
        {
            return await _context.Countries.AnyAsync(x => x.Code.ToUpper() == code.ToUpper() && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllCountriesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<Country, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.Countries.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.Code.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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
    }
}