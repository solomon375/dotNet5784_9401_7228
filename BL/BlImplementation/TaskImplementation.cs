using BlApi;
using BO;
using System.ComponentModel.DataAnnotations;

namespace BlImplementation;
internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Task item)
    {
        DO.Task doTask = new DO.Task(item.Id, item.Alias, item.Describtion, item.IsMilestone,
            item.CreatedAtDate, item.RequiredEffortTime, item.DeadLine,
            (DO.EngineerExperience?)item.Complexity, item.ScheduledDate, item.StartedDate,
            item.CompletedDate, item.Deliverable, item.Remarks, null);

        if (doTask.Alias =="")
        {
            throw new BO.BlInvalidException("INVALID ALIAS");
        }

        if(item.Complexity == EngineerExperience.Beginner) 
        { 
            item.Dependencies = null;
        }

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
        else if (item.Complexity == EngineerExperience.Intermidate)
        {
            foreach (var t in _dal.Task.ReadAll())
            {
                if (t.Complexity == DO.EngineerExperience.AdvancedBeginner)
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
        else if (item.Complexity == EngineerExperience.Advanced)
        {
            foreach (var t in _dal.Task.ReadAll())
            {
                if (t.Complexity == DO.EngineerExperience.Intermidate)
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
        else if (item.Complexity == EngineerExperience.Expert)
        {
            foreach (var t in _dal.Task.ReadAll())
            {
                if (t.Complexity == DO.EngineerExperience.Advanced)
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

        try
        {
            return _dal.Task.Create(doTask);
        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistException($"task with ID={item.Id} already exists", ex);
        }
    }

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

    public BO.Task? Read(int id)
    {
        var item = _dal.Task.Read(id);
    if (item == null)
    {
        throw new BlDoesNotExistException($"Task with ID={id} does not exist");
    }

    var result = (from t in _dal.Task.ReadAll()
                  join e in _dal.Engineer.ReadAll() on t.EngineerID equals e.Id
                  where t.Id == id
                  select new BO.Task
                  {
                      Id = t.Id,
                      Alias = t.Alias,
                      Describtion = t.Describtion,
                      status = Status.Scheduled,
                      Dependencies = null,
                      IsMilestone = false,
                      CreatedAtDate = t.CreatedAtDate,
                      Complexity = (EngineerExperience?)t.Complexity,
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

    return result;
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

    public void Update(BO.Task item)
    {
        DO.Task doTask = new DO.Task(item.Id, item.Alias, item.Describtion, item.IsMilestone,
            item.CreatedAtDate, item.RequiredEffortTime, item.DeadLine,
            (DO.EngineerExperience?)item.Complexity, item.ScheduledDate, item.StartedDate,
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

    public void UpdataOrAddDate(int id, DateTime date)
    {
        BO.Task? item = Read(id);

        if(item.Dependencies == null)
        {
            item.StartedDate = date;
            Update(item);
        }

        foreach(var i in item.Dependencies)
        {
            var item1 = Read(i.Id);
            if(item1.ScheduledDate == null)
            {
                throw new BO.BlStartDataOfDependsOnTaskException($"The task with the ID={item1.Id} has no start date and this task depends on it");
            }
            else if(item1.ScheduledDate < date)
            {
                throw new BO.BlStartDataOfDependsOnTaskException($"The task with the ID={item1.Id} schedule date is after the entered date" +
                    $" and this task depends on it");
            }
            else if(item1.DeadLine < date)
            {
                throw new BO.BlStartDataOfDependsOnTaskException($"The task with the ID={item1.Id} deadline date is befor the entered date" +
                    $" and this task depends on it");
            }
        }

        item.StartedDate = date;
        Update(item);
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

