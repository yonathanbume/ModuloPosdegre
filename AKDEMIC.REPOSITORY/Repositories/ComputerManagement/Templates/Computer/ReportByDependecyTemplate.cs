using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.ComputerManagement.Templates.Computer
{
    public class ReportByDependecyTemplate
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public List<object[]> Data { get; set; }
    }
}
