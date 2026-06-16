namespace METL.InjectorMerges.Base;

/// <summary>
/// Base class for injector merge strategies.
/// </summary>
public abstract class BaseInjectorMerge
{
    /// <summary>
    /// Gets the field name used for mail merge replacement.
    /// </summary>
    public abstract string FIELD_NAME { get; }

    /// <summary>
    /// Performs the merge operation with the provided argument.
    /// </summary>
    /// <param name="argument">The argument value to merge.</param>
    /// <returns>The merged result.</returns>
    public abstract string? Merge(string? argument = null);
}
