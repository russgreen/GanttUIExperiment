using Syncfusion.Windows.Controls.Gantt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CoreLibrary.Models;
using ui_models = GantUI.Models;

namespace GantUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly GantUI.ViewModels.ProjectPlanningViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new GantUI.ViewModels.ProjectPlanningViewModel(App.projectID);
            this.DataContext = _viewModel;

            this.Gantt.SelectedItems.CollectionChanged += SelectedItems_CollectionChanged;
            Closing += OnClosing;

            //bind to events
            _viewModel.DeleteEvent += DeleteEventHandler;
            
        }

        private void Gantt_Loaded(object sender, RoutedEventArgs e)
        {
            //restrict selection to a single task
            this.Gantt.GanttGrid.Model.Options.ListBoxSelectionMode = Syncfusion.Windows.Controls.Grid.GridSelectionMode.One;

            //hide the milestone column
            for (int i = 0; i < this.Gantt.GanttGrid.Columns.Count; i++)
            {
                Syncfusion.Windows.Controls.Grid.GridTreeColumn column = this.Gantt.GanttGrid.Columns[i];

                if (column.MappingName.Equals("Milestone"))
                {
                    this.Gantt.GanttGrid.Columns.Remove(column);
                }
            }

        }


        private void DeleteEventHandler()
        {
            if (MessageBox.Show("Click OK to delete the selected task and all child tasks. This action cannot be undone.",
                "Delete Task", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                _viewModel.DeleteTaskCommand.Execute(true);
            }
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SelectedItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Gantt.SelectedItems.Count > 0)
            {
                Debug.WriteLine($"Selected task ({((ui_models.GanttTaskModel)Gantt.SelectedItems.FirstOrDefault()).TaskName})");
                //pass the selected task to the viewmodel (can't see how to bind this in XAML)
                //TODO - bind this from the XAML
                _viewModel.SelectedTask = (ui_models.GanttTaskModel)Gantt.SelectedItems.FirstOrDefault();
            }
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (_viewModel.HasUnsavedChanges == true)
            {
                if (MessageBox.Show(this, "You may have unsaved changes that will be lost.", "Confirm close", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.Cancel)
                {
                    cancelEventArgs.Cancel = true;
                }
            }
        }

        #region CustomZooming
        private void Gantt_ZoomChanged(object sender, ZoomChangedEventArgs args)
        {
            //setup the cell created event handlers
            var CellCreatedEventHandler_Quater = new ScheduleCellCreatedEventHandler(Quater);
            var CellCreatedEventHandler_Month = new ScheduleCellCreatedEventHandler(Month);
            var CellCreatedEventHandler_MonthYear = new ScheduleCellCreatedEventHandler(MonthAndYear);
            var CellCreatedEventHandler_WeekNo = new ScheduleCellCreatedEventHandler(WeekNo);
            var CellCreatedEventHandler_WeekCommencing = new ScheduleCellCreatedEventHandler(WeekCommencing);

            if (args.ZoomFactor >= 30)
            {
                this.Gantt.ScheduleCellCreated += CellCreatedEventHandler_Quater;

                args.ScheduleHeaderInfo = new List<GanttScheduleRowInfo>
                {
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Years },
                    new GanttScheduleRowInfo{ CellsPerUnit = 3, TimeUnit = TimeUnit.Months, PixelsPerUnit = 15}
                };
            }
            if (args.ZoomFactor >= 40)
            {
                args.ScheduleHeaderInfo = new List<GanttScheduleRowInfo>
                {
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Years },
                    new GanttScheduleRowInfo{ CellsPerUnit = 3, TimeUnit = TimeUnit.Months, PixelsPerUnit = 30}
                };
            }
            if (args.ZoomFactor >= 50)
            {
                this.Gantt.ScheduleCellCreated -= CellCreatedEventHandler_Quater;
                this.Gantt.ScheduleCellCreated += CellCreatedEventHandler_Month;

                args.ScheduleHeaderInfo = new List<GanttScheduleRowInfo>
                {
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Years },
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Months, PixelsPerUnit = 40 }
                };
            }
            if (args.ZoomFactor >= 60)
            {
                args.ScheduleHeaderInfo = new List<GanttScheduleRowInfo>
                {
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Years },
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Months, PixelsPerUnit = 60 }
                };
            }
            if (args.ZoomFactor >= 70)
            {
                args.ScheduleHeaderInfo = new List<GanttScheduleRowInfo>
                {
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Years },
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Months, PixelsPerUnit = 70 }
                };
            }
            if (args.ZoomFactor >= 80)
            {
                args.ScheduleHeaderInfo = new List<GanttScheduleRowInfo>
                {
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Years },
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Months, PixelsPerUnit = 80 }
                };
            }
            if (args.ZoomFactor >= 90)
            {
                //this.Gantt.ScheduleCellCreated -= CellCreatedEventHandler_Month;
                //this.Gantt.ScheduleCellCreated += CellCreatedEventHandler_MonthYear;
                this.Gantt.ScheduleCellCreated += CellCreatedEventHandler_WeekNo;

                args.ScheduleHeaderInfo = new List<GanttScheduleRowInfo>
                {
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Years },
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Months },
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Weeks, PixelsPerUnit = 35 }
                };
            }
            if (args.ZoomFactor >= 100)
            {
                this.Gantt.ScheduleCellCreated -= CellCreatedEventHandler_Month;
                this.Gantt.ScheduleCellCreated += CellCreatedEventHandler_MonthYear;

                this.Gantt.ScheduleCellCreated -= CellCreatedEventHandler_WeekNo;
                this.Gantt.ScheduleCellCreated += CellCreatedEventHandler_WeekCommencing;
    
                args.ScheduleHeaderInfo = new List<GanttScheduleRowInfo>
                {
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Years },
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Months },
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Weeks },
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Days, PixelsPerUnit = 20 }
                };
            }
            if (args.ZoomFactor >= 120)
            {
                args.ScheduleHeaderInfo = new List<GanttScheduleRowInfo>
                {
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Years },
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Months },
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Weeks },
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Days, PixelsPerUnit = 35 }
                };
            }
            if (args.ZoomFactor >= 200)
            {
                args.ScheduleHeaderInfo = new List<GanttScheduleRowInfo>
                {
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Years },
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Months },
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Weeks },
                    new GanttScheduleRowInfo{ CellsPerUnit = 1, TimeUnit = TimeUnit.Days },
                    new GanttScheduleRowInfo{ CellsPerUnit = 12, TimeUnit = TimeUnit.Hours, PixelsPerUnit= 4, CellTextFormat="hh tt"}
                };
            }

            args.Handled = true;
        }

        private void WeekCommencing(object sender, ScheduleCellCreatedEventArgs args)
        {
            DateTime currentDate = args.CurrentCell.CellDate;

            if (args.CurrentCell.CellTimeUnit == TimeUnit.Weeks)
            {
                // Get the value for currentDate. DayOfWeek is an enum with 0 being Sunday, 1 Monday, etc
                var cellDayOfWeek = (int)currentDate.DayOfWeek;

                var dateStartOfWeek = currentDate;
                // If currentDate is not Monday, then get the date for Monday
                if (cellDayOfWeek != 1)
                {
                    // How many days to get back to Monday from today
                    var daysToStartOfWeek = (cellDayOfWeek - 1);
                    // Subtract from today's date the number of days to get to Monday
                    dateStartOfWeek = currentDate.AddDays(-daysToStartOfWeek);
                }

                args.CurrentCell.Content = $"{dateStartOfWeek.ToString("MMM dd yyyy")}";
            }
        }

        private void WeekNo(object sender, ScheduleCellCreatedEventArgs args)
        {
            DateTime currentDate = args.CurrentCell.CellDate;

            if (args.CurrentCell.CellTimeUnit == TimeUnit.Weeks)
            {
                args.CurrentCell.Content = $"{System.Globalization.ISOWeek.GetWeekOfYear(currentDate)}";
            }
        }

        private void MonthAndYear(object sender, ScheduleCellCreatedEventArgs args)
        {
            DateTime currentDate = args.CurrentCell.CellDate;

            if (args.CurrentCell.CellTimeUnit == TimeUnit.Months)
            {
                args.CurrentCell.Content = $"{currentDate.ToString("MMM")} {currentDate.Year}";
            }
        }

        private void Month(object sender, ScheduleCellCreatedEventArgs args)
        {
            DateTime currentDate = args.CurrentCell.CellDate;

            if (args.CurrentCell.CellTimeUnit == TimeUnit.Months)
            {
                args.CurrentCell.Content = currentDate.ToString("MMM");
            }
        }

        void Quater(object sender, ScheduleCellCreatedEventArgs args)
        {
            DateTime currentDate = args.CurrentCell.CellDate;

            if (args.CurrentCell.CellTimeUnit == TimeUnit.Months)
            {
                // Quarter 1 dates contains months below 3. since we are cheking the cell dates and changing the Content of the cell.
                if (currentDate.Month <= 3)
                {
                    args.CurrentCell.Content = "Q 1";
                    args.CurrentCell.CellToolTip = "Quarter 1";
                }

                // Quarter 2 dates contains months between 4 - 6. since we are cheking the cell dates and changing the Content of the cell.
                else if (currentDate.Month > 3 && currentDate.Month <= 6)
                {
                    args.CurrentCell.Content = "Q 2";
                    args.CurrentCell.CellToolTip = "Quarter 2";
                }

                // Quarter 3 dates contains months  between 6 - 9. since we are cheking the cell dates and changing the Content of the cell.
                else if (currentDate.Month > 6 && currentDate.Month <= 9)
                {
                    args.CurrentCell.Content = "Q 3";
                    args.CurrentCell.CellToolTip = "Quarter 3";
                }

                // Quarter 4 dates contains months below 9 - 12. since we are cheking the cell dates and changing the Content of the cell.
                else if (currentDate.Month > 9 && currentDate.Month <= 12)
                {
                    args.CurrentCell.Content = "Q 4";
                    args.CurrentCell.CellToolTip = "Quarter 4";
                }
            }
        }

        #endregion
    }
}
