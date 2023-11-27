using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class WorkerRetirementSystemHistory
    {
        public Guid Id { get; set; }
        [Required]
        public byte RetirementSystem { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? LeavingDate { get; set; }
        public string UserId { get; set; }
        public bool Active { get; set; }
        //AFP
        public string AfpUserCode { get; set; } //CUSPP
        public Guid? PrivateManagementPensionFundId { get; set; }
        public ApplicationUser User { get; set; }
        public PrivateManagementPensionFund PrivateManagementPensionFund { get; set; }
    }
}
