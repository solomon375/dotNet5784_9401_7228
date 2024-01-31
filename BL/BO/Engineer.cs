namespace BO;
/// <summary>
/// personal details of the engineer
/// </summary>
/// <param name="Id">engineer's ID</param>
/// <param name="Email">engineer's email</param>
/// <param name="Cost">engineer's salary per day</param>
/// <param name="Name">engineer's name</param>
/// <param name="Level">engineer's level</param>
/// <param name="Task">>the task's id and alias that the engineer currently work on</param>
public class Engineer
{
    public int Id { get; init; }
    public string? Email { get; set; }
    public double? Cost { get; set; }
    public string? Name { get; init; }
    public DO.EngineerExperience? Level { get; set; }
    public Tuple<BO.TaskInEngineer>? Task {  get; set; }
}