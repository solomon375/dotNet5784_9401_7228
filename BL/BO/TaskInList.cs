namespace BO;
/// <summary>
///  help method for Task
/// </summary>
/// <param name="Id">unique id of the mission</param>
/// <param name="Description">Description of the mission</param>
/// <param name="Alias">nickname</param>
/// <param name="Status">Status of the mission</param>
public class TaskInList
{
    public int Id { get; init; }
    public string? Description { get; init; }
    public string? Alias { get; init; }
    public BO.Status? Status { get; init; }

    public override string ToString() => this.ToStringProperty();
}
