using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services
{
    public interface ISmsService
    {
        Task<bool> SendSMS(string message, params string[] phoneNumber);
    }
}
