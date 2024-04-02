/// <summary>
/// Namespace containing the implementation of business logic for tasks.
/// </summary>
namespace BlImplementation;

/// <summary>
/// Imports necessary namespaces.
/// </summary>
using BlApi;
using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// Implementation of the task interface.
/// </summary>
internal class TaskImplementation : ITask
{
    private readonly IBl _bl;

    internal TaskImplementation(IBl bl)
    {
        _bl = bl ?? throw new ArgumentNullException(nameof(bl));
    }

    private static DateTime start = new BO.Clock().start;

    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Creates a new task.
    /// </summary>
    public int Create(BO.Task item)
    {
        if (start > _bl.Now)
        {
            Console.WriteLine("enter Task Alias");
            string temp = GetStringInput();
            if (temp != null) { item.Alias = temp; }

            Console.WriteLine("enter Task Describtion");
            temp = GetStringInput();
            if (temp != null) { item.Describtion = temp; }

            Console.WriteLine("enter Task Deliverable");
            temp = GetStringInput();
            if (temp != null) { item.Deliverable = temp; }

            Console.WriteLine("enter Task Remarks");
            temp = GetStringInput();
            if (temp != null) { item.Remarks = temp; }

            item.CreatedAtDate = _bl.Now;

            item.CompletedDate = null;

            item.Status = BO.Status.Unscheduled;

            if (item.RequiredEffortTime == null)
            {
                int tmp = GetRequiredEffortTime();
                if (tmp != 1) { item.RequiredEffortTime = new(tmp, 0, 0, 0); } else { item.RequiredEffortTime = new(1, 0, 0, 0); }
            }

            item.StartedDate = item.ScheduledDate;

            if (item.Complexity == null)
            {
                item.Complexity = GetTaskComplexity();
            }

            (item.ScheduledDate, item.DeadLine) = CalculateDates(item.Complexity);

            DO.Task doTask = new DO.Task(item.Id, item.Alias, item.Describtion, (DO.EngineerExperience?)item.Complexity,
                (DO.Status?)item.Status, item.ScheduledDate, item.RequiredEffortTime, item.DeadLine,
                 item.CreatedAtDate, item.StartedDate, item.CompletedDate, item.Deliverable, item.Remarks, null);

            ValidateDOTask(doTask);

            int it;
            try
            {
                it = _dal.Task.Create(doTask);
            }
            catch (DO.DalAlreadyExistException ex)
            {
                throw new BO.BlAlreadyExistException($"task with ID={item.Id} already exists", ex);
            }

            CreateDependencies(item, it);

            ValidateDependencies(item);

            return it;
        }
        else
        {
            throw new BO.BlPrograpStartException("you cant create a task now the progect alraedy started");
        }
    }

    /// <summary>
    /// Deletes a task by ID.
    /// </summary>
    public void Delete(int id)
    {
        BO.Task? item = Read(id);

        foreach (var d in _dal.Dependency.ReadAll())
        {
            if (id == d.DependsOnTask)
            {
                throw new BO.BlCantBeDeletedException("this item cant be deleted due to Dependencies");
            }
        }

        try
        {
            _dal.Task.Delete(id);

            List<int> dependenciesToDelete = new List<int>();

            foreach(var d in _dal.Dependency.ReadAll())
            {
                if(d.DependentTask == id||d.DependsOnTask == id)
                {
                    dependenciesToDelete.Add(d.Id);
                }
            }

            foreach (var d in dependenciesToDelete)
            {
                _dal.Dependency.Delete(d);
            }
        }
        catch (DO.DalNotExistException)
        {
            throw new BlDoesNotExistException($"task with ID={id} does Not exist");
        }
    }

    /// <summary>
    /// Reads a task by ID.
    /// </summary>
    public BO.Task? Read(int id)
    {
        DO.Task item = _dal.Task.Read(id);
        if (item == null)
        {
            throw new BlDoesNotExistException($"Task with ID={id} does not exist");
        }

        List<TaskInList> dependencies = new List<TaskInList>();
        foreach (var dependency in _dal.Dependency.ReadAll())
        {
            if (item.Id == dependency.DependentTask)
            {
                var t = _dal.Task.Read((int)dependency.DependsOnTask);
                if (t != null)
                {
                    dependencies.Add(new TaskInList
                    {
                        Id = t.Id,
                        Alias = t.Alias,
                        Description = t.Describtion,
                        Status = BO.Status.Scheduled
                    });
                }
            }
        }
        var result = (from task in _dal.Task.ReadAll() join engineer in _dal.Engineer.ReadAll() on task.EngineerID equals engineer.Id
                     where task.Id == id
                     select new BO.Task
                     {
                         Id = task.Id, Alias = task.Alias, Describtion = task.Describtion,
                         Complexity = (BO.EngineerExperience?)task.Complexity,
                         Status = (BO.Status?)task.Status, ScheduledDate = task.ScheduledDate,
                         RequiredEffortTime = task.RequiredEffortTime, DeadLine = task.DeadLine,
                         CreatedAtDate = task.CreatedAtDate, StartedDate = task.StartedDate,
                         CompletedDate = task.CompletedDate, Deliverable = task.Deliverable, Remarks = task.Remarks,
                         EngineerID = new EngineerInTask
                         {
                             Id = engineer.Id, Name = engineer.Name,
                         },
                         Dependencies = dependencies
                     }).FirstOrDefault();

        if(result == null)
        {
            result = new BO.Task
            {
                Id = item.Id, Alias = item.Alias, Describtion = item.Describtion,
                Complexity = (BO.EngineerExperience?)item.Complexity,
                Status = (BO.Status?)item.Status, ScheduledDate = item.ScheduledDate,
                RequiredEffortTime = item.RequiredEffortTime, DeadLine = item.DeadLine,
                CreatedAtDate = item.CreatedAtDate, StartedDate = item.StartedDate,
                CompletedDate = item.CompletedDate, Deliverable = item.Deliverable, Remarks = item.Remarks,
                EngineerID = null, Dependencies = dependencies
            };
        }
        return result;
    }

    /// <summary>
    /// Reads all tasks optionally filtered by a condition.
    /// </summary>
    public IEnumerable<BO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        List<DO.Task?> doresult = new List<DO.Task?>();
        List<BO.Task?> boresult = new List<BO.Task?>();

        foreach (var i in _dal.Task.ReadAll())
        {
            if (filter != null)
            {
                if (filter(i))
                {
                    doresult.Add(i);
                }
            }
            if (filter == null)
            {
                doresult.Add(i);
            }
        }

        foreach (var i in doresult)
        {
            boresult.Add(new BO.Task()
            {
                Id = i.Id, Alias = i.Alias, Describtion = i.Describtion,
                Complexity = (BO.EngineerExperience?)i.Complexity,
                Status = (BO.Status?)i.Status, ScheduledDate = i.ScheduledDate,
                RequiredEffortTime = i.RequiredEffortTime, DeadLine = i.DeadLine,
                CreatedAtDate = i.CreatedAtDate, StartedDate = i.StartedDate,
                CompletedDate = i.CompletedDate, Deliverable = i.Deliverable, Remarks = i.Remarks,
                EngineerID = null
            });
        }

        foreach (var x in boresult)
        {
            foreach (var y in _dal.Engineer.ReadAll())
            {
                if (x.Id == y.Task)
                {
                    x.EngineerID = new EngineerInTask
                    {
                        Id = y.Id, Name = y.Name,
                    };
                }
            }
        }

        return boresult;
    }

    /// <summary>
    /// Updates or adds the start date for a task.
    /// </summary>
    public void UpdataOrAddDate(int id, DateTime date)
    {
        BO.Task? item = Read(id);

        if (item?.Dependencies == null)
        {
            item.StartedDate = date;
            Update(item);
        }
        else
        {
            foreach (var i in item.Dependencies)
            {
                var item1 = Read(i.Id);
                if (item1?.ScheduledDate == null)
                {
                    throw new BO.BlStartDataOfDependsOnTaskException($"The task with the ID={item1.Id} has no start date and this task depends on it");
                }
                else if (item1.ScheduledDate < date)
                {
                    throw new BO.BlStartDataOfDependsOnTaskException($"The task with the ID={item1.Id} schedule date is after the entered date" +
                        $" and this task depends on it");
                }
                else if (item1.DeadLine < date)
                {
                    throw new BO.BlStartDataOfDependsOnTaskException($"The task with the ID={item1.Id} deadline date is befor the entered date" +
                        $" and this task depends on it");
                }
            }

            item.StartedDate = date;
            Update(item);
        }
    }

    /// <summary>
    /// Updates an existing task.
    /// </summary>
    public void Update(BO.Task item)
    {
        Console.WriteLine("enter Task Alias");
        string temp = GetStringInput();
        if (temp != null) { item.Alias = temp; }

        Console.WriteLine("enter Task Describtion");
        temp = GetStringInput();
        if (temp != null) { item.Describtion = temp; }

        Console.WriteLine("enter Task Deliverable");
        temp = GetStringInput();
        if (temp != null) { item.Deliverable = temp; }

        Console.WriteLine("enter Task Remarks");
        temp = GetStringInput();
        if (temp != null) { item.Remarks = temp; }

        if (item.RequiredEffortTime == null)
        {
            int tmp = GetRequiredEffortTime();
            if (tmp != 1) { item.RequiredEffortTime = new(tmp, 0, 0, 0); } else { item.RequiredEffortTime = new(1, 0, 0, 0); }
        }

        try
        {
            if (_bl.Now < start)
            {
                Console.WriteLine("enter task complexity\n 1=Beginner\n 2=AdvancedBeginner\n 3=Intermidate\n 4=Advanced\n 5=Expert");
                if (item.Complexity == null)
                {
                    item.Complexity = GetTaskComplexity();
                }

                (item.ScheduledDate, item.DeadLine) = CalculateDates(item.Complexity);

                Console.WriteLine("\nWould you like to enter a depended task? (Y/N)");
                string ans = Console.ReadLine();
                if (ans?.ToUpper() == "Y")
                {
                    List<int> d = choiceDependecy(item);
                }
            }
        }
        catch 
        {
            item.Complexity = Read(item.Id).Complexity;
            throw new BO.BlPrograpStartException("you cant create a task now the progect alraedy started"); 
        }

        DO.Task doTask = new DO.Task(item.Id, item.Alias, item.Describtion, (DO.EngineerExperience?)item.Complexity,
            (DO.Status?)item.Status, item.ScheduledDate, item.RequiredEffortTime, item.DeadLine,
             item.CreatedAtDate, item.StartedDate, item.CompletedDate, item.Deliverable, item.Remarks, null);

        ValidateDOTask(doTask);

        TimeSpan time1 = new(0, 0, 0, 0);
        if (doTask.RequiredEffortTime <= time1) { throw new BO.BlInvalidException("INVALID Required Effort Time"); }

        try
        {
            _dal.Task.Update(doTask);
        }
        catch (DO.DalNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"task with ID={item.Id} not exists", ex);
        }
    }

    /// <summary>
    /// Allows to add dependencies to task.
    /// </summary>
    public List<int> choiceDependecy(BO.Task item)
    {
        List<int> d = _dal.Task.ReadAll().Where(t => t.Complexity < (DO.EngineerExperience?)item.Complexity).Select(t => t.Id).ToList();
        Console.WriteLine("\nyou choose from this task id's");
        foreach (int id in d) { Console.Write($"{id}, "); }
        Console.WriteLine("\nenter the number of the depended task");
        int task;
        if (!int.TryParse(Console.ReadLine(), out task))
        {
            Console.WriteLine("please enter only int type\n");
        }
         addDependecy(item,task);
        return d;
    }

    /// <summary>
    /// Adds a dependency to the task.
    /// </summary>
    public void addDependecy(BO.Task item,int task)
    {
        if (_bl.Now < start)
        {
            if (task != 0)
            {
                if (_dal.Task.Read(task).Complexity < (DO.EngineerExperience)item.Complexity)
                {
                    _dal.Dependency.Create(new Dependency(0, item.Id, task));
                    item.Dependencies.Add(new TaskInList
                    {
                        Id = task,
                        Alias = _dal.Task.Read(task).Alias,
                        Description = _dal.Task.Read(task).Describtion,
                        Status = (BO.Status)_dal.Task.Read(task).Status
                    });
                }
                else
                {
                    throw new BO.BlInvalidException("you can't add Dependency at a comlexety that highr then your comlexety" +
                        "or at your comlexety");
                }
            }

            UpdateDependencies(item);
        }
        else { throw new BO.BlPrograpStartException("you cant create a task now the progect alraedy started"); }
    }

    /// <summary>
    /// Updates dependencies of the task based on complexity.
    /// </summary>
    private void UpdateDependencies(BO.Task item)
    {
        BO.EngineerExperience? c = (BO.EngineerExperience?)_dal.Task.Read(item.Id).Complexity;
        if (c != item.Complexity)
        {
            List<Dependency> dependencies = new List<Dependency>();
            foreach (var d in _dal.Dependency.ReadAll())
            {
                if (d.DependentTask == item.Id)
                {
                    dependencies.Add(d);
                }
            }
            foreach (var d in dependencies) { _dal.Dependency.Delete(d.Id); }

            foreach (var t in _dal.Task.ReadAll())
            {
                if ((BO.EngineerExperience?)t.Complexity == item.Complexity - 1 && t.Id != item.Id)
                {
                    item.Dependencies.Add(new TaskInList
                    {
                        Id = t.Id,
                        Alias = t.Alias,
                        Description = t.Describtion,
                        Status = BO.Status.Scheduled
                    });
                    Dependency nd = new Dependency(0, item.Id, t.Id);
                    _dal!.Dependency.Create(nd);
                }
            }
        }
    }

    // Helper methods...

    private string GetStringInput()
    {
        Console.WriteLine("Please enter a value:");
        return Console.ReadLine();
    }

    private int GetRequiredEffortTime()
    {
        Console.WriteLine("enter the Required Effort Time (in days)");
        if (!int.TryParse(Console.ReadLine(), out int days) || days < 1 || days > 6)
        {
            Console.WriteLine("the Required Effort Time is set to 1 day");
            return 1;
        }
        return days;
    }

    private BO.EngineerExperience GetTaskComplexity()
    {
        Console.WriteLine("enter task complexity\n 1=Beginner\n 2=AdvancedBeginner\n 3=Intermidate\n 4=Advanced\n 5=Expert");
        if (!int.TryParse(Console.ReadLine(), out int num) || num < 1 || num > 5)
        {
            Console.WriteLine("task complexity is set to Beginner");
            return BO.EngineerExperience.Beginner;
        }
        return (BO.EngineerExperience)(num-1);
    }

    private (DateTime ScheduledDate, DateTime DeadLine) CalculateDates(BO.EngineerExperience? complexity)
    {
        DateTime scheduledDate;
        DateTime deadLine;

        
        scheduledDate = start.AddDays(complexity switch
        {
            BO.EngineerExperience.Beginner => 0,
            BO.EngineerExperience.AdvancedBeginner => 8,
            BO.EngineerExperience.Intermidate => 15,
            BO.EngineerExperience.Advanced => 22,
            BO.EngineerExperience.Expert => 29,
            _ => 0  ,
        });
        deadLine = start.AddMonths(complexity switch
        {
            BO.EngineerExperience.Beginner => 7,
            BO.EngineerExperience.AdvancedBeginner => 14,
            BO.EngineerExperience.Intermidate => 21,
            BO.EngineerExperience.Advanced => 28,
            BO.EngineerExperience.Expert => 35,
            _ => 1,
        });

        return (scheduledDate, deadLine);
    }

    private void ValidateDOTask(DO.Task doTask)
    {
        if (doTask.Alias == "")
        {
            throw new BO.BlInvalidException("INVALID ALIAS");
        }
    }

    private void CreateDependencies(BO.Task item, int taskId)
    {
        foreach (var t in _dal.Task.ReadAll())
        {
            if ((BO.EngineerExperience?)t.Complexity == item.Complexity - 1 && t.Id != taskId)
            {
                item.Dependencies?.Add(new TaskInList
                {
                    Id = t.Id,
                    Alias = t.Alias,
                    Description = t.Describtion,
                    Status = BO.Status.Scheduled
                });
                Dependency nd = new Dependency(0, taskId, t.Id);
                _dal!.Dependency.Create(nd);
            }
        }
    }

    private void ValidateDependencies(BO.Task item)
    {
        if (item.Dependencies != null)
        {
            foreach (var i in item.Dependencies)
            {
                if (item.ScheduledDate < _dal.Task?.Read(i.Id)?.DeadLine)
                {
                    throw new BO.BlStartDataOfDependsOnTaskException($"The task with the id {i.Id} deadline is before the " +
                        $"the ScheduledDate of this task and this task Depends on the task with the id {i.Id}");
                }
            }
        }
    }

    public void updateTasksEngineer(int taskId,int engineerId)
    {
        BO.Task? t = Read(taskId);
        t.EngineerID = new EngineerInTask();
        t.EngineerID.Id = _bl.engineer.Read(engineerId).Id;
        t.EngineerID.Name = _bl.engineer.Read(engineerId).Name;
        Update(t);
    }
}