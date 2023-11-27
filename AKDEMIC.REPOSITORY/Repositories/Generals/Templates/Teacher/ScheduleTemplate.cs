using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Teacher
{
    public class ScheduleTemplate
    {
        public Guid Id { get; set; }
        public byte Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool AllDay { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
}
