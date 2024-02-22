﻿namespace BO;
/// <summary>
/// created new task with all properties
/// </summary>
/// <param name="Id">unique id(created automaticly)</param>
/// <param name="Alias">nickname</param>
/// <param name="Describtion">explantion</param>
/// <param name="status">task's status</param>
/// <param name="Dependencies">list of dependencies</param>
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
public class Task
{
    public int Id { get; set; }//--
    public string? Alias { get; set; }//הוספה עדכון
    public string? Describtion { get; set; }//הוספה עדכון
    public BO.Status? status { get; set; }//עדכון
    public List<BO.TaskInList>? Dependencies { get; set; }//--
    public bool? IsMilestone { get; set; }//--
    public DateTime? CreatedAtDate { get; set; }//--
    public BO.EngineerExperience? Complexity { get; set; }//הוספה ועדכון
    public DateTime? ScheduledDate { get; set; }//הוספה ועדכון
    public DateTime? StartedDate { get; set; }//--
    public TimeSpan? RequiredEffortTime { get; set; }//הוספה עדכון
    public DateTime? DeadLine { get; set; }//--
    public DateTime? CompletedDate { get; set; }//
    public string? Deliverable { get; set; }//הוספה ועדכון
    public string? Remarks { get; set; }//עדכון והוספה
    public BO.EngineerInTask? Engineer { get; set; }//

    public override string ToString() => this.StringProperty();
}
