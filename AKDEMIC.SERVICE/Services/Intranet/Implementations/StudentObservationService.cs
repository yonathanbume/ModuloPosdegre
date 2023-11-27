using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class StudentObservationService : IStudentObservationService
    {
        private readonly IStudentObservationRepository _studentObservationRepository;

        public StudentObservationService(IStudentObservationRepository studentObservationRepository)
        {
            _studentObservationRepository = studentObservationRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<StudentObservation>> GetStudentObservationDatatable(DataTablesStructs.SentParameters sentParameters, Guid? studentId = null, string searchValue = null)
            => await _studentObservationRepository.GetStudentObservationDatatable(sentParameters, studentId, searchValue);

        public async Task Insert(StudentObservation entity)
            => await _studentObservationRepository.Insert(entity);

        public async Task<DataTablesStructs.ReturnedData<object>> GetObservationsDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId)
            => await _studentObservationRepository.GetObservationsDatatable(sentParameters, studentId);
        public async Task<StudentObservation> GetByStudentId(Guid studentId)
            => await _studentObservationRepository.GetByStudentId(studentId);
        public async Task Update(StudentObservation entity)
            => await _studentObservationRepository.Update(entity);

        public async Task InsertRange(List<StudentObservation> entities)
            => await _studentObservationRepository.InsertRange(entities);

        public Task<IEnumerable<StudentObservation>> GetAllByType(byte type)
            => _studentObservationRepository.GetAllByType(type);

        public Task<StudentObservation> Add(StudentObservation entity)
            => _studentObservationRepository.Add(entity);

        public Task SaveChanges()
            => _studentObservationRepository.SaveChanges();

        public async Task<DataTablesStructs.ReturnedData<object>> GetResignatedStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
            => await _studentObservationRepository.GetResignatedStudentsDatatable(sentParameters, searchValue, termId, facultyId, careerId, user);

        public Task<StudentObservation> Get(Guid id)
            => _studentObservationRepository.Get(id);

        public async Task<List<StudentObservation>> GetStudentObservations(Guid termId, Guid studentId, byte type)
            => await _studentObservationRepository.GetStudentObservations(termId, studentId, type);
    }
}
