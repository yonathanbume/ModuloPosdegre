using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class WorkerDinaSupportExperienceRepository : Repository<WorkerDinaSupportExperience> , IWorkerDinaSupportExperienceRepository
    {
        public WorkerDinaSupportExperienceRepository(AkdemicContext context) :base(context) { }

        public async Task UpdateExperience(List<byte> types, Guid workerDinaId)
        {
            var experience = await _context.WorkerDinaSupportExperiences.Where(x => x.WorkerDinaId == workerDinaId).ToArrayAsync();
            var toAdd = types.Where(x => !experience.Select(y=>y.Type).Contains(x)).ToList();
            var toDelete = experience.Where(x => !types.Contains(x.Type)).ToList();

            var listToAdd = toAdd.
                Select(x => new WorkerDinaSupportExperience
                {
                    Type = x,
                    WorkerDinaId = workerDinaId
                }).ToList();

            await _context.WorkerDinaSupportExperiences.AddRangeAsync(listToAdd);
            _context.WorkerDinaSupportExperiences.RemoveRange(toDelete);

            await _context.SaveChangesAsync();
        }
    }
}
