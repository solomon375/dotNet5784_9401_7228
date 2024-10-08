﻿namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlAlreadyExistException : Exception
{
    public BlAlreadyExistException(string? message) : base(message) { }
    public BlAlreadyExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}

public class BlInvalidException : Exception
{
    public BlInvalidException(string? message) : base(message) { }
}

public class BlPrograpStartException : Exception
{
    public BlPrograpStartException(string? message) : base(message) { }
}

public class BlCantBeDeletedException : Exception
{
    public BlCantBeDeletedException(string? message) : base(message) { }
}

public class BlStartDataOfDependsOnTaskException : Exception
{
    public BlStartDataOfDependsOnTaskException(string? message) : base(message) { }
}

