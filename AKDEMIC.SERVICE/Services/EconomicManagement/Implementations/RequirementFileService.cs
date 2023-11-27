using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class RequirementFileService : IRequirementFileService
    {
        private readonly IRequirementFileRepository _requirementFileRepository;

        public RequirementFileService(IRequirementFileRepository requirementFileRepository)
        {
            _requirementFileRepository = requirementFileRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<RequirementFile>> GetRequirementFilesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _requirementFileRepository.GetRequirementFilesDatatable(sentParameters, searchValue);
        }

        public async Task Delete(RequirementFile requirementFile) =>
            await _requirementFileRepository.Delete(requirementFile);

        public async Task Insert(RequirementFile requirementFile) =>
            await _requirementFileRepository.Insert(requirementFile);
    }
}
