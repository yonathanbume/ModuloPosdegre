using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public sealed class TeacherExperienceRepository : Repository<TeacherExperience>, ITeacherExperienceRepository
    {
        public TeacherExperienceRepository(AkdemicContext context) : base(context) { }

        async Task<object> ITeacherExperienceRepository.GetAllAsModelA()
        {
            var teacherInstructions = await _context.TeacherExperiences.Select(
                x => new 
                {
                    Id = x.Id,
                    CountryId = x.CountryId,
                    State = x.State,
                    FilePath = x.FilePath,
                    TimeFrame = x.TimeFrame,
                    InstituteDescription = x.InstituteDescription,
                    LaborType = x.LaborType,
                    LevelPedagogy = x.LevelPedagogy,
                    Relation = x.Relation
                }).ToListAsync();

            return teacherInstructions;
        }
    }
}