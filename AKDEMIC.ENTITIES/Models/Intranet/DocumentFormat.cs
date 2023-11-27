using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class DocumentFormat : Entity, ITimestamp
    {
        [Key]
        public byte Id { get; set; } //El identificador es el tipo de constancia
        public string Title { get; set; }
        public string Content { get; set; }
        public byte Type { get; set; }
    }
}
