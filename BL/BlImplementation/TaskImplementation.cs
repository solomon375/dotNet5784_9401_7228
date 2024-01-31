using BlApi;

namespace BlImplementation;
internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Task item)
    {
        DO.Task doTask = new DO.Task(item.Id, item.Alias);

        if(doTask.Alias =="" ) 
        {
            throw new BO.BlInvalidException("INVALID ALIAS");
        }

        /*תוסיף תלויות עבור משימות קודמות מתוך רשימת המשימות הקיימת
אם הנתונים תקינים - תבצע ניסיון בקשות הוספה לשכבת הנתונים
*/
        try
        {
            return _dal.Task.Create(doTask);
        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistException($"Student with ID={item.Id} already exists", ex);
        }

    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Task? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void UpdataOrAddDate(int id, DateTime date)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task item)
    {
        throw new NotImplementedException();
    }
}
