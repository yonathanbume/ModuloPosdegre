using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.HelpDesk.Template
{
    //Type chart
    public class TypeChartItem
    {
        public int Key { get; set; }
        public int Count { get; set; }
        public List<TypeChartChildItem> Childs { get; set; }
    }
    public class TypeChartChildItem
    {
        public int parentKey { get; set; }
        public int Key { get; set; }
        public int Count { get; set; }
    }
    public class TypeChartTemplate
    {
        public List<TypeChartItem> Parents { get; set; }
    }

    //techs chart
    public class TechsChartItem
    {
        public string Key { get; set; }
        public int Count { get; set; }
    }
    public class IncidentTypeItem
    {
        public int IncidentTypeId { get; set; }
        public Guid IncidentId { get; set; }
        public int Qty { get; set; }
    }
    public class IncidentTypesByTechs
    {
        public List<IncidentTypeItem> incidentTypes { get; set; }
        public string techId { get; set; }
    }
    public class TechsChartTemplate
    {
        public List<TechsChartItem> Techs { get; set; }
        public List<IncidentTypesByTechs> IncidentsTypeListItem { get; set; }
    }

    //frecuencia mensual
    public class MonthlyChartItem
    {
        public int Year { get; set; }
        public int Key { get; set; }
        public int Count { get; set; }
        public byte Status { get; set; }
    }
    public class MonthlyChartTemplate
    {
        public List<MonthlyChartItem> list { get; set; }
    }
}
