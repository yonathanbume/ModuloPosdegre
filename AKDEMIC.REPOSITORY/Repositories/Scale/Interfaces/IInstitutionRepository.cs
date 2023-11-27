using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IInstitutionRepository:IRepository<Institution>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetAllInstitutionsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);

        Task<bool> AnyByName(string name, Guid? id = null);
    }
}
