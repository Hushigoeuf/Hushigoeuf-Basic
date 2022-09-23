using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Hushigoeuf.Basic
{
#if ODIN_INSPECTOR
    [IncludeMyAttributes]
    [ReadOnly]
#endif
    public class HGReadOnlyAttribute : Attribute
    {
    }

#if ODIN_INSPECTOR
    [IncludeMyAttributes]
    [Required]
#endif
    public class HGRequiredAttribute : Attribute
    {
    }

#if ODIN_INSPECTOR
    [IncludeMyAttributes]
    [ShowInInspector]
#endif
    public class HGShowInInspectorAttribute : Attribute
    {
    }

#if ODIN_INSPECTOR
    [IncludeMyAttributes]
    [AssetsOnly]
#endif
    public class HGAssetsOnlyAttribute : Attribute
    {
    }
}