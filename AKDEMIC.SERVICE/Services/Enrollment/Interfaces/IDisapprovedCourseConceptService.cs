using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IDisapprovedCourseConceptService
    {
        Task<IEnumerable<DisapprovedCourseConcept>> GetAll();
        Task DeleteRange(IEnumerable<DisapprovedCourseConcept> disapprovedCourseConcepts);
        Task Insert(DisapprovedCourseConcept disapprovedCourseConcept);
        Task InsertRange(IEnumerable<DisapprovedCourseConcept> disapprovedCourseConcepts);
    }
}
