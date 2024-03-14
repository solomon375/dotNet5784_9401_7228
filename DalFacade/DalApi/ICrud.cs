using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi;

public interface ICrud<T> where T : class
{
    int Create(T item);
    IEnumerable<T> ReadAll(Func<T, bool>? filter = null);
    T? Read(Func<T, bool> filter);
    T? Read(int id);
    void Update(T item);
    void Delete(int id);
}

