using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class ReasonViewModel
    {
        public string Reason { get; set; }
        public FileVM File { get; set; }
    }
    public class FileVM
    {
        public IFormFile File { get; set; }
    }
}
