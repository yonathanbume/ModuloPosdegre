using System;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class ImageCompany
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public Company Company { get; set; }      

    }
}
