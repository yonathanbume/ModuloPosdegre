using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template.CafobeRequest;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Language;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class CafobeRequestDetailService : ICafobeRequestDetailService
    {
        private readonly ICafobeRequestDetailRepository _cafobeRequestDetailRepository;
        public CafobeRequestDetailService(ICafobeRequestDetailRepository cafobeRequestDetailRepository)
        {
            _cafobeRequestDetailRepository = cafobeRequestDetailRepository;
        }

        public Task Delete(CafobeRequestDetail cafobeRequestDetail)
            => _cafobeRequestDetailRepository.Delete(cafobeRequestDetail);

        public Task<CafobeRequestDetail> Get(Guid id)
            => _cafobeRequestDetailRepository.Get(id);

        public Task<IEnumerable<CafobeRequestDetail>> GetAll()
            => _cafobeRequestDetailRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, int? status = null, Guid? termId = null, Guid? careerId = null, string searchValue = null)
            => _cafobeRequestDetailRepository.GetAllDatatable(sentParameters, type, status, termId, careerId, searchValue);

        public Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, int? status = null, int? sex = null, Guid? facultyId = null, Guid? termId = null, string searchValue = null)
            => _cafobeRequestDetailRepository.GetReportDatatable(sentParameters, type, status, sex, facultyId, termId, searchValue);


        public Task<CafobeRequestDetailTemplate> GetDataById(Guid id)
            => _cafobeRequestDetailRepository.GetDataById(id);

        public Task<List<CafobeRequestDetailTemplate>> GetReportData(int? type = null, int? status = null, int? sex = null, Guid? facultyId = null, Guid? termId = null)
            => _cafobeRequestDetailRepository.GetReportData(type, status, sex, facultyId, termId);

        public Task<DataTablesStructs.ReturnedData<object>> GetStudentRequestDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, int? type = null, int? status = null, string searchValue = null)
            => _cafobeRequestDetailRepository.GetStudentRequestDetailDatatable(sentParameters, studentId, type, status,searchValue);

        public Task Insert(CafobeRequestDetail cafobeRequestDetail)
            => _cafobeRequestDetailRepository.Insert(cafobeRequestDetail);

        public Task Update(CafobeRequestDetail cafobeRequestDetail)
            => _cafobeRequestDetailRepository.Update(cafobeRequestDetail);

    }
}
