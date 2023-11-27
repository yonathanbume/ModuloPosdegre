using System;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Evaluation
{
    public class RegisterCulturalActivity
    {
        public Guid Id { get; set; }
        public Guid CulturalActivityId { get; set; }
        public CulturalActivity CulturalActivity { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Dni { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public bool IsInternal { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime Date { get; set; }
        public string Age { get; set; }
        public string Address { get; set; }
        public string StudyLevel { get; set; }
    }
}
