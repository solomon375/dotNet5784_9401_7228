using Dal;
using DalApi;
using DalList;
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
            Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency);

            bool exit = false;

            do
            {
                Console.WriteLine("enter a number:\n " +
                "0 = exit\n 1 = task\n 2 = engineer\n 3 = dependency");

                char choice = Console.ReadKey().KeyChar;

                switch (choice)
                {
                    case '1':
                        chackTask();
                        break;

                    case '2':
                        chackEngineer();
                        break;

                    case '3':
                        chackDependency();
                        break;

                    case '0':
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            } while (!exit);
        }
        static void chackTask()
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
                        Choice(choice , "Task");
                        break;
                }
            } while (!exit);
        }

        static void chackEngineer()
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
        static void chackDependency()
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
        
        static void display()
        {
            Console.WriteLine($"Choose an operation:");
            Console.WriteLine("1. Create");
            Console.WriteLine("2. Read");
            Console.WriteLine("3. Read All");
            Console.WriteLine("4. Update");
            Console.WriteLine("5. Delete");
            Console.WriteLine("0. Exit");
        }

        static void Choice(char c,string s)
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

                default :
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        static void Create(string s)
        {
            switch(s)
            {
                case "Task":
                    
                    break;

                case "Engineer":
                    break;

                case "dependency":
                    break;
            }
        }
        static void Read(string s)
        {
            switch (s)
            {
                case "Task":
                    break;

                case "Engineer":
                    break;

                case "dependency":
                    break;
            }
        }
        static void ReadAll(string s)
        {
            switch (s)
            {
                case "Task":
                    break;

                case "Engineer":
                    break;

                case "dependency":
                    break;
            }
        }
        static void Update(string s)
        {
            switch (s)
            {
                case "Task":
                    break;

                case "Engineer":
                    break;

                case "dependency":
                    break;
            }
        }
        static void Delete(string s)
        {
            switch (s)
            {
                case "Task":
                    break;

                case "Engineer":
                    break;

                case "dependency":
                    break;
            }
        }
    }
}
