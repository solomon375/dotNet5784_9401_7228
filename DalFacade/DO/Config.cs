namespace DO;
/// <summary>
/// status on mission
/// </summary>
/// <param name="Status">mission's status</param>
/// <param name="StartDate">mission's real start date</param>
/// <param name="EndDate">mission's real end date</param>
public record Config
(
    DO.Status? Status,
    DateTime? StartDate,
    DateTime? EndDate
);
