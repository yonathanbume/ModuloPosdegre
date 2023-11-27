using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PreprofesionalPractice.Interfaces
{
    public interface IInternshipAspectService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, byte type);
        Task Insert(InternshipAspect entity);
        Task Update(InternshipAspect entity);
        Task<InternshipAspect> Get(Guid id);
        Task Delete(InternshipAspect entity);
        Task<IEnumerable<InternshipAspect>> GetAllByType(byte? type = null, bool? ignoreQueryFilters = null);
    }
}
