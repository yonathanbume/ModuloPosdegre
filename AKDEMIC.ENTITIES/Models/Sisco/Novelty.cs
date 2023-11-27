using System;

namespace AKDEMIC.ENTITIES.Models.Sisco
{
    public class Novelty
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public string NoveltyDate { get; set; }
        public string UrlImage { get; set; }
        public string UrlVideo { get; set; }
    }
}
