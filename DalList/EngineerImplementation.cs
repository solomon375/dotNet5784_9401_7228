namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Implementation of the engineer data access layer.
/// </summary>
internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// Creates a new engineer.
    /// </summary>
    public int Create(Engineer item)
    {
        if (Read(item.Id) is not null)
        {
            throw new DalAlreadyExistException($"Student with ID={item.Id} already exists");
        }
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    /// <summary>
    /// Reads all engineers optionally filtered by a condition.
    /// </summary>
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

    /// <summary>
    /// Reads a single engineer based on a filter condition.
    /// </summary>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads an engineer by ID.
    /// </summary>
    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Updates an existing engineer.
    /// </summary>
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

    /// <summary>
    /// Deletes an engineer by ID.
    /// </summary>
    public void Delete(int id)
    {
        Engineer foundEngineer = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == id)
                ?? throw new DalNotExistException($"Engineer with ID={id} does not exist");

        DataSource.Engineers.Remove(foundEngineer);
    }
}
