using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
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
    public class UserExternalProcedureFileRepository : Repository<UserExternalProcedureFile>, IUserExternalProcedureFileRepository
    {
        public UserExternalProcedureFileRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRequerimentDocuments(DataTablesStructs.SentParameters sentParameters, Guid id)
        {
            Expression<Func<UserExternalProcedureFile, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.FileName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Size;
                    break;
                default:
                    orderByPredicate = (x) => x.FileName;
                    break;
            }

            var query = _context.UserExternalProcedureFiles
                .Where(x => x.UserExternalProcedureId == id)
                .AsQueryable();


            var recordsFiltered = await query.CountAsync();


            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    id = x.Id,
                    filename = x.FileName,
                    size= x.Size,
                    path = x.Path
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

        public async Task<List<UserExternalProcedureFile>> GetExternalProcedureFilesByUserExternalProcedure(Guid id)
            => await _context.UserExternalProcedureFiles.Where(x => x.UserExternalProcedureId == id).ToListAsync();
    }
}
