using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class WorkShiftService : IWorkShiftService
    {
        private readonly IWorkShiftRepository _workShiftRepository;

        public WorkShiftService(IWorkShiftRepository workShiftRepository)
        {
            _workShiftRepository = workShiftRepository;
        }

        public async Task Delete(WorkShift workShift)
    => await _workShiftRepository.Delete(workShift);

        public async Task<WorkShift> Get(Guid id)
            => await _workShiftRepository.Get(id);

        public async Task<IEnumerable<WorkShift>> GetAll()
            => await _workShiftRepository.GetAll();

        public async Task Insert(WorkShift workShift)
    => await _workShiftRepository.Insert(workShift);

        public async Task Update(WorkShift workShift)
            => await _workShiftRepository.Update(workShift);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllWorkShiftsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _workShiftRepository.GetAllWorkShiftsDatatable(sentParameters, searchValue);

    }
}
