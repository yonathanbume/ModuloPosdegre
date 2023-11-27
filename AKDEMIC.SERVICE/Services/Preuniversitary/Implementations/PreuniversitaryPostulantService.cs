using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Templates;
using AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Implementations
{
    public class PreuniversitaryPostulantService : IPreuniversitaryPostulantService
    {
        private readonly IPreuniversitaryPostulantRepository _preuniversitaryPostulantRepository;

        public PreuniversitaryPostulantService(IPreuniversitaryPostulantRepository preuniversitaryPostulantRepository)
        {
            _preuniversitaryPostulantRepository = preuniversitaryPostulantRepository;
        }

        public async Task<PreuniversitaryPostulant> Get(Guid id)
            => await _preuniversitaryPostulantRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAdmittedPostulantsReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null, bool? onlyPaid = null)
            => await _preuniversitaryPostulantRepository.GetAdmittedPostulantsReportDatatable(sentParameters, termId, searchValue, onlyPaid);

        public async Task<object> GetGradesByStudent(Guid pid)
            => await _preuniversitaryPostulantRepository.GetGradesByStudent(pid);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPaymentDetailsDatatable(DataTablesStructs.SentParameters sentParameters, Guid guid)
            => await _preuniversitaryPostulantRepository.GetPaymentDetailsDatatable(sentParameters, guid);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantFileDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
            => await _preuniversitaryPostulantRepository.GetPostulantFileDatatable(sentParameters, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantPaymentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null)
            => await _preuniversitaryPostulantRepository.GetPostulantPaymentDatatable(sentParameters, termId, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantPaymentDatatable_V2(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue)
            => await _preuniversitaryPostulantRepository.GetPostulantPaymentDatatable_V2(sentParameters, termId, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantPreRegistersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
            => await _preuniversitaryPostulantRepository.GetPostulantPreRegistersDatatable(sentParameters, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantsDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue)
            => await _preuniversitaryPostulantRepository.GetPostulantsDatatable(sentParameters, termId, searchValue);

        public async Task<List<PostulantTemplate>> GetPostulantsReportData(Guid termId, string searchValue = null, bool? onlyPaid = null)
            => await _preuniversitaryPostulantRepository.GetPostulantsReportData(termId, searchValue, onlyPaid);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantsReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null, bool? onlyPaid = null)
            => await _preuniversitaryPostulantRepository.GetPostulantsReportDatatable(sentParameters, termId, searchValue, onlyPaid);

        public async Task Insert(PreuniversitaryPostulant entity)
            => await _preuniversitaryPostulantRepository.Insert(entity);

        public async Task Update(PreuniversitaryPostulant entity)
            => await _preuniversitaryPostulantRepository.Update(entity);

        public async Task<string> UsersWithCodeExist(string userCodePrefix)
            => await _preuniversitaryPostulantRepository.UsersWithCodeExist(userCodePrefix);
    }
}
