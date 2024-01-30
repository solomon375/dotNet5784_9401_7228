namespace Dal;
using System.Collections.Generic;

/// <summary>
/// Represents a static class responsible for managing in-memory data sources.
/// </summary>
/// <remarks>
/// The DataSource class contains static lists for tasks, engineers, and dependencies,
/// as well as configuration values for generating unique identifiers for tasks and dependencies.
/// </remarks>
internal static class DataSource
{
    /// <summary>
    /// Internal configuration settings for generating unique identifiers.
    /// </summary>
    internal static class Config
    {
        /// <summary>
        /// The starting point for task identifiers.
        /// </summary>
        internal const int StartTaskId = 1;

        /// <summary>
        /// The starting point for dependency identifiers.
        /// </summary>
        internal const int StartDependencyId = 1000;

        /// <summary>
        /// Private static field to track the next available task identifier.
        /// </summary>
        private static int nextTaskId = StartTaskId;

        /// <summary>
        /// Gets the next available task identifier and increments the counter.
        /// </summary>
        internal static int NextTaskId => nextTaskId++;

        /// <summary>
        /// Private static field to track the next available dependency identifier.
        /// </summary>
        private static int nextDependencyId = StartDependencyId;

        /// <summary>
        /// Gets the next available dependency identifier and increments the counter.
        /// </summary>
        internal static int NextDependencyId => nextDependencyId++;
    }

    /// <summary>
    /// Gets the in-memory list of tasks.
    /// </summary>
    /// <value>A List of Task objects.</value>
    internal static List<DO.Task> Tasks { get; } = new();

    /// <summary>
    /// Gets the in-memory list of engineers.
    /// </summary>
    /// <value>A List of Engineer objects.</value>
    internal static List<DO.Engineer> Engineers { get; } = new();

    /// <summary>
    /// Gets the in-memory list of dependencies.
    /// </summary>
    /// <value>A List of Dependency objects.</value>
    internal static List<DO.Dependency> Dependencys { get; } = new();

}
