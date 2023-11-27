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
    public class ExpenseOutputRepository : Repository<ExpenseOutput>, IExpenseOutputRepository
    {
        public ExpenseOutputRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExpenseOutPutsDatatables(DataTablesStructs.SentParameters sentParameters, string userId, string search)
        {
            Expression<Func<ExpenseOutput, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "5":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "6":
                    orderByPredicate = (x) => x.Amount;
                    break;
                case "7":
                    orderByPredicate = (x) => x.ExpenseReport;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }


            var query = _context.ExpenseOutputs
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var userDependencies = await _context.UserDependencies.Where(x => x.UserId == userId).Select(x => x.DependencyId).ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Description.Contains(search));
            }

            query = _context.ExpenseOutputs
                .Where(x => userDependencies.Contains(x.DependencyId))
                .AsNoTracking();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    description = x.Description,
                    user = x.User.UserName,
                    name = x.User.FullName,
                    amount = x.Amount,
                    dependencyId = x.DependencyId,
                    dependency = x.Dependency.Name,
                    state = x.ExpenseReport ? "Rendido" : "Sin rendir",
                    date = x.CreatedAt.ToLocalDateFormat(),
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
        public async Task<List<ExpenseOutput>> GetExpenseOutputReportList(Guid id)
        {
            var data = await _context.ExpenseOutputs
                .Where(x => x.DependencyId == id).ToListAsync();

            return data;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetExpenseOutPutsTesoDatatables(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<ExpenseOutput, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "5":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "6":
                    orderByPredicate = (x) => x.Amount;
                    break;
                case "7":
                    orderByPredicate = (x) => x.ExpenseReport;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }


            var query = _context.ExpenseOutputs
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Description.Contains(search));
            }

            query = _context.ExpenseOutputs
                .AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    description = x.Description,
                    user = string.IsNullOrEmpty(x.UserId) ? "-": x.User.UserName,
                    name = string.IsNullOrEmpty(x.UserId) ? "-": x.User.FullName,
                    amount = x.Amount,
                    dependencyId = x.DependencyId,
                    dependency = x.Dependency.Name,
                    state = x.ExpenseReport ? "Rendido" : "Sin rendir",
                    date = x.CreatedAt.ToLocalDateFormat()
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
