using System;

namespace GanttDemoWPF
{
    public class ChartTimeSpan
    {
        public bool selected { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public string name { get; set; }
    }
}