using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingResearch;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingResearch.Interfaces
{
    public interface ITeachingResearchConvocationRepository : IRepository<Convocation>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetConvocationsDatatable(DataTablesStructs.SentParameters parameters, string searchValue);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
    }
}
