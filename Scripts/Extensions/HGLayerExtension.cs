using UnityEngine;

namespace Hushigoeuf
{
    public static class HGLayerExtension
    {
        public static bool HGLayerInLayerMask(this LayerMask layerMask, int layer) => ((1 << layer) & layerMask) != 0;
    }
}