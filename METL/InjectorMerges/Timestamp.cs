using System;
using System.Globalization;

using METL.InjectorMerges.Base;

namespace METL.InjectorMerges
{
    public class Timestamp : BaseInjectorMerge
    {
        public override string FIELD_NAME => "TIMESTAMP";

        public override string Merge(string argument = null) => DateTime.Now.ToString(CultureInfo.InvariantCulture);
    }
}