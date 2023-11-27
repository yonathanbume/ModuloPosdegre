namespace AKDEMIC.CORE.Structs
{
    public class ChartJsStructs
    {
        public struct Line
        {
            public string[] BackgroundColor { get; set; }
            public string[] BorderColor { get; set; }
            public LineData[] Data { get; set; }
        }

        public struct LineData
        {
            public object X { get; set; }
            public object Y { get; set; }
        }

        public struct Doughtnut
        {
            public string[] BackgroundColor { get; set; }
            public string[] BorderColor { get; set; }
            public int[] Data { get; set; }
        }
    }
}
