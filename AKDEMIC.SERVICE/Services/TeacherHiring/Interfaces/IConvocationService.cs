using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task Insert(Convocation entity);
        Task Update(Convocation entity);
        Task<Convocation> Get(Guid id);
    }
}
