using System;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Career
{
    public class ModelATemplate
    {
        public Guid Id { get;  set; }
        public string Name { get;  set; }

        public string AcademicCoordinator { get;  set; }
        public string AcademicCoordinatorId { get;  set; }

        public string AcademicSecretary { get;  set; }
        public string AcademicSecretaryId { get;  set; }

        public string CareerDirector { get;  set; }
        public string CareerDirectorId { get;  set; }

        public string AcademicDepartmentDirector { get;  set; }
        public string AcademicDepartmentDirectorId { get;  set; }
    }
    public class HisotricTemplate
    {
        public Guid Id { get; set; }
        public string Date { get; set; }

        public string AcademicCoordinator { get; set; }
        public string AcademicCoordinatorId { get; set; }

        public string AcademicSecretary { get; set; }
        public string AcademicSecretaryId { get; set; }

        public string CareerDirector { get; set; }
        public string CareerDirectorId { get; set; }

        public string AcademicDepartmentDirector { get; set; }
        public string AcademicDepartmentDirectorId { get; set; }
    }
}
