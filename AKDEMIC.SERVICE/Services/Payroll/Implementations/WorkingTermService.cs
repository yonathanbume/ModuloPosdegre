using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template.CafobeRequest;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Template.WorkingTerm;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class WorkingTermService : IWorkingTermService
    {
        private readonly IWorkingTermRepository _workingTermRepository;

        public WorkingTermService(IWorkingTermRepository workingTermRepository)
        {
            _workingTermRepository = workingTermRepository;
        }

        public async Task DeleteById(Guid id)
            => await _workingTermRepository.DeleteById(id);

        public async Task<WorkingTerm> Get(Guid id)
            => await _workingTermRepository.Get(id);

        public async Task<WorkingTerm> GetActive()
            => await _workingTermRepository.GetActive();

        public async Task<IEnumerable<WorkingTerm>> GetAll()
            => await _workingTermRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
                    => _workingTermRepository.GetAllDatatable(sentParameters, searchValue);

        public async Task<(IEnumerable<WorkingTerm> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter)
            => await _workingTermRepository.GetAllByPaginationParameter(paginationParameter);

        public async Task Insert(WorkingTerm workingTerm)
            => await _workingTermRepository.Insert(workingTerm);

        public async Task Update(WorkingTerm workingTerm)
            => await _workingTermRepository.Update(workingTerm);

        public Task<int> MaxNumberByYearMonth(int year, int month)
            => _workingTermRepository.MaxNumberByYearMonth(year, month);

        public Task<bool> AnyActive()
            => _workingTermRepository.AnyActive();

        public Task<WorkingTermTemplate> GetLastActive()
            => _workingTermRepository.GetLastActive();


    }
}
