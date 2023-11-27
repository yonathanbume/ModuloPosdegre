using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.VisaNet.Templates
{
    public class SessionKeyResponseTemplate
    {
        public string SessionKey { get; set; }
        public string ExpirationTime { get; set; }
        public string MainKey { get; set; }
    }
}
