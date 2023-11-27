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
    public class WorkerDoctoralDegreeRepository : Repository<WorkerDoctoralDegree>, IWorkerDoctoralDegreeRepository
    {
        public WorkerDoctoralDegreeRepository(AkdemicContext context) : base(context) { }

        public async Task<List<WorkerDoctoralDegree>> GetAllByUserId(string userId)
        {
            return await _context.WorkerDoctoralDegrees.Include(x => x.Institution).Where(x => x.UserId == userId).AsQueryable().ToListAsync();
        }

        public async Task<int> GetWorkerDoctoralDegreesQuantity(string userId)
        {
            var records = await _context.WorkerDoctoralDegrees
                .Where(x => x.UserId == userId)
                .CountAsync();

            return records;
        }

        public async Task<List<DoctoralDegreesTemplate>> GetWorkerDoctoralDegreesByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            var query = _context.WorkerDoctoralDegrees
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
                .Select(x => new DoctoralDegreesTemplate
                {
                    Id = x.Id,
                    Specialty = x.Specialty,
                    Institution = x.Institution.Name,
                    ExpeditionFormattedDate = x.ExpeditionDate.ToLocalDateFormat(),
                    StudyDocument = x.StudyDocument
                }).ToListAsync();

            return pagedList;
        }

        public async Task<WorkerDoctoralDegree> GetWithIncludes(Guid id)
        {
            var query = await _context.WorkerDoctoralDegrees.Include(x => x.Institution).Where(x => x.Id == id).FirstOrDefaultAsync();
            return query;
        }
    }
}
