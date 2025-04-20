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
        [TestMethod]
        public void Constructor_ShouldInitializeProjectCorrectly()
        {
            var project = new Project("AI Dev", DateTime.Today, DateTime.Today.AddDays(30));

            Assert.AreEqual("AI Dev", project.ProjectName);
            Assert.AreEqual(DateTime.Today, project.StartDate);
            Assert.AreEqual(DateTime.Today.AddDays(30), project.EndDate);
            Assert.AreEqual(0, project.Tasks.Count);
        }

        [TestMethod]
        public void AddTask_ShouldAddTaskAndLinkToProject()
        {
            var project = new Project("Backend Refactor", DateTime.Today, DateTime.Today.AddDays(15));
            var task = new UserTask("Refactor Models", "Refactor legacy models to new structure", DateTime.Today.AddDays(2), Priority.Medium);

            project.AddTask(task);

            Assert.AreEqual(1, project.Tasks.Count);
            Assert.AreEqual(project, task.Project);
        }

        [TestMethod]
        public void GetPendingTasks_ShouldReturnOnlyNonCompletedTasks()
        {
            var project = new Project("Mobile App", DateTime.Today, DateTime.Today.AddDays(20));

            var task1 = new UserTask("Design UI", "Create wireframes", DateTime.Today.AddDays(3), Priority.High);
            var task2 = new UserTask("Setup Firebase", "Integrate DB", DateTime.Today.AddDays(5), Priority.Medium);
            var task3 = new UserTask("Push Notification", "Enable FCM", DateTime.Today.AddDays(4), Priority.Low);
            task2.CompleteTask();

            project.AddTask(task1);
            project.AddTask(task2);
            project.AddTask(task3);

            var pendingTasks = project.GetPendingTasks();

            Assert.AreEqual(2, pendingTasks.Count);
            Assert.IsFalse(pendingTasks.Any(t => t.TaskStatus == Status.Completed));
        }

        [TestMethod]
        public void GetOverdueTasks_ShouldReturnOnlyOverdueAndNotCompleted()
        {
            var project = new Project("WebApp", DateTime.Today.AddDays(-10), DateTime.Today.AddDays(10));

            var task1 = new UserTask("Bug Fixes", "Fix reported issues", DateTime.Today.AddDays(-5), Priority.High);
            var task2 = new UserTask("Deployment", "Push to Azure", DateTime.Today.AddDays(-2), Priority.Medium);
            var task3 = new UserTask("Write Docs", "Technical docs", DateTime.Today.AddDays(3), Priority.Low);
            var task4 = new UserTask("QA Testing", "Regression test", DateTime.Today.AddDays(-1), Priority.High);
            
            task2.CompleteTask();

            project.AddTask(task1);
            project.AddTask(task2);
            project.AddTask(task3);
            project.AddTask(task4);

            var overdueTasks = project.GetOverdueTasks();

            Assert.AreEqual(2, overdueTasks.Count);
            Assert.IsTrue(overdueTasks.All(t => t.DueDate < DateTime.Now && t.TaskStatus != Status.Completed));
        }

        [TestMethod]
        public void GetTasksByPriority_ShouldReturnTasksFilteredByPriority()
        {
            var project = new Project("CRM Tool", DateTime.Today, DateTime.Today.AddDays(25));

            var task1 = new UserTask("Set Up Auth", "Implement login", DateTime.Today.AddDays(2), Priority.High);
            var task2 = new UserTask("Build Dashboard", "Admin UI", DateTime.Today.AddDays(4), Priority.Medium);
            var task3 = new UserTask("Optimize Queries", "Speed up endpoints", DateTime.Today.AddDays(1), Priority.High);

            project.AddTask(task1);
            project.AddTask(task2);
            project.AddTask(task3);

            var highPriorityTasks = project.GetTasksByPriority(Priority.High);

            Assert.AreEqual(2, highPriorityTasks.Count);
            Assert.IsTrue(highPriorityTasks.All(t => t.TaskPriority == Priority.High));
            Assert.IsTrue(highPriorityTasks[0].DueDate >= highPriorityTasks[1].DueDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddTask_NullTask_ShouldThrowArgumentNullException()
        {
            var project = new Project("Error Handling", DateTime.Today, DateTime.Today.AddDays(5));
            project.AddTask(null!);
        }
    }
}