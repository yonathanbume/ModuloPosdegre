using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.VisitManagement
{
    public class Visit 
    {
        public Guid Id { get; set; }

        public DateTime VisitDate { get; set; }
        public string Reason { get; set; }
        public Guid DependencyId { get; set; }
        public Dependency Dependency { get; set; }

        //USER TO VISIT INFORMATION
        public string UserToVisitId { get; set; }
        public ApplicationUser UserToVisit { get; set; }

        //VISITOR INFORMATION
        public Guid VisitorInformationId { get; set; }
        public VisitorInformation VisitorInformation { get; set; }

        [NotMapped]
        public string VisitFormattedDateTime { get; set; }

        [NotMapped]
        public string VisitFormattedDate { get; set; }
        [NotMapped]
        public string VisitFormattedTime { get; set; }

    }
}
