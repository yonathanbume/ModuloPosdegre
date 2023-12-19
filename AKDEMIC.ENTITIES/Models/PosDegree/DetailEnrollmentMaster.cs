using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.PosDegree
{
    public  class DetailEnrollmentMaster
    {
        public Guid Id { get; set; }
        public Guid MasterId { get; set; }
        public Master master { get; set; }
        public Guid EnrollmentId { get; set; }
        public Enrollment Enrollment{ get; set; }
    }
}
