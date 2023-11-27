using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.AcademicExchange
{
    public class Postulation : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid ScholarshipId { get; set; }
        public Scholarship Scholarship { get; set; }
        public Guid QuestionnaireId { get; set; }
        public Questionnaire Questionnaire { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public byte State { get; set; } = ConstantHelpers.SCHOLARSHIP.POSTULATION.States.PENDING;
        public bool? Admitted { get; set; }
        public string Commentary { get; set; }
        public bool IsCompleted { get; set; }
        public bool CanEdit { get; set; }
        public bool HasUserResponse { get; set; }
        public Guid? PostulationInformationId { get; set; }
        public PostulationInformation PostulationInformation { get; set; }
        public ICollection<QuestionnaireAnswerByUser> QuestionnaireAnswerByUsers { get; set; }
        public ICollection<PostulationFile> PostulationFiles { get; set; }
    }
}
