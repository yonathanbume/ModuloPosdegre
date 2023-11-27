using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.ContinuingEducation
{
    public class RegisterSection : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid SectionId { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public bool IsInternal { get; set; }
        public bool IsValid { get; set; } = false;
        public DateTime Date { get; set; }
        public string Age { get; set; }
        public string Address { get; set; }
        public string StudyLevel { get; set; }
        public ApplicationUser User { get; set; }
        public Section Section { get; set; }
    }
}
