namespace BlImplementation;

using BlApi;
using BO;

internal class Bl : IBl
{
    public ITask task => new TaskImplementation(this);

    public IEngineer engineer => new EngineerImplementation(this);

    public Clock clock => new Clock();

    private static DateTime s_Now = DateTime.Now.Date;

    public DateTime Now { get { return s_Now; } private set { s_Now = value; } }

    public void AdvanceTimeByMonth()
    {
        Now = Now.AddMonths(1);
    }

    public void AdvanceTimeByDay()
    {
        Now = Now.AddDays(1);
    }

    public void AdvanceTimeByHour()
    {
        Now = Now.AddHours(1);
    }

    public void InitializeClock()
    {
        Now = DateTime.Now;
    }

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
