using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Overrides;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.ViewModels.UserProcedureViewModels
{
    public class UserProcedureDetailViewModel
    {
        public Guid Id { get; set; }
        public Guid? DependencyId { get; set; }
        public Guid? TermId { get; set; }
        public Guid ProcedureId { get; set; }
        public Guid? PaymentId { get; set; }
        public string UserId { get; set; }
        public string UrlImage { get; set; }
        public bool HasReceipt { get; set; }
        public bool HasPicture { get; set; }
        [MaxLength(8)]
        [RegularExpression("[^0-9]")]
        public string DNI { get; set; }
        public int GeneratedId { get; set; }
        [Required]
        public int Status { get; set; } = 1;
        public string Comment { get; set; }
        public string Observation { get; set; }
        public ProcedureViewModel ProcedureViewModel { get; set; }
        public UserViewModel UserViewModel { get; set; }
        public DependencyDetailViewModel DependencyViewModel { get; set; }
        public PaymentViewModel PaymentViewModel { get; set; }
        public UserProcedureDerivationViewModel UserProcedureDerivationViewModel { get; set; }
    }

    public class UserViewModel
    {
        public string Id { get; set; }

        [NotMapped]
        public string FullName { get; set; }
        [NotMapped]
        public string FullNameCode { get; set; }
        [NotMapped]
        public int Type { get; set; }
        [NotMapped]
        public string Phone { get; set; }
        [NotMapped]
        public string Email { get; set; }
        [NotMapped]
        public string Dni { get; set; }
        [NotMapped]
        public string DepartmentAcademic { get; set; }
        [NotMapped]
        public string Faculty { get; set; }
        [NotMapped]
        public string Career { get; set; }
        public string Username { get; set; }
        [NotMapped]
        public string Dependency { get; set; }
    }

    public class ProcedureViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Code { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Duration { get; set; }

        [Required]
        public int Score { get; set; }
        public string Accounting { get; set; }
        public string LegalBase { get; set; }
    }
    public class DependencyDetailViewModel
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }
    }
    public class PaymentViewModel
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public Guid? InvoiceId { get; set; }
        public string UserId { get; set; }
        public string OperationCodeB { get; set; }
        public string PayDateTime { get; set; }
        public string Description { get; set; }
        public decimal Discount { get; set; } = 0.00M;
        public decimal IgvAmount { get; set; } = 0.00M;
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public decimal LateCharge { get; set; } = 0.00M;
        public decimal Quantity { get; set; } = 1;
        public byte Status { get; set; } = ConstantHelpers.PAYMENT.STATUS.PENDING;
        public decimal SubTotal { get; set; } = 0.00M;
        public decimal Total { get; set; } = 0.00M;
        public byte Type { get; set; } = 0;
    }

    public class UserProcedureDerivationViewModel
    {
        public Guid Id { get; set; }
        public Guid DependencyId { get; set; }
        public Guid ActivityProcedureId { get; set; }
        public string UserDependencyId { get; set; }
        public Guid UserProcedureId { get; set; }

        public string Observation { get; set; }
        public int Status { get; set; }
        public UserProcedureViewModel UserProcedureViewModel { get; set; }

        [DataType(DataType.Upload)]
        [Extensions(ConstantHelpers.DOCUMENTS.FILE_EXTENSION_GROUP.DOCUMENTS)]
        public IEnumerable<IFormFile> DocumentFiles { get; set; }
    }
}
