using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class WorkAreaService : IWorkAreaService
    {
        private readonly IWorkAreaRepository _workAreaRepository;

        public WorkAreaService(IWorkAreaRepository workAreaRepository)
        {
            _workAreaRepository = workAreaRepository;
        }

        public Task<bool> AnyByName(string name, Guid? id = null)
            => _workAreaRepository.AnyByName(name, id);

        public async Task DeleteById(Guid id)
            => await _workAreaRepository.DeleteById(id);

        public async Task<WorkArea> Get(Guid id)
            => await _workAreaRepository.Get(id);

        public async Task<IEnumerable<WorkArea>> GetAll()
            => await _workAreaRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
                    => _workAreaRepository.GetAllDatatable(sentParameters, searchValue);

        public async Task Insert(WorkArea workArea)
            => await _workAreaRepository.Insert(workArea);

        public async Task Update(WorkArea workArea)
            => await _workAreaRepository.Update(workArea);
    }
}
