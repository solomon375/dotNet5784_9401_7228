namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    //method for create engineer
    public int Create(Engineer item)
    {
        foreach(Engineer index in DataSource.Engineers)
        {
            if (index.Id == item.Id)
            {
                throw new DalAlreadyExistException($"Engineer with ID={item.Id} already exist");
            }
        }
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    //method for delete engineer
    public void Delete(int id)
    {
        foreach (Engineer index in DataSource.Engineers)
        {
            if (index.Id == id)
            {
                DataSource.Engineers.Remove(index);
                return;
            }
        }
        throw new DalNotExistException($"Dependencys with ID={id} does Not exist");
    }

    //method for read engineer
    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(item => item.Id == id);
    }

    //method for read all engineer
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Engineers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Engineers
               select item;
    }


    //method for update engineer
    public void Update(Engineer item)
    {
        foreach(Engineer Index in DataSource.Engineers)
        {
            if (Index.Id == item.Id)
            {
                DataSource.Engineers.Remove(Index);

                DataSource.Engineers.Insert(0, item);
            }
        }

        throw new DalNotExistException($"Engineer with ID={item.Id} does Not exist"); 
    }
}
