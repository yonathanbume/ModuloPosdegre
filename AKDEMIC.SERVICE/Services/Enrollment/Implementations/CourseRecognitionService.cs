using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CourseRecognitionService : ICourseRecognitionService
    {
        private readonly ICourseRecognitionRepository _courseRecognitionRepository;
        public CourseRecognitionService(ICourseRecognitionRepository courseRecognitionRepository)
        {
            _courseRecognitionRepository = courseRecognitionRepository;
        }

        public async Task DeleteById(Guid id) => await _courseRecognitionRepository.DeleteById(id);

        public async Task<object> GetRecognitionAcademicHistoriesDatatable(Guid recognitionId)
            => await _courseRecognitionRepository.GetRecognitionAcademicHistoriesDatatable(recognitionId);
    }
}
