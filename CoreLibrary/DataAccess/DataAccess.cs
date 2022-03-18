using CoreLibrary.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace CoreLibrary.DataAccess
{
    public static class DataAccess
    {
        
        //in reality this method is called to pupulate the List<TaskModel> from the BD and also the related links between taks. 
        public static List<TaskModel> GetTasks_ByProject(int projectID)
        {
            //string sql = "SELECT * " +
            //            "FROM tblTasks " +
            //            "WHERE (ProjectID = @ProjectID);";

            //using (IDbConnection dbConnection = new SqlConnection(GlobalConfig.CnnString))
            //{
            //    dbConnection.Open();
            //    var taskList = dbConnection.Query<TaskModel>(sql, new { ProjectID = projectID }).AsList<TaskModel>();

            //    return taskList;
            //}

            string sql = "SELECT tblTasks.*, tblTasksLinks.* " +
                        "FROM tblTasks " +
                        "LEFT OUTER JOIN tblTasksLinks ON tblTasks.TaskID = tblTasksLinks.EndID " +
                        "WHERE (ProjectID = @ProjectID);";

            using (IDbConnection dbConnection = new SqlConnection("SQL DB Connection String"))
            {
                dbConnection.Open();

                var taskDictionary = new Dictionary<int, TaskModel>();

                var taskList = dbConnection.Query<TaskModel, TaskLinkModel, TaskModel>(
                    sql,
                    (task, tasklink) =>
                    {
                        TaskModel taskEntry;

                        if (!taskDictionary.TryGetValue(task.TaskID, out taskEntry))
                        {
                            taskEntry = task;
                            taskDictionary.Add(taskEntry.TaskID, taskEntry);
                        }

                        if (tasklink != null)
                        {
                            taskEntry.Links.Add(tasklink);
                        }

                        return taskEntry;
                    },
                         new { ProjectID = projectID },
                         splitOn: "LinkID")
                    .AsList<TaskModel>();

                return taskList;
            }

        }

        public static List<TaskModel> GetTasks_FakeData()
        {
            List<TaskModel> taskList = new List<TaskModel>();

            //each project has a summary task
            taskList.Add(new TaskModel() { TaskID = 1, TaskName = "Project Summary Task", ParentID = 0, StartDate = new DateTime(2021, 2, 1), EndDate = new DateTime(2021, 12, 1) });

            //add some child tasks
            taskList.Add(new TaskModel() { TaskID = 2, TaskName = "Sub-Task 1", ParentID = 1, StartDate = new DateTime(2021, 2, 1), EndDate = new DateTime(2021, 3, 1) });
            taskList.Add(new TaskModel() { TaskID = 3, TaskName = "Sub-Task 2", ParentID = 1, StartDate = new DateTime(2021, 3, 1), EndDate = new DateTime(2021, 5, 1) });
            taskList.Add(new TaskModel() { TaskID = 4, TaskName = "Sub-Task 3", ParentID = 1, StartDate = new DateTime(2021, 6, 1), EndDate = new DateTime(2021, 8, 1) });

            //add some child-child tasks
            taskList.Add(new TaskModel() { TaskID = 5, TaskName = "Child-Task 1", ParentID = 2, StartDate = new DateTime(2021, 2, 1), EndDate = new DateTime(2021, 2, 15) });
            taskList.Add(new TaskModel() { TaskID = 6, TaskName = "Child-Task 2", ParentID = 2, StartDate = new DateTime(2021, 2, 15), EndDate = new DateTime(2021, 3, 1) });
            taskList.Add(new TaskModel() { TaskID = 7, TaskName = "Child-Task 3", ParentID = 6, StartDate = new DateTime(2021, 2, 15), EndDate = new DateTime(2021, 3, 1) });

            //add some predecessors
            taskList[5].Links.Add( new TaskLinkModel() { LinkID = 1, EndID = 6, StartID = 5, LinkType = 1, Offset = 0 } );
            taskList[6].Links.Add(new TaskLinkModel() { LinkID = 2, EndID = 7, StartID = 6, LinkType = 1, Offset = 0 });

            return taskList;
        }

        public static ProjectModel GetProject(int projectID)
        {
            ProjectModel model = new ProjectModel()
            {
                ProjectID = projectID,
                ProjectName = "Project Name",
                ProjectNumber = "0001",
                ProjectIdentifier = "PID",
                SummaryTaskID = 1,
                DateFeeProposealIssued = new DateTime(2021, 1, 25),
                DateContract = new DateTime(2021, 5, 1),
                DateSiteStart = new DateTime(2021, 6, 1)
            };

            return model;
        }

    }
}
