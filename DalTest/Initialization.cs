namespace DalTest;

using Dal;
using DalApi;
using DO;
using System.Data.Common;
using System.Security.Cryptography;
using System.Xml.Linq;

public static class Initialization
{
    private static IDal? s_dal;

    private static readonly Random s_rand = new();

    private static DateTime start = new(2024, 4, 5);

    private static void createTask()
    {
        string[] TaskAlias =
        {//Beginner:
            "A1","A2","A3","A4","A5","A6","A7","A8",
        //Advanced Beginner:
            "B1","B2","B3","B4","B5","B6",
        //Intermediate:
            "C1","C2","C3","C4",
        //Advanced:
            "D1",
        //Expert
            "E1"
        };

        string[] Taskdescription =
        {
        //Beginner:
        "Main screen and navigation.",
        "User authentication via Facebook or Google.",
        "Daily task list.",
        "Creating a new task.",
        "Task completion and marking as done.",
        "Add sensors.",
        "Notifications for upcoming tasks.",
        "Social media sharing.",
        //Advanced Beginner:
        "Adding categories to tasks.",
        "Filter by category.",
        "Taking and adding photos.",
        "Progress statistics.",
        "Sub-tasks for main tasks.",
        "Exporting and sharing reports and statistics.",
        //Intermediate:
        "Sorting and organizing by timeline.",
        "Daily reminder.",
        "Setting goals for tasks.",
        "Personal notes and ideas.",
        //Advanced
        "Completing a time management course.",
        //Expert
        "Collaboration and shared tasks."
        };

        for (int i = 1; i<=20; i++)
        {
            int _id = i;

            string _alias = TaskAlias[i-1];

            string _describtion = Taskdescription[i-1];

            EngineerExperience _Complexity;

            Status _status;

            DateTime _ScheduledDate;

            TimeSpan _RequiredEffortTime;

            DateTime _DeadLine;

            if (i >= 1 && i < 9)
            {
                _Complexity = EngineerExperience.Beginner; _status=Status.Unscheduled;
                _ScheduledDate=start; _RequiredEffortTime=new(1, 0, 0, 0); _DeadLine=start.AddDays(7); ;
            }
            else if (i >= 9 && i < 15)
            {
                _Complexity = EngineerExperience.AdvancedBeginner; _status=Status.Unscheduled;
                _ScheduledDate=start.AddDays(8); _RequiredEffortTime=new(1, 0, 0, 0); _DeadLine=start.AddDays(14);
            }
            else if (i >= 15 && i < 19)
            {
                _Complexity = EngineerExperience.Intermidate; _status=Status.Unscheduled;
                _ScheduledDate=start.AddDays(15); _RequiredEffortTime=new(2, 0, 0, 0); _DeadLine=start.AddDays(21);
            }
            else if (i == 19)
            {
                _Complexity = EngineerExperience.Advanced; _status=Status.Unscheduled;
                _ScheduledDate=start.AddDays(22); _RequiredEffortTime=new(3, 0, 0, 0); _DeadLine=start.AddDays(28);
            }
            else if (i == 20)
            {
                _Complexity = EngineerExperience.Expert; _status=Status.Unscheduled;
                _ScheduledDate=start.AddDays(29); _RequiredEffortTime=new(5, 0, 0, 0); _DeadLine=start.AddDays(35);
            }
            else
            {
                _Complexity = EngineerExperience.Beginner; _status=Status.Unscheduled;
                _ScheduledDate=start; _RequiredEffortTime=new(1, 0, 0, 0); _DeadLine=start.AddDays(7);
            }

            DateTime _Startdate = _ScheduledDate;

            DateTime _CreatedAtDate = DateTime.Now;

            Task task = new(_id, _alias, _describtion, _Complexity, _status, _ScheduledDate, _RequiredEffortTime, _DeadLine, _CreatedAtDate, _Startdate);

            s_dal?.Task!.Create(task);
        }
    }

    private static void createEngineer()
    {
        string[] engineerNames =
        {
        "Dani Levi", "Eli Amar", "Yair Cohen",
        "Ariela Levin", "Dina Klein"
        };

        int counter = 0;
        foreach (var _name in engineerNames)
        {
            int _id;
            do
                _id = s_rand.Next(100000, 1000000);
            while (s_dal?.Engineer?.Read(_id) != null);

            string _email = _name.Substring(0, _name.IndexOf(" "));
            _email = _email+"@jct.com";

            int _cost = 35 + (counter * 20);

            EngineerExperience? _level;

            if (_cost <= 50)
            { _level = EngineerExperience.Beginner; }
            else if (_cost <= 70 && _cost > 50)
            { _level = EngineerExperience.AdvancedBeginner; }
            else if (_cost <= 90 && _cost > 70)
            { _level = EngineerExperience.Intermidate; }
            else if (_cost <= 100 && _cost > 90)
            { _level = EngineerExperience.Advanced; }
            else if (_cost <= 150 && _cost > 100)
            { _level = EngineerExperience.Expert; }
            else
            { _level = null; }

            counter++;

            Engineer engineer = new(_id, _email, _cost, _name, _level);

            s_dal?.Engineer!.Create(engineer);
        }
    }

    private static void createDependency()
    {
        int[] DependentTask =
        {
            14, 16, 20, 10, 18, 13, 11, 19, 12, 15, 17, 09, 19, 14, 13, 10, 11, 16, 12, 20, 15,
            13, 14, 18, 11, 17, 15, 19, 13, 16, 18, 10, 18, 09, 20, 12, 17, 15, 13, 16, 10, 15
        };

        int[] DependsOnTask =
        {
            06, 14, 18, 03, 01, 06, 04, 02, 07, 11, 05, 07, 16, 08, 07, 04, 02, 05, 01, 17, 10,
            08, 02, 08, 03, 13, 08, 17, 01, 04, 13, 05, 02, 01, 02, 02, 07, 05, 03, 08, 02, 08
        };

        for (int i = 1; i<=40; i++)
        {
            int id = i;
            int dependentTask = DependentTask[i];
            int dependsOnTask = DependsOnTask[i];

            Dependency dependency = new(id, dependentTask, dependsOnTask);

            s_dal?.Dependency!.Create(dependency);
        }
    }

    public static void Do()
    {
        s_dal = DalApi.Factory.Get;

        createTask();
        createEngineer();
        createDependency();
    }

    public static void Resat()
    {
        s_dal = DalApi.Factory.Get;


        //if int the sile dal-config at line 3 is list then
        //אם עובדים עם המפעל של רשימה
        //dalList
        List<int> dIds = new List<int>();
        foreach (var i in s_dal.Task.ReadAll()) { dIds.Add(i.Id); }
        foreach (var i in dIds) { s_dal.Task.Delete(i); }
        dIds.Clear();
        foreach (var i in s_dal.Engineer.ReadAll()) { dIds.Add(i.Id); }
        foreach (var i in dIds) { s_dal.Engineer.Delete(i); }
        dIds.Clear();
        foreach (var i in s_dal.Dependency.ReadAll()) { dIds.Add(i.Id); }
        foreach (var i in dIds) { s_dal.Dependency.Delete(i); }

        /*
        //if int the sile dal-config at line 3 is xml then
        //אם עובדים עם המפעל של קובץ אקס אמ אל
        //dalXml
        int num;
        foreach (var i in s_dal.Task.ReadAll()) { num = i.Id; s_dal.Task.Delete(num); }
        foreach (var i in s_dal.Engineer.ReadAll()) { num = i.Id; s_dal.Engineer.Delete(num); }
        foreach (var i in s_dal.Dependency.ReadAll()) { num = i.Id; s_dal.Dependency.Delete(num); }*/

    }
}
