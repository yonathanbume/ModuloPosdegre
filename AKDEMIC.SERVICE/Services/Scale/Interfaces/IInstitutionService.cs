using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IInstitutionService
    {
        Task<Institution> Get(Guid id);
        Task<bool> AnyByName(string name, Guid? id = null);
        Task<IEnumerable<Institution>> GetAll();
        Task Insert(Institution institution);
        Task Update(Institution institution);
        Task Delete(Institution institution);

        Task<DataTablesStructs.ReturnedData<object>> GetAllInstitutionsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
