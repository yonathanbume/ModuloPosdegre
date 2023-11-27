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
    public class RemunerationPayrollTypeService : IRemunerationPayrollTypeService
    {
        private readonly IRemunerationPayrollTypeRepository _remunerationPayrollTypeRepository;

        public RemunerationPayrollTypeService(IRemunerationPayrollTypeRepository remunerationPayrollTypeRepository)
        {
            _remunerationPayrollTypeRepository = remunerationPayrollTypeRepository;
        }

        public Task Delete(RemunerationPayrollType remunerationPayrollType)
            => _remunerationPayrollTypeRepository.Delete(remunerationPayrollType);

        public Task<RemunerationPayrollType> Get(Guid id)
            => _remunerationPayrollTypeRepository.Get(id);

        public Task<IEnumerable<RemunerationPayrollType>> GetAll()
            => _remunerationPayrollTypeRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, Guid? payrollTypeId = null, string searchValue = null)
            => _remunerationPayrollTypeRepository.GetAllDatatable(sentParameters, payrollTypeId, searchValue);

        public Task<RemunerationPayrollType> GetWithIncludes(Guid id)
            => _remunerationPayrollTypeRepository.GetWithIncludes(id);

        public Task Insert(RemunerationPayrollType remunerationPayrollType)
            => _remunerationPayrollTypeRepository.Insert(remunerationPayrollType);

        public Task Update(RemunerationPayrollType remunerationPayrollType)
            => _remunerationPayrollTypeRepository.Update(remunerationPayrollType);
    }
}
