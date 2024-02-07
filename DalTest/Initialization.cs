namespace DalTest;

using DalApi;
using DO;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Xml.Linq;

/// <summary>
/// Provides methods for initializing the data in the Data Access Layer for testing purposes.
/// </summary>
public static class Initialization
{
    private static IDal? s_dal; //stage 2

    private static readonly Random s_rand = new();

    /// <summary>
    /// Creates and initializes task entities.
    /// </summary>
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
        "Assemble the robot's frame.",
        "Install the motors.",
        "Connect the wheels.",
        "Add the battery.",
        "Program basic movements (forward, backward, left, right).",
        "Add sensors.",
        "Program sensor-based movements.",
        "Add a camera.",
    //Advanced Beginner:
        "Add a display.",
        "Add voice recognition.",
        "Add speech synthesis.",
        "Add wireless communication.",
        "Add a gyroscope.",
        "Program advanced movements.",
    //Intermediate:
        "Add machine learning.",
        "Add object recognition.",
        "Add facial recognition.",
        "Add emotion recognition.",
    //Advanced
        "Add autonomous navigation.",
    //Expert
        "Create a custom robot with advanced features according to user requirements."
    };

        int daycounter = 20;

        for (int i = 1; i <= 20; i++)
        {

            int _id = i;

            string _Alias = TaskAlias[i-1];

            string _Describtion = Taskdescription[i-1];

            bool _IsMilestone = false;

            DateTime _CreatedAtDate = DateTime.Now;

            TimeSpan _RequiredEffortTime = TimeSpan.Zero;

            DateTime _DeadLine = DateTime.Now;

            //DateTime _CreatedAtDate = currentTime.AddDays((-daycounter)*2);//before datetime.now

            DO.EngineerExperience _Complexity;

            if (i < 9 && i >= 1)
            {
                _Complexity = EngineerExperience.Beginner;

                _CreatedAtDate = new DateTime(2025, 5, 1);

                _DeadLine = new DateTime(2025, 5, 29);

                _RequiredEffortTime = new(7, 0, 0);
            }
            else if (i < 15 && i >= 9)
            {
                _Complexity = EngineerExperience.AdvancedBeginner;

                _CreatedAtDate = new DateTime(2025, 6, 1);

                _DeadLine = new DateTime(2025, 6, 29);

                _RequiredEffortTime = new(7, 0, 0);
            }
            else if (i < 19 && i >= 15)
            {
                _Complexity = EngineerExperience.Intermidate;

                _CreatedAtDate = new DateTime(2025, 7, 1);

                _DeadLine = new DateTime(2025, 7, 29);

                _RequiredEffortTime = new(10, 0, 0);
            }
            else if (i < 20 && i >= 19)
            {
                _Complexity = EngineerExperience.Advanced;

                _CreatedAtDate = new DateTime(2025, 8, 1);

                _DeadLine = new DateTime(2025, 8, 29);

                _RequiredEffortTime = new(15, 0, 0);
            }
            else if (i <= 20)
            {
                _Complexity = EngineerExperience.Expert;

                _CreatedAtDate = new DateTime(2025, 9, 1);

                _DeadLine = new DateTime(2025, 9, 29);

                _RequiredEffortTime = new(25, 0, 0);
            }
            else
            {
                _Complexity = EngineerExperience.Beginner;

                _CreatedAtDate = new DateTime(2025, 5, 1);

                _DeadLine = new DateTime(2025, 5, 29);

                _RequiredEffortTime = new(7, 0, 0);
            }

            daycounter--;

            Task newStu = new(_id, _Alias, _Describtion, _IsMilestone, _CreatedAtDate, _RequiredEffortTime,
                _DeadLine ,_Complexity);

            s_dal!.Task.Create(newStu);
        }
    }

    /// <summary>
    /// Creates and initializes engineer entities.
    /// </summary>
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
                _id = s_rand.Next(100000,1000000 );
            while (s_dal!.Engineer.Read(_id) != null);

            string _email = _name.Substring(0, _name.IndexOf(" "));
            _email = _email+"@jct.com";

            int _cost = 350 + counter * 100;

            EngineerExperience? _level;

            if (_cost <= 400)
            {
                _level = EngineerExperience.Beginner;
            }
            else if (_cost <= 500 && _cost > 400)
            {
                _level = EngineerExperience.AdvancedBeginner;
            }
            else if (_cost <= 600 && _cost > 500)
            {
                _level = EngineerExperience.Intermidate;
            }
            else if (_cost <= 700 && _cost > 600)
            {
                _level = EngineerExperience.Advanced;
            }
            else if (_cost <= 800 && _cost > 700)
            {
                _level = EngineerExperience.Expert;
            }
            else
            {
                _level = null;
            }

            counter++;

            Engineer newStu = new(_id, _email, _cost, _name, _level);

            s_dal!.Engineer.Create(newStu);

        }

    }
    /// <summary>
    /// Creates and initializes dependency entities.
    /// </summary>
    private static void createDependency()
    {
        for (int i = 0; i < 40; i++)
        {
            int _id = i;

            int _dependentTask = s_rand.Next(0, 20);

            int _dependsOnTask = s_rand.Next(0, _dependentTask);

            Dependency newStu = new(_id, _dependentTask, _dependsOnTask);

            s_dal!.Dependency.Create(newStu);
        }
    }

    /// <summary>
    /// Initializes the data in the Data Access Layer for testing purposes.
    /// </summary>
    public static void Do() //stage 4
    {

        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        s_dal = DalApi.Factory.Get; //stage 4
        createTask();
        createEngineer();
        createDependency();

    }
}


