using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Implementations
{
    public class TransparencySubMenuFileRepository : Repository<TransparencySubMenuFile> , ITransparencySubMenuFileRepository
    {
        public TransparencySubMenuFileRepository(AkdemicContext context) : base(context)
        {
        }
    }
}
