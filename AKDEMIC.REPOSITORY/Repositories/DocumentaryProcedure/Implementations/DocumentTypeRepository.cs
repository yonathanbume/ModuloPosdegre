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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class DocumentTypeRepository : Repository<DocumentType>, IDocumentTypeRepository
    {
        public DocumentTypeRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<DocumentType>> GetDocumentTypesDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<DocumentType, DocumentType>> selectPredicate = null, Expression<Func<DocumentType, dynamic>> orderByPredicate = null, Func<DocumentType, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.DocumentTypes.AsNoTracking();
            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<DocumentType>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        private async Task<Select2Structs.ResponseParameters> GetDocumentTypesSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<DocumentType, Select2Structs.Result>> selectPredicate = null, Func<DocumentType, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.DocumentTypes.AsNoTracking();
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

        public async Task<bool> AnyDocumentTypeByCode(string code)
        {
            var query = _context.DocumentTypes.Where(x => x.Code == code);

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<DocumentType>> GetDocumentTypes()
        {
            var query = _context.DocumentTypes.SelectDocumentType();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<DocumentType>> GetDocumentTypesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<DocumentType, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Code);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetDocumentTypesDatatable(sentParameters, ExpressionHelpers.SelectDocumentType(), orderByPredicate, (x) => new[] { x.Code, x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetDocumentTypesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetDocumentTypesSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        #endregion




















        public async Task<Tuple<int, List<DocumentType>>> GetDatatableDocumentTypes(DataTablesStructs.SentParameters sentParameters)
        {
            var query = _context.DocumentTypes
                .Where(x => string.IsNullOrWhiteSpace(sentParameters.SearchValue) ||
                            x.Code.Contains(sentParameters.SearchValue) ||
                            x.Name.Contains(sentParameters.SearchValue)
                )
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
                default:
                    query = sentParameters.OrderDirection.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Code) : query.OrderBy(q => q.Code);
                    break;
            }

            var pagedList = await query.Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new DocumentType
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    HasRelated = x.UserExternalProcedureRecords.Any() || x.UserProcedureRecords.Any()
                })
                .AsNoTracking()
                .ToListAsync();

            return new Tuple<int, List<DocumentType>>(records, pagedList);
        }

        public async Task<bool> HasCode(string code)
        {
            return await _context.DocumentTypes.AnyAsync(x => x.Code == code);
        }

        public async Task<bool> HasCode(string code, Guid id)
        {
            return await _context.DocumentTypes.IgnoreQueryFilters().AnyAsync(x => x.Code == code && x.Id != id);
        }

        public async Task<bool> HasRelated(Guid id)
        {
            return  await _context.UserExternalProcedureRecords.AnyAsync(x => x.DocumentTypeId == id) ||
                    await _context.UserProcedureRecords.AnyAsync(x => x.DocumentTypeId == id);
        }

        public async Task<DocumentType> GetByCode(string code)
            => await _context.DocumentTypes.Where(x => x.Code.ToLower() == code.ToLower()).FirstOrDefaultAsync();

        public async Task<DocumentType> GetByName(string name)
            => await _context.DocumentTypes.Where(x => x.Name.ToLower().Equals(name.ToLower())).FirstOrDefaultAsync();
    }
}
