namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// Represents the implementation of the Dependency-related operations in the Data Access Layer.
/// </summary>
internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// Creates a new dependency.
    /// </summary>
    /// <param name="item">The Dependency object to be created.</param>
    /// <returns>The identifier of the newly created dependency.</returns>
    public int Create(Dependency item)
    {
        int next = DataSource.Config.NextDependencyId;

        Dependency item1 = item with { Id = next };

        DataSource.Dependencys.Add(item1);

        return item1.Id;
    }

    /// <summary>
    /// Deletes a dependency based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of the dependency to be deleted.</param>
    public void Delete(int id)
    {
        Dependency foundDependency = DataSource.Dependencys.FirstOrDefault(dep => dep.Id == id)
                ?? throw new DalNotExistException($"Dependency with ID={id} does not exist");

        DataSource.Dependencys.Remove(foundDependency);
        /*foreach(Dependency index in DataSource.Dependencys)
        {
            if(index.Id == id)
            {
                DataSource.Dependencys.Remove(index);
                return;
            }
        }
        throw new DalNotExistException($"Dependencys with ID={id} does Not exist");*/
    }

    /// <summary>
    /// Reads a dependency based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of the dependency to be read.</param>
    /// <returns>The Dependency object if found; otherwise, null.</returns>
    public Dependency? Read(int id)
    {
        return DataSource.Dependencys.FirstOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Reads all dependencies, optionally filtered by a specified condition.
    /// </summary>
    /// <param name="filter">The filter condition to apply (optional).</param>
    /// <returns>An IEnumerable of Dependency objects.</returns>
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Dependencys
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Dependencys
               select item;
    }


    /// <summary>
    /// Updates an existing dependency.
    /// </summary>
    /// <param name="item">The Dependency object with updated values.</param>
    public void Update(Dependency item)
    {
        foreach (Dependency Index in DataSource.Dependencys)
        {
            if (Index.Id == item.Id)
            {
                DataSource.Dependencys.Remove(Index);

                DataSource.Dependencys.Insert(0, item);

                return;
            }
        }

        throw new DalNotExistException($"dependency with ID={item.Id} does Not exist"); 
    }

    /// <summary>
    /// Reads a dependency based on a specified filter condition.
    /// </summary>
    /// <param name="filter">The filter condition to apply.</param>
    /// <returns>The Dependency object if found; otherwise, null.</returns>
    public Dependency? Read(Func<Dependency, bool> filter) //stage 2
    {
        return DataSource.Dependencys.FirstOrDefault(filter);
    }
}
