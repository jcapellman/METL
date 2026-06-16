using METL.InjectorMerges.Base;

namespace METL.InjectorMerges;

/// <summary>
/// Merge strategy for embedding a unique GUID identifier.
/// Useful for tracking or identifying specific payloads.
/// </summary>
public class UniqueIdMerge : BaseInjectorMerge
{
    /// <inheritdoc/>
    public override string FIELD_NAME => "UNIQUE_ID";

    /// <inheritdoc/>
    public override string? Merge(string? argument = null)
    {
        // Use provided GUID or generate new one
        var guid = string.IsNullOrEmpty(argument) ? Guid.NewGuid().ToString() : argument;

        return $"string uniqueId = \"{guid}\";";
    }
}
