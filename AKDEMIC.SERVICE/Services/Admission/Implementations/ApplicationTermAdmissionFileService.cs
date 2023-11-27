using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class ApplicationTermAdmissionFileService : IApplicationTermAdmissionFileService
    {
        private readonly IApplicationTermAdmissionFileRepository _applicationTermAdmissionFileRepository;

        public ApplicationTermAdmissionFileService(IApplicationTermAdmissionFileRepository applicationTermAdmissionFileRepository)
        {
            _applicationTermAdmissionFileRepository = applicationTermAdmissionFileRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid applicationTermId)
            => await _applicationTermAdmissionFileRepository.GetDatatable(parameters, applicationTermId);

        public async Task InsertRange(List<ApplicationTermAdmissionFile> entities)
            => await _applicationTermAdmissionFileRepository.InsertRange(entities);
    }
}
