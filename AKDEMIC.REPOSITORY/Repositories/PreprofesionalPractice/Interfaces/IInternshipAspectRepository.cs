using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Interfaces
{
    public interface IInternshipAspectRepository : IRepository<InternshipAspect>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, byte type);
        Task<IEnumerable<InternshipAspect>> GetAllByType(byte? type = null, bool? ignoreQueryFilters = null);
    }
}
