using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class AcademicAgreementService : IAcademicAgreementService
    {
        private readonly IAcademicAgreementRepository _academicAgreementRepository;

        public AcademicAgreementService(IAcademicAgreementRepository academicAgreementRepository)
        {
            _academicAgreementRepository = academicAgreementRepository;
        }

        public async Task<int> Count()
        {
            return await _academicAgreementRepository.Count();
        }

        public async Task DeleteById(Guid id)
        {
            await _academicAgreementRepository.DeleteById(id);
        }

        public async Task<ENTITIES.Models.AcademicExchange.AcademicAgreement> Get(Guid id)
        {
            return await _academicAgreementRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAcademicAgreementDataTable(DataTablesStructs.SentParameters parameters, Guid? type, int statusId, string startDate, bool? onlyActive, string search, Guid? careerId = null, Guid? facultyId = null)
        {
            return await _academicAgreementRepository.GetAcademicAgreementDataTable(parameters, type, statusId, startDate, onlyActive, search, careerId, facultyId);
        }

        public async Task<object> GetAcademicAgreementChart()
            => await _academicAgreementRepository.GetAcademicAgreementChart();

        public async Task<IEnumerable<Select2Structs.Result>> GetAcademicAgreementSelect2ClientSide(byte? type)
        {
            return await _academicAgreementRepository.GetAcademicAgreementSelect2ClientSide(type);
        }

        public async Task Insert(ENTITIES.Models.AcademicExchange.AcademicAgreement newAgreement)
        {
            await _academicAgreementRepository.Insert(newAgreement);
        }

        public async Task<bool> IsAnotherWithResolutionNumber(string resolutionNumber, Guid? id = null)
        {
            return await _academicAgreementRepository.IsAnotherWithResolutionNumber(resolutionNumber, id);
        }

        public async Task Update(ENTITIES.Models.AcademicExchange.AcademicAgreement editing)
        {
            await _academicAgreementRepository.Update(editing);
        }

        public async Task<object> GetAcademicAgreementActiveAndInactiveChart()
            => await _academicAgreementRepository.GetAcademicAgreementActiveAndInactiveChart();
    }
}
