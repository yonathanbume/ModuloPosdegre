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
    public class WageLevelService : IWageLevelService
    {
        private readonly IWageLevelRepository _wageLevelRepository;

        public WageLevelService(IWageLevelRepository wageLevelRepository)
        {
            _wageLevelRepository = wageLevelRepository;
        }
        public Task<bool> AnyByCode(string code, Guid? id = null)
            => _wageLevelRepository.AnyByCode(code, id);

        public Task Delete(WageLevel wageLevel)
            => _wageLevelRepository.Delete(wageLevel);

        public Task<WageLevel> Get(Guid id)
            => _wageLevelRepository.Get(id);

        public Task<IEnumerable<WageLevel>> GetAll()
            => _wageLevelRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllWageLevelsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _wageLevelRepository.GetAllWageLevelsDatatable(sentParameters, searchValue);

        public Task Insert(WageLevel wageLevel)
            => _wageLevelRepository.Insert(wageLevel);

        public Task Update(WageLevel wageLevel)
            => _wageLevelRepository.Update(wageLevel);
    }
}
