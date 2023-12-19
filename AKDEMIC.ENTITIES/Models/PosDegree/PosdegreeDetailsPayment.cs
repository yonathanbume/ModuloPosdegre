using AKDEMIC.ENTITIES.Models.EconomicManagement;
using DocumentFormat.OpenXml.ExtendedProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.PosDegree
{
    public  class PosdegreeDetailsPayment
    {
      public Guid Id { get; set; }
      public string TypePayment { get; set;}
      public string DescriptionPayment { get; set;}
      public Guid PosdegreeStudentId { get; set; }
      public PosdegreeStudent PosdegreeStudent { get; set; }
      public Guid PaymentId { get; set; }
      public Payment Payment { get; set; }
      

    }
}
