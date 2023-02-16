#region Info

// Tsukat tool - by Horlov Andrii (andreygorlovv@gmail.com)
// Tsukat -> https://tsukat.com/

#endregion

using System;
using UnityEngine.SceneManagement;

namespace TsukatTool.Editor.CustomSceneManager
{
    [Serializable]
    public class ScenesSettings
    {
        public SceneData[] ScenesData;
    }
    
    [Serializable]
    public class SceneData
    {
        /// <summary>
        /// Scene. Or hash-code of scene.
        /// </summary>
        public Scene Scene;
        /// <summary>
        /// Scene name
        /// </summary>
        public string Name;
        /// <summary>
        /// Path to scene
        /// </summary>
        public string Path;
        /// <summary>
        /// Is build added to build settings?
        /// </summary>
        public bool IsBuildAdded;
        /// <summary>
        /// Is build added to Custom loader?
        /// </summary>
        public bool IsCustomSceneLoader;
        /// <summary>
        /// Selected target platforms
        /// </summary>
        public TargetPlatformSettings TargetPlatformSettings;
        /// <summary>
        /// Is build target selected in Multi Tool?
        /// (using only in Multi Tool Window)
        /// </summary>
        public bool IsBuildTargetSelected;
    }
}