﻿namespace DalTest;

using DalApi;
using DO;
using System.ComponentModel.DataAnnotations.Schema;
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
        "A1","A2","A3","A4","A5","A6","A7","A8",
    //Advanced:
        "B1","B2","B3","B4","B5","B6",
    //Intermediate:
        "C1","C2","C3","C4",
    //Advanced Beginner:
        "D1",
    //Beginner:
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

            DateTime currentTime = DateTime.Now;

            DateTime _CreatedAtDate = currentTime.AddDays((-daycounter)*2);//before datetime.now

            DO.EngineerExperience _Complexity;

            if (i < 9 && i >= 1)
            {
                _Complexity = EngineerExperience.Beginner;
            }
            else if (i < 15 && i >= 9)
            {
                _Complexity = EngineerExperience.AdvancedBeginner;
            }
            else if (i < 19 && i >= 15)
            {
                _Complexity = EngineerExperience.Intermidate;
            }
            else if (i < 20 && i >= 19)
            {
                _Complexity = EngineerExperience.Advanced;
            }
            else if (i <= 20)
            {
                _Complexity = EngineerExperience.Expert;
            }
            else
            {
                _Complexity = EngineerExperience.Beginner;
            }

            daycounter--;

            Task newStu = new(_id, _Alias, _Describtion, _IsMilestone, _CreatedAtDate, _Complexity);

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

   //public static void Do(IDal dal) //stage 2
    public static void Do() //stage 4
    {

        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        s_dal = DalApi.Factory.Get; //stage 4
        createTask();
        createEngineer();
        createDependency();

    }
}


