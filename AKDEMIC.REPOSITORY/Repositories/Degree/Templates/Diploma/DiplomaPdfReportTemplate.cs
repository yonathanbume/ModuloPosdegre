namespace AKDEMIC.REPOSITORY.Repositories.Degree.Templates.Diploma
{
    public class DiplomaPdfReportTemplate
    {
        public string Path { get; set; }
        public string NationalEmblem { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string AcademicProgram { get; set; }
        public string StudentName { get; set; }
        public byte Type { get; set; }
        public string StudentPaternalSurName { get; set; }
        public string StudentMaternalSurName { get; set; }
        public string FormatedDate { get; set; }
        public string AcademicDegreeDenomination { get; set; }

        //Decano, rector, secretario General
        public string Dean { get; set; }
        public string Rector { get; set; }
        public string GeneralSecretary { get; set; }

        //Tabla1
        public string Book { get; set; }
        public string Foil { get; set; }
        public string RegisterNumber { get; set; }
        public string DiplomaNumber { get; set; }

        //Tabla2
        public string UniversityCode{ get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string GradeAbrev { get; set; }
        public string GradeModalityGain { get; set; }
        public string StudentModality { get; set; }
        public string UniversitaryDate { get; set; }
        public string UniveristaryDateDay { get; set; }
        public string UniveristaryDateMonth { get; set; }
        public string UniveristaryDateYear { get; set; }
        public string ResolutionNumber { get; set; }
        public string ResoultionRectorDate { get; set; }
        public string EmittedDiplomaType { get; set; }

        //QRCode
        public string QRCode { get; set; }

        //GRADO ACADEMICO
        public string GradeTypeDescription { get; set; }

        public string CurrentDay { get; set; }
        public string CurrentMonth { get; set; }
        public string CurrentYear { get; set; }

        public string Shield { get; set; }
        public bool OriginDiplomatDateBoolean { get; set; }
        public bool IsAccredited { get; set; }

        public string DuplicatedDiplomaDate { get; set; }
        public string OriginalDiplomaDate { get; set; }


        public string UniversityCouncilDate { get; set; }
    }
}
