using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface ISchoolRepository : IRepository<School>
    {
        Task<object> GetSelect2ServerSide(string term, int? type = null, Guid? departmentId = null, Guid? provinceId = null);
        Task<object> GetSelect2ClientSide();
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, byte? type = null, string search = null);
    }
}
