using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    //Actividad Económica (Clase) - SUNAT
    public class EconomicActivity : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public Guid? EconomicActivityDivisionId { get; set; }
        public EconomicActivityDivision EconomicActivityDivision { get; set; }
    }
}
