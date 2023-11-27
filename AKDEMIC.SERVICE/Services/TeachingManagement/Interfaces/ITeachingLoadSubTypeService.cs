using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface ITeachingLoadSubTypeService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTeachingLoadSubTypeDatatable(DataTablesStructs.SentParameters parameters, Guid? teachingLoadTypeId);
        Task Insert(TeachingLoadSubType entity);
        Task Update(TeachingLoadSubType entity);
        Task<TeachingLoadSubType> Get(Guid id);
        Task Delete(TeachingLoadSubType entity);
        Task<object> GetTeachingLoadSubTypeSelect2(Guid? teachingLoadtypeId, bool? enabled = null);
    }
}
