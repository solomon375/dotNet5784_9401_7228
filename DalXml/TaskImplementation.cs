namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class TaskImplementation:ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml) ?? new List<Task>();

        int next = Config.NextTaskId;

        Task item1 = item with { Id = next };

        tasks.Add(item1);

        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);

        return item1.Id;
    }

    public void Delete(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml) ?? new List<Task>();

        foreach (Task index in tasks)
        {
            if (index.Id == id)
            {
                tasks.Remove(index);
                return;
            }
        }

        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);

        throw new DalNotExistException($"Dependencys with ID={id} does Not exist");
    }

    public Task? Read(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml) ?? new List<Task>();

        return tasks.FirstOrDefault(item => item.Id == id);
    }

    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml) ?? new List<Task>();

        return tasks.FirstOrDefault(filter);
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml) ?? new List<Task>();

        if (filter != null)
        {
            return from item in tasks
                   where filter(item)
                   select item;
        }
        return from item in tasks
               select item;
    }

    public void Update(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml) ?? new List<Task>();

        foreach (Task Index in tasks)
        {
            if (Index.Id == item.Id)
            {
                tasks.Remove(Index);

                tasks.Insert(0, item);
            }
        }

        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);

        throw new DalNotExistException($"Task with ID={item.Id} does Not exist");
    }
}
