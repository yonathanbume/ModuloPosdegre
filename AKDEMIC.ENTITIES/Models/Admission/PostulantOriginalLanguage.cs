using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class PostulantOriginalLanguage
    {
        public Guid Id { get; set; }
        public Guid PostulantId { get; set; }
        public int Language   { get; set; }
        public int Level { get; set; }
        public Postulant Postulant { get; set; }
    }
}
