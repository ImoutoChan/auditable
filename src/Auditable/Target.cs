using Newtonsoft.Json.Linq;

namespace Auditable;

public class Target
{
    public required string Id { get; set; }
    
    public required string Type { get; set; }
    
    public object? Instance { get; set; }
    
    public string? Before { get; set; }
    
    public JToken? Delta { get; set; }
    
    public required AuditType AuditType { get; set; }
    
    public required ActionStyle ActionStyle { get; set; }
}
