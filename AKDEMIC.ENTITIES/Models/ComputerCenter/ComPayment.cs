using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;

namespace AKDEMIC.ENTITIES.Models.ComputerCenter
{
    public class ComPayment
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime DateIssuance { get; set; }        
        public Guid ComCourseId { get; set; }        
        public ComCourse ComCourse { get; set; }        
        public int Nquota { get; set; }
        public bool Isissued { get; set; } = false ;
        public Guid? PaymentId { get; set; }
        public Payment Payment { get; set; }
    }
}
