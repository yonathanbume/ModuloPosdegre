using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class TeacherPortfolio : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string File { get; set; }
        public byte Folder { get; set; }
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
