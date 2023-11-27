using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencySubMenuService : ITransparencySubMenuService
    {
        private readonly ITransparencySubMenuRepository _transparencySubMenuRepository;

        public TransparencySubMenuService(ITransparencySubMenuRepository transparencySubMenuRepository)
        {
            _transparencySubMenuRepository = transparencySubMenuRepository;
        }

        public async Task Delete(TransparencySubMenu entity)
            => await _transparencySubMenuRepository.Delete(entity);

        public async Task<TransparencySubMenu> Get(Guid id)
            => await _transparencySubMenuRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTransparencySubMenuDatatable(DataTablesStructs.SentParameters parameters)
            => await _transparencySubMenuRepository.GetTransparencySubMenuDatatable(parameters);

        public async Task Insert(TransparencySubMenu entity)
            => await _transparencySubMenuRepository.Insert(entity);

        public async Task Update(TransparencySubMenu entity)
            => await _transparencySubMenuRepository.Update(entity);
    }
}
