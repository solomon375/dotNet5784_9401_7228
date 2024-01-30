namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

/// <summary>
/// Implementation of the Data Access Layer for managing engineers using XML storage.
/// </summary>
internal class EngineerImplementation:IEngineer
{
    // XML file name for engineer storage
    readonly string s_engineers_xml = "engineers";

    /// <summary>
    /// Creates a new engineer.
    /// </summary>
    /// <param name="item">The engineer to create.</param>
    /// <returns>The ID of the newly created engineer.</returns>
    public int Create(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml) ?? new List<Engineer>();

        if (Read(item.Id) is not null)
        {
            throw new DalAlreadyExistException($"Student with ID={item.Id} already exists");
        }

        engineers.Add(item);

        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);

        return item.Id;
    }

    /// <summary>
    /// Deletes an engineer by its ID.
    /// </summary>
    /// <param name="id">The ID of the engineer to delete.</param>
    public void Delete(int id)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml) ?? new List<Engineer>();

        foreach (Engineer index in engineers)
        {
            if (index.Id == id)
            {
                engineers.Remove(index);

                XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);

                return;
            }
        }

        throw new DalNotExistException($"Dependencys with ID={id} does Not exist");
    }

    /// <summary>
    /// Reads an engineer by its ID.
    /// </summary>
    /// <param name="id">The ID of the engineer to read.</param>
    /// <returns>The engineer with the specified ID, or null if not found.</returns>
    public Engineer? Read(int id)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml) ?? new List<Engineer>();

        return engineers.FirstOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Reads an engineer based on a filter condition.
    /// </summary>
    /// <param name="filter">The filter condition.</param>
    /// <returns>The first engineer matching the filter condition, or null if not found.</returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml) ?? new List<Engineer>();

        return engineers.FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads all engineers, optionally applying a filter condition.
    /// </summary>
    /// <param name="filter">The optional filter condition.</param>
    /// <returns>A collection of engineers that match the filter condition.</returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml) ?? new List<Engineer>();

        if (filter != null)
        {
            return from item in engineers
                   where filter(item)
                   select item;
        }
        return engineers;
    }

    /// <summary>
    /// Updates an existing engineer.
    /// </summary>
    /// <param name="item">The updated engineer.</param>
    public void Update(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml) ?? new List<Engineer>();

        foreach (Engineer Index in engineers)
        {
            if (Index.Id == item.Id)
            {
                engineers.Remove(Index);

                engineers.Insert(0, item);

                XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);

                return;
            }
        }

        throw new DalNotExistException($"Engineer with ID={item.Id} does Not exist");
    }
}
