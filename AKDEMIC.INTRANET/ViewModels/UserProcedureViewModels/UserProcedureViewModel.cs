using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Overrides;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.ViewModels.UserProcedureViewModels
{
    public class UserProcedureViewModel
    {
        public Guid Id { get; set; }
        public Guid? ConceptId { get; set; }
        public Guid ProcedureId { get; set; }
        public string UserId { get; set; }
        public bool HasPicture { get; set; }
        public bool HasReceipt { get; set; }
        public string NameProcedure { get; set; }
        [Required]
        public int Status { get; set; } = 1;
        public string Comment { get; set; } = "";
        public decimal Cost { get; set; } = 0;
        public decimal CostReq { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public decimal Paid { get; set; } = 0;
        public int Duration { get; set; }
        public PaymentReceiptViewModel PaymentReceipt { get; set; }

        public string StartDependency { get; set; }

        [DataType(DataType.Upload)]
        [Extensions(ConstantHelpers.DOCUMENTS.FILE_EXTENSION_GROUP.IMAGES, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.FILE_EXTENSIONS)]
        public IFormFile Image { get; set; }
        public string urlCropImg { get; set; }

        [DataType(DataType.Upload)]
        [Extensions(ConstantHelpers.DOCUMENTS.FILE_EXTENSION_GROUP.DOCUMENTS)]
        public List<IFormFile> DocumentFiles { get; set; }
        public List<DependencyViewModel> Dependencies {get; set;}
        public List<ProcedureRequirementViewModel> ProcedureRequirement { get; set; }

        [MaxLength(8)]
        [RegularExpression("[^0-9]")]
        public string DNI { get; set; }

        public UITViewModel UITViewModel { get; set; }

        public Dictionary<int, string> UserProcedureStatusValues { get; set; }
    }

    public class ProcedureRequirementViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Cost { get; set; }
        public bool HasUserProcedureRecordRequirement { get; set; }
    }

    public class DependencyViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class PaymentReceiptViewModel
    {
        public string Datetime { get; set; }

        public string Sequence { get; set; }

        public decimal Amount { get; set; }
    }
}
