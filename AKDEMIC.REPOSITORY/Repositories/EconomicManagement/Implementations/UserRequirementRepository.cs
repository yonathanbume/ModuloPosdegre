using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.UserRequirement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class UserRequirementRepository : Repository<UserRequirement>, IUserRequirementRepository
    {
        public UserRequirementRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<UserRequirement, dynamic>> GetUserRequirementsDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
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

        private async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetHigherUITUserRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string requirementUserId = null, string roleId = null, Expression<Func<UserRequirement, UserRequirement>> selectPredicate = null, Expression<Func<UserRequirement, dynamic>> orderByPredicate = null, Func<UserRequirement, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var uit = _context.UITs
                .OrderByDescending(x => x.Year)
                .FirstOrDefault();
            var query = _context.UserRequirements
                .Where(x => x.Cost / uit.Value >= 8)
                .WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            if (requirementUserId != null)
            {
                query = query.Where(x => x.Requirement.UserId == requirementUserId);
            }

            if (roleId != null)
            {
                query = query.Where(x => x.RoleId == roleId);
            }

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        private async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetLowerUITUserRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string requirementUserId = null, string roleId = null, Expression<Func<UserRequirement, UserRequirement>> selectPredicate = null, Expression<Func<UserRequirement, dynamic>> orderByPredicate = null, Func<UserRequirement, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var uit = _context.UITs
                .OrderByDescending(x => x.Year)
                .FirstOrDefault();
            var query = _context.UserRequirements
                .Where(x => x.Cost / uit.Value <= 8)
                .WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            if (requirementUserId != null)
            {
                query = query.Where(x => x.Requirement.UserId == requirementUserId);
            }

            if (roleId != null)
            {
                query = query.Where(x => x.RoleId == roleId);
            }

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        private async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetUserRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string requirementUserId = null, string roleId = null, Expression<Func<UserRequirement, UserRequirement>> selectPredicate = null, Expression<Func<UserRequirement, dynamic>> orderByPredicate = null, Func<UserRequirement, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.UserRequirements
                .WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            if (requirementUserId != null)
            {
                query = query.Where(x => x.Requirement.UserId == requirementUserId);
            }

            if (roleId != null)
            {
                query = query.Where(x => x.RoleId == roleId);
            }

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC

        public async Task<List<UserRequirement>> GetListByRequirementId(Guid reqid)
            => await _context.UserRequirements.Where(x => x.RequirementId == reqid).ToListAsync();
        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetHigherUITUserRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await GetHigherUITUserRequirementsDatatable(sentParameters, null, null, ExpressionHelpers.SelectUserRequirement(), GetUserRequirementsDatatableOrderByPredicate(sentParameters), (x) => new[] { x.Requirement.Subject + "" }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetHigherUITUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string searchValue = null)
        {
            return await GetHigherUITUserRequirementsDatatable(sentParameters, requirementUserId, null, ExpressionHelpers.SelectUserRequirement(), GetUserRequirementsDatatableOrderByPredicate(sentParameters), (x) => new[] { x.Requirement.Subject + "" }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetHigherUITUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string roleId, string searchValue = null)
        {
            return await GetHigherUITUserRequirementsDatatable(sentParameters, requirementUserId, roleId, ExpressionHelpers.SelectUserRequirement(), GetUserRequirementsDatatableOrderByPredicate(sentParameters), (x) => new[] { x.Requirement.Subject + "" }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetHigherUITUserRequirementsDatatableByRole(DataTablesStructs.SentParameters sentParameters, string roleId, string searchValue = null)
        {
            return await GetHigherUITUserRequirementsDatatable(sentParameters, null, roleId, ExpressionHelpers.SelectUserRequirement(), GetUserRequirementsDatatableOrderByPredicate(sentParameters), (x) => new[] { x.Requirement.Subject + "" }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetLowerUITUserRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await GetLowerUITUserRequirementsDatatable(sentParameters, null, null, ExpressionHelpers.SelectUserRequirement(), GetUserRequirementsDatatableOrderByPredicate(sentParameters), (x) => new[] { x.Requirement.Subject + "" }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetLowerUITUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string searchValue = null)
        {
            return await GetLowerUITUserRequirementsDatatable(sentParameters, requirementUserId, null, ExpressionHelpers.SelectUserRequirement(), GetUserRequirementsDatatableOrderByPredicate(sentParameters), (x) => new[] { x.Requirement.Subject + "" }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetLowerUITUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string roleId, string searchValue = null)
        {
            return await GetLowerUITUserRequirementsDatatable(sentParameters, requirementUserId, roleId, ExpressionHelpers.SelectUserRequirement(), GetUserRequirementsDatatableOrderByPredicate(sentParameters), (x) => new[] { x.Requirement.Subject + "" }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetLowerUITUserRequirementsDatatableByRole(DataTablesStructs.SentParameters sentParameters, string roleId, string searchValue = null)
        {
            return await GetLowerUITUserRequirementsDatatable(sentParameters, null, roleId, ExpressionHelpers.SelectUserRequirement(), GetUserRequirementsDatatableOrderByPredicate(sentParameters), (x) => new[] { x.Requirement.Subject + "" }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetUserRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await GetUserRequirementsDatatable(sentParameters, null, null, ExpressionHelpers.SelectUserRequirement(), GetUserRequirementsDatatableOrderByPredicate(sentParameters), (x) => new[] { x.Requirement.Subject + "" }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string searchValue = null)
        {
            return await GetUserRequirementsDatatable(sentParameters, requirementUserId, null, ExpressionHelpers.SelectUserRequirement(), GetUserRequirementsDatatableOrderByPredicate(sentParameters), (x) => new[] { x.Requirement.Subject + "" }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string roleId, string searchValue = null)
        {
            return await GetUserRequirementsDatatable(sentParameters, requirementUserId, roleId, ExpressionHelpers.SelectUserRequirement(), GetUserRequirementsDatatableOrderByPredicate(sentParameters), (x) => new[] { x.Requirement.Subject + "" }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetUserRequirementsDatatableByRole(DataTablesStructs.SentParameters sentParameters, string roleId, string searchValue = null)
        {
            return await GetUserRequirementsDatatable(sentParameters, null, roleId, ExpressionHelpers.SelectUserRequirement(), GetUserRequirementsDatatableOrderByPredicate(sentParameters), (x) => new[] { x.Requirement.Subject + "" }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRequirementDatatable(DataTablesStructs.SentParameters sentParameters, int UserRequirementIndex, Guid dependencyId, int filterValue, string code, int status)
        {
            var uit = await _context.UITs.OrderByDescending(e => e.Id).FirstOrDefaultAsync();
            var Roles = new List<ApplicationRole>();
            Roles = await _context.Roles.Where(x => x.Name == ConstantHelpers.ROLES.COST_CENTER ||
                                                   x.Name == ConstantHelpers.ROLES.PROGRAMMING_UNIT ||
                                                   x.Name == ConstantHelpers.ROLES.BUDGET_OFFICE ||
                                                   x.Name == ConstantHelpers.ROLES.PROCUREMENTS_UNIT ||
                                                   x.Name == ConstantHelpers.ROLES.SELECTION_PROCESS_UNIT ||
                                                   x.Name == ConstantHelpers.ROLES.MARKET_RESEARCH_PROGRAMMING_A ||
                                                   x.Name == ConstantHelpers.ROLES.MARKET_RESEARCH_PROGRAMMING_B ||
                                                   x.Name == ConstantHelpers.ROLES.MARKET_RESEARCH_PROGRAMMING_C ||
                                                   x.Name == ConstantHelpers.ROLES.MARKET_RESEARCH_PROGRAMMING_D ||
                                                   x.Name == ConstantHelpers.ROLES.QUOTATIONS ||
                                                   x.Name == ConstantHelpers.ROLES.COMPARISON_CHART_QUOTES ||
                                                   x.Name == ConstantHelpers.ROLES.PURCHASE_ORDERS_A ||
                                                   x.Name == ConstantHelpers.ROLES.PURCHASE_ORDERS_B ||
                                                   x.Name == ConstantHelpers.ROLES.PURCHASE_ORDERS_C ||
                                                   x.Name == ConstantHelpers.ROLES.PURCHASE_ORDERS_D ||
                                                   x.Name == ConstantHelpers.ROLES.PREVIOUS_CONTROL ||
                                                   x.Name == ConstantHelpers.ROLES.REQUIREMENT_RECORD ||
                                                   x.Name == ConstantHelpers.ROLES.LOGISTIC).ToListAsync();

            Expression<Func<UserRequirement, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Requirement.CodeNumber;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Requirement.User.UserDependencies.Select(y => y.Dependency.Name).FirstOrDefault();
                    break;
                case "2":
                    orderByPredicate = (x) => x.Requirement.Subject;
                    break;
                case "3":
                    orderByPredicate = (x) => x.UpdateStatus.HasValue ? x.UpdateStatus.Value.ToLocalDateTimeFormat() : (x.Requirement.CreatedAt.HasValue ? x.Requirement.CreatedAt.Value.ToLocalDateTimeFormat() : "");
                    break;
                case "4":
                    orderByPredicate = (x) => x.Requirement.Folio;
                    break;
                case "5":
                    orderByPredicate = (x) => ConstantHelpers.USER_REQUIREMENTS.STATUS.VALUES[x.Status];
                    break;
                case "6":
                    orderByPredicate = (x) => x.Role.NormalizedName;
                    break;
                default:
                    orderByPredicate = (x) => x.Requirement.CreatedAt;
                    break;
            }


            var query = _context.UserRequirements.Include(x => x.Role).Include(x => x.Requirement).Where(x => Roles.Contains(x.Role)).AsNoTracking();


            if (filterValue == 1)
            {
                query = query.Where(x => x.Cost != null && ((x.Cost.Value / uit.Value) < 8));
            }
            if (filterValue == 2)
            {
                query = query.Where(x => x.Cost != null && ((x.Cost.Value / uit.Value) > 8));
            }
            if (dependencyId != Guid.Empty)
            {
                query = query.Where(x => x.Requirement.DependencyId == dependencyId);

            }
            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(q => q.Requirement.CodeNumber.Contains(code));
            }

            if (status != 0)
            {
                query = query.Where(q => q.Status == status);
            }

            if (UserRequirementIndex != 1)
            {
                query = query.Where(x => x.Role.Name != ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[1]);
            }

            var recordsFiltered = await query.CountAsync();
            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    Code = x.Requirement.CodeNumber,
                    x.Requirement.Subject,
                    updatedAt = x.UpdateStatus.HasValue ? x.UpdateStatus.Value.ToLocalDateTimeFormat() : (x.Requirement.CreatedAt.HasValue ? x.Requirement.CreatedAt.Value.ToLocalDateTimeFormat() : ""),
                    meta = _context.CatalogItemGoals.Include(i => i.CatalogGoal).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogGoal.Description).FirstOrDefault(),
                    area = x.Role.NormalizedName,
                    dependency = x.Requirement.Dependency.Name,
                    canDerive = (x.Role.Name == ConstantHelpers.USER_REQUIREMENTES_ROLES.VALUES[UserRequirementIndex]) ? true : false,
                    StatusInt = x.Status,
                    Status = ConstantHelpers.USER_REQUIREMENTS.STATUS.VALUES[x.Status],
                    UserRequirementIndex,
                    HasOrder = x.OrderId.HasValue ? true : false,
                    less8Uit = (x.Cost != null && ((x.Cost.Value / uit.Value) < 8)) ? true : false,
                    item = _context.UserRequirementItems.Include(i => i.CatalogItem).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogItem.Description).FirstOrDefault(),
                    secFunction = _context.CatalogItemGoals.Include(i => i.CatalogGoal).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogGoal.SecFunc).FirstOrDefault(),
                    program = _context.CatalogItemGoals.Include(i => i.CatalogGoal).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogGoal.Program).FirstOrDefault(),
                    prodPry = _context.CatalogItemGoals.Include(i => i.CatalogGoal).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogGoal.ProdPry).FirstOrDefault(),
                    actWork = _context.CatalogItemGoals.Include(i => i.CatalogGoal).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogGoal.ActWork).FirstOrDefault(),
                    function = _context.CatalogItemGoals.Include(i => i.CatalogGoal).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogGoal.Function).FirstOrDefault(),
                    divisionFunc = _context.CatalogItemGoals.Include(i => i.CatalogGoal).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogGoal.DivisionFunc).FirstOrDefault()
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

        public async Task<List<UserRequirementTemplate>> GetRequirementDatatableToReport(int UserRequirementIndex, Guid? dependencyId = null, int? filterValue = null, string code = null)
        {
            var uit = await _context.UITs.OrderByDescending(e => e.Id).FirstOrDefaultAsync();
            var Roles = new List<ApplicationRole>();
            Roles = await _context.Roles.Where(x => x.Name == ConstantHelpers.ROLES.COST_CENTER ||
                                                   x.Name == ConstantHelpers.ROLES.PROGRAMMING_UNIT ||
                                                   x.Name == ConstantHelpers.ROLES.BUDGET_OFFICE ||
                                                   x.Name == ConstantHelpers.ROLES.PROCUREMENTS_UNIT ||
                                                   x.Name == ConstantHelpers.ROLES.SELECTION_PROCESS_UNIT ||
                                                   x.Name == ConstantHelpers.ROLES.MARKET_RESEARCH_PROGRAMMING_A ||
                                                   x.Name == ConstantHelpers.ROLES.MARKET_RESEARCH_PROGRAMMING_B ||
                                                   x.Name == ConstantHelpers.ROLES.MARKET_RESEARCH_PROGRAMMING_C ||
                                                   x.Name == ConstantHelpers.ROLES.MARKET_RESEARCH_PROGRAMMING_D ||
                                                   x.Name == ConstantHelpers.ROLES.QUOTATIONS ||
                                                   x.Name == ConstantHelpers.ROLES.COMPARISON_CHART_QUOTES ||
                                                   x.Name == ConstantHelpers.ROLES.PURCHASE_ORDERS_A ||
                                                   x.Name == ConstantHelpers.ROLES.PURCHASE_ORDERS_B ||
                                                   x.Name == ConstantHelpers.ROLES.PURCHASE_ORDERS_C ||
                                                   x.Name == ConstantHelpers.ROLES.PURCHASE_ORDERS_D ||
                                                   x.Name == ConstantHelpers.ROLES.PREVIOUS_CONTROL ||
                                                   x.Name == ConstantHelpers.ROLES.REQUIREMENT_RECORD ||
                                                   x.Name == ConstantHelpers.ROLES.LOGISTIC).ToListAsync();


            var query = _context.UserRequirements.Include(x => x.Role).Include(x => x.Requirement).ThenInclude(x => x.User).Where(x => Roles.Contains(x.Role)).AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (filterValue == 1)
            {
                query = query.Where(x => x.Cost != null && ((x.Cost.Value / uit.Value) < 8));
            }
            if (filterValue == 2)
            {
                query = query.Where(x => x.Cost != null && ((x.Cost.Value / uit.Value) > 8));
            }

            if (dependencyId.HasValue)
            {
                if (dependencyId != Guid.Empty)
                {
                    query = query.Where(x => x.Requirement.DependencyId == dependencyId);

                }
            }

            if (!string.IsNullOrEmpty(code))
            {
                query = query.Where(q => q.Requirement.CodeNumber.Contains(code));
            }

            query = query.AsQueryable();

            var data = await query
                .Select(x => new UserRequirementTemplate
                {
                    CreateAt = x.Requirement.CreatedAt,
                    Code = x.Requirement.CodeNumber,
                    Subject = x.Requirement.Subject,
                    Anio = x.UpdateStatus.HasValue ? x.UpdateStatus.Value.Year.ToString() : "",
                    UpdatedAt = x.UpdateStatus.HasValue ? x.UpdateStatus.Value.ToLocalDateTimeFormat() : (x.Requirement.CreatedAt.HasValue ? x.Requirement.CreatedAt.Value.ToLocalDateTimeFormat() : ""),
                    Area = x.Role.NormalizedName,
                    CodeDependency = x.Requirement.Dependency.Acronym,
                    Dependency = x.Requirement.Dependency.Name,
                    Status = ConstantHelpers.USER_REQUIREMENTS.STATUS.VALUES[x.Status],
                    Item = _context.UserRequirementItems.Include(i => i.CatalogItem).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogItem.Description).FirstOrDefault(),
                    CodeItem = _context.UserRequirementItems.Include(i => i.CatalogItem).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogItem.Code).FirstOrDefault(),
                    TypeItem = _context.UserRequirementItems.Include(i => i.CatalogItem).Where(i => i.UserRequirementId == x.Id).Select(a => ConstantHelpers.ECONOMICMANAGEMENT.TYPECATALOG.VALUES[a.CatalogItem.Type]).FirstOrDefault(),
                    UnitMeasurementItem = _context.UserRequirementItems.Include(i => i.CatalogItem).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogItem.UnitMeasurement).FirstOrDefault(),
                    CommentItem = !string.IsNullOrEmpty(_context.UserRequirementItems.Where(i => i.UserRequirementId == x.Id).Select(a => a.Comment).FirstOrDefault()) ? _context.UserRequirementItems.Where(i => i.UserRequirementId == x.Id).Select(a => a.Comment).FirstOrDefault() : "",
                    QuantityItem = _context.UserRequirementItems.Where(i => i.UserRequirementId == x.Id).Select(a => a.Quantity.ToString()).FirstOrDefault(),
                    ValueItem = _context.UserRequirementItems.Where(i => i.UserRequirementId == x.Id).Select(a => a.Value.ToString()).FirstOrDefault(),
                    UserId = x.Requirement.User.UserName,
                    UserName = x.Requirement.User.FullName,
                    SecFunction = _context.CatalogItemGoals.Include(i => i.CatalogGoal).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogGoal.SecFunc).FirstOrDefault(),
                    Meta = _context.CatalogItemGoals.Include(i => i.CatalogGoal).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogGoal.Description).FirstOrDefault(),
                    Program = _context.CatalogItemGoals.Include(i => i.CatalogGoal).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogGoal.Program).FirstOrDefault(),
                    ProdPry = _context.CatalogItemGoals.Include(i => i.CatalogGoal).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogGoal.ProdPry).FirstOrDefault(),
                    ActWork = _context.CatalogItemGoals.Include(i => i.CatalogGoal).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogGoal.ActWork).FirstOrDefault(),
                    Function = _context.CatalogItemGoals.Include(i => i.CatalogGoal).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogGoal.Function).FirstOrDefault(),
                    DivisionFunc = _context.CatalogItemGoals.Include(i => i.CatalogGoal).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogGoal.DivisionFunc).FirstOrDefault(),
                    Classifier = _context.UserRequirementItems.Include(i => i.Classifier).Where(i => i.UserRequirementId == x.Id).Select(a => a.Classifier.Name).FirstOrDefault(),
                    Activity = _context.UserRequirementItems.Include(i => i.CatalogActivity).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogActivity.Name).FirstOrDefault(),
                    IsPac = x.IsPAC ? "Sí" : "No"

                }).OrderByDescending(x => x.CreateAt).ToListAsync();

            return data;
        }

        public async Task<UserRequirement> GetUserRequirementById(Guid id)
            => await _context.UserRequirements
            .Include(x => x.Requirement)
            .Include(x => x.Role)
            .Where(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<object> GetUnitProgram(Guid id)
        {
            var userRequirement = await _context.UserRequirements.Where(x => x.Id == id)
                .Select(x => new
                {
                    x.IsPAC,
                    x.Comment,
                    x.SubStatus,
                    x.Cost,
                    x.Status
                })
                .FirstOrDefaultAsync();

            return userRequirement;
        }

        public async Task<object> GetBudgetOffice(Guid id)
        {
            var userRequirement = await _context.UserRequirements.Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Cost
                })
                .FirstOrDefaultAsync();

            return userRequirement;
        }

        public async Task<UserRequirement> GetWithIncludesId(Guid id)
            => await _context.UserRequirements
            .Include(x => x.Role)
            .Include(x => x.ExecuteObservation)
            .Include(x => x.Order)
            .Include(x => x.Requirement).ThenInclude(x => x.Supplier)
            .Include(x => x.Requirement).ThenInclude(x => x.RequirementSuppliers).ThenInclude(x => x.Supplier)
            .Include(x => x.Requirement).ThenInclude(x => x.Dependency)
            .Include(x => x.Requirement).ThenInclude(x => x.PhaseRequirement)
            .Include(x => x.Requirement).ThenInclude(x => x.User).ThenInclude(x => x.UserDependencies).ThenInclude(x => x.Dependency)
            .Where(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<string> GetItemById(Guid id)
            => await _context.UserRequirementItems
            .Include(i => i.CatalogItem)
            .Where(i => i.UserRequirementId == id).Select(a => a.CatalogItem.Description).FirstOrDefaultAsync();

        public async Task SaveCHanges()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetQuantityItem(Guid id)
            => await _context.UserRequirementItems.Where(i => i.UserRequirementId == id).Select(a => a.Quantity.HasValue ? a.Quantity.Value : 0).FirstOrDefaultAsync(); 

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserRequrimentByOrder(DataTablesStructs.SentParameters sentParameters, Guid orderId)
        {
            Expression<Func<UserRequirement, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => _context.UserRequirementItems.Include(i => i.CatalogItem).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogItem.Description).FirstOrDefault());

                    break;
                case "2":
                    orderByPredicate = ((x) =>  _context.UserRequirementItems.Include(i => i.CatalogItem).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogItem.UnitMeasurement).FirstOrDefault());

                    break;
                case "3":
                    orderByPredicate = ((x) => _context.UserRequirementItems.Where(i => i.UserRequirementId == x.Id).Select(a => a.Quantity.ToString()).FirstOrDefault());

                    break;
                case "4":
                    orderByPredicate = ((x) => _context.ReceivedOrders.Where(i => i.UserRequirementId == x.Id).Select(a => a.QuantityReceived).FirstOrDefault());
                    break;
                default:
                    orderByPredicate = ((x) => x.UpdateAsignOrder);
                    break;
            }

            var query = _context.UserRequirements.Where(x => x.OrderId == orderId).AsNoTracking();


            var recordsFiltered = await query.CountAsync();

            query = query
                .AsQueryable();


            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    item = _context.UserRequirementItems.Include(i => i.CatalogItem).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogItem.Description).FirstOrDefault(),
                    unitMeasurementItem = _context.UserRequirementItems.Include(i => i.CatalogItem).Where(i => i.UserRequirementId == x.Id).Select(a => a.CatalogItem.UnitMeasurement).FirstOrDefault(),
                    quantityItem = _context.UserRequirementItems.Where(i => i.UserRequirementId == x.Id).Select(a => a.Quantity.ToString()).FirstOrDefault(), 
                    received = _context.ReceivedOrders.Where(i => i.UserRequirementId == x.Id).Select(a => a.QuantityReceived).FirstOrDefault(),
                    update = x.UpdateAsignOrder.HasValue ? x.UpdateAsignOrder.ToLocalDateTimeFormat() : "-",
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<UserRequirementOrderDetailTemplate> GetFirstUserRequirementOrderDetail(string supplierRuc, int orderNumber)
        {
            var result = await _context.UserRequirements
                .Where(x => x.Requirement.Supplier.RUC == supplierRuc && x.Order.Number == orderNumber)
                .Select(x => new UserRequirementOrderDetailTemplate
                {
                    Id = x.Id,
                    FundingSource = x.Order.FundingSource,
                    FundingSourceText = ConstantHelpers.ORDERS.FUNDING_SOURCE.VALUES.ContainsKey(x.Order.FundingSource) ? 
                                ConstantHelpers.ORDERS.FUNDING_SOURCE.VALUES[x.Order.FundingSource] : "Desconocido",
                    IsDelivered = false, //En duro por ahora
                    IsDeliveredText = ConstantHelpers.ORDERS.DELIVERY.VALUES.ContainsKey(false) ?
                        ConstantHelpers.ORDERS.DELIVERY.VALUES[false] : "Desconocido",
                    Status = x.Order.Status,
                    StatusText = ConstantHelpers.ORDERS.STATUS.VALUES.ContainsKey(x.Order.Status) ?
                        ConstantHelpers.ORDERS.STATUS.VALUES[x.Order.Status] : "Desconocido",
                    Type = x.Order.Type, //Esto tambien se necesita cambiar
                    TypeText = ConstantHelpers.ORDERS.TYPE.VALUES.ContainsKey(x.Order.Type) ?
                        ConstantHelpers.ORDERS.TYPE.VALUES[x.Order.Type] : "Desconocido",
                    RequirementId = x.RequirementId,
                    Title = x.Order.Title,
                    Dependency = x.Requirement.Dependency.Name,
                    Description = x.Order.Description,                    
                    Item = x.UserRequirementItems.Where(y => y.UserRequirementId == x.Id).Select(y => y.CatalogItem.Description).FirstOrDefault(),
                    StartDate = x.Order.StartDate.ToLocalDateFormat(),
                    EndDate = x.Order.EndDate.ToLocalDateFormat(),
                    SupplierName = x.Requirement.Supplier.Name,
                    SupplierRUC = x.Requirement.Supplier.RUC,
                    Cost = x.Order.Cost
                })
                .FirstOrDefaultAsync();


            return result;
        }
        #endregion
    }
}
