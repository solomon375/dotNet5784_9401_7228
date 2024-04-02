using DalApi;
using DO;
using System.Linq;

namespace Dal;

/// <summary>
/// Implementation of the task data access layer.
/// </summary>
internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";

    /// <summary>
    /// Creates a new task.
    /// </summary>
    public int Create(DO.Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml) ?? new List<DO.Task>();
        bool exist = tasks.Any(t => t.Alias == item.Alias && t.Describtion == item.Describtion &&
        t.Complexity == item.Complexity && t.Status == item.Status && t.RequiredEffortTime == item.RequiredEffortTime &&
        t.Deliverable == item.Deliverable && t.Remarks == item.Remarks);
        if (exist == false)
        {
            int next = Config.NextTaskId;
            DO.Task item1 = item with { Id = next };
            tasks.Add(item1);
            XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
            return item1.Id;
        }
        return -1;
    }

    /// <summary>
    /// Deletes a task by ID.
    /// </summary>
    public void Delete(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml) ?? new List<DO.Task>();
        foreach (DO.Task index in tasks)
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
    /// Reads a task by ID.
    /// </summary>
    public DO.Task? Read(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml) ?? new List<DO.Task>();
        return tasks.FirstOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Reads a single task based on a filter condition.
    /// </summary>
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml) ?? new List<DO.Task>();
        return tasks.FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads all tasks optionally filtered by a condition.
    /// </summary>
    public IEnumerable<DO.Task> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml) ?? new List<DO.Task>();
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
    public void Update(DO.Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml) ?? new List<DO.Task>();

        foreach (DO.Task Index in tasks)
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
