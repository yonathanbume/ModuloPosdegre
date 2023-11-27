using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class BankDepositRepository : Repository<BankDeposit>, IBankDepositRepository
    {
        public BankDepositRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<List<BankDeposit>> GetAllByPettyCashBookId(Guid pettyCashBookId)
        {
            var deposits = await _context.BankDeposits
                .Where(x => x.PettyCashBookId == pettyCashBookId)
                .Include(x => x.CurrentAccount)
                .ToListAsync();

            return deposits;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetBankDepositDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<BankDeposit, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Number;
                    break;
                case "1":
                    orderByPredicate = (x) => x.CurrentAccount.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Date;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Amount;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Description;
                    break;
                default:
                    orderByPredicate = (x) => x.Date;
                    break;
            }


            var query = _context.BankDeposits
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();



            if (!string.IsNullOrEmpty(search))
                query = query
                    .Where(x => x.Number.ToUpper().Contains(search)
                            || x.CurrentAccount.Name.ToUpper().Contains(search)
                            || x.Description.ToUpper().Contains(search));

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    number = x.Number,
                    currentAccount = x.CurrentAccount.Name,
                    amount = x.Amount,
                    description = x.Description,
                    date = x.Date.ToLocalDateFormat()
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

        public async Task<object> GetObjectById(Guid id)
        {
            var data = await _context.BankDeposits.Include(x => x.CurrentAccount).Where(x => x.Id == id).Select(x => new
            {
                id = x.Id,
                number = x.Number,
                currentAccountId = x.CurrentAccountId,
                date = x.Date.ToLocalDateFormat(),
                amount = x.Amount,
                description = x.Description
            }).FirstOrDefaultAsync();

            return data;
        }
    }
}
