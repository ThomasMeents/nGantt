using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace GanttDemoWPF
{
    /// <summary>
    /// Interaction logic for Gantt.xaml
    /// </summary>
    public partial class GanttChart
    {
        readonly gantt gantt = new gantt();

        public Data data { get; set; }

        private int DaysToShow { get; set; }
        private string Search { get; set; }

        public GanttChart(Data chartdata)
        {
            InitializeComponent();
            DaysToShow = 30;
            DaysSelect.Text = DaysToShow.ToString();
            data = chartdata;
        }

        private void ButtonMonthBack_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime minDate = ganttControl.GanttData.MinDate.AddMonths(-1);
            DateTime maxDate = ganttControl.GanttData.MaxDate.AddMonths(-1);

            ganttControl.ClearGantt();
            gantt.CreateData(this, data, minDate, maxDate);
        }

        private void ButtonDayBack_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime minDate = ganttControl.GanttData.MinDate.AddDays(-1);
            DateTime maxDate = ganttControl.GanttData.MaxDate.AddDays(-1);

            ganttControl.ClearGantt();
            gantt.CreateData(this, data, minDate, maxDate);
        }

        private void ButtonMonthForth_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime minDate = ganttControl.GanttData.MinDate.AddMonths(1);
            DateTime maxDate = ganttControl.GanttData.MaxDate.AddMonths(1);

            ganttControl.ClearGantt();
            gantt.CreateData(this, data, minDate, maxDate);
        }

        private void ButtonDayForth_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime minDate = ganttControl.GanttData.MinDate.AddDays(1);
            DateTime maxDate = ganttControl.GanttData.MaxDate.AddDays(1);

            ganttControl.ClearGantt();
            gantt.CreateData(this, data, minDate, maxDate);
        }

        private void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime minDate = DatePicker.DisplayDate;
            DateTime maxDate = minDate.AddDays(DaysToShow);

            ganttControl.ClearGantt();
            gantt.CreateData(this, data, minDate, maxDate);
        }

        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            DaysToShow = Convert.ToInt32(DaysSelect.Text);

            DateTime minDate = ganttControl.GanttData.MinDate;
            DateTime maxDate = minDate.AddDays(DaysToShow);

            ganttControl.ClearGantt();
            gantt.CreateData(this, data, minDate, maxDate);
        }

        private void DaysSelect_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DaysToShow = Convert.ToInt32(DaysSelect.Text);
        }

        private void DaysSelect_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"^[0-9]"))
                e.Handled = false;
            else
            {
                e.Handled = true;
                MessageBox.Show("Mistake");
            }
        }

        private void ButtonFirst_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime minDate = data.ListOfChartTimeSpans.Min(c => c.from).Date;
            DateTime maxDate = minDate.AddMonths(2);

            ganttControl.ClearGantt();
            gantt.CreateData(this, data, minDate, maxDate);
        }

        private void ButtonLast_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime maxDate = data.ListOfChartTimeSpans.Max(c => c.to).Date;
            DateTime minDate = maxDate.AddMonths(-2);

            ganttControl.ClearGantt();
            gantt.CreateData(this, data, minDate, maxDate);
        }

        private void SearchTextBox_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Search = DaysSelect.Text;
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            ganttControl.ClearGantt();
            if (SearchTextBox.Text != "")
                foreach (ChartTimeSpan charttimespan in data.ListOfChartTimeSpans)
                    charttimespan.selected = charttimespan.name.Contains(SearchTextBox.Text);
            else if (SearchTextBox.Text == "")
                foreach (ChartTimeSpan charttimespan in data.ListOfChartTimeSpans)
                    charttimespan.selected = false;

            gantt.CreateData(this, data, ganttControl.GanttData.MinDate, ganttControl.GanttData.MaxDate);
        }
    }
}