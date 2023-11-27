using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class JobOffer : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }

        [Required]
        public string Position { get; set; }

        public byte Status { get; set; }

        //public Guid OfficeId { get; set; }

        public string Area { get; set; }

        public byte WorkType { get; set; } //Full time, part time

        public decimal Salary { get; set; }

        public string Requirements { get; set; }

        [Required]
        public string Benefits { get; set; }

        [Required]
        public string Functions { get; set; }

        public byte Type { get; set; } //Practicante Pre, Practicante Pro, Egresado
        
        public byte MinimumFormationRequired { get; set; }

        public int Vacancies { get; set; } = 1;

        public string ClosureReason { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Company Company { get; set; }

        public string Document { get; set; }

        //public Office Office { get; set; }

        //public ICollection<Employee> Employees { get; set; }

        public ICollection<JobOfferLanguage> JobOfferLanguages { get; set; }

        public ICollection<JobOfferAbility> JobOfferAbilities { get; set; }

        public ICollection<JobOfferApplication> JobOfferApplications { get; set; }

        public ICollection<JobOfferCareer> JobOfferCareers { get; set; }
        public string Location { get; set; }
    }
}
