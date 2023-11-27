using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using AKDEMIC.REPOSITORY.Base;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.ResolutiveActs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.REPOSITORY.Extensions;
using System.Linq;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System.Security.Claims;

namespace AKDEMIC.REPOSITORY.Repositories.ResolutiveActs.Implementations
{
    public class DocumentRepository : Repository<Document>,IDocumentRepository
    {
        public DocumentRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private Expression<Func<Document,dynamic>> GetDocumentsDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Number);
                case "1":
                    return ((x) => x.Matter);
                case "2":
                    return ((x) => x.Sorter.Name);
                case "3":
                    return ((x) => x.ResolutionDate);
                case "4":
                    return ((x) => x.Dependency.Name);
                default:
                    return ((x) => x.CreatedAt);
            }
        }

        private Func<Document,string[]> GetDocumentsDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Number + "",
                x.SorterId + "",
                x.ResolutionDate + "",
                x.Matter + "",
                x.Dependency.Name + "",
            };
        }

        private async Task<DataTablesStructs.ReturnedData<Document>> GetDocumentsDatatable(
            DataTablesStructs.SentParameters sentParameters, 
            string number, string matter, Guid? dependencyId, 
            string resolutionDate,Guid? sorterId, Guid? categoryId,string startDate,
            string endDate,Expression<Func<Document, Document>> selectPredicate = null, 
            Expression<Func<Document, dynamic>> orderByPredicate = null, Func<Document, string[]> searchValuePredicate = null, 
            string searchValue = null, ClaimsPrincipal user = null, Guid? facultyId = null, 
            int? year = null, byte? status = null, byte? type = null)
        {
            var query = _context.Documents
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION,orderByPredicate)
                .AsNoTracking();

            if(user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if(user.IsInRole(ConstantHelpers.ROLES.LEGAL_SECREATARY))
                {
                    query = query.Where(x => x.Dependency.LegalSecretaryId == userId);
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.RESOLUTIVE_ACTS_VALIDATOR))
                {
                    query = query.Where(x => x.Dependency.ResolutiveActsValidatorId == userId && (x.Status == ConstantHelpers.RESOLUTIVE_ACT.DOCUMENT.STATUS.GENERATED || x.Status == ConstantHelpers.RESOLUTIVE_ACT.DOCUMENT.STATUS.APPROVED || x.Status == ConstantHelpers.RESOLUTIVE_ACT.DOCUMENT.STATUS.DENIED));
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.RESOLUTIVE_ACTS_DIRECTOR))
                {
                    query = query.Where(x => x.Dependency.ResolutiveActsDirectorId == userId && ( x.Status == ConstantHelpers.RESOLUTIVE_ACT.DOCUMENT.STATUS.SUBSCRIBE || x.Status == ConstantHelpers.RESOLUTIVE_ACT.DOCUMENT.STATUS.FINISHED) );
                }
            }

            if (status!=0)
                query = query.Where(x => x.Status == status);

            if (type!=0)
                query = query.Where(x => x.Type == type);

            if (!string.IsNullOrEmpty(number))
                query = query.Where(x => x.Number.Trim().ToLower().Contains(number.Trim().ToLower()));

            if (!string.IsNullOrEmpty(matter))
                query = query.Where(x => x.Matter.Trim().ToLower().Contains(matter.Trim().ToLower()));

            if (dependencyId.HasValue)
                query = query.Where(x => x.DependencyId == dependencyId);

            if (!string.IsNullOrEmpty(resolutionDate))
            {
                var datepickerUtc = ConvertHelpers.DatepickerToUtcDateTime(resolutionDate);
                query = query.Where(x => x.ResolutionDate == datepickerUtc);
            }

            if (sorterId.HasValue)
                query = query.Where(x => x.SorterId == sorterId);

            if (categoryId.HasValue)
                query = query.Where(x => x.ResolutionCategoryId == categoryId);

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                var startDateUTC = ConvertHelpers.DatepickerToUtcDateTime(startDate);
                var endDateUTC = ConvertHelpers.DatepickerToUtcDateTime(endDate);
                query = query.Where(x => x.ResolutionDate >= startDateUTC &&
                    x.ResolutionDate <= endDateUTC);
            }

            if (year != null)
            {
                query = query.Where(x => x.ResolutionDate.Year == year);
            }

            var result = query
                .Select(x => new Document
                {
                    Id = x.Id,
                    Number = x.Number,
                    Sorter = x.Sorter != null ? new Sorter
                    {
                        Id = x.Sorter.Id,
                        Name = x.Sorter.Name
                    } : null,
                    ResolutionCategory = x.ResolutionCategory != null ? new ResolutionCategory
                    {
                        Id = x.ResolutionCategory.Id,
                        Name = x.ResolutionCategory.Name
                    } : null,
                    ResolutionDate = x.ResolutionDate,
                    Matter =  x.Matter,
                    Status = x.Status,
                    StatusStr = ConstantHelpers.RESOLUTIVE_ACT.DOCUMENT.STATUS.VALUES[x.Status],
                    Dependency = x.Dependency,
                    DocumentFiles = x.DocumentFiles.Select(y=> new DocumentFile
                    {
                        UrlFile = y.UrlFile,
                        Name = y.Name
                    }).ToList()
                }, searchValue)
                .AsNoTracking();

            return await result.ToDataTables(sentParameters,selectPredicate);
    
        }

        private async Task<DataTablesStructs.ReturnedData<Document>> GetDocumentsDatatableByUserId(DataTablesStructs.SentParameters sentParameters,string userId,string numberOfAct, string matter,Guid? dependencyId,string date, Expression<Func<Document, Document>> selectPredicate = null, Expression<Func<Document, dynamic>> orderByPredicate = null, Func<Document, string[]> searchValuePredicate = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            var dependencies = await _context.UserDependencies.Where(x => x.UserId == userId).Select(x=>x.DependencyId).ToArrayAsync();
            var query = _context.Documents
                .Where(x => dependencies.Contains(x.DependencyId))
                //.WhereSearchValue(searchValuePredicate, searchValue)
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsQueryable();

            if (!string.IsNullOrEmpty(numberOfAct))
                query = query.Where(x => x.Number.Trim().ToLower().Contains(numberOfAct.Trim().ToLower()));

            if (!string.IsNullOrEmpty(matter))
                query = query.Where(x => x.Matter.Trim().ToLower().Contains(matter.Trim().ToLower()));

            if (dependencyId.HasValue)
                query = query.Where(x => x.DependencyId == dependencyId);

            if (!string.IsNullOrEmpty(date))
            {
                var dateUTC = ConvertHelpers.DatepickerToUtcDateTime(date);
                query = query.Where(x => x.ResolutionDate == dateUTC);
            }

            var result = query
                .Select(x => new Document
                {
                    Id = x.Id,
                    Number = x.Number,
                    Sorter = new Sorter
                    {
                        Id = x.Sorter.Id,
                        Name = x.Sorter.Name
                    },
                    ResolutionCategory = new ResolutionCategory
                    {
                        Id = x.ResolutionCategory.Id,
                        Name = x.ResolutionCategory.Name
                    },
                    ResolutionDate = x.ResolutionDate,
                    Matter = x.Matter,
                    Dependency = x.Dependency
                }, searchValue)
                .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC

        public async Task<DataTablesStructs.ReturnedData<Document>> GetDocumentsDatatable(DataTablesStructs.SentParameters sentParameters, string number = null, string matter = null, Guid? dependencyId = null, string resolutionDate = null,Guid? sorterId = null ,Guid? categoryId = null,string startDate = null,string endDate = null,string searchValue = null, ClaimsPrincipal user = null, Guid? facultyId = null, int? year = null, byte? status = null, byte? type = null)
            => await GetDocumentsDatatable(sentParameters, number, matter, dependencyId, resolutionDate,sorterId,categoryId,startDate,endDate, null, GetDocumentsDatatableOrderByPredicate(sentParameters), GetDocumentsDatatableSearchValuePredicate(), searchValue, user, facultyId, year, status , type);

        public async Task<DataTablesStructs.ReturnedData<Document>> GetDocumentsDatatableByUserId(DataTablesStructs.SentParameters sentParameters, string userId, string numberOfAct = null, string matter = null, Guid? dependencyId = null, string date = null, string searchValue = null)
            => await GetDocumentsDatatableByUserId(sentParameters, userId, numberOfAct, matter, dependencyId, date, null, GetDocumentsDatatableOrderByPredicate(sentParameters), GetDocumentsDatatableSearchValuePredicate(), searchValue);

        public async Task<IEnumerable<Select2Structs.Result>> GetDependencies()
        {
            var dependencies = await _context.Dependencies
               .Select(x => new Select2Structs.Result
               {
                   Id = x.Id,
                   Text = x.Name
               }).ToArrayAsync();

            return dependencies;
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetDependenciesByUserId(string userId)
        {
            var dependencies = await _context.Dependencies
                .Where(x=>x.UserDependencies.Any(y=>y.UserId == userId))
               .Select(x => new Select2Structs.Result
               {
                   Id = x.Id,
                   Text = x.Name
               }).ToArrayAsync();

            return dependencies;
        }

        public async Task<Dependency> GetDependencyById(Guid dependencyId)
            => await _context.Dependencies.Where(x => x.Id == dependencyId).FirstOrDefaultAsync();

        public async Task<IEnumerable<Tuple<int,int>>> GetReportByDependencyId(Guid? dependencyId, Guid? facultyId = null)
        {
            var query = _context.Documents.AsQueryable();

            if (dependencyId.HasValue)
                query = query.Where(x => x.DependencyId == dependencyId);

            var result = await query.GroupBy(x => x.ResolutionDate.Month)
                .Select(x => new Tuple<int, int>(x.Key, x.Count())).ToArrayAsync();

            return result;
        }

        public async Task<bool> AnyByNumber(string number, Guid? ignoredId = null)
            => await _context.Documents.AnyAsync(x => x.Number.ToLower().Equals(number.ToLower()) && x.Id != ignoredId);

        public async Task<bool> AnyByResolutionCategoryId(Guid resolutionCategoryId)
            => await _context.Documents.AnyAsync(x => x.ResolutionCategoryId == resolutionCategoryId);

        public async Task<bool> AnyBySorterId(Guid sorterId)
            => await _context.Documents.AnyAsync(x => x.SorterId == sorterId);

        #endregion
    }
}
