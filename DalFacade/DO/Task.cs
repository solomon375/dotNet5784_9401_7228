namespace DO;

public record Task
(
    //(הוספה עדכון)
    int Id,//מספר מזהה (--)

    string? Alias = null,//כינוי (++)

    string? Describtion = null,//תיאור (++)

    DO.EngineerExperience? Complexity = null,//רמת הקושי של המשימה (-+)

    DO.Status? Status = null,//סטטוס מצב המשימה (--)

    DateTime? ScheduledDate = null,//תאריך בו מהנדס יכול לקחת את המשימה (--)

    TimeSpan? RequiredEffortTime = null,//משך הזמן לביצוע המשימה (++)

    DateTime? DeadLine = null,//תאריך סיום אחרון אפשרי (--)

    DateTime? CreatedAtDate = null,

    DateTime? StartedDate = null,//תאריך בו מהנדס לקח את המשימה (--)

    DateTime? CompletedDate = null,//תאריך בו המהנדס סיים את המשימה (--)

    string? Deliverable = null,// ? (++)

    string? Remarks = null,//הערות (++)

    int? EngineerID = null//המספר מזהה של המהנדס שלקח את המשימה (--)
)
{
    public Task() : this(0) { } 
}
