using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class VacantService : IVacantService
    {
        private readonly IVacantRepository _vacantRepository;

        public VacantService(IVacantRepository vacantRepository)
        {
            _vacantRepository = vacantRepository;
        }

        public async Task InsertVacant(Vacant vacant) =>
            await _vacantRepository.Insert(vacant);

        public async Task AddAsync(Vacant vacant)
            => await _vacantRepository.Add(vacant);
        public async Task UpdateVacant(Vacant vacant) =>
            await _vacantRepository.Update(vacant);

        public async Task DeleteVacant(Vacant vacant) =>
            await _vacantRepository.Delete(vacant);

        public async Task<Vacant> GetVacantById(Guid id) =>
            await _vacantRepository.Get(id);

        public async Task<IEnumerable<Vacant>> GetAllVacants() =>
            await _vacantRepository.GetAll();

        public async Task<List<Vacant>> GetVacantsIncludeCareerAdmissionTerm(CareerApplicationTerm careerTerm)
            => await _vacantRepository.GetVacantsIncludeCareerAdmissionTerm(careerTerm);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCareerTermsVacanciesDatatable(DataTablesStructs.SentParameters sentParameters, int? year, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null)
        {
            return await _vacantRepository.GetCareerTermsVacanciesDatatable(sentParameters, year, applicationTermId, careerId, admissionTypeId, user);
        }

        public async Task<object> GetCareerTermsVacanciesChart(int? year, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null)
        {
            return await _vacantRepository.GetCareerTermsVacanciesChart(year, applicationTermId, careerId, admissionTypeId, user);
        }

        public async Task<IEnumerable<Vacant>> GetAllVacantsWithData(Guid careerApplicationTermId, Guid? academicProgramId = null)
            => await _vacantRepository.GetAllVacantsWithData(careerApplicationTermId, academicProgramId);

        public async Task<List<Vacant>> GetVacantByIdList(Guid id)
            => await _vacantRepository.GetVacantByIdList(id);

        public async Task<IEnumerable<Select2Structs.Result>> GetAvailableCareersSelect2(Guid applicationTermId)
            => await _vacantRepository.GetAvailableCareersSelect2(applicationTermId);

        public async Task<IEnumerable<Select2Structs.Result>> GetAcademicProgramVacantsSelect2(Guid applicationTermId, Guid careerId)
            => await _vacantRepository.GetAcademicProgramVacantsSelect2(applicationTermId, careerId);

        public async Task<IEnumerable<Select2Structs.Result>> GetAvailableCampusesSelect2(Guid applicationTermId, Guid careerId, Guid? academicProgramId = null)
            => await _vacantRepository.GetAvailableCampusesSelect2(applicationTermId, careerId, academicProgramId);

        public async Task DeleteRange(List<Vacant> vacants) 
            => await _vacantRepository.DeleteRange(vacants);

        public async Task<bool> IsCareerAvailableInCampus(Guid campusId, Guid careerId, Guid? academicProgramId = null)
            => await _vacantRepository.IsCareerAvailableInCampus(campusId, careerId, academicProgramId);

        public async Task InsertRange(List<Vacant> vacants)
            => await _vacantRepository.InsertRange(vacants);
    }
}
