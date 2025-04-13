namespace Project_Management_Tool;

class Program
{
    static void Main(string[] args)
    {
        /*
            Design a task/project management tool that allows users to:
            •	Create and manage tasks and projects.
            •	Track deadlines, priority levels, and progress of each task.
            •	Mark tasks as completed.
            •	Sort and filter tasks by different criteria (e.g., due date, priority).
            •	Notify users about task status updates.
        */

        // Authentication
        int action = 0;
        do
        {
            Console.Clear();
            Console.WriteLine("1. Create New User");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Input choice: ");
            action = Convert.ToInt32(Console.ReadLine());
            
            switch (action)
            {
                case 1:
                    // Create new user
                    break;
                case 2:
                    Console.WriteLine("");
                    break ;
            }
        } while (action != 3);
    }

    public static void CreateNewUser()
    {
        string? name, password;
        bool valid = true;
        do
        {
            try
            {

                do
                {
                    Console.Write("Enter User Name: ");
                    name = Console.ReadLine();
                } while (name != null || name == "");

                do
                {
                    Console.Write("Enter User Password: ");
                    password = Console.ReadLine();
                } while (password != null || password.Length < 8 );
                
            }
            catch (System.Exception)
            {
                
                throw;
            }
        } while (!valid);

        User user = new User("julien","password1234");
    }
}

/*
🔐 Authentication / Setup
	1.	Create New User
	2.	Login as Existing User
	3.	Exit

Once logged in:

⸻

📁 Project Management
	4.	Create a New Project
	5.	View All My Projects
	6.	Select a Project to Manage

Once a project is selected:

🔨 Inside a Selected Project
	7.	View All Tasks in This Project
	8.	Add a New Task to This Project
	9.	Get Pending Tasks
	10.	Get Overdue Tasks
	11.	Get Tasks by Priority (Low, Medium, High)
	12.	Back to Main Menu

⸻

📋 Task Overview (across all projects)
	13.	View All Tasks (Sorted by Due Date)
	14.	View Completed Tasks (Sorted by Priority)
	15.	Mark Task as Completed
	16.	Update Task Status
	17.	Back to Main Menu

*/