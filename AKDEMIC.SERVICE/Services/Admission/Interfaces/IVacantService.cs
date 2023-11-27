using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IVacantService
    {
        Task InsertVacant(Vacant vacant);
        Task InsertRange(List<Vacant> vacants);
        Task AddAsync(Vacant vacant);
        Task UpdateVacant(Vacant vacant);
        Task DeleteVacant(Vacant vacant);
        Task DeleteRange(List<Vacant> vacants);
        Task<Vacant> GetVacantById(Guid id);
        Task<IEnumerable<Vacant>> GetAllVacants();
        Task<List<Vacant>> GetVacantsIncludeCareerAdmissionTerm(CareerApplicationTerm careerTerm);
        Task<DataTablesStructs.ReturnedData<object>> GetCareerTermsVacanciesDatatable(DataTablesStructs.SentParameters sentParameters, int? year, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null);
        Task<object> GetCareerTermsVacanciesChart(int? year, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null);
        Task<IEnumerable<Vacant>> GetAllVacantsWithData(Guid careerApplicationTermId, Guid? academicProgramId = null);
        Task<List<Vacant>> GetVacantByIdList(Guid id);
        Task<IEnumerable<Select2Structs.Result>> GetAvailableCareersSelect2(Guid applicationTermId);
        Task<IEnumerable<Select2Structs.Result>> GetAcademicProgramVacantsSelect2(Guid applicationTermId, Guid careerId);
        Task<IEnumerable<Select2Structs.Result>> GetAvailableCampusesSelect2(Guid applicationTermId, Guid careerId, Guid? academicProgramId = null);
        Task<bool> IsCareerAvailableInCampus(Guid campusId, Guid careerId, Guid? academicProgramId = null);
    }
}
