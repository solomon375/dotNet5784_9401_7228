/// <summary>
/// Implementation of the ITask interface in the BL (Business Logic) layer.
/// </summary>
/// <remarks>
/// This class provides the functionality for managing Task entities, including creating, reading, updating, and deleting tasks.
/// </remarks>
using BlApi;
using BO;
using DO;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlImplementation;
internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// Creates a new Task entity in the system.
    /// </summary>
    /// <param name="item">The Task object to create.</param>
    /// <returns>The ID of the newly created Task.</returns>
    public int Create(BO.Task item)
    {
        DO.Task doTask = new DO.Task(item.Id, item.Alias, item.Describtion, item.IsMilestone,
            item.CreatedAtDate, item.RequiredEffortTime, item.DeadLine,
            (DO.EngineerExperience?)item.Complexity,(DO.Status?)item.status ,item.ScheduledDate, item.StartedDate,
            item.CompletedDate, item.Deliverable, item.Remarks, null);

        if (doTask.Alias =="")
        {
            throw new BO.BlInvalidException("INVALID ALIAS");
        }

        
        int it;
        try
        {
            it = _dal.Task.Create(doTask);
        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistException($"task with ID={item.Id} already exists", ex);
        }

        /*if (item.Id <=20 || item.Id == 0)
        {
            foreach (var d in _dal.Dependency.ReadAll())
            {
                if (item.Id == d.DependentTask && d.DependsOnTask != null)
                {
                    var t = _dal.Task.Read((int)d.DependsOnTask);
                    if (t != null)
                    {
                        item.Dependencies.Add(new TaskInList
                        {
                            Id = t.Id,
                            Alias = t.Alias,
                            Description = t.Describtion,
                            Status = BO.Status.Scheduled
                        });

                    }
                    else
                    {
                        item.Dependencies = null;
                    }

                }
            }
        }*/
        //else
        {
            foreach (var t in _dal.Task.ReadAll())
            {
                if ((BO.EngineerExperience?)t.Complexity == item.Complexity-1 && t.Id != it)
                {
                    item.Dependencies.Add(new TaskInList
                    {
                        Id = t.Id,
                        Alias = t.Alias,
                        Description = t.Describtion,
                        Status = BO.Status.Scheduled
                    });
                    Dependency nd = new Dependency(0, it, t.Id);
                    _dal!.Dependency.Create(nd);
                }
            }
            /*if (item.Complexity == BO.EngineerExperience.Beginner)
            {
                item.Dependencies = null;
            }

            else if (item.Complexity == BO.EngineerExperience.AdvancedBeginner && item.Dependencies.Count == 0)
            {
                foreach (var t in _dal.Task.ReadAll())
                {
                    if (t.Complexity == DO.EngineerExperience.Beginner && t.Id != it)
                    {
                        item.Dependencies.Add(new TaskInList
                        {
                            Id = t.Id,
                            Alias = t.Alias,
                            Description = t.Describtion,
                            Status = BO.Status.Scheduled
                        });
                        Dependency nd = new Dependency(0,it, t.Id);
                        _dal!.Dependency.Create(nd);
                    }
                }
            }
            else if (item.Complexity == BO.EngineerExperience.Intermidate && item.Dependencies.Count == 0)
            {
                foreach (var t in _dal.Task.ReadAll())
                {
                    if (t.Complexity == DO.EngineerExperience.AdvancedBeginner && t.Id != it)
                    {
                        item.Dependencies.Add(new TaskInList
                        {
                            Id = t.Id,
                            Alias = t.Alias,
                            Description = t.Describtion,
                            Status = BO.Status.Scheduled
                        });
                        Dependency nd = new Dependency(0,it, t.Id);
                        _dal!.Dependency.Create(nd);
                    }
                }
            }
            else if (item.Complexity == BO.EngineerExperience.Advanced && item.Dependencies.Count == 0)
            {
                foreach (var t in _dal.Task.ReadAll())
                {
                    if (t.Complexity == DO.EngineerExperience.Intermidate && t.Id != it)
                    {
                        item.Dependencies.Add(new TaskInList
                        {
                            Id = t.Id,
                            Alias = t.Alias,
                            Description = t.Describtion,
                            Status = BO.Status.Scheduled
                        });
                        Dependency nd = new Dependency(0,it, t.Id);
                        _dal!.Dependency.Create(nd);
                    }
                }
            }
            else if (item.Complexity == BO.EngineerExperience.Expert && item.Dependencies.Count == 0)
            {
                foreach (var t in _dal.Task.ReadAll())
                {
                    if (t.Complexity == DO.EngineerExperience.Advanced && t.Id != it)
                    {
                        item.Dependencies.Add(new TaskInList
                        {
                            Id = t.Id,
                            Alias = t.Alias,
                            Description = t.Describtion,
                            Status = BO.Status.Scheduled
                        });
                        Dependency nd = new Dependency(0,it, t.Id);
                        _dal!.Dependency.Create(nd);
                    }
                }
            }*/
        }

        foreach (var i in item.Dependencies)
        {
            if (item.ScheduledDate < _dal.Task.Read(i.Id).ScheduledDate)
            {
                throw new BO.BlStartDataOfDependsOnTaskException($"The task with the ID={item.Id} schedule date is after the entered date" +
                    $" and this task depends on it");
            }
        }

        return it;
    }

    /// <summary>
    /// Deletes a Task entity from the system based on its ID.
    /// </summary>
    /// <param name="id">The ID of the Task to delete.</param>
    public void Delete(int id)
    {
        BO.Task? item = Read(id);

        foreach(var d in _dal.Dependency.ReadAll())
        {
            if(id == d.DependsOnTask)
            {
                throw new BO.BlCantBeDeletedException("this item cant be deleted due to Dependencies");
            }
        }

        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalNotExistException)
        {
            throw new BlDoesNotExistException($"task with ID={id} does Not exist");
        }
    }

    /// <summary>
    /// Retrieves a Task entity from the system based on its ID.
    /// </summary>
    /// <param name="id">The ID of the Task to retrieve.</param>
    /// <returns>The retrieved Task object.</returns>
    public BO.Task? Read(int id)
    {
        var item = _dal.Task.Read(id);
        if (item == null)
        {
            throw new BlDoesNotExistException($"Task with ID={id} does not exist");
        }

        List<TaskInList> d = new List<TaskInList>();

        //if (item.Id <=20 || item.Id == 0)
        {
            foreach (var de in _dal.Dependency.ReadAll())
            {
                if (item.Id == de.DependentTask && de.DependsOnTask != null)
                {
                    var t = _dal.Task.Read((int)de.DependsOnTask);
                    if (t != null)
                    {
                        d.Add(new TaskInList
                        {
                            Id = t.Id,
                            Alias = t.Alias,
                            Description = t.Describtion,
                            Status = BO.Status.Scheduled
                        });
                    }
                    /*else
                    {
                        d = null;
                    }*/

                }
            }
        }
        //
        /*else
        {
            if (item.Complexity == DO.EngineerExperience.Beginner)
            {
                d = null;
            }

            else if (item.Complexity == DO.EngineerExperience.AdvancedBeginner)
            {
                foreach (var t in _dal.Task.ReadAll())
                {
                    if (t.Complexity == DO.EngineerExperience.Beginner && t.Id != item.Id)
                    {
                        d.Add(new TaskInList
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
            else if (item.Complexity == DO.EngineerExperience.Intermidate)
            {
                foreach (var t in _dal.Task.ReadAll())
                {
                    if (t.Complexity == DO.EngineerExperience.AdvancedBeginner)
                    {
                        d.Add(new TaskInList
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
            else if (item.Complexity == DO.EngineerExperience.Advanced)
            {
                foreach (var t in _dal.Task.ReadAll())
                {
                    if (t.Complexity == DO.EngineerExperience.Intermidate)
                    {
                        d.Add(new TaskInList
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
            else if (item.Complexity == DO.EngineerExperience.Expert)
            {
                foreach (var t in _dal.Task.ReadAll())
                {
                    if (t.Complexity == DO.EngineerExperience.Advanced)
                    {
                        d.Add(new TaskInList
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
            //
        }*/

        var result = (from t in _dal.Task.ReadAll()
                      join e in _dal.Engineer.ReadAll() on t.EngineerID equals e.Id
                      where t.Id == id
                      select new BO.Task
                      {
                          Id = t.Id,
                          Alias = t.Alias,
                          Describtion = t.Describtion,
                          status = (BO.Status)t.status,
                          Dependencies = d,
                          IsMilestone = false,
                          CreatedAtDate = t.CreatedAtDate,
                          Complexity = (BO.EngineerExperience?)t.Complexity,
                          ScheduledDate = t.ScheduledDate,
                          StartedDate = t.StartedDate,
                          RequiredEffortTime = t.RequiredEffortTime,
                          DeadLine = t.DeadLine,
                          CompletedDate = t.CompletedDate,
                          Deliverable = t.Deliverable,
                          Remarks = t.Remarks,
                          Engineer = new EngineerInTask
                          {
                              Id = e.Id,
                              Name = e.Name,
                          }
                      }).FirstOrDefault();

        if (result == null)
        {
            result = new BO.Task
            {
                Id = item.Id,
                Alias = item.Alias,
                Describtion = item.Describtion,
                status = (BO.Status)item.status,
                Dependencies = d,
                IsMilestone = false,
                CreatedAtDate = item.CreatedAtDate,
                Complexity = (BO.EngineerExperience?)item.Complexity,
                ScheduledDate = item.ScheduledDate,
                StartedDate = item.StartedDate,
                RequiredEffortTime = item.RequiredEffortTime,
                DeadLine = item.DeadLine,
                CompletedDate = item.CompletedDate,
                Deliverable = item.Deliverable,
                Remarks = item.Remarks,
                Engineer = null
            };
        }

        return result;
    }

    /// <summary>
    /// Retrieves a list of Task entities from the system.
    /// </summary>
    /// <param name="filter">An optional filter to apply to the list of tasks.</param>
    /// <returns>The list of TaskInList objects representing the tasks.</returns>
    public IEnumerable<TaskInList?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        if (filter != null)
        {
            return from DO.Task item in _dal.Task.ReadAll()
                   where filter(item)
                   select new BO.TaskInList
                   {
                       Id = item.Id,
                       Description = item.Describtion,
                       Alias = item.Alias,
                       Status = (BO.Status)item.status
                   };
        }
        return from DO.Task item in _dal.Task.ReadAll()
               select new BO.TaskInList
               {
                   Id = item.Id,
                   Description = item.Describtion,
                   Alias = item.Alias,
                   Status = (BO.Status)item.status
               };
    }

    /// <summary>
    /// Updates a Task entity in the system.
    /// </summary>
    /// <param name="item">The Task object with updated information.</param>
    public void Update(BO.Task item)
    {
        Console.WriteLine("enter the task id");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("please enter only datetime type\n");
        }
        item.Id = id;

        DO.EngineerExperience? c = _dal.Task.Read(item.Id).Complexity;

        if((BO.EngineerExperience?)c != item.Complexity) 
        {
            List<Dependency> dependencies = new List<Dependency>();
            foreach(var d in _dal.Dependency.ReadAll())
            {
                if(d.DependentTask == item.Id)
                {
                    dependencies.Add(d);
                }
            }
            foreach(var d in dependencies) { _dal.Dependency.Delete(d.Id); }

            foreach (var t in _dal.Task.ReadAll())
            {
                if ((BO.EngineerExperience?)t.Complexity == item.Complexity-1 && t.Id != item.Id)
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

        DO.Task doTask = new DO.Task(item.Id, item.Alias, item.Describtion, item.IsMilestone,
            item.CreatedAtDate, item.RequiredEffortTime, item.DeadLine,
            (DO.EngineerExperience?)item.Complexity,(DO.Status?)item.status ,item.ScheduledDate, item.StartedDate,
            item.CompletedDate, item.Deliverable, item.Remarks, null);

        if (doTask.Alias =="")
        {
            throw new BO.BlInvalidException("INVALID ALIAS");
        }

        TimeSpan time1 = new(0, 0, 0);
        if (doTask.RequiredEffortTime <= time1)
        {
            throw new BO.BlInvalidException("INVALID Required Effort Time");
        }
        
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
    /// Updates or adds the start date for a Task entity in the system.
    /// </summary>
    /// <param name="id">The ID of the Task to update.</param>
    /// <param name="date">The new start date to set for the Task.</param>
    public void UpdataOrAddDate(int id, DateTime date)
    {
        BO.Task? item = Read(id);

        if (item.Dependencies == null)
        {
            item.StartedDate = date;
            Update(item);
        }
        else
        {
            foreach (var i in item.Dependencies)
            {
                var item1 = Read(i.Id);
                if (item1.ScheduledDate == null)
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
}



























/*{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(BO.Task item)
    {
        DO.Task doTask = new DO.Task(item.Id, item.Alias, item.Describtion, item.IsMilestone,
            item.CreatedAtDate, item.RequiredEffortTime, item.DeadLine,
            (DO.EngineerExperience?)item.Complexity,item.ScheduledDate,item.StartedDate,
            item.CompletedDate, item.Deliverable, item.Remarks, null );

        if (doTask.Alias =="")
        {
            throw new BO.BlInvalidException("INVALID ALIAS");
        }
        
        // תוסיף תלויות עבור משימות קודמות מתוך רשימת המשימות הקיימת
        // אם הנתונים תקינים - תבצע ניסיון בקשות הוספה לשכבת הנתונים 
        
        try
        {
            return _dal.Task.Create(doTask);
        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistException($"Student with ID={item.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        BO.Task? item = Read(id);
        if (item?.Dependencies == null)
        {
            try
            {
                _dal.Task.Delete(id);
            }
            catch (DO.DalNotExistException)
            {
                throw new BlDoesNotExistException($"Student with ID={id} does Not exist");
            }
        }
        else
        {
            throw new BO.BlCantBeDeletedException("this item cant be deleted due to Dependencies");
        }

        if (item?.Dependencies == null)
        {
            try
            {
                _dal.Task.Delete(id);
            }
            catch (DO.DalNotExistException)
            {
                throw new BlDoesNotExistException($"task with ID={id} does Not exist");
            }
        }
        else
        {
            throw new BO.BlCantBeDeletedException("this item cant be deleted due to Dependencies");
        }
    }

    public BO.Task? Read(int id)
    {
        DO.Task? item = _dal.Task.Read(id);
        if (item == null)
        {
            throw new BlDoesNotExistException($"Student with ID={id} does Not exist");
        }
        if(item.EngineerID == null)
        {
            return new BO.Task()
            {
                Id =  item.Id,
                Alias = item.Alias,
                Describtion = item.Describtion,
                status = Status.Scheduled,
                Dependencies = null,
                IsMilestone = false,
                CreatedAtDate = item.CreatedAtDate,
                Complexity = (EngineerExperience?)item.Complexity,
                ScheduledDate = item.ScheduledDate,
                StartedDate = item.StartedDate,
                RequiredEffortTime = item.RequiredEffortTime,
                DeadLine = item.DeadLine,
                CompletedDate = item.CompletedDate,
                Deliverable = item.Deliverable,
                Remarks = item.Remarks,
                Engineer = null
            };
        }
        return new BO.Task()
        {
            Id =  item.Id,
            Alias = item.Alias,
            Describtion = item.Describtion,
            status = Status.Scheduled,
            Dependencies = null,
            IsMilestone = false,
            CreatedAtDate = item.CreatedAtDate,
            Complexity = (EngineerExperience?)item.Complexity,
            ScheduledDate = item.ScheduledDate,
            StartedDate = item.StartedDate,
            RequiredEffortTime = item.RequiredEffortTime,
            DeadLine = item.DeadLine,
            CompletedDate = item.CompletedDate,
            Deliverable = item.Deliverable,
            Remarks = item.Remarks,
            //Engineer
        };
        else if (item.Complexity == EngineerExperience.AdvancedBeginner)
        {
            foreach(var t in _dal.Task.ReadAll())
            {
                if(t.Complexity == DO.EngineerExperience.Beginner)
                {
                    item.Dependencies.Add(new TaskInList
                    {
                        Id = t.Id,
                        Alias = t.Alias,
                        Description = t.Describtion,
                        Status = Status.Scheduled
                    });
                }
            }
        }
    }

    public IEnumerable<TaskInList?> ReadAll(Func<DO.Task, bool>? filter = null)
    {

        if (filter != null)
        {
            return from DO.Task item in _dal.Task.ReadAll()
                   where filter(item)
                   select new BO.TaskInList
                   {
                       Id = item.Id,
                       Description = item.Describtion,
                       Alias = item.Alias,
                       Status = Status.Scheduled
                   };
        }
        return from DO.Task item in _dal.Task.ReadAll()
               select new BO.TaskInList
               {
                   Id = item.Id,
                   Description = item.Describtion,
                   Alias = item.Alias,
                   Status = Status.Scheduled
               }; 
    }

    public void UpdataOrAddDate(int id, DateTime date)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task item)
    {
        DO.Task doTask = new DO.Task(item.Id, item.Alias, item.Describtion, item.IsMilestone,
            item.CreatedAtDate, item.RequiredEffortTime, item.DeadLine,
            (DO.EngineerExperience?)item.Complexity,item.ScheduledDate,item.StartedDate,
            item.CompletedDate, item.Deliverable, item.Remarks, null);

        if (doTask.Alias =="")
        {
            throw new BO.BlInvalidException("INVALID ALIAS");
        }

        TimeSpan time1 = new(0, 0, 0);
        if(doTask.RequiredEffortTime <= time1)
        {
            throw new BO.BlInvalidException("INVALID Required Effort Time");
        }
        //צריך להסיף תלות
        try
        {
            _dal.Task.Update(doTask);
        }
        catch (DO.DalNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Student with ID={item.Id} not exists", ex);
        }
    }
}*/

