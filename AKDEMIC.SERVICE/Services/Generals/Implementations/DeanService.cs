using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class DeanService : IDeanService
    {
        private readonly IDeanRepository _deanRepository;

        public DeanService(IDeanRepository deanRepository)
        {
            _deanRepository = deanRepository;
        }

        public async Task DeleteById(string deanId)
        {
            await _deanRepository.DeleteById(deanId);
        }

        public async Task<Dean> Get(string id)
            => await _deanRepository.Get(id);

        public async Task Insert(Dean dean)
        {
            await _deanRepository.Insert(dean);
        }
    }
}
