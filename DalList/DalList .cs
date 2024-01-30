namespace Dal;
using DalApi;

/// <summary>
/// Represents a concrete implementation of the Data Access Layer using in-memory lists.
/// </summary>
/// <remarks>
/// This implementation is designed for testing and development purposes, providing an in-memory
/// storage solution for tasks, engineers, and dependencies.
/// </remarks>
sealed internal class DalList : IDal
{
    /// <summary>
    /// Gets the singleton instance of the DalList class.
    /// </summary>
    /// <remarks>
    /// The singleton pattern is used to ensure that only one instance of DalList is created
    /// throughout the application's lifetime.
    /// </remarks>
    public static IDal Instance { get; } = new DalList();

    /// <summary>
    /// Private constructor to prevent external instantiation of the DalList class.
    /// </summary>
    private DalList() { }

    /// <summary>
    /// Gets the implementation for the Task-related operations.
    /// </summary>
    /// <returns>An instance of the TaskImplementation class.</returns>
    public ITask Task => new TaskImplementation();

    /// <summary>
    /// Gets the implementation for the Engineer-related operations.
    /// </summary>
    /// <returns>An instance of the EngineerImplementation class.</returns>
    public IEngineer Engineer => new EngineerImplementation();

    /// <summary>
    /// Gets the implementation for the Dependency-related operations.
    /// </summary>
    /// <returns>An instance of the DependencyImplementation class.</returns>
    public IDependency Dependency => new DependencyImplementation();
}