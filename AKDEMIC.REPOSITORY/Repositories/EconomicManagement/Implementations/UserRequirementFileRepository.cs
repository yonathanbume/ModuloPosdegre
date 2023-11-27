using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class UserRequirementFileRepository : Repository<UserRequirementFile>, IUserRequirementFileRepository
    {
        public UserRequirementFileRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<UserRequirementFile, dynamic>> GetUserRequirementFilesDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.UserRequirementId);
                case "1":
                    return ((x) => x.UserRequirementId);
                default:
                    return ((x) => x.UserRequirementId);
            }
        }

        private async Task<DataTablesStructs.ReturnedData<UserRequirementFile>> GetUserRequirementFilesDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<UserRequirementFile, UserRequirementFile>> selectPredicate = null, Expression<Func<UserRequirementFile, dynamic>> orderByPredicate = null, Func<UserRequirementFile, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.UserRequirementFiles
                .WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC

        public async Task<DataTablesStructs.ReturnedData<UserRequirementFile>> GetUserRequirementFilesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await GetUserRequirementFilesDatatable(sentParameters, ExpressionHelpers.SelectUserRequirementFile(), GetUserRequirementFilesDatatableOrderByPredicate(sentParameters), (x) => new[] { x.UserRequirementId + "", x.UserRequirementId + "" }, searchValue);
        }
        public IQueryable<UserRequirementFile> GetQueryable(Guid id)
            => _context.UserRequirementFiles.Where(x => x.UserRequirementId == id).AsQueryable();

        public async Task<object> GetUserRequirementFilesDetail(Guid id)
        {
            var result = await _context.UserRequirementFiles.Where(x => x.UserRequirementId == id)
            .Select(x => new
            {
                id = x.Id,
                name = x.FileName,
                size = x.Size,
                isdatabase = true
            }).ToListAsync();

            return result;
        }

        public async Task<List<UserRequirementFile>> GetLstDeleteds(List<Guid> LstDeleteds) 
            => await _context.UserRequirementFiles.Where(x => LstDeleteds.Contains(x.Id)).ToListAsync();

        #endregion
    }
}
