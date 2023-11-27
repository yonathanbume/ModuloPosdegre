using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Structs.DataTablesStructs;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IStudentAbilityService
    {
        Task <IEnumerable<StudentAbility>> GetAllByStudent(Guid studentId);
        Task<IEnumerable<StudentAbility>> GetAllWithIncludesByStudent(Guid studentId);
        Task<List<AbilityDate>> GetAllByStudentTemplate(Guid studentId);
        Task DeleteRange(IEnumerable<StudentAbility> studentAbilities);
        Task InsertRange(IEnumerable<StudentAbility> studentAbilities);
        Task<bool> ExistByAbility(Guid abilityId);

        Task<object> GetJobExchangeStudentAbilityReportChart();
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentAbilityReportDatatable(DataTablesStructs.SentParameters sentParameters);
    }
}
