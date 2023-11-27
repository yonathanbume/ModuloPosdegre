using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Implementations
{
    public class ComputerService : IComputerService
    {
        private readonly IComputerRepository _computerRepository;

        public ComputerService(IComputerRepository computerRepository)
        {
            _computerRepository = computerRepository;
        }

        public async Task Delete(Computer computer)
            => await _computerRepository.Delete(computer);

        public async Task DeleteById(Guid id)
            => await _computerRepository.DeleteById(id);

        public async Task<Computer> Get(Guid id)
            => await _computerRepository.Get(id);

        public async Task<IEnumerable<Computer>> GetAll()
            => await _computerRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetComputerDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? dependencyId = null, string brand = null, int? start_year = null, int? end_year = null, Guid? status = null, string start_purchase = null, string end_purchase = null, string start_createdat = null, string end_createdat = null)
            => await _computerRepository.GetComputerDatatable(sentParameters, searchValue, dependencyId, brand, start_year, end_year, status, start_purchase, end_purchase, start_createdat, end_createdat);

        public async Task<object> GetComputerReportChart(Guid? dependencyId, Guid? type = null, Guid? state = null)
            => await _computerRepository.GetComputerReportChart(dependencyId, type, state);

        public async Task<DataTablesStructs.ReturnedData<object>> GetComputerReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? dependencyId, Guid? type = null, Guid? state = null, string searchValue = null)
            => await _computerRepository.GetComputerReportDatatable(sentParameters, dependencyId, type, state, searchValue);

        public async Task<object> GetReportByDependencyChart()
            => await _computerRepository.GetReportByDependencyChart();

        public async Task Insert(Computer computer)
            => await _computerRepository.Insert(computer);

        public async Task Update(Computer computer)
            => await _computerRepository.Update(computer);
    }
}
