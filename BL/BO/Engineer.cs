namespace BO;

public class Engineer
{
    //(הוספה עדכון)
    public int Id { get; set; }//מספר מזהה של המהנדס (-+)
    public string? Email { get; set; }//אמייל של המהנדס (++)
    public double? Cost { get; set; }//שכר של המהנדס (++)
    public string? Name { get; set; }//של המהנדס (++)
    public BO.EngineerExperience? Level { get; set; }//(רמת המהנדס (מתעדכן לפי השכר (--)
    public BO.TaskInEngineer? Task { get; set; }//המשימה שהמהנדס עובל עליה(+-)

    public override string ToString() => this.ToStringProperty();
}
