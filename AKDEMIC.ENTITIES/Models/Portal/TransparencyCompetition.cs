using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyCompetition
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; } //Fecha de publicacion ?
        public int Type { get; set; } //Para docentes, jefe de practica ,CAS , Contratos por Suplencia
        public int State { get; set; } //Abierto,Cerrado

        public string ExternalLink { get; set; }

        [NotMapped]
        public string ParsedPublishDate => PublishDate.ToLocalDateFormat();

        [NotMapped]
        public string Announcement => $"{PublishDate.Year}-{ConstantHelpers.Institution.Abbreviations[ConstantHelpers.GENERAL.Institution.Value]} ({PublishDate.ToLocalDateFormat()})";

        public ICollection<TransparencyCompetitionFile> TransparencyCompetitionFiles { get; set; }
    }
}
