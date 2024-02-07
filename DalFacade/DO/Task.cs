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
    DateTime? CreatedAtDate = null,
    TimeSpan? RequiredEffortTime = null,
    DateTime? DeadLine = null,
    DO.EngineerExperience? Complexity = null,
    DateTime? ScheduledDate = null,
    DateTime? StartedDate = null,
    DateTime? CompletedDate = null,
    string? Deliverable = null,
    string? Remarks = null,
    int? EngineerID = null
)
{
    public Task(int id, string? alias) : this(0) { }      //ctor for lvl 3
}
