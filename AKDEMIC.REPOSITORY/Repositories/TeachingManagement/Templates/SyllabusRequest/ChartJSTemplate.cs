using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.SyllabusRequest
{
    public class ChartJSTemplate
    {
        public string[] Categories { get; set; }
        public Guid[] CategoriesId { get; set; }
        public List<DataTemplate> Data { get; set; }
    }

    public class DataTemplate
    {
        public string Name { get; set; }
        public int[] Data { get; set; }
    }
}
