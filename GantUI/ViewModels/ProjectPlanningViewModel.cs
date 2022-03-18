using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Specialized;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;
using CoreLibrary.Models;
using CoreLibrary.DataAccess;
using GantUI.Models;
using Syncfusion.Windows.Controls.Gantt;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace GantUI.ViewModels
{
    public class ProjectPlanningViewModel : CoreLibrary.ViewModels.BaseViewModel
    {
        #region Private Properties
        ProjectModel _project;
        int _summaryTaskID;
        private GanttTaskModel _selectedTask;
        #endregion

        #region Public Properties
        /// <summary>
        /// The title of the main window of the application
        /// </summary>
        public string WindowTitle { get; private set; }

        /// <summary>
        /// Display name of the project
        /// </summary>
        public string ProjectName { get; private set; }

        /// <summary>
        /// Collection of <see cref="TaskModel"/> bound to Gantt
        /// </summary>
        public ObservableCollection<GanttTaskModel> TaskCollection { get; set; }

        /// <summary>
        /// Used to overlay project milestone dates
        /// </summary>
        public List<StripLineInfo> StripCollection { get; set; }

        /// <summary>
        /// Zoom level of the gantt chart
        /// </summary>
        public double ZoomFactor { get; set; } = 100;

        /// <summary>
        /// Control if save button is enabled / disabled
        /// </summary>
        public bool HasUnsavedChanges { get; private set; } = false;

        //ControlEnablers used to enable / disable UI elements
        public bool CanAddTask { get; private set; } = false;
        public bool CanDeleteTask { get; private set; } = false;
        public bool CanIndentTask { get; private set; } = false;
        public bool CanOutdentTask { get; private set; } = false;

        /// <summary>
        /// <see cref="ui_models.GanttTaskModel"/> of the selected task
        /// </summary>
        public GanttTaskModel SelectedTask
        {
            get => _selectedTask;
            set
            {
                SetProperty(ref _selectedTask, value);
                SetControlEnablers();
            }
        }
        #endregion

        #region Commands
        public ICommand SaveChangesCommand { get; }
        public ICommand AddTaskCommand { get; }
        public ICommand ConfirmDeleteCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand IndentTaskCommand { get; }
        public ICommand OutdentTaskCommand { get; }
        public ICommand DetectChangeCommand { get; }

        #endregion

        #region Events
        public event Action DeleteEvent;
        //public event Action RefreshGantt;
        #endregion

        public ProjectPlanningViewModel(int projectID)
        {
            //load the project
            //_project = GlobalConfig.Connection.GetProject(projectID);
            _project = DataAccess.GetProject(projectID);
            ProjectName = _project.ProjectNumberNameIdentifier;

            StripCollection = GetProjectDates();

            //load the list to the collection
            List<TaskModel> tasks = DataAccess.GetTasks_FakeData();
            List<GanttTaskModel> ganttTasks = new List<GanttTaskModel>();

            foreach (var item in tasks)
            {
                GanttTaskModel newTask = new GanttTaskModel
                {
                    TaskID = item.TaskID,
                    ProjectID = item.ProjectID,
                    TaskName = item.TaskName,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    ParentID = item.ParentID,
                    Progress = item.Progress,
                    TaskDescription = item.TaskDescription,
                    BudgetCost = item.BudgetCost,
                    BudgetHours = item.BudgetHours,
                    PercentageFee = item.PercentageFee,
                    Enable = item.Enable
                };

                //add the predecessors by mapping across from the List<TaskLinkModel> 
                if (item.Links.Count > 0)
                {
                    foreach (var link in item.Links)
                    {
                        newTask.Predecessors.Add(new Predecessor()
                        {
                            GanttTaskIndex = link.StartID,
                            GanttTaskRelationship = (GanttTaskRelationship)link.LinkType,
                            Offset = link.Offset
                        });
                    }
                }

                ganttTasks.Add(newTask);
            }

            LoadTasksToCollection(ganttTasks);

            //we've loaded the data so no unsaved changes yet
            HasUnsavedChanges = false;

            //set the window title information
            var informationVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            WindowTitle = $"Demo Gantt {informationVersion} - Project Plan for {_project.ProjectNumberNameIdentifier}";

            //setup the commands
            SaveChangesCommand = new RelayCommand(SaveChanges);
            AddTaskCommand = new RelayCommand(AddTask);
            ConfirmDeleteCommand = new RelayCommand(ConfirmDelete);
            DeleteTaskCommand = new RelayCommand(DeleteTask);
            IndentTaskCommand = new RelayCommand(IndentTask);
            OutdentTaskCommand = new RelayCommand(OutdentTask);
            DetectChangeCommand = new RelayCommand(DetectChange);
        }

        private List<StripLineInfo> GetProjectDates()
        {
            //StyleSelector styleSelector = GanttDictionaries.GanttStyleDictionary["styleselector"] as StyleSelector;
            //DataTemplateSelector templateSelector = GanttDictionaries.GanttStyleDictionary["tempselector"] as DataTemplateSelector;
            //DataTemplate template = GanttDictionaries.GanttStyleDictionary["temp"] as DataTemplate;

            List<StripLineInfo> data = new List<StripLineInfo>();

            //data.Add(new StripLineInfo() { Content = RepeatStripContent, StartDate = new DateTime(2012, 6, 4), EndDate = new DateTime(2012, 6, 4), HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, Background = RepeatStripColor, RepeatBehavior = Repeat.Week, RepeatFor = 1, RepeatUpto = new DateTime(2012, 12, 10), ContentTemplate = template });
            //data.Add(new StripLineInfo() { Content = NonRepeatStripContent, StartDate = new DateTime(2012, 6, 1), EndDate = new DateTime(2012, 6, 1), HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, Background = NonRepeatStripColor });
            //data.Add(new StripLineInfo() { Content = "Demo of the product to Customer", StartDate = new DateTime(2012, 12, 13), EndDate = new DateTime(2012, 12, 13), HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, Background = (Brush)new BrushConverter().ConvertFrom("#FFF79608") });

            if (_project.DateFeeProposealIssued != null)
            {
                data.Add(new StripLineInfo() { Content = "Fee Proposal Issued", StartDate = (DateTime)_project.DateFeeProposealIssued, EndDate = (DateTime)_project.DateFeeProposealIssued, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, Background = Brushes.LightGray });
            }

            if (_project.DateContract != null)
            {
                data.Add(new StripLineInfo() { Content = "Contract Date", StartDate = (DateTime)_project.DateContract, EndDate = (DateTime)_project.DateContract, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, Background = Brushes.LightGray });
            }

            if (_project.DateSiteStart != null)
            {
                data.Add(new StripLineInfo() { Content = "Start on Site", StartDate = (DateTime)_project.DateSiteStart, EndDate = (DateTime)_project.DateSiteStart, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, Background = Brushes.LightGray });
            }

            if (_project.DateSiteEnd != null)
            {
                data.Add(new StripLineInfo() { Content = "Practical Completion", StartDate = (DateTime)_project.DateSiteEnd, EndDate = (DateTime)_project.DateSiteEnd, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, Background = Brushes.LightGray });
            }

            if (_project.DateEndOfDefects != null)
            {
                data.Add(new StripLineInfo() { Content = "End of Defects ", StartDate = (DateTime)_project.DateEndOfDefects, EndDate = (DateTime)_project.DateEndOfDefects, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, Background = Brushes.LightGray });
            }

            return data;
        }

        /// <summary>
        /// Control the bool values that enable / disable UI elements
        /// </summary>
        private void SetControlEnablers()
        {
            if (_selectedTask == null)
            {
                //no task selected so do nothing
                CanAddTask = false;
                CanDeleteTask = false;
                CanIndentTask = false;
                CanOutdentTask = false;
            }
            else
            {
                //we have a task selected and can do stuff
                CanAddTask = true;
                CanDeleteTask = true;
                CanIndentTask = true;
                CanOutdentTask = true;

                if (_selectedTask.TaskID == _summaryTaskID)
                {
                    //we have the summary task selected
                    CanAddTask = true;
                    CanDeleteTask = false;
                    CanIndentTask = false;
                    CanOutdentTask = false;
                }

                if (_selectedTask.ParentID == _summaryTaskID)
                {
                    //we have a tasks immediately below summary task selected
                    CanAddTask = true;
                    CanDeleteTask = true;
                    CanIndentTask = true;
                    CanOutdentTask = false;
                }
            }
        }

        /// <summary>
        /// Load a list of <see cref="TaskModel" /> to the TaskCollection
        /// </summary>
        /// <param name="taskList"></param>
        private void LoadTasksToCollection(List<GanttTaskModel> taskList)
        {
            //populate the child tasks for each task
            foreach (var task in taskList)
            {
                task.ChildTasks = new ObservableCollection<GanttTaskModel>(taskList.Where(t => t.ParentID == task.TaskID).ToList());
                task.Predecessors.CollectionChanged += PredecessorCollection_CollectionChanged;
                task.PropertyChanged += TaskModel_PropertyChanged;
            }

            //add the summary task to the collection....it contains all the nested children
            List<GanttTaskModel> tasks = taskList.Where(task => task.ParentID == 0).ToList();
            _summaryTaskID = tasks[0].TaskID;

            TaskCollection = new ObservableCollection<GanttTaskModel>(tasks);
            TaskCollection.CollectionChanged += TaskCollection_CollectionChanged;
        }

        /// <summary>
        /// This will get called when the collection is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TaskCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine("Change type: " + e.Action);

            //just notify the user of unsaved changed
            HasUnsavedChanges = true;
        }

        /// <summary>
        /// This will get called when an item in the collection is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("Item changed: " + e.PropertyName);

            //just notify the user of unsaved changed
            HasUnsavedChanges = true;
        }

        /// <summary>
        /// This will get called if tasks predecessors collection is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PredecessorCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //https://docs.microsoft.com/en-us/dotnet/api/system.collections.specialized.notifycollectionchangedeventargs?view=net-5.0
            //Add       0   An item was added to the collection.
            //Move      3   An item was moved within the collection.
            //Remove    1   An item was removed from the collection.
            //Replace   2   An item was replaced in the collection.
            //Reset     4   The contents of the collection changed dramatically.

            Debug.WriteLine($"Change type {e.Action}");
            //Debug.WriteLine($"Task {((GanttTaskModel)sender).TaskID} {((GanttTaskModel)sender).TaskName} linked {e.Action}");

            //just notify the user of unsaved changed
            HasUnsavedChanges = true;
        }

        /// <summary>
        /// Convert the observable collection back to a list of <see cref="TaskModel" />
        /// </summary>
        /// <returns></returns>
        private List<GanttTaskModel> FlatList()
        {
            //flatten the ObservableCollection
            //var flatList = from t in TaskCollection.SelectMany(oc => oc.ChildTasks) select t;
            var flatList = new List<GanttTaskModel>();
            flatList.Add(TaskCollection.FirstOrDefault());
            flatList.AddRange(getChildTasks(TaskCollection.FirstOrDefault()));

            return flatList;
        }

        private List<GanttTaskModel> getChildTasks(GanttTaskModel task)
        {
            var lst = task.ChildTasks.ToList();
            foreach (var item in task.ChildTasks.ToList())
            {
                if (item.ChildTasks.Count > 0)
                {
                    lst.AddRange(getChildTasks(item));
                }
            }
            return lst;
        }

        /// <summary>
        /// Save unsaved changes back to the database
        /// </summary>
        private void SaveChanges()
        {
            Debug.WriteLine("Save changes clicked!");

            foreach (var task in FlatList())
            {
                //GlobalConfig.Connection.UpdateTask(task);
            }

            HasUnsavedChanges = false;
        }


        /// <summary>
        /// Adds a new tasks to the collection and the DB
        /// </summary>
        private void AddTask()
        {
            Debug.WriteLine("Add task clicked!");
            GanttTaskModel task = new GanttTaskModel
            {
                ProjectID = _project.ProjectID,
                ParentID = _selectedTask.TaskID,
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date.AddDays(30),
                TaskName = "New Task"
            };

            //save the task into the database
            //GlobalConfig.Connection.CreateTask(task);
            task.TaskID = FlatList().OrderByDescending(t => t.TaskID).First().TaskID + 1;

            //add the task to the collection
            _selectedTask.ChildTasks.Add(task);

            //attach the event handler so we get notified of changes
            task.PropertyChanged += TaskModel_PropertyChanged;
        }

        /// <summary>
        /// Command to get confirmation if the taks should be deleted
        /// </summary>
        private void ConfirmDelete()
        {
            DeleteEvent?.Invoke();
        }

        /// <summary>
        /// Delete the task from the collection and the DB
        /// </summary>
        private void DeleteTask()
        {
            Debug.WriteLine("Delete task clicked!");
            //first delete any child tasks
            if (_selectedTask.ChildTasks.Count > 0)
            {
                foreach (var task in _selectedTask.ChildTasks)
                {
                    //GlobalConfig.Connection.DeleteTask(task);
                }
            }

            //delete the selected task
            //GlobalConfig.Connection.DeleteTask(_selectedTask);

            //flatten the collection
            var tasks = FlatList();
            tasks.Remove(_selectedTask);

            //remove the task from the list
            TaskCollection.Remove(_selectedTask);

            //re-build the collection
            LoadTasksToCollection(tasks);

            //refresh the UI
            _selectedTask = null;
            SetControlEnablers();

            //force the zoom to reset to where it was left
            ZoomFactor += .1;
            ZoomFactor -= .1;
        }

        /// <summary>
        /// Indent the taks - change the parentID
        /// </summary>
        private void IndentTask()
        {
            Debug.WriteLine("Indent task clicked!");
        }

        /// <summary>
        /// Outdent the taks - change the parentID
        /// </summary>
        private void OutdentTask()
        {
            Debug.WriteLine("Outdent task clicked!");
        }

        /// <summary>
        /// Detect any changes that need a manual save to the DB
        /// </summary>
        private void DetectChange()
        {
            //just notify the user of unsaved changed
            HasUnsavedChanges = true;
        }


    }
}
