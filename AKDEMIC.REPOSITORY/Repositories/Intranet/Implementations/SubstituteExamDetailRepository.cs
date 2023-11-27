using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class SubstituteExamDetailRepository : Repository<SubstituteExamDetail> , ISubstituteExamDetailRepository
    {
        public SubstituteExamDetailRepository(AkdemicContext context) : base(context)
        {

        }
    }
}
