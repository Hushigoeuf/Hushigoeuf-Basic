using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using System.Collections;
#endif

namespace Hushigoeuf
{
    /// <summary>
    /// Зависит от OdinInspector и просто визуально превращает bool в выпадающий список.
    /// </summary>
#if ODIN_INSPECTOR
    [IncludeMyAttributes]
    [ValueDropdown("@" + nameof(HGBoolToDropdownAttribute) + "." + nameof(Values))]
#endif
    public class HGBoolToDropdownAttribute : Attribute
    {
#if ODIN_INSPECTOR
        public static IEnumerable Values = new ValueDropdownList<bool>()
        {
            {"False", false},
            {"True", true},
        };
#endif
    }
}