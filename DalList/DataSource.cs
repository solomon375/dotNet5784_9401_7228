namespace Dal;
using System.Collections.Generic;

internal static class DataSource
{
    internal static class Config
    {
        internal const int StartTaskId = 1;

        private static int nextTaskId = StartTaskId;
        internal static int NextTaskId => nextTaskId++;


        internal const int StartDependencyId = 1000;

        private static int nextDependencyId = StartDependencyId;
        internal static int NextDependencyId => nextDependencyId++;
    }

    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.Dependency> Dependencys { get; } = new();

}
