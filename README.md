# Introduction 
Expanded sample to show how I'm trying to integrate the Syncfusion Gantt control into a solution that previously used a Telerik control.

The Gantt UI is a standalone exe in the solution called from a main winforms application.  An argument is passed in for the ID of the project that should be loaded and the tasks associated with that project are pulled from the database and displayed in the gantt.

Tasks are used in other areas of the application and not just the gantt UI hence the TaskModel in the core lib.  

Half a thought that there might be a web or other platform UI in the future so ViewModels in the core lib....looks like I need to rethink that....this is complicated enough for now!!!

GantUI shows the structure I started out with but ran into difficulties when trying to implement predecessors. The main reasons for starting out this was was trying to utlise an existing database structure. At the moment this demo loads the data OK but PredecessorMapping="Predecessor" is not enabled at line 187 in MainWindow.xaml.

GantUI Alternative Structure shows the structure I think it probably needs to move towards 

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references

# Build and Test
TODO: Describe and show how to build your code and run the tests. 

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)