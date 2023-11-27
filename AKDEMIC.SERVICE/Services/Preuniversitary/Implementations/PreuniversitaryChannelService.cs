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
    public class PreuniversitaryChannelService : IPreuniversitaryChannelService
    {
        private readonly IPreuniversitaryChannelRepository _preuniversitaryChannelepository;
        public PreuniversitaryChannelService(IPreuniversitaryChannelRepository preuniversitaryChannelRepository)
        {
            _preuniversitaryChannelepository = preuniversitaryChannelRepository;
        }

        public async Task DeleteById(PreuniversitaryChannel entity)
            => await _preuniversitaryChannelepository.DeleteById(entity);

        public async Task<PreuniversitaryChannel> Get(Guid id)
            => await _preuniversitaryChannelepository.Get(id);

        public async Task<IEnumerable<PreuniversitaryChannel>> GetAll()
            => await _preuniversitaryChannelepository.GetAll();

        public async Task Insert(PreuniversitaryChannel entity)
            => await _preuniversitaryChannelepository.Insert(entity);

        public async Task Update(PreuniversitaryChannel entity)
            => await _preuniversitaryChannelepository.Update(entity);
    }
}
