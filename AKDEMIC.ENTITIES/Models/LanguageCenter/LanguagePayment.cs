using System;

namespace AKDEMIC.ENTITIES.Models.LanguageCenter
{
    public class LanguagePayment
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime DateIssuance { get; set; }
        public Guid LanguageSectionId { get; set; }
        public LanguageSection LanguageSection { get; set; }
        public int Quota { get; set; }
        public bool Isissued { get; set; } = false;
    }
}
