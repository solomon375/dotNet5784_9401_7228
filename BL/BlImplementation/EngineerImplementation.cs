/// <summary>
/// Namespace containing the implementation of business logic for engineers.
/// </summary>
namespace BlImplementation;

/// <summary>
/// Imports necessary namespaces.
/// </summary>
using BlApi;
using BO;
using DalApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Implementation of the engineer interface.
/// </summary>
internal class EngineerImplementation : BlApi.IEngineer
{
    private readonly IBl _bl;

    internal EngineerImplementation(IBl bl)
    {
        _bl = bl ?? throw new ArgumentNullException(nameof(bl));
    }

    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Creates a new engineer.
    /// </summary>
    public int Create(BO.Engineer item)
    {
        int temp = GetIntegerInput("enter engineer id");
        if (temp != 0) { item.Id = temp; }

        string tmp = GetStringInput("enter engineer name");
        if (tmp != null) { item.Name = tmp; }

        Console.WriteLine("\n enter engineer cost between 0-150");
        Console.WriteLine("Beginner cost between 0-50");
        Console.WriteLine("AdvancedBeginner cost between 50-70");
        Console.WriteLine("Intermidate cost between 70-90");
        Console.WriteLine("Advanced cost between 90-100");
        Console.WriteLine("Expert cost between 100-150");
        temp = GetRangedIntegerInput(0, 150, "please enter only int type\n", "the cost is set to 35");
        if (temp != 0) { item.Cost = temp; }

        item.Level = GetExperienceLevel(item.Cost);

        tmp = GetStringInput("enter engineer email");
        if (tmp != null) { item.Email = tmp; }

        DO.Engineer doEngineer = new DO.Engineer(item.Id, item.Email, item.Cost, item.Name, (DO.EngineerExperience?)item.Level);

        ValidateDOEngineer(doEngineer);

        try
        {
            return _dal.Engineer.Create(doEngineer);
        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistException($"engineer with ID={doEngineer.Id} already exists", ex);
        }
    }

    /// <summary>
    /// Deletes an engineer by ID.
    /// </summary>
    public void Delete(int id)
    {
        BO.Engineer? item = Read(id);
        if (item?.Task != null)
        {
            throw new BO.BlCantBeDeletedException("this item cant be deleted due to task active");
        }
        else
        {
            try
            {
                _dal.Engineer.Delete(id);
            }
            catch (DO.DalNotExistException)
            {
                throw new BlDoesNotExistException($"engineer with ID={id} does Not exist");
            }
        }
    }

    /// <summary>
    /// Reads an engineer by ID.
    /// </summary>
    public BO.Engineer? Read(int id)
    {
        DO.Engineer? item = _dal.Engineer.Read(id);
        if (item == null)
        {
            throw new BlDoesNotExistException($"Engineer with ID={id} does Not exist");
        }

        var taskInEngineer = _dal.Task.ReadAll()
            .Where(t => t.EngineerID == id)
            .Select(t => new TaskInEngineer
            {
                Id = t.Id,
                Alias = t.Alias
            })
            .FirstOrDefault();

        return new BO.Engineer
        {
            Id = item.Id,
            Name = item.Name,
            Cost = item.Cost,
            Email = item.Email,
            Level = (BO.EngineerExperience?)item.Level,
            Task = taskInEngineer
        };
    }

    /// <summary>
    /// Reads all engineers optionally filtered by a condition.
    /// </summary>
    public IEnumerable<BO.Engineer?> ReadAll(Func<DO.Engineer, bool>? filter = null)
    {
        var doresult = _dal.Engineer.ReadAll().Where(filter ?? (i => true));

        var boresult = doresult.Select(i => new BO.Engineer
        {
            Id = i.Id,
            Name = i.Name,
            Cost = i.Cost,
            Email = i.Email,
            Level = (BO.EngineerExperience?)i.Level,
            Task = null
        }).ToList();

        foreach (var x in boresult)
        {
            var matchingTasks = _dal.Task.ReadAll().Where(y => x.Id == y.EngineerID);

            x.Task = matchingTasks.Select(y => new TaskInEngineer
            {
                Id = y.Id,
                Alias = y.Alias
            }).SingleOrDefault();
        }

        return boresult;
    }

    /// <summary>
    /// Updates an existing engineer.
    /// </summary>
    public void Update(BO.Engineer item)
    {
        string tmp = GetStringInput("enter engineer name");
        if (tmp != null) { item.Name = tmp; }

        Console.WriteLine("\n enter engineer cost between 0-150");
        Console.WriteLine("Beginner cost between 0-50");
        Console.WriteLine("AdvancedBeginner cost between 50-70");
        Console.WriteLine("Intermidate cost between 70-90");
        Console.WriteLine("Advanced cost between 90-100");
        Console.WriteLine("Expert cost between 100-150");
        int temp = GetRangedIntegerInput(0, 150, "please enter only int type\n", "the cost is set to 35");
        if (temp != 0) { item.Cost = temp; }

        item.Level = GetExperienceLevel(item.Cost);

        tmp = GetStringInput("enter engineer email");
        if (tmp != null) { item.Email = tmp; }

        /*if (_bl.Now > new BO.Clock().start)
        {
            if (_bl.engineer.Read(item.Id).Task != null)
            {
                Console.WriteLine("do you finish the task? (y/n)");
                if (GetYesNoInput())
                {
                    finishTask(item);
                }
            }

            if (_bl.engineer.Read(item.Id).Task == null)
            {
                Console.WriteLine("do you want to take a task?(y/n)");
                if (GetYesNoInput())
                {
                    List<DO.Task> d = ListTaskCanTake(item);
                    int id = chooseTask();
                    takeTask(item,id);
                }
            }
        }*/

        DO.Engineer doEngineer = new DO.Engineer(item.Id, item.Email, item.Cost, item.Name,
           (DO.EngineerExperience?)item.Level, item.Task?.Id);

        ValidateDOEngineer(doEngineer);

        try
        {
            _dal.Engineer.Update(doEngineer);
        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistException($"engineer with ID={doEngineer.Id} already exists", ex);
        }
    }

    /// <summary>
    /// Marks a task as finished for a given engineer.
    /// </summary>
    public void finishTask(BO.Engineer item)
    {
        if (_bl.Now > new BO.Clock().start)
        {
            var t = _dal.Task.Read(item.Task.Id) with { CompletedDate = _bl.Now, Status = DO.Status.Done, EngineerID=null };
            item.Task = new TaskInEngineer();
            item.Task = null;
            _dal.Task.Update(t);
        }
        else
        {
            throw new BO.BlPrograpStartException("you cant take a task the progect dont started yet");
        }
    }

    /// <summary>
    /// Lists tasks that can be assigned to the engineer.
    /// </summary>
    public List<DO.Task> ListTaskCanTake(BO.Engineer item)
    {
        if (_bl.Now > new BO.Clock().start)
        {
            bool hasInjaprtyTask = _dal.Task.ReadAll().Any(task => task.Status == DO.Status.InJeopardy);

            List<int?> taskIdOfThetask = new List<int?>();

            List<DO.Task> tasks = new List<DO.Task>();

            foreach (var task in _dal.Task.ReadAll())
            {
                foreach (var dep in _dal.Dependency.ReadAll()) { if (dep.DependentTask == task.Id) { taskIdOfThetask.Add(dep.DependsOnTask); } }

                bool allTasksFinished = taskIdOfThetask.All(id => _dal.Task.ReadAll().Any(task => task.Id == id && task.Status == DO.Status.Done));

                if (hasInjaprtyTask)
                {
                    if (item.Level >= (BO.EngineerExperience?)task.Complexity && task.EngineerID == null
                        &&(task.Status == DO.Status.InJeopardy)&& _bl.Now < task.DeadLine - task.RequiredEffortTime
                        && allTasksFinished)
                    {
                        Console.WriteLine(task.Id); Console.WriteLine(task.Describtion); Console.WriteLine(task.Complexity);
                        tasks.Add(task);
                    }
                }
                else
                {
                    if (item.Level >= (BO.EngineerExperience?)task.Complexity && task.EngineerID == null
                        &&(task.Status == DO.Status.Scheduled)&& _bl.Now < task.DeadLine - task.RequiredEffortTime
                        && allTasksFinished)
                    {
                        Console.WriteLine(task.Id); Console.WriteLine(task.Describtion); Console.WriteLine(task.Complexity);
                        tasks.Add(task);
                    }
                }

                taskIdOfThetask.Clear();
            }

            return tasks;
        }
        else
        {
            throw new BO.BlPrograpStartException("you cant take a task the progect dont started yet");
        }
    }

    /// <summary>
    /// Allows the engineer to choose a task.
    /// </summary>
    public int chooseTask()
    {
        int id;
        Console.WriteLine("enter the task id you want to take");
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("please enter only int type\n");
        }
        return id;
    }

    /// <summary>
    /// Assigns a task to the engineer.
    /// </summary>
    public void takeTask(BO.Engineer item, int id)
    {
        if (_bl.Now > new BO.Clock().start)
        {
            item.Task = new TaskInEngineer();
            item.Task.Id = id;
            item.Task.Alias = _dal.Task.Read(id)?.Alias;


            var ta = _dal.Task.Read(id) with
            {
                EngineerID = item.Id,
                StartedDate = _bl.Now,
                DeadLine = _bl.Now + _dal.Task.Read(id).RequiredEffortTime,
                Status = DO.Status.OnTrack
            };

            _dal.Task.Update(ta);
        }
        else
        {
            throw new BO.BlPrograpStartException("you cant take a task the progect dont started yet");
        }
    }

    // Helper methods...

    private int GetIntegerInput(string message)
    {
        int result;
        Console.WriteLine(message);
        if (!int.TryParse(Console.ReadLine(), out result))
        {
            Console.WriteLine("please enter only int type\n");
        }
        return result;
    }

    private string GetStringInput(string message)
    {
        Console.WriteLine(message);
        return Console.ReadLine();
    }

    private int GetRangedIntegerInput(int min, int max, string errorMessage, string successMessage)
    {
        int result = GetIntegerInput("");
        if (result <= 0 || result > max)
        {
            Console.WriteLine(errorMessage);
            result = min;
            Console.WriteLine(successMessage);
        }
        return result;
    }

    private BO.EngineerExperience? GetExperienceLevel(double? cost)
    {
        return cost switch
        {
            <= 50 => BO.EngineerExperience.Beginner,
            <= 70 => BO.EngineerExperience.AdvancedBeginner,
            <= 90 => BO.EngineerExperience.Intermidate,
            <= 100 => BO.EngineerExperience.Advanced,
            <= 150 => BO.EngineerExperience.Expert,
            _ => null,
        };
    }

    private void ValidateDOEngineer(DO.Engineer doEngineer)
    {
        if (doEngineer.Id <= 0) throw new BO.BlInvalidException("INVALID ID");
        if (doEngineer.Name == "") throw new BO.BlInvalidException("INVALID NAME");
        if (doEngineer.Cost <= 0) throw new BO.BlInvalidException("INVALID COST");
        if (!new EmailAddressAttribute().IsValid(doEngineer.Email)) throw new BO.BlInvalidException("INVALID EMAIL");
    }

    private bool GetYesNoInput()
    {
        string? ans;
        Console.WriteLine("please enter 'y' or 'n'");
        ans = Console.ReadLine();
        if (ans != "y" && ans != "n") { return false; }

        return ans == "y";
    }
}
