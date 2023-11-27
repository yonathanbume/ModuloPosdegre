using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces
{
    public interface INormRepository : IRepository<Norm>
    {
        Task<DataTablesStructs.ReturnedData<NormTemplate>> GetAllNormDatatable(DataTablesStructs.SentParameters sentParameters, SearchNormTemplate search);
        Task<NormTemplate> GetNormById(Guid id);
    }
}
