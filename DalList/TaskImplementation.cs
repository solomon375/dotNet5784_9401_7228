namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// Represents the implementation of the Task-related operations in the Data Access Layer.
/// </summary>
internal class TaskImplementation : ITask
{
    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="item">The Task object to be created.</param>
    /// <returns>The identifier of the newly created task.</returns>
    public int Create(Task item)
    {
        int next = DataSource.Config.NextTaskId;

        Task item1 = item with { Id = next };

        DataSource.Tasks.Add(item1);
        
        return item1.Id;
    }

    /// <summary>
    /// Deletes a task based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of the task to be deleted.</param>
    public void Delete(int id)
    {
        foreach (Task index in DataSource.Tasks)
        {
            if (index.Id == id)
            {
                DataSource.Tasks.Remove(index);
                return;
            }
        }
        throw new DalNotExistException($"Dependencys with ID={id} does Not exist");
    }

    /// <summary>
    /// Reads a task based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of the task to be read.</param>
    /// <returns>The Task object if found; otherwise, null.</returns>
    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(item => item.Id == id);
    }

    /// <summary>
    /// Reads all tasks, optionally filtered by a specified condition.
    /// </summary>
    /// <param name="filter">The filter condition to apply (optional).</param>
    /// <returns>An IEnumerable of Task objects.</returns>
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Tasks
               select item;
    }


    /// <summary>
    /// Updates an existing task.
    /// </summary>
    /// <param name="item">The Task object with updated values.</param>
    public void Update(Task item)
    {
        foreach (Task Index in DataSource.Tasks)
        {
            if (Index.Id == item.Id)
            {
                DataSource.Tasks.Remove(Index);

                DataSource.Tasks.Insert(0, item);

                return;
            }
        }

        throw new DalNotExistException($"Task with ID={item.Id} does Not exist");
    }

    /// <summary>
    /// Reads a task based on a specified filter condition.
    /// </summary>
    /// <param name="filter">The filter condition to apply.</param>
    /// <returns>The Task object if found; otherwise, null.</returns>
    public Task? Read(Func<Task, bool> filter) //stage 2
    {
        return DataSource.Tasks.FirstOrDefault(filter);
    }
}
