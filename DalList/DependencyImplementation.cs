namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{
    //method for create dependency
    public int Create(Dependency item)
    {
        int next = DataSource.Config.NextDependencyId;

        Dependency item1 = item with { Id = next };

        DataSource.Dependencys.Add(item1);

        return item1.Id;
    }

    //method for delete dependency
    public void Delete(int id)
    {
        foreach(Dependency index in DataSource.Dependencys)
        {
            if(index.Id == id)
            {
                DataSource.Dependencys.Remove(index);
                return;
            }
        }
        throw new DalNotExistException($"Dependencys with ID={id} does Not exist");
    }

    //method for read dependency
    public Dependency? Read(int id)
    {
        return DataSource.Dependencys.FirstOrDefault(item => item.Id == id);
    }

    //method for read all dependency
    public List<Dependency> ReadAll()
    {
        return DataSource.Dependencys.Select(item => item).ToList();
    }

    //method for update dependency
    public void Update(Dependency item)
    {
        foreach (Dependency Index in DataSource.Dependencys)
        {
            if (Index.Id == item.Id)
            {
                DataSource.Dependencys.Remove(Index);

                DataSource.Dependencys.Insert(0, item);
            }
        }

        throw new DalNotExistException($"dependency with ID={item.Id} does Not exist"); 
    }
}
