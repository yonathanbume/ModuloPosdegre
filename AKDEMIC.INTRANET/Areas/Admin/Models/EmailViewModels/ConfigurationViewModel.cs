using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.EmailViewModels
{
    public class ConfigurationViewModel
    {
        public string Email_Sender { get; set; }
        public string Email_Password { get; set; }
        public string Email_Port { get; set; }
        public string Email_Host { get; set; }
        public bool Multiple_Email { get; set; }
    }
}
