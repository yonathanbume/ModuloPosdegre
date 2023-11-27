using AKDEMIC.ENTITIES.Models.Admission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Interfaces
{
    public interface IAdmissionTypeDescountService
    {
        Task InsertAdmissionTypeDescount(AdmissionTypeDescount admissionTypeDescount);
        Task InsertRange(IEnumerable<AdmissionTypeDescount> admissionTypeDescount);
        Task UpdateAdmissionTypeDescount(AdmissionTypeDescount admissionTypeDescount);
        Task DeleteAdmissionTypeDescount(AdmissionTypeDescount admissionTypeDescount);
        void RemoveRange(IEnumerable<AdmissionTypeDescount> admissionTypeDescounts);
        Task AddAsync(AdmissionTypeDescount admissionTypeDescount);
        Task AddRange(List<AdmissionTypeDescount> admissionTypeDescount);
        Task<AdmissionTypeDescount> GetAdmissionTypeDescountById(Guid id);
        Task<IEnumerable<AdmissionTypeDescount>> GetAllAdmissionTypeDescounts();
        Task<List<AdmissionTypeDescount>> GetAdmissionTypeDescountByTermId(Guid termId);
        Task<AdmissionTypeDescount> GetAdmissionTypeDescountByAdmissionTypeIdAndTermId(Guid admissionTypeId, Guid id);
    }
}
