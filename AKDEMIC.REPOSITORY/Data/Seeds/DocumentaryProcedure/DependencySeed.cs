using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds.DocumentaryProcedure
{
    public class DependencySeed
    {
        public static Dependency[] Seed(AkdemicContext apiContext)
        {
            var result = new List<Dependency>()
            {
                new Dependency { Acronym = "BIB", Name = "Biblioteca" , PhoneNumber = "953248010" , Email = "biblioteca@enchufate.pe", RelationId = "BIB" },
                new Dependency { Acronym = "CEN", Name = "Centro Médico" , PhoneNumber = "910248010" , Email = "centromedico@enchufate.pe", RelationId = "CEN" },
                new Dependency { Acronym = "D001", Name = "Dependencia" , PhoneNumber = "912152145" , Email = "dependencia@enchufate.pe", RelationId = "D001" },
                new Dependency { Acronym = "HIS", Name = "Histórico" , PhoneNumber = "995541655" , Email = "historico@enchufate.pe", RelationId = "HIS" },
                new Dependency { Acronym = "MES", Name = "Mesa de Partes" , PhoneNumber = "902248010" , Email = "mesapartes@enchufate.pe", RelationId = "MES" },
                new Dependency { Acronym = "OFI", Name = "Oficina" , PhoneNumber = "995248019" , Email = "oficina@enchufate.pe", RelationId = "OFI" },
                new Dependency { Acronym = "SEC", Name = "Secretaria General" , PhoneNumber = "953318010" , Email = "secretariageneral@enchufate.pe", RelationId = "SEC" },
                new Dependency { Acronym = "TES", Name = "Tesorería" , PhoneNumber = "953148019" , Email = "tesoreria@enchufate.pe", RelationId = "TES" }
            };

            return result.ToArray();
        }
    }
}
