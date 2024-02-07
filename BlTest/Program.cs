using DalApi;
using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace BlTest
{
    internal class Program
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        
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
        static void TestReadAll<T>() where T : class
        {
            if (typeof(T) == typeof(BO.Task))
            {
                foreach (var item in s_bl.task.ReadAll())
                {
                    Console.WriteLine($"task id = {item.Id}, task alias = {item.Alias}, " +
                        $"task Description = {item.Description}, task status = {item.Status}");
                }
            }

            if (typeof(T) == typeof(BO.Engineer))
            {
                foreach (var item in s_bl.engineer.ReadAll())
                {
                    Console.WriteLine(item.Name);
                }
            }
        }
        static void TestCreate<T>() where T : class
        {
            if (typeof(T) == typeof(BO.Task))
            {
                BO.Task item = GetTaskItem();
                Console.WriteLine(s_bl.task?.Create(item));
            }

            if (typeof(T) == typeof(BO.Engineer))
            {
                BO.Engineer item1 = GetEngineerItem();
                Console.WriteLine(s_bl.engineer?.Create(item1));
            }
        }

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
                BO.Task item = GetTaskItem();
                s_bl.task?.Update(item);
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
                BO.Engineer item = GetEngineerItem();
                s_bl.engineer?.Update(item);
            }
        }
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

            Console.WriteLine("enter the task Scheduled Date");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("please enter only datetime type\n");
            }
            task1.ScheduledDate = date;

            if(date <= DateTime.Now) { task1.status = BO.Status.Scheduled; }
            else { task1.status = BO.Status.Unscheduled; }

            task1.StartedDate = null;

            Console.WriteLine("enter the task Effort Time");
            task1.RequiredEffortTime = new(7, 0, 0);
            Console.WriteLine("(default time is 7 days)\n");
            if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan time))
            {
                Console.WriteLine("please enter only timespan type\n");
            }
            task1.RequiredEffortTime = time;

            task1.DeadLine = null;

            task1.CompletedDate = null;

            Console.WriteLine("enter the task Deliverable");
            task1.Deliverable = Console.ReadLine();

            Console.WriteLine("enter the task Remarks");
            task1.Remarks = Console.ReadLine();

            task1.Engineer = null;

            s_bl.task.Create(task1);

            return task1;
        }



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

            if (!int.TryParse(Console.ReadLine(), out int cost))
            {
                Console.WriteLine("please enter only int type\n");
            }

            engineer1.Cost = cost;

            Console.Write($"\n enter engineer email");

            engineer1.Email = Console.ReadLine();

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
                    engineer1.Level = DO.EngineerExperience.Beginner;
                    break;

                case "2":
                    engineer1.Level = DO.EngineerExperience.AdvancedBeginner;
                    break;

                case "3":
                    engineer1.Level = DO.EngineerExperience.Intermidate;
                    break;

                case "4":
                    engineer1.Level = DO.EngineerExperience.Advanced;
                    break;

                case "5":
                    engineer1.Level = DO.EngineerExperience.Expert;
                    break;

                default:
                    engineer1.Level = DO.EngineerExperience.Beginner;
                    break;
            }

            engineer1.Task = null;

            s_bl.engineer.Create(engineer1);

            return engineer1;
        }
    }
}
