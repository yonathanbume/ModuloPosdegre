
using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.INTRANET.ViewModels.ForumViewModels
{
    public class CategoryViewModel
    {
        public Guid GategoryID { get; set; }
        public List<Topic> Topics { get; set; }
    }
}
