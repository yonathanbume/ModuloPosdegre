using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class RecognitionHistory
    {
        public Guid Id { get; set; }
        public Guid RecognitionId { get; set; }
        public Recognition Recognition{ get; set; }
        public string Comment { get; set; }
        public string ResolutionNumber { get; set; }
        public string ResolutionFilePath { get; set; }
        public DateTime ResolutionIssueDate { get; set; }
        public string ResolutionDescription { get; set; }
        public string ResolutionTableName { get; set; }
        public string ResolutionUserName { get; set; }
    }
}
