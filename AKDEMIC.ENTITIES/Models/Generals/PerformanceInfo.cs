using System;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class PerformanceInfo
    {
        public string Name { get; set; }
        public TimeSpan StopwatchEllapsed { get; set; }
        public long StopwatchEllapsedMilliseconds { get; set; }
        public long StopwatchEllapsedTicks { get; set; }
        public long Score { get; set; }
    }
}
