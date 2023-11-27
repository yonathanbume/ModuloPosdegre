using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces
{
    public interface IMedicalDiagnosticRepository : IRepository<MedicalDiagnostic>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetMedicalDiagnosticDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<object> GetMedicalDiagnosticSelect2ClientSide();
    }
}
