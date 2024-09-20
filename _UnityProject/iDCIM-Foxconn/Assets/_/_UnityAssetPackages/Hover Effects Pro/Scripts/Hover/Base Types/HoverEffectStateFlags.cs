using System;

namespace HoverEffectsPro
{
    [Flags]
    public enum HoverEffectStateFlags
    {
        Inactive = 0,
        Playing = 1,
        Entering = 2,
        Exiting = 4
    }
}
