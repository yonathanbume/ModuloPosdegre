using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates
{
    public class CulturalActivityHomeTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Place { get; set; }
        public string Price { get; set; }


        public string Objective { get; set; }
        public string Strategy { get; set; }
        public string Activities { get; set; }
        public string Users { get; set; }
        public string Competencies { get; set; }
        public string Career { get; set; }
        public bool IsPrivate { get; set; }
        public List<ActivityFile> Files { get; set; }

    }

    public class ActivityFile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
