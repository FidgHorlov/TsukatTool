#region Info

// Tsukat tool - by Horlov Andrii (andreygorlovv@gmail.com)
// Tsukat -> https://tsukat.com/

#endregion

using UnityEditor;
using UnityEngine;

namespace TsukatTool.Editor.CustomSceneManager
{
    public class CustomSceneWindowsManager : EditorWindow
    {
        private const string SceneManagerPath = "Tsukat/Custom Scene Manager/Scene Manager";
        private const string PrepareBuildPath = "Tsukat/Custom Scene Manager/Prepare build";
        private const string SceneManagerSettingsPath = "Tsukat/Custom Scene Manager/Settings";
        
        private const string SceneManagerWindowName = "Custom Scene Manager";
        private const string SettingsWindowName = "Settings";
        private const string PrepareBuildWindowName = "Prepare build";

        private const int WindowSizeMinX = 300;
        private const int WindowSizeMinY = 200;
        private const int WindowSizeMaxX = 200;
        private const int WindowSizeMaxY = 720;

        private const int SettingsWindowSizeYMin = 200;
        private const int SettingsWindowSizeYMax = 600;

        [MenuItem(SceneManagerPath, priority = -1000)]
        public static void OpenWindow()
        {
            EditorWindow wnd = GetWindow<SceneManagerWindow>();
            wnd.titleContent = new GUIContent(SceneManagerWindowName);
            wnd.minSize = new Vector2(WindowSizeMinX, WindowSizeMinY);
            wnd.maxSize = new Vector2(WindowSizeMaxX, WindowSizeMaxY);
            wnd.position = new Rect(Screen.currentResolution.width / 2f, Screen.currentResolution.height / 2f, WindowSizeMinX, WindowSizeMinY);
        }
        
        [MenuItem(PrepareBuildPath, priority = -910)]
        public static void OpenPrepareBuildWindow()
        {
            EditorWindow wnd = GetWindow<PrepareBuildWindow>();
            wnd.titleContent = new GUIContent(PrepareBuildWindowName);
            wnd.minSize = new Vector2(WindowSizeMinX, SettingsWindowSizeYMin);
            wnd.maxSize = new Vector2(WindowSizeMaxX, SettingsWindowSizeYMax);
            wnd.position = new Rect(Screen.currentResolution.width / 2f, Screen.currentResolution.height / 2f, WindowSizeMinX, SettingsWindowSizeYMax);
            wnd.Show(immediateDisplay: true);
        }
        
        [MenuItem(SceneManagerSettingsPath, priority = -909)]
        public static void OpenSettingsWindow()
        {
            EditorWindow wnd = GetWindow<SettingsCustomSceneMangerWindow>();
            wnd.titleContent = new GUIContent(SettingsWindowName);
            wnd.minSize = new Vector2(WindowSizeMinX, SettingsWindowSizeYMin);
            wnd.maxSize = new Vector2(WindowSizeMaxX, SettingsWindowSizeYMax);
            wnd.position = new Rect(Screen.currentResolution.width / 2f, Screen.currentResolution.height / 2f, WindowSizeMinX, SettingsWindowSizeYMax);
            wnd.Show(immediateDisplay: true);
        }
    }
}
