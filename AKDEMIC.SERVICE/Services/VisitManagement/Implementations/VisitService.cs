using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.VisitManagement;
using AKDEMIC.REPOSITORY.Repositories.VisitManagement.Interfaces;
using AKDEMIC.SERVICE.Services.VisitManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.VisitManagement.Implementations
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _visitRepository;

        public VisitService(IVisitRepository visitRepository)
        {
            _visitRepository = visitRepository;
        }


        public async Task<Visit> Get(Guid id)
            => await _visitRepository.Get(id);

        public async Task Insert(Visit visit)
            => await _visitRepository.Insert(visit);

        public async Task Delete(Visit visit)
            => await _visitRepository.Delete(visit);

        public async Task<object> GetVisitByDependenciesChart()
            => await _visitRepository.GetVisitByDependenciesChart();

        public async Task<IEnumerable<Visit>> GetAll()
            => await _visitRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<Visit>> GetVisitDatatable(DataTablesStructs.SentParameters sentParameters, string startDate = null, string endDate = null, Guid? dependencyId = null, string search = null)
            => await _visitRepository.GetVisitDatatable(sentParameters, startDate, endDate, dependencyId, search);

        public async Task<DataTablesStructs.ReturnedData<Visit>> GetVisitDatatableToPublic(DataTablesStructs.SentParameters sentParameters, string dateTime)
            => await _visitRepository.GetVisitDatatableToPublic(sentParameters, dateTime);
    }
}
