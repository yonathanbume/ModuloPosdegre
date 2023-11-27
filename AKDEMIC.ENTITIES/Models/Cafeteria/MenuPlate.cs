using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Cafeteria
{
    public class MenuPlate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }        

        public ICollection<MenuPlateSupply> menuPlateSupplies { get; set; }
       
    }
}
