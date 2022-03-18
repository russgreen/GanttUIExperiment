using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Models
{
    public class TaskLinkModel : BaseModel
    {
        //private int _linkType;
        //private int _predecessorTaskID;

        //public TaskLinkModel()
        //{
        //    _linkType = 1;
        //    _predecessorTaskID = 0;
        //}

        public int LinkID { get; set; }

        public int StartID { get; set; }

        public int EndID { get; set; }

        /// <summary>
        /// Type of the link between tasks:
        /// FinishToFinish = 0, 
        /// FinishToStart = 1, 
        /// StartToFinish = 2, 
        /// StartToStart = 3
        /// </summary>
        public int LinkType { get; set; }

        public double Offset { get; set; } //not enabled in DB yet

        ////added this propery to mimic the Synfusion predecessor class
        //public int GanttTaskIndex { get => _predecessorTaskID; set => _predecessorTaskID = value; }

        ////added this propery to mimic the Synfusion predecessor class
        //public GanttTaskRelationship GanttTaskRelationship
        //{
        //    get => (GanttTaskRelationship)_linkType;
        //    set => _linkType = (int)value;
        //}
    }

    ////added this enum to mimic the Synfusion predecessor class
    //public enum GanttTaskRelationship
    //{
    //    FinishToFinish = 0,
    //    FinishToStart = 1,
    //    StartToFinish = 2,
    //    StartToStart = 3
    //}
}
