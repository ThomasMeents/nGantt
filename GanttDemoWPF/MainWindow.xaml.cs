using System;
using System.Collections.Generic;
using System.Windows;

namespace GanttDemoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            gantt gantt = new gantt();
            
            gantt.ShowGantt(Example());
        }


        private Data Example()
        {
            return new Data
            {
                ListOfChartTimeSpans = new List<ChartTimeSpan>
                {
                    new ChartTimeSpan
                    {
                        @from = new DateTime(2014, 08, 01),
                        to = new DateTime(2014, 08, 05),
                        name = "test-a"
                    },
                    new ChartTimeSpan
                    {
                        @from = new DateTime(2014, 08, 10),
                        to = new DateTime(2014, 08, 12),
                        name = "test-b"
                    },
                    new ChartTimeSpan
                    {
                        @from = new DateTime(2014, 08, 13),
                        to = new DateTime(2014, 08, 20),
                        name = "test-c"
                    },
                    new ChartTimeSpan
                    {
                        @from = new DateTime(2014, 09, 01),
                        to = new DateTime(2014, 09, 05),
                        name = "test-d"
                    },
                    new ChartTimeSpan
                    {
                        @from = new DateTime(2014, 09, 04),
                        to = new DateTime(2014, 09, 05),
                        name = "test-e"
                    },
                    new ChartTimeSpan
                    {
                        @from = new DateTime(2014, 09, 13),
                        to = new DateTime(2014, 09, 30),
                        name = "test-f"
                    },
                }
            };
        }
    }
}
