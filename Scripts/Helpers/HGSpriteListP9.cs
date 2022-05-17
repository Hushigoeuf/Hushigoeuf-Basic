using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Hushigoeuf
{
    public enum GESpriteListP9Points
    {
        LeftTop,
        Top,
        RightTop,
        Left,
        Middle,
        Right,
        LeftBottom,
        Bottom,
        RightBottom
    }

    [CreateAssetMenu(menuName = HGEditor.PATH_ASSET_COMMON + nameof(HGSpriteListP9))]
    public sealed class HGSpriteListP9 : HGScriptableObject
    {
        private const int MATRIX_WIDTH = 3;
        private const int MATRIX_HEIGHT = 3;

        [HideInInspector] [SerializeField] private Sprite[] _sprites = new Sprite[MATRIX_WIDTH * MATRIX_HEIGHT];

        public Dictionary<GESpriteListP9Points, Sprite> Sprites => new Dictionary<GESpriteListP9Points, Sprite>
        {
            {GESpriteListP9Points.LeftTop, GetSprite(GESpriteListP9Points.LeftTop)},
            {GESpriteListP9Points.Top, GetSprite(GESpriteListP9Points.Top)},
            {GESpriteListP9Points.RightTop, GetSprite(GESpriteListP9Points.RightTop)},
            {GESpriteListP9Points.Left, GetSprite(GESpriteListP9Points.Left)},
            {GESpriteListP9Points.Middle, GetSprite(GESpriteListP9Points.Middle)},
            {GESpriteListP9Points.Right, GetSprite(GESpriteListP9Points.Right)},
            {GESpriteListP9Points.LeftBottom, GetSprite(GESpriteListP9Points.LeftBottom)},
            {GESpriteListP9Points.Bottom, GetSprite(GESpriteListP9Points.Bottom)},
            {GESpriteListP9Points.RightBottom, GetSprite(GESpriteListP9Points.RightBottom)},
        };

        public Sprite GetSprite(GESpriteListP9Points point)
        {
            switch (point)
            {
                case GESpriteListP9Points.Top:
                    return _sprites[1];
                case GESpriteListP9Points.RightTop:
                    return _sprites[2];
                case GESpriteListP9Points.Left:
                    return _sprites[3];
                case GESpriteListP9Points.Middle:
                    return _sprites[4];
                case GESpriteListP9Points.Right:
                    return _sprites[5];
                case GESpriteListP9Points.LeftBottom:
                    return _sprites[6];
                case GESpriteListP9Points.Bottom:
                    return _sprites[7];
                case GESpriteListP9Points.RightBottom:
                    return _sprites[8];
                default:
                    return _sprites[0];
            }
        }

#if UNITY_EDITOR && ODIN_INSPECTOR
        [TableMatrix(SquareCells = true)] [OnValueChanged(nameof(_EditorOnValueChanged))] [ShowInInspector]
        private Sprite[,] _editorSprites = new Sprite[MATRIX_WIDTH, MATRIX_HEIGHT];

        [OnInspectorInit]
        private void _EditorOnInspectorInit()
        {
            for (var y = 0; y < MATRIX_HEIGHT; y++)
            for (var x = 0; x < MATRIX_WIDTH; x++)
                _editorSprites[x, y] = _sprites[x + MATRIX_WIDTH * y];
        }

        private void _EditorOnValueChanged()
        {
            for (var y = 0; y < MATRIX_HEIGHT; y++)
            for (var x = 0; x < MATRIX_WIDTH; x++)
                _sprites[x + MATRIX_WIDTH * y] = _editorSprites[x, y];
        }
#endif
    }
}