using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class StudentAbilityService: IStudentAbilityService
    {
        private readonly IStudentAbilityRepository _studentAbilityRepository;

        public StudentAbilityService(IStudentAbilityRepository studentAbilityRepository)
        {
            _studentAbilityRepository = studentAbilityRepository;
        }

        public async Task DeleteRange(IEnumerable<StudentAbility> studentAbilities)
        {
            await _studentAbilityRepository.DeleteRange(studentAbilities);
        }

        public async Task<bool> ExistByAbility(Guid abilityId)
        {
            return await _studentAbilityRepository.ExistByAbility(abilityId);
        }

        public async Task<IEnumerable<StudentAbility>> GetAllByStudent(Guid studentId)
        {
            return await _studentAbilityRepository.GetAllByStudent(studentId);
        }

        public async Task<List<ProfileDetailTemplate.AbilityDate>> GetAllByStudentTemplate(Guid studentId)
        {
            return await _studentAbilityRepository.GetAllByStudentTemplate(studentId);
        }

        public async Task<IEnumerable<StudentAbility>> GetAllWithIncludesByStudent(Guid studentId)
        {
            return await _studentAbilityRepository.GetAllWithIncludesByStudent(studentId);
        }

        public async Task InsertRange(IEnumerable<StudentAbility> studentAbilities)
        {
            await _studentAbilityRepository.InsertRange(studentAbilities);
        }
        public Task<object> GetJobExchangeStudentAbilityReportChart()
            => _studentAbilityRepository.GetJobExchangeStudentAbilityReportChart();

        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentAbilityReportDatatable(DataTablesStructs.SentParameters sentParameters)
            => _studentAbilityRepository.GetJobExchangeStudentAbilityReportDatatable(sentParameters);
    }
}
