using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Payroll.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Payroll.Implementations
{
    public class AdministrativeTableService: IAdministrativeTableService
    {
        private readonly IAdministrativeTableRepository _administrativeTableRepository;

        public AdministrativeTableService(IAdministrativeTableRepository administrativeTableRepository)
        {
            _administrativeTableRepository = administrativeTableRepository;
        }

        public Task<bool> AnyByCode(string code, int type ,Guid? id = null)
            => _administrativeTableRepository.AnyByCode(code, type,id);

        public async Task Delete(AdministrativeTable administrativeTable)
            => await _administrativeTableRepository.Delete(administrativeTable);

        public async Task<AdministrativeTable> Get(Guid id)
            => await _administrativeTableRepository.Get(id);

        public async Task<IEnumerable<AdministrativeTable>> GetAll()
            => await _administrativeTableRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllAdministrativeTablesDatatable(DataTablesStructs.SentParameters sentParameters, int type, string searchValue = null)
            => await _administrativeTableRepository.GetAllAdministrativeTablesDatatable(sentParameters, type,searchValue);

        public Task<object> GetSelect2(int? type = null)
            => _administrativeTableRepository.GetSelect2(type);

        public async Task Insert(AdministrativeTable administrativeTable)
            => await _administrativeTableRepository.Insert(administrativeTable);

        public async Task Update(AdministrativeTable administrativeTable)
            => await _administrativeTableRepository.Update(administrativeTable);
    }
}
