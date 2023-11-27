using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class PayrollConceptService : IPayrollConceptService
    {
        private readonly IPayrollConceptRepository _payrollConceptRepository;

        public PayrollConceptService(IPayrollConceptRepository payrollConceptRepository)
        {
            _payrollConceptRepository = payrollConceptRepository;
        }

        public Task<bool> AnyByCode(string code, Guid? id = null)
          => _payrollConceptRepository.AnyByCode(code, id);
  

        public async Task Delete(PayrollConcept payrollConcept)
            => await _payrollConceptRepository.Delete(payrollConcept);

        public async Task<PayrollConcept> Get(Guid id)
            => await _payrollConceptRepository.Get(id);

        public async Task<IEnumerable<PayrollConcept>> GetAll()
            => await _payrollConceptRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllPayrollConceptsDatatable(DataTablesStructs.SentParameters sentParameters, int type, string searchValue = null)
            => await _payrollConceptRepository.GetAllPayrollConceptsDatatable(sentParameters, type, searchValue);



        public async Task Insert(PayrollConcept payrollConcept)
            => await _payrollConceptRepository.Insert(payrollConcept);

        public async Task Update(PayrollConcept payrollConcept)
            => await _payrollConceptRepository.Update(payrollConcept);
    }
}
