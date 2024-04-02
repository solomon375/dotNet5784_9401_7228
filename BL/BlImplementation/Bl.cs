/// <summary>
/// Namespace containing the business logic implementation.
/// </summary>
namespace BlImplementation;

/// <summary>
/// Imports interfaces and classes from other namespaces.
/// </summary>
using BlApi;
using BO;

/// <summary>
/// Implements the business logic interface.
/// </summary>
internal class Bl : IBl
{
    /// <summary>
    /// Property to access task functionality.
    /// </summary>
    public ITask task => new TaskImplementation(this);

    /// <summary>
    /// Property to access engineer functionality.
    /// </summary>
    public IEngineer engineer => new EngineerImplementation(this);

    /// <summary>
    /// Property representing the system clock.
    /// </summary>
    public Clock clock => new Clock();

    /// <summary>
    /// Static field to store the current date and time.
    /// </summary>
    private static DateTime s_Now = DateTime.Now.Date;

    /// <summary>
    /// Property to get and set the current date and time.
    /// </summary>
    public DateTime Now { get { return s_Now; } private set { s_Now = value; } }

    /// <summary>
    /// Advances the current time by one month.
    /// </summary>
    public void AdvanceTimeByMonth()
    {
        Now = Now.AddMonths(1);
    }

    /// <summary>
    /// Advances the current time by one day.
    /// </summary>
    public void AdvanceTimeByDay()
    {
        Now = Now.AddDays(1);
    }

    /// <summary>
    /// Advances the current time by one hour.
    /// </summary>
    public void AdvanceTimeByHour()
    {
        Now = Now.AddHours(1);
    }

    /// <summary>
    /// Initializes the system clock with the current time.
    /// </summary>
    public void InitializeClock()
    {
        Now = DateTime.Now;
    }

    /// <summary>
    /// Updates the status of tasks based on the current time.
    /// </summary>
    public void UpdateTasksStatus()
    {
        DateTime currentTime = Now;
        foreach (var i in task.ReadAll())
        {
            if (i.Status == Status.Scheduled && currentTime > (i.DeadLine-i.RequiredEffortTime))
            {
                i.Status = Status.InJeopardy;
                i.DeadLine += i.RequiredEffortTime;
                foreach (var j in task.ReadAll())
                {
                    TaskInList t = new TaskInList() { Id=i.Id, Alias=i.Alias };
                    if (j.Dependencies.Contains(t))
                    {
                        j.ScheduledDate+=i.RequiredEffortTime;
                        j.DeadLine += i.RequiredEffortTime;
                        task.Update(j);
                    }
                }
                task.Update(i);
            }
            else if (i.Status == Status.OnTrack && currentTime > i.DeadLine)
            {
                i.Status = Status.InJeopardy;
                i.DeadLine += i.RequiredEffortTime;
                foreach (var j in task.ReadAll())
                {
                    if (j.Dependencies != null)
                    {
                        TaskInList t = new TaskInList() { Id=i.Id, Alias=i.Alias, Description=i.Describtion, Status=i.Status };
                        if (j.Dependencies.Contains(t))
                        {
                            j.ScheduledDate+=i.RequiredEffortTime;
                            j.DeadLine += i.RequiredEffortTime;
                            task.Update(j);
                        }
                    }
                }
                task.Update(i);
            }
            else if (i.Status == Status.InJeopardy && currentTime > i.DeadLine)
            {
                foreach (var j in task.ReadAll())
                {
                    TaskInList t = new TaskInList() { Id=i.Id, Alias=i.Alias };
                    if (j.Dependencies.Contains(t))
                    {
                        j.ScheduledDate+=i.RequiredEffortTime;
                        j.DeadLine += i.RequiredEffortTime;
                        task.Update(j);
                    }
                }
                task.Update(i);
            }
            else if (i.Status == Status.Unscheduled && currentTime > i.ScheduledDate)
            {
                i.Status=Status.Scheduled;
                task.Update(i);
            }
        }
    }
}
