using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template.CafobeRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface ICafobeRequestDetailService
    {
        Task<CafobeRequestDetail> Get(Guid id);
        Task<CafobeRequestDetailTemplate> GetDataById(Guid id);
        Task<List<CafobeRequestDetailTemplate>> GetReportData(int? type = null, int? status = null, int? sex = null, Guid? facultyId = null, Guid? termId = null);
        Task<IEnumerable<CafobeRequestDetail>> GetAll();
        Task Insert(CafobeRequestDetail cafobeRequestDetail);
        Task Update(CafobeRequestDetail cafobeRequestDetail);
        Task Delete(CafobeRequestDetail cafobeRequestDetail);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, int? status = null, Guid? termId = null, Guid? careerId = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, int? status = null, int? sex = null, Guid? facultyId = null, Guid? termId = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentRequestDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, int? type = null, int? status = null, string searchValue = null);

    }
}
