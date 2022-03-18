using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLibrary.Models;
using Syncfusion.Windows.Controls.Gantt;

namespace GantUI.Models
{
    public class GanttTaskModel : TaskModel
    {
        private bool _milestone;

        public ObservableCollection<GanttTaskModel> ChildTasks { get; set; } = new ObservableCollection<GanttTaskModel>();

        public ObservableCollection<Predecessor> Predecessors { get; set; } = new ObservableCollection<Predecessor>();

        public bool Milestone
        {
            get => StartDate.Date == EndDate.Date;
            set
            {
                _milestone = value;
            }
        }
    }
}
