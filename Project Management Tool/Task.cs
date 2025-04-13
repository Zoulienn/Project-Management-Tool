using System;

namespace Project_Management_Tool;

/*
	•	Task:
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
    public string? Title { get; set; }
    public string? Description { get; set;}
    public DateTime DueDate { get; set; }
    public Priority TaskPriority { get; set; }
    public Status TaskStatus { get; set; }
    public Project project { get; set; }

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