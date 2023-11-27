using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public sealed class AcademicSecretaryService : IAcademicSecretaryService
    {
        private readonly IAcademicSecretaryRepository _academicSecretaryRepository;

        public AcademicSecretaryService(IAcademicSecretaryRepository academicSecretaryRepository)
        {
            _academicSecretaryRepository = academicSecretaryRepository;
        }

        Task<bool> IAcademicSecretaryService.AnyByTeacherIdAndFacultyId(Guid teacherId, Guid facultyId)
            => _academicSecretaryRepository.AnyByTeacherIdAndFacultyId(teacherId, facultyId);

        Task<int> IAcademicSecretaryService.CountByTeacherId(Guid teacherId)
            => _academicSecretaryRepository.CountByTeacherId(teacherId);

        Task IAcademicSecretaryService.DeleteAsync(AcademicSecretary academicSecretary)
            => _academicSecretaryRepository.Delete(academicSecretary);

        Task<object> IAcademicSecretaryService.GetAllAsModelA(Guid? facultyId)
            => _academicSecretaryRepository.GetAllAsModelA(facultyId);

        Task<object> IAcademicSecretaryService.GetAsModelB(Guid? id)
            => _academicSecretaryRepository.GetAsModelB(id);

        Task<AcademicSecretary> IAcademicSecretaryService.GetAsync(Guid id)
            => _academicSecretaryRepository.Get(id);

        Task IAcademicSecretaryService.InsertAsync(AcademicSecretary academicSecretary)
            => _academicSecretaryRepository.Insert(academicSecretary);

        Task IAcademicSecretaryService.UpdateAsync(AcademicSecretary academicSecretary)
            => _academicSecretaryRepository.Update(academicSecretary);
    }
}