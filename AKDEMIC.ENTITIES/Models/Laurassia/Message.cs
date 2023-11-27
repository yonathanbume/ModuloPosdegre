using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class Message : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public string UserId { get; set; }
        public string Contenido { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Chat Chat { get; set; }
    }
}
