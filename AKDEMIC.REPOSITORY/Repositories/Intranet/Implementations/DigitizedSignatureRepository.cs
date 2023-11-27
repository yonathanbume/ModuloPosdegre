using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class DigitizedSignatureRepository : Repository<DigitizedSignature> , IDigitizedSignatureRepository
    {
        public DigitizedSignatureRepository(AkdemicContext context) : base(context) { }
    }
}
