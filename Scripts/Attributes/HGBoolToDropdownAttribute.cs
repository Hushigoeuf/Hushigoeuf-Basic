using System;
using System.Collections;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
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
        public static IEnumerable Values = new ValueDropdownList<bool>()
        {
            {"False", false},
            {"True", true},
        };
    }
}