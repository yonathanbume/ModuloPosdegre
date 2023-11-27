using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class StudentPortfolioType : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? DependencyId { get; set; }
        public Dependency Dependency { get; set; }

        public bool CanUploadStudent { get; set; }
        public byte Type { get; set; } = ConstantHelpers.Intranet.STUDENT_PORTFOLIO_TYPE.GENERAL;

        public ICollection<StudentPortfolio> StudentPortfolios { get; set; }
    }
}
