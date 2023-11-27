using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IExoneratePaymentService
    {
        Task Insert(ExoneratePayment exoneratePayment);
    }
}
