using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Project_Management_Tool;

// Resolve ambiguity with System.Threading.Tasks.Task
using UserTask = Project_Management_Tool.Task;

namespace TestProject
{
    [TestClass]
    public class TestProject
    {
        private Project _project = null!;
        private DateTime _today;

        [TestInitialize]
        public void Setup()
        {
            _today = DateTime.Today;
            _project = new Project("Test Project", _today, _today.AddDays(30));
        }

        [TestMethod]
        public void Constructor_ShouldInitializeProjectCorrectly()
        {
            Assert.AreEqual("Test Project", _project.ProjectName);
            Assert.AreEqual(_today, _project.StartDate);
            Assert.AreEqual(_today.AddDays(30), _project.EndDate);
            Assert.AreEqual(0, _project.Tasks.Count);
        }

        [TestMethod]
        public void AddTask_ShouldAddTaskAndLinkToProject()
        {
            var task = new UserTask("Refactor Models", "Refactor legacy models", _today.AddDays(2), Priority.Medium);
            _project.AddTask(task);

            Assert.AreEqual(1, _project.Tasks.Count);
            Assert.AreEqual(_project, task.Project);
        }

        [TestMethod]
        public void GetPendingTasks_ShouldReturnOnlyNonCompletedTasks()
        {
            var task1 = new UserTask("Design UI", "Create wireframes", _today.AddDays(3), Priority.High);
            var task2 = new UserTask("Setup Firebase", "Integrate DB", _today.AddDays(5), Priority.Medium);
            var task3 = new UserTask("Push Notification", "Enable FCM", _today.AddDays(4), Priority.Low);
            task2.CompleteTask();

            _project.AddTask(task1);
            _project.AddTask(task2);
            _project.AddTask(task3);

            var pendingTasks = _project.GetPendingTasks();

            Assert.AreEqual(2, pendingTasks.Count);
            Assert.IsFalse(pendingTasks.Any(t => t.TaskStatus == Status.Completed));
        }

        [TestMethod]
        public void GetOverdueTasks_ShouldReturnOnlyOverdueAndNotCompleted()
        {
            var task1 = new UserTask("Bug Fixes", "Fix issues", _today.AddDays(-5), Priority.High);
            var task2 = new UserTask("Deployment", "Push to Azure", _today.AddDays(-2), Priority.Medium);
            var task3 = new UserTask("Write Docs", "Docs", _today.AddDays(3), Priority.Low);
            var task4 = new UserTask("QA Testing", "Regression test", _today.AddDays(-1), Priority.High);

            task2.CompleteTask();

            _project.AddTask(task1);
            _project.AddTask(task2);
            _project.AddTask(task3);
            _project.AddTask(task4);

            var overdueTasks = _project.GetOverdueTasks();

            Assert.AreEqual(2, overdueTasks.Count);
            Assert.IsTrue(overdueTasks.All(t => t.DueDate < DateTime.Now && t.TaskStatus != Status.Completed));
        }

        [TestMethod]
        public void GetTasksByPriority_ShouldReturnTasksFilteredByPriority()
        {
            var task1 = new UserTask("Set Up Auth", "Login flow", _today.AddDays(2), Priority.High);
            var task2 = new UserTask("Build Dashboard", "Admin UI", _today.AddDays(4), Priority.Medium);
            var task3 = new UserTask("Optimize Queries", "Speed up endpoints", _today.AddDays(1), Priority.High);

            _project.AddTask(task1);
            _project.AddTask(task2);
            _project.AddTask(task3);

            var highPriorityTasks = _project.GetTasksByPriority(Priority.High);

            Assert.AreEqual(2, highPriorityTasks.Count);
            Assert.IsTrue(highPriorityTasks.All(t => t.TaskPriority == Priority.High));
            Assert.IsTrue(highPriorityTasks[0].DueDate >= highPriorityTasks[1].DueDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddTask_NullTask_ShouldThrowArgumentNullException()
        {
            _project.AddTask(null!);
        }
    }
}