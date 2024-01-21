namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class EngineerImplementation:IEngineer
{
    readonly string s_engineers_xml = "engineers";

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

    public void Delete(int id)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml) ?? new List<Engineer>();

        foreach (Engineer index in engineers)
        {
            if (index.Id == id)
            {
                engineers.Remove(index);
                return;
            }
        }
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);

        throw new DalNotExistException($"Dependencys with ID={id} does Not exist");
    }

    public Engineer? Read(int id)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml) ?? new List<Engineer>();

        return engineers.FirstOrDefault(item => item.Id == id);
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml) ?? new List<Engineer>();

        return engineers.FirstOrDefault(filter);
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml) ?? new List<Engineer>();

        if (filter != null)
        {
            return from item in engineers
                   where filter(item)
                   select item;
        }
        return from item in engineers
               select item;
    }

    public void Update(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml) ?? new List<Engineer>();

        foreach (Engineer Index in engineers)
        {
            if (Index.Id == item.Id)
            {
                engineers.Remove(Index);

                engineers.Insert(0, item);
            }
        }

        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);

        throw new DalNotExistException($"Engineer with ID={item.Id} does Not exist");
    }
}
