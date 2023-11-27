using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Moodle
{
    public class MoodleErrorResponse
    {
        public string Exception { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
