using AKDEMIC.ENTITIES.Models.AcademicExchange;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Templates
{
    public class GalleryTemplate
    {
        public IEnumerable<Gallery> Gallery { get; set; }
        public int TotalRecords { get; set; }
    }
}
