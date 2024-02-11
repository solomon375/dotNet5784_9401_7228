using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;
/// <summary>
/// crud method for engineer
/// </summary>
public interface IEngineer
{
    public IEnumerable<BO.Engineer?> ReadAll(Func<DO.Engineer, bool>? filter = null);
    public BO.Engineer? Read(int id);
    public int Create(BO.Engineer item);
    public void Delete(int id);
    public void Update(BO.Engineer item);

}
