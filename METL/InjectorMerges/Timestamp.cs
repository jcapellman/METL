using System.Globalization;

using METL.InjectorMerges.Base;

namespace METL.InjectorMerges;

/// <summary>
/// Merge strategy for inserting current timestamp.
/// </summary>
public class Timestamp : BaseInjectorMerge
{
    /// <inheritdoc/>
    public override string FIELD_NAME => "TIMESTAMP";

    /// <inheritdoc/>
    public override string? Merge(string? argument = null) => DateTime.Now.ToString(CultureInfo.InvariantCulture);
}
