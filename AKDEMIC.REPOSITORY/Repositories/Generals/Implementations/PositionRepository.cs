using AKDEMIC.ENTITIES.Models;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public sealed class PositionRepository : Repository<Position>, IPositionRepository
    {
        public PositionRepository(AkdemicContext context) : base(context) { }

        async Task<object> IPositionRepository.GetAllAsModelA()
        {
            var result = await _context.Positions
                .OrderByDescending(x => x.CreatedAt.Value)
                .Select(
                x => new
                {
                    id = x.Id,
                    description = x.Description,
                    age = x.Age,
                    category = x.Category,
                    dedication = x.Dedication,
                    academicDegree = x.AcademicDegree,
                    jobTitle = x.JobTitle,
                    createdAt = x.CreatedAt
                }).ToListAsync();

            return result;
        }
    }
}