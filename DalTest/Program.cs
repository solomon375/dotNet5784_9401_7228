using Dal;
using DalApi;
using DO;
using System;
using System.Security.Cryptography;

namespace DalTest
{
    internal class Program
    {
        private static ITask? s_dalTask = new TaskImplementation();
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();

        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency);

                bool exit = false;

                do
                {
                    Console.WriteLine("enter a number:\n " +
                    "0 = exit\n 1 = task\n 2 = engineer\n 3 = dependency");

                    //choose the case depend on what user enter
                    char choice = Console.ReadKey().KeyChar;

                    switch (choice)
                    {
                        case '1':
                            checkTask();
                            break;

                        case '2':
                            checkEngineer();
                            break;

                        case '3':
                            checkDependency();
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
        //check the Task.cs
        static void checkTask()
        {
            bool exit = false;
            do
            {
                display();

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

        //check the Engineer.cs
        static void checkEngineer()
        {
            bool exit = false;
            do
            {
                display();

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

        //check the Dependency.cs
        static void checkDependency()
        {
            bool exit = false;
            do
            {
                display();

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

        //print the menu for the user to choose what to do next
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

        //do something depend on what the user chose
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

        //send the user to the right Create method
        static void Create(string s)
        {
            switch (s)
            {
                case "Task":
                    DO.Task item = GetTaskItem();
                    Console.WriteLine(s_dalTask?.Create(item));
                    break;

                case "Engineer":
                    DO.Engineer item1 = GetEngineerItem();
                    Console.WriteLine(s_dalEngineer?.Create(item1));
                    break;

                case "dependency":
                    DO.Dependency item2 = GetDependencyItem();
                    Console.WriteLine(s_dalDependency?.Create(item2));
                    break;
            }
            //send the user to the right create method
        }

        //send the user to the right Read method
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
                    Console.WriteLine(s_dalTask?.Read(id));
                    break;

                case "Engineer":
                    Console.Write("Enter task id\n");
                    if (!int.TryParse(Console.ReadLine(), out int id1))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dalEngineer?.Read(id1));
                    break;

                case "dependency":
                    Console.Write("Enter task id\n");
                    if (!int.TryParse(Console.ReadLine(), out int id2))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dalDependency?.Read(id2));
                    break;
            }
        }

        //send the user to the right ReadAll method
        static void ReadAll(string s)
        {
            switch (s)
            {
                case "Task":

                    foreach (var item in s_dalTask?.ReadAll())
                    {
                        Console.WriteLine(item);
                    }
                    break;

                case "Engineer":
                    foreach (var item in s_dalEngineer?.ReadAll())
                    {
                        Console.WriteLine(item);
                    }
                    break;

                case "dependency":
                    foreach (var item in s_dalDependency?.ReadAll())
                    {
                        Console.WriteLine(item);
                    }
                    break;
            }
        }

        //send the user to the right Update method
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
                    Console.WriteLine(s_dalTask?.Read(id));

                    Console.WriteLine("Enter new values and the same id\n");
                    DO.Task item = GetTaskItem();
                    s_dalTask?.Update(item);
                    break;

                case "Engineer":
                    Console.WriteLine("Enter engineer ID\n");
                    if (!int.TryParse(Console.ReadLine(), out int id1))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dalEngineer?.Read(id1));

                    Console.WriteLine("Enter new values and the same id\n");
                    DO.Engineer item1 = GetEngineerItem();
                    s_dalEngineer?.Update(item1);
                    break;

                case "dependency":
                    Console.WriteLine("Enter Dependency ID\n");
                    if (!int.TryParse(Console.ReadLine(), out int id2))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dalDependency?.Read(id2));

                    Console.WriteLine("Enter new values and the same id\n");
                    DO.Dependency item2 = GetDependencyItem();
                    s_dalDependency?.Update(item2);
                    break;
            }
        }

        //send the user to the right Delete method
        static void Delete(string s)
        {
            switch (s)
            {
                case "Task":
                    DO.Task item = GetTaskItem();
                    int ID = item.Id;
                    s_dalTask?.Delete(ID);
                    break;

                case "Engineer":
                    DO.Engineer item1 = GetEngineerItem();
                    int ID1 = item1.Id;
                    s_dalEngineer?.Delete(ID1);
                    break;

                case "dependency":
                    DO.Dependency item2 = GetDependencyItem();
                    int ID2 = item2.Id;
                    s_dalDependency?.Delete(ID2);
                    break;
            }
        }

        //ask from the user to enter Task's details and recive them
        static DO.Task GetTaskItem()
        {
            Console.WriteLine("create new task item\n");

            Console.Write("enter task alias\n");
            string alias = Console.ReadLine();

            Console.Write("enter task description\n");
            string description = Console.ReadLine();

            DateTime createdAtDate = DateTime.Now;

            DO.Task item = new DO.Task(0, alias, description, false, createdAtDate);
            return item;
        }

        //ask from the user to enter Engineer's details and recive them
        static DO.Engineer GetEngineerItem()
        {
            Console.WriteLine("create new Engineer item\n");

            Console.Write("Enter engineer id\n");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("please enter only int type\n");
            }

            Console.Write("enter engineer email\n");
            string email = Console.ReadLine();

            Console.Write("Enter engineer id\n");
            if (!double.TryParse(Console.ReadLine(), out double cost))
            {
                Console.WriteLine("please enter only int type\n");
            }

            Console.Write("enter engineer email\n");
            string name = Console.ReadLine();

            DO.Engineer item = new DO.Engineer(id, email, cost, name);
            return item;
        }

        //ask from the user to enter Dependency's details and recive them
        static DO.Dependency GetDependencyItem()
        {
            DO.Dependency item = new DO.Dependency(0, 0, 0);
            return item;
        }
    }
}
