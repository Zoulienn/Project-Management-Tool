using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project_Management_Tool;
using System;
using System.Linq;

// Resolve ambiguity with System.Threading.Tasks.Task
using UserTask = Project_Management_Tool.Task;

namespace TestProject
{
    [TestClass]
    public sealed class TestUser
    {
        [TestMethod]
        public void CreateUser_WithValidPassword_ShouldSucceed()
        {
            // Arrange
            string username = "zou";
            string password = "strongpass";

            // Act
            User user = new User(username, password);

            // Assert
            Assert.AreEqual(username, user.UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateUser_WithShortPassword_ShouldThrow()
        {
            // Arrange + Act
            var user = new User("zou", "short");
        }

        [TestMethod]
        public void AddProject_ShouldAddProjectToUser()
        {
            // Arrange
            var user = new User("zou", "securepass");
            var project = new Project("MyProject", DateTime.Today, DateTime.Today.AddDays(7));

            // Act
            user.AddProject(project);

            // Assert
            Assert.IsTrue(user.UserProjects.Contains(project));
        }

        [TestMethod]
        public void AssignTask_ToUserProject_ShouldAddTask()
        {
            // Arrange
            var user = new User("zou", "securepass");
            var project = new Project("MyProject", DateTime.Today, DateTime.Today.AddDays(7));
            user.AddProject(project);
            var task = new UserTask("Task1", "Desc", DateTime.Today.AddDays(3), Priority.Medium);

            // Act
            user.AssignTask(task, project);

            // Assert
            Assert.AreEqual(1, project.Tasks.Count);
            Assert.AreEqual("Task1", project.Tasks[0].Title);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssignTask_ToNonUserProject_ShouldThrow()
        {
            // Arrange
            var user = new User("zou", "securepass");
            var project = new Project("OtherProject", DateTime.Today, DateTime.Today.AddDays(7));
            var task = new UserTask("Task1", "Desc", DateTime.Today.AddDays(3), Priority.Medium);

            // Act
            user.AssignTask(task, project);
        }

        [TestMethod]
        public void GetAllTasks_ShouldReturnSortedTasks()
        {
            // Arrange
            var user = new User("zou", "securepass");
            var project = new Project("MyProject", DateTime.Today, DateTime.Today.AddDays(7));
            user.AddProject(project);

            var task1 = new UserTask("Late", "desc", DateTime.Today.AddDays(5), Priority.Low);
            var task2 = new UserTask("Sooner", "desc", DateTime.Today.AddDays(2), Priority.Medium);

            user.AssignTask(task1, project);
            user.AssignTask(task2, project);

            // Act
            var tasks = user.GetAllTasks();

            // Assert
            Assert.AreEqual(2, tasks.Count);
            Assert.AreEqual("Sooner", tasks[0].Title); // Sooner due date first
        }

        [TestMethod]
        public void GetCompletedTasks_ShouldReturnOnlyCompletedOrderedByPriority()
        {
            // Arrange
            var user = new User("zou", "securepass");
            var project = new Project("MyProject", DateTime.Today, DateTime.Today.AddDays(7));
            user.AddProject(project);

            var task1 = new UserTask("LowPriority", "desc", DateTime.Today.AddDays(3), Priority.Low);
            var task2 = new UserTask("HighPriority", "desc", DateTime.Today.AddDays(3), Priority.High);

            task1.CompleteTask();
            task2.CompleteTask();

            user.AssignTask(task1, project);
            user.AssignTask(task2, project);

            // Act
            var completedTasks = user.GetCompletedTasks();

            // Assert
            Assert.AreEqual(2, completedTasks.Count);
            Assert.AreEqual("HighPriority", completedTasks[0].Title); // High comes first by priority
        }
    }
}