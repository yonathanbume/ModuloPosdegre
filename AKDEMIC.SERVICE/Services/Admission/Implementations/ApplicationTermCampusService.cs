using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class ApplicationTermCampusService: IApplicationTermCampusService
    {
        private readonly IApplicationTermCampusRepository _applicationTermCampusRepository;
        public ApplicationTermCampusService(IApplicationTermCampusRepository applicationTermCampusRepository)
        {
            _applicationTermCampusRepository = applicationTermCampusRepository;
        }

        public async Task<Tuple<bool, string>> AddRemoveApplicationTermExamCampusId(Guid applicationTermId, Guid examCampusId, int type)
            => await _applicationTermCampusRepository.AddRemoveApplicationTermExamCampusId(applicationTermId, examCampusId, type);

        public Task<bool> AnyPostulantByApplicationTermIdAndExamCampusId(Guid applicationTermId, Guid examCampusId)
            => _applicationTermCampusRepository.AnyPostulantByApplicationTermIdAndExamCampusId(applicationTermId, examCampusId);

        public Task<object> GetCampusByApplicationTermSelect(Guid applicationTermId, int? type = null)
            => _applicationTermCampusRepository.GetCampusByApplicationTermSelect(applicationTermId, type);

        public Task<DataTablesStructs.ReturnedData<object>> GetCampusesDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId)
            => _applicationTermCampusRepository.GetCampusesDatatable(sentParameters, applicationTermId);
    }
}
