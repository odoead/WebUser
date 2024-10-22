namespace WebUser.Domain.exceptions;

public class RelatedEntityNotFoundException : NotFoundException
{
    public string EntityName { get; }
    public string ClassName { get; }
    public string FunctionName { get; }

    public RelatedEntityNotFoundException(string entityName, string className, string functionName)
        : base(GenerateMessage(entityName, className, functionName))
    {
        EntityName = entityName;
        ClassName = className;
        FunctionName = functionName;
    }

    private static string GenerateMessage(string entityName, string className, string functionName)
    {
        return $"Error with related entity '{entityName}' in {className}.{functionName}";
    }
}
