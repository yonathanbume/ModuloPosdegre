using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class RequirementFileRepository : Repository<RequirementFile>, IRequirementFileRepository
    {
        public RequirementFileRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<RequirementFile, dynamic>> GetRequirementFilesDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.RequirementId);
                case "1":
                    return ((x) => x.RequirementId);
                default:
                    return ((x) => x.RequirementId);
            }
        }

        private Func<RequirementFile, string[]> GetRequirementFilesDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.FileName + "",
                x.Path + "",
                x.Size + ""
            };
        }

        private async Task<DataTablesStructs.ReturnedData<RequirementFile>> GetRequirementFilesDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<RequirementFile, RequirementFile>> selectPredicate = null, Expression<Func<RequirementFile, dynamic>> orderByPredicate = null, Func<RequirementFile, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.RequirementFiles
                .WhereSearchValue(searchValuePredicate, searchValue)
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC

        public async Task<DataTablesStructs.ReturnedData<RequirementFile>> GetRequirementFilesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await GetRequirementFilesDatatable(sentParameters, ExpressionHelpers.SelectRequirementFile(), GetRequirementFilesDatatableOrderByPredicate(sentParameters), GetRequirementFilesDatatableSearchValuePredicate(), searchValue);
        }

        #endregion
    }
}
