using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IAdmissionTypeRepository : IRepository<AdmissionType>
    {
        Task<IEnumerable<Select2Structs.Result>> GetAdmissionTypesSelect2ClientSide(bool includeTitle = false);
        Task<AdmissionTypeDescount> GetAdmissionTypeDescounts(Guid admissionTypeId, Guid currentApplicationTermId);
        Task<List<Select2Structs.Result>> GetAdmissionTypeJson();
        Task<AdmissionType> GetNameByCellExcel(string cell);
        Task<object> GetAmissionTypeIrregular();
        Task<Guid> GetGuidFirst();
        Task<object> GetAdmissionTypesTerm(Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetAdmissionTypesCategories(DataTablesStructs.SentParameters sentParameters, string search, bool showInactive);
        Task<List<AdmissionType>> GetByAbbreviation(string v);
        Task<AdmissionType> GetAgreementAdmissionType();
    }
}
