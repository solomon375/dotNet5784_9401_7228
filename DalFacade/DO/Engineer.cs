namespace DO;
/// <summary>
/// personal details of the engineer
/// </summary>
/// <param name="Id">engineer's ID</param>
/// <param name="Email">engineer's email</param>
/// <param name="Cost">engineer's salary per day</param>
/// <param name="Name">engineer's name</param>
/// <param name="level">engineer's level</param>
public record Engineer
(
    int? Id = null,
    string? Email = null,
    double? Cost = null,
    string? Name = null,
    DO.EngineerExperience? level = null
);
