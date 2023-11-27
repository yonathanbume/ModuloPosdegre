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
    public class StructureForExpenseRepository : Repository<StructureForExpense>, IStructureForExpenseRepository
    {
        public StructureForExpenseRepository(AkdemicContext context) : base(context) { }

        public async Task<DateTime> GetLastDate()
        {
            var result = await _context.StructureForExpenses.OrderByDescending(x => x.CreateAt).Select(x => x.CreateAt).FirstOrDefaultAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStructureForExpensesExcelImportDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string caseFile, string year)
        {
            Expression<Func<StructureForExpense, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.ExecutionYear;
                    break;
                case "2":
                    orderByPredicate = (x) => x.ExecutionMonth;
                    break;
                case "3":
                    orderByPredicate = (x) => x.CaseFile;
                    break;
                case "4":
                    orderByPredicate = (x) => x.FinancingSource;
                    break;
                case "5":
                    orderByPredicate = (x) => x.DocumentCod;
                    break;
                case "6":
                    orderByPredicate = (x) => x.DocumentName;
                    break;
                case "7":
                    orderByPredicate = (x) => x.DocumentNum;
                    break;
                case "8":
                    orderByPredicate = (x) => x.DocumentDate;
                    break;
                case "9":
                    orderByPredicate = (x) => x.ExpenseClassifier;
                    break;
                case "10":
                    orderByPredicate = (x) => x.ExpenseClassifierName;
                    break;
                case "11":
                    orderByPredicate = (x) => x.ProjectActivityCode;
                    break;
                case "12":
                    orderByPredicate = (x) => x.ProjectActivityName;
                    break;
                case "13":
                    orderByPredicate = (x) => x.Goals;
                    break;
                case "14":
                    orderByPredicate = (x) => x.Amount;
                    break;
                case "15":
                    orderByPredicate = (x) => x.Provider;
                    break;
                case "16":
                    orderByPredicate = (x) => x.POAOperationalActivityCod;
                    break;
                case "17":
                    orderByPredicate = (x) => x.POAOperationalActivityName;
                    break;
                case "18":
                    orderByPredicate = (x) => x.Dependency.Acronym;
                    break;
                case "19":
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
            }


            var query = _context.StructureForExpenses
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();


            if (dependencyId != Guid.Empty)
            {
                query = query.Where(q => q.DependencyId == dependencyId);
            }

            if (!string.IsNullOrEmpty(caseFile))
            {
                var CaseFile = int.Parse(caseFile);
                query = query.Where(q => q.CaseFile == CaseFile);
            }
            if (!string.IsNullOrEmpty(year))
            {
                var Year = int.Parse(year);
                query = query.Where(q => q.ExecutionYear == Year);
            }

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    id = x.Id,
                    executionYear = x.ExecutionYear,
                    executionMonth = x.ExecutionMonth,
                    caseFile = x.CaseFile,
                    financingSource = x.FinancingSource,
                    documentCod = x.DocumentCod,
                    documentName = x.DocumentName,
                    documentNum = x.DocumentNum,
                    documentDate = $"{x.DocumentDate:dd/MM/yyyy}",
                    expenseClassifier = x.ExpenseClassifier,
                    expenseClassifierName = x.ExpenseClassifierName,
                    projectActivityCode = x.ProjectActivityCode,
                    projectActivityName = x.ProjectActivityName,
                    goals = x.Goals,
                    amount = x.Amount,
                    provider = x.Provider,
                    pOAOperationalActivityCod = x.POAOperationalActivityCod,
                    pOAOperationalActivityName = x.POAOperationalActivityName,
                    costCenterCod = x.Dependency.Acronym,
                    costCenterName = x.Dependency.Name
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
