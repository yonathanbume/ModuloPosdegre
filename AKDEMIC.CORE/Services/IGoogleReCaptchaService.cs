using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services
{
    public interface IGoogleReCaptchaService
    {
        Task<bool> VerifiyToken(string token);
    }
}
