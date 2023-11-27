using AKDEMIC.ENTITIES.Models.VisitManagement;
using AKDEMIC.REPOSITORY.Repositories.VisitManagement.Interfaces;
using AKDEMIC.SERVICE.Services.VisitManagement.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.VisitManagement.Implementations
{
    public class VisitInformationService : IVisitInformationService
    {
        private readonly IVisitInformationRepository _visitInformationRepository;

        public VisitInformationService(IVisitInformationRepository visitInformationRepository)
        {
            _visitInformationRepository = visitInformationRepository;
        }

        public async Task Delete(VisitorInformation visitorInformation)
            => await _visitInformationRepository.Delete(visitorInformation);

        public async Task<VisitorInformation> Get(Guid id)
            => await _visitInformationRepository.Get(id);

        public async Task<object> GetVisitsByVisitorsChart()
            => await _visitInformationRepository.GetVisitsByVisitorsChart();
    }
}
