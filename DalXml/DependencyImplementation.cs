using DalApi;
using DO;

namespace Dal;

/// <summary>
/// Implementation of the dependency interface.
/// </summary>
internal class DependencyImplementation : IDependency
{
    readonly string s_dependencys_xml = "dependencys";

    /// <summary>
    /// Creates a new dependency.
    /// </summary>
    public int Create(Dependency item)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml) ?? new List<Dependency>();
        bool exist = dependencys.Any(dependency => dependency.DependentTask == item.DependentTask &&
        dependency.DependsOnTask == item.DependsOnTask);
        if (exist == false)
        {
            int next = Config.NextDependencyId;
            Dependency item1 = item with { Id = next };
            dependencys.Add(item1);
            XMLTools.SaveListToXMLSerializer(dependencys, s_dependencys_xml);
            return item1.Id;
        }
        return -1;
    }

    /// <summary>
    /// Deletes a dependency by ID.
    /// </summary>
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
    /// Reads a dependency by ID.
    /// </summary>
    public Dependency? Read(int id)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml) ?? new List<Dependency>();
        return dependencys.FirstOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Reads a single dependency based on a filter condition.
    /// </summary>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml) ?? new List<Dependency>();
        return dependencys.FirstOrDefault(filter);
    }


    /// <summary>
    /// Reads all dependencies optionally filtered by a condition.
    /// </summary>
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
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
