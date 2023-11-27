using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class ExtraTeachingLoadService : IExtraTeachingLoadService
    {
        private readonly IExtraTeachingLoadRepository _extraTeachingLoadRepository;

        public ExtraTeachingLoadService(IExtraTeachingLoadRepository extraTeachingLoadRepository)
        {
            _extraTeachingLoadRepository = extraTeachingLoadRepository;
        }

        public async Task<ExtraTeachingLoad> GetExtraTeachingLoad(Guid termId, string teacherId)
            => await _extraTeachingLoadRepository.GetExtraTeachingLoad(termId, teacherId);

        public async Task Insert(ExtraTeachingLoad entity)
            => await _extraTeachingLoadRepository.Insert(entity);

        public async Task Update(ExtraTeachingLoad entity)
            => await _extraTeachingLoadRepository.Update(entity);
    }
}
