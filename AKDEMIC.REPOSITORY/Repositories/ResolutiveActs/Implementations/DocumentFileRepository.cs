using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.ResolutiveActs.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.ResolutiveActs.Implementations
{
    public class DocumentFileRepository : Repository<DocumentFile>, IDocumentFileRepository
    {
        public DocumentFileRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<DocumentFile, dynamic>> GetDocumentFilesDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Name);
                case "1":
                    return ((x) => x.UploadDate);
                default:
                    return ((x) => x.Name);
            }
        }

        private Func<DocumentFile, string[]> GetDocumentFilesDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Name + "",
                x.UploadDate+ "",
            };
        }

        private async Task<DataTablesStructs.ReturnedData<DocumentFile>> GetDocumentFilesDatatable(DataTablesStructs.SentParameters sentParameters,Guid? documentId,Expression<Func<DocumentFile, DocumentFile>> selectPredicate = null, Expression<Func<DocumentFile, dynamic>> orderByPredicate = null, Func<DocumentFile, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.DocumentFiles
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsQueryable();

            if (documentId.HasValue)
                query = query.Where(x => x.DocumentId == documentId);

            var result = query
                .Select(x => new DocumentFile
                {
                    Id = x.Id,
                    Name = x.Name,
                    UploadFormattedDate = x.UploadDate.ToLocalDateTimeFormat(),
                    UrlFile = x.UrlFile
                }, searchValue)
                .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);

        }

        #endregion

        #region PUBLIC

        public async Task<DataTablesStructs.ReturnedData<DocumentFile>> GetDocumentFilesDatatable(DataTablesStructs.SentParameters sentParameters,Guid? documentId,string searchValue = null)
        {
            return await GetDocumentFilesDatatable(sentParameters,documentId ,null, GetDocumentFilesDatatableOrderByPredicate(sentParameters), GetDocumentFilesDatatableSearchValuePredicate(), searchValue);
        }

        public async Task<IEnumerable<DocumentFile>> GetDocumentFilesByDocumentId(Guid documentId)
            => await _context.DocumentFiles.Where(x => x.DocumentId == documentId).ToArrayAsync();

        #endregion
    }
}
