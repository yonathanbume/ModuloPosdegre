using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class TeacherSurvey
    {
        public Guid Id { get; set; }

        public Guid SurveyUserId { get; set; }
        public string UserId { get; set; }

        public SurveyUser SurveyUser { get; set; }
        public ApplicationUser User { get; set; }
    }
}
