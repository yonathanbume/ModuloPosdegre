using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class RecognitionService : IRecognitionService
    {
        private readonly IRecognitionRepository _recognitionRepository;

        public RecognitionService(IRecognitionRepository recognitionRepository)
        {
            _recognitionRepository = recognitionRepository;
        }
      
        public async Task Add(Recognition recognition) => await _recognitionRepository.Add(recognition);

        public async Task<Recognition> Get(Guid id) => await _recognitionRepository.Get(id);

        public async Task<Recognition> GetByStudentId(Guid studentId)
        {
            return await _recognitionRepository.GetByStudentId(studentId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid recognitionId, string searchValue = null)
            => await _recognitionRepository.GetCoursesDataDatatable(sentParameters, recognitionId, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null)
            => await _recognitionRepository.GetDataDatatable(sentParameters, searchValue, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, Guid? admissionType = null, ClaimsPrincipal user = null)
            => await _recognitionRepository.GetStudentsDataDatatable(sentParameters, searchValue, facultyId, careerId, admissionType, user);

        public async Task Insert(Recognition recognition) => await _recognitionRepository.Insert(recognition);

        public async Task Update(Recognition recognition) => await _recognitionRepository.Update(recognition);
    }
}
