namespace WiSave.Core.EventStore.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class EventTypeAttribute(string typeName) : Attribute
{
    public string TypeName { get; } = typeName ?? throw new ArgumentNullException(nameof(typeName));
}