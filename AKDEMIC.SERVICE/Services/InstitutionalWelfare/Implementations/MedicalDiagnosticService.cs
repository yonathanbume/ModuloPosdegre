using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class MedicalDiagnosticService : IMedicalDiagnosticService
    {
        private readonly IMedicalDiagnosticRepository  _medicalDiagnosticRepository;

        public MedicalDiagnosticService(IMedicalDiagnosticRepository medicalDiagnosticRepository)
        {
            _medicalDiagnosticRepository = medicalDiagnosticRepository;
        }

        public async Task Delete(MedicalDiagnostic medicalDiagnostic)
        {
            await _medicalDiagnosticRepository.Delete(medicalDiagnostic);
        }

        public async Task<MedicalDiagnostic> Get(Guid id)
        {
            return await _medicalDiagnosticRepository.Get(id);
        }

        public async Task<IEnumerable<MedicalDiagnostic>> GetAll()
        {
            return await _medicalDiagnosticRepository.GetAll();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetMedicalDiagnosticDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _medicalDiagnosticRepository.GetMedicalDiagnosticDatatable(sentParameters, searchValue);
        }

        public async Task<object> GetMedicalDiagnosticSelect2ClientSide()
        {
            return await _medicalDiagnosticRepository.GetMedicalDiagnosticSelect2ClientSide();
        }

        public async Task Insert(MedicalDiagnostic medicalDiagnostic)
        {
            await _medicalDiagnosticRepository.Insert(medicalDiagnostic);
        }

        public async Task InsertRange(IEnumerable<MedicalDiagnostic> entities)
            => await _medicalDiagnosticRepository.InsertRange(entities);

        public async Task Update(MedicalDiagnostic medicalDiagnostic)
        {
            await _medicalDiagnosticRepository.Update(medicalDiagnostic);
        }
    }
}
