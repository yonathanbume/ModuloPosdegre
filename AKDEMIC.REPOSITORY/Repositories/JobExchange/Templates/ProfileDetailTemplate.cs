using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates
{
    public class ProfileDetailTemplate
    {
        public class AcademicFormationDate
        {
            public string Description { get; set; }
            public byte AcademicLevel { get; set; }
            public string StringAcademicLevel { get; set; }
            public string RangeDate { get; set; }
            public bool IsStudying { get; set; }
        }
        public class DegreeDate
        {
            public string Description { get; set; }
            public string Institution { get; set; }
        }
        public class CertificateDate
        {
            public string Description { get; set; }
            public string Institution { get; set; }
        }
        public class AbilityDate
        {
            public string Ability { get; set; }
            public byte Level { get; set; }
            public string StringLevel { get; set; }
        }
        public class LanguageDate
        {
            public string Language { get; set; }
            public byte Level { get; set; }
            public string StringLevel { get; set; }
        }

        public class ExperienceDate
        {
            public string Description { get; set; }
            public string RangeDate { get; set; }
            public string Position { get; set; }
        }

        public class TemplateDataCV
        {
            public Guid StudentId { get; set; }
            public string FullName { get; set; }
            public string PaternalSurName { get; set; }
            public string MaternalSurName { get; set; }
            public string Birthday { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string Career { get; set; }
            public string Description { get; set; }
            public string CivilStatus { get; set; }
            public string Sex { get; set; }
            public string Image { get; set; }
            public string DNI { get; set; }
            public string LinkedIn { get; set; }
            public List<AcademicFormationDate> LstAcademicFormationDates { get; set; }
            public List<DegreeDate> LstDegreeDates { get; set; }
            public List<CertificateDate> LstCertificateDates { get; set; }
            public List<AbilityDate> LstAbilityDates { get; set; }
            public List<LanguageDate> LstLanguageDates { get; set; }
            public List<ExperienceDate> LstExperienceDates { get; set; }

        }
    }
}
