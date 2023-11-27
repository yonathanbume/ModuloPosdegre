using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class GradeReport : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? ConceptId { get; set; }
        public Guid? ProcedureId { get; set; }
        public Guid StudentId { get; set; }
        
        public string BachelorOrigin { get; set; } // PROC_BACH
        public decimal Credits { get; set; } // NUM_CRED
        public DateTime Date { get; set; }
        public int GradeType { get; set; } // ABRE_GYT (B: 1, T: 2)
        public DateTime? GraduationDate { get; set; } // EGRES_FEC
        public string Number { get; set; } // DIPL_NUM
        public string Observation { get; set; } // RESO_NUM
        public string OriginDegreeCountry { get; set; } // PROC_REV_PAIS
        public string PedagogicalTitleOrigin { get; set; } // PROC_TITULO_PED
        public decimal PromotionGrade { get; set; }
        public string ResearchWork { get; set; } // TRAB_INV
        public string ResearchWorkURL { get; set; } // REG_METADATO
        public int SemesterStudied { get; set; }
        public int Status { get; set; }
        public string StudyModality { get; set; } // MOD_EST
        public int Year { get; set; }
        public int YearsStudied { get; set; }

        public string DegreeTitleCommision { get; set; } //comisión de grados y títulos
        public string CommisionInform { get; set; } // Informe de la Comisión.
        public DateTime? CommisionInformDate { get; set; } // Fecha de Informe de la comisión.

        public string GraduatedResolution { get; set; } //Resolución de egreso
        public DateTime? GraduatedResolutionDate { get; set; } //Fecha de Resolución
        public string GraduatedResolutionPath { get; set; } //Resolución en digital

        public string GraduatedCertificatePath { get; set; } //Constancia de Egreso en Digital

        public Concept Concept { get; set; }
        public Procedure Procedure { get; set; }
        public Student Student { get; set; }

        public ICollection<GradeReportRequirement> GradeReportRequirements { get; set; }
    }
}
