namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        if (Read(item.Id) is not null)
        {
            throw new DalAlreadyExistException($"Student with ID={item.Id} already exists");
        }
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
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

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(filter);
    }

    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(item => item.Id == id);
    }

    public void Update(Engineer item)
    {
        foreach (Engineer Index in DataSource.Engineers)
        {
            if (Index.Id == item.Id)
            {
                DataSource.Engineers.Remove(Index);
                DataSource.Engineers.Insert(0, item);
                return;
            }
        }
        throw new DalNotExistException($"Engineer with ID={item.Id} does Not exist");
    }

    public void Delete(int id)
    {
        Engineer foundEngineer = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == id)
                ?? throw new DalNotExistException($"Engineer with ID={id} does not exist");

        DataSource.Engineers.Remove(foundEngineer);
    }
}
