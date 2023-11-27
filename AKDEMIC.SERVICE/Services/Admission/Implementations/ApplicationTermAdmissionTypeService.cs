using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class ApplicationTermAdmissionTypeService: IApplicationTermAdmissionTypeService
    {
        private readonly IApplicationTermAdmissionTypeRepository _applicationTermAdmissionTypeRepository;
        public ApplicationTermAdmissionTypeService(IApplicationTermAdmissionTypeRepository applicationTermAdmissionTypeRepository)
        {
            _applicationTermAdmissionTypeRepository = applicationTermAdmissionTypeRepository;
        }

        public async Task AddRemoveApplicationTermAdmissionType(Guid applicationTermId, Guid admissionTypeId)
            => await _applicationTermAdmissionTypeRepository.AddRemoveApplicationTermAdmissionType(applicationTermId, admissionTypeId);

        public async Task<bool> AnyPostulantByApplicationTermIdAndAdmissionTypeId(Guid applicationTermId, Guid admissionTypeId)
            => await _applicationTermAdmissionTypeRepository.AnyPostulantByApplicationTermIdAndAdmissionTypeId(applicationTermId, admissionTypeId);

        public async Task<DataTablesStructs.ReturnedData<object>> GeApplicationTermAdmissionTypesDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, string searchValue)
            => await _applicationTermAdmissionTypeRepository.GeApplicationTermAdmissionTypesDatatable(sentParameters, applicationTermId, searchValue);

        public async Task<ApplicationTermAdmissionType> Get(Guid Id)
        {
            return await _applicationTermAdmissionTypeRepository.Get(Id);
        }

        public async Task<IEnumerable<ApplicationTermAdmissionType>> GetAllByApplicationTermId(Guid applicationTermId)
            => await _applicationTermAdmissionTypeRepository.GetAllByApplicationTermId(applicationTermId);

        public async  Task<object> GetByApplicationTermId(DataTablesStructs.SentParameters sentParameters, Guid id,int type, bool isAllChecked, string searchValue)
        {
            return await _applicationTermAdmissionTypeRepository.GetByApplicationTermId(sentParameters,id,type, isAllChecked, searchValue);
        }
        public async Task<object> GetByApplicationTermId( Guid id)
        {
            return await _applicationTermAdmissionTypeRepository.GetByApplicationTermId( id);
        }

        public async Task<object> GetByApplicationTermIdSelect2(Guid id)
        {
            return await _applicationTermAdmissionTypeRepository.GetByApplicationTermIdSelect2(id);
        }

        public async Task SaveApplicationTermAdmissionTypes(Guid id, bool isCheckAll, List<Guid> lstToAdd, List<Guid> lstToAvoid)
        {
            await _applicationTermAdmissionTypeRepository.SaveApplicationTermAdmissionTypes(id, isCheckAll, lstToAdd, lstToAvoid);
        }
    }
}
