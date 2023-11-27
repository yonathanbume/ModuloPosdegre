using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class SiafExpenseRepository : Repository<SiafExpense>, ISiafExpenseRepository
    {
        public SiafExpenseRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSiafExpenseDatatable(DataTablesStructs.SentParameters sentParameters, byte? status = null)
        {
            Expression<Func<SiafExpense, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Record;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Document;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Date;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Client;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Amount;
                    break;
                case "5":
                    orderByPredicate = (x) => x.AssociatedDocument;
                    break;
                case "6":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "7":
                    orderByPredicate = (x) => x.Observations;
                    break;
                case "8":
                    orderByPredicate = (x) => x.Received;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }


            var query = _context.SiafExpenses
                .AsNoTracking();

            if (status.HasValue)
            {
                switch (status.Value)
                {
                    case 1:
                        query = query.Where(x => !x.Received && string.IsNullOrEmpty(x.RoleId));
                        break;
                    case 2:
                        query = query.Where(x => !x.Received && !string.IsNullOrEmpty(x.RoleId));
                        break;
                    case 3:
                        query = query.Where(x => x.Received);
                        break;
                    default:
                        break;
                }
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    id = x.Id,
                    record = x.Record,
                    document = x.Document,
                    date = $"{x.Date:dd/MM/yyyy}",
                    client = x.Client,
                    amount = x.Amount,
                    documenta = x.AssociatedDocument,
                    description = x.Description,
                    received = x.Received,
                    derived = !string.IsNullOrEmpty(x.RoleId),
                    observations = x.Observations
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetDerivedExpensesDatatable(DataTablesStructs.SentParameters sentParameters, string userId)
        {
            Expression<Func<SiafExpense, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Record;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Document;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Date;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Client;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Amount;
                    break;
                case "5":
                    orderByPredicate = (x) => x.AssociatedDocument;
                    break;
                case "6":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "7":
                    orderByPredicate = (x) => x.Received;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }


            var query = _context.SiafExpenses
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var roles = await _context.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToListAsync();
            query = query.Where(x => roles.Any(r => r == x.RoleId)).AsQueryable();


            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    record = x.Record,
                    document = x.Document,
                    date = $"{x.Date:dd/MM/yyyy}",
                    client = x.Client,
                    amount = x.Amount,
                    documenta = x.AssociatedDocument,
                    description = x.Description,
                    received = x.Received
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
    }
}
