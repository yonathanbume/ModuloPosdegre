using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class WorkerBachelorDegreeRepository : Repository<WorkerBachelorDegree>, IWorkerBachelorDegreeRepository
    {
        public WorkerBachelorDegreeRepository(AkdemicContext context) : base(context) { }

        public async Task<List<WorkerBachelorDegree>> GetAllByUserId(string userId)
        {
            return await _context.WorkerBachelorDegrees.Include(x => x.Institution).Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<int> GetWorkerBachelorDegreesQuantity(string userId)
        {
            var records = await _context.WorkerBachelorDegrees
                .Where(x => x.UserId == userId)
                .CountAsync();

            return records;
        }

        public async Task<List<BachelorDegreesTemplate>> GetWorkerBachelorDegreesByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            var query = _context.WorkerBachelorDegrees
                .Where(x => x.UserId == userId)
                .AsQueryable();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.Specialty) : query.OrderBy(q => q.Specialty);
                    break;
                case "1":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.Institution) : query.OrderBy(q => q.Institution);
                    break;
                case "2":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ExpeditionDate) : query.OrderBy(q => q.ExpeditionDate);
                    break;
                default:
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.Specialty) : query.OrderBy(q => q.Specialty);
                    break;
            }

            var pagedList = await query.Skip(paginationParameter.CurrentNumber).Take(paginationParameter.RecordsPerPage)
                .Select(x => new BachelorDegreesTemplate
                {
                    Id = x.Id,
                    Name = x.Name,
                    Specialty = x.Specialty,
                    Institution = x.Institution.Name,
                    ExpeditionFormattedDate = x.ExpeditionDate.ToLocalDateFormat(),
                    StudyDocument = x.StudyDocument
                }).ToListAsync();

            return pagedList;
        }

        public async Task<WorkerBachelorDegree> GetWithIncludes(Guid id)
        {
            var query = await _context.WorkerBachelorDegrees.Include(x => x.Institution).Where(x => x.Id == id).FirstOrDefaultAsync();

            return query;
        }
    }
}
