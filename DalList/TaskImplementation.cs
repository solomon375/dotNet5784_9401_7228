namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        bool exist = DataSource.Tasks.Any(t => t.Alias == item.Alias && t.Describtion == item.Describtion &&
        t.Complexity == item.Complexity && t.Status == item.Status && t.RequiredEffortTime == item.RequiredEffortTime &&
        t.Deliverable == item.Deliverable && t.Remarks == item.Remarks);
        if (exist == false)
        {
            int next = DataSource.Config.NextTaskId;
            Task item1 = item with { Id = next };
            DataSource.Tasks.Add(item1);
            return item1.Id;
        }
        return -1;
    }

    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
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

    public Task? Read(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(filter);
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(item => item.Id == id);
    }

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

    public void Delete(int id)
    {
        Task foundEngineer = DataSource.Tasks.FirstOrDefault(task => task.Id == id)
                ?? throw new DalNotExistException($"Dependencys with ID={id} does Not exist");

        DataSource.Tasks.Remove(foundEngineer);
    }
}
