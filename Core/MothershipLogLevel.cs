using System;

namespace LegedaryTools.Mothership
{
    [Flags]
    public enum MothershipLogLevel
    {
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 4,
        All = 8,
    }
}