using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class RoleRepository : Repository<ApplicationRole>, IRoleRepository
    {
        public RoleRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<ApplicationRole>> GetProcedureRolesDatatable(DataTablesStructs.SentParameters sentParameters, Guid procedureId, Expression<Func<ApplicationRole, ApplicationRole>> selectPredicate = null, Expression<Func<ApplicationRole, dynamic>> orderByPredicate = null, Func<ApplicationRole, string[]> searchValuePredicate = null, string searchValue = null, List<string> rolesFiltered = null)
        {
            var query = _context.Roles.AsNoTracking();

            if (rolesFiltered != null && rolesFiltered.Any())
            {
                query = query.Where(x => rolesFiltered.Contains(x.Name));
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(q => q.Name.ToUpper().Contains(searchValue));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new ApplicationRole
                {
                    Id = x.Id,
                    Name = x.Name,
                    NormalizedName = x.NormalizedName,
                    IsInProcedure = x.ProcedureRoles.Any(i => i.ProcedureId == procedureId && i.RoleId == x.Id) ? true : false
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<ApplicationRole>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }


        private async Task<Select2Structs.ResponseParameters> GetRolesSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<ApplicationRole, Select2Structs.Result>> selectPredicate, Func<ApplicationRole, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Roles
                //.WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        #endregion

        #region PUBLIC

        public async Task<IEnumerable<ApplicationRole>> GetProcedureRoles(Guid procedureId)
        {
            var query = _context.Roles
                .Select(x => new ApplicationRole
                {
                    Id = x.Id,
                    ConcurrencyStamp = x.ConcurrencyStamp,
                    Name = x.Name,
                    NormalizedName = x.NormalizedName,
                    IsInProcedure = x.ProcedureRoles.Any(y => y.ProcedureId == procedureId)
                });

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<ApplicationRole>> GetProcedureRolesDatatable(DataTablesStructs.SentParameters sentParameters, Guid procedureId, string searchValue = null, List<string> rolesFiltered = null)
        {
            Expression<Func<ApplicationRole, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetProcedureRolesDatatable(sentParameters, procedureId, ExpressionHelpers.SelectApplicationRole(_context, procedureId), orderByPredicate, (x) => new[] { x.Name }, searchValue, rolesFiltered);
        }


        public async Task<ApplicationRole> GetByName(string roleName)
            => await _context.Roles.Where(x => x.Name == roleName).FirstOrDefaultAsync();

        public async Task<IEnumerable<ApplicationRole>> GetAllByName(IEnumerable<string> roleNames)
            => await _context.Roles.Where(x => roleNames.Contains(x.Name)).ToListAsync();

        public async Task<IEnumerable<ApplicationRole>> GetAllById(IEnumerable<string> roleIds)
            => await _context.Roles.Where(x => roleIds.Contains(x.Id)).ToListAsync();

        public async Task<Select2Structs.ResponseParameters> GetRolesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetRolesSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name,
            }, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<object> GetAllAsSelect2ClientSide()
        {
            var result = await _context.Roles
                        .Select(x => new
                        {
                            id = x.Id,
                            text = x.Name
                        }).ToListAsync();

            return result;
        }

        public async Task<object> GetAllAsSelect2ByStudentAndEnterpriseClientSide()
        {
            var result = await _context.Roles.Where(x => x.Name == ConstantHelpers.ROLES.STUDENTS || x.Name == ConstantHelpers.ROLES.ENTERPRISE)
               .Select(x => new
               {
                   id = x.Id,
                   text = x.Name
               }).ToListAsync();

            result.Insert(0, new { id = " ", text = "Todas" });
            return result;
        }

        public async Task<object> GetAllAsSelect2ClientSide(IEnumerable<string> exceptionNames)
        {
            var result = await _context.Roles
                        .Where(x => exceptionNames.All(e => e != x.Name))
                        .OrderBy(x => x.Name)
                        .Select(x => new
                        {
                            id = x.Id,
                            text = x.Name
                        }).ToListAsync();

            return result;
        }

        public async Task<ApplicationRole> GetRolByName(string name)
            => await _context.Roles.Where(x => x.Name == name).FirstOrDefaultAsync();

        public async Task<object> GetUnits()
        {
            var data = await _context.Roles
             .Where(x => x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.PROGRAMMING_UNIT] || x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.PROCUREMENTS_UNIT])
            .Select(x => new
            {
                id = x.Id,
                text = x.Name
            }).ToListAsync();

            return data;
        }

        public async Task<object> GetUnitMark()
        {
            var data = await _context.Roles
             .Where(x => x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.PROGRAMMING_UNIT])
            .Select(x => new
            {
                id = x.Id,
                text = x.Name
            }).ToListAsync();

            return data;
        }

        public async Task<object> GetUnitsPurchase()
        {
            var data = await _context.Roles
                 .Where(x => x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.PURCHASE_ORDERS_A] ||
                             x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.PURCHASE_ORDERS_B] ||
                             x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.PURCHASE_ORDERS_C] ||
                             x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.PURCHASE_ORDERS_D])
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).OrderBy(x => x.text).ToListAsync();

            return data;
        }

        public async Task<object> GetAdqui(int UserRequirementIndex)
        {
            var data = await _context.Roles
                .Where(x => x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.QUOTATIONS] ||
                            x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.COMPARATIVE_CHART_QUOTATIONS] ||
                            x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.PURCHASE_ORDERS_A] ||
                            x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.PURCHASE_ORDERS_B] ||
                            x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.PURCHASE_ORDERS_C] ||
                            x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.PURCHASE_ORDERS_D] ||
                            x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.PREVIOUS_CONTROL] ||
                            x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.PROCUREMENTS_UNIT] ||
                            x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.LOGISTIC])
               .Select(x => new
               {
                   id = x.Id,
                   text = x.Name,
                   rol = ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[UserRequirementIndex]
               }).OrderBy(x => x.text).ToListAsync();

            return data;
        }

        public async Task<object> GetMarketResearchProgramiming()
        {
            var data = await _context.Roles
             .Where(x => x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.MARKET_RESEARCH_PROGRAMMING_A] ||
                         x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.MARKET_RESEARCH_PROGRAMMING_B] ||
                         x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.MARKET_RESEARCH_PROGRAMMING_C] ||
                         x.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[ConstantHelpers.USER_REQUIREMENTES_ROLES.MARKET_RESEARCH_PROGRAMMING_D])
            .Select(x => new
            {
                id = x.Id,
                text = x.Name
            }).OrderBy(x => x.text).ToListAsync();

            return data;
        }

        #endregion
    }
}