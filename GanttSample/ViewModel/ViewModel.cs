#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Syncfusion.Windows.Controls.Gantt;
using Syncfusion.Windows.Shared;
using System.Collections;

namespace ExternalPropertyBinding
{
    class ViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel()
        {
            _taskCollection =  ViewModel.GetData();
        }

        private ObservableCollection<Task> _taskCollection;

        /// <summary>
        /// Gets or sets the appointment item source.
        /// </summary>
        /// <value>The appointment item source.</value>
        public ObservableCollection<Task> TaskCollection
        {
            get
            {
                return _taskCollection;
            }
            set
            {
                _taskCollection = value;
            }
        }
        

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<Task> GetData()
        {
            var data = new ObservableCollection<Task>();
            data.Add(new Task() { Id = 1, Name = "Scope", StDate = new DateTime(2011, 7, 3), EndDate = new DateTime(2011, 7, 14), Complete = 40d });
            data[0].ChildTask.Add(new Task() { Id = 2, Name = "Scope", StDate = new DateTime(2011, 7, 3), EndDate = new DateTime(2011, 7, 14), Complete = 40d });
            data[0].ChildTask.Add((new Task() { Id = 3, Name = "Justify Project Offfice via business model", StDate = new DateTime(2011, 7, 6), EndDate = new DateTime(2011, 7, 7), Complete = 20d }));
            data[0].ChildTask.Add((new Task() { Id = 4, Name = "Secure executive sponsorship", StDate = new DateTime(2011, 7, 10), EndDate = new DateTime(2011, 7, 14), Complete = 10d, }));
            data[0].ChildTask.Add((new Task() { Id = 5, Name = "Secure complete", StDate = new DateTime(2011, 7, 14), EndDate = new DateTime(2011, 7, 14), Complete = 10d }));
            
            data.Add(new Task() { Id = 6, Name = "Risk Assessment", StDate = new DateTime(2011, 7, 15), EndDate = new DateTime(2011, 7, 24) });
            data[1].ChildTask.Add((new Task() { Id = 7, Name = "Perform risk assessment", StDate = new DateTime(2011, 7, 15), EndDate = new DateTime(2011, 7, 21), Complete = 20d, }));
            data[1].ChildTask.Add((new Task() { Id = 8, Name = "Evaluate risk assessment", StDate = new DateTime(2011, 7, 21), EndDate = new DateTime(2011, 7, 23), Complete = 20d, }));
            data[1].ChildTask.Add((new Task() { Id = 9, Name = "Prepare contingency plans", StDate = new DateTime(2011, 7, 21), EndDate = new DateTime(2011, 7, 24), Complete = 20d, }));
            data[1].ChildTask.Add((new Task() { Id = 10, Name = "Risk Assessment complete", StDate = new DateTime(2011, 7, 24), EndDate = new DateTime(2011, 7, 24), Complete = 30d }));

            data.Add(new Task() { Id = 11, Name = "Monitoring", StDate = new DateTime(2011, 7, 25), EndDate = new DateTime(2011, 8, 6), Duration = new TimeSpan(1, 0, 0, 0) });
            data[2].ChildTask.Add((new Task() { Id = 12, Name = "Prepare Meeting agenda", StDate = new DateTime(2011, 7, 25), EndDate = new DateTime(2011, 7, 26), Complete = 20d, }));
            data[2].ChildTask.Add((new Task() { Id = 13, Name = "Conduct review meeting", StDate = new DateTime(2011, 7, 27), EndDate = new DateTime(2011, 7, 30), Complete = 20d, }));
            data[2].ChildTask.Add((new Task() { Id = 14, Name = "Migrate critical issues", StDate = new DateTime(2011, 7, 31), EndDate = new DateTime(2011, 8, 2), Complete = 20d, }));
            data[2].ChildTask.Add((new Task() { Id = 15, Name = "Estabilish change mgmt Control", StDate = new DateTime(2011, 8, 3), EndDate = new DateTime(2011, 8, 6), Complete = 30d, }));
            data[2].ChildTask.Add((new Task() { Id = 16, Name = "Monitoring Complete", StDate = new DateTime(2011, 8, 6), EndDate = new DateTime(2011, 8, 6), Complete = 30d }));

            data.Add(new Task() { Id = 17, Name = "Post Implementation", StDate = new DateTime(2011, 7, 25), EndDate = new DateTime(2011, 8, 12) });
            data[3].ChildTask.Add((new Task() { Id = 18, Name = "Obtain User feedback", StDate = new DateTime(2011, 7, 25), EndDate = new DateTime(2011, 7, 29), Complete = 20d, }));
            data[3].ChildTask.Add((new Task() { Id = 19, Name = "Evaluate lessons learned", StDate = new DateTime(2011, 7, 29), EndDate = new DateTime(2011, 8, 5), Complete = 20d, }));
            data[3].ChildTask.Add((new Task() { Id = 20, Name = "Modify items as necessary", StDate = new DateTime(2011, 8, 2), EndDate = new DateTime(2011, 8, 8), Complete = 20d, }));
            data[3].ChildTask.Add((new Task() { Id = 21, Name = "Post Implementation complete", StDate = new DateTime(2011, 8, 8), EndDate = new DateTime(2011, 8, 12), Complete = 30d }));

        
            data[1].ChildTask[0].Resource.Add(new Resource() { ID = 5, Name = "Neil" });
            data[1].ChildTask[1].Resource.Add(new Resource() { ID = 7, Name = "Johnson" });
            data[1].ChildTask[1].Resource.Add(new Resource() { ID = 8, Name = "Julie" });
            data[1].ChildTask[2].Resource.Add(new Resource() { ID = 9, Name = "Peterson" });
            data[1].ChildTask[3].Resource.Add(new Resource() { ID = 10, Name = "Thomas" });

            data[3].ChildTask[1].Resource.Add(new Resource() { ID = 5, Name = "David" });
            data[3].ChildTask[2].Resource.Add(new Resource() { ID = 7, Name = "Peter" });
            data[3].ChildTask[3].Resource.Add(new Resource() { ID = 8, Name = "Thomas" });
 
            return data;
        }
    }
}
