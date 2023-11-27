using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyScholarshipService
    {
        Task Insert(TransparencyScholarship entity);
        Task Delete(TransparencyScholarship entity);
        Task Update(TransparencyScholarship entity);
        Task<TransparencyScholarship> Get(Guid id);

        Task<DataTablesStructs.ReturnedData<object>> GetScholarshipDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
    }
}
