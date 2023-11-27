using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IMedicalDiagnosticService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetMedicalDiagnosticDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<IEnumerable<MedicalDiagnostic>> GetAll();      
        Task Insert(MedicalDiagnostic medicalDiagnostic);
        Task InsertRange(IEnumerable<MedicalDiagnostic> entities);
        Task Update(MedicalDiagnostic medicalDiagnostic);
        Task Delete(MedicalDiagnostic medicalDiagnostic);
        Task<MedicalDiagnostic> Get(Guid id);
        Task<object> GetMedicalDiagnosticSelect2ClientSide();
    }
}
