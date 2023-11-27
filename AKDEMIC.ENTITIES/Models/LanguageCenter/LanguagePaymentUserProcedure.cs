using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;

namespace AKDEMIC.ENTITIES.Models.LanguageCenter
{
    public class LanguagePaymentUserProcedure
    {
        public Guid Id { get; set; }
        public Guid LanguagePaymentId { get; set; }
        public Guid UserProcedureId { get; set; }
        public UserProcedure UserProcedure { get; set; }
        public LanguagePayment LanguagePayment { get; set; }
    }
}
