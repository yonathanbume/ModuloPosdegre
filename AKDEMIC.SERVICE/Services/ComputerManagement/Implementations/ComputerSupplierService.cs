using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Interfaces;
using AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Implementations
{
    public class ComputerSupplierService: IComputerSupplierService
    {
        private readonly IComputerSupplierRepository _computerSupplierRepository;
        public ComputerSupplierService(IComputerSupplierRepository computerSupplierRepository)
        {
            _computerSupplierRepository = computerSupplierRepository;
        }

        public async Task Delete(ComputerSupplier computerSupplier)
            => await _computerSupplierRepository.Delete(computerSupplier);

        public async Task<ComputerSupplier> Get(Guid id)
            => await _computerSupplierRepository.Get(id);

        public async Task<IEnumerable<ComputerSupplier>> GetAll()
            => await _computerSupplierRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetComputerSupplierDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _computerSupplierRepository.GetComputerSupplierDatatable(sentParameters, searchValue);

        public async Task Insert(ComputerSupplier computerSupplier)
            => await _computerSupplierRepository.Insert(computerSupplier);

        public async Task Update(ComputerSupplier computerSupplier)
            => await _computerSupplierRepository.Update(computerSupplier);
        
    }
}
