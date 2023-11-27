using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class CriterionRepository : Repository<Criterion> , ICriterionRepository
    {
        public CriterionRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<Criterion,dynamic>> GetCriterionDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Code);
                case "1":
                    return ((x) => x.Name);
                //case "2":
                //    return ((x) => x.Career.Name);
                default:
                    return ((x) => x.Name);
            }
        }

        private Func<Criterion, string[]> GetCriterionDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Code+"",
                x.Name+"",
                //x.Career.Name+"",
                x.Name+""
            };
        }

        private async Task<DataTablesStructs.ReturnedData<Criterion>> GetCriterionDatatable(DataTablesStructs.SentParameters sentParameters,string name,Expression<Func<Criterion,Criterion>> selectPredicate = null,Expression<Func<Criterion,dynamic>> orderByPredicate=null,Func<Criterion,string[]> searchValuePredicate = null,string userId = null,string searchValue= null)
        {
            var query = _context.Criterions
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(name.Trim().ToLower()));

            //if (!string.IsNullOrEmpty(userId))
            //{
            //    var careerId = await _context.Teachers.Where(x => x.UserId == userId).Select(x => x.CareerId).FirstOrDefaultAsync();
            //    query = query.Where(x => careerId == x.CareerId);
            //}

            var result = query
                .Select(x => new Criterion
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    RelatedStandars=x.RelatedStandars,
                    //CareerName = x.Career.Name,
                    UrlFile = x.UrlFile
                })
                .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);

        }

        private async Task<Select2Structs.ResponseParameters> GetCriterionsByAcademicProgramIdSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Criterion, Select2Structs.Result>> selectPredicate, Func<Criterion, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Criterions
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        #endregion

        #region PUBLIC

        public async Task<DataTablesStructs.ReturnedData<Criterion>> GetCriterionsDatatable(DataTablesStructs.SentParameters sentParameters, string name = null, string userId = null,string searchValue = null)
            => await GetCriterionDatatable(sentParameters, name, null, GetCriterionDatatableOrderByPredicate(sentParameters), GetCriterionDatatableSearchValuePredicate(), userId,searchValue);

        public async Task<bool> ExistName(string name, Guid? currentCriterionId = null)
        {
            var query = _context.Criterions.AsQueryable();

            if (currentCriterionId.HasValue)
            {
                var result = await query.AnyAsync(x => x.Name.Trim().ToLower() == name.Trim().ToLower() /*&& x.CareerId == careerId*/ && currentCriterionId != x.Id);
                return result;
            }
            else
            {
                var result = await query.AnyAsync(x => x.Name.Trim().ToLower() == name.Trim().ToLower() /*&& x.CareerId == careerId*/);
                return result;
            }

        }

        public async Task<bool> ExistCode(string code, Guid? currentCriterionId = null)
        {
            var query = _context.Criterions.AsQueryable();

            if (currentCriterionId.HasValue)
            {
                var result = await _context.Criterions.AnyAsync(x => x.Code.Trim().ToLower() == code.Trim().ToLower() /*&& x.CareerId == careerId */&& currentCriterionId != x.Id );
                return result;
            }
            else
            {
                var result = await _context.Criterions.AnyAsync(x => x.Code.Trim().ToLower() == code.Trim().ToLower() /*&& x.CareerId == careerId*/);
                return result;
            }
        }

        public async Task<Select2Structs.ResponseParameters> GetCriterionsByAcademicProgramIdSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetCriterionsByAcademicProgramIdSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name,
            }, (x) => new[] { x.Name }, searchValue);
        }

        #endregion
    }
}
