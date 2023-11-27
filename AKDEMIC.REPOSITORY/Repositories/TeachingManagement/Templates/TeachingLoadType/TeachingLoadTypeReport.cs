using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeachingLoadType
{
    public class TeachingLoadTypeReport
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string LoadType { get; set; }
        public string Name { get; set; }
        public decimal Hours { get; set; }
        public string EndDate { get; set; }
    }
}
