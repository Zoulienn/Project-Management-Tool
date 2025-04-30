using System;
using System.Text.Json.Serialization;

namespace Project_Management_Tool;

/*
	•	Task

	•	Properties:
	•	Title (string)
	•	Description (string)
	•	DueDate (DateTime)
	•	Priority (enum: Low, Medium, High)
	•	Status (enum: NotStarted, InProgress, Completed)
	•	Project (Project) — a task belongs to a project
    
	•	Methods:
	•	CompleteTask() — change status to Completed
	•	UpdateTaskStatus(Status newStatus) — change status to a new status (e.g., InProgress)
*/

public class Task
{
    public int TaskId { get; set; } // PK
    public string Title { get; set; }
    public string Description { get; set;}
    public DateTime DueDate { get; set; }
    public Priority TaskPriority { get; set; }
    public Status TaskStatus { get; private set; }
    
    public int ProjectId { get; set; } // FK

    [JsonIgnore] // prevents circular reference issue
    public Project? Project { get; private set; }

    public Task() {} // For EF core

    public Task (string title, string description,DateTime dueDate,Priority taskPriority)
    {
        this.Title = title;
        this.Description = description;
        this.DueDate = dueDate;
        this.TaskPriority = taskPriority;
        this.TaskStatus = Status.NotStarted;
    }

    public void SetProject(Project project)
    {
        this.Project = project;
    }

    public void UpdateTaskStatus(Status newStatus)
    {
        this.TaskStatus = newStatus;
    }

    public void CompleteTask(){
        this.TaskStatus = Status.Completed;
    }

}

public enum Priority
{
    Low,
    Medium,
    High
}

public enum Status
{
    NotStarted,
    InProgress,
    Completed
}