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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class ProcedureFolderRepository : Repository<ProcedureFolder>, IProcedureFolderRepository
    {
        public ProcedureFolderRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<object> GetSelect2DataClientSide(Guid dependencyId)
        {
            var result = await _context.ProcedureFolders
                .Where(x => x.DependencyId == dependencyId)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name,
                })
                .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string search = null)
        {
            var query = _context.ProcedureFolders
                .Where(x => x.DependencyId == dependencyId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToLower().Contains(search.ToLower().Trim()) || x.Code.ToLower().Contains(search.ToLower().Trim()));

            var recordsTotal = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    dependency = x.Dependency.Name
                })
                .ToListAsync();

            var recordsFiltered = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
            };
            return result;

        }

        public async Task<bool> AnyByCode(string code, Guid dependecyId, Guid? ignoredId = null)
            => await _context.ProcedureFolders.AnyAsync(x => x.DependencyId == dependecyId && x.Code.ToLower().Trim() == code.ToLower().Trim() && x.Id != ignoredId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Guid procedureFolderId, Guid dependencyId)
        {
            var query = _context.UserInternalProcedures.Where(x => x.ProcedureFolderId == procedureFolderId).AsNoTracking();

            query = query.Where(x => x.DependencyId == dependencyId);

            var recordsTotal = await query.CountAsync();

            var data = await query
            .Select(x => new UserInternalProcedure
            {
                CreatedAt = x.CreatedAt,
                Dependency = new Dependency
                {
                    Name = x.Dependency.Name
                },
                StatusStr = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.VALUES[x.Status],
                Status = x.Status,
                InternalProcedure = new InternalProcedure
                {
                    Code = x.InternalProcedure.Code,
                    DependencyId = x.InternalProcedure.DependencyId,
                    DocumentTypeId = x.InternalProcedure.DocumentTypeId,
                    InternalProcedureParentId = x.InternalProcedure.InternalProcedureParentId,
                    UserId = x.InternalProcedure.UserId,
                    Content = x.InternalProcedure.Content,
                    Number = x.InternalProcedure.Number,
                    IsTransparency = x.InternalProcedure.IsTransparency,
                    Pages = x.InternalProcedure.Pages,
                    Priority = x.InternalProcedure.Priority,
                    SearchNode = x.InternalProcedure.SearchNode,
                    SearchTree = x.InternalProcedure.SearchTree,
                    Subject = x.InternalProcedure.Subject,
                    CreatedAt = x.InternalProcedure.CreatedAt,
                    GeneratedId = x.InternalProcedure.GeneratedId,
                    FromExternal = x.InternalProcedure.FromExternal,
                    DocumentType = new DocumentType
                    {
                        Code = x.InternalProcedure.DocumentType.Code,
                        Name = x.InternalProcedure.DocumentType.Name
                    }
                },
            })
           .ToListAsync();

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal,
            };
            return result;

        }
    }
}
