namespace BO;

public class Task
{
    //(הוספה עדכון)
    public int Id { get; init; }//--
    public string? Alias { get; set; }//כינוי (++)
    public string? Describtion { get; set; }//תיאור (++)
    public BO.EngineerExperience? Complexity { get; set; }//רמת הקושי של המשימה (-+)
    public BO.Status? Status { get; set; }//סטטוס מצב המשימה (--)
    public DateTime? ScheduledDate { get; set; }//תאריך בו מהנדס יכול לקחת את המשימה (--)
    public TimeSpan? RequiredEffortTime { get; set; }//משך הזמן לביצוע המשימה (++)
    public DateTime? DeadLine { get; set; }//תאריך סיום אחרון אפשרי (--)
    public DateTime? CreatedAtDate { get; set; }//נוצר בתאריך(--)
    public DateTime? StartedDate { get; set; }//תאריך בו מהנדס לקח את המשימה (--)
    public DateTime? CompletedDate { get; set; }//תאריך בו המהנדס סיים את המשימה (--)
    public string? Deliverable { get; set; }// ? (++)
    public string? Remarks { get; set; }//הערות (++)
    public BO.EngineerInTask? EngineerID { get; set; }//המספר מזהה של המהנדס שלקח את המשימה (--)
    public List<BO.TaskInList>? Dependencies { get; set; }//משימות שמשימה זו תלויה בהם. כל המשימות ברמה אחת נמוכה יותר(--)

    public override string ToString() => this.ToStringProperty();
}
