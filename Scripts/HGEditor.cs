using UnityEditor;
using UnityEngine;

namespace Hushigoeuf.Basic
{
    /// <summary>
    /// Класс для работы в режиме редактора.
    /// Содержит константы именований и путей, чтобы стандартизировать код.
    ///
    /// Формат AddComponentMenu:
    /// [AddComponentMenu(HGEditor.PATH_MENU + nameof(CLASS_NAME))]
    ///
    /// Формат CreateAssetMenu:
    /// [CreateAssetMenu(menuName = HGEditor.PATH_ASSET + nameof(CLASS_NAME))]
    /// </summary>
    public partial class HGEditor
    {
        // Общие константы:
        // ----------------------------------------------------------------
        public const string SEPARATOR = "/";
        public const string SPACE = "\n";
        public const string TAB = "\t";
        public const string SPACE_TAB = SPACE + TAB;
        public const string BASE = nameof(Hushigoeuf);
        public const string COMMON = "Common";
        public const string GUI = "UI";
        public const string TP = "ThirdParty";
        public const string CURRENT = "Current";

        // Константы наименований групп в инспекторе (для OdinInspector):
        // ----------------------------------------------------------------
        public const string GROUP_SETTINGS = "SETTINGS";
        public const string GROUP_INPUTS = "INPUTS";
        public const string GROUP_BINDINGS = "BINDINGS";
        public const string GROUP_EVENTS = "EVENTS";
        public const string GROUP_DEBUG = "DEBUG";

        // Константы для стандартизации групп в инспекторе (для OdinInspector):
        // ----------------------------------------------------------------
        public const string PATH_GROUP_SETTINGS = GROUP_SETTINGS;

        public const string PATH_GROUP_INPUTS = GROUP_INPUTS;
        public const string PATH_GROUP_BINDINGS = GROUP_BINDINGS;
        public const string PATH_GROUP_EVENTS = GROUP_EVENTS;
        public const string PATH_GROUP_DEBUG = GROUP_DEBUG;

        // Константы для стандартизации путей:
        // ----------------------------------------------------------------
        public const string PATH_RESOURCES = HGResources.START_PATH;
        public const string PATH_ASSET = BASE + SEPARATOR;
        public const string PATH_ASSET_COMMON = PATH_ASSET + COMMON + SEPARATOR;
        public const string PATH_ASSET_GUI = PATH_ASSET + GUI + SEPARATOR;
        public const string PATH_ASSET_TP = PATH_ASSET + TP + SEPARATOR;
        public const string PATH_ASSET_CURRENT = PATH_ASSET + CURRENT + SEPARATOR;
        public const string PATH_MENU = BASE + SEPARATOR;
        public const string PATH_MENU_COMMON = PATH_MENU + COMMON + SEPARATOR;
        public const string PATH_MENU_GUI = PATH_MENU + GUI + SEPARATOR;
        public const string PATH_MENU_TP = PATH_MENU + TP + SEPARATOR;
        public const string PATH_MENU_CURRENT = PATH_MENU + CURRENT + SEPARATOR;

        // Константы для стандартизации MenuItem:
        // ----------------------------------------------------------------
        public const string MENU_ITEM_PATH = nameof(GameObject) + SEPARATOR + BASE + SEPARATOR;

        public const int MENU_ITEM_PRIORITY = 0;

        public static bool IsPlaying
        {
            get
            {
#if UNITY_EDITOR
                return EditorApplication.isPlaying;
#endif
                return false;
            }
        }

        public static bool IsPaused
        {
            get
            {
#if UNITY_EDITOR
                return EditorApplication.isPaused;
#endif
                return false;
            }
        }

        public static void Play()
        {
#if UNITY_EDITOR
            if (!IsPlaying)
                EditorApplication.isPlaying = true;
#endif
        }

        public static void Pause()
        {
#if UNITY_EDITOR
            if (IsPlaying)
                EditorApplication.isPaused = true;
#endif
        }

        public static void Stop()
        {
#if UNITY_EDITOR
            if (IsPlaying)
            {
                EditorApplication.isPlaying = false;
            }
            else if (IsPaused)
            {
                EditorApplication.isPaused = false;
                EditorApplication.isPlaying = false;
            }
#endif
        }
    }
}