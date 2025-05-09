using System;

namespace Project_Management_Tool;

/*
	•	Project

	•	Properties:
	•	ProjectName (string)
	•	StartDate (DateTime)
	•	EndDate (DateTime)
	•	Tasks (List) — the tasks within a project

	•	Methods:
	•	AddTask(Task task) — add a new task to the project
	•	GetPendingTasks() — return tasks that are not yet completed
	•	GetOverdueTasks() — return tasks that are past due
	•	GetTasksByPriority(Priority priority) — filter tasks by priority using LINQ
*/
public class Project
{
    public int ProjectId { get; set; } // Primary Key
    public string ProjectName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Task> Tasks { get; set; } = new List<Task>();

    public int UserId { get; set; } // FK
    public User User { get; set; }  // Navigation

     public Project() {} // For EF core
     
    public Project(string projectName, DateTime startDate, DateTime endDate)
    {
        this.ProjectName = projectName;
        this.StartDate = startDate;
        this.EndDate = endDate;
    }

    public void AddTask(Task task)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task), "Task cannot be empty.");
        }
        task.SetProject(this); //link task to project
        Tasks.Add(task);
    }

    public List<Task> GetPendingTasks()
    {
        var result = Tasks.Where(task => task.TaskStatus != Status.Completed)
                    .OrderBy(task => task.DueDate)
                    .ToList();
        return result;
    }

    public List<Task> GetOverdueTasks()
    {
        var result = Tasks.Where(task => task.DueDate < DateTime.Now && task.TaskStatus != Status.Completed)
                    .OrderBy(task => task.TaskPriority)
                    .ToList();
        return result;
    }

    public List<Task> GetTasksByPriority(Priority priority)
    {
        return Tasks.Where(task => task.TaskPriority == priority)
                    .OrderByDescending(task => task.DueDate)
                    .ToList();
    }
}
