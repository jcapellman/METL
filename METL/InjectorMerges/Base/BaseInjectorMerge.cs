namespace METL.InjectorMerges.Base
{
    public abstract class BaseInjectorMerge
    {
        public abstract string FIELD_NAME { get; }

        public abstract string Merge();
    }
}