namespace WiSave.Core.EventStore.Serialization;

public static class TypeProvider
{
    public static Type? GetFirstMatchingTypeFromCurrentDomainAssembly(string typeName)
    {
        var type = Type.GetType(typeName);
        if (type != null) return type;
        
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = assembly.GetType(typeName);
            if (type != null) return type;
        }

        return null;
    }
}