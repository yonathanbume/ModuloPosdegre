namespace AKDEMIC.DEGREE.Areas.Admin.Models.DiplomaViewModels
{
    public class DiplomaPdfViewModel
    {
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string AcademicProgram { get; set; }
        public string StudentName { get; set; }
        public string StudentPaternalSurName { get; set; }
        public string StudentMaternalSurName { get; set; }
        public string FormatedDate { get; set; }

        //Decano, rector, secretario General
        public string Dean { get; set; }
        public string Rector { get; set; }
        public string GeneralSecretary { get; set; }
    }
}
