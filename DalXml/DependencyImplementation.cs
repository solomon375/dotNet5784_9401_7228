namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class DependencyImplementation:IDependency
{
    readonly string s_dependencys_xml = "dependencys";

    public int Create(Dependency item)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml) ?? new List<Dependency>();

        int next = Config.NextDependencyId;

        Dependency item1 = item with { Id = next };

        dependencys.Add(item1);

        XMLTools.SaveListToXMLSerializer(dependencys, s_dependencys_xml);

        return item1.Id;
    }

    public void Delete(int id)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml) ?? new List<Dependency>();

        foreach (Dependency index in dependencys)
        {
            if (index.Id == id)
            {
                dependencys.Remove(index);
                return;
            }
        }

        XMLTools.SaveListToXMLSerializer(dependencys, s_dependencys_xml);

        throw new DalNotExistException($"Dependencys with ID={id} does Not exist");
    }

    public Dependency? Read(int id)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml) ?? new List<Dependency>();

        return dependencys.FirstOrDefault(item => item.Id == id);
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml) ?? new List<Dependency>();

        return dependencys.FirstOrDefault(filter);
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml) ?? new List<Dependency>();

        if (filter != null)
        {
            return from item in dependencys
                   where filter(item)
                   select item;
        }
        return from item in dependencys
               select item;
    }

    public void Update(Dependency item)
    {
        List<Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml) ?? new List<Dependency>();

        foreach (Dependency Index in dependencys)
        {
            if (Index.Id == item.Id)
            {
                dependencys.Remove(Index);

                dependencys.Insert(0, item);
            }
        }

        XMLTools.SaveListToXMLSerializer(dependencys, s_dependencys_xml);

        throw new DalNotExistException($"dependency with ID={item.Id} does Not exist");
    }
}
