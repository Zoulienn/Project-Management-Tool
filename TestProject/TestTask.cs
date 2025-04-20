using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Project_Management_Tool;

// for resolving naming clash
using UserTask = Project_Management_Tool.Task;

namespace TestProject
{
    [TestClass]
    public class TestTask
    {
        private UserTask _task = null!;
        private string _title = "Design UI";
        private string _description = "Create a pretty home page for an app";
        private DateTime _dueDate;
        private Priority _priority;

        [TestInitialize]
        public void Setup()
        {
            _dueDate = DateTime.Today.AddDays(5);
            _priority = Priority.High;
            _task = new UserTask(_title, _description, _dueDate, _priority);
        }

        [TestMethod]
        public void Constructor_ShouldInitializeTaskCorrectly()
        {
            Assert.AreEqual(_title, _task.Title);
            Assert.AreEqual(_description, _task.Description);
            Assert.AreEqual(_dueDate, _task.DueDate);
            Assert.AreEqual(_priority, _task.TaskPriority);
            Assert.AreEqual(Status.NotStarted, _task.TaskStatus);
            Assert.IsNull(_task.Project); // project is not set yet
        }

        [TestMethod]
        public void UpdateTaskStatus_ShouldChangeStatus()
        {
            _task.UpdateTaskStatus(Status.InProgress);
            Assert.AreEqual(Status.InProgress, _task.TaskStatus);

            _task.UpdateTaskStatus(Status.Completed);
            Assert.AreEqual(Status.Completed, _task.TaskStatus);
        }

        [TestMethod]
        public void CompleteTask_ShouldSetStatusToCompleted()
        {
            _task.CompleteTask();
            Assert.AreEqual(Status.Completed, _task.TaskStatus);
        }

        [TestMethod]
        public void SetProject_ShouldLinkProjectToTask()
        {
            var project = new Project("Test Project", DateTime.Today, DateTime.Today.AddDays(10));
            _task.SetProject(project);
            Assert.AreEqual(project, _task.Project);
        }
    }
}