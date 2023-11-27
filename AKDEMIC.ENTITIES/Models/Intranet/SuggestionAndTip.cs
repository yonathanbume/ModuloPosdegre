using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class SuggestionAndTip
    {
        public Guid Id { get; set; }
        public string GeneralTitle { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public Guid WelfareCategoryId { get; set; }

        public WelfareCategory WelfareCategory { get; set; }
    }
}
