using Org.BouncyCastle.Asn1.Sec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Options
{
    public class ReCaptchaCredentials
    {
        public string SiteKey { get; set; }
        public string SecretKey { get; set; }
    }
}
