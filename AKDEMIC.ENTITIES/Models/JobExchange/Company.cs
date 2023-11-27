using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class Company : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid CompanySizeId { get; set; }
        public Guid CompanyTypeId { get; set; }
        public Guid EconomicActivityId { get; set; }
        public Guid? SectorId { get; set; }
        public Guid? AgreementId { get; set; }
        public string UserId { get; set; }       
        public int State { get; set; } = 0; //CORE.Helpers.ConstantHelpers.EXTERNAL_REQUEST_STATES.OBSERVED
        public string Observations { get; set; }

        public string Description { get; set; }
        public DateTime FormationTime { get; set; }
        public bool IsExternalRequest { get; set; } = false;
        public bool IsFromSystem { get; set; } = false; //Creado por el sistema - Superadmin o Encuesta de egresado
        public string Phone { get; set; }

        [StringLength(11)]
        public string RUC { get; set; }
        public string Url { get; set; }

        public ApplicationUser User { get; set; }
        public CompanySize CompanySize { get; set; }
        public CompanyType CompanyType { get; set; }
        public EconomicActivity EconomicActivity { get; set; }
        public Sector Sector { get; set; }
        public Agreement Agreement { get; set; }
        public ICollection<ChannelContact> ChannelContacts { get; set; }
        //public ICollection<Employee> Employees { get; set; }
        public ICollection<JobOffer> JobOffers { get; set; }
        //public ICollection<Office> Offices { get; set; }
        public ICollection<Sede> Sedes { get; set; }
        public ICollection<StudentExperience> StudentExperiences { get; set; }
        public ICollection<ImageCompany> ImageCompanies { get; set; }
    }
}