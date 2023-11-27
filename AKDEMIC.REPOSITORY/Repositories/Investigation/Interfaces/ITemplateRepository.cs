using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces
{
    public interface ITemplateRepository
    {
        Task<bool> AnyTemplateByName(string name, Guid? id);
        Task<int> Count();
        Task<Template> Get(Guid id);
        Task<object> GetTemplate(Guid id);
        Task<IEnumerable<Template>> GetAll();
        Task<IEnumerable<object>> GetTemplates();
        Task<DataTablesStructs.ReturnedData<object>> GetTemplatesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId, string searchValue = null);
        Task DeleteById(Guid id);
        Task Insert(Template template);
        Task Update(Template template);
    }
}
