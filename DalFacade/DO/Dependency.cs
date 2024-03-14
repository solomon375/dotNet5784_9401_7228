namespace DO;

public record Dependency
(
    int Id,//מספר מזהה של התלות
    int? DependentTask = null,//מספר מזהה של המשימה המדוברת
    int? DependsOnTask = null//מספר מזהה של המשימה שהמימה הזו תלויה בה
)
{
    public Dependency() : this(0) { }
}
