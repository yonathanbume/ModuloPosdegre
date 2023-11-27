using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ComputerManagement.Interfaces
{
    public interface IComputerSupplierService
    {
        //Generals
        Task<ComputerSupplier> Get(Guid id);
        Task<IEnumerable<ComputerSupplier>> GetAll();
        Task Insert(ComputerSupplier computerSupplier);
        Task Delete(ComputerSupplier computerSupplier);
        Task Update(ComputerSupplier computerSupplier);

        Task<DataTablesStructs.ReturnedData<object>> GetComputerSupplierDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
