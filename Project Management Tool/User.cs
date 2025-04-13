using System;

namespace Project_Management_Tool;

/*
	•	User

	•	Properties:
	•	Username (string)
	•	UserPassword (string)
	•	AssignedProjects (List)
    
	•	Methods:
	•	AssignTask(Task task, Project project) — assign a task to a specific project
	•	GetAllTasks() — retrieve all tasks assigned to the user
	•	GetCompletedTasks() — retrieve all completed tasks
*/
public class User
{
    public string UserName { get; set;}
    private string UserPassword;
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

    public User (string UserName, string password)
    {
        this.UserName = UserName;
        this.Password = password;
        this.UserProjects = new List<Project>();
    }

}
