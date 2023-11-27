using System;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Templates.PostulantCardSection
{
    public class PostulantCardSectionTemplate
    {
        public string Description { get;  set; }
        public string Name { get;  set; }
        public Guid Id { get;  set; }
        public bool IsActive { get;  set; }
    }
    public class PostulantCardSectionFieldTemplate
    {
        public int Type { get;  set; }
        public Guid Id { get;  set; }
        public bool IsActive { get;  set; }
        public bool IsFileRequired { get;  set; }
        public bool IsRequired { get;  set; }
        public Guid PostulantCardSectionId { get;  set; }
    }
}
