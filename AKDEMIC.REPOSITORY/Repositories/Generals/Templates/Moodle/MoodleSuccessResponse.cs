using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Moodle
{
    public class MoodleSuccessResponse
    {
        public List<Warning> warnings { get; set; }
    }

    public class Warning
    {
        public string warning { get; set; }
    }
}
