using System;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.SuggestionAndTip
{
    public class SuggestionAndTipTemplate
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string GeneralTitle { get; set; }
        public string Description { get; set; }
        public string ColorRGB { get; set; }
        public int Type { get; set; }
    }
}
