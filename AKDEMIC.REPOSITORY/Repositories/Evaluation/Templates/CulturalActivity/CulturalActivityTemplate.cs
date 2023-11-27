using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates.CulturalActivity
{
    public class CulturalActivityTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        public string UrlPicture { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
