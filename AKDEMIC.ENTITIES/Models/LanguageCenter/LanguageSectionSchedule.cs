using AKDEMIC.CORE.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.LanguageCenter
{
    public class LanguageSectionSchedule
    {
        public Guid Id { get; set; }
        public Guid LanguageSectionId { get; set; }
        public LanguageSection LanguageSection { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        [NotMapped] public string StartTimeText => StartTime.ToLocalDateTimeFormatUtc();
        [NotMapped] public string EndTimeText => EndTime.ToLocalDateTimeFormatUtc();
        public int WeekDay { get; set; }
    }
}
