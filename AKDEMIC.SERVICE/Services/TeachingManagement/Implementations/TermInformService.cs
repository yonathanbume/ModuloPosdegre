using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TermInform;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public sealed class TermInformService : ITermInformService
    {
        private readonly ITermInformRepository _termInformRepository;

        public TermInformService(ITermInformRepository termInformRepository)
        {
            _termInformRepository = termInformRepository;
        }

        public async Task<bool> AnyByType(Guid termId, byte type)
            => await _termInformRepository.AnyByType(termId, type);

        public async Task<bool> AnyTermInformTeacher(Guid id)
            => await _termInformRepository.AnyTermInformTeacher(id);

        public async Task<object> GetByFilters(Guid termId, byte type)
            => await _termInformRepository.GetByFilters(termId, type);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTermInformDatatable(DataTablesStructs.SentParameters parameters, Guid? termId)
            => await _termInformRepository.GetTermInformDatatable(parameters, termId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTermInformReportDatatable(DataTablesStructs.SentParameters parameters, Guid termId, byte type, string searchValue)
            => await _termInformRepository.GetTermInformReportDatatable(parameters, termId, type, searchValue);

        public async Task<TermInformTemplate> GetTermInformTemplate(string teacherId, byte requestType)
            => await _termInformRepository.GetTermInformTemplate(teacherId, requestType);

        Task ITermInformService.DeleteAsync(TermInform termInform)
            => _termInformRepository.Delete(termInform);

        Task<TermInform> ITermInformService.GetAsync(Guid id)
            => _termInformRepository.Get(id);

        Task ITermInformService.InsertAsync(TermInform termInform)
            => _termInformRepository.Insert(termInform);

        Task ITermInformService.UpdateAsync(TermInform termInform)
            => _termInformRepository.Update(termInform);
    }
}