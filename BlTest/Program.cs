//לא הרצנו כדי לבדוק אם עובד 
//אחרי שבנינו את זה בפעם הראשונה עשינו כמה שינויים בכל הפרוייקט ואז הובץ הזה נמחק לנו
//נסינו לשחזר את זה אבל לא היה זמן להריץ ולבדוק אם זה עובד טוב
//למרות שלא צריכה להיות בעיה
/*/// <summary>
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
                Console.WriteLine(s_bl.task.Create(new BO.Task()));
            }

            if (typeof(T) == typeof(BO.Engineer))
            {
                Console.WriteLine(s_bl.engineer.Create(new BO.Engineer()));
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
                s_bl.task?.Update(new BO.Task());
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
                s_bl.engineer?.Update(new BO.Engineer());
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

            s_bl.task.UpdataOrAddDate(id, date);
        }
    }
}*/