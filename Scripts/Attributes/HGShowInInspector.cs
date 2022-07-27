using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Hushigoeuf
{
#if ODIN_INSPECTOR
    [IncludeMyAttributes]
    [FoldoutGroup(HGEditor.PATH_GROUP_SETTINGS)]
#endif
    public class HGShowInSettings : Attribute
    {
    }

#if ODIN_INSPECTOR
    [IncludeMyAttributes]
    [FoldoutGroup(HGEditor.PATH_GROUP_INPUTS)]
#endif
    public class HGShowInInputs : Attribute
    {
    }

#if ODIN_INSPECTOR
    [IncludeMyAttributes]
    [FoldoutGroup(HGEditor.PATH_GROUP_BINDINGS)]
#endif
    public class HGShowInBindings : Attribute
    {
    }

#if ODIN_INSPECTOR
    [IncludeMyAttributes]
    [FoldoutGroup(HGEditor.PATH_GROUP_EVENTS)]
#endif
    public class HGShowInEvents : Attribute
    {
    }

#if ODIN_INSPECTOR
    [IncludeMyAttributes]
    [FoldoutGroup(HGEditor.PATH_GROUP_DEBUG)]
#endif
    public class HGShowInDebug : Attribute
    {
    }

#if ODIN_INSPECTOR
    [IncludeMyAttributes]
    [FoldoutGroup(HGEditor.PATH_GROUP_DEBUG)]
    [ReadOnly]
    [ShowInInspector]
#endif
    public class HGDebugField : Attribute
    {
    }

#if ODIN_INSPECTOR
    [IncludeMyAttributes]
    [FoldoutGroup(HGEditor.PATH_GROUP_DEBUG)]
    [ShowInInspector]
#endif
    public class HGDebugEditField : Attribute
    {
    }
}