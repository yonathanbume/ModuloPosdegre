using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Classifier
{
    public class ClassifierTemplate
    {
        //public string Code { get; set; }
        //public string Name { get; set; }
        //public decimal Total { get; set; }

        //public List<ClassifierChildTemplate> Childs { get; set; }

        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Total { get; set; }
        public Guid? ParentId { get; set; }
    }

    public class ClassifierChildTemplate
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Total { get; set; }
        public Guid? ParentId { get; set; }
    }
}
