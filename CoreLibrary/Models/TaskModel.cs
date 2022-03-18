using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreLibrary.Models
{

    public class TaskModel : BaseModel
    {
        public int TaskID { get; set; }
        [Required]
        public int ProjectID { get; set; }
        [Required]
        [StringLength(125)]
        public string TaskName { get; set; }
        [Required]
        public bool Enable { get; set; }
        public string TaskDescription { get; set; }
        /// <summary>
        /// % of the overall project fee
        /// </summary>
        [Range(0, 100)]
        public double PercentageFee { get; set; }
        public double BudgetCost { get; set; }
        public double BudgetHours { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan Duration { get; set; }

        [Range(0, 100)]
        public double Progress { get; set; }
        /// <summary>
        /// The parent task of the task.  Used when drawing gantt charts
        /// </summary>
        [Required]
        public int ParentID { get; set; }

        public List<TaskLinkModel> Links { get; set; } = new List<TaskLinkModel>();

        //these are added in the UI project as only needed for the gantt chart implementation
        //public ObservableCollection<TaskModel> ChildTasks { get; set; } = new ObservableCollection<TaskModel>();

        //public ObservableCollection<TaskLinkModel> Predecessors { get; set; } = new ObservableCollection<TaskLinkModel>();
    }
}
