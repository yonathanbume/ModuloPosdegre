using System;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.User
{
    public class TeacherStudent
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public Guid CareerId { get; set; }
        public string CareerName { get; set; }
        public string UserName { get; set; }
        public bool Type { get; set; }
    }
}
