namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    //method for create task
    public int Create(Task item)
    {
        int next = DataSource.Config.NextTaskId;

        Task item1 = item with { Id = next };

        DataSource.Tasks.Add(item1);
        
        return item1.Id;
    }

    //method for delete task
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

    //method for read task
    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(item => item.Id == id);
    }

    //method for read all task
    public List<Task> ReadAll()
    {
        return DataSource.Tasks.Select(item => item).ToList();
    }

    //method for update task
    public void Update(Task item)
    {
        foreach (Task Index in DataSource.Tasks)
        {
            if (Index.Id == item.Id)
            {
                DataSource.Tasks.Remove(Index);

                DataSource.Tasks.Insert(0, item);
            }
        }

        throw new DalNotExistException($"Task with ID={item.Id} does Not exist");
    }
}
