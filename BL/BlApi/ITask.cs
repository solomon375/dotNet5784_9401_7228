using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;
/// <summary>
/// crud methad for task 
/// </summary>
public interface ITask
{
    public IEnumerable<BO./*TaskInList?*/Task?> ReadAll(Func<DO.Task, bool>? filter = null);
    public BO.Task? Read(int id);
    public int Create(BO.Task item);
    public void Delete(int id);
    public void Update(BO.Task item);
    public void UpdataOrAddDate(int id, DateTime date);
}
