using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.Activity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public sealed class ActivityRepository : Repository<Activity>, IActivityRepository
    {
        public ActivityRepository(AkdemicContext context) : base(context) { }

        async Task<object> IActivityRepository.GetAllAsModelA()
        {
            var result = await _context.Activities.Where(x => x.DeletedAt == null).Select(x => new
            {
                id = x.Id,
                name = x.Name,
                description = x.Description,
                minHours = x.MinHours,
                maxHours = x.MaxHours
            }).ToListAsync();

            return result;
        }

        async Task<ActivityTemplateA> IActivityRepository.GetAsModelB(Guid? id)
        {
            var query = _context.Activities.AsQueryable();

            if (id.HasValue)
                query = query.Where(x => x.Id == id);

            var model = await query
                .Select(
                    x => new ActivityTemplateA
                    {
                        Id = x.Id,
                        Description = x.Description,
                        Name = x.Name,
                        MinHours = x.MinHours,
                        MaxHours = x.MaxHours,
                        Number = x.Resolution.Number,
                        IssueDate = $"{ x.Resolution.IssueDate:dd/MM/yyyy}",
                        ResolutionPath = x.Resolution.FilePath
                    }
                )
                .FirstOrDefaultAsync();

            return model;
        }
    }
}