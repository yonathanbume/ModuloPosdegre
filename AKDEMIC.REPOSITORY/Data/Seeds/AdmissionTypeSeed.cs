using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Admission;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class AdmissionTypeSeed
    {
        public static AdmissionType[] Seed(AkdemicContext context)
        {
            var result = new List<AdmissionType>()
            {
                new AdmissionType { ResolutionDate = DateTime.UtcNow, ValidityStart = DateTime.UtcNow.AddYears(-1), ValidityEnd = DateTime.UtcNow.AddYears(1), Abbreviation = "R",  Resolution = "N° 016-2018", Name = "Regular", /*IsRegular = true,*/ IsTransfer = false, /*Cost = 150.0M*/},
                new AdmissionType { ResolutionDate = DateTime.UtcNow, ValidityStart = DateTime.UtcNow.AddYears(-1), ValidityEnd = DateTime.UtcNow.AddYears(1), Abbreviation = "TI", Resolution = "N° 016-2018", Name = "Traslado Interno", /*IsRegular = false,*/ IsTransfer = true, /*Cost = 50.0M*/ },
                new AdmissionType { ResolutionDate = DateTime.UtcNow, ValidityStart = DateTime.UtcNow.AddYears(-1), ValidityEnd = DateTime.UtcNow.AddYears(1), Abbreviation = "TE", Resolution = "N° 016-2018", Name = "Traslado Externo", /*IsRegular = false,*/ IsTransfer = true, /*Cost = 100.0M */},
                new AdmissionType { ResolutionDate = DateTime.UtcNow, ValidityStart = DateTime.UtcNow.AddYears(-1), ValidityEnd = DateTime.UtcNow.AddYears(1), Abbreviation = "CE", Resolution = "N° 016-2018", Name = "CEPU", /*IsRegular = false,*/ IsTransfer = false, /*Cost = 0*/ },
                new AdmissionType { ResolutionDate = DateTime.UtcNow, ValidityStart = DateTime.UtcNow.AddYears(-1), ValidityEnd = DateTime.UtcNow.AddYears(1), Abbreviation = "PR", Resolution = "N° 016-2018", Name = "Profesionales", /*IsRegular = false,*/ IsTransfer = false, /*Cost = 100.0M*/ },
                new AdmissionType { ResolutionDate = DateTime.UtcNow, ValidityStart = DateTime.UtcNow.AddYears(-1), ValidityEnd = DateTime.UtcNow.AddYears(1), Abbreviation = "1P", Resolution = "N° 016-2018", Name = "1eros Puestos", /*IsRegular = false,*/ IsTransfer = false, /*Cost = 0*/ },
                new AdmissionType { ResolutionDate = DateTime.UtcNow, ValidityStart = DateTime.UtcNow.AddYears(-1), ValidityEnd = DateTime.UtcNow.AddYears(1), Abbreviation = "DC", Resolution = "N° 016-2018", Name = "Deportistas Calificados", /*IsRegular = false,*/ IsTransfer = false, /*Cost = 0*/ },
                new AdmissionType { ResolutionDate = DateTime.UtcNow, ValidityStart = DateTime.UtcNow.AddYears(-1), ValidityEnd = DateTime.UtcNow.AddYears(1), Abbreviation = "VT", Resolution = "N° 016-2018", Name = "Victima del Terrorismo", /*IsRegular = false, */IsTransfer = false, /*Cost = 0*/ },
                new AdmissionType { ResolutionDate = DateTime.UtcNow, ValidityStart = DateTime.UtcNow.AddYears(-1), ValidityEnd = DateTime.UtcNow.AddYears(1), Abbreviation = "HT", Resolution = "N° 016-2018", Name = "Hijo Trabajador", /*IsRegular = false,*/ IsTransfer = false, /*Cost = 0*/ },
                new AdmissionType { ResolutionDate = DateTime.UtcNow, ValidityStart = DateTime.UtcNow.AddYears(-1), ValidityEnd = DateTime.UtcNow.AddYears(1), Abbreviation = "DI", Resolution = "N° 016-2018", Name = "Discapacitados", /*IsRegular = false, */IsTransfer = false, /*Cost = 0*/ }
            };

            return result.ToArray();
        }
    }
}
