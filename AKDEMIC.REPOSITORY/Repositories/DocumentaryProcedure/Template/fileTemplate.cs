using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Template
{
    public class FileTemplate
    {
        public Guid Id { get; set; }
        public string RequirementName { get; set; }
        public string RequirementCode { get; set; }
        public string Filename { get; set; }
        public long Filesize { get; set; }
        public string Path { get; set; }
        public string Dependency { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
    }
}
