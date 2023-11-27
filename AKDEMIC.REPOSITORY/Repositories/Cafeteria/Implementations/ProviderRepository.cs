using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Implementations
{
    public class ProviderRepository : Repository<Provider>, IProviderRepository
    {
        public ProviderRepository(AkdemicContext context) : base(context)
        {
        }

        public async override Task<Provider> Get(Guid id)
        {
            return await _context.Providers.Include(x=>x.User).Include(x=>x.Career).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Provider> GetProviderByUserId(string UserId)
        {
            return await _context.Providers.Where(x => x.UserId == UserId).FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetProvidersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<Provider, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Email);

                    break;
                case "5":
                    orderByPredicate = ((x) => x.User.Email);

                    break;
                default:
                    orderByPredicate = ((x) => x.Career.Name);

                    break;
            }

            return await GetProvidersDatatable(sentParameters, (x) => new {
                x.Id,
                x.Code,
                FullName = (x.User.PaternalSurname != null && x.User.MaternalSurname != null) ? x.User.FullName : x.User.Name,
                x.User.Email,
                DocumentType = ConstantHelpers.DOCUMENT_TYPES.VALUES[x.User.DocumentType],
                DocumentNumber = x.User.Document,
                Career =x.Career.Name

            }, orderByPredicate, searchValue);
        }

        //x.User.Dni, x.User.FullName, x.User.PhoneNumber, x.User.Email

        public async Task<object> GetSelectProviders()
        {
            var result = await _context.Providers.Include(x=>x.User).Select(x => new
            {
                x.Id,
                Text = $"{x.User.Document} - {x.User.FullName}"
            }).ToListAsync();
            return result;
        }

        public async Task<bool> ValidateProviderCode(Guid? providerId, string code)
        {
            var query = _context.Providers.AsQueryable();
            if (providerId!= null)
            {
                query = query.Where(x => x.Id != providerId.Value);
            }
            return await query.AnyAsync(x => x.Code == code);
        }

        private async Task<DataTablesStructs.ReturnedData<object>> GetProvidersDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<Provider, dynamic>> selectPredicate = null, Expression<Func<Provider, dynamic>> orderByPredicate = null, string searchValue = null)
        {
            var query = _context.Providers.Include(x=>x.User).Include(x=>x.Career)                
                .AsNoTracking();
            if (searchValue != null)
            {
                query = query.Where(x => x.User.Dni.Contains(searchValue) || x.User.FullName.Contains(searchValue) || x.User.Email.Contains(searchValue) || x.Code.Contains(searchValue));
            }

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }
    }
}
