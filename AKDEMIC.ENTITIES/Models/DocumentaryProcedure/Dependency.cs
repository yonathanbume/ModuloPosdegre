using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.ComputersManagement;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.VirtualDirectory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class Dependency : Entity, ITimestamp, ISoftDelete
    {
        public Guid Id { get; set; }
        public Guid? SuperiorDependencyId { get; set; }
        public Guid? FacultyId { get; set; }
        public Guid? CareerId { get; set; }

        public string UserId { get; set; }
        public bool IsActive { get; set; }

        [Required]
        [StringLength(50)]
        public string Acronym { get; set; }

        [StringLength(255)]
        public string Annex { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [StringLength(255)]
        public string Signature { get; set; }

        //Actos resolutivos
        public bool EnabledResolutiveActs { get; set; }
        public string LegalSecretaryId { get; set; }
        public string ResolutiveActsDirectorId { get; set; }
        public string ResolutiveActsValidatorId { get; set; }
        public ApplicationUser LegalSecretary { get; set; }
        public ApplicationUser ResolutiveActsDirector { get; set; }
        public ApplicationUser ResolutiveActsValidator { get; set; }

        [NotMapped]
        public bool IsProcedureDependency { get; set; }

        [NotMapped]
        public string DependencyPattern { get; set; }

        [NotMapped]
        public string AreaManager { get; set; }
        [NotMapped]
        public string Secretary { get; set; }

        public ApplicationUser User { get; set; }
        public Dependency SuperiorDependency { get; set; }
        public Faculty Faculty { get; set; }
        public Career Career { get; set; }

        public ICollection<BudgetFramework> BudgetFrameworks { get; set; }
        public ICollection<Computer> Computers { get; set; }
        public ICollection<Equipment> Equipments { get; set; }
        public ICollection<Concept> Concepts { get; set; }
        public ICollection<Dependency> Dependencies { get; set; }
        public ICollection<ExpenditureProvision> ExpenditureProvisions { get; set; }
        public ICollection<Expense> Expenses { get; set; }
        public ICollection<ExpenseOutput> ExpenseOutputs { get; set; }
        public ICollection<ExternalProcedure> ExternalProcedures { get; set; }
        public ICollection<Income> Incomes { get; set; }
        public ICollection<StructureForExpense> StructureForExpenses { get; set; }
        public ICollection<StructureForIncome> StructureForIncomes { get; set; }
        public ICollection<UserDependency> UserDependencies { get; set; }
        public ICollection<UserExternalProcedure> UserExternalProcedures { get; set; }
        public ICollection<UserInternalProcedure> UserInternalProcedures { get; set; }
        public ICollection<UserProcedure> UserProcedures { get; set; }
        public ICollection<DirectoryDependency> DirectoryDependencies { get; set; }
    }
}
