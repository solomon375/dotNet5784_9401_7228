using Dal;
using DalApi;
using DO;
using System;
using System.Security.Cryptography;
using System.Threading.Channels;

namespace DalTest
{
    internal class Program
    {
        private static DateTime start = new(15/3/24);//משתנה גלובלי שמקבל את הזמן של תחילת התוכנית

        static readonly IDal s_dal = DalApi.Factory.Get;

        static void Main(string[] args)
        {
            try
            {
                string? ans;

                //if in the sile dal-config at line 3 is list then do:
                Initialization.Do();

                //if in the sile dal-config at line 3 is xml then do:
                //Console.WriteLine("\nif you starting now prees y");
                /*Console.WriteLine("\nWould you like to create Initial data? (Y/N)");
                ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                if (ans?.ToUpper() == "Y")
                {
                    Initialization.Do();
                }*/

                bool exit = false;
                
                do
                {
                    Console.WriteLine("enter a number:\n " +
                    "0 = exit\n 1 = task\n 2 = engineer\n 3 = dependency\n 4 = Initializ");

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
                        case '4':
                            Console.WriteLine("\nWould you like to create Initial data? (Y/N)");
                            Console.WriteLine("(the data already Initializ)");
                                ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                            if (ans?.ToUpper() == "Y")
                            {
                                Initialization.Resat();
                                Initialization.Do();
                            }
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
        static void display()
        {
            Console.WriteLine($"\nChoose an operation:");
            Console.WriteLine("1. Create");
            Console.WriteLine("2. Read");
            Console.WriteLine("3. Read All");
            Console.WriteLine("4. Update");
            Console.WriteLine("5. Delete");
            Console.WriteLine("0. Exit");
        }
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
        static void Create(string s)
        {
            switch (s)
            {
                case "Task":
                    DO.Task item = GetTaskItem();
                    int? i = s_dal?.Task?.Create(item);
                    if (i == -1) { Console.WriteLine("the item alrady exist"); }
                    else { Console.WriteLine(i); }
                    break;

                case "Engineer":
                    DO.Engineer item1 = GetEngineerItem();
                    Console.WriteLine(s_dal?.Engineer?.Create(item1));
                    break;

                case "Dependency":
                    DO.Dependency item2 = GetDependencyItem();
                    i = s_dal?.Dependency?.Create(item2);
                    if (i == -1) { Console.WriteLine("the item alrady exist"); }
                    else { Console.WriteLine(i); }
                    Console.WriteLine();
                    break;
            }
        }
        static void Read(string s)
        {
            switch (s)
            {
                case "Task":
                    Console.WriteLine("\nEnter task id");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dal?.Task?.Read(id));
                    break;
                case "Engineer":
                    Console.WriteLine("\nEnter engineer id");
                    if (!int.TryParse(Console.ReadLine(), out int id1))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dal?.Engineer?.Read(id1));
                    break;
                case "Dependency":
                    Console.WriteLine("\nEnter dependency id");
                    if (!int.TryParse(Console.ReadLine(), out int id2))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dal?.Engineer?.Read(id2));
                    break;
            }
        }
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
        static void Update(string s)
        {
            switch (s)
            {
                case "Task":
                    Console.WriteLine("\nEnter task id");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dal?.Task?.Read(id));
                    DO.Task item = GetTaskItemForUpdate(s_dal?.Task.Read(id)!);
                    s_dal?.Task?.Update(item);
                    break;
                case "Engineer":
                    Console.WriteLine("\nEnter engineer ID");
                    if (!int.TryParse(Console.ReadLine(), out int id1))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dal?.Engineer?.Read(id1));
                    DO.Engineer item1 = GetEngineerItemForUpdate(s_dal?.Engineer.Read(id1)!);
                    s_dal?.Engineer?.Update(item1);
                    break;
                case "Dependency":
                    Console.WriteLine("\nEnter Dependency ID");
                    if (!int.TryParse(Console.ReadLine(), out int id2))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    Console.WriteLine(s_dal?.Dependency?.Read(id2));
                    DO.Dependency item2 = GetDependencyItemForUpdate(s_dal?.Dependency.Read(id2)!);
                    s_dal?.Dependency?.Update(item2);
                    break;
            }
        }
        static void Delete(string s)
        {
            switch (s)
            {
                case "Task":
                    Console.WriteLine("\nEnter task id");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    s_dal?.Task?.Delete(id);
                    break;
                case "Engineer":
                    Console.WriteLine("\nEnter engineer id");
                    if (!int.TryParse(Console.ReadLine(), out int id1))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    s_dal?.Engineer?.Delete(id1);
                    break;
                case "Dependency":
                    Console.WriteLine("\nEnter dependency id");
                    if (!int.TryParse(Console.ReadLine(), out int id2))
                    {
                        Console.WriteLine("please enter only int type\n");
                    }
                    s_dal?.Dependency?.Delete(id2);
                    break;
            }
        }
        static DO.Task GetTaskItem()
        {
            //כל פעם כאשר אנו נגשים ליצירת משימה חדשה השתנה עכשיו מקבל את הזמן הנוכחי 
            //והזמן התחלה לא מתעדכן לזמן הנוכחי אלה נשאר אותו דבר
            //בגלל שיצרתי אות עם אחת בתחילת הפרויקט ולא שיניתי אותו מאז 
            DateTime now = DateTime.Now;

            Console.Write("\nenter task alias\n");
            string? alias = Console.ReadLine();

            Console.Write("enter task description\n");
            string? description = Console.ReadLine();

            Console.Write("enter task complexity\n 1=Beginner\n 2=AdvancedBeginner\n 3=Intermidate\n 4=Advanced\n 5=Expert\n");
            if (!int.TryParse(Console.ReadLine(), out int num))
            {
                Console.WriteLine("please enter only int type\n");
            }
            DO.EngineerExperience complexity;
            if (num == 1)
            { complexity = EngineerExperience.Beginner; }
            else if (num == 2)
            { complexity = EngineerExperience.AdvancedBeginner; }
            else if (num == 3)
            { complexity = EngineerExperience.Intermidate; }
            else if (num == 4)
            { complexity = EngineerExperience.Advanced; }
            else if (num == 5)
            { complexity = EngineerExperience.Expert; }
            else
            { complexity = EngineerExperience.Beginner; }

            DO.Status status = Status.Unscheduled;

            //אם אנחנו מנסים להכניס משימה ברמה מסוימת כאשר אנו עוברים כרגע על רמה אחרת
            //אם רמת המשימה שאנחנו רוצים להכניס נמוכה מרמת המשימות שאנחנו עובדים עליהם כרגע
            //אנו עושים שהתאריך מתוכנן יהיה עכשיו והתאריך סיום יהיה חודש מעכשיו
            //אם רמת המשימה שאנחנו רוצים להכניס גבוהה מרמת המשימות שאנחנו עובדים עליהם כרגע
            //אנו עושים שהתאריך מתוכנן והתאריך סיום של המשימה יהיו כמו של כל המשימות עבור הרמה של המשימה הנכנסת
            //לא רשלוונתי עבור שלב 1
            DateTime ScheduledDate;
            DateTime DeadLine;
            if (complexity == EngineerExperience.Beginner)
            { ScheduledDate = DateTime.Now; DeadLine = DateTime.Now.AddMonths(1); }
            else if (complexity == EngineerExperience.AdvancedBeginner)
            { if(now<=start.AddMonths(1)) { 
                ScheduledDate = DateTime.Now.AddMonths(1); DeadLine = DateTime.Now.AddMonths(2); }
            else{ ScheduledDate = DateTime.Now; DeadLine = DateTime.Now.AddMonths(1); } }
            else if (complexity == EngineerExperience.Intermidate)
            { if(now<=start.AddMonths(1)) { 
                ScheduledDate = DateTime.Now.AddMonths(2); DeadLine = DateTime.Now.AddMonths(3); }
            else{ ScheduledDate = DateTime.Now; DeadLine = DateTime.Now.AddMonths(1); } }
            else if (complexity == EngineerExperience.Advanced)
            { if(now<=start.AddMonths(1)) { 
                ScheduledDate = DateTime.Now.AddMonths(3); DeadLine = DateTime.Now.AddMonths(4); }
            else{ ScheduledDate = DateTime.Now; DeadLine = DateTime.Now.AddMonths(1); } }
            else if (complexity == EngineerExperience.Expert)
            { if(now<=start.AddMonths(1)) { 
                ScheduledDate = DateTime.Now.AddMonths(4); DeadLine = DateTime.Now.AddMonths(5); }
            else{ ScheduledDate = DateTime.Now; DeadLine = DateTime.Now.AddMonths(1); } }
            else
            { ScheduledDate = DateTime.Now; DeadLine = DateTime.Now.AddMonths(1); }

            DateTime _CreatedAtDate = DateTime.Now;

            DateTime? StartedDate = null;

            DateTime? CompletedDate = null;

            TimeSpan requiredEffortTime = TimeSpan.Zero;
            Console.Write("enter task required effort time (in days)\n");
            if (!int.TryParse(Console.ReadLine(), out int time))
            {
                Console.WriteLine("the Required Effort Time is 7 days\n");
            }
            requiredEffortTime = new(time, 0, 0, 0);
            if (requiredEffortTime ==  TimeSpan.Zero) { requiredEffortTime = new(7, 0, 0, 0); }

            Console.Write("enter task deliverable\n");
            string? Deliverable = Console.ReadLine();

            Console.Write("enter task remarks\n");
            string? Remarks = Console.ReadLine();

            DO.Task task = new DO.Task(0,alias,description,complexity,status,
                ScheduledDate,requiredEffortTime,DeadLine, _CreatedAtDate, StartedDate, CompletedDate,Deliverable,Remarks);

            return task;
        }
        static DO.Task GetTaskItemForUpdate(DO.Task item)
        {
            Console.Write("\nenter task alias\n");
            string? alias = Console.ReadLine();

            Console.Write("enter task description\n");
            string? description = Console.ReadLine();

            TimeSpan requiredEffortTime = TimeSpan.Zero;
            Console.Write("enter task required effort time (in days)\n");
            if (!int.TryParse(Console.ReadLine(), out int time))
            {
                Console.WriteLine("the Required Effort Time is 7 days\n");
            }
            requiredEffortTime = new(time, 0, 0, 0);
            if (requiredEffortTime ==  TimeSpan.Zero) { requiredEffortTime = new(7, 0, 0, 0); }

            Console.Write("enter task deliverable\n");
            string? deliverable = Console.ReadLine();

            Console.Write("enter task remarks\n");
            string? remarks = Console.ReadLine();

            return item with { Alias = alias, Describtion = description,
                RequiredEffortTime=requiredEffortTime, Deliverable=deliverable, Remarks=remarks };
        }
        static DO.Engineer GetEngineerItem()
        {
            Console.Write("\nEnter engineer id\n");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("please enter only int type\n");
            }

            Console.Write("enter engineer email\n");
            string? email = Console.ReadLine();

            Console.Write("Enter engineer cost betueen 20 to 150\n");
            if (!double.TryParse(Console.ReadLine(), out double cost))
            {
                Console.WriteLine("please enter only int type\n");
            }

            Console.Write("enter engineer name\n");
            string? name = Console.ReadLine();

            EngineerExperience? level;
            if (cost <= 50)
            { level = EngineerExperience.Beginner; }
            else if (cost <= 70 && cost > 50)
            { level = EngineerExperience.AdvancedBeginner; }
            else if (cost <= 90 && cost > 70)
            { level = EngineerExperience.Intermidate; }
            else if (cost <= 100 && cost > 90)
            { level = EngineerExperience.Advanced; }
            else if (cost <= 150 && cost > 100)
            { level = EngineerExperience.Expert; }
            else
            { level = null; }

            DO.Engineer item = new DO.Engineer(id, email, cost, name, level);
            return item;
        }
        static DO.Engineer GetEngineerItemForUpdate(DO.Engineer item)
        {
            Console.Write("\nenter engineer email\n");
            string? email = Console.ReadLine();

            Console.Write("Enter engineer cost betueen 20 to 150\n");
            if (!double.TryParse(Console.ReadLine(), out double cost))
            {
                Console.WriteLine($"the cost is{item.Cost}\n");
            }

            Console.Write("enter engineer name\n");
            string? name = Console.ReadLine();

            EngineerExperience? level;
            if (cost <= 50)
            { level = EngineerExperience.Beginner; }
            else if (cost <= 70 && cost > 50)
            { level = EngineerExperience.AdvancedBeginner; }
            else if (cost <= 90 && cost > 70)
            { level = EngineerExperience.Intermidate; }
            else if (cost <= 100 && cost > 90)
            { level = EngineerExperience.Advanced; }
            else if (cost <= 150 && cost > 100)
            { level = EngineerExperience.Expert; }
            else
            { level = null; }

            return item with { Cost = cost, Email=email, Name=name,Level=level };
        }
        static DO.Dependency GetDependencyItem()
        {
            Console.Write("\nenter Dependency DependentTask\n");
            if (!int.TryParse(Console.ReadLine(), out int DependentTask))
            {
                Console.WriteLine("please enter only int type\n");
            }

            Console.Write("\nenter Dependency DependsOnTask\n");
            if (!int.TryParse(Console.ReadLine(), out int DependsOnTask))
            {
                Console.WriteLine("please enter only int type\n");
            }

            DO.Dependency item = new DO.Dependency(0, DependentTask, DependsOnTask);
            return item;
        }
        static DO.Dependency GetDependencyItemForUpdate(DO.Dependency item)
        {
            Console.Write("\nenter Dependency DependentTask\n");
            if (!int.TryParse(Console.ReadLine(), out int DependentTask))
            {
                Console.WriteLine("please enter only int type\n");
            }

            Console.Write("\nenter Dependency DependsOnTask\n");
            if (!int.TryParse(Console.ReadLine(), out int DependsOnTask))
            {
                Console.WriteLine("please enter only int type\n");
            }

            return item with { DependentTask = DependentTask,DependsOnTask=DependsOnTask };
        }
    }
}