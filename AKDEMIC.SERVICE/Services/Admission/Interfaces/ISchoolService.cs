using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface ISchoolService
    {
        Task DeleteById(Guid id);
        Task<School> Get(Guid id);
        Task<IEnumerable<School>> GetAll();
        Task<object> GetSelect2ClientSide();
        Task<object> GetSelect2ServerSide(string term, int? type = null, Guid? departmentId = null, Guid? provinceId = null);
        Task Insert(School school);
        Task InsertRange(List<School> schools);
        Task Update(School school);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, byte? type = null, string search = null);
    }
}
