namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

/// <summary>
/// Implementation of the Data Access Layer for managing tasks using XML storage.
/// </summary>
internal class TaskImplementation:ITask
{
    // XML file name for task storage
    readonly string s_tasks_xml = "tasks";

    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="item">The task to create.</param>
    /// <returns>The ID of the newly created task.</returns>
    public int Create(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml) ?? new List<Task>();

        int next = Config.NextTaskId;

        Task item1 = item with { Id = next };

        tasks.Add(item1);

        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);

        return item1.Id;
    }

    /// <summary>
    /// Deletes a task by its ID.
    /// </summary>
    /// <param name="id">The ID of the task to delete.</param>
    public void Delete(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml) ?? new List<Task>();

        foreach (Task index in tasks)
        {
            if (index.Id == id)
            {
                tasks.Remove(index);

                XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);

                return;
            }
        }

        throw new DalNotExistException($"Dependencys with ID={id} does Not exist");
    }

    /// <summary>
    /// Reads a task by its ID.
    /// </summary>
    /// <param name="id">The ID of the task to read.</param>
    /// <returns>The task with the specified ID, or null if not found.</returns>
    public Task? Read(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml) ?? new List<Task>();

        return tasks.FirstOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Reads a task based on a filter condition.
    /// </summary>
    /// <param name="filter">The filter condition.</param>
    /// <returns>The first task matching the filter condition, or null if not found.</returns>
    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml) ?? new List<Task>();

        return tasks.FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads all tasks, optionally applying a filter condition.
    /// </summary>
    /// <param name="filter">The optional filter condition.</param>
    /// <returns>A collection of tasks that match the filter condition.</returns>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml) ?? new List<Task>();

        if (filter != null)
        {
            return from item in tasks
                   where filter(item)
                   select item;
        }
        return tasks;
    }

    /// <summary>
    /// Updates an existing task.
    /// </summary>
    /// <param name="item">The updated task.</param>
    public void Update(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml) ?? new List<Task>();

        foreach (Task Index in tasks)
        {
            if (Index.Id == item.Id)
            {
                tasks.Remove(Index);

                tasks.Insert(0, item);

                XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);

                return;
            }
        }

        throw new DalNotExistException($"Task with ID={item.Id} does Not exist");
    }
}
