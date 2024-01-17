namespace DalTest;

using DalApi;
using DO;
using System.Security.Cryptography;
using System.Xml.Linq;

public static class Initialization
{
    private static IDal? s_dal; //stage 2

    private static readonly Random s_rand = new();

    //method for create task
    private static void createTask()
    {

        string[] TaskAlias =
    {//Expert
        "E4","E3","E2","E1",
    //Advanced:
        "D4","D3","D2","D1",
    //Intermediate:
        "C4","C3","C2","C1",
    //Advanced Beginner:
        "B4","B3","B2","B1",
    //Beginner:
        "A4","A3","A2","A1"
    };
        string[] Taskdescription =
    {//Expert
        "Chatbot Interaction: Engage in natural conversations and interactions.",
        "Natural Language Processing: Understand and generate human language.",
        "Neural Network Integration: Use complex algorithms for intelligent tasks.",
        "Machine Learning: Implement learning capabilities from experience or data.",
    //Advanced
        "Voice Recognition: Enable the robot to understand and respond to voice commands.",
        "Facial Recognition: Recognize and respond based on identified faces.",
        "Navigation with GPS and Compass: Make the robot navigate or follow paths.",
        "Balance and Motion: Use gyroscope/accelerometer for balancing or tricks.",
    //Intermediate:
        "Gripper Control: Control a gripper to pick and drop objects.",
        "Camera Functionality: Capture and stream video or store it.",
        "Audio I/O: Integrate speaker and microphone for sound playback and recording.",
        "Bluetooth Communication: Establish communication between robot and external devices.",
    //Advanced Beginner:
        "Temperature Display: Display temperature using a sensor, either on a screen or serial monitor.",
        "Light Sensing Behavior: Program robot to move towards or away from light.",
        "Line Following Capability: Enable the robot to track and follow lines on a surface.",
        "Obstacle Detection: Implement an ultrasonic sensor to stop on detecting obstacles.",
    //Beginner:
        "Button Control and Speed Regulation: Use push button & potentiometer to control movement.",
        "Sound and Light Indicators: Integrate buzzer, LED, and create indicators for movement.",
        "Power Management: Add battery, switch, ensure functionality.",
        "Basic Assembly and Movement: Construct robot, connect motors, and program basic movements."
    };

        for (int i = 0; i < 20; i++)
        {
            int _id = i;

            string _Alias = TaskAlias[i];

            string _Describtion = Taskdescription[i];

            bool _IsMilestone = false;

            DateTime currentTime = DateTime.Now;

            DateTime _CreatedAtDate = currentTime.AddDays((-i)*2);//before datetime.now

            DO.EngineerExperience _Complexity;

            if (i <= 3)
            {
                _Complexity = EngineerExperience.Expert;
            }
            else if (i <= 7 && i > 3)
            {
                _Complexity = EngineerExperience.Advanced;
            }
            else if (i <= 11 && i > 7)
            {
                _Complexity = EngineerExperience.Intermidate;
            }
            else if (i <= 15 && i > 11)
            {
                _Complexity = EngineerExperience.AdvancedBeginner;
            }
            else if (i <= 19 && i > 15)
            {
                _Complexity = EngineerExperience.Beginner;
            }
            else
            {
                _Complexity = EngineerExperience.Beginner;
            }

            Task newStu = new(_id, _Alias, _Describtion, _IsMilestone, null , _Complexity);

            s_dal!.Task.Create(newStu);
        }
    }
    //method for create engineer
    private static void createEngineer()
    {
        string[] engineerNames =
    {
        "Dani Levi", "Eli Amar", "Yair Cohen",
        "Ariela Levin", "Dina Klein"
    };
        foreach (var _name in engineerNames)
        {
            int _id;
            do
                _id = s_rand.Next(100000,1000000 );
            while (s_dal!.Engineer.Read(_id) != null);

            string _email = _name.Substring(0, _name.IndexOf(" "));
            _email = _email+"@jct.com";

            int _cost = s_rand.Next(200, 800);

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
            Engineer newStu = new(_id, _email, _cost, _name, _level);

            s_dal!.Engineer.Create(newStu);

        }

    }
    //method for create dependency
    private static void createDependency()
    {
        for (int i = 0; i < 40; i++)
        {
            int _id = i;

            int num = s_rand.Next(0, 20);

            int _dependentTask = s_rand.Next(0, num);

            int _dependsOnTask = s_rand.Next(num, 20);

            Dependency newStu = new(_id, _dependentTask,_dependsOnTask);

            s_dal!.Dependency.Create(newStu);
        }
    }

    public static void Do(IDal dal) //stage 2
    {

        s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2

        createTask();
        createEngineer();
        createDependency();

    }
}
