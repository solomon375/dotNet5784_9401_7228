namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// Represents the implementation of the Engineer-related operations in the Data Access Layer.
/// </summary>
internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// Creates a new engineer.
    /// </summary>
    /// <param name="item">The Engineer object to be created.</param>
    /// <returns>The identifier of the newly created engineer.</returns>
    public int Create(Engineer item)
    {
        // Check if an engineer with the same ID already exists.
        if (Read(item.Id) is not null)
        {
            throw new DalAlreadyExistException($"Student with ID={item.Id} already exists");
        }

        // Add the new engineer to the list.
        DataSource.Engineers.Add(item);
        
        return item.Id;
    }

    /*//method for create engineer
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
    }*/

    /// <summary>
    /// Deletes an engineer based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of the engineer to be deleted.</param>
    public void Delete(int id)
    {
        Engineer foundEngineer = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == id)
                ?? throw new DalNotExistException($"Engineer with ID={id} does not exist");

        DataSource.Engineers.Remove(foundEngineer);
    }

    /// <summary>
    /// Reads an engineer based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of the engineer to be read.</param>
    /// <returns>The Engineer object if found; otherwise, null.</returns>

    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Reads all engineers, optionally filtered by a specified condition.
    /// </summary>
    /// <param name="filter">The filter condition to apply (optional).</param>
    /// <returns>An IEnumerable of Engineer objects.</returns>
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


    /// <summary>
    /// Updates an existing engineer.
    /// </summary>
    /// <param name="item">The Engineer object with updated values.</param>
    public void Update(Engineer item)
    {
        foreach(Engineer Index in DataSource.Engineers)
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
    /// Reads an engineer based on a specified filter condition.
    /// </summary>
    /// <param name="filter">The filter condition to apply.</param>
    /// <returns>The Engineer object if found; otherwise, null.</returns>
    public Engineer? Read(Func<Engineer, bool> filter) //stage 2
    {
        return DataSource.Engineers.FirstOrDefault(filter);
    }
}
