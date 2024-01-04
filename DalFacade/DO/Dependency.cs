﻿namespace DO;
/// <summary>
/// depends between mission
/// </summary>
/// <param name="Id">task's id</param>
/// <param name="DependentTask">id of mission that depend on that mission</param>
/// <param name="DependsOnTask">id of mission that this mission based on</param>
public record Dependency
(
    int? Id = null,
    int? DependentTask = null,
    int? DependsOnTask = null
);
