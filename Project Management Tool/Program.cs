using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Project_Management_Tool;

class Program
{
    static List<User> users = LoadUsers();
    static User? currentUser = null;

    static void Main(string[] args)
    {
        int action = 0;
        do
        {
            Console.Clear();
            Console.WriteLine("==== Task & Project Management ====");
            Console.WriteLine("1. Create New User");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Input choice: ");
            int.TryParse(Console.ReadLine(), out action);

            switch (action)
            {
                case 1:
                    User newUser = CreateNewUser();
                    users.Add(newUser);
                    SaveUsers(users);
                    Console.WriteLine("User created successfully!");
                    Console.ReadKey();
                    break;

                case 2:
                    currentUser = Login();
                    if (currentUser != null)
                    {
                        UserMenu(currentUser);
                    }
                    break;
            }

        } while (action != 3);

        SaveUsers(users);
    }

    public static User CreateNewUser()
    {
        string? name;
        string? password;

        while (true)
        {
            Console.Write("Enter User Name: ");
            name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Username cannot be empty or whitespace.");
            }
            else
            {
                break;
            }
        }

        while (true)
        {
            Console.Write("Enter User Password: ");
            password = Console.ReadLine();

            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                Console.WriteLine("Password must be at least 8 characters long.");
            }
            else
            {
                break;
            }
        }

        return new User(name, password);
    }

    public static User? Login()
    {
        Console.Write("Enter Username: ");
        string? name = Console.ReadLine();
        Console.Write("Enter Password: ");
        string? pass = Console.ReadLine();

        User? found = users.FirstOrDefault(u => u.UserName == name && u.Password == pass);

        if (found != null)
        {
            Console.WriteLine("Login successful!");
            Console.ReadKey();
            return found;
        }
        else
        {
            Console.WriteLine("Invalid username or password.");
            Console.ReadKey();
            return null;
        }
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
        return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
    }

    public static void UserMenu(User user)
    {
        int choice = 0;
        do
        {
            Console.Clear();
            Console.WriteLine($"Welcome, {user.UserName}!");
            Console.WriteLine("4. Create New Project");
            Console.WriteLine("5. View All My Projects");
            Console.WriteLine("6. Select a Project to Manage");
            Console.WriteLine("-1. Logout");
            Console.Write("Choice: ");
            int.TryParse(Console.ReadLine(), out choice);

            switch (choice)
            {
                case 4:
                    Console.Write("Enter project name: ");
                    string projectName = Console.ReadLine()??"";
                    Console.Write("Enter start date (yyyy-mm-dd): ");
                    DateTime start = DateTime.Parse(Console.ReadLine()??"");
                    Console.Write("Enter end date (yyyy-mm-dd): ");
                    DateTime end = DateTime.Parse(Console.ReadLine()??"");
                    Project project = new Project(projectName, start, end);
                    user.AddProject(project);
                    SaveUsers(users);
                    break;

                case 5:
                    foreach (var p in user.UserProjects)
                    {
                        Console.WriteLine($"- {p.ProjectName} ({p.StartDate.ToShortDateString()} to {p.EndDate.ToShortDateString()})");
                    }
                    Console.ReadKey();
                    break;

                case 6:
                    //project manage
                    break;
            }

        } while (choice != -1);
    }
}
