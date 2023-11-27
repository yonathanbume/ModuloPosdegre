using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class StudentAbilityRepository:Repository<StudentAbility> , IStudentAbilityRepository
    {
        public StudentAbilityRepository(AkdemicContext context) : base (context){ }

        public async Task<bool> ExistByAbility(Guid abilityId)
        {
            return await _context.StudentAbilities.AnyAsync(x => x.AbilityId == abilityId);
        }

        public async Task<IEnumerable<StudentAbility>> GetAllByStudent(Guid studentId)
        {
            var query = _context.StudentAbilities
                    .Where(x => x.StudentId == studentId);

            return await query.ToListAsync();
        }

        public async Task<List<AbilityDate>> GetAllByStudentTemplate(Guid studentId)
        {
            var result =  await _context.StudentAbilities
                .Where(x => x.StudentId == studentId)
                .Select(x => new AbilityDate
                {
                    Ability = x.Ability.Description,
                    Level = x.Level,
                    StringLevel = ConstantHelpers.LEVEL_EXPERIENCE.VALUES[Convert.ToInt32(x.Level)]
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<StudentAbility>> GetAllWithIncludesByStudent(Guid studentId)
        {
            var query = _context.StudentAbilities
                    .Include(x => x.Student)
                    .Include(x => x.Ability)
                    .Where(x => x.StudentId == studentId);

            return await query.ToListAsync();
        }

        public async Task<object> GetJobExchangeStudentAbilityReportChart()
        {
            var categoriesData = new List<string>();
            var seriesData = new List<int>();

            var abilitiesQuery = _context.Abilities.AsNoTracking();
            var studentAbilitiesQuery = _context.StudentAbilities.Where(x => x.Student.StudentExperiences.Any(x => x.CurrentWork)).AsNoTracking();

            var data = await abilitiesQuery
                .Select(x => new
                {
                    Name = x.Description,
                    Count = studentAbilitiesQuery.Where(y => y.AbilityId == x.Id).Select(x => x.StudentId).Distinct().Count()
                })
                .ToListAsync();

            return new
            {
                categoriesData = data.Select(x => x.Name).ToList(),
                seriesData = data.Select(x => x.Count).ToList(),
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentAbilityReportDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            var categoriesData = new List<string>();
            var seriesData = new List<int>();

            var abilitiesQuery = _context.Abilities.AsNoTracking();
            var studentAbilitiesQuery = _context.StudentAbilities.Where(x => x.Student.StudentExperiences.Any(x => x.CurrentWork)).AsNoTracking();

            var data = await abilitiesQuery
                .Select(x => new
                {
                    Name = x.Description,
                    Count = studentAbilitiesQuery.Where(y => y.AbilityId == x.Id).Select(x => x.StudentId).Distinct().Count()
                })
                .ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }
    }
}
