using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hushigoeuf.Basic
{
    [HGBorders]
    [Serializable]
    public struct HGSprite
    {
        public Sprite Sprite;
        public Color Color;

        public void To(SpriteRenderer target)
        {
            if (Sprite == null) return;

            target.sprite = Sprite;
            target.color = Color;
        }

        public void To(Image target)
        {
            if (Sprite == null) return;

            target.sprite = Sprite;
            target.color = Color;
        }
    }

    public static class HGSpriteExtensions
    {
        public static void HGSet(this SpriteRenderer target, HGSprite sprite)
        {
            sprite.To(target);
        }

        public static void HGSet(this Image target, HGSprite sprite)
        {
            sprite.To(target);
        }
    }
}