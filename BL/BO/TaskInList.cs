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
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public BO.Status? Status { get; set; }
}
