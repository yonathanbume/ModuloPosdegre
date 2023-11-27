using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Implementations
{
    public class PreuniversitaryExamService : IPreuniversitaryExamService
    {
        private readonly IPreuniversitaryExamRepository _preuniversitaryExamRepository;

        public PreuniversitaryExamService(IPreuniversitaryExamRepository preuniversitaryExamRepository)
        {
            _preuniversitaryExamRepository = preuniversitaryExamRepository;
        }

        public async Task<bool> AnyByCode(string code, Guid preuniversitaryTermId, Guid? ignoredId = null)
            => await _preuniversitaryExamRepository.AnyByCode(code, preuniversitaryTermId, ignoredId);

        public async Task Delete(PreuniversitaryExam entity)
            => await _preuniversitaryExamRepository.Delete(entity);

        public async Task<PreuniversitaryExam> Get(Guid id)
            => await _preuniversitaryExamRepository.Get(id);

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid? preuniversitaryTermId)
            => await _preuniversitaryExamRepository.GetDatatable(sentParameters, preuniversitaryTermId);

        public async Task Insert(PreuniversitaryExam entity)
            => await _preuniversitaryExamRepository.Insert(entity);

        public async Task Update(PreuniversitaryExam entity)
            => await _preuniversitaryExamRepository.Update(entity);
    }
}
