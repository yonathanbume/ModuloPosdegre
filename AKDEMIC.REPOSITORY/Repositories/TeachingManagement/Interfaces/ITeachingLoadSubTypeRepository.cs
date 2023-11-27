using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface ITeachingLoadSubTypeRepository : IRepository<TeachingLoadSubType>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTeachingLoadSubTypeDatatable(DataTablesStructs.SentParameters parameters, Guid? teachingLoadTypeId);
        Task<object> GetTeachingLoadSubTypeSelect2(Guid? teachingLoadtypeId, bool? enabled = null);
    }
}
