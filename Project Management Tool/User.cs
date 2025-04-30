using System;

namespace Project_Management_Tool;

/*
	•	User

	•	Properties:
	•	Username (string)
	•	UserPassword (string)
	•	AssignedProjects (List)
    
	•	Methods:
    •	AddProject(Project project) - add projects to user
	•	AssignTask(Task task, Project project) — assign a task to a specific project
	•	GetAllTasks() — retrieve all tasks assigned to the user
	•	GetCompletedTasks() — retrieve all completed tasks
*/
public class User
{
    public int UserId { get; set; } // PK
    public string UserName { get; set;}
    private string UserPassword = string.Empty;
    public string Password { 
        get { return UserPassword;}
        private set { 
            if (value.Length < 8) 
            {
                throw new ArgumentException("Password must be at least 8 letters long");
            }else
            {
                UserPassword = value;
            }
        }}
    public List<Project> UserProjects{ get; set; }

     public User() {} // for EF Core

    public User (string UserName, string password)
    {
        this.UserName = UserName;
        Password = password;
        this.UserProjects = new List<Project>();
    }

    public void AddProject(Project project)
    {
        this.UserProjects.Add(project);
    }

    public void AssignTask(Task task, Project project)
    {
        if (!UserProjects.Contains(project))
        {
            throw new InvalidOperationException("User does not own this project.");
        }
        else
        {
            project.AddTask(task);
        }
    }

    public List<Task> GetAllTasks()
    {
        // SelectMany as ONE project has MANY tasks and the user has MANY projects
        return UserProjects.SelectMany(project => project.Tasks)
                           .OrderBy(task => task.DueDate)
                           .ToList();
    }

    public List<Task> GetCompletedTasks()
    {
        return UserProjects.SelectMany(project => project.Tasks)
                           .Where(task => task.TaskStatus == Status.Completed)
                           .OrderByDescending(task => task.TaskPriority)
                           .ToList();   
    }
}
