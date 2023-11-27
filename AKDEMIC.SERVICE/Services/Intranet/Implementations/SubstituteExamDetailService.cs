using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class SubstituteExamDetailService : ISubstituteExamDetailService
    {
        private readonly ISubstituteExamDetailRepository _substituteExamDetailRepository;

        public SubstituteExamDetailService(ISubstituteExamDetailRepository substituteExamDetailRepository)
        {
            _substituteExamDetailRepository = substituteExamDetailRepository;
        }

        public async Task Insert(SubstituteExamDetail entity)
            => await _substituteExamDetailRepository.Insert(entity);
    }
}
