using AKDEMIC.ENTITIES.Models.TeachingManagement;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluation
{
    public class EvaluationDetail
    {
        public string FromRoleId { get; set; }
        public string FromRoleName { get; set; }
        public string ToUserId { get; set; }
        public string ToUserName { get; set; }
        public string ToUserFullname { get; set; }
        public object Evaluation { get; set; }
        public string AcademicDepartment { get; set; }
    }
}
