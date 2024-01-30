namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

/// <summary>
/// Implementation of the Data Access Layer for managing dependencies using XML storage.
/// </summary>
internal class DependencyImplementation:IDependency
{
    // XML file name for dependency storage
    readonly string s_dependencys_xml = "dependencys";

    /// <summary>
    /// Creates a new dependency.
    /// </summary>
    /// <param name="item">The dependency to create.</param>
    /// <returns>The ID of the newly created dependency.</returns>
    public int Create(Dependency item)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml) ?? new List<Dependency>();

        int next = Config.NextDependencyId;

        Dependency item1 = item with { Id = next };

        dependencys.Add(item1);

        XMLTools.SaveListToXMLSerializer(dependencys, s_dependencys_xml);

        return item1.Id;
    }

    /// <summary>
    /// Deletes a dependency by its ID.
    /// </summary>
    /// <param name="id">The ID of the dependency to delete.</param>
    public void Delete(int id)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml) ?? new List<Dependency>();

        foreach (Dependency index in dependencys)
        {
            if (index.Id == id)
            {
                dependencys.Remove(index);

                XMLTools.SaveListToXMLSerializer(dependencys, s_dependencys_xml);

                return;
            }
        }

        throw new DalNotExistException($"Dependencys with ID={id} does Not exist");
    }

    /// <summary>
    /// Reads a dependency by its ID.
    /// </summary>
    /// <param name="id">The ID of the dependency to read.</param>
    /// <returns>The dependency with the specified ID, or null if not found.</returns>
    public Dependency? Read(int id)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml) ?? new List<Dependency>();

        return dependencys.FirstOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Reads a dependency based on a filter condition.
    /// </summary>
    /// <param name="filter">The filter condition.</param>
    /// <returns>The first dependency matching the filter condition, or null if not found.</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml) ?? new List<Dependency>();

        return dependencys.FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads all dependencies, optionally applying a filter condition.
    /// </summary>
    /// <param name="filter">The optional filter condition.</param>
    /// <returns>A collection of dependencies that match the filter condition.</returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml) ?? new List<Dependency>();

        if (filter != null)
        {
            return from item in dependencys
                   where filter(item)
                   select item;
        }
        return dependencys;
    }

    /// <summary>
    /// Updates an existing dependency.
    /// </summary>
    /// <param name="item">The updated dependency.</param>
    public void Update(Dependency item)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml) ?? new List<Dependency>();

        foreach (Dependency Index in dependencys)
        {
            if (Index.Id == item.Id)
            {
                dependencys.Remove(Index);

                dependencys.Insert(0, item);

                XMLTools.SaveListToXMLSerializer(dependencys, s_dependencys_xml);

                return;
            }
        }

        throw new DalNotExistException($"dependency with ID={item.Id} does Not exist");
    }
}
