using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class SupportOfficeService : ISupportOfficeService
    {
        private readonly ISupportOfficeRepository  _supportOfficeRepository;

        public SupportOfficeService(ISupportOfficeRepository supportOfficeRepository)
        {
            _supportOfficeRepository = supportOfficeRepository;
        }

        public async Task Delete(SupportOffice supportOffice)
            => await _supportOfficeRepository.Delete(supportOffice);

        public async Task DeleteById(Guid id)
            => await _supportOfficeRepository.DeleteById(id);

        public async Task<SupportOffice> Get(Guid supportOfficeId)
            => await _supportOfficeRepository.Get(supportOfficeId);

        public async Task<IEnumerable<SupportOffice>> GetAll()
            => await _supportOfficeRepository.GetAll();

        public async Task Insert(SupportOffice supportOffice)
            => await _supportOfficeRepository.Insert(supportOffice);

        public async Task Update(SupportOffice supportOffice)
            => await _supportOfficeRepository.Update(supportOffice);
        public async Task<SupportOffice> Get(string supportOfficeId)
            => await _supportOfficeRepository.Get(supportOfficeId);
        public async Task<DataTablesStructs.ReturnedData<object>> GetSupportOfficeDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _supportOfficeRepository.GetSupportOfficeDatatable(sentParameters, search);
        public async Task<List<SupportOffice>> GetAllWithOut(Guid? supportOfficeId = null)
            => await _supportOfficeRepository.GetAllWithOut(supportOfficeId);

        public Task<object> GetSelect2WithOut(Guid? supportOfficeId = null)
            => _supportOfficeRepository.GetSelect2WithOut(supportOfficeId);
    }
}
