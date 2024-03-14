namespace DO;

public record Engineer
(
    //(הוספה עדכון)

    int Id,//מספר מזהה של המהנדס (-+)

    string? Email = null,//אמייל של המהנדס (++)

    double? Cost = null,//שכר של המהנדס (++)

    string? Name = null,//של המהנדס (++)

    DO.EngineerExperience? Level = null,//(רמת המהנדס (מתעדכן לפי השכר (--)

    int? Task = null//המשימה שהמהנדס עובל עליה(+-)
)
{
    public Engineer() : this(0) { }
}
