using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public sealed class UnitResourceRepository : Repository<UnitResource>, IUnitResourceRepository
    {
        public UnitResourceRepository(AkdemicContext context) : base(context) { }

        async Task<IEnumerable<UnitResource>> IUnitResourceRepository.GetAllByCourseUnitId(Guid courseUnitId)
        {
            var units = await _context.UnitResources
                .Where(x => x.CourseUnitId == courseUnitId)
                .ToListAsync();

            return units;
        }

        async Task<object> IUnitResourceRepository.GetAsModelA(Guid id)
        {
            var result = await _context.UnitResources
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    id = x.Id,
                    unitId = x.CourseUnitId,
                    name = x.Name,
                    week = x.Week
                }).FirstOrDefaultAsync();

            return result;
        }
    }
}