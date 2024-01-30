namespace Dal;

/// <summary>
/// Provides configuration settings for the Data Access Layer.
/// </summary>
internal static class Config
{
    // XML file name for data configuration
    static string s_data_config_xml = "data-config";

    /// <summary>
    /// Gets the next available Task ID and increments it.
    /// </summary>
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }

    /// <summary>
    /// Gets the next available Dependency ID and increments it.
    /// </summary>
    internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }
}
