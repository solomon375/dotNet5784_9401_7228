/// <summary>
/// Implementation of the IEngineer interface in the BL (Business Logic) layer.
/// </summary>
using BlApi;
using BO;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.Marshalling;

namespace BlImplementation;
internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Creates a new Engineer entity in the system.
    /// </summary>
    /// <param name="item">The Engineer object to create.</param>
    /// <returns>The ID of the newly created Engineer.</returns>
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

    /// <summary>
    /// Deletes an Engineer entity from the system based on its ID.
    /// </summary>
    /// <param name="id">The ID of the Engineer to delete.</param>
    public void Delete(int id)
    {
        BO.Engineer? item = Read(id);
        if (item?.Task != null)
        {
            throw new BO.BlCantBeDeletedException("this item cant be deleted due to task active");
        }
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalNotExistException)
        {
            throw new BlDoesNotExistException($"engineer with ID={id} does Not exist");
        }
    }

    /// <summary>
    /// Retrieves an Engineer entity from the system based on its ID.
    /// </summary>
    /// <param name="id">The ID of the Engineer to retrieve.</param>
    /// <returns>The retrieved Engineer object.</returns>
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

    /// <summary>
    /// Retrieves a list of Engineer entities from the system.
    /// </summary>
    /// <param name="filter">An optional filter to apply to the list of engineers.</param>
    /// <returns>The list of Engineer objects.</returns>
    public IEnumerable<BO.Engineer?> ReadAll(Func<DO.Engineer, bool>? filter = null)
    {
        List<DO.Engineer?> doresult = new List<DO.Engineer?> ();
        List<BO.Engineer?> boresult = new List<BO.Engineer?> ();

        foreach (var i in  _dal.Engineer.ReadAll())
        {
            if(filter != null)
            {
                if (filter(i))
                {
                    doresult.Add(i);
                }
            }
            if(filter == null)
            {
                doresult.Add(i);
            }
        }

        foreach(var i in doresult)
        {
            boresult.Add(new BO.Engineer()
            {
                Id = i.Id,
                Name = i.Name,
                Cost = i.Cost,
                Email = i.Email,
                Level = i.level,
                Task = null
            });
        }

        foreach(var x in boresult)
        {
            foreach(var y in _dal.Task.ReadAll())
            {
                if (x.Id == y.EngineerID)
                {
                    x.Task = new TaskInEngineer
                    {
                        Id = y.Id,
                        Alias = y.Alias
                    };
                }
            }
        }

        return boresult;
    }

    /// <summary>
    /// Updates an Engineer entity in the system.
    /// </summary>
    /// <param name="item">The Engineer object with updated information.</param>
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
            
            Console.WriteLine("do you wont to take a mission?(y/n)");
            string ans = Console.ReadLine();
            if (ans == "y")
            {
                if (item.Level == DO.EngineerExperience.Beginner)
                {
                    foreach (var t in _dal.Task.ReadAll())
                    {
                        if (item.Level == t.Complexity && t.EngineerID == null)
                        { Console.WriteLine(t.Id); Console.WriteLine(t.Describtion); Console.WriteLine(t.Complexity); }
                    }
                }
                else if (item.Level == DO.EngineerExperience.AdvancedBeginner)
                {
                    foreach (var t in _dal.Task.ReadAll())
                    {
                        if (item.Level >= t.Complexity && t.EngineerID == null)
                        { Console.WriteLine(t.Id); Console.WriteLine(t.Describtion); Console.WriteLine(t.Complexity); }
                    }
                }
                else if (item.Level == DO.EngineerExperience.Intermidate)
                {
                    foreach (var t in _dal.Task.ReadAll())
                    {
                        if (item.Level >= t.Complexity && t.EngineerID == null)
                        { Console.WriteLine(t.Id); Console.WriteLine(t.Describtion); Console.WriteLine(t.Complexity); }
                    }
                }
                else if (item.Level == DO.EngineerExperience.Advanced)
                {
                    foreach (var t in _dal.Task.ReadAll())
                    {
                        if (item.Level >= t.Complexity && t.EngineerID == null)
                        { Console.WriteLine(t.Id); Console.WriteLine(t.Describtion); Console.WriteLine(t.Complexity); }
                    }
                }
                else if (item.Level == DO.EngineerExperience.Expert)
                {
                    foreach (var t in _dal.Task.ReadAll())
                    {
                        if (item.Level >= t.Complexity && t.EngineerID == null)
                        { Console.WriteLine(t.Id); Console.WriteLine(t.Describtion); Console.WriteLine(t.Complexity); }
                    }
                }
                int id;
                Console.WriteLine("enter the task id you want to take");
                while (!int.TryParse(Console.ReadLine(), out id))
                {
                    Console.WriteLine("please enter only int type\n");
                }


                int c = id;
                item.Task = new TaskInEngineer();
                item.Task.Id = c;

                
                item.Task.Alias = _dal.Task.Read(c).Alias;
                var ta = _dal.Task.Read(c) with
                {
                    EngineerID = item.Id,
                    StartedDate = DateTime.Now,
                    DeadLine = DateTime.Now + _dal.Task.Read(c).RequiredEffortTime
                };

                _dal.Task.Update(ta);


            }
            _dal.Engineer.Update(doEngineer);
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
        /*List<DO.Engineer> DoEngineers = new List<DO.Engineer>();
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
