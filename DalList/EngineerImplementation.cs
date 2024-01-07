namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        foreach(Engineer index in DataSource.Engineers)
        {
            if (index.Id == item.Id)
            {
                throw new Exception($"Engineer with ID={item.Id} already exist");
            }
        }
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();//need more ditails 
    }

    public Engineer? Read(int id)
    {
        foreach (Engineer Index in DataSource.Engineers)
        {
            if (Index.Id == id)
            {
                return Index;
            }
        }
        return null;
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        foreach(Engineer Index in DataSource.Engineers)
        {
            if (Index.Id == item.Id)
            {
                DataSource.Engineers.Remove(Index);

                DataSource.Engineers.Insert(0, item);
            }
        }

        throw new Exception($"Engineer with ID={item.Id} does Not exist"); 
    }
}
