using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeacherHiring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeacherHiring.Interfaces
{
    public interface IConvocationRepository : IRepository<Convocation>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
    }
}
