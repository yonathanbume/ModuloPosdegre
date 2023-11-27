using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template.CafobeRequest;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class CafobeRequestService : ICafobeRequestService
    {
        private readonly ICafobeRequestRepository _cafobeRequestRepository;
        public CafobeRequestService(ICafobeRequestRepository cafobeRequestRepository)
        {
            _cafobeRequestRepository = cafobeRequestRepository;
        }

        public Task Delete(CafobeRequest cafobeRequest)
            => _cafobeRequestRepository.Delete(cafobeRequest);

        public Task<CafobeRequest> Get(Guid id)
            => _cafobeRequestRepository.Get(id);

        public Task<IEnumerable<CafobeRequest>> GetAll()
            => _cafobeRequestRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, int? status = null, Guid? termId = null, Guid? careerId = null, string searchValue = null)
            => _cafobeRequestRepository.GetAllDatatable(sentParameters, type, status, termId, careerId , searchValue);

        public Task<DataTablesStructs.ReturnedData<object>> GetReportDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, int? status = null, int? sex = null, Guid? facultyId = null, Guid? termId = null, string searchValue = null)
            => _cafobeRequestRepository.GetReportDatatable(sentParameters, type, status, sex, facultyId, termId, searchValue);


        public Task<CafobeRequestTemplate> GetDataById(Guid id)
            => _cafobeRequestRepository.GetDataById(id);

        public Task<List<CafobeRequestTemplate>> GetReportData(int? type = null, int? status = null, int? sex = null, Guid? facultyId = null, Guid? termId = null)
            => _cafobeRequestRepository.GetReportData(type, status, sex, facultyId, termId);

        public Task<DataTablesStructs.ReturnedData<object>> GetStudentRequestDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, int? type = null, int? status = null, string searchValue = null)
            => _cafobeRequestRepository.GetStudentRequestDatatable(sentParameters, studentId, type, status, searchValue);
        public Task<bool> GetLastByStudent(Guid studentId,Guid actualTermId, int type)
            => _cafobeRequestRepository.GetLastByStudent(studentId,actualTermId, type);

        public Task Insert(CafobeRequest cafobeRequest)
            => _cafobeRequestRepository.Insert(cafobeRequest);

        public Task Update(CafobeRequest cafobeRequest)
            => _cafobeRequestRepository.Update(cafobeRequest);
    }
}
