using System;

namespace AKDEMIC.INTRANET.ViewModels.SurveyViewModels
{
    public class SurveyViewModel
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; } 
        public string Code { get; set; } 
        public string PublicationDate { get; set; } 
        public string FinishDate { get; set; }
    }
}
