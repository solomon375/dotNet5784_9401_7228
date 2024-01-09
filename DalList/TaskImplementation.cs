namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int next = DataSource.Config.NextTaskId;

        Task item1 = item with { Id = next };

        DataSource.Tasks.Add(item1);
        
        return item1.Id;
    }

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
        throw new Exception($"Dependencys with ID={id} does Not exist");
    }

    public Task? Read(int id)
    {
        foreach (Task Index in DataSource.Tasks)
        {
            if (Index.Id == id)
            {
                return Index;
            }
        }
        return null;
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);//need more ditails  
    }

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

        throw new Exception($"Task with ID={item.Id} does Not exist");
    }
}
