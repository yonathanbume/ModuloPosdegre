using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Interfaces
{
    public interface INormService
    {
        Task InsertNorm(Norm norm);
        Task UpdateNorm(Norm norm);
        Task DeleteNorm(Norm norm);
        Task<Norm> GetNormById(Guid id);
        Task<DataTablesStructs.ReturnedData<NormTemplate>> GetAllNormDatatable(DataTablesStructs.SentParameters sentParameters, SearchNormTemplate search);
        Task<NormTemplate> GetNormTemplateById(Guid id);
    }
}
