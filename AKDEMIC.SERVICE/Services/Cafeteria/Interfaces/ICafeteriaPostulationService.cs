using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Cafeteria.Interfaces
{
    public interface ICafeteriaPostulationService
    {
        Task Insert(CafeteriaPostulation cafeteriaPostulation);
        Task Update(CafeteriaPostulation cafeteriaPostulation);

        Task<CafeteriaPostulation> FirstOrDefaultById(Guid id);

        Task<DataTablesStructs.ReturnedData<object>> GetAllStudentsByIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid serviceTermId ,string searchValue = null);
    }
}
