using System;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class ChannelContact
    {
        public Guid Id { get; set; }
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }
        public String Description { get; set; }
        public byte Channel { get; set; }
    }
}
