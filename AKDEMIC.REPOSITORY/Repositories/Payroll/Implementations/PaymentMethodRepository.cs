using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class PaymentMethodRepository :  Repository<PaymentMethod>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(AkdemicContext context) : base(context) { }

        public async Task ChangeStatus(Guid id)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);
            paymentMethod.IsActive = !paymentMethod.IsActive;
        }

        public async Task<object> GetPaymentMethods()
        {
            var result = await _context.PaymentMethods.Select(
             x => new
             {
                 Id = x.Id,
                 Code = x.Code,
                 Name = x.Name,
                 IsActive = x.IsActive
             }).ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllPaymentMethodsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<PaymentMethod, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.PaymentMethods.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper())
                                    || x.Code.ToUpper().Contains(searchValue.ToUpper()));
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
