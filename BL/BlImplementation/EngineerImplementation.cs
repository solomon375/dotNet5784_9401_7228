using BlApi;
using BO;
using System.ComponentModel.DataAnnotations;

namespace BlImplementation;
internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Engineer item)
    {
        DO.Engineer doEngineer = new DO.Engineer(item.Id, item.Email, item.Cost, item.Name,
            (DO.EngineerExperience?)item.Level);

        if (doEngineer.Id <= 0)
        {
            throw new BO.BlInvalidException("INVALID ID");
        }

        if (doEngineer.Name == "")
        {
            throw new BO.BlInvalidException("INVALID NAME");
        }

        if (doEngineer.Cost <= 0)
        {
            throw new BO.BlInvalidException("INVALID COST");
        }

        if (!new EmailAddressAttribute().IsValid(doEngineer.Email))
        {
            throw new BO.BlInvalidException("INVALID EMAIL");
        }

        try
        {
            return _dal.Engineer.Create(doEngineer);
        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistException($"engineer with ID={doEngineer.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        BO.Engineer? item = Read(id);
        if (item?.Task != null)
        {
            throw new BO.BlCantBeDeletedException("this item cant be deleted due to task active");
        }
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalNotExistException)
        {
            throw new BlDoesNotExistException($"engineer with ID={id} does Not exist");
        }
    }

    public BO.Engineer? Read(int id)
    {
        BO.Engineer? result = null;
        DO.Engineer? item = _dal.Engineer.Read(id);
        if (item == null)
        {
            throw new BlDoesNotExistException($"Student with ID={id} does Not exist");
        }

        foreach (var e in _dal.Engineer.ReadAll())
        {
            foreach (var t in _dal.Task.ReadAll())
            {
                if (e.Id == t.EngineerID)
                {
                    result = new BO.Engineer()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Cost = item.Cost,
                        Email = item.Email,
                        Level = item.level,
                        Task = new TaskInEngineer
                        {
                            Id=t.Id,
                            Alias = t.Alias,
                        }
                    };
                    return result;
                }
            }
        }
        result = new BO.Engineer()
        {
            Id = item.Id,
            Name = item.Name,
            Cost = item.Cost,
            Email = item.Email,
            Level = item.level,
            Task = null
        };
        return result;
        
    }

    public IEnumerable<BO.Engineer?> ReadAll(Func<DO.Engineer, bool>? filter = null)
    {
        List<DO.Engineer> DoEngineers = new List<DO.Engineer>();
        List<BO.Engineer> BoEngineers = new List<BO.Engineer>();
        List<BO.Engineer> returnList = new List<BO.Engineer>();

        foreach (var e in _dal.Engineer.ReadAll()) 
        {
            foreach(var t in _dal.Task.ReadAll())
            {
                if(e.Id == t.EngineerID)
                {
                    BoEngineers.Add(new BO.Engineer()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Cost = e.Cost,
                        Email = e.Email,
                        Level = e.level,
                        Task = new TaskInEngineer
                        {
                            Id=t.Id,
                            Alias = t.Alias,
                        }
                    });
                    DoEngineers.Add(new DO.Engineer()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Cost = e.Cost,
                        Email = e.Email,
                    });
                }
                else
                {
                    BoEngineers.Add(new BO.Engineer()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Cost = e.Cost,
                        Email = e.Email,
                        Level = e.level,
                        Task = null
                    });
                    DoEngineers.Add(new DO.Engineer()
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Cost = e.Cost,
                        Email = e.Email,
                    });
                }
            }
        }

        if(filter != null)
        {
            for(int i = 0; i < DoEngineers.Count; i++)
            {
                if (filter(DoEngineers[i])) { returnList.Add(BoEngineers[i]); }
            }
        }
        else
        {
            for (int i = 0; i < DoEngineers.Count; i++)
            {
               returnList.Add(BoEngineers[i]);
            }
        }
        return returnList;
    }

    public void Update(BO.Engineer item)
    {
        DO.Engineer doEngineer = new DO.Engineer(item.Id, item.Email, item.Cost, item.Name,
            (DO.EngineerExperience?)item.Level);

        if (doEngineer.Id <= 0)
        {
            throw new BO.BlInvalidException("INVALID ID");
        }

        if (doEngineer.Name == "")
        {
            throw new BO.BlInvalidException("INVALID NAME");
        }

        if (doEngineer.Cost <= 0)
        {
            throw new BO.BlInvalidException("INVALID COST");
        }

        if (!new EmailAddressAttribute().IsValid(doEngineer.Email))
        {
            throw new BO.BlInvalidException("INVALID EMAIL");
        }

        try
        {
            _dal.Engineer.Update(doEngineer);

            foreach (var t in _dal.Task.ReadAll())
            {
                if (t.Id == item.Task.Id)
                {
                    var ta = _dal.Task.Read(t.Id) with { EngineerID = item.Id,StartedDate = DateTime.Now,
                        DeadLine = DateTime.Now + _dal.Task.Read(t.Id).RequiredEffortTime
                    };

                    _dal.Task.Update(ta);
                }
            }
        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistException($"engineer with ID={doEngineer.Id} already exists", ex);
        }
    }
}



























/*{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(Engineer item)
    {
        DO.Engineer doEngineer = new DO.Engineer(item.Id, item.Email,item.Cost,item.Name,
            (DO.EngineerExperience?)item.Level);

        if (doEngineer.Id <= 0)
        {
            throw new BO.BlInvalidException("INVALID ID");
        }

        if (doEngineer.Name == "")
        {
            throw new BO.BlInvalidException("INVALID NAME");
        }

        if (doEngineer.Cost <= 0)
        {
            throw new BO.BlInvalidException("INVALID COST");
        }

        if (!new EmailAddressAttribute().IsValid(doEngineer.Email))
        {
            throw new BO.BlInvalidException("INVALID EMAIL");
        }

        try
        {
            return _dal.Engineer.Create(doEngineer);
        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistException($"Student with ID={doEngineer.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        BO.Engineer? item = Read(id);
        if(item?.Task != null)
        {
            throw new BO.BlCantBeDeletedException("this item cant be deleted due to task active");
        }
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalNotExistException)
        {
            throw new BlDoesNotExistException($"Student with ID={id} does Not exist");
        }
    }

    public Engineer? Read(int id)
    {
        DO.Engineer? item = _dal.Engineer.Read(id);
        if (item == null)
        {
            throw new BlDoesNotExistException($"Student with ID={id} does Not exist");
        }
        return new BO.Engineer()
        {
            Id = item.Id,
            Name = item.Name,
            Cost = item.Cost,
            Email = item.Email,
            Level = item.level,
            //Task =
        };

        DO.Engineer? item = _dal.Engineer.Read(id);
        if (item == null)
        {
            throw new BlDoesNotExistException($"engineer with ID={id} does not exist");
        }

        var relevantTasks = _dal.Task.ReadAll().Where(t => t.EngineerID == id);
        
        var result = new BO.Engineer()
        {
            Id = item.Id,
            Name = item.Name,
            Cost = item.Cost,
            Email = item.Email,
            Level = item.level,
            Task = relevantTasks.Any()
                ? new TaskInEngineer
                {
                    Id = relevantTasks.First().Id,
                    Alias = relevantTasks.First().Alias,
                }
                : null
        };

        return result;

        
    }

    public IEnumerable<Engineer?> ReadAll(Func<DO.Engineer, bool>? filter = null)
    {
        if (filter != null)
        {

            return from DO.Engineer item in _dal.Engineer.ReadAll()
                   where filter(item)
                   select new BO.Engineer
                   {
                       Id = item.Id,
                       Name = item.Name,
                       Cost = item.Cost,
                       Email = item.Email,
                       Level = item.level,
                       //Task =
                   };
        }
        return from DO.Engineer item in _dal.Engineer.ReadAll()
               select new BO.Engineer
               {
                   Id = item.Id,
                   Name = item.Name,
                   Cost = item.Cost,
                   Email = item.Email,
                   Level = item.level,
                   //Task =
               };
    }

    public void Update(Engineer item)
    {
        DO.Engineer doEngineer = new DO.Engineer(item.Id, item.Email, item.Cost, item.Name,
            (DO.EngineerExperience?)item.Level);

        if (doEngineer.Id <= 0)
        {
            throw new BO.BlInvalidException("INVALID ID");
        }

        if (doEngineer.Name == "")
        {
            throw new BO.BlInvalidException("INVALID NAME");
        }

        if (doEngineer.Cost <= 0)
        {
            throw new BO.BlInvalidException("INVALID COST");
        }

        if (!new EmailAddressAttribute().IsValid(doEngineer.Email))
        {
            throw new BO.BlInvalidException("INVALID EMAIL");
        }

        try
        {
            _dal.Engineer.Update(doEngineer);
        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistException($"Student with ID={doEngineer.Id} already exists", ex);
        }
    }
}*/
