namespace FamilySafety.Domain.Exceptions;

/// <summary>
/// Base exception for domain-specific errors
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when an entity is not found
/// </summary>
public class EntityNotFoundException : DomainException
{
    public string EntityName { get; }
    public object Key { get; }

    public EntityNotFoundException(string entityName, object key)
        : base($"Entity '{entityName}' with key '{key}' was not found.")
    {
        EntityName = entityName;
        Key = key;
    }
}

/// <summary>
/// Exception thrown when business rules are violated
/// </summary>
public class BusinessRuleViolationException : DomainException
{
    public string RuleName { get; }

    public BusinessRuleViolationException(string ruleName, string message)
        : base(message)
    {
        RuleName = ruleName;
    }
}

/// <summary>
/// Exception thrown when user is not authorized for an operation
/// </summary>
public class UnauthorizedAccessException : DomainException
{
    public UnauthorizedAccessException(string message) : base(message) { }
}
