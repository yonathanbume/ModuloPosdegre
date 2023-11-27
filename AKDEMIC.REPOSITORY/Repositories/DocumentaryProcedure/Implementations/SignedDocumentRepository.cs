using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class SignedDocumentRepository : Repository<SignedDocument>, ISignedDocumentRepository
    {
        public SignedDocumentRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSignedDocumentsDatatable(DataTablesStructs.SentParameters sentParameters, string userId = null)
        {
            Expression<Func<SignedDocument, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.FileName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "2":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "3":
                    orderByPredicate = ((x) => x.CreatedAt); break;
                case "4":
                    orderByPredicate = ((x) => x.IsSigned); break;
                default:
                    break;
            }

            var query = _context.SignedDocuments.AsNoTracking();
            if (!string.IsNullOrEmpty(userId)) query = query.Where(x => x.UserId == userId);

            var records = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    fileName = x.FileName,
                    user = x.User.UserName,
                    fullName = x.User.FullName,
                    date = x.CreatedAt.ToLocalDateFormat(),
                    isSigned = x.IsSigned,
                    isImported = string.IsNullOrEmpty(x.Html) && !string.IsNullOrEmpty(x.FilePath),
                    url = x.FilePath
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = records,
                RecordsTotal = records
            };
        }

        public async Task<SignedDocument> GetUnsignedDocument(string userId)
        {
            var document = await _context.SignedDocuments.FirstOrDefaultAsync(x => x.UserId == userId && !x.IsSigned);
            return document;
        }
    }
}
