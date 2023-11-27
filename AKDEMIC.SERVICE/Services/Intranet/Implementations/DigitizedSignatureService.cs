using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class DigitizedSignatureService : IDigitizedSignatureService
    {
        private readonly IDigitizedSignatureRepository _digitizedSignatureRepository;

        public DigitizedSignatureService(IDigitizedSignatureRepository digitizedSignatureRepository)
        {
            _digitizedSignatureRepository = digitizedSignatureRepository;
        }

        public async Task<DigitizedSignature> GetFirst()
            => await _digitizedSignatureRepository.First();

        public async Task Insert(DigitizedSignature entity)
            => await _digitizedSignatureRepository.Insert(entity);

        public async Task Update(DigitizedSignature entity)
            => await _digitizedSignatureRepository.Update(entity);
    }
}
