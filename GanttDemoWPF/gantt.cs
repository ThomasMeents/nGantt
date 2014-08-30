using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using nGantt.GanttChart;
using nGantt.PeriodSplitter;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace GanttDemoWPF
{
    public class gantt
    {
        readonly CultureInfo culture = new CultureInfo("de-DE");
        private readonly ObservableCollection<ContextMenuItem> ganttTaskContextMenuItems = new ObservableCollection<ContextMenuItem>();

        public void ShowGantt(Data data)
        {
            GanttChart ganttWindow = new GanttChart(data)
            {
                Height = 600,
                Width = 800,
                Owner = Application.Current.MainWindow,
                Title = "Gantt-Chart",
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            DateTime minDate = data.ListOfChartTimeSpans.Min(c => c.from);
            minDate = minDate.AddDays(-minDate.Day);
            CreateData(ganttWindow, data, minDate, minDate.AddMonths(2));

            ganttTaskContextMenuItems.Add(new ContextMenuItem(ViewClicked, "Information"));
            ganttWindow.ganttControl.GanttTaskContextMenuItems = ganttTaskContextMenuItems;

            ganttWindow.ShowDialog();
        }

        private void ViewClicked(GanttTask ganttTask)
        {
            string message = "Start: " + ganttTask.Start + "\nEnd: " + ganttTask.End + "\n\n" + ganttTask.Name;
            MessageBox.Show(message);
        }

        public void CreateData(GanttChart ganttWindow, Data data, DateTime minDate, DateTime maxDate)
        {
            // Set max and min dates
            ganttWindow.ganttControl.Initialize(minDate, maxDate);

            // Create timelines and define how they should be presented
            ganttWindow.ganttControl.CreateTimeLine(new PeriodYearSplitter(minDate, maxDate), FormatYear);
            ganttWindow.ganttControl.CreateTimeLine(new PeriodMonthSplitter(minDate, maxDate), FormatMonth);
            var gridLineTimeLine = ganttWindow.ganttControl.CreateTimeLine(new PeriodDaySplitter(minDate, maxDate), FormatDay);
            ganttWindow.ganttControl.CreateTimeLine(new PeriodDaySplitter(minDate, maxDate), FormatDayName);

            // Set the timeline to attach gridlines to
            ganttWindow.ganttControl.SetGridLinesTimeline(gridLineTimeLine, DetermineBackground);

            HeaderedGanttRowGroup rowgroupprojectphases = ganttWindow.ganttControl.CreateGanttRowGroup("Example-Heading");

            foreach (ChartTimeSpan chartTimeSpan in data.ListOfChartTimeSpans)
            {
                GanttRow row = ganttWindow.ganttControl.CreateGanttRow(rowgroupprojectphases, chartTimeSpan.name);
                List<ChartTimeSpan> sorted = data.ListOfChartTimeSpans.FindAll(p => p.name.Equals(chartTimeSpan.name));

                foreach (ChartTimeSpan sortedchartTimeSpan in sorted)
                    ganttWindow.ganttControl.AddGanttTask(row, new GanttTask
                    {
                        Start = sortedchartTimeSpan.from,
                        End = sortedchartTimeSpan.to,
                        Name = sortedchartTimeSpan.name,
                        Color = sortedchartTimeSpan.selected ? Colors.OrangeRed : Colors.DodgerBlue,
                        Radius = (sortedchartTimeSpan.to - sortedchartTimeSpan.from).TotalDays < 3 ? 0 : 5
                    });
            }
        }

        private Brush DetermineBackground(TimeLineItem timeLineItem)
        {
            if (timeLineItem.End.Date.DayOfWeek == DayOfWeek.Saturday || timeLineItem.End.Date.DayOfWeek == DayOfWeek.Sunday)
                return new SolidColorBrush(Colors.LightBlue);
            return new SolidColorBrush(Colors.Transparent);
        }

        private string FormatYear(Period period)
        {
            return period.Start.Year.ToString();
        }

        private string FormatMonth(Period period)
        {
            return period.Start.Month.ToString();
        }

        private string FormatDay(Period period)
        {
            return period.Start.Day.ToString();
        }

        private string FormatDayName(Period period)
        {
            string returns = culture.DateTimeFormat.DayNames[(int)period.Start.DayOfWeek];
            return returns.Substring(0, 2);
        }
    }
}
