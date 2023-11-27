using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Resolution;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class ScaleResolutionTypeRepository : Repository<ScaleResolutionType>, IScaleResolutionTypeRepository
    {
        public ScaleResolutionTypeRepository(AkdemicContext context) : base(context) { }
        
        public async Task<List<ScaleResolutionType>> GetScaleResolutionTypesBySectionId(Guid sectionId)
        {
            var query = _context.ScaleResolutionTypes
                .Join
                (
                    _context.ScaleSectionResolutionTypes,
                    rt => rt.Id,
                    srt => srt.ScaleResolutionTypeId,
                    (rt, srt) => new {rt, srt}
                )
                .Where(x => x.srt.ScaleSectionId == sectionId && x.srt.Status == ConstantHelpers.STATES.ACTIVE && x.srt.Status == ConstantHelpers.STATES.ACTIVE)
                .Select(x => new ScaleResolutionType
                {
                    Id = x.rt.Id,
                    Name = x.rt.Name,
                    Description = x.rt.Description
                })
                .AsQueryable();

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<List<ScaleResolutionType>> GetScaleResolutionTypesBySectionId(Guid sectionId, string search, PaginationParameter paginationParameter)
        {
            var query = _context.ScaleResolutionTypes
                .Join
                (
                    _context.ScaleSectionResolutionTypes,
                    rt => rt.Id,
                    srt => srt.ScaleResolutionTypeId,
                    (rt, srt) => new {rt, srt}
                )
                .Where(x => (string.IsNullOrWhiteSpace(search) || x.rt.Name.Contains(search) || x.rt.Description.Contains(search)) &&
                            x.srt.ScaleSectionId == sectionId && x.srt.Status == ConstantHelpers.STATES.ACTIVE)
                .AsQueryable();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.rt.Name) : query.OrderBy(q => q.rt.Name);
                    break;
                case "1":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.srt.Status) : query.OrderBy(q => q.srt.Status);
                    break;
                case "2":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.rt.Description) : query.OrderBy(q => q.rt.Description);
                    break;
                default:
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.rt.Name) : query.OrderBy(q => q.rt.Name);
                    break;
            }

            var result = await query.Skip(paginationParameter.CurrentNumber).Take(paginationParameter.RecordsPerPage)
                .Select(x => new ScaleResolutionType
                {
                    Id = x.rt.Id,
                    Name = x.rt.Name,
                    Status = x.rt.Status,
                    Description = x.rt.Description
                })
                .ToListAsync();

            return result;
        }

        public async Task<int> GetScaleResolutionTypesQuantityBySectionId(Guid sectionId, string search)
        {
            var records = await _context.ScaleResolutionTypes
                .Join
                (
                    _context.ScaleSectionResolutionTypes,
                    rt => rt.Id,
                    srt => srt.ScaleResolutionTypeId,
                    (rt, srt) => new { rt, srt }
                )
                .Where(x => (string.IsNullOrWhiteSpace(search) || x.rt.Name.Contains(search) || x.rt.Description.Contains(search)) &&
                            x.srt.ScaleSectionId == sectionId && x.srt.Status == ConstantHelpers.STATES.ACTIVE)
                .CountAsync();
            
            return records;
        }

        public async Task<List<ScaleResolutionType>> GetScaleNotAssignedResolutionTypesBySectionId(Guid sectionId, string search, PaginationParameter paginationParameter)
        {
            var query = _context.ScaleResolutionTypes
                .Where(x => !_context.ScaleSectionResolutionTypes.Where(y => y.ScaleSectionId == sectionId && y.Status == ConstantHelpers.STATES.ACTIVE).Select(y => y.ScaleResolutionTypeId).Contains(x.Id) && 
                            (string.IsNullOrWhiteSpace(search) || x.Name.Contains(search) || x.Description.Contains(search)) &&
                            x.Status == ConstantHelpers.STATES.ACTIVE)
                .AsQueryable();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.Name) : query.OrderBy(q => q.Name);
                    break;
                case "1":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.Description) : query.OrderBy(q => q.Description);
                    break;
                default:
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.Name) : query.OrderBy(q => q.Name);
                    break;
            }

            var result = await query.Skip(paginationParameter.CurrentNumber).Take(paginationParameter.RecordsPerPage)
                .Select(x => new ScaleResolutionType
                {
                    Id = x.Id,
                    Name = x.Name,
                    Status = x.Status,
                    Description = x.Description
                })
                .ToListAsync();

            return result;
        }

        public async Task<int> GetScaleNotAssignedResolutionTypesQuantityBySectionId(Guid sectionId, string search)
        {
            var records = await _context.ScaleResolutionTypes
                .Where(x => !_context.ScaleSectionResolutionTypes.Where(y => y.ScaleSectionId == sectionId && y.Status == ConstantHelpers.STATES.ACTIVE).Select(y => y.ScaleResolutionTypeId).Contains(x.Id) &&
                            (string.IsNullOrWhiteSpace(search) || x.Name.Contains(search) || x.Description.Contains(search)) &&
                            x.Status == ConstantHelpers.STATES.ACTIVE)
                .CountAsync();

            return records;
        }
        
        public async Task<Tuple<int, List<Tuple<string, int>>>> GetResolutionTypeQuantityReportByPaginationParameters(PaginationParameter paginationParameter)
        {
            var baseQuery = _context.ScaleResolutionTypes
                .Where(x => string.IsNullOrWhiteSpace(paginationParameter.SearchValue) || x.Name.Contains(paginationParameter.SearchValue))
                .AsQueryable();

            var records = await baseQuery.CountAsync();

            var query = baseQuery.Select(x => new
            {
                name = x.Name,
                quantity = _context.ScaleResolutions.Count(y => y.ScaleSectionResolutionType.ScaleResolutionTypeId == x.Id)
            }).AsQueryable();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.name) : query.OrderBy(q => q.name);
                    break;
                case "1":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.quantity) : query.OrderBy(q => q.quantity);
                    break;
                default:
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.name) : query.OrderBy(q => q.name);
                    break;
            }

            var data = await query.Skip(paginationParameter.CurrentNumber).Take(paginationParameter.RecordsPerPage).ToListAsync();

            var result = data.Select(x => new Tuple<string, int>(x.name, x.quantity)).ToList();

            return new Tuple<int, List<Tuple<string, int>>>(records, result);
        }

        public async Task<List<Tuple<string, int>>> GetResolutionTypeQuantityReport(string search)
        {
            var data = await _context.ScaleResolutionTypes
                .Where(x => string.IsNullOrWhiteSpace(search) || x.Name.Contains(search))
                .Select(x => new
                {
                    name = x.Name,
                    quantity = _context.ScaleResolutions.Count(y => y.ScaleSectionResolutionType.ScaleResolutionTypeId == x.Id)
                })
                .AsQueryable()
                .ToListAsync();
            
            var result = data.Select(x => new Tuple<string, int>(x.name, x.quantity)).ToList();

            return result;
        }

        public async Task<List<ScaleResolutionTypeTemplate>> GetScaleResolutionTypesBySectionIdAndUser(Guid sectionId, string userId)
        {
            var query = _context.ScaleResolutions
                    .Where(x => x.UserId == userId && x.ScaleSectionResolutionType.ScaleSectionId == sectionId)
                    .AsQueryable();

            var grouped = query
                .GroupBy(x => new { x.ScaleSectionResolutionType.ScaleResolutionTypeId })
                .Select(x => new
                {
                    id = x.Key.ScaleResolutionTypeId,
                    count = x.Count()
                }).AsQueryable();

            var data = await _context.ScaleSectionResolutionTypes
                .Where(x => x.ScaleSectionId == sectionId)
                .Select(x => new ScaleResolutionTypeTemplate
                {
                    Id = x.ScaleResolutionTypeId,

                    Name = x.ScaleResolutionType.Name,
                    Description = x.ScaleResolutionType.Description,
                    Count = grouped.Where(y => y.id == x.ScaleResolutionTypeId).Select(y => y.count).FirstOrDefault()
                }).ToListAsync();

            return data;

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetScaleresolutionTypeDatatable(DataTablesStructs.SentParameters sentParameters, int? status = null, string search = null)
        {
            Expression<Func<ScaleResolutionType, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Description); break;
                case "2":
                    orderByPredicate = ((x) => x.Status); break;
                default:
                    orderByPredicate = ((x) => x.Name); break;
            }

            var query = _context.ScaleResolutionTypes.AsQueryable();

            if (status.HasValue)
                query = query.Where(x => x.Status == status);


            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(x => x.Name.Trim().ToLower().Contains(search) || x.Description.Trim().ToLower().Contains(search));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new 
                  {
                      id = x.Id,
                      name = x.Name,
                      description = x.Description,
                      status = x.Status,
                      isDefault = x.IsSystemDefault
                  })
                  .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<IEnumerable<ScaleResolutionType>> GetAll(string searchValue = null, bool? onlyActive = false)
        {
            var query = _context.ScaleResolutionTypes.AsQueryable();

            if (onlyActive.HasValue && onlyActive.Value)
                query = query.Where(x => x.Status == ConstantHelpers.STATES.ACTIVE);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _context.ScaleResolutionTypes.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.Id != ignoredId);

        public async Task<bool> ExistResolutionType(string name)
        {
            var resolutionType = await _context.ScaleResolutionTypes.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();

            if (resolutionType == null)
                return false;
            else
                return true;
        }

        public async Task<ScaleResolutionType> GetByName(string name)
        {
            return await _context.ScaleResolutionTypes.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }
    }
}
