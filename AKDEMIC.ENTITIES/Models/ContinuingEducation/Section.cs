using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.ContinuingEducation
{
    public class Section : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime InscriptionStart { get; set; }
        public DateTime InscriptionEnd { get; set; }
        public DateTime ClassStart { get; set; }
        public DateTime ClassEnd { get; set; }
        public int Vacancies { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public ICollection<RegisterSection> RegisterSections { get; set; }
    }
}
