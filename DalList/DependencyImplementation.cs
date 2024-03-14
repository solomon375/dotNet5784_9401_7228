namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        bool exist = DataSource.Dependencys.Any(dependency => dependency.DependentTask == item.DependentTask &&
        dependency.DependsOnTask == item.DependsOnTask);
        if (exist == false)
        {
            int next = DataSource.Config.NextDependencyId;
            Dependency dependency = item with { Id =  next };
            DataSource.Dependencys.Add(dependency);
            return item.Id;
        }
        return -1;
    }

    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Dependencys
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Dependencys
               select item;
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependencys.FirstOrDefault(filter);
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencys.FirstOrDefault(item => item.Id == id);
    }

    public void Update(Dependency item)
    {
        foreach (Dependency Index in DataSource.Dependencys)
        {
            if (Index.Id == item.Id)
            {
                DataSource.Dependencys.Remove(Index);
                DataSource.Dependencys.Insert(0, item);
                return;
            }
        }
        throw new DalNotExistException($"dependency with ID={item.Id} does Not exist");
    }

    public void Delete(int id)
    {
        Dependency foundDependency = DataSource.Dependencys.FirstOrDefault(dep => dep.Id == id)
                ?? throw new DalNotExistException($"Dependency with ID={id} does not exist"); ;

        DataSource.Dependencys.Remove(foundDependency);
    }
}
