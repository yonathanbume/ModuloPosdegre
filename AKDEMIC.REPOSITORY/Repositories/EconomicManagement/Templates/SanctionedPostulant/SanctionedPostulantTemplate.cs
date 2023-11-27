using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.SanctionedPostulant
{
    public class SanctionedPostulantTemplate
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Document { get; set; }
        public string Term { get; set; }
        public string ApplicationTerm { get; set; }    
    }
}
