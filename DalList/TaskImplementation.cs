﻿namespace Dal;
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
    public Task? Read(Func<Task, bool> filter) //stage 2
    {
        return DataSource.Tasks.FirstOrDefault(filter);
    }
}
