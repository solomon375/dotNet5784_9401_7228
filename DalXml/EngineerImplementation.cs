using DalApi;
using DO;

namespace Dal;

/// <summary>
/// Implementation of the engineer data access layer.
/// </summary>
internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";

    /// <summary>
    /// Creates a new engineer.
    /// </summary>
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
    /// Deletes an engineer by ID.
    /// </summary>
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
    /// Reads an engineer by ID.
    /// </summary>
    public Engineer? Read(int id)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml) ?? new List<Engineer>();
        return engineers.FirstOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Reads a single engineer based on a filter condition.
    /// </summary>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml) ?? new List<Engineer>();
        return engineers.FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads all engineers optionally filtered by a condition.
    /// </summary>
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
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
