using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class StudentFamilyService : IStudentFamilyService
    {
        private readonly IStudentFamilyRepository _studentFamilyRepository;

        public StudentFamilyService(IStudentFamilyRepository studentFamilyRepository)
        {
            _studentFamilyRepository = studentFamilyRepository;
        }

        public async Task AddAsync(StudentFamily studentFamily)
            => await _studentFamilyRepository.Add(studentFamily);
        public async Task InsertStudentFamily(StudentFamily studentFamily) =>
            await _studentFamilyRepository.Insert(studentFamily);

        public async Task UpdateStudentFamily(StudentFamily studentFamily) =>
            await _studentFamilyRepository.Update(studentFamily);

        public async Task DeleteStudentFamily(StudentFamily studentFamily) =>
            await _studentFamilyRepository.Delete(studentFamily);

        public async Task<StudentFamily> GetStudentFamilyById(Guid id) =>
            await _studentFamilyRepository.Get(id);

        public async Task<IEnumerable<StudentFamily>> GetAllStudentFamilys() =>
            await _studentFamilyRepository.GetAll();

        public async Task<IEnumerable<StudentFamily>> GetAllStudentFamilysWithData()
            => await _studentFamilyRepository.GetAllStudentFamilysWithData();

        public async Task<List<StudentFamily>> GetStudentFamilyByStudentId(Guid studentId)
            => await _studentFamilyRepository.GetStudentFamilyByStudentId(studentId);
        public async Task<StudentFamily> GetStudentFamilyByUsername(string username)
            => await _studentFamilyRepository.GetStudentFamilyByUsername(username);
        public async Task<object> GetStudentFamilySelectById(Guid id)
            => await _studentFamilyRepository.GetStudentFamilySelectById(id);
        public async Task<object> GetStudentFamilyByStudentIdSelect(Guid studentId)
            => await _studentFamilyRepository.GetStudentFamilyByStudentIdSelect(studentId);
        public async Task<StudentFamily> GetStudentFamilyByStudentInformationInclude(Guid studentInformationId)
            => await _studentFamilyRepository.GetStudentFamilyByStudentInformationInclude(studentInformationId);

        public async Task<object> GetStudentFamilySelecByIdInformation(Guid id)
            => await _studentFamilyRepository.GetStudentFamilySelecByIdInformation(id);

        public Task<DataTablesStructs.ReturnedData<object>> GetAllFamilyMembersFromStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId)
            => _studentFamilyRepository.GetAllFamilyMembersFromStudentDatatable(sentParameters, studentId);
    }
}
