using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class DirectiveService : IDirectiveService
    {
        private readonly IDirectiveRepository _directiveRepository;
        public DirectiveService(IDirectiveRepository directiveRepository)
        {
            _directiveRepository = directiveRepository;
        }

        public async Task DeleteById(Guid id) => await _directiveRepository.DeleteById(id);

        public async Task<Directive> Get(Guid id) => await _directiveRepository.Get(id);

        public async Task<IEnumerable<Directive>> GetAll() => await _directiveRepository.GetAll();

        public async Task<object> GetDataTable(DataTablesStructs.SentParameters sentParameters)
             => await _directiveRepository.GetDataTable(sentParameters);

        public IQueryable<Directive> GetIQueryable()
            => _directiveRepository.GetIQueryable();

        public async Task Insert(Directive directive) => await _directiveRepository.Insert(directive);

        public async Task Update(Directive directive) => await _directiveRepository.Update(directive);
    }
}
