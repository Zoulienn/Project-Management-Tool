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
    public string ProjectName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    List<Task> Tasks { get; set; } = new List<Task>();

    public Project(string projectName, DateTime startDate, DateTime endDate)
    {
        this.ProjectName = projectName;
        this.StartDate = startDate;
        this.EndDate = endDate;
    }

    public void AddTask(Task task)
    {
        task.SetProject(this); //link task to project
        Tasks.Add(task);
    }

    
}
