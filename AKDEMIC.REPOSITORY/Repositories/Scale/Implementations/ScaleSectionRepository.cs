using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class ScaleSectionRepository : Repository<ScaleSection>, IScaleSectionRepository
    {
        public ScaleSectionRepository(AkdemicContext context) : base(context) { }

        public async Task<ScaleSection> GetScaleSectionByNumber(byte sectionNumber)
        {
            return await _context.ScaleSections.FirstOrDefaultAsync(x => x.SectionNumber == sectionNumber);
        }

        public async Task<Tuple<int, List<Tuple<string, int>>>> GetScaleSectionQuantityReportByPaginationParameters(PaginationParameter paginationParameter)
        {
            var baseQuery = _context.ScaleSections
                .Where(x => string.IsNullOrWhiteSpace(paginationParameter.SearchValue) || x.Name.Contains(paginationParameter.SearchValue))
                .AsQueryable();

            var records = await baseQuery.CountAsync();

            var query = baseQuery.Select(x => new
            {
                name = x.Name,
                quantity = _context.ScaleResolutions.Count(y => y.ScaleSectionResolutionType.ScaleSectionId == x.Id)
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

        public async Task<List<Tuple<string, int>>> GetScaleSectionQuantityReport(string search)
        {
            var data = await _context.ScaleSections
                .Where(x => string.IsNullOrWhiteSpace(search) || x.Name.Contains(search))
                .Select(x => new
                {
                    name = x.Name,
                    quantity = _context.ScaleResolutions.Count(y => y.ScaleSectionResolutionType.ScaleSectionId == x.Id)
                }).AsQueryable()
                .ToListAsync();

            var result = data.Select(x => new Tuple<string, int>(x.name, x.quantity)).ToList();

            return result;
        }
    }
}
