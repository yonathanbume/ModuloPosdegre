using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class ExternalProcedureRepository : Repository<ExternalProcedure>, IExternalProcedureRepository
    {
        public ExternalProcedureRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<ExternalProcedure>> GetExternalProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Guid? classifierId = null, Guid? dependencyId = null, Expression<Func<ExternalProcedure, ExternalProcedure>> selectPredicate = null, Expression<Func<ExternalProcedure, dynamic>> orderByPredicate = null, Func<ExternalProcedure, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.ExternalProcedures.Include(x => x.Concept).AsNoTracking();

            if (classifierId != null && classifierId != Guid.Empty)
            {
                query = query.Where(x => x.ClassifierId == classifierId);
            }

            if (dependencyId != null && dependencyId != Guid.Empty)
            {
                query = query.Where(x => x.DependencyId == dependencyId);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower().Trim();
                query = query.Where(x => x.Name.ToLower().Contains(searchValue) || x.Code.ToLower().Contains(searchValue));
            }

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(selectPredicate)
                .ToListAsync();
            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<ExternalProcedure>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        private async Task<Select2Structs.ResponseParameters> GetExternalProceduresSelect2(Select2Structs.RequestParameters requestParameters, Guid? classifierId = null, Guid? dependencyId = null, Expression<Func<ExternalProcedure, Select2Structs.Result>> selectPredicate = null, Func<ExternalProcedure, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.ExternalProcedures.AsNoTracking();

            if (classifierId != null)
            {
                query = query.Where(x => x.ClassifierId == classifierId);
            }

            if (dependencyId != null)
            {
                query = query.Where(x => x.DependencyId == dependencyId);
            }

            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            var results = await query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        #endregion

        #region PUBLIC

        public async Task<bool> AnyExternalProcedureByCode(string code)
        {
            var query = _context.ExternalProcedures.Where(x => x.Code == code);

            return await query.AnyAsync();
        }

        public async Task<ExternalProcedure> GetExternalProcedure(Guid id)
        {
            var query = _context.ExternalProcedures
                .Where(x => x.Id == id)
                .SelectExternalProcedure();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ExternalProcedure>> GetExternalProcedures()
        {
            var query = _context.ExternalProcedures.SelectExternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ExternalProcedure>> GetExternalProceduresByDependency(Guid dependencyId)
        {
            var query = _context.ExternalProcedures
                .Where(x => x.DependencyId == dependencyId)
                .SelectExternalProcedure();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<ExternalProcedure>> GetExternalProceduresDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<ExternalProcedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Code);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.Classifier.Name);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.Dependency.Name);

                    break;
                case "4":
                case "5":
                    orderByPredicate = ((x) => x.Cost);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetExternalProceduresDatatable(sentParameters, null, null, ExpressionHelpers.SelectExternalProcedure(), orderByPredicate, (x) => new[] { x.Code, x.Cost + "", x.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<ExternalProcedure>> GetExternalProceduresDatatableByClassifier(DataTablesStructs.SentParameters sentParameters, Guid classifierId, string searchValue = null)
        {
            Expression<Func<ExternalProcedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Code);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.Classifier.Name);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.Dependency.Name);

                    break;
                case "4":
                case "5":
                    orderByPredicate = ((x) => x.Cost);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetExternalProceduresDatatable(sentParameters, classifierId, null, ExpressionHelpers.SelectExternalProcedure(), orderByPredicate, (x) => new[] { x.Code, x.Cost + "", x.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<ExternalProcedure>> GetExternalProceduresDatatableByClassifier(DataTablesStructs.SentParameters sentParameters, Guid classifierId, Guid dependencyId, string searchValue = null)
        {
            Expression<Func<ExternalProcedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Code);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.Classifier.Name);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.Dependency.Name);

                    break;
                case "4":
                case "5":
                    orderByPredicate = ((x) => x.Cost);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetExternalProceduresDatatable(sentParameters, classifierId, dependencyId, ExpressionHelpers.SelectExternalProcedure(), orderByPredicate, (x) => new[] { x.Code, x.Cost + "", x.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<ExternalProcedure>> GetExternalProceduresDatatableByDependency(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string searchValue = null)
        {
            Expression<Func<ExternalProcedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Code);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.Classifier.Name);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.Dependency.Name);

                    break;
                case "4":
                case "5":
                    orderByPredicate = ((x) => x.Cost);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetExternalProceduresDatatable(sentParameters, null, dependencyId, ExpressionHelpers.SelectExternalProcedure(), orderByPredicate, (x) => new[] { x.Code, x.Cost + "", x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetExternalProceduresSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetExternalProceduresSelect2(requestParameters, null, null, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetExternalProceduresSelect2ByClassifier(Select2Structs.RequestParameters requestParameters, Guid classifierId, string searchValue = null)
        {
            return await GetExternalProceduresSelect2(requestParameters, classifierId, null, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetExternalProceduresSelect2ByClassifier(Select2Structs.RequestParameters requestParameters, Guid classifierId, Guid dependencyId, string searchValue = null)
        {
            return await GetExternalProceduresSelect2(requestParameters, classifierId, dependencyId, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetExternalProceduresSelect2ByDependency(Select2Structs.RequestParameters requestParameters, Guid dependencyId, string searchValue = null)
        {
            return await GetExternalProceduresSelect2(requestParameters, null, dependencyId, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        #endregion
























        public async Task<bool> HasAnyUserExternalProcedure(Guid externalProcedureId)
        {
            return await _context.UserExternalProcedures.AnyAsync(x => x.ExternalProcedureId == externalProcedureId);
        }

        public async Task<bool> IsCodeDuplicated(string code)
        {
            return await _context.ExternalProcedures.AnyAsync(x => x.Code == code);
        }

        public async Task<bool> IsCodeDuplicated(string code, Guid externalProcedureId)
        {
            return await _context.ExternalProcedures.AnyAsync(x => x.Id != externalProcedureId && x.Code == code);
        }

        public async Task<Tuple<int, List<ExternalProcedure>>> GetExternalProcedures(DataTablesStructs.SentParameters sentParameters)
        {
            var query = _context.ExternalProcedures
                .Where(x => string.IsNullOrWhiteSpace(sentParameters.SearchValue) || x.Name.Contains(sentParameters.SearchValue) ||
                            x.Code.Contains(sentParameters.SearchValue))
                .AsQueryable();

            var records = await query.CountAsync();

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Code) : query.OrderBy(q => q.Code);
                    break;
                case "1":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Name) : query.OrderBy(q => q.Name);
                    break;
                case "2":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Cost) : query.OrderBy(q => q.Cost);
                    break;
                case "3":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Cost) : query.OrderBy(q => q.Cost);
                    break;
                case "4":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Dependency.Name) : query.OrderBy(q => q.Dependency.Name);
                    break;
                default:
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Code) : query.OrderBy(q => q.Code);
                    break;
            }

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .SelectExternalProcedure()
                .ToListAsync();

            return new Tuple<int, List<ExternalProcedure>>(records, pagedList);
        }

        public async Task<Tuple<int, List<ExternalProcedure>>> GetExternalProceduresByDependencyId(Guid dependencyId, DataTablesStructs.SentParameters sentParameters)
        {
            var query = _context.ExternalProcedures
                .Where(x => (string.IsNullOrWhiteSpace(sentParameters.SearchValue) || x.Name.Contains(sentParameters.SearchValue) ||
                            x.Code.Contains(sentParameters.SearchValue)) && 
                            x.DependencyId == dependencyId)
                .AsQueryable();

            var records = await query.CountAsync();

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Code) : query.OrderBy(q => q.Code);
                    break;
                case "1":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Name) : query.OrderBy(q => q.Name);
                    break;
                case "2":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Cost) : query.OrderBy(q => q.Cost);
                    break;
                case "3":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Cost) : query.OrderBy(q => q.Cost);
                    break;
                case "4":
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Dependency.Name) : query.OrderBy(q => q.Dependency.Name);
                    break;
                default:
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Code) : query.OrderBy(q => q.Code);
                    break;
            }

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord).Take(sentParameters.RecordsPerDraw)
                .SelectExternalProcedure()
                .ToListAsync();

            return new Tuple<int, List<ExternalProcedure>>(records, pagedList);
        }

        public async Task<List<ExternalProcedure>> GetExternalProceduresBySearchValue(string searchValue)
        {
            if (!string.IsNullOrWhiteSpace(searchValue))
                searchValue = searchValue.ToLower();

            var query = _context.ExternalProcedures
                .Where(x => string.IsNullOrWhiteSpace(searchValue) ||
                             x.Code.Contains(searchValue) ||
                             x.Name.Contains(searchValue))
                .SelectExternalProcedure()
                .OrderBy(x => x.Name)
                .AsQueryable();

            return await query.ToListAsync();
        }
    }
}
