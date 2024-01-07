namespace DalTest;

using DalApi;
using DO;
public static class Initialization
{
    private static ITask? s_delTask;
    private static IEngineer? s_delEngineer;
    private static IDependency? s_delDependency;

    private static readonly Random s_rand = new();
}
