using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IStudentAbilityRepository:IRepository<StudentAbility>
    {
        Task <IEnumerable<StudentAbility>> GetAllByStudent(Guid studentId);
        Task<IEnumerable<StudentAbility>> GetAllWithIncludesByStudent(Guid studentId);
        Task<List<AbilityDate>> GetAllByStudentTemplate(Guid studentId);
        Task<bool> ExistByAbility(Guid abilityId);
        Task<object> GetJobExchangeStudentAbilityReportChart();
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentAbilityReportDatatable(DataTablesStructs.SentParameters sentParameters);
    }
}
