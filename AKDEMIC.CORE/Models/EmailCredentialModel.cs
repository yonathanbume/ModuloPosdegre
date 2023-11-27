using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Models
{
    public class EmailCredentialModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpHost { get; set; }
    }
}
