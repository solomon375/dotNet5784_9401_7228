namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int next = DataSource.Config.NextDependencyId;

        Dependency item1 = item with { Id = next };

        DataSource.Dependencys.Add(item1);

        return item1.Id;
    }

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

    public Dependency? Read(int id)
    {
        foreach (Dependency Index in DataSource.Dependencys)
        {
            if (Index.Id == id)
            {
                return Index;
            }
        }
        return null;
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencys);
    }

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
