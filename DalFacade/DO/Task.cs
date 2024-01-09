namespace DO;
/// <summary>
/// created new task with all properties
/// </summary>
/// <param name="Id">unique id(created automaticly)</param>
/// <param name="Alias">nickname</param>
/// <param name="Describtion">explantion</param>
/// <param name="IsMilestone">is it milestone</param>
/// <param name="CreatedAtDate">when the task added to the system</param>
/// <param name="ScheduledDate">planed start date</param>
/// <param name="StartedDate">the real start date</param>
/// <param name="RequiredEffortTime">how many men-days needed to the task</param>
/// <param name="DeadLine">the lastest completed date</param>
/// <param name="CompletedDate">real completion time</param>
/// <param name="Deliverable">description of deliverables for MS completion</param>
/// <param name="Remarks">free remarks for project meeting</param>
/// <param name="EngineerID">id of the engineer</param>
/// <param name="Complexity">Complexity of task and needed minimum expiriense of engineer</param>
public record Task
(
    int Id,
    string? Alias = null,
    string? Describtion = null,
    bool? IsMilestone = null,
    DO.EngineerExperience? Complexity = null,
    DateTime? CreatedAtDate = null,
    DateTime? ScheduledDate = null,
    DateTime? StartedDate = null,
    TimeSpan? RequiredEffortTime = null,
    DateTime? DeadLine = null,
    DateTime? CompletedDate = null,
    string? Deliverable = null,
    string? Remarks = null,
    int? EngineerID = null
)
{
    public Task() : this(0) { }      //ctor for lvl 3
}
    


/*//static int readInt()
    {
        int value = 0;
        while (!int.TryParse(Console.ReadLine(), out value))
        {
            Console.WriteLine("Wrong number! enter again");
        }
        return value;
    }

    static void readDouble(out double number)
{
    while (!double.TryParse(Console.ReadLine(), out number))
    {
        Console.WriteLine("Wrong number! enter again");
    }
}
static void readDate(out DateTime date)
{
    while (!DateTime.TryParse(Console.ReadLine(), out date))
    {
        Console.WriteLine("Wrong number! enter again");
    }
}

static Task Create()
{
    Console.WriteLine("Enter alias");
    string? alias = Console.ReadLine();

    readDate(out DateTime createdAtDate);


    var task = new Task(alias, createdAtDate);
    return task;

    task =  _dal.task.Create(task);
    _dal.Task.Add(task);
}

internal static class PrintExtansion
{
    internal static string PrintList<TEntity>(this IEnumerable<TEntity> entities)
        => string.Join(Environment.NewLine, entities);
}*/

