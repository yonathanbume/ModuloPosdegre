using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class DisapprovedCourseConceptService : IDisapprovedCourseConceptService
    {
        private readonly IDisapprovedCourseConceptRepository _disapprovedCourseConceptRepository;
        public DisapprovedCourseConceptService(IDisapprovedCourseConceptRepository disapprovedCourseConceptRepository)
        {
            _disapprovedCourseConceptRepository = disapprovedCourseConceptRepository;
        }

        public async Task DeleteRange(IEnumerable<DisapprovedCourseConcept> disapprovedCourseConcepts) => await _disapprovedCourseConceptRepository.DeleteRange(disapprovedCourseConcepts);

        public async Task<IEnumerable<DisapprovedCourseConcept>> GetAll() => await _disapprovedCourseConceptRepository.GetAll();

        public async Task Insert(DisapprovedCourseConcept disapprovedCourseConcept) => await _disapprovedCourseConceptRepository.Insert(disapprovedCourseConcept);

        public async Task InsertRange(IEnumerable<DisapprovedCourseConcept> disapprovedCourseConcepts) => await _disapprovedCourseConceptRepository.InsertRange(disapprovedCourseConcepts);
    }
}
