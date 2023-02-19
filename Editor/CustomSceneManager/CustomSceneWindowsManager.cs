#region Info

// Tsukat tool - by Horlov Andrii (andreygorlovv@gmail.com)
// Tsukat -> https://tsukat.com/

#endregion

using System;
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
        private const int WindowSizeMinY = 400;
        private const int WindowSizeMaxX = 600;
        private const int WindowSizeMaxY = 720;

        private const int SettingsWindowSizeYMin = 150;
        private const int SettingsWindowSizeYMax = 600;

        private const int PrepareBuildWindowSizeYMin = 300;
        private const int PrepareBuildWindowSizeYMax = 800;

        private const int PrepareBuildWindowSizeMinX = 600;
        private const int PrepareBuildWindowSizeMaxX = 800;

        [MenuItem(SceneManagerPath, priority = -1000)]
        public static void OpenWindow()
        {
            if (HasOpenInstances<SceneManagerWindow>())
            {
                FocusWindowIfItsOpen<SceneManagerWindow>();
                return;
            }
            
            EditorWindow wnd = GetWindow<SceneManagerWindow>();
            wnd.titleContent = new GUIContent(SceneManagerWindowName);
            wnd.minSize = new Vector2(WindowSizeMinX, WindowSizeMinY);
            wnd.maxSize = new Vector2(WindowSizeMaxX, WindowSizeMaxY);
            wnd.position = new Rect(Screen.currentResolution.width / 2f, Screen.currentResolution.height / 2f, WindowSizeMinX, WindowSizeMinY);
        }

        [MenuItem(PrepareBuildPath, priority = -910)]
        public static void OpenPrepareBuildWindow()
        {
            if (HasOpenInstances<PrepareBuildWindow>())
            {
                FocusWindowIfItsOpen<PrepareBuildWindow>();
                return;
            }
            
            EditorWindow wnd = GetWindow<PrepareBuildWindow>();
            wnd.titleContent = new GUIContent(PrepareBuildWindowName);
            wnd.minSize = new Vector2(PrepareBuildWindowSizeMinX, PrepareBuildWindowSizeYMin);
            wnd.maxSize = new Vector2(PrepareBuildWindowSizeMaxX, PrepareBuildWindowSizeYMax);
            wnd.position = new Rect(Screen.currentResolution.width / 2f, Screen.currentResolution.height / 2f, PrepareBuildWindowSizeMinX, WindowSizeMinY);
            wnd.Show(immediateDisplay: true);
        }

        [MenuItem(SceneManagerSettingsPath, priority = -909)]
        public static void OpenSettingsWindow()
        {
            if (HasOpenInstances<SettingsCustomSceneMangerWindow>())
            {
                FocusWindowIfItsOpen<SettingsCustomSceneMangerWindow>();
                return;
            }
            
            EditorWindow wnd = GetWindow<SettingsCustomSceneMangerWindow>();
            wnd.titleContent = new GUIContent(SettingsWindowName);
            wnd.minSize = new Vector2(WindowSizeMinX, SettingsWindowSizeYMin);
            wnd.maxSize = new Vector2(WindowSizeMaxX, SettingsWindowSizeYMax);
            wnd.position = new Rect(Screen.currentResolution.width / 2f, Screen.currentResolution.height / 2f, WindowSizeMinX, SettingsWindowSizeYMin);
            wnd.Show(immediateDisplay: true);
        }
    }
}