using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyScholarship
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsFile { get; set; }
    }
}
