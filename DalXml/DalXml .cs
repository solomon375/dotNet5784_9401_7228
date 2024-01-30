using DalApi;
using System.Diagnostics;
namespace Dal;

/// <summary>
/// Implementation of the Data Access Layer using XML storage.
/// </summary>
//stage 3
sealed internal class DalXml : IDal
{
    /// <summary>
    /// Gets the singleton instance of DalXml.
    /// </summary>
    public static IDal Instance { get; } = new DalXml();

    // Private constructor to prevent external instantiation
    private DalXml() { }

    /// <summary>
    /// Gets the implementation for accessing task-related operations.
    /// </summary>
    public ITask Task => new TaskImplementation();

    /// <summary>
    /// Gets the implementation for accessing engineer-related operations.
    /// </summary>
    public IEngineer Engineer => new EngineerImplementation();

    /// <summary>
    /// Gets the implementation for accessing dependency-related operations.
    /// </summary>
    public IDependency Dependency => new DependencyImplementation();
}
