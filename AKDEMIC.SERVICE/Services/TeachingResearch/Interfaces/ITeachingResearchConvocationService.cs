using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingResearch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingResearch.Interfaces
{
    public interface ITeachingResearchConvocationService
    {
        Task Insert(Convocation entity);
        Task Update(Convocation entity);
        Task Delete(Convocation entity);
        Task<Convocation> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetConvocationsDatatable(DataTablesStructs.SentParameters parameters, string searchValue);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
    }
}
