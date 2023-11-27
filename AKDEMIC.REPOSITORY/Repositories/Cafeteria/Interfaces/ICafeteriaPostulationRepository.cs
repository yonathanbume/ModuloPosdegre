using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces
{
    public interface ICafeteriaPostulationRepository : IRepository<CafeteriaPostulation>
    {
        Task<CafeteriaPostulation> FirstOrDefaultById(Guid id);

        Task<DataTablesStructs.ReturnedData<object>> GetAllStudentsByIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid serviceTermId,string searchValue = null);
    }
}
