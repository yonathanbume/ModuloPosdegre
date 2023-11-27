using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Implementations
{
    public class PreuniversitaryChannelRepository : Repository<PreuniversitaryChannel>, IPreuniversitaryChannelRepository
    {
        public PreuniversitaryChannelRepository(AkdemicContext context) : base(context)
        {
        }
    }
}
