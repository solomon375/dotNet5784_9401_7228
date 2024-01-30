using Dal;
using DalApi;
using DO;
using System;
using System.Security.Cryptography;

namespace DalTest
{
    internal class Program
    {
        /// <summary>
        /// Data Access Layer instance for interacting with tasks, engineers, and dependencies.
        /// </summary>
        //static readonly IDal s_dal = new DalList(); //stage 2
        //static readonly IDal s_dal = new DalXml(); //stage 3
        static readonly IDal s_dal = Factory.Get; //stage 4

        /// <summary>
        /// Main method for the Dal Test program.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            try
            {

                bool exit = false;

                do
                {
                    /// Display menu for user input
                    Console.WriteLine("enter a number:\n " +
                    "0 = exit\n 1 = task\n 2 = engineer\n 3 = dependency\n 4 = Initializ");

                    // Choose the case based on user input
                    char choice = Console.ReadKey().KeyChar;

                    switch (choice)
                    {
                        case '1':
                            checkTask();// Perform operations related to tasks
                            break;

                        case '2':
                            checkEngineer();// Perform operations related to engineers
                            break;

                        case '3':
                            checkDependency();// Perform operations related to dependencies
                            break;

                        case '4':
                            Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
                            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                            if (ans == "Y") //stage 3
                                //Initialization.Do(s_dal); //stage 2
                                Initialization.Do(); //stage 4
                            break;

                        case '0':
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.\n");
                            break;
                    }
                } while (!exit);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }
        /// <summary>
        /// Method to handle operations related to Task.
        /// </summary>
        static void checkTask()
        {
            bool exit = false;
            do
            {
                display();// Display menu for CRUD operations

                char choice = Console.ReadKey().KeyChar;

                switch (choice)
                {
                    case '0':
                        exit = true;
                        break;

                    default:
                        Choice(choice, "Task");
                        break;
                }
            } while (!exit);
        }

        /// <summary>
        /// Method to handle operations related to Engineer.
        /// </summary>
        static void checkEngineer()
        {
            bool exit = false;
            do
            {
                display();// Display menu for CRUD operations

                char choice = Console.ReadKey().KeyChar;

                switch (choice)
                {
                    case '0':
                        exit = true;
                        break;

                    default:
                        Choice(choice, "Engineer");
                        break;
                }
            } while (!exit);
        }

        /// <summary>
        /// Method to handle operations related to Dependency.
        /// </summary>
        static void checkDependency()
        {
            bool exit = false;
            do
            {
                display();// Display menu for CRUD operations

                char choice = Console.ReadKey().KeyChar;

                switch (choice)
                {
                    case '0':
                        exit = true;
                        break;

                    default:
                        Choice(choice, "Dependency");
                        break;
                }
            } while (!exit);
        }

        /// <summary>
        /// Display menu for CRUD operations.
        /// </summary>
        static void display()
        {
            Console.WriteLine($"Choose an operation:");
            Console.WriteLine("1. Create");
            Console.WriteLine("2. Read");
            Console.WriteLine("3. Read All");
            Console.WriteLine("4. Update");
            Console.WriteLine("5. Delete");
            Console.WriteLine("0. Exit\n");
        }

        /// <summary>
        /// Perform CRUD operations based on user choice.
        /// </summary>
        /// <param name="c">User choice character.</param>
        /// <param name="s">Entity type (Task, Engineer, Dependency).</param>
        static void Choice(char c, string s)
        {
            switch (c)
            {
                case '1':
                    Create(s);
                    break;

                case '2':
                    Read(s);
                    break;

                case '3':
                    ReadAll(s);
                    break;

                case '4':
                    Update(s);
                    break;

                case '5':
                    Delete(s);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.\n");
                    break;
            }
        }

        /// <summary>
        /// Create a new item based on the user's choice (Task, Engineer, Dependency).
        /// </summary>
        /// <param name="s">Entity type (Task, Engineer, Dependency).</param>
        static void Create(string s)
        {
            switch (s)
            {
                case "Task":
                    DO.Task item = GetTaskItem();
                    Console.WriteLine(s_dal.Task?.Create(item));
                    break;

                case "Engineer":
                    DO.Engineer item1 = GetEngineerItem();
                    Console.WriteLine(s_dal.Engineer?.Create(item1));
                    break;

                case "Dependency":
                    DO.Dependency item2 = GetDependencyItem();
                    Console.WriteLine(s_dal.Dependency?.Create(item2));
                    break;
            }
            //send the user to the right create method
        }

        /// <summary>
        /// Read an item based on the user's choice (Task, Engineer, Dependency).
        /// </summary>
        /// <param name="s">Entity type (Task, Engineer, Dependency).</param>
        static void Read(string s)
        {
            switch (s)
            {
                case "Task":
                    Console.Write("Enter task id\n");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dal.Task?.Read(id));
                    break;

                case "Engineer":
                    Console.Write("Enter task id\n");
                    if (!int.TryParse(Console.ReadLine(), out int id1))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dal.Engineer?.Read(id1));
                    break;

                case "Dependency":
                    Console.Write("Enter task id\n");
                    if (!int.TryParse(Console.ReadLine(), out int id2))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dal.Dependency?.Read(id2));
                    break;
            }
        }

        /// <summary>
        /// Read all items based on the user's choice (Task, Engineer, Dependency).
        /// </summary>
        /// <param name="s">Entity type (Task, Engineer, Dependency).</param>
        static void ReadAll(string s)
        {
            switch (s)
            {
                case "Task":

                    foreach (var item in s_dal.Task.ReadAll())
                    {
                        Console.WriteLine(item);
                    }
                    break;

                case "Engineer":
                    foreach (var item in s_dal.Engineer.ReadAll())
                    {
                        Console.WriteLine(item);
                    }
                    break;

                case "Dependency":
                    foreach (var item in s_dal.Dependency.ReadAll())
                    {
                        Console.WriteLine(item);
                    }
                    break;
            }
        }

        /// <summary>
        /// Update an item based on the user's choice (Task, Engineer, Dependency).
        /// </summary>
        /// <param name="s">Entity type (Task, Engineer, Dependency).</param>
        static void Update(string s)
        {
            switch (s)
            {
                case "Task":
                    Console.WriteLine("Enter Task ID\n");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dal.Task?.Read(id));

                    Console.WriteLine("Enter new values and the same id\n");
                    DO.Task item = GetTaskItem();
                    s_dal.Task?.Update(item);
                    break;

                case "Engineer":
                    Console.WriteLine("Enter engineer ID\n");
                    if (!int.TryParse(Console.ReadLine(), out int id1))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dal.Engineer?.Read(id1));

                    Console.WriteLine("Enter new values and the same id\n");
                    DO.Engineer item1 = GetEngineerItem();
                    s_dal.Engineer?.Update(item1);
                    break;

                case "Dependency":
                    Console.WriteLine("Enter Dependency ID\n");
                    if (!int.TryParse(Console.ReadLine(), out int id2))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dal.Dependency?.Read(id2));

                    Console.WriteLine("Enter new values and the same id\n");
                    DO.Dependency item2 = GetDependencyItem();
                    s_dal.Dependency?.Update(item2);
                    break;
            }
        }

        /// <summary>
        /// Delete an item based on the user's choice (Task, Engineer, Dependency).
        /// </summary>
        /// <param name="s">Entity type (Task, Engineer, Dependency).</param>
        static void Delete(string s)
        {
            switch (s)
            {
                case "Task":
                    Console.Write("Enter task id\n");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    s_dal.Task?.Delete(id);
                    break;

                case "Engineer":
                    Console.Write("Enter task id\n");
                    if (!int.TryParse(Console.ReadLine(), out int id1))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    s_dal.Engineer?.Delete(id1);
                    break;

                case "Dependency":
                    Console.Write("Enter task id\n");
                    if (!int.TryParse(Console.ReadLine(), out int id2))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    s_dal.Dependency?.Delete(id2);
                    break;
            }
        }

        /// <summary>
        /// Ask the user to enter Task's details and receive them.
        /// </summary>
        /// <returns>A new Task object with user-entered details.</returns>
        static DO.Task GetTaskItem()
        {
            Console.WriteLine("create new task item\n");
            Console.Write("Enter task id\n");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("please enter only int type\n");
            }

            Console.Write("enter task alias\n");
            string? alias = Console.ReadLine();

            Console.Write("enter task description\n");
            string? description = Console.ReadLine();

            DateTime createdAtDate = DateTime.Now;

            Console.Write("enter task complexity\n 1=Beginner\n 2=AdvancedBeginner\n 3=Intermidate\n" +
                "4=Advanced\n 5=Expert\n");
            if (!int.TryParse(Console.ReadLine(), out int num))
            {
                Console.WriteLine("please enter only int type\n");
            }

            DO.EngineerExperience complexity;
            if (num == 1)
            {
                complexity = EngineerExperience.Beginner;
            }
            else if (num == 2)
            {
                complexity = EngineerExperience.AdvancedBeginner;
            }
            else if (num == 3)
            {
                complexity = EngineerExperience.Intermidate;
            }
            else if (num == 4)
            {
                complexity = EngineerExperience.Advanced;
            }
            else if (num == 5)
            {
                complexity = EngineerExperience.Expert;
            }
            else
            {
                complexity = EngineerExperience.Beginner;
            }

            DO.Task item = new DO.Task(id, alias, description, false, createdAtDate, complexity);
            return item;
        }

        /// <summary>
        /// Ask the user to enter Engineer's details and receive them.
        /// </summary>
        /// <returns>A new Engineer object with user-entered details.</returns>
        static DO.Engineer GetEngineerItem()
        {
            Console.WriteLine("create new Engineer item\n");

            Console.Write("Enter engineer id\n");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("please enter only int type\n");
            }

            Console.Write("enter engineer email\n");
            string? email = Console.ReadLine();

            Console.Write("Enter engineer cost\n");
            if (!double.TryParse(Console.ReadLine(), out double cost))
            {
                Console.WriteLine("please enter only int type\n");
            }

            Console.Write("enter engineer name\n");
            string? name = Console.ReadLine();

            EngineerExperience? level;

            if (cost <= 400)
            {
                level = EngineerExperience.Beginner;
            }
            else if (cost <= 500 && cost > 400)
            {
                level = EngineerExperience.AdvancedBeginner;
            }
            else if (cost <= 600 && cost > 500)
            {
                level = EngineerExperience.Intermidate;
            }
            else if (cost <= 700 && cost > 600)
            {
                level = EngineerExperience.Advanced;
            }
            else if (cost <= 800 && cost > 700)
            {
                level = EngineerExperience.Expert;
            }
            else
            {
                level = null;
            }

            DO.Engineer item = new DO.Engineer(id, email, cost, name,level);
            return item;
        }

        /// <summary>
        /// Ask the user to enter Dependency's details and receive them.
        /// </summary>
        /// <returns>A new Dependency object with user-entered details.</returns>
        static DO.Dependency GetDependencyItem()
        {
            Console.Write("Enter task id\n");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("please enter only int type\n");
            }

            Console.Write("enter Dependency DependentTask\n");
            if (!int.TryParse(Console.ReadLine(), out int DependentTask))
            {
                Console.WriteLine("please enter only int type\n");
            }

            Console.Write("enter Dependency DependsOnTask\n");
            if (!int.TryParse(Console.ReadLine(), out int DependsOnTask))
            {
                Console.WriteLine("please enter only int type\n");
            }

            DO.Dependency item = new DO.Dependency(id, DependentTask, DependsOnTask);
            return item;
        }

    }
}
