using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Project_Management_Tool;

class Program
{
    static List<User> users = LoadUsers();
    static User? currentUser = null;

    static void Main(string[] args)
    {
        int action;
        do
        {
            PrintHeader("Task & Project Management");

            PrintMenuOption(1, "Create New User");
            PrintMenuOption(2, "Login");
            PrintMenuOption(3, "Exit");
            PrintDivider();

            Console.Write("Input choice: ");
            int.TryParse(Console.ReadLine(), out action);

            switch (action)
            {
                case 1:
                    User newUser = CreateNewUser();
                    users.Add(newUser);
                    SaveUsers(users);
                    Console.WriteLine("\nUser created successfully!");
                    PressToContinue();
                    break;

                case 2:
                    currentUser = Login();
                    if (currentUser != null)
                        UserMenu(currentUser);
                    break;
            }

        } while (action != 3);

        SaveUsers(users);
    }

    public static User CreateNewUser()
    {
        string? name, password;

        PrintHeader("Create New User");

        while (true)
        {
            Console.Write("Enter User Name: ");
            name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
                Console.WriteLine("⚠ Username cannot be empty.");
            else break;
        }

        while (true)
        {
            Console.Write("Enter Password (min 8 chars): ");
            password = Console.ReadLine();
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                Console.WriteLine("Password must be at least 8 characters.");
            else break;
        }

        return new User(name, password);
    }

    public static User? Login()
    {
        PrintHeader("User Login");

        Console.Write("Enter Username: ");
        string? name = Console.ReadLine();
        Console.Write("Enter Password: ");
        string? pass = Console.ReadLine();

        User? found = users.FirstOrDefault(u => u.UserName == name && u.Password == pass);

        if (found != null)
        {
            Console.WriteLine("\nLogin successful!");
            PressToContinue();
            return found;
        }
        else
        {
            Console.WriteLine("Invalid username or password.");
            PressToContinue();
            return null;
        }
    }

    public static void UserMenu(User user)
    {
        int choice;
        do
        {
            PrintHeader($"Welcome, {user.UserName}");

            PrintMenuOption(4, "Create New Project");
            PrintMenuOption(5, "View All My Projects");
            PrintMenuOption(6, "Select a Project to Manage");
            Console.WriteLine();
            PrintMenuOption(13, "View All Tasks (by Due Date)");
            PrintMenuOption(14, "View Completed Tasks (by Priority)");
            PrintMenuOption(15, "Mark Task as Completed");
            PrintMenuOption(16, "Update Task Status");
            Console.WriteLine();
            PrintMenuOption(-1, "Logout");
            PrintDivider();

            Console.Write("Choice: ");
            int.TryParse(Console.ReadLine(), out choice);

            switch (choice)
            {
                case 4: CreateProject(user); break;
                case 5: ShowProjects(user); break;
                case 6: ManageProject(); break;
                case 13: ShowAllTasks(); break;
                case 14: ShowCompletedTasks(); break;
                case 15: MarkTaskCompleted(); SaveUsers(users); break;
                case 16: UpdateTaskStatus(); SaveUsers(users); break;
            }

        } while (choice != -1);
    }

    static void CreateProject(User user)
    {
        PrintHeader("Create New Project");

        Console.Write("Project Name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Start Date (yyyy-mm-dd): ");
        DateTime start = DateTime.Parse(Console.ReadLine() ?? "");

        Console.Write("End Date (yyyy-mm-dd): ");
        DateTime end = DateTime.Parse(Console.ReadLine() ?? "");

        var project = new Project(name, start, end);
        user.AddProject(project);

        SaveUsers(users);
        Console.WriteLine("Project created and saved!");
        PressToContinue();
    }

    static void ShowProjects(User user)
    {
        PrintHeader("Your Projects");
        foreach (var project in user.UserProjects)
        {
            Console.WriteLine($"- {project.ProjectName} ({project.StartDate.ToShortDateString()} to {project.EndDate.ToShortDateString()})");
        }
        PressToContinue();
    }

    static void ManageProject()
    {
        PrintHeader("Manage Project");
        Console.Write("Enter Project Name: ");
        string? name = Console.ReadLine();
        var project = currentUser!.UserProjects.FirstOrDefault(p => p.ProjectName == name);

        if (project == null)
        {
            Console.WriteLine("Project not found.");
            PressToContinue();
            return;
        }

        int choice;
        do
        {
            PrintHeader($"Managing: {project.ProjectName}");

            PrintMenuOption(7, "View All Tasks");
            PrintMenuOption(8, "Add New Task");
            PrintMenuOption(9, "Pending Tasks");
            PrintMenuOption(10, "Overdue Tasks");
            PrintMenuOption(11, "Tasks by Priority");
            PrintMenuOption(12, "Back");
            PrintDivider();

            Console.Write("Choice: ");
            int.TryParse(Console.ReadLine(), out choice);

            switch (choice)
            {
                case 7:
                    foreach (var task in project.Tasks)
                        Console.WriteLine($"{task.Title} - {task.TaskStatus} - 📅 {task.DueDate.ToShortDateString()}");
                    PressToContinue();
                    break;

                case 8:
                    Console.Write("Title: ");
                    string title = Console.ReadLine() ?? "";
                    Console.Write("Description: ");
                    string desc = Console.ReadLine() ?? "";
                    Console.Write("Due Date (yyyy-mm-dd): ");
                    DateTime due = DateTime.Parse(Console.ReadLine() ?? "");
                    Console.Write("Priority (Low/Medium/High): ");
                    Priority priority = Enum.Parse<Priority>(Console.ReadLine() ?? "", true);

                    var newtask = new Task(title, desc, due, priority);
                    newtask.SetProject(project);
                    project.Tasks.Add(newtask);
                    SaveUsers(users);
                    Console.WriteLine("Task added!");
                    PressToContinue();
                    break;

                case 9:
                    foreach (var t in project.GetPendingTasks())
                        Console.WriteLine($"{t.Title} - {t.TaskStatus}");
                    PressToContinue();
                    break;

                case 10:
                    foreach (var t in project.GetOverdueTasks())
                        Console.WriteLine($"{t.Title} - {t.DueDate.ToShortDateString()}");
                    PressToContinue();
                    break;

                case 11:
                    Console.Write("Priority: ");
                    Priority p = Enum.Parse<Priority>(Console.ReadLine() ?? "", true);
                    foreach (var t in project.GetTasksByPriority(p))
                        Console.WriteLine($"{t.Title} - {t.DueDate.ToShortDateString()}");
                    PressToContinue();
                    break;
            }

        } while (choice != 12);
    }

    static void ShowAllTasks()
    {
        PrintHeader("All Tasks");
        foreach (var task in currentUser!.GetAllTasks())
            Console.WriteLine($"{task.Title} - Due: {task.DueDate.ToShortDateString()} - {task.TaskStatus}");
        PressToContinue();
    }

    static void ShowCompletedTasks()
    {
        PrintHeader("Completed Tasks");
        foreach (var task in currentUser!.GetCompletedTasks())
            Console.WriteLine($"{task.Title} - Priority: {task.TaskPriority}");
        PressToContinue();
    }

    static void MarkTaskCompleted()
    {
        PrintHeader("Mark Task Completed");
        Console.Write("Enter Task Title: ");
        string? title = Console.ReadLine();
        var task = currentUser!.GetAllTasks().FirstOrDefault(t => t.Title == title);
        task?.CompleteTask();
        Console.WriteLine("Task marked as completed.");
        PressToContinue();
    }

    static void UpdateTaskStatus()
    {
        PrintHeader("Update Task Status");
        Console.Write("Enter Task Title: ");
        string? title = Console.ReadLine();
        var task = currentUser!.GetAllTasks().FirstOrDefault(t => t.Title == title);

        if (task == null) return;

        Console.Write("New Status (NotStarted, InProgress, Completed): ");
        Status status = Enum.Parse<Status>(Console.ReadLine() ?? "", true);
        task.UpdateTaskStatus(status);
        Console.WriteLine("Task status updated.");
        PressToContinue();
    }

    public static void SaveUsers(List<User> users)
    {
        string json = JsonSerializer.Serialize(users);
        File.WriteAllText("users.json", json);
    }

    public static List<User> LoadUsers()
    {
        if (!File.Exists("users.json"))
            return new List<User>();

        string json = File.ReadAllText("users.json");
        var loadedUsers = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

        foreach (var user in loadedUsers)
        {
            foreach (var project in user.UserProjects)
            {
                foreach (var task in project.Tasks)
                {
                    task.SetProject(project);
                }
            }
        }

        return loadedUsers;
    }

    static void PrintHeader(string title)
    {
        Console.Clear();
        string border = new string('═', title.Length + 4);
        Console.WriteLine($"╔{border}╗");
        Console.WriteLine($"║  {title.ToUpper()}  ║");
        Console.WriteLine($"╚{border}╝");
    }

    static void PressToContinue()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    static void PrintMenuOption(int number, string text)
    {
        Console.WriteLine($"  [{number}] {text}");
    }

    static void PrintDivider()
    {
        Console.WriteLine("────────────────────────────────────");
    }
}