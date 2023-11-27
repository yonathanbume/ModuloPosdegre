using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.EventViewModels
{
    public class FileViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public byte ToDo { get; set; }
        public IFormFile File { get; set; }
        public string UrlFile { get; set; }
    }
}
