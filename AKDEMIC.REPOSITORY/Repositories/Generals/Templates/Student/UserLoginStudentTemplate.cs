using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class UserLoginStudentTemplate
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Career { get; set; }
        public DateTime FirstLogin { get; set; }
        public string FirstLoginStr { get; set; }
        public DateTime LastLogin { get; set; }
        public string LastLoginStr { get; set; }
    }
}
