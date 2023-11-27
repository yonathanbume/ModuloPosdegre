using System;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Degree
{
    public class RegistryPattern : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? ConceptId { get; set; }
        public Guid? ForeignUniversityOriginId { get; set; }
        public Guid? ProcedureId { get; set; }
        public Guid StudentId { get; set; }

        public string AcademicDegree { get; set; } // GRAD_TITU
        public string AcademicDegreeDenomination { get; set; } // DEN_GRADO
        public string AcademicProgram { get; set; }
        public string AcademicResponsible { get; set; } // CARGO3
        public string AcademicResponsibleFullName { get; set; } // AUTORIDAD3
        public string BachelorOrigin { get; set; } // PROC_BACH
        public string BookCode { get; set; } // REG_LIBRO
        public string BookTitle { get; set; }
        public string BussinesSocialReason { get; set; } // RAZ_SOC
        public string Correlative { get; set; }
        public DateTime? FacultyCouncilDate { get; set; }
        public DateTime? UniversityCouncilDate { get; set; }
        public decimal Credits { get; set; } // NUM_CRED
        public DateTime? DateEnrollmentProgram { get; set; } // FEC_MAT_PROG
        public string DegreeProgram { get; set; } // PROG_ESTU
        public DateTime? DuplicateDiplomatDate { get; set; } // DIPL_FEC_DUP
        public DateTime? OriginDiplomatDate { get; set; } // DIPL_FEC_ORG
        public string DiplomatNumber { get; set; } // DIPL_NUM
        public byte DocumentType { get; set; }
        public string EmissionTypeOfDiplomat { get; set; } // DIPL_TIP_EMI
        public DateTime? EndDateEnrollmentProgram { get; set; } // FEC_FIN_PROG
        public DateTime? StartDateEnrollmentProgram { get; set; } // FEC_INICIO_PROG
        public string FolioCode { get; set; } // REG_FOLIO
        public string GeneralSecretary { get; set; } // CARGO2
        public string GeneralSecretaryFullName { get; set; } // AUTORIDAD2
        public string GradDenomination { get; set; } // PROC_REV_GRADO
        public string GradeAbbreviation { get; set; } // ABRE_GYT
        public int GradeType { get; set; } // ABRE_GYT (B: 1, T: 2)
        public DateTime? GraduationDate { get; set; } // EGRES_FEC
        public string Initial { get; set; }
        public string ManagingDirector { get; set; } // CARGO1
        public string ManagingDirectorFullName { get; set; } // AUTORIDAD1
        public string ObtainingDegreeModality { get; set; } // MOD_OBT
        public string OfficeNumber { get; set; } // REG_OFICIO
        public string OriginDegreeCountry { get; set; } // PROC_REV_PAIS
        public string OriginDegreeUniversity { get; set; } // PROC_REV_UNIV
        public string OriginPreRequisiteDegreeCountry { get; set; } // PROC_PAIS_EXT
        public string OriginPreRequisiteDegreeDenomination { get; set; } // PROC_GRADO_EXT
        public string PedagogicalTitleOrigin { get; set; } // PROC_TITULO_PED
        public string PostGraduateSchool { get; set; } // ESC_POST
        public string RegistryNumber { get; set; } // REG_REGISTRO
        public string Relation { get; set; } // RELACION
        public string ResearchWork { get; set; } // TRAB_INV
        public string ResearchWorkURL { get; set; } // REG_METADATO


        public string ResolutionNumber { get; set; } // RESO_NUM  -- Resolución Rectoral o de Consejo Universitario de Bachiller
        public DateTime? ResolutionDateByUniversityCouncil { get; set; } // RESO_FEC -- Fecha de Resolución  Rectoral o de Consejo Universitario de Bachiller
        public string ResolutionByUniversityCouncilPath { get; set; } // Resolución Rectoral o de Consejo Universitario de Bachiller  en digital 

        public string ResolutionNumberByFacultyCouncil { get; set; } // Resolución Decanal o Consejo de Facultad de Bachiller
        public DateTime? ResolutionDateByFacultyCouncil { get; set; } // Fecha de Resolución  Decanal o de Consejo de Facultad de Bachiller
        public string ResolutionByFacultyCouncilPath { get; set; } // Resolución Decanal o de Consejo de Facultad de Bachiller  en digital 

        public string DegreeGradeFilePath { get; set; } // Grado Bachiller en Digital  or Título Profesional en digital

        public int SustainabilityMode { get; set; } //MOD_SUSTENTACIÓN(nuevo) -- Modalidad de Titulación
        public DateTime? DegreeSustentationDate { get; set; } //FEC_TRAB_GRAD_TIT(nuevo) -- Fecha de Sustentación de Titulación

        public string InvestigationLine { get; set; } // Línea de Investigación
        public string ApplicationSector { get; set; } // Sector de aplicación
        public string RecordSustentationFilePath { get; set; } // Acta de Sustentación digital



        public string Specialty { get; set; } // SEG_ESP
        public string SpecialtyMention { get; set; } // ESPECIALIDAD O MENCION
        public int Status { get; set; }
        public int DiplomaStatus { get; set; }
        public string StudyModality { get; set; } // MOD_EST
        public string UniversityCode { get; set; } // CODUNIV
        public string UniversityCouncilType { get; set; }
        public string Year { get; set; }
        public bool ReadyToSunedu { get; set; } = false;

        public bool SineaceAcreditation { get; set; } //PROG_ACREDIT (nuevo) ||
        public DateTime? SineaceAcreditationStartDate { get; set; } //FEC_INICIO_ACREDIT (nuevo) ||
        public DateTime? SineaceAcreditationEndDate { get; set; } //FEC_FIN_ACREDIT (nuevo) ||

        public DateTime? SineaceAcreditationDegreeModalityStartDate { get; set; } //FEC_INICIO_MOD_TIT_ACREDIT(nuevo) ||
        public DateTime? SineaceAcreditationDegreeModalityEndDate { get; set; } //FEC_FIN_MOD_TIT_ACREDIT(nuevo) ||

        public DateTime? ProcessDegreeDate  { get; set; } //FEC_SOLICIT_GRAD_TIT(nuevo)

        public bool IsOriginal { get; set; } //TRAB_INVEST_ORIGINAL(nuevo) ||

        public bool ComplianceRevalidationCriteria { get; set; } //CRIT_REVL(nuevo) ||

        public Concept Concept { get; set; }
        public ForeignUniversityOrigin ForeignUniversityOrigin { get; set; }
        public Procedure Procedure { get; set; }
        public Student Student { get; set; }
    }
}
