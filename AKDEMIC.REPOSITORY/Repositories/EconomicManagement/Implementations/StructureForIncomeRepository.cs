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
    public class StructureForIncomeRepository : Repository<StructureForIncome>, IStructureForIncomeRepository
    {
        public StructureForIncomeRepository(AkdemicContext context) : base (context) { }

        public async Task<DateTime> GetLastDate()
        {
            var result = await _context.StructureForIncomes.OrderByDescending(x => x.CreateAt).Select(x => x.CreateAt).FirstOrDefaultAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStructureForIncomesExcelImportDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string caseFile, string year, string userId)
        {
            Expression<Func<StructureForIncome, dynamic>> orderByPredicate = null;

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
                    orderByPredicate = (x) => x.Amount;
                    break;
                case "10":
                    orderByPredicate = (x) => x.IncomeClassifier;
                    break;
                case "11":
                    orderByPredicate = (x) => x.IncomeClassifierName;
                    break;
                case "12":
                    orderByPredicate = (x) => x.Dependency.Acronym;
                    break;
                case "13":
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.CreateAt;
                    break;
            }


            var query = _context.StructureForIncomes
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var userDependecy = await _context.UserDependencies.Where(x => x.UserId == userId).ToListAsync();

            var rol = await _context.UserRoles.Where(x => x.UserId == userId && x.Role.Name == CORE.Helpers.ConstantHelpers.ROLES.COST_CENTER).FirstOrDefaultAsync();
            

            if (rol != null)
            {
                query = query.Where(q => q.Dependency.Name.Contains("Biblioteca"));
            }


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
                    amount = x.Amount,
                    incomeClassifier = x.IncomeClassifier,
                    incomeClassifierName = x.IncomeClassifierName,
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
