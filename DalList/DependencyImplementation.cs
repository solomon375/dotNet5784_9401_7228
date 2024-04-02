/// <summary>
/// Implementation of the dependency data access layer.
/// </summary>
namespace Dal;

/// <summary>
/// Imports necessary namespaces.
/// </summary>
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Implementation of the dependency interface.
/// </summary>
internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// Creates a new dependency.
    /// </summary>
    public int Create(Dependency item)
    {
        bool exist = DataSource.Dependencys.Any(dependency => dependency.DependentTask == item.DependentTask &&
        dependency.DependsOnTask == item.DependsOnTask);
        if (exist == false)
        {
            int next = DataSource.Config.NextDependencyId;
            Dependency dependency = item with { Id =  next };
            DataSource.Dependencys.Add(dependency);
            return item.Id;
        }
        return -1;
    }

    /// <summary>
    /// Reads all dependencies optionally filtered by a condition.
    /// </summary>
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
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
    /// Reads a single dependency based on a filter condition.
    /// </summary>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependencys.FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads a dependency by ID.
    /// </summary>
    public Dependency? Read(int id)
    {
        return DataSource.Dependencys.FirstOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Updates an existing dependency.
    /// </summary>
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
    /// Deletes a dependency by ID.
    /// </summary>
    public void Delete(int id)
    {
        Dependency foundDependency = DataSource.Dependencys.FirstOrDefault(dep => dep.Id == id)
                ?? throw new DalNotExistException($"Dependency with ID={id} does not exist"); ;

        DataSource.Dependencys.Remove(foundDependency);
    }
}
