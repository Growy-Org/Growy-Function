namespace FamilyMerchandise.Function.Exceptions;

public class DeletionFailureException : Exception
{
    public string? TableName { get; }
    public string? ForeignKeyName { get; }

    // Default constructor
    public DeletionFailureException()
    {
    }

    // Constructor with a custom message
    public DeletionFailureException(string message)
        : base(message)
    {
    }

    // Constructor with a custom message and inner exception
    public DeletionFailureException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    // Constructor with custom message and additional details
    public DeletionFailureException(string tableName, string foreignKeyName)
        : base($"Deletion failed due to a foreign key constraint on table '{tableName}' (ForeignKey: '{foreignKeyName}').")
    {
        TableName = tableName;
        ForeignKeyName = foreignKeyName;
    }
}