﻿using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Hushigoeuf
{
#if ODIN_INSPECTOR
    [IncludeMyAttributes]
    [ListDrawerSettings(Expanded = true, ShowPaging = false)]
#endif
    public class HGListDrawerSettings : Attribute
    {
    }
}