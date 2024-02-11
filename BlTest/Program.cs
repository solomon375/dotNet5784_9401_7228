/// <summary>
/// Test Program for BL (Business Logic) functionality.
/// </summary>
/// <remarks>
/// The program allows manual testing of the main functionalities provided by the BL layer.
/// The supported entities are Task and Engineer.
/// </remarks>
using BO;
using DalApi;
using System;
using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace BlTest
{
    /// <summary>
    /// The instance of the BL interface used throughout the program.
    /// </summary>
    internal class Program
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// The main entry point of the program.
        /// </summary>
        static void Main()
        {
            Console.WriteLine("Welcome to BL Test Program!");
            Console.WriteLine("Would you like to create Initial data? (Y/N)");

            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans?.ToUpper() == "Y")
            {
                DalTest.Initialization.Do();
            }

            MainMenu();
        }

        /// <summary>
        /// Displays the main menu and handles user input for entity testing.
        /// </summary>
        static void MainMenu()
        {
            bool exit = false;

            do
            {
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. Task");
                Console.WriteLine("2. Engineer");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "0":
                        exit = true;
                        break;

                    case "1":
                        TestEntity<BO.Task>();
                        break;

                    case "2":
                        TestEntity<BO.Engineer>();
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

            } while (!exit);
        }
        /// <summary>
        /// Displays the sub-menu for testing operations on a specific entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to test.</typeparam>
        static void TestEntity<T>() where T : class
        {
            bool exit = false;

            do
            {
                Console.WriteLine($"\nTesting {typeof(T).Name}:");

                Console.WriteLine("1. Read");
                Console.WriteLine("2. ReadAll");
                Console.WriteLine("3. Create");
                Console.WriteLine("4. Update");
                Console.WriteLine("5. Delete");
                Console.WriteLine("6. Update Or Add Start Date (for Task)");
                Console.WriteLine("0. Back to Main Menu");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        TestRead<T>();
                        break;

                    case "2":
                        TestReadAll<T>();
                        break;

                    case "3":
                        TestCreate<T>();
                        break;

                    case "4":
                        TestUpdate<T>();
                        break;

                    case "5":
                        TestDelete<T>();
                        break;

                    case "6":
                        TestUpdataOrAddDate<T>();
                        break;

                    case "0":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

            } while (!exit);
        }
        /// <summary>
        /// Tests the Read operation for a specified entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to test.</typeparam>
        static void TestRead<T>() where T : class
        {
            if (typeof(T) == typeof(BO.Task))
            {
                Console.Write($"\n enter Task id");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("please enter only int type\n");
                }
                Console.WriteLine(s_bl.task?.Read(id));
            }

            if (typeof(T) == typeof(BO.Engineer))
            {
                Console.Write($"\n enter Engineer id");

                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("please enter only int type\n");
                }
                Console.WriteLine(s_bl.engineer.Read(id));
            }
        }
        /// <summary>
        /// Tests the ReadAll operation for a specified entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to test.</typeparam>
        static void TestReadAll<T>() where T : class
        {
            if (typeof(T) == typeof(BO.Task))
            {
                foreach (var item in s_bl.task.ReadAll())
                {
                    Console.WriteLine(item);
                }
            }

            if (typeof(T) == typeof(BO.Engineer))
            {
                foreach (var item in s_bl.engineer.ReadAll())
                {
                    Console.WriteLine(item);
                }
            }
        }
        /// <summary>
        /// Tests the Create operation for a specified entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to test.</typeparam>
        static void TestCreate<T>() where T : class
        {
            if (typeof(T) == typeof(BO.Task))
            {
                Console.WriteLine(s_bl.task.Create(GetTaskItem()));
            }

            if (typeof(T) == typeof(BO.Engineer))
            {
                Console.WriteLine(s_bl.engineer.Create(GetEngineerItem()));
            }
        }
        /// <summary>
        /// Tests the update operation for a specified entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to test.</typeparam>
        static void TestUpdate<T>() where T : class
        {
            if (typeof(T) == typeof(BO.Task))
            {
                Console.WriteLine("Enter Task ID\n");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("please enter only int type\n");
                }
                Console.WriteLine(s_bl.task?.Read(id));

                Console.WriteLine("Enter new values and the same id\n");
                //BO.Task item = GetTaskItem();
                s_bl.task?.Update(GetTaskItem());
            }

            if (typeof(T) == typeof(BO.Engineer))
            {
                Console.WriteLine("Enter Engineer ID\n");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("please enter only int type\n");
                }
                Console.WriteLine(s_bl.engineer?.Read(id));

                Console.WriteLine("Enter new values and the same id\n");
                //BO.Engineer item = GetEngineerItem();
                s_bl.engineer?.Update(GetEngineerItem());
            }
        }
        /// <summary>
        /// Tests the delete operation for a specified entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to test.</typeparam>
        static void TestDelete<T>() where T : class
        {
            if (typeof(T) == typeof(BO.Task))
            {
                Console.Write("Enter task id\n");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("please enter only int type\n");
                }
                s_bl.task?.Delete(id);
            }

            if (typeof(T) == typeof(BO.Engineer))
            {
                Console.Write("Enter Engineer id\n");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("please enter only int type\n");
                }
                s_bl.engineer?.Delete(id);
            }
        }
        /// <summary>
        /// Tests the update or add time operation for a specified entity.
        /// </summary>
        /// <typeparam name="T">The type of entity to test.</typeparam>
        static void TestUpdataOrAddDate<T>() where T : class
        {
            Console.Write($"\n enter Task id and start date");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("please enter only int type\n");
            }

            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("please enter only int type\n");
            }

            s_bl.task.UpdataOrAddDate(id,date);
        }

        /// <summary>
        /// Gets a new BO.Task item from user input.
        /// </summary>
        /// <returns>The created BO.Task item.</returns>
        private static BO.Task GetTaskItem()
        {
            BO.Task task1 = new BO.Task();

            Console.WriteLine("enter task alias");
            task1.Alias =  Console.ReadLine();

            Console.WriteLine("enter task Describtion");
            task1.Describtion =  Console.ReadLine();

            task1.CreatedAtDate = DateTime.Now;

            Console.WriteLine("enter the engineer level");
            Console.WriteLine("1. Beginner");
            Console.WriteLine("2. AdvancedBeginner");
            Console.WriteLine("3. Intermidate");
            Console.WriteLine("4. Advanced");
            Console.WriteLine("5. Expert");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    task1.Complexity = BO.EngineerExperience.Beginner;
                    break;

                case "2":
                    task1.Complexity = BO.EngineerExperience.AdvancedBeginner;
                    break;

                case "3":
                    task1.Complexity = BO.EngineerExperience.Intermidate;
                    break;

                case "4":
                    task1.Complexity = BO.EngineerExperience.Advanced;
                    break;

                case "5":
                    task1.Complexity = BO.EngineerExperience.Expert;
                    break;

                default:
                    task1.Complexity = BO.EngineerExperience.Beginner;
                    break;
            }

            task1.Dependencies = new List<TaskInList>();

            Console.WriteLine("enter the task Scheduled Date");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("please enter only datetime type\n");
            }
            task1.ScheduledDate = date;
            if(date ==  DateTime.MinValue)
            {
                task1.ScheduledDate = new DateTime(2025, 1, 1);
            }

            if(date <= DateTime.Now) { task1.status = BO.Status.Scheduled; }
            else { task1.status = BO.Status.Unscheduled; }

            task1.StartedDate = null;

            Console.WriteLine("enter the task Effort Time");
            Console.WriteLine("(default time is 7 days)\n");
            if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan time))
            {
                Console.WriteLine("please enter only timespan type\n");
            }
            task1.RequiredEffortTime = time;
            if(task1.RequiredEffortTime == (TimeSpan)TimeSpan.Zero)
            {
                task1.RequiredEffortTime = new(7, 0, 0);
            }

            task1.DeadLine = null;

            task1.CompletedDate = null;

            Console.WriteLine("enter the task Deliverable");
            task1.Deliverable = Console.ReadLine();

            Console.WriteLine("enter the task Remarks");
            task1.Remarks = Console.ReadLine();

            task1.Engineer = null;

            //Console.WriteLine(s_bl.task.Create(task1));

            return task1;
        }


        /// <summary>
        /// Gets a new BO.Engineer item from user input.
        /// </summary>
        /// <returns>The created BO.Engineer item.</returns>

        private static BO.Engineer GetEngineerItem()
        {
            BO.Engineer engineer1 = new BO.Engineer();

            Console.Write($"\n enter engineer id");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("please enter only int type\n");
            }

            engineer1.Id = id;

            Console.Write($"\n enter engineer name");

            engineer1.Name = Console.ReadLine();

            Console.Write($"\n enter engineer cost");

            Console.WriteLine("enter the engineer level");
            Console.WriteLine("Beginner cost between 0-400");
            Console.WriteLine("AdvancedBeginner cost between 400-500");
            Console.WriteLine("Intermidate cost between 500-600");
            Console.WriteLine("Advanced cost between 600-700");
            Console.WriteLine("Expert cost between 700-800");

            if (!int.TryParse(Console.ReadLine(), out int cost))
            {
                Console.WriteLine("please enter only int type\n");
            }

            engineer1.Cost = cost;

            Console.Write($"\n enter engineer email");

            engineer1.Email = Console.ReadLine();

            if (cost <= 400)
            {
                engineer1.Level = DO.EngineerExperience.Beginner;
            }
            else if (cost <= 500 && cost > 400)
            {
                engineer1.Level = DO.EngineerExperience.AdvancedBeginner;
            }
            else if (cost <= 600 && cost > 500)
            {
                engineer1.Level = DO.EngineerExperience.Intermidate;
            }
            else if (cost <= 700 && cost > 600)
            {
                engineer1.Level = DO.EngineerExperience.Advanced;
            }
            else if (cost <= 800 && cost > 700)
            {
                engineer1.Level = DO.EngineerExperience.Expert;
            }
            else
            {
                engineer1.Level = DO.EngineerExperience.Beginner;
            }

            engineer1.Task = null;

            //Console.WriteLine(s_bl.engineer.Create(engineer1));

            return engineer1;
        }
    }
}
