using System;

using METL.InjectorMerges.Base;

namespace METL.InjectorMerges
{
    public class Timestamp : BaseInjectorMerge
    {
        public override string FIELD_NAME => "TIMESTAMP";

        public override string Merge() => DateTime.Now.ToString();
    }
}